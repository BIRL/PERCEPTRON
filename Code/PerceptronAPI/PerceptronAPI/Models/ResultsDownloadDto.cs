using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ResultsDownloadDto
    {
        public string ZipFileWithPath;
        public List<byte[]> ListOfFileBlobs;
        
        public ResultsDownloadDto(string cZipFileWithPath, List<byte[]> cListOfFileBlobs)
        {
            ZipFileWithPath = cZipFileWithPath;
            ListOfFileBlobs = cListOfFileBlobs;
        }
    }
}