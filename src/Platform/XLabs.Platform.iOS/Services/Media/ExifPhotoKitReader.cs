using CoreImage;
using Foundation;
using Photos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using System.Linq;
using System.Diagnostics;
namespace XLabs.Platform.Services.Media
{
    /// <summary>
    /// Retreive Exif tag using PhotoKit on iOS for iOS version >= 8
    /// </summary>
    public sealed class ExifPhotoKitReader :ExifReaderBase, IExifReader
    {
        private NSUrl _assetURL;
       

        

        public ExifPhotoKitReader(NSUrl assetURL):base()
        {
            if (assetURL == null) throw new ArgumentException("assetURL");
            _assetURL = assetURL;
            ReadExifTags().ContinueWith((task) =>
            {
                if (!task.IsCompleted)
                {

                    throw new MediaExifException("Could not read exif data from photokit");
                }
            });
        }

        /// <summary>
        /// Get meta data dictionaries from PhotoKit
        /// </summary>
        /// <returns></returns>
        private Task ReadExifTags()
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            try
            {

                var res = PHAsset.FetchAssets(new NSUrl[] { _assetURL }, new PHFetchOptions());
             
                //Since we selected only one pic there should be only one item in the list
                PHAsset photo = (PHAsset)res.FirstOrDefault();
                if (photo == null)
                    tcs.SetException(new MediaFileNotFoundException(_assetURL.ToString()));
                if (PHPhotoLibrary.AuthorizationStatus != PHAuthorizationStatus.Authorized)
                {
                    PHPhotoLibrary.RequestAuthorization((status) =>
                    {
                        if (status != PHAuthorizationStatus.Authorized)
                        {
                            tcs.SetCanceled();
                            return;
                        }
                    });
                }
                photo.RequestContentEditingInput(new PHContentEditingInputRequestOptions() { NetworkAccessAllowed = false }, (input, options) =>
                {
                    try
                    {
                        //Get the orginial image (FullSize)
                        CIImage img = CIImage.FromUrl(input.FullSizeImageUrl);
                        var prop = img.Properties;
                       
                        SetGlobalData(prop.Dictionary);
                        if (prop.Gps != null)
                            SetGpsData(prop.Gps.Dictionary);
                        if (prop.Exif != null)
                            SetExifData(prop.Exif.Dictionary);
                        if (prop.Tiff != null)
                            SetTiffData(prop.Tiff.Dictionary);
                       
                        tcs.SetResult(0);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exif parsing failed : " + ex.ToString());
                  
                        tcs.SetException(ex);
                        //fail silently
                    }


                });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                tcs.TrySetException(ex);

            }
            return tcs.Task;
        }



       

       

        public void Dispose()
        {
            
        }
    }
}
