namespace XLabs.Platform.Services.IO
{
    using System;
    using System.IO;

    /// <summary>
    /// Interface IFileManager provides access to files located in Isolated Storage.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Directories the exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <returns>Stream.</returns>
        Stream OpenFile(string path, FileMode mode, FileAccess access);

        /// <summary>
        /// Opens the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <param name="share">The share.</param>
        /// <returns>Stream.</returns>
        Stream OpenFile(string path, FileMode mode, FileAccess access, FileShare share);

        /// <summary>
        /// Checks if file exists.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if file exists, <c>false</c> otherwise.</returns>
        bool FileExists(string path);

        /// <summary>
        /// Gets the last write time.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>DateTimeOffset.</returns>
        DateTimeOffset GetLastWriteTime(string path);

        /// <summary>
        /// Deletes a file.
        /// </summary>
        /// <param name="path">Path to the file.</param>
        void DeleteFile(string path);

        /// <summary>
        /// Deletes a directory.
        /// </summary>
        /// <param name="path">Path to the directory.</param>
        void DeleteDirectory(string path);
    }

    /// <summary>
    /// Enum FileMode
    /// </summary>
    public enum FileMode
    {
        /// <summary>
        /// The create new
        /// </summary>
        CreateNew = 1,
        /// <summary>
        /// The create
        /// </summary>
        Create = 2,
        /// <summary>
        /// The open
        /// </summary>
        Open = 3,
        /// <summary>
        /// The open or create
        /// </summary>
        OpenOrCreate = 4,
        /// <summary>
        /// The truncate
        /// </summary>
        Truncate = 5,
        /// <summary>
        /// The append
        /// </summary>
        Append = 6,
    }

    /// <summary>
    /// Enum FileAccess
    /// </summary>
    public enum FileAccess
    {
        /// <summary>
        /// The read
        /// </summary>
        Read = 1,
        /// <summary>
        /// The write
        /// </summary>
        Write = 2,
        /// <summary>
        /// The read write
        /// </summary>
        ReadWrite = 3,
    }

    /// <summary>
    /// Contains constants for controlling the kind of access other <see cref="IFileManager"/>
    /// objects can have to the same file.
    /// </summary>
    [Flags]
    public enum FileShare
    {
        // Summary:
        //     Declines sharing of the current file. Any request to open the file (by this
        //     process or another process) will fail until the file is closed.
        /// <summary>
        /// The none
        /// </summary>
        None = 0,
        //
        // Summary:
        //     Allows subsequent opening of the file for reading. If this flag is not specified,
        //     any request to open the file for reading (by this process or another process)
        //     will fail until the file is closed.
        /// <summary>
        /// The read
        /// </summary>
        Read = 1,
        //
        // Summary:
        //     Allows subsequent opening of the file for writing. If this flag is not specified,
        //     any request to open the file for writing (by this process or another process)
        //     will fail until the file is closed.
        /// <summary>
        /// The write
        /// </summary>
        Write = 2,
        //
        // Summary:
        //     Allows subsequent opening of the file for reading or writing. If this flag
        //     is not specified, any request to open the file for reading or writing (by
        //     this process or another process) will fail until the file is closed.
        /// <summary>
        /// The read write
        /// </summary>
        ReadWrite = 3,
        //
        // Summary:
        //     Allows subsequent deleting of a file.
        /// <summary>
        /// The delete
        /// </summary>
        Delete = 4,
        //
        // Summary:
        //     Makes the file handle inheritable by child processes.
        /// <summary>
        /// The inheritable
        /// </summary>
        Inheritable = 16,
    }
}
