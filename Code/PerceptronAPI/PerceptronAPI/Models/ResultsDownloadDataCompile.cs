using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerceptronAPI.Engine;
using PerceptronAPI.Repository;

namespace PerceptronAPI.Models
{
    public class ResultsDownloadDataCompile
    {
        public string QueryId;
        public string ResultId;
        public string MassSpectrumImageFilePath;
        public string JsonExpThrTable;


        public ResultsDownloadDataCompile(string cQueryId, string cResultId, string cMassSpectrumImageFilePath, string cJsonExpThrTable)
        {
            QueryId = cQueryId;
            ResultId = cResultId;
            MassSpectrumImageFilePath = cMassSpectrumImageFilePath;
            JsonExpThrTable = cJsonExpThrTable;
        }

    }
}