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
using System.IO;


namespace PerceptronAPI.Engine
{
    public class ResultsDownload
    {
        readonly IDataAccessLayer _dataLayer;

        public ResultsDownload()
        {
            _dataLayer = new SqlDatabase();
        }

        public List<ResultsDownloadDataCompile> Download(string QueryId)
        {
            var tempScanResults = _dataLayer.Scan_Results(QueryId); //Scanning Results   // Can get FileUniqueId from here
            
            var MegaData = new List<ResultsDownloadDataCompile>();

            for (int NoOfFile = 0; NoOfFile < tempScanResults.Count; NoOfFile++) // Looping On Files
            {
                var tempSummaryResults = _dataLayer.Summary_results(QueryId, tempScanResults[NoOfFile].FileId);  // Considering 0 as a Single File (batch mode will after )For the Time being

                for (int NoOfResultIds = 0; NoOfResultIds < tempSummaryResults.Count; NoOfResultIds++) // Looping On Results of Each File
                {
                    try
                    {

                        var tempDetailResults = _dataLayer.Detailed_Results(QueryId, tempSummaryResults[NoOfResultIds].ResultId);

                        var tempDetailedProteinHitView = _dataLayer.DetailedProteinHitView_Results(QueryId, tempSummaryResults[NoOfResultIds].ResultId);

                        var ImageForm = new DetailedProteinView();
                        var NameofFile = ImageForm.writeOnImage(tempDetailedProteinHitView);

                        var MassSpectra = new FormForGraph();
                        var InsilicoSpectra = MassSpectra.fillChart(tempDetailedProteinHitView);
                        //var abc = JsonConvert.DeserializeObject<AssembleInsilicoSpectra>(JsonString);


                        WritingCompleteDetailedResults(QueryId, tempDetailResults, tempDetailedProteinHitView, InsilicoSpectra);

                        //var Data = new ResultsDownloadDataCompile(QueryId, tempSummaryResults[NoOfResultIds].ResultId, NameofFile, JsonString);
                        //MegaData.Add(Data);
                        
                        
                    }
                    catch(Exception e)
                    {
                        // Drop msg here for User If any problem Happend  // Enhancements
                    }
                }
                //WritingCompleteDetailedResults(QueryId, tempSummaryResults,  MegaData);
            }

            MegaData = new List<ResultsDownloadDataCompile>();
            return MegaData;
        }
        public void WritingCompleteDetailedResults(string QueryId, DetailedResults DetailResults, DetailedProteinHitView DetailedProteinHitView, AssembleInsilicoSpectra InsilicoSpectra)
        {
            var searchParameters = _dataLayer.GetSearchParmeters(QueryId);

            //List<char> Data = DataPrepForWritingTxtFile(SearchParmeters, CompiledResults);

            var Results = DetailedProteinHitView.Results.Results;
            // For Loop
            //string NameofFile = QueryId + "_Rid_" + CompiledResults[0].ResultId + ".txt";

            string NameofFile = QueryId + "_Rid_" + DetailedProteinHitView.Results.Results.ResultId + ".txt";
            string FileWithPath = @"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Engine\Results\CompleteResults_"+ NameofFile ;

            if (File.Exists(NameofFile))
                    File.Delete(NameofFile); //Deleted Pre-existing file


            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            //File.CreateText(FileWithPath); //Create

            string Enabled = "Enabled";
            string Disabled = "Disabled";

            var sw = new StreamWriter(fout);

            /* SEARCH PARAMETERS */
            sw.WriteLine("User Defined Search Parameters");
            sw.WriteLine("\n");
            sw.WriteLine("Title = " + searchParameters.Title);

            //File Names  /// HERE 
            if (searchParameters.EmailId != "")
                sw.WriteLine("Email Id = " + searchParameters.EmailId);
            else
                sw.WriteLine("Email Id = User not Provided");

            sw.WriteLine("GuiMass = " + searchParameters.GuiMass);  // DEL IT NOT NEEDED
            sw.WriteLine("Protein Database = " + searchParameters.ProtDb);
            sw.WriteLine("Number Of Output Results = " + searchParameters.NumberOfOutputs);

            //BUG Mass Mode Not Present

            string FilterDb;
            if (searchParameters.FilterDb == 1)
                FilterDb = Enabled;
            else
                FilterDb = Disabled;

            sw.WriteLine("Filter Database = " + FilterDb);

            sw.WriteLine("Molecular Weight Tolerance = " + searchParameters.MwTolerance);

            string Autotune;
            if (searchParameters.Autotune == 1)
                Autotune = Enabled;
            else
                Autotune = Disabled;

            sw.WriteLine("Mass Tuner = " + Autotune);
            //sw.WriteLine("Autotune Tolerance = " + searchParameters.SliderValue);
            sw.WriteLine("Neutral Loss = " + searchParameters.NeutralLoss);


            sw.WriteLine("Peptide Tolerance = " + searchParameters.PeptideTolerance + searchParameters.PeptideToleranceUnit);
            sw.WriteLine("Insilico Fragmentation Type = " + searchParameters.InsilicoFragType);
            sw.WriteLine("Special Ions = " + searchParameters.HandleIons);

            string DenovoAllow;
            if (searchParameters.DenovoAllow == 1)
                DenovoAllow = Enabled;
            else
                DenovoAllow = Disabled;
            sw.WriteLine("Peptide Sequence Tag (PST) = " + DenovoAllow);
            sw.WriteLine("Minimum PST tag Length = " + searchParameters.MinimumPstLength);
            sw.WriteLine("Maximum PST tag Length = " + searchParameters.MaximumPstLength);

            sw.WriteLine("Peptide Sequence Tag Hop Threshhold = " + searchParameters.HopThreshhold);
            sw.WriteLine("Peptide Sequence Tag Hop Threshold Unit = " + searchParameters.HopTolUnit);
            sw.WriteLine("Peptide Sequence Tag Tolerance = " + searchParameters.PSTTolerance);


            string Truncation;
            if (searchParameters.Truncation == 1)
                Truncation = Enabled;
            else
                Truncation = Disabled;
            sw.WriteLine("Truncation = " + Truncation);

            sw.WriteLine("TerminalModification = " + searchParameters.TerminalModification);

            string PtmAllow;
            if (searchParameters.PtmAllow == 1)
                PtmAllow = Enabled;
            else
                PtmAllow = Disabled;
            sw.WriteLine("Post Translational Modification (PTM) = " + PtmAllow);

            sw.WriteLine("Post Translational Modification Tolerance = " + searchParameters.PtmTolerance);
            sw.WriteLine("Cysteine Chemical Modification = " + searchParameters.CysteineChemicalModification);
            sw.WriteLine("Methionine Chemical Modification = " + searchParameters.MethionineChemicalModification);

            
            sw.WriteLine("Molecular Scoring Weight = " + searchParameters.MwSweight);
            sw.WriteLine("PST Scoring Weight = " + searchParameters.PstSweight);
            sw.WriteLine("Insilico Scoring Weight = " + searchParameters.InsilicoSweight);

            /* PEAK LIST FILE DATA */
            sw.WriteLine("\n");
            sw.WriteLine("\n");
            sw.WriteLine("Peak List File Data");
            sw.WriteLine("\n");
            sw.WriteLine("Peak List File Masses = ");
            sw.WriteLine("Peak List File Intensities = ");


            string Nill = "Nill";
            /* RESULTS */
            sw.WriteLine("\n");
            sw.WriteLine("\n");
            sw.WriteLine("Results ");
            sw.WriteLine("\n");

            sw.WriteLine("Original Sequence = " + Results.OriginalSequence);
            sw.WriteLine("Sequence = " + Results.Sequence);
            sw.WriteLine("Peptide Sequence Score = " + Results.PstScore);
            sw.WriteLine("Insilico Score = " + Results.InsilicoScore);
            sw.WriteLine("Post Translational Modification Score = " + Results.PtmScore);
            sw.WriteLine("Average Score = " + Results.Score);
            sw.WriteLine("Molecular Weight Score = " + Results.MwScore);
            sw.WriteLine("Molecular Weight = " + Results.Mw);

            if (Results.PSTTags == "")
                Results.PSTTags = "Not Found Any PST Tag";
            sw.WriteLine("Peptide Sequence Tag(s) = " + Results.PSTTags);

            if (Results.RightMatchedIndex == "")
                Results.RightMatchedIndex = Nill;
            sw.WriteLine("Right Matched Index = " + Results.RightMatchedIndex);

            if(Results.RightPeakIndex == "")
                Results.RightPeakIndex = Nill;
            sw.WriteLine("Right Peak Index =" + Results.RightPeakIndex);

            if (Results.RightType == "")
                Results.RightType = Nill;
            sw.WriteLine("Right Type(s) = " + Results.RightType);

            if (Results.LeftMatchedIndex == "")
                Results.LeftMatchedIndex = Nill;
            sw.WriteLine("Left Matched Index = " + Results.LeftMatchedIndex);

            if (Results.LeftPeakIndex == "")
                Results.LeftPeakIndex = Nill;
            sw.WriteLine("Left Peak Index = " + Results.LeftPeakIndex);

            if (Results.LeftType == "")
                Results.LeftType = Nill;
            sw.WriteLine("Left Type(s) = " + Results.LeftType);

            if (Results.InsilicoMassLeft == "")
                Results.InsilicoMassLeft = Nill;
            sw.WriteLine("Insilico Mass Left Ions = " + Results.InsilicoMassLeft);

            if (Results.InsilicoMassRight == "")
                Results.InsilicoMassRight = Nill;
            sw.WriteLine("Insilico Mass Right Ions = " + Results.InsilicoMassRight);

            if (Results.InsilicoMassRight == "")
                Results.InsilicoMassLeftAo = Nill;
            sw.WriteLine("Insilico Mass Left Ao Ions = " + Results.InsilicoMassLeftAo);

            if (Results.InsilicoMassRight == "")
                Results.InsilicoMassRight = Nill;
            sw.WriteLine("Insilico Mass Left Bo Ions = " + Results.InsilicoMassLeftBo);

            if (Results.InsilicoMassLeftAstar == "")
                Results.InsilicoMassLeftAstar = Nill;
            sw.WriteLine("Insilico Mass Left Astar Ions = " + Results.InsilicoMassLeftAstar);

            if (Results.InsilicoMassLeftBstar == "")
                Results.InsilicoMassLeftBstar = Nill;
            sw.WriteLine("Insilico Mass Left Bstart Ions = " + Results.InsilicoMassLeftBstar);

            if (Results.InsilicoMassRightYo == "")
                Results.InsilicoMassRightYo = Nill;
            sw.WriteLine("Insilico Mass Right Yo Ions = " + Results.InsilicoMassRightYo);

            if (Results.InsilicoMassRightYo == "")
                Results.InsilicoMassRightYo = Nill;
            sw.WriteLine("Insilico Mass Right Ystar Ions = " + Results.InsilicoMassRightYstar);

            if (Results.InsilicoMassRightZo == "")
                Results.InsilicoMassRightZo = Nill;
            sw.WriteLine("Insilico Mass Right Zo Ions = " + Results.InsilicoMassRightZo);

            if (Results.InsilicoMassRightZoo == "")
                Results.InsilicoMassRightZoo = Nill;
            sw.WriteLine("Insilico Mass Right Zoo Ions = " + Results.InsilicoMassRightZoo);

            sw.WriteLine("Terminal Modification = " + Results.TerminalModification);

            if (Results.TruncationSite == "")
                Results.TruncationSite = Nill;
            sw.WriteLine("Truncation Site = " + Results.TruncationSite);

            sw.WriteLine("Truncation Index" + Results.TruncationIndex);



            /* Experimental & Theoretical Mass Matches */
            sw.WriteLine("\n");
            sw.WriteLine("\n");
            sw.WriteLine("Experimental & Theoretical Mz Matches");
            sw.WriteLine("\n");

            sw.WriteLine("Matced Index(es) = " + string.Join(",", InsilicoSpectra.ListIndices));
            sw.WriteLine("Fragment Ion(s) = " + string.Join(",", InsilicoSpectra.ListFragIon));
            sw.WriteLine("Experimental Mz(s) = " + string.Join(",", InsilicoSpectra.ListExperimental_mz));
            sw.WriteLine("Theoretical Mz(s) = " + string.Join(",", InsilicoSpectra.ListTheoretical_mz));
            sw.WriteLine("Absolute Error = " + string.Join(",", InsilicoSpectra.ListAbsError));







            //sw.WriteLine("Testing...");
            sw.Close();
            fout.Close();


            int adashfksdf = 1;

            //using (StreamWriter writer = new StreamWriter(fout, UF))
            //{
            //    writer.Write("fsdsfsfsf");
            //    writer.WriteLine("asasaasaafasdfsdfsdfdjklf");
                

                

            //}
            

        }

        public List<char> DataPrepForWritingTxtFile(SearchParameter SearchParameters, List<ResultsDownloadDataCompile> CompiledResults)
        {
            
            var aqw = SearchParameters.ToString();
            var chara = Convert.ToChar(SearchParameters);





            var aass = new List<char>();
            return aass;
        }
    }
}