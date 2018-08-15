using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epic.Internals.Shared.Utils
{
    /// <summary>
    /// Holds a collection of utilities to use with Files
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Searches the given directory and it's sub-directories for files matching the given text
        /// </summary>
        /// <param name="directoryPath">The directory path to search</param>
        /// <param name="searchText">The filename/pattern to find</param>
        /// <returns>The list of matching results</returns>
        public static List<string> SearchDirectory(string directoryPath, string searchText)
        {
            List<string> foundFiles = new List<string>();
            SearchDirectoryPrivate(directoryPath, searchText, ref foundFiles);
            return foundFiles;
        }

        /// <summary>
        /// Searches the given directory and it's sub-directories for the first matching file
        /// </summary>
        /// <param name="directoryPath">The directory path to search</param>
        /// <param name="searchText">The filename/pattern to find</param>
        /// <returns>The full filepath for the first matching result</returns>
        public static string FindFirstFile(string directoryPath, string searchText)
        {
            List<string> foundFiles = new List<string>();
            SearchDirectoryPrivate(directoryPath, searchText, ref foundFiles, true);
            return foundFiles.Count > 0 ? foundFiles[0] : "";
        }


        /// <summary>
        /// Searches the given directory and it's sub-directories for files matching the search text
        /// </summary>
        /// <param name="directoryPath">The directory path to search</param>
        /// <param name="searchText">The filename to find</param>
        /// <param name="foundFiles">The list of matching files</param>
        /// <param name="findFirstMatch">Flag to only find the first match</param>
        private static void SearchDirectoryPrivate(string directoryPath, string searchText, ref List<string> foundFiles, bool findFirstMatch = false)
        {
            try
            {
                //First try files in the given dir
                foreach (string fileName in Directory.GetFiles(directoryPath, searchText))
                {
                    foundFiles.Add(fileName);
                    if (findFirstMatch)
                    {
                        return;
                    }
                }

                //If not found, recurse through dirs
                foreach (string dirName in Directory.GetDirectories(directoryPath))
                {
                    SearchDirectoryPrivate(dirName, searchText, ref foundFiles);
                }
            }
            catch (Exception)
            {
                //dont do anything
            }

        }
    }
}
