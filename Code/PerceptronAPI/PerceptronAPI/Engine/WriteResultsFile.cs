using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using PerceptronAPI.Models;

namespace PerceptronAPI.Engine
{
    public class WriteResultsFile
    {
        public List<string> ResultFilesWrite(ScanResultsDownloadDataDto ScanData, string filePath)
        {
            List<string> AllResultFilesNames = new List<string>();
            var TopProteinsOfEachFile = new List<ProteinDto>();


            int NoOfFiles = ScanData.FileUniqueIdsList.Count;

            for (int i = 0; i < NoOfFiles; i++)
            {
                var IndividualFileId = ScanData.FileUniqueIdsList[i];   /// IndividualFileId will direct and/or seggregate the results of different input data files in SearchResults
                var IndividualNameOfFile = ScanData.FileNamesList[i];
                var IndividualUniqueFileName = ScanData.UniqueFileNameList[i];
                AllResultFilesNames.Add(WriteSingleResultsFile(ScanData, filePath, IndividualFileId, IndividualNameOfFile, IndividualUniqueFileName, TopProteinsOfEachFile));
            }

            if (NoOfFiles > 1)
            {
                string FileWithPath = filePath + ScanData.searchParameters.Title + ".csv";
                AllResultFilesNames.Add(WriteBatchResultsFile(FileWithPath, TopProteinsOfEachFile));
            }

            return AllResultFilesNames;
        }

        public string WriteSingleResultsFile(ScanResultsDownloadDataDto ScanData, string filePath, string IndividualFileId, string IndividualNameOfFile, string IndividualUniqueFileName, 
            List<ProteinDto> TopProteinsOfEachFile)
        {
            //var currentdirectory = Directory.GetCurrentDirectory();

            //var navigatepath = Path.GetFullPath(Path.Combine(currentdirectory, "..\\..\\PerceptronApi-tempResultsFolder\\"));
            string isEmptyFile = "FileIsEmpty";     //Cheking Whether Any data is written into the file or not...
            double ElapsedTime = 0.0;

            //[ON HOLD ENHANCEMENT] To avoiding replacing files because of same name using "IndividualUniqueFileName" (UniqueFileName) for saving the result fiel but User will see the file name of "IndividualNameOfFile"
            // (FileName) {a User selected file name of Input Peak List File}
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

                    sw.WriteLine("> " + ResultsData.Header + " | Score: " + Math.Round(ResultsData.Score, 6) + " | Molweight: " + Math.Round(ResultsData.Mw, 4)
                        + " | # Matched Fragments: " + Matches + " | Terminal Modification: " + ResultsData.TerminalModification + " | E-Value: " + ResultsData.Evalue);
                    sw.WriteLine(ResultsData.Sequence);

                    int NoOfPtmModifications = 0;
                    for (int index = 0; index < ScanData.PtmSites.Count; index++)
                    {
                        var PtmInfo = ScanData.PtmSites;

                        if (ResultsData.ResultId == PtmInfo[index].ResultId)
                        {
                            var PtmSitesInfo = new PostTranslationModificationsSiteDto(PtmInfo[index]);
                            NoOfPtmModifications = PtmSitesInfo.ListModName.Count;
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
                    if (ResultsData.ProteinRank == 1)   //Collecting Data for Batch File (*.csv). Each Files' Top Protein which will be written there
                    {
                        isEmptyFile = "FileIsNotEmpty"; //Setting Flag that File contains some data...
                        var tempTopProtein = new ProteinDto(Path.GetFileName(IndividualNameOfFile), ResultsData.Header, ResultsData.TerminalModification, ResultsData.Sequence, ResultsData.TruncationSite,
                            ResultsData.TruncationIndex, ResultsData.Score, ResultsData.Mw, NoOfPtmModifications, Matches, ElapsedTime, ResultsData.Evalue);
                        TopProteinsOfEachFile.Add(tempTopProtein);  
                    }

                }

            }
            if (isEmptyFile == "FileIsEmpty")   //If File does not contain any data...
            {
                sw.WriteLine("No Result Found Please search with another set of parameters");
            }
            sw.Close();
            return Path.GetFileName(FileWithPath);
        }


        public string WriteBatchResultsFile(string FileWithPath, List<ProteinDto> TopProteinsOfEachFile)
        {



            //string FileWithPath = filePath + NameofFile;


            ////Preparation of Data for Printing...

            //var TopProteinOfEachFile = ScanData.ListOfSearchResults.Where(x => x.ProteinRank == 1).ToList();            //Select(x => x.ResultId).Distinct().ToList();
            //TopProteinOfEachFile = TopProteinOfEachFile.OrderByDescending(x => x.Score).ToList();

            //Preparation of Data for Printing...

            if (File.Exists(FileWithPath))
                File.Delete(FileWithPath); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            //MAKING COLUMN NAMES
            string HeaderOfCsv = "File Name,Protein Header,Terminal Modification,Protein Seqeunce,Protein Truncation,Truncation Position,Score,Molecular Weight,No of Modifications,No of Fragments Matched,Run Time,E-Value";
            sw.WriteLine(HeaderOfCsv);

            for (int i = 0; i < TopProteinsOfEachFile.Count; i++) //is this correct alternate for directorycontents
            {
                var Protein = TopProteinsOfEachFile[i];

                var Truncation_Message = "";
                if (Protein.TruncationSite == "Left")
                {
                    Truncation_Message = "Truncation at N-Terminal Side";
                }
                else if (Protein.TruncationSite == "Right")
                {
                    Truncation_Message = "Truncation at C-Terminal Side";
                }
                else
                {
                    Truncation_Message = "No Truncation";
                }


                //Writing in file
                sw.WriteLine(Protein.FileName + "," + Protein.Header + "," + Protein.TerminalModification + "," + Protein.Sequence +
                    "," + Truncation_Message + "," + Protein.TruncationSite + "," + Math.Round(Protein.Score, 6) + "," +
                    Math.Round(Protein.Mw, 4) + "," +
                    Protein.NoOfPtmModifications + "," + Protein.NoOfFragmentsMatched + "," + Protein.ElapsedTime + "," + Protein.Evalue);

                // Fetching File Name
                //int NoOfFiles = ScanData.FileUniqueIdsList.Count;
                //string IndividualFileId = "";
                //string FileName = "";

                //for (int index = 0; index < NoOfFiles; index++)
                //{
                //    IndividualFileId = ScanData.FileUniqueIdsList[index];   /// IndividualFileId will direct and/or seggregate the results of different input data files in SearchResults

                //    if (TopProteinOfEachFile[i].FileUniqueId == IndividualFileId)
                //    {
                //        FileName = Path.GetFileName(ScanData.FileNamesList[index]);   //   ScanData.FileNamesList[index]  "index"  value of is exaclty correspond to "index" value of ScanData.FileUniqueIdsList[index]
                //    }
                //}
                // Fetching File Name

                // Fetching Number of Matches

                //int LeftMatchedIndex = 0;
                //int RightMatchedIndex = 0;
                //if (TopProteinOfEachFile[i].LeftMatchedIndex != "")
                //    LeftMatchedIndex = TopProteinOfEachFile[i].LeftMatchedIndex.Split(',').Select(int.Parse).ToList().Count;
                //if (TopProteinOfEachFile[i].RightMatchedIndex != "")
                //    RightMatchedIndex = TopProteinOfEachFile[i].RightMatchedIndex.Split(',').Select(int.Parse).ToList().Count;
                //int NoOfFragmentsMatched = LeftMatchedIndex + RightMatchedIndex;
                // Fetching Number of Matches

                // Fetching Number of Ptm Sites
                //int NoOfPtmModifications = 0;
                //for (int iter = 0; iter < ScanData.PtmSites.Count; iter++)
                //{
                //    var PtmInfo = ScanData.PtmSites;

                //    if (TopProteinOfEachFile[i].ResultId == PtmInfo[iter].ResultId)
                //    {
                //        var PtmSitesInfo = new PostTranslationModificationsSiteDto(PtmInfo[iter]);
                //        NoOfPtmModifications = PtmSitesInfo.ListSite.Count;
                //    }

                //}
                // Fetching Number of Ptm Sites


            }
            sw.Close();
            return Path.GetFileName(FileWithPath);
        }


        public string WriteParametersInTXTFile(SearchParameter searchParameters, string filePath)
        {
            string FileName = "SearchParameters-qid-" + searchParameters.QueryId + ".txt";
            string FileWithPath = filePath + "\\" + FileName;
            if (File.Exists(FileWithPath))
                File.Delete(FileWithPath); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            string Enabled = "Enabled";
            string Disabled = "Disabled";

            /* SEARCH PARAMETERS */
            sw.WriteLine("User Defined Search Parameters");
            sw.WriteLine("\n");
            sw.WriteLine("Query Id = " + searchParameters.QueryId);  //Showing Query Id to the User
            sw.WriteLine("\n");
            sw.WriteLine("Title = " + searchParameters.Title);

            //File Names  /// HERE 
            if (searchParameters.EmailId != "")
                sw.WriteLine("Email Id = " + searchParameters.EmailId);
            else
                sw.WriteLine("Email Id = N/A");

            sw.WriteLine("Protein Database = " + searchParameters.ProteinDatabase);
            sw.WriteLine("Number Of Output Results = " + searchParameters.NumberOfOutputs);

            //BUG Mass Mode Not Present

            string FilterDb;
            if (searchParameters.FilterDb == "True")
                FilterDb = Enabled;
            else
                FilterDb = Disabled;

            sw.WriteLine("Filter Database = " + FilterDb);

            sw.WriteLine("Molecular Weight Tolerance = " + searchParameters.MwTolerance);

            if (searchParameters.Autotune == "True")
            {
                sw.WriteLine("Mass Tuner = Enabled");
                sw.WriteLine("Autotune Tolerance = " + searchParameters.SliderValue);
                sw.WriteLine("Neutral Loss = " + searchParameters.NeutralLoss);
            }
            else
                sw.WriteLine("Mass Tuner = Disabled");




            sw.WriteLine("Peptide Tolerance = " + searchParameters.PeptideTolerance + searchParameters.PeptideToleranceUnit);
            sw.WriteLine("Insilico Fragmentation Type = " + searchParameters.InsilicoFragType);
            sw.WriteLine("Special Ions = " + searchParameters.HandleIons);

            if (searchParameters.DenovoAllow == "True")
            {
                sw.WriteLine("Peptide Sequence Tag (PST) = Enabled");
                sw.WriteLine("Minimum PST tag Length = " + searchParameters.MinimumPstLength);
                sw.WriteLine("Maximum PST tag Length = " + searchParameters.MaximumPstLength);

                sw.WriteLine("Peptide Sequence Tag Hop Threshhold = " + searchParameters.HopThreshhold);
                sw.WriteLine("Peptide Sequence Tag Hop Threshold Unit = Da");
                sw.WriteLine("Peptide Sequence Tag Tolerance = " + searchParameters.PSTTolerance);
            }
            else
                sw.WriteLine("Peptide Sequence Tag (PST) = Disabled");

            string Truncation;
            if (searchParameters.Truncation == "True")
                Truncation = Enabled;
            else
                Truncation = Disabled;
            sw.WriteLine("Truncation = " + Truncation);

            sw.WriteLine("Terminal Modification = " + searchParameters.TerminalModification);

            string PtmAllow;
            if (searchParameters.PtmAllow == "True")
                PtmAllow = Enabled;
            else
                PtmAllow = Disabled;
            sw.WriteLine("Post Translational Modification (PTM) = " + PtmAllow);

            sw.WriteLine("Post Translational Modification Tolerance = " + searchParameters.PtmTolerance + "Units");
            sw.WriteLine("Cysteine Chemical Modification = " + searchParameters.CysteineChemicalModification);
            sw.WriteLine("Methionine Chemical Modification = " + searchParameters.MethionineChemicalModification);


            sw.WriteLine("Molecular Scoring Weight = " + searchParameters.MwSweight);
            sw.WriteLine("PST Scoring Weight = " + searchParameters.PstSweight);
            sw.WriteLine("Insilico Scoring Weight = " + searchParameters.InsilicoSweight);

            sw.Close();
            return FileName;
        }
    }
}




//var RawResults = Select y From AllResults Where (x=>x.FileId);//AllResults.Select(x => x.FileUniqueId).ToList();
//From Results in AllResults where (x=>x.FileId) select score;