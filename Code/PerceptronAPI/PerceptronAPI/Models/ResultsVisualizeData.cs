using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;

namespace PerceptronAPI.Models
{
    public class ResultsVisualizeData
    {
        public string QueryId;
        public byte[] blob;
        //public string NameOfFileWithPath;
        public AssembleInsilicoSpectra InsilicoSpectra;
        public List<MsPeaksDto> PeakListData;  // Just Used in Results Writing in TXT file

        public ResultsVisualizeData(string cQueryId, byte[] cblob, AssembleInsilicoSpectra cInsilicoSpectra, List<MsPeaksDto> cPeakListData)
        {
            QueryId = cQueryId;
            blob = cblob;
            //NameOfFileWithPath = cNameOfFileWithPath;  //20200715  -- NameofFile
            InsilicoSpectra = cInsilicoSpectra;
            PeakListData = cPeakListData;
        }
    }
}