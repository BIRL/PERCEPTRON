using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerceptronAPI.Models;
using System.IO;

namespace PerceptronAPI.Engine
{
    public class WriteResultsFile
    {
        public List<string> ResultFilesWrite(ScanResultsDownloadDataDto ScanData, string filePath)
        {
            List<string> AllResultFilesNames = new List<string>();

            

            int NoOfFiles = ScanData.FileUniqueIdsList.Count;

            for (int i = 0; i < NoOfFiles; i++)
            {
                var IndividualFileId = ScanData.FileUniqueIdsList[i];   /// IndividualFileId will direct and/or seggregate the results of different input data files in SearchResults
                var IndividualNameOfFile = ScanData.FileNamesList[i];
                AllResultFilesNames.Add(WriteSingleResultsFile(ScanData, filePath, IndividualFileId, IndividualNameOfFile));
            }

            if (NoOfFiles > 1)
            {
                AllResultFilesNames.Add(WriteBatchResultsFile(ScanData, filePath));
            }

            return AllResultFilesNames;
        }

        public string WriteSingleResultsFile(ScanResultsDownloadDataDto ScanData, string filePath, string IndividualFileId, string IndividualNameOfFile)
        {
            //var currentdirectory = Directory.GetCurrentDirectory();

            //var navigatepath = Path.GetFullPath(Path.Combine(currentdirectory, "..\\..\\PerceptronApi-tempResultsFolder\\"));

            string InputPeakListName = Path.GetFileName(IndividualNameOfFile);  /// IndividualNameOfFile   is a a name of input PeakList
            string FileWithPath = filePath + "\\" + ScanData.searchParameters.Title + "_" + InputPeakListName;

            List<SearchResult> AllRawResults = ScanData.ListOfSearchResults;
            AllRawResults = AllRawResults.OrderBy(x => x.ProteinRank).ToList();

            if (File.Exists(FileWithPath))
                File.Delete(FileWithPath); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            for (int j = 0; j < AllRawResults.Count; j++)
            {
                if (AllRawResults[j].FileUniqueId == IndividualFileId)
                {
                    var ResultsData = AllRawResults[j];

                    // Fetching Number of Matches
                    int LeftMatchedIndex = 0;
                    int RightMatchedIndex = 0;
                    if (ResultsData.LeftMatchedIndex != "")
                        LeftMatchedIndex = ResultsData.LeftMatchedIndex.Split(',').Select(int.Parse).ToList().Count;
                    if (ResultsData.RightMatchedIndex != "")
                        RightMatchedIndex = ResultsData.RightMatchedIndex.Split(',').Select(int.Parse).ToList().Count;
                    int Matches = LeftMatchedIndex + RightMatchedIndex;
                    // Fetching Number of Matches

                    sw.WriteLine("> " + ResultsData.Header + " | Score: " + ResultsData.Score + " | Molweight: " + Math.Round(ResultsData.Mw, 4)
                        + " | # Matched Fragments: " + Matches + " | Terminal Modification: " + ResultsData.TerminalModification + " | E-Value: " + Math.Round(ResultsData.Evalue), 4);
                    sw.WriteLine(ResultsData.Sequence);

                    for (int index = 0; index < ScanData.PtmSites.Count; index++)
                    {
                        var PtmInfo = ScanData.PtmSites;

                        if (ResultsData.ResultId == PtmInfo[index].ResultId)
                        {
                            var PtmSitesInfo = new PostTranslationModificationsSiteDto(PtmInfo[index]);

                            for (int iter = 0; iter < PtmSitesInfo.ListSiteIndex.Count; iter++)
                            {
                                sw.WriteLine("Modification Name: " + PtmSitesInfo.ListModName[iter] + " | Modification Site: " +
                                    PtmSitesInfo.ListSite[iter] + " | Site Index: " + PtmSitesInfo.ListSiteIndex[iter]);
                            }
                        }
                    }

                    if (ScanData.searchParameters.PtmAllow == "True")
                    {
                        var BlindPtm = new BlindPtmInfo(ResultsData.BlindPtmLocalization);

                        if (BlindPtm.BlindPtmLocalizationStart != -1)
                        {
                            sw.WriteLine("Modification Name: Unknown | Modification Weight: " + BlindPtm.BlindPtmLocalizationMass
                                + " | Modification lies between index: " + BlindPtm.BlindPtmLocalizationStart + " - " + BlindPtm.BlindPtmLocalizationEnd);
                        }
                    }


                }
            }
            sw.Close();
            return Path.GetFileName(FileWithPath);
        }


        public string WriteBatchResultsFile(ScanResultsDownloadDataDto ScanData, string filePath)
        {

            //MAKING FILE TO WRITE IN //BATCHRUN
            string NameofFile = ScanData.searchParameters.Title + ".csv";


            string FileWithPath = filePath + NameofFile;


            //Preparation of Data for Printing...

            var TopProteinOfEachFile = ScanData.ListOfSearchResults.Where(x => x.ProteinRank == 1).ToList();            //Select(x => x.ResultId).Distinct().ToList();
            TopProteinOfEachFile.OrderByDescending(x => x.Score);

            //Preparation of Data for Printing...

            if (File.Exists(NameofFile))
                File.Delete(NameofFile); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            //MAKING COLUMN NAMES
            string HeaderOfCsv = "File Name,Protein Header,Terminal Modification,Protein Seqeunce,Protein Truncation,Truncation Position,Score,Molecular Weight,No of Modifications,No of Fragments Matched,Run Time,E-Value";
            sw.WriteLine(HeaderOfCsv);

            for (int i = 0; i < TopProteinOfEachFile.Count; i++) //is this correct alternate for directorycontents
            {

                // Fetching File Name
                int NoOfFiles = ScanData.FileUniqueIdsList.Count;
                string IndividualFileId = "";
                string FileName = "";

                for (int index = 0; index < NoOfFiles; index++)
                {
                    IndividualFileId = ScanData.FileUniqueIdsList[index];   /// IndividualFileId will direct and/or seggregate the results of different input data files in SearchResults

                    if (TopProteinOfEachFile[index].FileUniqueId == IndividualFileId)
                    {
                        FileName = Path.GetFileName(ScanData.FileNamesList[index]);   //   ScanData.FileNamesList[index]  "index"  value of is exaclty correspond to "index" value of ScanData.FileUniqueIdsList[index]
                    }
                }
                // Fetching File Name

                // Fetching Number of Matches

                int LeftMatchedIndex = 0;
                int RightMatchedIndex = 0;
                if (TopProteinOfEachFile[i].LeftMatchedIndex != "")
                    LeftMatchedIndex = TopProteinOfEachFile[i].LeftMatchedIndex.Split(',').Select(int.Parse).ToList().Count;
                if (TopProteinOfEachFile[i].RightMatchedIndex != "")
                    RightMatchedIndex = TopProteinOfEachFile[i].RightMatchedIndex.Split(',').Select(int.Parse).ToList().Count;
                int NoOfFragmentsMatched = LeftMatchedIndex + RightMatchedIndex;
                // Fetching Number of Matches
                
                // Fetching Number of Ptm Sites
                int NoOfPtmModifications = 0;
                for (int iter = 0; iter < ScanData.PtmSites.Count; iter++)
                {
                    var PtmInfo = ScanData.PtmSites;

                    if (TopProteinOfEachFile[i].ResultId == PtmInfo[iter].ResultId)
                    {
                        var PtmSitesInfo = new PostTranslationModificationsSiteDto(PtmInfo[iter]);
                        NoOfPtmModifications = PtmSitesInfo.ListSite.Count;
                    }

                }
                // Fetching Number of Ptm Sites

                var Truncation_Message = "";
                if (TopProteinOfEachFile[i].TruncationSite == "Left")
                {
                    Truncation_Message = "Truncation at N-Terminal Side";
                }
                else if (TopProteinOfEachFile[i].TruncationSite == "Right")
                {
                    Truncation_Message = "Truncation at C-Terminal Side";
                }
                else
                {
                    Truncation_Message = "No Truncation";
                    TopProteinOfEachFile[i].TruncationSite = "-1";
                }

                
                double ElapsedTime = Math.Round(0.0, 4);
                //Writing in file
                sw.WriteLine(FileName + "," + TopProteinOfEachFile[i].Header + "," + TopProteinOfEachFile[i].TerminalModification + "," + TopProteinOfEachFile[i].Sequence +
                    "," + Truncation_Message + "," + TopProteinOfEachFile[i].TruncationSite + "," + Math.Round(TopProteinOfEachFile[i].Score, 4) + "," +
                    Math.Round(TopProteinOfEachFile[i].Mw, 4) + "," +
                    NoOfPtmModifications + "," + NoOfFragmentsMatched + "," + ElapsedTime + "," + TopProteinOfEachFile[i].Evalue);
            }
            sw.Close();
            return NameofFile;
        }
    }
}




//var RawResults = Select y From AllResults Where (x=>x.FileId);//AllResults.Select(x => x.FileUniqueId).ToList();
//From Results in AllResults where (x=>x.FileId) select score;