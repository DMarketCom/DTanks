using System;
using System.IO;

namespace SHLibrary.Utils
{
    /// <summary>
    ///     Helper class to use instead of all File and Directory static methods.
    ///     It is important and very useful for cross-platform code.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        ///     Combines two path strings.
        /// </summary>
        /// <param name="path1">The first path. </param>
        /// <param name="path2">The second path.</param>
        /// <returns>
        ///     A string containing the combined paths. If one of the specified paths is a zero-length string, this method
        ///     returns the other path. If path2 contains an absolute path, this method returns path2.
        /// </returns>
        /// <seealso cref="Path.Combine" />
        public static string CombinePath(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        ///     Determines whether the given path refers to an existing directory on disk.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>true if path refers to an existing directory; otherwise, false.</returns>
        /// <seealso cref="Directory.Exists(string)" />
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public static bool FileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        ///     Creates all directories and sub-directories as specified by path.
        /// </summary>
        /// <param name="path">The directory path to create. </param>
        /// <seealso cref="Directory.CreateDirectory(string)" />
        public static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last
        ///     written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain write date and time information. </param>
        /// <returns>
        ///     A DateTime structure set to the date and time that the specified file or directory was last written to. This
        ///     value is expressed in UTC time.
        /// </returns>
        /// <seealso cref="File.GetLastWriteTimeUtc" />
        public static DateTime GetLastFileWriteTimeUtc(string path)
        {
            return File.GetLastWriteTimeUtc(path);
        }

        /// <summary>
        ///     Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading. </param>
        /// <returns>A StreamReader on the specified path.</returns>
        /// <seealso cref="File.OpenText" />
        public static StreamReader OpenTextFile(string path)
        {
            return File.OpenText(path);
        }

        /// <summary>
        ///     Opens an existing file for reading.
        /// </summary>
        /// <param name="path">The file to be opened for reading.</param>
        /// <returns>A read-only FileStream on the specified path.</returns>
        /// <seealso cref="File.OpenRead" />
        public static FileStream OpenFile(string path)
        {
            return File.OpenRead(path);
        }

        /// <summary>
        ///     Deletes the specified file. An exception is not thrown if the specified file does not exist.
        /// </summary>
        /// <param name="path">The name of the file to be deleted. </param>
        /// <seealso cref="File.Delete" />
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        ///     If exists - open file for writing first, and if not - try to create this file.
        /// </summary>
        /// <param name="path">The file to open or create. </param>
        /// <returns>An unshared FileStream that provides access to the specified file, with the write access.</returns>
        /// <seealso cref="File.Open(string,System.IO.FileMode)" />
        public static FileStream CreateOrRewriteFile(string path)
        {
            return FileExists(path)
                ? File.Open(path, FileMode.Truncate, FileAccess.Write)
                : File.Open(path, FileMode.Create, FileAccess.Write);
        }
    }
}
