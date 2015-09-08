namespace XLabs.Platform.Services.Media
{
	using System;
	using System.IO;

	/// <summary>
    /// Class MediaExtensions.
    /// </summary>
    public static class MediaExtensions
    {
        /// <summary>
        /// Verifies the options.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <exception cref="System.ArgumentNullException">options</exception>
        /// <exception cref="System.ArgumentException">options.Directory must be a relative folder;options</exception>
        public static void VerifyOptions(this MediaStorageOptions self)
        {
            if (self == null)
            {
                throw new ArgumentNullException("self");
            }
            //if (!Enum.IsDefined (typeof(MediaFileStoreLocation), options.Location))
            //    throw new ArgumentException ("options.Location is not a member of MediaFileStoreLocation");
            //if (options.Location == MediaFileStoreLocation.Local)
            //{
            //if (String.IsNullOrWhiteSpace (options.Directory))
            //	throw new ArgumentNullException ("options", "For local storage, options.Directory must be set");
            if (Path.IsPathRooted(self.Directory))
            {
                throw new ArgumentException("options.Directory must be a relative folder", "self");
            }
            //}
        }

        /// <summary>
        /// Gets the output file name with folder.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="rootPath">The root folder.</param>
        /// <returns>System.String.</returns>
        public static string GetMediaFileWithPath(this MediaStorageOptions self, string rootPath)
        {
            var isPhoto = !(self is VideoMediaStorageOptions);
            var name = (self != null) ? self.Name : null;
            var directory = (self != null) ? self.Directory : null;

            return MediaFileHelpers.GetMediaFileWithPath(isPhoto, rootPath, directory, name);
        }

        /// <summary>
        /// Gets the unique filepath.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="rootPath">The root folder.</param>
        /// <param name="checkExists">The check exists.</param>
        /// <returns>System.String.</returns>
        public static string GetUniqueMediaFileWithPath(this MediaStorageOptions self, string rootPath,
            Func<string, bool> checkExists)
        {
            var isPhoto = !(self is VideoMediaStorageOptions);
            var path = self.GetMediaFileWithPath(rootPath);

            var folder = Path.GetDirectoryName(path);
            var name = Path.GetFileNameWithoutExtension(path);

            return MediaFileHelpers.GetUniqueMediaFileWithPath(isPhoto, folder, name, checkExists);
        }
    }

    /// <summary>
    /// Class MediaFileHelpers.
    /// </summary>
    public static class MediaFileHelpers
    {
        /// <summary>
        /// Gets the output file with folder.
        /// </summary>
        /// <param name="isPhoto">if set to <c>true</c> [is photo].</param>
        /// <param name="folder">The root folder.</param>
        /// <param name="subdir">The subdir.</param>
        /// <param name="name">The name.</param>
        /// <returns>System.String.</returns>
        public static string GetMediaFileWithPath(bool isPhoto, string folder, string subdir, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                if (isPhoto)
                {
                    name = "IMG_" + timestamp + ".jpg";
                }
                else
                {
                    name = "VID_" + timestamp + ".mp4";
                }
            }

            var ext = Path.GetExtension(name);
            if (ext == String.Empty)
            {
                ext = ((isPhoto) ? ".jpg" : ".mp4");
            }

            name = Path.GetFileNameWithoutExtension(name);

            var newFolder = Path.Combine(folder ?? String.Empty, subdir ?? String.Empty);

            return Path.Combine(newFolder, name + ext);
        }

        /// <summary>
        /// Gets the unique file folder.
        /// </summary>
        /// <param name="isPhoto">if set to <c>true</c> [is photo].</param>
        /// <param name="folder">The folder.</param>
        /// <param name="name">The name.</param>
        /// <param name="checkExists">The check exists.</param>
        /// <returns>System.String.</returns>
        public static string GetUniqueMediaFileWithPath(bool isPhoto, string folder, string name, Func<string, bool> checkExists)
        {
            var ext = Path.GetExtension(name);

            if (String.IsNullOrEmpty(ext))
            {
                ext = (isPhoto) ? ".jpg" : "mp4";
            }

            var nname = name + ext;
            var i = 1;
            while (checkExists(Path.Combine(folder, nname)))
            {
                nname = name + "_" + (i++) + ext;
            }

            return Path.Combine(folder, nname);
        }
    }

}