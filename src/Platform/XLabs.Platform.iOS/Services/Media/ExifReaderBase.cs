using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using XLabs.Platform.Extensions;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// Base class for iOS Exif Readers, parse meta data in native iOS dictionaries and store data in a single dictionary in order to easy latter search.
    /// Note that all Exif tags are not available on this iOS implementation since each tag must be retreived from
    /// one of the below dictionnary, if you need more tags make a request a github or add your tags in a PR :)
    /// To add a tag you must match the iOS propery name : https://developer.apple.com/library/ios/documentation/GraphicsImaging/Reference/CGImageProperties_Reference/#//apple_ref/doc/constant_group/EXIF_Dictionary_Keys
    /// with the local ExifTags enum an find the right return type using : http://www.exiv2.org/tags.html
    /// </summary>
    public abstract class ExifReaderBase
    {
        protected Dictionary<ExifTags, object> _exifCache;


        

        public ExifReaderBase()
        {
            _exifCache = new Dictionary<ExifTags, object>();
        }
        /// <summary>
        /// Gps Data
        /// </summary>
        /// <param name="gpsdico"></param>
        protected void SetGpsData(NSDictionary gpsdico)
        {
            _exifCache.Add(ExifTags.GPSAltitude, (gpsdico[ImageIO.CGImageProperties.GPSAltitude] != null) ? (double?)gpsdico[ImageIO.CGImageProperties.GPSAltitude].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.GPSAltitudeRef, (gpsdico[ImageIO.CGImageProperties.GPSAltitudeRef] != null) ? (ushort?)gpsdico[ImageIO.CGImageProperties.GPSAltitudeRef].ToObject(typeof(ushort?)) : null);
            _exifCache.Add(ExifTags.GPSLatitude,(gpsdico[ImageIO.CGImageProperties.GPSLatitude] != null) ? (double?)gpsdico[ImageIO.CGImageProperties.GPSLatitude].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.GPSLongitude,(gpsdico[ImageIO.CGImageProperties.GPSLongitude] != null) ? (double?)gpsdico[ImageIO.CGImageProperties.GPSLongitude].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.GPSLongitudeRef,(gpsdico[ImageIO.CGImageProperties.GPSLongitudeRef] != null) ? ((string)gpsdico[ImageIO.CGImageProperties.GPSLongitudeRef].ToObject(typeof(string))) : null);
            _exifCache.Add(ExifTags.GPSLatitudeRef, (gpsdico[ImageIO.CGImageProperties.GPSLatitudeRef] != null) ? ((string)gpsdico[ImageIO.CGImageProperties.GPSLatitudeRef].ToObject(typeof(string))) : null);
            _exifCache.Add(ExifTags.GPSDestBearing,(gpsdico[ImageIO.CGImageProperties.GPSDestBearing] != null) ? (double?)gpsdico[ImageIO.CGImageProperties.GPSDestBearing].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.GPSDestBearingRef, (gpsdico[ImageIO.CGImageProperties.GPSDestBearingRef] != null) ? ((string)gpsdico[ImageIO.CGImageProperties.GPSDestBearingRef].ToObject(typeof(string))) : null);
            _exifCache.Add(ExifTags.GPSSpeed,(gpsdico[ImageIO.CGImageProperties.GPSSpeed] != null) ? (double?)gpsdico[ImageIO.CGImageProperties.GPSSpeed].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.GPSSpeedRef, (gpsdico[ImageIO.CGImageProperties.GPSSpeedRef] != null) ? ((string)gpsdico[ImageIO.CGImageProperties.GPSSpeedRef].ToObject(typeof(string))) : null);
           
        }
        /// <summary>
        /// Tiff data
        /// </summary>
        /// <param name="tiffdico"></param>
        protected void SetTiffData(NSDictionary tiffdico)
        {
            _exifCache.Add(ExifTags.YResolution,(tiffdico[ImageIO.CGImageProperties.TIFFYResolution] != null) ? (double?)tiffdico[ImageIO.CGImageProperties.TIFFYResolution].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.XResolution,(tiffdico[ImageIO.CGImageProperties.TIFFXResolution] != null) ? (double?)tiffdico[ImageIO.CGImageProperties.TIFFXResolution].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.Model,(tiffdico[ImageIO.CGImageProperties.TIFFModel] != null) ? (string)tiffdico[ImageIO.CGImageProperties.TIFFModel].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.Make,(tiffdico[ImageIO.CGImageProperties.TIFFMake] != null) ? (string)tiffdico[ImageIO.CGImageProperties.TIFFMake].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.ResolutionUnit,(tiffdico[ImageIO.CGImageProperties.TIFFResolutionUnit] != null) ? (ExifTagResolutionUnit?)((int)tiffdico[ImageIO.CGImageProperties.TIFFResolutionUnit].ToObject(typeof(int))) : null);
            _exifCache.Add(ExifTags.Software,(string)tiffdico[ImageIO.CGImageProperties.TIFFSoftware].ToObject(typeof(string)));
            _exifCache.Add(ExifTags.DateTime,(string)tiffdico[ImageIO.CGImageProperties.TIFFDateTime].ToObject(typeof(string)));
            _exifCache.Add(ExifTags.ImageDescription, (string)tiffdico[ImageIO.CGImageProperties.TIFFImageDescription].ToObject(typeof(string)));
            _exifCache.Add(ExifTags.Artist, (string)tiffdico[ImageIO.CGImageProperties.TIFFArtist].ToObject(typeof(string)));
            _exifCache.Add(ExifTags.Copyright, (string)tiffdico[ImageIO.CGImageProperties.PNGCopyright].ToObject(typeof(string)));
            _exifCache.Add(ExifTags.Compression, (tiffdico[ImageIO.CGImageProperties.TIFFCompression] != null) ? (ExifTagCompression?)(int)tiffdico[ImageIO.CGImageProperties.TIFFCompression].ToObject(typeof(int)) : null);

           
        }
        /// <summary>
        /// Global data
        /// </summary>
        /// <param name="metadata"></param>
        protected void SetGlobalData(NSDictionary metadata)
        {
             _exifCache.Add(ExifTags.Orientation,(metadata[ImageIO.CGImageProperties.Orientation] != null) ? (ExifTagOrientation?)metadata[ImageIO.CGImageProperties.Orientation].ToObject(typeof(ushort)) : null);
        }
     
        protected void SetExifData(NSDictionary exifdico)
        {
            _exifCache.Add(ExifTags.Flash,(exifdico[ImageIO.CGImageProperties.ExifFlash] != null) ? (ExifTagFlash?)(int)exifdico[ImageIO.CGImageProperties.ExifFlash].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.ExposureTime,(exifdico[ImageIO.CGImageProperties.ExifExposureTime] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifExposureTime].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.FNumber,(exifdico[ImageIO.CGImageProperties.ExifFNumber] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifFNumber].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.ExposureProgram, (exifdico[ImageIO.CGImageProperties.ExifExposureProgram] != null) ? (ExifTagExposureProgram?)(int)exifdico[ImageIO.CGImageProperties.ExifExposureProgram].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.SpectralSensitivity, (exifdico[ImageIO.CGImageProperties.ExifSpectralSensitivity] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifSpectralSensitivity].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.ExifVersion, (exifdico[ImageIO.CGImageProperties.ExifVersion] != null) ? ((NSArray)exifdico[ImageIO.CGImageProperties.ExifVersion]).ToEnumerable<int, NSNumber>() : null);
            _exifCache.Add(ExifTags.ISOSpeedRatings, (exifdico[ImageIO.CGImageProperties.ExifISOSpeedRatings] != null) ? ((NSArray)exifdico[ImageIO.CGImageProperties.ExifISOSpeedRatings]).ToEnumerable<int,NSNumber>() : null);
            _exifCache.Add(ExifTags.DateTimeOriginal,(exifdico[ImageIO.CGImageProperties.ExifDateTimeOriginal] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifDateTimeOriginal].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.ColorSpace,(exifdico[ImageIO.CGImageProperties.ExifColorSpace] != null) ? (ExifTagColorSpace?)exifdico[ImageIO.CGImageProperties.ExifColorSpace].ToObject(typeof(ushort)) : null);
            _exifCache.Add(ExifTags.DateTimeDigitized,(exifdico[ImageIO.CGImageProperties.ExifDateTimeDigitized] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifDateTimeDigitized].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.OECF, (exifdico[ImageIO.CGImageProperties.ExifOECF] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifOECF].ToObject(typeof(string)) : null);

            _exifCache.Add(ExifTags.ComponentsConfiguration, (exifdico[ImageIO.CGImageProperties.ExifComponentsConfiguration] != null) ? ((NSArray)exifdico[ImageIO.CGImageProperties.ExifComponentsConfiguration]).ToEnumerable<int, NSNumber>() : null);
            _exifCache.Add(ExifTags.ShutterSpeedValue, (exifdico[ImageIO.CGImageProperties.ExifShutterSpeedValue] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifShutterSpeedValue].ToObject(typeof(double?)) : null);

            _exifCache.Add(ExifTags.ApertureValue, (exifdico[ImageIO.CGImageProperties.ExifApertureValue] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifApertureValue].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.BrightnessValue, (exifdico[ImageIO.CGImageProperties.ExifBrightnessValue] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifBrightnessValue].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.UserComment, (exifdico[ImageIO.CGImageProperties.ExifUserComment] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifUserComment].ToObject(typeof(string)) : null);

            _exifCache.Add(ExifTags.CompressedBitsPerPixel, (exifdico[ImageIO.CGImageProperties.ExifCompressedBitsPerPixel] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifCompressedBitsPerPixel].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.ExposureBiasValue, (exifdico[ImageIO.CGImageProperties.ExifExposureBiasValue] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifExposureBiasValue].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.MaxApertureValue, (exifdico[ImageIO.CGImageProperties.ExifMaxApertureValue] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifMaxApertureValue].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.SubjectDistance, (exifdico[ImageIO.CGImageProperties.ExifSubjectDistance] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifSubjectDistance].ToObject(typeof(double?)) : null);

            _exifCache.Add(ExifTags.MeteringMode, (exifdico[ImageIO.CGImageProperties.ExifMeteringMode] != null) ? (ExifTagMeteringMode?)(int)exifdico[ImageIO.CGImageProperties.ExifMeteringMode].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.LightSource, (exifdico[ImageIO.CGImageProperties.ExifLightSource] != null) ? (ExifTagLightSource?)(int)exifdico[ImageIO.CGImageProperties.ExifLightSource].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.FocalLength, (exifdico[ImageIO.CGImageProperties.ExifFocalLength] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifFocalLength].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.FlashEnergy, (exifdico[ImageIO.CGImageProperties.ExifFlashEnergy] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifFlashEnergy].ToObject(typeof(double?)) : null);

            _exifCache.Add(ExifTags.FocalPlaneXResolution, (exifdico[ImageIO.CGImageProperties.ExifFocalPlaneXResolution] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifFocalPlaneXResolution].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.FocalPlaneYResolution, (exifdico[ImageIO.CGImageProperties.ExifFocalPlaneYResolution] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifFocalPlaneYResolution].ToObject(typeof(double?)) : null);
            //Should be an enum for that
            _exifCache.Add(ExifTags.FocalPlaneResolutionUnit, (exifdico[ImageIO.CGImageProperties.ExifFocalPlaneResolutionUnit] != null) ? (int?)exifdico[ImageIO.CGImageProperties.ExifFocalPlaneResolutionUnit].ToObject(typeof(int?)) : null);

            _exifCache.Add(ExifTags.SubjectLocation, (exifdico[ImageIO.CGImageProperties.ExifSubjectLocation] != null) ? (int?)exifdico[ImageIO.CGImageProperties.ExifSubjectLocation].ToObject(typeof(int?)) : null);
            _exifCache.Add(ExifTags.ExposureIndex, (exifdico[ImageIO.CGImageProperties.ExifExposureIndex] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifExposureIndex].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.SensingMethod, (exifdico[ImageIO.CGImageProperties.ExifSensingMethod] != null) ? (ExifTagSensingMethod?)(int)exifdico[ImageIO.CGImageProperties.ExifSensingMethod].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.FlashpixVersion, (exifdico[ImageIO.CGImageProperties.ExifFlashPixVersion] != null) ? ((NSArray)exifdico[ImageIO.CGImageProperties.ExifFlashPixVersion]).ToEnumerable<int, NSNumber>() : null);
            _exifCache.Add(ExifTags.PixelXDimension, (exifdico[ImageIO.CGImageProperties.ExifPixelXDimension] != null) ? (int?)exifdico[ImageIO.CGImageProperties.ExifPixelXDimension].ToObject(typeof(int?)) : null);
            _exifCache.Add(ExifTags.PixelYDimension, (exifdico[ImageIO.CGImageProperties.ExifPixelYDimension] != null) ? (int?)exifdico[ImageIO.CGImageProperties.ExifPixelYDimension].ToObject(typeof(int?)) : null);
            _exifCache.Add(ExifTags.RelatedSoundFile, (exifdico[ImageIO.CGImageProperties.ExifRelatedSoundFile] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifRelatedSoundFile].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.FileSource, (exifdico[ImageIO.CGImageProperties.ExifFileSource] != null) ? (ExifTagFileSource?)(int)exifdico[ImageIO.CGImageProperties.ExifFileSource].ToObject(typeof(int)) : null);
            //Array ?
            _exifCache.Add(ExifTags.CFAPattern, (exifdico[ImageIO.CGImageProperties.ExifCFAPattern] != null) ? (int?)exifdico[ImageIO.CGImageProperties.ExifCFAPattern].ToObject(typeof(int?)) : null);
            _exifCache.Add(ExifTags.CustomRendered, (exifdico[ImageIO.CGImageProperties.ExifCustomRendered] != null) ? (ExifTagCustomRendered?)(int)exifdico[ImageIO.CGImageProperties.ExifCustomRendered].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.ExposureMode, (exifdico[ImageIO.CGImageProperties.ExifExposureMode] != null) ? (ExifTagExposureMode?)(int)exifdico[ImageIO.CGImageProperties.ExifExposureMode].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.WhiteBalance, (exifdico[ImageIO.CGImageProperties.ExifWhiteBalance] != null) ? (ExifTagWhiteBalance?)(int)exifdico[ImageIO.CGImageProperties.ExifWhiteBalance].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.DigitalZoomRatio, (exifdico[ImageIO.CGImageProperties.ExifDigitalZoomRatio] != null) ? (double?)exifdico[ImageIO.CGImageProperties.ExifDigitalZoomRatio].ToObject(typeof(double?)) : null);
            _exifCache.Add(ExifTags.FocalLengthIn35mmFilm, (exifdico[ImageIO.CGImageProperties.ExifFocalLenIn35mmFilm] != null) ? (int?)exifdico[ImageIO.CGImageProperties.ExifFocalLenIn35mmFilm].ToObject(typeof(int?)) : null);
            _exifCache.Add(ExifTags.SceneCaptureType, (exifdico[ImageIO.CGImageProperties.ExifSceneCaptureType] != null) ? (ExifTagSceneCaptureType?)(int)exifdico[ImageIO.CGImageProperties.ExifSceneCaptureType].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.GainControl, (exifdico[ImageIO.CGImageProperties.ExifGainControl] != null) ? (ExifTagGainControl?)(int)exifdico[ImageIO.CGImageProperties.ExifGainControl].ToObject(typeof(int)) : null);

            _exifCache.Add(ExifTags.SceneType, (exifdico[ImageIO.CGImageProperties.ExifSceneType] != null) ? (ExifTagSceneType?)(int)exifdico[ImageIO.CGImageProperties.ExifSceneType].ToObject(typeof(int)) : null);


            _exifCache.Add(ExifTags.Contrast, (exifdico[ImageIO.CGImageProperties.ExifContrast] != null) ? (ExifTagContrast?)(int)exifdico[ImageIO.CGImageProperties.ExifContrast].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.Saturation, (exifdico[ImageIO.CGImageProperties.ExifSaturation] != null) ? (ExifTagSaturation?)(int)exifdico[ImageIO.CGImageProperties.ExifSaturation].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.Sharpness, (exifdico[ImageIO.CGImageProperties.ExifSharpness] != null) ? (ExifTagSharpness?)(int)exifdico[ImageIO.CGImageProperties.ExifSharpness].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.DeviceSettingDescription, (exifdico[ImageIO.CGImageProperties.ExifDeviceSettingDescription] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifDeviceSettingDescription].ToObject(typeof(string)) : null);
            _exifCache.Add(ExifTags.SubjectDistanceRange, (exifdico[ImageIO.CGImageProperties.ExifSubjectDistRange] != null) ? (ExifTagSubjectDistanceRange?)(int)exifdico[ImageIO.CGImageProperties.ExifSubjectDistRange].ToObject(typeof(int)) : null);
            _exifCache.Add(ExifTags.ImageUniqueID, (exifdico[ImageIO.CGImageProperties.ExifImageUniqueID] != null) ? (string)exifdico[ImageIO.CGImageProperties.ExifImageUniqueID].ToObject(typeof(string)) : null);

          
        }

        public bool TryGetTagValue<T>(ushort tagID, out T result)
        {
            if (_exifCache.ContainsKey((ExifTags)tagID))
            {
                try
                {
                    result = (T)_exifCache[(ExifTags)tagID];
                    return true;
                }
                catch (Exception)
                {
                    result = default(T);
                    return false;
                }
            }
            else
            {
                //For image that dont have any metadata
                result = default(T);
                return false;
            }

        }

        public bool TryGetTagValue<T>(ExifTags tag, out T result)
        {
            
            return TryGetTagValue((ushort)tag, out result);
        }

    }
}
