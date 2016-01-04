using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// This class is a shortcut to access the most used exif tag, it also take care of formating the Raw exif data like transforming latitude/longitude from int[3] to double
    /// if a tag is missing use MediaFile TryGetRawTagValue to get your particular exif data.
    /// </summary>
    /// <remarks>
    /// For a reference of ExifTags see : http://www.exiv2.org/tags.html
    /// </remarks>
    public class JpegInfo : IJpegInfo
    {
        private IExifReader _reader;


        #region exif properties


      

        /// <summary>
        /// Information specific to compressed data. When a compressed file is recorded, the valid height of the meaningful image must be recorded in this tag, whether or not there is padding data or a restart marker. This tag should not exist in an uncompressed file. Since data padding is unnecessary in the vertical direction, 
        /// the number of lines recorded in this valid image height tag will in fact be the same as that recorded in the SOF.
        /// </summary>
        public UInt32? PixelYDimension
        {
            get {
                UInt32 result = 0;
                return _reader.TryGetTagValue<UInt32>(ExifTags.PixelYDimension, out result) ? result : new Nullable<UInt32>();
            }
        }

        /// <summary>
        /// Information specific to compressed data. When a compressed file is recorded, the valid width of the meaningful image must be recorded in this tag, whether or not there is padding data or a restart marker. This tag should not exist in an uncompressed file.
        /// </summary>
        public UInt32? PixelXDimension
        {
            get
            {
                UInt32 result = 0;
                return _reader.TryGetTagValue<UInt32>(ExifTags.PixelXDimension, out result) ? result : new Nullable<UInt32>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ColorModel
        {
            get;
            set;
        }
        /// <summary>
        /// The color space information tag is always recorded as the color space specifier. Normally sRGB is used to define the color space based on the PC monitor conditions and environment. If a color space other than sRGB is used, Uncalibrated is set. Image data recorded as Uncalibrated can be treated as sRGB when it is converted to FlashPix.
        /// </summary>
        public ExifTagColorSpace? ColorSpace
        {
            get
            {
                ushort result = 0;
                return _reader.TryGetTagValue<ushort>(ExifTags.ColorSpace, out result) ?  new Nullable<ExifTagColorSpace>((ExifTagColorSpace)(result)):new Nullable<ExifTagColorSpace>() ;
            }
        }
        /// <summary>
        /// The date and time when the image was stored as digital data.
        /// </summary>
        public string DateTimeDigitized
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.DateTimeDigitized, out result) ?  result:null;
            }
        }

        /// <summary>
        /// Orientation of the image.
        /// </summary>
        public ExifTagOrientation? Orientation
        {
            get
            {
                ushort result = 0;
                return _reader.TryGetTagValue<ushort>(ExifTags.Orientation, out result) ? new Nullable<ExifTagOrientation>((ExifTagOrientation)(result)) : new Nullable<ExifTagOrientation>();
            }
        }

        /// <summary>
        /// The number of pixels per ResolutionUnit in the ImageWidth direction. When the image resolution is unknown, 72 [dpi] is designated.
        /// </summary>
        public double? XResolution
        {
            get
            {
                double result = 0;
                return _reader.TryGetTagValue<double>(ExifTags.XResolution, out result) ? new Nullable<double>((result)) : new Nullable<double>();
            }
        }

        /// <summary>
        /// The number of pixels per ResolutionUnit in the ImageLength direction. The same value as XResolution is designated.
        /// </summary>
        public double? YResolution
        {
            get
            {
                double result = 0;
                return _reader.TryGetTagValue<double>(ExifTags.YResolution, out result) ? new Nullable<double>((result)) : new Nullable<double>();
            }
        }

        /// <summary>
        /// Resolution unit of the image.
        /// </summary>
        public ExifTagResolutionUnit? ResolutionUnit
        {
            get
            {
                ushort result = 0;
                return _reader.TryGetTagValue<ushort>(ExifTags.ResolutionUnit, out result) ? new Nullable<ExifTagResolutionUnit>((ExifTagResolutionUnit)(result)) : new Nullable<ExifTagResolutionUnit>();
            }
        }

        /// <summary>
        /// Date at which the image was taken.
        /// </summary>
        public string DateTime
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.DateTime, out result) ? result : null;
            }
        }
       

        /// <summary>
        /// Date at which the image was taken. Created by Lumia devices.
        /// </summary>
        public string DateTimeOriginal
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.DateTimeOriginal, out result) ? result : null;
            }
        }

        /// <summary>
        /// Description of the image.
        /// </summary>
        public string ImageDescription
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.ImageDescription, out result) ? result : null;
            }
        }

        /// <summary>
        /// Camera manufacturer.
        /// </summary>
        public string Make
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.Make, out result) ? result : null;
            }
        }

        /// <summary>
        /// Camera model.
        /// </summary>
        public string Model
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.Model, out result) ? result : null;
            }
        }

        /// <summary>
        /// Software used to create the image.
        /// </summary>
        public string Software
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.Software, out result) ? result : null;
            }
        }

        /// <summary>
        /// Image artist.
        /// </summary>
        public string Artist
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.Artist, out result) ? result : null;
            }
        }

        /// <summary>
        /// Image copyright.
        /// </summary>
        public string Copyright
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.Copyright, out result) ? result : null;
            }
        }

        /// <summary>
        /// Image user comments.
        /// </summary>
        public string UserComment
        {
            get
            {
                string result = null; ;
                return _reader.TryGetTagValue<string>(ExifTags.UserComment, out result) ? result : null;
            }
        }

        /// <summary>
        /// Exposure time, in seconds.
        /// </summary>
        public double? ExposureTime
        {
            get
            {
                double result = 0;
                return _reader.TryGetTagValue<double>(ExifTags.ExposureTime, out result) ? new Nullable<double>((result)) : new Nullable<double>();
            }
        }

        /// <summary>
        /// F-number (F-stop) of the camera lens when the image was taken.
        /// </summary>
        public double? FNumber
        {
            get
            {
                double result = 0;
                return _reader.TryGetTagValue<double>(ExifTags.FNumber, out result) ? new Nullable<double>((result)) : new Nullable<double>();
            }
        }

        /// <summary>
        /// Flash settings of the camera when the image was taken.
        /// </summary>
        public ExifTagFlash? Flash
        {
            get
            {
                ushort result = 0;
                return _reader.TryGetTagValue<ushort>(ExifTags.Flash, out result) ? new Nullable<ExifTagFlash>((ExifTagFlash)(result)) : new Nullable<ExifTagFlash>();
            }
        }

       

        #region GPS Data



        /// <summary>
        /// GPS latitude reference (North, South).
        /// </summary>
        public ExifTagGpsLatitudeRef? GpsLatitudeRef
        {
            get
            {
                string result ;
                return _reader.TryGetTagValue<string>(ExifTags.GPSLatitudeRef, out result) ? new Nullable<ExifTagGpsLatitudeRef>((ExifTagGpsLatitudeRef)((char)result[0])) : new Nullable<ExifTagGpsLatitudeRef>();
            }
        }

        /// <summary>
        /// GPS latitude in decimal form.
        /// </summary>
        public double? GpsLatitude
        {
            get
            {
                
                 //On Droid the Exif reader return an array of double
                double[] result = new double[3];
                return _reader.TryGetTagValue<double[]>(ExifTags.GPSLatitude, out result)?new Nullable<double>(ExifLatCoordinateToDouble(result[0], result[1], result[2], this.GpsLatitudeRef.Value)):null;
                
            }
        }

        /// <summary>
        /// GPS longitude reference (East, West).
        /// </summary>
        public ExifTagGpsLongitudeRef? GpsLongitudeRef
        {
            get
            {
                string result;
                return _reader.TryGetTagValue<string>(ExifTags.GPSLongitudeRef, out result) ? new Nullable<ExifTagGpsLongitudeRef>((ExifTagGpsLongitudeRef)((char)result[0])) : new Nullable<ExifTagGpsLongitudeRef>();
            }
        }

        /// <summary>
        /// GPS longitudein decimal form.
        /// </summary>
        public double? GpsLongitude
        {
            get
            {
                double[] result = new double[3];
                return _reader.TryGetTagValue<double[]>(ExifTags.GPSLongitude, out result) ? new Nullable<double>(ExifLngCoordinateToDouble(result[0], result[1], result[2], this.GpsLongitudeRef.Value)) : null;
            }
        }
        /// <summary>
        /// Gps altitude in meters
        /// </summary>
        public double? GpsAltitude
        {
           get
            {
                double result;
                return _reader.TryGetTagValue<double>(ExifTags.GPSAltitude, out result) ? new Nullable<double>(result) : new Nullable<double>();
            }
        }
        /// <summary>
        /// Indicates the altitude used as the reference altitude. If the reference is sea level and the altitude is above sea level, 0 is given. If the altitude is below sea level, a value of 1 is given and the altitude is indicated as an absolute value in the GSPAltitude tag. The reference unit is meters.
        /// </summary>
        public ExifTagGpsAltitudeRef? GpsAltitudeRef
        {
            get
            {
                byte result;
                return _reader.TryGetTagValue<byte>(ExifTags.GPSAltitudeRef, out result) ? new Nullable<ExifTagGpsAltitudeRef>((ExifTagGpsAltitudeRef)(Convert.ToUInt16(result))) : new Nullable<ExifTagGpsAltitudeRef>();
            }
        }
        /// <summary>
        /// Gps bearing when the piture was taken
        /// </summary>
        public double? GpsDestBearing
        {
            get
            {
                double result;
                return _reader.TryGetTagValue<double>(ExifTags.GPSDestBearing, out result) ? new Nullable<double>(result) : new Nullable<double>();
            }
        }
        /// <summary>
        /// Indicates the reference used for giving the bearing to the destination point. "T" denotes true direction and "M" is magnetic direction.
        /// </summary>
        public ExifTagGpsBearingRef? GpsDestBearingRef
        {
            get
            {
                string result;
                return _reader.TryGetTagValue<string>(ExifTags.GPSDestBearingRef, out result) ? new Nullable<ExifTagGpsBearingRef>((ExifTagGpsBearingRef)((char)result[0])) : new Nullable<ExifTagGpsBearingRef>();
            }
        }
        /// <summary>
        /// Indicates the speed of GPS receiver movement, unit is given by the GPSSpeedRef property
        /// </summary>
        public double? GPSSpeed
        {
            get
            {
                double result;
                return _reader.TryGetTagValue<double>(ExifTags.GPSSpeed, out result) ? new Nullable<double>(result) : new Nullable<double>();
            }
        }
        /// <summary>
        /// Indicates the unit used to express the GPS receiver speed of movement. "K" "M" and "N" represents kilometers per hour, miles per hour, and knots.
        /// </summary>
        public ExifTagGpsSpeedRef? GPSSpeedRef
        {
            get
            {
                string result;
                return _reader.TryGetTagValue<string>(ExifTags.GPSSpeedRef, out result) ? new Nullable<ExifTagGpsSpeedRef>((ExifTagGpsSpeedRef)((char)result[0])) : new Nullable<ExifTagGpsSpeedRef>();
            }
        }

        #endregion
        #endregion

        public JpegInfo(string  filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");
            _reader = new ExifBinaryReader(filePath);
        }

        /// <summary>
        /// Retreive one exif tag value according to the tag name, this value is not formated, so you have to know the return type on each platform.
        /// For instance latitude is an array of 3 double on Android and  a single double on iOS
        /// </summary>
        /// <typeparam name="T">Type of the result value</typeparam>
        /// <param name="tag">Tag name</param>
        /// <param name="result">Result</param>
        /// <returns>Did succed</returns>
        public bool TryGetRawTagValue<T>(ExifTags tag, out T result)
        {
            try
            {
                return _reader.TryGetTagValue(tag, out result);
            }
            catch (Exception ex)
            {
                result = default(T);
                return false;
            }
        }

        private  double ExifLatCoordinateToDouble(double deg, double min, double sec, ExifTagGpsLatitudeRef coordRef)
        {
            if (coordRef == ExifTagGpsLatitudeRef.North)
                return deg + (min / 60) + (sec / 3600);
            else
                return (deg + (min / 60) + (sec / 3600)) * -1;

        }

        private  double ExifLngCoordinateToDouble(double deg, double min, double sec, ExifTagGpsLongitudeRef coordRef)
        {
            if (coordRef == ExifTagGpsLongitudeRef.East)
                return deg + (min / 60) + (sec / 3600);
            else
                return (deg + (min / 60) + (sec / 3600)) * -1;
        }

        public void Dispose()
        {
            if (_reader != null)
                _reader.Dispose();
        }
    }



}
