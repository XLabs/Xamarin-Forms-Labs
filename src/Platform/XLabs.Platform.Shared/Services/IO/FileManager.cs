// ***********************************************************************
// Assembly         : XLabs.Platform.WP81
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="FileManager.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 
#if !NETFX_CORE
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace XLabs.Platform.Services.IO
{
    /// <summary>
    /// Class FileManager.
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <summary>
        /// The isolated storage file
        /// </summary>
        private readonly IsolatedStorageFile isolatedStorageFile;

        /// <summary>
        /// Initialized new instance of <see cref="FileManager" /> using user store for application.
        /// </summary>
        public FileManager() : this(IsolatedStorageFile.GetUserStoreForApplication())
        {
            
        }

        /// <summary>
        /// Initialized new instance of <see cref="FileManager" />.
        /// </summary>
        /// <param name="isolatedStorageFile">Isolated storage file to use.</param>
        public FileManager(IsolatedStorageFile isolatedStorageFile)
        {
            this.isolatedStorageFile = isolatedStorageFile;
        }

        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DirectoryExists(string path)
        {
            return this.isolatedStorageFile.DirectoryExists(path);
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CreateDirectory(string path)
        {
            this.isolatedStorageFile.CreateDirectory(path);
        }

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <returns>Stream.</returns>
        public Stream OpenFile(string path, FileMode mode, FileAccess access)
        {
            return this.isolatedStorageFile.OpenFile(path, (System.IO.FileMode)mode, (System.IO.FileAccess)access);
        }

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        /// <returns>Stream.</returns>
        public Stream OpenFile(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return this.isolatedStorageFile.OpenFile(path, (System.IO.FileMode)mode, (System.IO.FileAccess)access, (System.IO.FileShare)share);
        }

        /// <summary>
        /// Files the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool FileExists(string path)
        {
            return this.isolatedStorageFile.FileExists(path);
        }

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTimeOffset.</returns>
        public DateTimeOffset GetLastWriteTime(string path)
        {
            return this.isolatedStorageFile.GetLastWriteTime(path);
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteFile(string path)
        {
            this.isolatedStorageFile.DeleteFile(path);
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteDirectory(string path)
        {
            this.isolatedStorageFile.DeleteDirectory(path);
        }

#if !WINDOWS_PHONE
        /// <summary>
        /// Copies a directory to another.
        /// </summary>
        /// <param name="source">Source directory.</param>
        /// <param name="destination">Destination directory. Created when necessary.</param>
        /// <exception cref="System.ArgumentException">Source directory does not exist</exception>
        /// <exception cref="ArgumentException">Exception is thrown if source directory doesn't exist.</exception>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            if (!source.Exists)
            {
                throw new ArgumentException("Source directory does not exist");
            }

            if (!destination.Exists)
            {
                destination.Create();
            }

            foreach (var dir in source.GetDirectories())
            {
                CopyDirectory(dir, new DirectoryInfo(Path.Combine(destination.FullName, dir.Name)));
            }

            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
            }
        }
#endif
    }
}
#endif