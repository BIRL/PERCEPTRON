using System;
using System.IO;

namespace PerceptronAPI.Utility
{
    public class AddSuffixInName
    {
        public string AddSuffix(string filename, string suffix) // For adding FileUniqueId before the extension of the file
        {
            string FileDirectory = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            return Path.Combine(FileDirectory, String.Concat(fName, suffix, fExt));
        }

        public string QueryIdWithPathChange(string filename, string suffix, string AddPath) // For adding FileUniqueId before the extension of the file
        {
            string FileDirectory = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            return Path.Combine(FileDirectory, AddPath, String.Concat(suffix, fExt));
        }
    }
}