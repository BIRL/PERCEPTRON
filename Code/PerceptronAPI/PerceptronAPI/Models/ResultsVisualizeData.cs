using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ResultsVisualizeData
    {
        public string QueryId;
        public string NameOfFileWithPath;
        public AssembleInsilicoSpectra InsilicoSpectra;

        public ResultsVisualizeData(string cQueryId, string cNameOfFileWithPath, AssembleInsilicoSpectra cInsilicoSpectra)
        {
            QueryId = cQueryId;
            NameOfFileWithPath = cNameOfFileWithPath;
            InsilicoSpectra = cInsilicoSpectra;
        }
    }
}