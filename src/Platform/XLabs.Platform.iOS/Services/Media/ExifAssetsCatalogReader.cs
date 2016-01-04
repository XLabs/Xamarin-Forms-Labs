using AssetsLibrary;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// Retreive Exif tag using AssetsCatalog on iOS for iOS version lower than 8
    /// </summary>
    public sealed class ExifAssetsCatalogReader : ExifReaderBase, IExifReader
    {
        NSUrl _assetURL;

       

        public ExifAssetsCatalogReader(NSUrl assetURL)
            : base()
        {
            if (assetURL == null) throw new ArgumentException("assetURL");
            _assetURL = assetURL;
           
            ReadExifTags().ContinueWith((task) =>
            {
                if (!task.IsCompleted)
                {

                    throw new MediaExifException("Could not read exif data from AssetCatalog");
                }
            });
        }

        /// <summary>
        /// Get meta data dictionaries from AssetsCatalog
        /// </summary>
        /// <returns></returns>
        private Task ReadExifTags()
        {
            ALAssetsLibrary library = new ALAssetsLibrary();
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
           
            library.AssetForUrl(_assetURL, (asset) =>
            {
                //Set metadata only if there is some
                if (asset != null)
                {
                    try
                    {
                        NSDictionary metadata = asset.DefaultRepresentation.Metadata;
                        
                        SetGlobalData(metadata);
                        NSDictionary gpsdico = ((NSDictionary)metadata[ImageIO.CGImageProperties.GPSDictionary]);
                        if (gpsdico != null)
                             SetGpsData(gpsdico);
                        var exifdico = ((NSDictionary)metadata[ImageIO.CGImageProperties.ExifDictionary]);
                        if (exifdico != null)
                            SetExifData(exifdico);
                        var tiffdico = ((NSDictionary)metadata[ImageIO.CGImageProperties.TIFFDictionary]);
                        if (tiffdico != null)
                            SetTiffData(tiffdico);
                       
                        tcs.SetResult(0);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exif parsing failed : " + ex.ToString());
                        tcs.SetException(ex);
                        //Fail silently
                    }

                }
                else
                {
                    
                    tcs.SetResult(0);
                }



            }, (error) =>
            {
                tcs.SetException(new Exception(error.ToString()));
            });
            return tcs.Task;
        }

       

        public void Dispose()
        {
           
        }
    }
}
