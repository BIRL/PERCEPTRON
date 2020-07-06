using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerceptronAPI.Models;
using PerceptronAPI.ServiceLayer;
using PerceptronAPI.Repository;
using PerceptronAPI.Controllers;
using GraphForm;
using Newtonsoft.Json;


namespace PerceptronAPI.Engine
{
    public class ResultsDownload
    {
        readonly IDataAccessLayer _dataLayer;

        public ResultsDownload()
        {
            _dataLayer = new SqlDatabase();
        }

        public void Download(string QueryId)
        {
            var tempScanResults = _dataLayer.Scan_Results(QueryId); //Scanning Results   // Can get FileUniqueId from here

            int i = 0;

            //QueryId
            //ResultId
            //ListNameOfImageFile
            //ListOfJsonExpThrTable
            //List

            
            var MegaData = new List<ResultsDownloadDataCompile>();

            for (int NoOfFile = 0; NoOfFile < tempScanResults.Count; NoOfFile++) // Looping On Files
            {
                var tempSummaryResults = _dataLayer.Summary_results(QueryId, tempScanResults[NoOfFile].FileId);  // Considering 0 as a Single File (batch mode will after )For the Time being

                for (int NoOfResultIds = 0; NoOfResultIds < tempSummaryResults.Count; NoOfResultIds++) // Looping On Results of Each File
                {
                    try
                    {
                        if(NoOfResultIds == 9){
                            int asaa = 1;
                        }


                        var tempDetailResults = _dataLayer.Detailed_Results(QueryId, tempSummaryResults[NoOfResultIds].ResultId);

                        var tempDetailedProteinHitView = _dataLayer.DetailedProteinHitView_Results(QueryId, tempSummaryResults[NoOfResultIds].ResultId);

                        var ImageForm = new DetailedProteinView();
                        var NameofFile = ImageForm.writeOnImage(tempDetailedProteinHitView);

                        string Json = "";
                        //SavingResultsDownloadData(QueryId, ResultId, NameofFile, Json);

                        var MassSpectra = new FormForGraph();
                        var JsonString = MassSpectra.fillChart(tempDetailedProteinHitView);
                        var abc = JsonConvert.DeserializeObject<AssembleInsilicoSpectra>(JsonString);

                        int a = 0;

                        var Data = new ResultsDownloadDataCompile(QueryId, tempSummaryResults[NoOfResultIds].ResultId, NameofFile, JsonString);
                        MegaData.Add(Data);
                    }
                    catch(Exception e)
                    {
                        int we = 0;
                    }                    


                }
            }







        }
    }
}