using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ResultsDownloadDto
    {
        public List<string> AllResultFilesNames;
        public List<byte[]> ListOfFileBlobs;
        
        public ResultsDownloadDto(List<string> cAllResultFilesNames, List<byte[]> cListOfFileBlobs)
        {
            AllResultFilesNames = cAllResultFilesNames;
            ListOfFileBlobs = cListOfFileBlobs;
        }
    }
}