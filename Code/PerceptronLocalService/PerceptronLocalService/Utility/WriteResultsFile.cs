using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using PerceptronLocalService.DTO;
using System.Diagnostics;


namespace PerceptronLocalService.Utility
{
    public class WriteResultsFile
    {
        public string WriteParametersInTxtFile(SearchParametersDto SearchParameters, string filePath)
        {

            string FileWithPath = filePath + SearchParameters.Title + "_SearchParameters" + ".txt";
            if (File.Exists(FileWithPath))
                File.Delete(FileWithPath); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            string Enabled = "Enabled";
            string Disabled = "Disabled";

            /* SEARCH PARAMETERS */
            sw.WriteLine("User Defined Search Parameters");
            sw.WriteLine("\n");
            sw.WriteLine("Query Id = " + SearchParameters.Queryid);  //Showing Query Id to the User
            sw.WriteLine("\n");
            sw.WriteLine("Title = " + SearchParameters.Title);

            //File Names  /// HERE 
            if (SearchParameters.EmailId != "")
                sw.WriteLine("Email Id = " + SearchParameters.EmailId);
            else
                sw.WriteLine("Email Id = N/A");

            sw.WriteLine("Protein Database = " + SearchParameters.ProtDb);

            if (SearchParameters.FDRCutOff != "N/A")        //Updated 20210209
                sw.WriteLine("FDR Cutoff = " + SearchParameters.FDRCutOff + "%");
            else                  //Updated 20210209
                sw.WriteLine("FDR Cutoff = " + "N/A");

            sw.WriteLine("Number Of Output Results = " + SearchParameters.NumberOfOutputs);

            //BUG Mass Mode Not Present

            string FilterDb;
            if (SearchParameters.FilterDb == "True")
                FilterDb = Enabled;
            else
                FilterDb = Disabled;

            sw.WriteLine("Filter Database = " + FilterDb);

            sw.WriteLine("Molecular Weight Tolerance = " + SearchParameters.MwTolerance);

            if (SearchParameters.Autotune == "True")
            {
                sw.WriteLine("Mass Tuner = Enabled");
                sw.WriteLine("Autotune Tolerance = " + SearchParameters.SliderValue);
                sw.WriteLine("Neutral Loss = " + SearchParameters.NeutralLoss);
            }
            else
                sw.WriteLine("Mass Tuner = Disabled");




            sw.WriteLine("Peptide Tolerance = " + SearchParameters.PeptideTolerance + SearchParameters.PeptideToleranceUnit);
            sw.WriteLine("Insilico Fragmentation Type = " + SearchParameters.InsilicoFragType);
            sw.WriteLine("Special Ions = " + SearchParameters.HandleIons);

            if (SearchParameters.DenovoAllow == "True")
            {
                sw.WriteLine("Peptide Sequence Tag (PST) = Enabled");
                sw.WriteLine("Minimum PST tag Length = " + SearchParameters.MinimumPstLength);
                sw.WriteLine("Maximum PST tag Length = " + SearchParameters.MaximumPstLength);

                sw.WriteLine("Peptide Sequence Tag Hop Threshhold = " + SearchParameters.HopThreshhold);
                sw.WriteLine("Peptide Sequence Tag Hop Threshold Unit = Da");
                sw.WriteLine("Peptide Sequence Tag Tolerance = " + SearchParameters.PSTTolerance);
            }
            else
                sw.WriteLine("Peptide Sequence Tag (PST) = Disabled");

            string Truncation;
            if (SearchParameters.Truncation == "True")
                Truncation = Enabled;
            else
                Truncation = Disabled;
            sw.WriteLine("Truncation = " + Truncation);

            sw.WriteLine("Terminal Modification = " + SearchParameters.TerminalModification);

            string PtmAllow;
            if (SearchParameters.PtmAllow == "True")
                PtmAllow = Enabled;
            else
                PtmAllow = Disabled;
            sw.WriteLine("Post Translational Modification (PTM) = " + PtmAllow);

            sw.WriteLine("Post Translational Modification Tolerance = " + SearchParameters.PtmTolerance + "Units");
            sw.WriteLine("Cysteine Chemical Modification = " + SearchParameters.CysteineChemicalModification);
            sw.WriteLine("Methionine Chemical Modification = " + SearchParameters.MethionineChemicalModification);


            sw.WriteLine("Molecular Scoring Weight = " + SearchParameters.MwSweight);
            sw.WriteLine("PST Scoring Weight = " + SearchParameters.PstSweight);
            sw.WriteLine("Insilico Scoring Weight = " + SearchParameters.InsilicoSweight);

            sw.Close();
            return FileWithPath;
        }

        public List<string> WriteIndividualResultsFile(string ProteinSearchTitle, List<ResultsDownloadToBeWrite> ResultsDownloadToBeWriteList, string filePath)
        {
            var ListFileName = new List<string>(ResultsDownloadToBeWriteList.Count);
            //var _NoOfMatchedFragments = new NoOfMatchedFragments();

            string FileWithPath = "";
            var CandidateList = new List<ProteinDto>();



            for (int iter = 0; iter < ResultsDownloadToBeWriteList.Count; iter++)
            {
                CandidateList = ResultsDownloadToBeWriteList[iter].CandidateList;
                FileWithPath = filePath + ProteinSearchTitle + "_" + ResultsDownloadToBeWriteList[iter].FileName + "_Results.txt";
                ListFileName.Add(FileWithPath);

                if (CandidateList.Count != 0)
                {


                    if (File.Exists(FileWithPath))
                        File.Delete(FileWithPath); //Deleted Pre-existing file

                    var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);

                    var sw = new StreamWriter(fout);

                    for (int index = 0; index < CandidateList.Count; index++)
                    {
                        var Protein = CandidateList[index];



                        //var Matches = _NoOfMatchedFragments.NoOfMatchedFragmentsCount(0, Protein.LeftMatchedIndex, Protein.RightMatchedIndex); //Initial value of Matches = 0   //Updated 20201221 - Commented as we already have a MatchCounter so no need to recalculate 

                        sw.WriteLine("> " + Protein.Header + " | Score: " + Math.Round(Protein.Score, 6) + " | Molweight: " + Math.Round(Protein.Mw, 4)
                            + " | # Matched Fragments: " + Protein.MatchCounter + " | Terminal Modification: " + Protein.TerminalModification + " | E-Value: " + Protein.Evalue);  // Updated 20201221  Bug Fix
                        sw.WriteLine(Protein.Sequence);

                        if (Protein.PtmParticulars.Count != 0)
                        {
                            for (int i = 0; i < Protein.PtmParticulars.Count; i++)
                            {
                                sw.WriteLine("Modification Name: " + Protein.PtmParticulars[i].ModName + " | Modification Site: " + Protein.PtmParticulars[i].Site + " | Site Index: " + Protein.PtmParticulars[i].Index);
                            }
                        }
                        if (Protein.BlindPtmLocalizationInfo.Start != -1)
                        {
                            sw.WriteLine("Modification Name: Unknown | Modification Weight: " + Protein.BlindPtmLocalizationInfo.Mass
                                + " | Modification lies between index: " + Protein.BlindPtmLocalizationInfo.Start + " - " + Protein.BlindPtmLocalizationInfo.End);
                        }


                    }
                    sw.Close();
                }
                else
                {
                    if (File.Exists(FileWithPath))
                        File.Delete(FileWithPath); //Deleted Pre-existing file

                    var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);

                    var sw = new StreamWriter(fout);
                    sw.WriteLine("No Result Found Please search with another set of parameters");
                    sw.Close();

                }
            }
            return ListFileName;
        }

        //private int NoOfMatchedFragments(int Matches, List<int> LeftMatchedIndex, List<int> RightMatchedIndex)
        //{
        //    Matches = 0;
        //    if (LeftMatchedIndex.Count != 0)
        //    {
        //        Matches = LeftMatchedIndex.Count;
        //    }
        //    else if (RightMatchedIndex.Count != 0)
        //    {
        //        Matches = Matches + RightMatchedIndex.Count;
        //    }
        //    return Matches;
        //}

        public string WriteBatchResultsFile(string ProteinSearchTitle, string FDRCutOff, List<FalseDiscoveryRateDto> BatchModeFileProteins, string filePath)
        {
            string FileWithPath = filePath + ProteinSearchTitle + "_Results.csv";
            //var _NoOfMatchedFragments = new NoOfMatchedFragments();
            var _NoOfPtmModifications = new NoOfPtmModifications();

            if (File.Exists(FileWithPath))
                File.Delete(FileWithPath); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            //MAKING COLUMN NAMES

            var _TruncationIndexed = new TruncationIndexed();
            var _TruncationMessage = new TruncationMessage();
            int TruncationIndex;
            string TruncationAtSite = "";
            if (FDRCutOff == "N/A")  // || FDRCutOff == "0")  // Will Work for Simple File // Updated 20210301 - Bug fix
            {

                if (BatchModeFileProteins.Count == 0)  //For Empty File  Updated 20210326
                {
                    sw.WriteLine("No Result Found Please search with another set of parameters");
                    sw.Close();
                    return FileWithPath;
                }

                string HeaderOfCsv = "File Name,Protein Header,Terminal Modification,Protein Seqeunce,Protein Truncation,Truncation Position,Score,Molecular Weight,No of Modifications,No of Fragments Matched,Run Time,E-Value";
                sw.WriteLine(HeaderOfCsv);

                for (int i = 0; i < BatchModeFileProteins.Count; i++) //is this correct alternate for directorycontents
                {
                    var Protein = BatchModeFileProteins[i];

                    TruncationIndex = _TruncationIndexed.TruncationIndxing(Protein.TruncationIndex);
                    TruncationAtSite = _TruncationMessage.TypeOfTruncation(Protein.Truncation);

                    sw.WriteLine(BatchModeFileProteins[i].FileName + "," + Protein.Header + "," + Protein.TerminalModification + "," + Protein.Sequence +
                    "," + TruncationAtSite + "," + TruncationIndex + "," + Math.Round(Protein.Score, 6) + "," +
                    Math.Round(Protein.Mw, 4) + "," +
                    Protein.NumOfModifications + "," + Protein.MatchedFragments + "," + Protein.RunTime + "," + Protein.Evalue);
                }
            }


            else  // Will Work for FDR - Decoy Side
            {

                if (BatchModeFileProteins[0].BatchTargetList.Count == 0)  //For Empty File  Updated 20210326
                {
                    sw.WriteLine("No Result Found Please search with another set of parameters");
                    sw.Close();
                    return FileWithPath;
                }

                string HeaderOfCsv = "File Name,Protein Header,Terminal Modification,Protein Seqeunce,Protein Truncation,Truncation Position,Score,Molecular Weight,No of Modifications,No of Fragments Matched,Run Time,E-Value,FDR";

                sw.WriteLine(HeaderOfCsv);

                for (int i = 0; i < BatchModeFileProteins[0].BatchTargetList.Count; i++) //is this correct alternate for directorycontents   //Updated 20201222  Bug Fix
                {
                    var Protein = BatchModeFileProteins[0].BatchTargetList[i];

                    TruncationIndex = _TruncationIndexed.TruncationIndxing(Protein.TruncationIndex);
                    TruncationAtSite = _TruncationMessage.TypeOfTruncation(Protein.Truncation);

                    sw.WriteLine(Protein.FileName + "," + Protein.Header + "," + Protein.TerminalModification + "," + Protein.Sequence +
                    "," + TruncationAtSite + "," + TruncationIndex + "," + Math.Round(Protein.Score, 6) + "," +
                    Math.Round(Protein.Mw, 4) + "," +
                    Protein.NumOfModifications + "," + Protein.MatchedFragments + "," + Protein.RunTime + "," + Protein.Evalue + "," + BatchModeFileProteins[0].FdrValue[i]);
                }
                sw.WriteLine();
                int TotalProteins = BatchModeFileProteins[0].TargetListCount;   // Updated 20210213
                sw.WriteLine("Total protein count reported by experiment: " + TotalProteins);  //+1 is for because we remove the last entry in FalseDiscoveryRate.cs
                sw.WriteLine("Total protein count reported by experiment with E-value greater than 1E-10: " + BatchModeFileProteins[0].EvalueCount); // EvalueCount is different to the evalue i.e. the evalue of protein (Evalue)
                sw.WriteLine("Total protein reported with " + FDRCutOff + "% FDR: " + BatchModeFileProteins[0].NoOfProteins);
                sw.WriteLine("Unique proteins count reported with " + FDRCutOff + "% FDR: " + BatchModeFileProteins[0].NoOfUniqueProteins);
            }

            sw.Close();
            return FileWithPath;
        }

        public string ZippingOutputFiles(string Title, string QueryId, List<string> ResultsDownloadFileNames, string filePath)
        {
            string ZipFullFileName = filePath + Title + "_" + QueryId + ".zip";
            if (File.Exists(ZipFullFileName))
                File.Delete(ZipFullFileName); //Deleted Pre-existing file

            using (var archieve = ZipFile.Open(ZipFullFileName, ZipArchiveMode.Create)) // Creating Zip File
            {
                for (int i = 0; i < ResultsDownloadFileNames.Count; i++)
                {

                    archieve.CreateEntryFromFile(ResultsDownloadFileNames[i], Path.GetFileName(ResultsDownloadFileNames[i]));   // Adding all results files into the zip file
                    File.Delete(ResultsDownloadFileNames[i]); //Deleted Pre-existing file
                }
            }
            return ZipFullFileName;
        }
    }
}
