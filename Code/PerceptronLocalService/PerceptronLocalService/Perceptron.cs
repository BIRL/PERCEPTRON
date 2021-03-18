using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.IO;
using PerceptronLocalService.Engine;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Repository;
using PerceptronLocalService.Utility;
using System.Management;
using System.Globalization;
//using System.IO.Compression;
////using PerceptronLocalService.Testing;
///////For getting GPU version/////
//using Cudafy;
//using Cudafy.Host;
//using Cudafy.Translator;
using System.Runtime.InteropServices;
using PerceptronLocalService.Models;

namespace PerceptronLocalService
{
    public class Perceptron
    {
        readonly IPeptideSequenceTagScoring _pstFilter;
        readonly IPeptideSequenceTagGenerator _pstGenerator;
        readonly IDataAccessLayer _dataLayer;
        private readonly IWholeProteinMassTuner _wholeProteinMassTuner;
        private readonly IMolecularWeightModule _molecularWeightModule;
        private readonly IPostTranslationalModificationModule _postTranslationalModificationModule;
        private readonly IInsilicoFragmentsAdjustment _insilicoFragmentsAdjustment;
        private readonly IInsilicoFilter _insilicoFilter;
        private readonly IPeakListFileReader _peakListFileReader;
        private readonly IProteinRepository _proteinRepository;
        private readonly ITruncation _Truncation;  // Protection Level Decided Later
        private readonly ITerminalModifications _TerminalModifications; //Protection Level Decided Later
        private readonly IBlindPTMModule _BlindPostTranslationalModificationModule;



        public Perceptron()
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////// THAT CODE WILL RUN THE CPU FILES(*Cpu.cs/*CPU.cs) FOR PROCESSING THE JOB/////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            _pstFilter = new PstFilterCpu();
            _pstGenerator = new PstGeneratorCpu();
            _dataLayer = new SQLDatabase();
            _wholeProteinMassTuner = new WholeProteinMassTunerCpu();
            _molecularWeightModule = new MwModule();
            _proteinRepository = new ProteinRepositorySql();
            _postTranslationalModificationModule = new PostTranslationalModificationModuleCpu();
            _BlindPostTranslationalModificationModule = new BlindPtmCpu();
            _insilicoFragmentsAdjustment = new InsilicoFragmentsAdjustmentCpu();
            _insilicoFilter = new InsilicoFilterCpu();
            _peakListFileReader = new PeakListFileReader();
            _Truncation = new TruncationCPU();
            _TerminalModifications = new TerminalModificationsCPU();


            ///////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////// THAT CODE WILL RUN THE GPU FILES(*Gpu.cs/*GPU.cs) FOR PROCESSING THE JOB/////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////


            ////_pstFilter = new PstFilterGpu();
            //_pstGenerator = new PstGeneratorGpu();
            //_dataLayer = new SQLDatabase();
            //_wholeProteinMassTuner = new WholeProteinMassTunerGpu();
            //_molecularWeightModule = new MwModule();
            //_proteinRepository = new ProteinRepositorySql();
            //_postTranslationalModificationModule = new PtmCpu();
            //_insilicoFragmentsAdjustment = new InsilicoFragmentsAdjustmentCpu();
            //_insilicoFilter = new InsilicoFilterCpu();
            //_peakListFileReader = new PeakListFileReader();

        }

        private List<List<ProteinDto>> ParameterBasedDbSelection(SearchParametersDto parameters, List<List<ProteinDto>> AllDatabasesOfProteins)
        {
            var SqlDatabases = new List<List<ProteinDto>>() { new List<ProteinDto>(), new List<ProteinDto>() };

            if (parameters.ProtDb == "Human")  // For Selecting Simple Human Database
            {
                SqlDatabases[0] = AllDatabasesOfProteins[0];
                if (parameters.FDRCutOff != "N/A")      // Will work for FDR side. ...For Selecting Human Decoy Database
                {
                    SqlDatabases[1] = AllDatabasesOfProteins[1];
                }
            }
            else       // For Selecting Simple Ecoli Database
            {
                SqlDatabases[0] = AllDatabasesOfProteins[2];
                if (parameters.FDRCutOff != "N/A")       // Will work for FDR side. ...For Selecting Ecoli Decoy Database
                {
                    SqlDatabases[1] = AllDatabasesOfProteins[3];
                }
            }
            return SqlDatabases;
        }

        private bool CheckGpu()
        {
            bool IsGpu = false;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DisplayConfiguration");    //ITS HEALTHY....    
            string graphicsCard = string.Empty;
            foreach (ManagementObject mo in searcher.Get())
            {
                foreach (PropertyData property in mo.Properties)
                {
                    if (property.Name == "Description")
                    {
                        graphicsCard = property.Value.ToString();
                        NativeCudaCalls.InitializingGpu();
                        //IsGpu = true;
                    }
                }
            }
            return IsGpu;
        }


        public void Start()
        {
            PerceptronSdkResultsAvailable();
            DeleteOldResultFiles();  //COMMENTED FOR THE TIME BEING...!!!  //20201224
            bool IsGpu = CheckGpu();   // Check is Gpu exist into the system
            Stopwatch AllDatabasesOfProteinsTime = new Stopwatch();
            AllDatabasesOfProteinsTime.Start();
            var AllDatabasesOfProteins = FastaReader.FetchingSqlDatabaseProteins();   // Will fetch all four databases (Human, Human Decoy, Ecoli, Ecoli Decoy)
            AllDatabasesOfProteinsTime.Stop();

            while (true)
            {

                var pendingJobs = _dataLayer.ServerStatus();
                var pendingJobsParameters = pendingJobs.Select(element => _dataLayer.GetParameters(element.QueryId));

                
                foreach (var searchParameters in pendingJobsParameters)
                {
                    //Send_Results_Link(searchParameters);
                    //_dataLayer.Set_Progress(searchParameters.Queryid, 100);

                    var SqlDatabases = ParameterBasedDbSelection(searchParameters, AllDatabasesOfProteins);

                    var TotalTime = new Stopwatch();
                    TotalTime.Start();
                    PerformSearch(IsGpu, searchParameters, SqlDatabases);
                    TotalTime.Stop();
                    string time = TotalTime.Elapsed.ToString();
                    int a = 1;
                }
                //System.Threading.Thread.Sleep(10000);
            }
        }

        public void Stop()
        {
            //ignored
        }

        private void PerceptronSdkResultsAvailable()
        {
            DateTime JobSubmissionTime = DateTime.Now.AddDays(-20);
            List<PerceptronSdkResults> PerceptronSdkResults = _dataLayer.PreparePerceptronSdkResults(JobSubmissionTime);

        }

        public void DeleteOldResultFiles()   //Delete the result files except of last 2 days
        {
            string OldPath = @"C:\PerceptronResultsDownload\ResultsReadilyAvailable";
            string NewPath = @"C:\PerceptronResultsDownload\ResultsToBeDeleted\";

            var JobStatusDataStoreInDb = new PerceptronDatabaseEntities();

            DateTime startDate = DateTime.Now.AddYears(-1);  //Start from last year
            DateTime endDate = DateTime.Now.AddDays(-2);     //Till last 2days

            DirectoryInfo di = new DirectoryInfo(OldPath);
            FileInfo[] files = di.GetFiles();
            List<string> FilesToBeDeleted = files.Where(fi => fi.CreationTime >= startDate && fi.CreationTime <= endDate).Select(fi => fi.FullName).ToList();   //#Enhancement Don't use {{ Select }}

            int TotalFilesToBeDel = FilesToBeDeleted.Count;
            for (int iter = 0; iter < TotalFilesToBeDel - 1; iter++)
            {
                File.Delete(FilesToBeDeleted[iter]);
            }
        }

        public static void Sending_Email(SearchParametersDto p, string EmailMsg)
        {
            var emailaddress = p.EmailId;
            using (var mm = new MailMessage("dummyemail@lums.edu.pk", emailaddress))
            {
                string BaseUrl = "https://perceptron.lums.edu.pk/";

                if (EmailMsg == "ResultsReady")
                {
                    mm.Subject = "PERCEPTRON: Protein Search Results";
                    var body = "Dear User,";
                    body += "<br/><br/> The results for protein search query submitted at " + DateTime.Now.ToString() + " with job title \"" +
                            p.Title + "\" have been completed. The complete results are available at";
                    body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                    body += " If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If you encounter any kind of problem, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";

                    body += "</br></br>Thank You for using Perceptron.";
                    body += "</br><b>The PERCEPTRON Team</b>";
                    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                    mm.Body = body;
                }
                else if (EmailMsg == "ProteinListEmpty" || EmailMsg == "Exception") // Email Msg for Something Wrong With Entered Query
                {
                    mm.Subject = "PERCEPTRON: Protein Search Results";
                    var body = "Dear User,";
                    body += "<br/><br/> Search couldn't complete for protein search query submitted at " + DateTime.Now.ToString() + " with job title \"" +
                            p.Title + "\" Please check your search parameters and data file.";
                    //body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                    body += "</br> If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If problem still persists, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";
                    body += "</br> Results are kept on the server for two days. Please download your results. There is no way to retrieve the data older than 48 hours.";

                    body += "</br></br>Thank You for using Perceptron.";
                    body += "</br><b>The PERCEPTRON Team</b>";
                    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                    mm.Body = body;
                }
                //else if (EmailMsg == -2)
                //{
                //    mm.Subject = "PERCEPTRON: Invalid Parameters";
                //    var body = "Dear User,";
                //    body += "<br/><br/> Search couldn't complete for protein search query submitted at " + DateTime.Now.ToString() + " with job title \"" +
                //            p.Title + "\" Either MS1 or MS2 are not accurate enough to perform Mass Tuning. Deactivate auto-tune option in main GUI to proceed.";
                //    //body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                //    body += "</br> If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If problem still persists, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";

                //    body += "</br></br>Thank You for using Perceptron.";
                //    body += "</br><b>The PERCEPTRON Team</b>";
                //    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                //    mm.Body = body;
                //}


                mm.IsBodyHtml = true;
                var networkCred = new NetworkCredential("dummyemail@lums.edu.pk", "*****");
                var smtp = new SmtpClient
                {
                    Host = "smtp.office365.com",
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Credentials = networkCred,
                    Port = 587
                };
                try
                {
                    smtp.Send(mm);
                }
                catch (Exception e)
                {
                    if (e is System.Net.Mail.SmtpException)
                        emailaddress = "das bad";

                }
            }
        }


        private void PerformSearch(bool IsGpu, SearchParametersDto parameters, List<List<ProteinDto>> SqlDatabases)
        {
            

            //Logging.CreateDirectory();
            //Logging.DumpParameters(parameters);

            //var counter = 0;
            string EmailMsg = "";
            int ProgressStatus = 10;  // If ProgressStatus = 10(Job is running) & ProgressStatus = 100 (Job is done) & ProgressStatus = -1 (Job is not complete an error occured) //Updated 20201118
            var numberOfPeaklistFiles = parameters.PeakListFileName.Length;  //Number of files uploaded by user

            WriteResultsFile _WriteResultsFile = new WriteResultsFile();
            string Path = @"C:\PerceptronResultsDownload\ResultsReadilyAvailable\";

            List<string> ResultsDownloadFileNames = new List<string>(numberOfPeaklistFiles + 2);  // Used to Collect the names of the files for Zipping (File Zip) Purpose // Just for approximate Capacity of the list.
            //var DecoyTopFinalCandidateProteinList = new List<ProteinDto>(numberOfPeaklistFiles);
            List<ResultsDownloadToBeWrite> ResultsDownloadToBeWriteList = new List<ResultsDownloadToBeWrite>();   // For storing all individual(single) results files


            //List<ResultsDownloadToBeWrite> BatchModeFileProteins = new List<ResultsDownloadToBeWrite>(numberOfPeaklistFiles);
            //var ResultDataStoreInDb = new PerceptronDatabaseEntities();
            //var PeakDataStoreInDb = new PerceptronDatabaseEntities();
            //var ZipFileDataStoreInDb = new PerceptronDatabaseEntities();
            //var JobStatusDataStoreInDb = new PerceptronDatabaseEntities();

            //_dataLayer.Set_Progress(JobStatusDataStoreInDb, parameters.Queryid, ProgressStatus);  // Showing Status of Query as Runnning...!!!
            _dataLayer.Set_Progress(parameters.Queryid, ProgressStatus);  // Showing Status of Query as Runnning...!!!

            Stopwatch PstTime = new Stopwatch();
            Stopwatch SpectralComparisonTime = new Stopwatch();
            Stopwatch BlindPtmSearchTime = new Stopwatch();
            Stopwatch TruncatedTime = new Stopwatch();
            Stopwatch InsilicoFragTime = new Stopwatch();
            Stopwatch EvalueTime = new Stopwatch();
            Stopwatch AddRangeTime = new Stopwatch();




            //var SqlDatabases = _proteinRepository.FetchingSqlDatabaseProteins(parameters);

            int iterate = 1;
            if (parameters.FDRCutOff != "N/A") // Will work for FDR side   //Updated 20210209
            {
                iterate = 2;
            }
            
            var DataForBatchFileAndFdr = new List<FalseDiscoveryRateDto>(numberOfPeaklistFiles);
            var DecoyDataForBatchFileAndFdr = new List<FalseDiscoveryRateDto>(numberOfPeaklistFiles);

            for (var fileNumber = 0; fileNumber < numberOfPeaklistFiles; fileNumber++)
            {
                //Logging.CreatePeakFileDirectory(fileNumber);
                //EmailMsg = 0;  // EmailMsg == 1 Means All Good & == -1 Means Something Wrong & == -2 Means Invalid Parameters (for Mass Tuner)
                

                try
                {
                    var executionTimes = new ExecutionTimeDto();
                    var pipeLineTimer = new Stopwatch();
                    pipeLineTimer.Start();


                    //Step 0 - Reading the Peaklist File
                    var massSpectrometryData = PeakListFileReaderModuleModule(parameters, fileNumber, executionTimes);
                    //  Logging.DumpMsData(massSpectrometryData);


                    //Step 1 - 1st Algorithm - Mass Tuner 
                    var old = massSpectrometryData.WholeProteinMolecularWeight;
                    var PstTags = new List<PstTagList>();

                    if (IsGpu == false)  // CPU side Mass Tuner & Pst
                    {
                        //Logging.DumpMwTunerResult(massSpectrometryData);
                        ExecuteMassTunerModule(parameters, massSpectrometryData, executionTimes);
                        //Step  - 2nd Algorithm - Peptide Sequence Tags (PSTs)
                        PstTime.Start();
                        
                        PstTags = ExecuteDenovoModule(parameters, massSpectrometryData, executionTimes);
                        PstTime.Stop();
                    }
                    else  // GPU side Mass Tuner & Pst  //// --- GPU Code Below ---   Updated: 20210223
                    {
                        if (parameters.Autotune == "True" || parameters.DenovoAllow == "True")
                        {
                            var MassTunerAndPstData = ExecuteMassTunerModuleGpu(massSpectrometryData, parameters, executionTimes);
                            PstTags = ConvertStructToPstTagList(MassTunerAndPstData);
                            massSpectrometryData.WholeProteinMolecularWeight = MassTunerAndPstData[0].MassTuner;
                        }
                        
                    }   //// --- GPU Code Above ---   Updated: 20210223

                    if (massSpectrometryData.WholeProteinMolecularWeight == 0)
                    {
                        massSpectrometryData.WholeProteinMolecularWeight = old; //If Mass Tuner gives tunned mass = 0 etc. then, use the Peak list file Intact mass 
                    }

                    

                    //Logging.DumpModifiedProteins(candidateProteins);
                    List<newMsPeaksDto> peakData2DList = peakDataList(massSpectrometryData); //Another "Peak data" storing List //Temporary

                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//
                    //* From Here Simple Database and decoy database work will start  //Updated 20201116 *//
                    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~//

                    for (int iterations = 0; iterations < iterate; iterations++)   // This Loop's first iteration is used for Search with UniProtDB and second iteration for  Search with DecoyDB
                    {
                        var SQLDataBaseProteins = new List<ProteinDto>();

                        if (iterations == 0)
                        {
                            SQLDataBaseProteins = SqlDatabases[0];  //For Simple Database
                        }
                        else
                        {
                            SQLDataBaseProteins = SqlDatabases[1];  //For Decoy Database;
                        }

                        //Step 2 - (1st)Candidate Protein List (Simple) & Candidate Protein List Truncated  --- (In SPECTRUM: Score_Mol_Weight{Adding scores with respect to the Mass difference with Intact Mass})
                        var candidateProteins = new List<ProteinDto>();
                        var CandidateProteinListTruncated = new List<ProteinDto>();


                        //Fetching Candidate Proteins From User Selected DataBase
                        ////// SHOULD USE THIS........""  List<newMsPeaksDto> peakData2DList  ""
                        var CandidateProteinListsInfo = GetCandidateProtein(parameters, massSpectrometryData, PstTags, SQLDataBaseProteins, executionTimes);
                        candidateProteins = CandidateProteinListsInfo.CandidateProteinList;
                        CandidateProteinListTruncated = CandidateProteinListsInfo.CandidateProteinListTruncated;

                        //Score Proteins on Intact Protein Mass  (Adding scores with respect to the Mass difference with Intact Mass)
                        ScoringByMolecularWeight(parameters, massSpectrometryData.WholeProteinMolecularWeight, candidateProteins); // Scoring for Simple Candidate Protein List

                        //Logging.DumpCandidateProteins(candidateProteins);

                        //////UpdatedParse_database.m
                        //candidateProteins = new List<ProteinDto>();
                        candidateProteins = UpdateGetCandidateProtein(parameters, PstTags, candidateProteins, peakData2DList[0].Mass);
                        if (candidateProteins.Count == 0 && CandidateProteinListTruncated.Count == 0) // Its Beacuse Data File Having not Enough Info(Number of MS2s are vary few)
                        {
                            EmailMsg = "ProteinListEmpty"; // -1;
                            if (numberOfPeaklistFiles == 1 && iterations == 0) //If user gives one input file and file has not Candidte Protein list  //Updated 20201216
                            {
                                ProgressStatus = -1;
                            }
                            else if (numberOfPeaklistFiles > 1 && ProgressStatus == 10 && iterations == 0)  // If all files have not Candidte Protein list  //Updated 20201216
                            {
                                ProgressStatus = -1;
                            }

                            // Results Download Part 1 of 3  BELOW //
                            if (iterations == 0)
                            {
                                var tempResultsDownloadToBeWriteList = new ResultsDownloadToBeWrite(System.IO.Path.GetFileNameWithoutExtension(parameters.PeakListFileName[fileNumber]), candidateProteins);
                                ResultsDownloadToBeWriteList.Add(tempResultsDownloadToBeWriteList);
                            }
                            // Results Download Part 1 of 3  ABOVE //

                            continue;
                        }

                        InsilicoFragTime.Start();
                        candidateProteins = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(candidateProteins, parameters.InsilicoFragType, parameters.HandleIons);
                        InsilicoFragTime.Stop();

                        // Blind PTM Algos (BlindPTMExtraction & BlindPTMGeneral)

                        BlindPtmSearchTime.Start();
                        var CandidateProteinListBlindPtmModified = new List<ProteinDto>();
                        CandidateProteinListBlindPtmModified = ExecutePostTranslationalModificationsModule(parameters, candidateProteins, peakData2DList, executionTimes);
                        candidateProteins.AddRange(CandidateProteinListBlindPtmModified);
                        BlindPtmSearchTime.Stop();


                        //Step 4 - ??? Algorithm - Spectral Comparison
                        SpectralComparisonTime.Start();
                        var CandidateProteinswithInsilicoScores = new List<ProteinDto>();
                        CandidateProteinswithInsilicoScores = ExecuteSpectralComparisonModule(parameters, candidateProteins, peakData2DList, executionTimes);
                        SpectralComparisonTime.Stop();



                        //BlindPTMLocalization: Localizing Unknown mass shift
                        BlindPtmSearchTime.Start();
                        CandidateProteinswithInsilicoScores = _BlindPostTranslationalModificationModule.BlindPTMLocalization(CandidateProteinswithInsilicoScores, peakData2DList[0].Mass, parameters);
                        BlindPtmSearchTime.Stop();

                        //Logging.DumpInsilicoScores(candidateProteins);

                        //Executing Truncation 
                        TruncatedTime.Start();
                        var CandidateProteinListTrucnatedwithInsilicoScores = Truncation_Engine(parameters, CandidateProteinListTruncated, PstTags, peakData2DList, executionTimes);
                        TruncatedTime.Stop();

                        
                        int FinalCandidateProteinListCapacity = CandidateProteinswithInsilicoScores.Count + CandidateProteinListTrucnatedwithInsilicoScores.Count;
                        var FinalCandidateProteinListforFinalScoring = new List<ProteinDto>(FinalCandidateProteinListCapacity);    // Updated 20201203 Capacity defined

                        AddRangeTime.Start();
                        FinalCandidateProteinListforFinalScoring.AddRange(CandidateProteinswithInsilicoScores);
                        FinalCandidateProteinListforFinalScoring.AddRange(CandidateProteinListTrucnatedwithInsilicoScores);
                        AddRangeTime.Stop();

                        if (FinalCandidateProteinListforFinalScoring.Count == 0) // Its Beacuse Data File Having not Enough Info(Number of MS2s are vary few)   //Updated 20200114
                        {
                            EmailMsg = "ProteinListEmpty"; // -1;
                            if (numberOfPeaklistFiles > 1 && ProgressStatus == 10 && iterations == 0)  // If all files have not Candidte Protein list  //Updated 20200114
                            {
                                ProgressStatus = -1;
                            }
                            else if (numberOfPeaklistFiles == 1 && ProgressStatus == 10 && iterations == 0)
                            {
                                ProgressStatus = -1;
                            }

                            // Results Download Part 1 of 3  BELOW //
                            if (iterations == 0) //For simple protein database: If file have not any Candidate Protein List
                            {
                                var tempResultsDownloadToBeWriteList = new ResultsDownloadToBeWrite(System.IO.Path.GetFileNameWithoutExtension(parameters.PeakListFileName[fileNumber]), FinalCandidateProteinListforFinalScoring);  //Updated 20210120 Bug fix
                                ResultsDownloadToBeWriteList.Add(tempResultsDownloadToBeWriteList);
                            }
                            else if (iterations == 1) //For decoy protein database: If file have not any Candidate Protein List   //Updated 20200114    //
                            {

                            }
                            // Results Download Part 1 of 3  ABOVE //

                            continue;
                        }

                        //CandidateProteinswithInsilicoScores = ExecuteProteoformScoringModule(parameters, CandidateProteinswithInsilicoScores); Its Healthy Just List Name Changed
                        FinalCandidateProteinListforFinalScoring = ExecuteProteoformScoringModule(parameters, FinalCandidateProteinListforFinalScoring);

                        // Ranking the Candidate Proteins according to their scores
                        FinalCandidateProteinListforFinalScoring = RankCandidateProteinsList(FinalCandidateProteinListforFinalScoring);

                        //Evalue 
                        EvalueTime.Start();
                        Evalue _Evalue = new Evalue();
                        _Evalue.ComputeEvalue(FinalCandidateProteinListforFinalScoring);
                        EvalueTime.Stop();

                        //Logging.DumpTotalScores(candidateProteins);
                        if (iterations == 0)
                        {
                            // Results Download Part 2 of 3  BELOW //

                            var tempResultsDownloadToBeWriteList = new ResultsDownloadToBeWrite(System.IO.Path.GetFileNameWithoutExtension(parameters.PeakListFileName[fileNumber]), FinalCandidateProteinListforFinalScoring);
                            ResultsDownloadToBeWriteList.Add(tempResultsDownloadToBeWriteList);

                            if (numberOfPeaklistFiles > 1) // For Batch Mode
                            {
                                //////~~~~~~~~~~~~~~~~~~~~~~~~~~    ON HOLD NOT FECTHING DATA FOR WRITING EXCEL FILE...
                                ////var tempBatchModeFileProteins = new ResultsDownloadToBeWrite(System.IO.Path.GetFileNameWithoutExtension(parameters.PeakListFileName[fileNumber]), FinalCandidateProteinListforFinalScoring[0], pipeLineTimer.Elapsed.ToString());
                                //////Fetching First Protein (having ProteinRank = 1) for making Batch Mode File
                                ////BatchModeFileProteins.Add(tempBatchModeFileProteins);
                                //var _NoOfMatchedFragments = new NoOfMatchedFragments();
                                //var NoOfMatchedFragments = _NoOfMatchedFragments.NoOfMatchedFragmentsCount(0, FinalCandidateProteinListforFinalScoring[0].LeftMatchedIndex, FinalCandidateProteinListforFinalScoring[0].RightMatchedIndex);

                                var _NoOfPtmModifications = new NoOfPtmModifications();
                                var NoOfPtmModifications = _NoOfPtmModifications.NoOfPtmModificationsCount(0, FinalCandidateProteinListforFinalScoring[0].PtmParticulars);  // NoOfPtmModifications is Initialized from 0

                                var tempDataForBatchFileAndFdr = new FalseDiscoveryRateDto(System.IO.Path.GetFileName(parameters.PeakListFileName[fileNumber]), FinalCandidateProteinListforFinalScoring[0].Header,
                        FinalCandidateProteinListforFinalScoring[0].TerminalModification, FinalCandidateProteinListforFinalScoring[0].Sequence,
                        FinalCandidateProteinListforFinalScoring[0].Truncation, FinalCandidateProteinListforFinalScoring[0].TruncationIndex,
                        FinalCandidateProteinListforFinalScoring[0].Score, FinalCandidateProteinListforFinalScoring[0].Mw, NoOfPtmModifications, FinalCandidateProteinListforFinalScoring[0].MatchCounter, pipeLineTimer.Elapsed.ToString(),
                        FinalCandidateProteinListforFinalScoring[0].Evalue);
                                DataForBatchFileAndFdr.Add(tempDataForBatchFileAndFdr);

                            }

                            // Results Download Part 2 of 3  ABOVE //
                            pipeLineTimer.Stop();
                            executionTimes.TotalTime = pipeLineTimer.Elapsed.ToString();
                            executionTimes.JobSubmission = parameters.JobSubmission;

                            Stopwatch time = new Stopwatch();
                            time.Start();
                            StoreSearchResults(parameters, FinalCandidateProteinListforFinalScoring, executionTimes, fileNumber);
                            //peakData2DList = peakData2DList.OrderByDescending(x => x.Mass).ToList();
                            StorePeakListData(parameters.FileUniqueIdArray[fileNumber], peakData2DList, parameters.JobSubmission);
                            time.Stop();

                            int test = 1;
                            
                        }
                        else
                        {
                            pipeLineTimer.Stop();
                            //var _NoOfMatchedFragments = new NoOfMatchedFragments();
                            //var NoOfMatchedFragments = _NoOfMatchedFragments.NoOfMatchedFragmentsCount(0, FinalCandidateProteinListforFinalScoring[0].LeftMatchedIndex, FinalCandidateProteinListforFinalScoring[0].RightMatchedIndex);

                            var _NoOfPtmModifications = new NoOfPtmModifications();
                            var NoOfPtmModifications = _NoOfPtmModifications.NoOfPtmModificationsCount(0, FinalCandidateProteinListforFinalScoring[0].PtmParticulars);  // NoOfPtmModifications is Initialized from 0

                            var tempDecoyDataForBatchFileAndFdr = new FalseDiscoveryRateDto(System.IO.Path.GetFileName(parameters.PeakListFileName[fileNumber]), FinalCandidateProteinListforFinalScoring[0].Header,
                    FinalCandidateProteinListforFinalScoring[0].TerminalModification, FinalCandidateProteinListforFinalScoring[0].Sequence,
                    FinalCandidateProteinListforFinalScoring[0].Truncation, FinalCandidateProteinListforFinalScoring[0].TruncationIndex,
                    FinalCandidateProteinListforFinalScoring[0].Score, FinalCandidateProteinListforFinalScoring[0].Mw, NoOfPtmModifications, FinalCandidateProteinListforFinalScoring[0].MatchCounter, pipeLineTimer.Elapsed.ToString(),
                    FinalCandidateProteinListforFinalScoring[0].Evalue);
                            DecoyDataForBatchFileAndFdr.Add(tempDecoyDataForBatchFileAndFdr);

                            //DecoyTopFinalCandidateProteinList.Add(FinalCandidateProteinListforFinalScoring[0]);
                        }
                    }
                }
                catch (Exception r)
                {
                    if (parameters.EmailId != "")
                    {
                        EmailMsg = "Exception";
                    }

                    string k = r.Message;
                    System.Diagnostics.Debug.WriteLine(r.Message);
                    ProgressStatus = -1;
                }


                //Logging.DumpTotalTime(executionTimes);
                //Logging.ExitPeakFileDirectory();
            }

            try
            {
                // Results Download Part 3 of 2  BELOW //
                ResultsDownloadFileNames.AddRange(_WriteResultsFile.WriteIndividualResultsFile(parameters.Title, ResultsDownloadToBeWriteList, Path));   // For Individual Files
                ResultsDownloadFileNames.Add(_WriteResultsFile.WriteParametersInTxtFile(parameters, Path)); // For Parameters File

                var ResultsOfFDR = new List<FalseDiscoveryRateDto>(DataForBatchFileAndFdr.Count);
                if (parameters.FDRCutOff != "N/A")  // Will Work for FDR - Decoy Side  //Updated 20210209
                {
                    DataForBatchFileAndFdr = DataForBatchFileAndFdr.OrderByDescending(x => x.Score).ToList();
                    DecoyDataForBatchFileAndFdr = DecoyDataForBatchFileAndFdr.OrderByDescending(x => x.Score).ToList();

                    double OutDouble;
                    FalseDiscoveryRate _FalseDiscoveryRate = new FalseDiscoveryRate();
                    double.TryParse(parameters.FDRCutOff, out OutDouble);
                    var tempResultsOfFDR = _FalseDiscoveryRate.FDR(OutDouble, DataForBatchFileAndFdr, DecoyDataForBatchFileAndFdr);
                    ResultsOfFDR.AddRange(tempResultsOfFDR);
                    ResultsDownloadFileNames.Add(_WriteResultsFile.WriteBatchResultsFile(parameters.Title, parameters.FDRCutOff, ResultsOfFDR, Path));
                }
                else if (numberOfPeaklistFiles > 1)
                {
                    ResultsDownloadFileNames.Add(_WriteResultsFile.WriteBatchResultsFile(parameters.Title, parameters.FDRCutOff, DataForBatchFileAndFdr, Path));
                }



                var ZipFileWithQueryId = _WriteResultsFile.ZippingOutputFiles(parameters.Title, parameters.Queryid, ResultsDownloadFileNames, Path);
                string ZipFileName = parameters.Title + ".zip";   //This Name will show to the User.
                _dataLayer.StoreZipResultsForDownload(parameters.Queryid, ZipFileName, ZipFileWithQueryId, parameters.JobSubmission);
                // Results Download Part 3 of 2  ABOVE //
                EmailMsg = "";
                ProgressStatus = 100;

            }
            catch (Exception e)
            {
                EmailMsg = "Exception";
                ProgressStatus = -1;
            }

            if (parameters.EmailId != "")
            {
                if (numberOfPeaklistFiles >= 1 && EmailMsg != "ProteinListEmpty" && EmailMsg != "Exception") // Email Sent: Single or Batch Mode Results are Ready
                {
                    EmailMsg = "ResultsReady";
                    //Sending_Email(parameters, EmailMsg);
                }
                else if (EmailMsg == "ProteinListEmpty" || EmailMsg == "Exception") // Email Sent: Search couldn't completed
                {
                    //Sending_Email(parameters, EmailMsg);
                }
            }

            _dataLayer.Set_Progress(parameters.Queryid, ProgressStatus);
        }

        private void ScoringByMolecularWeight(SearchParametersDto parameters, double IntactProteinMass, List<ProteinDto> CandidateProteinsList)
        {
            if (parameters.MwSweight != 0)
            {
                double mass, error, mw_score;
                for (var i = 0; i < CandidateProteinsList.Count; ++i)
                {
                    mass = CandidateProteinsList[i].Mw;
                    error = Math.Abs(mass - IntactProteinMass);
                    if (error == 0)
                        mw_score = 1;
                    else
                        mw_score = 1 / (Math.Pow(2, error));
                    CandidateProteinsList[i].MwScore = mw_score;
                }
            }
        }

        private List<ProteinDto> Truncation_Engine(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncated, List<PstTagList> PstTags, List<newMsPeaksDto> peakData2DList, ExecutionTimeDto executionTimes)
        {
            Stopwatch modulerTimer = Stopwatch.StartNew();

            var CandidateProteinListUnModified = new List<ProteinDto>();
            var CandidateProteinListTrucnatedwithInsilicoScores = new List<ProteinDto>();
            var CandidateProteinList = new List<ProteinDto>();

            Stopwatch OnlyTruncation = new Stopwatch();    // DELME Execution Time Working
            Stopwatch OnlyPreTruncation = Stopwatch.StartNew();    // DELME Execution Time Working
            Stopwatch OnlyTruncationLeft = Stopwatch.StartNew();    // DELME Execution Time Working
            Stopwatch OnlyTruncationRight = Stopwatch.StartNew();    // DELME Execution Time Working

            if (parameters.Truncation == "True" && CandidateProteinListTruncated.Count != 0)   /// #TESTRUN : Additionally, added "" && CandidateProteinListTruncated.Count !=0 ""
            {

                OnlyTruncation.Start();     // DELME Execution Time Working

                /* (Above) Updated 20201207  -- For Time Efficiancy  */
                TerminalModificationsList _TerminalModifications = new TerminalModificationsList();
                var IndividualModifications = _TerminalModifications.TerminalModifications(parameters.TerminalModification);

                int ListCount = CandidateProteinListTruncated.Count;
                int Capacity = ListCount * IndividualModifications.Count;
                double MwTolerance = parameters.MwTolerance;
                string PtmAllow = parameters.PtmAllow;
                /* (Above) Updated 20201207  -- For Time Efficiancy  */


                var CandidateProteinListTruncatedLeft = new List<ProteinDto>(ListCount);
                var CandidateProteinListTruncatedRight = new List<ProteinDto>(Capacity);

                var CandidateListTruncationLeftProcessed = new List<ProteinDto>();  // Processed by TruncationLeft
                var RemainingProteinsLeft = new List<ProteinDto>();  // Processed by TruncationLeft

                var CandidateListTruncationRightProcessed = new List<ProteinDto>();  // Processed by TruncationRight
                var RemainingProteinsRight = new List<ProteinDto>();  // Processed by TruncationRight


                var CandidateProteinsListModifiedLeft = new List<ProteinDto>();
                var CandidateProteinsListModifiedRight = new List<ProteinDto>();

                OnlyPreTruncation.Start();    // DELME Execution Time Working

                _Truncation.PreTruncation(MwTolerance, IndividualModifications, CandidateProteinListTruncated, CandidateProteinListTruncatedLeft, CandidateProteinListTruncatedRight, peakData2DList);

                OnlyPreTruncation.Stop();           // DELME Execution Time Working

                OnlyTruncationLeft.Start(); // DELME Execution Time Working
                _Truncation.TruncationLeft(PtmAllow, CandidateProteinListTruncatedLeft, CandidateListTruncationLeftProcessed, RemainingProteinsLeft, peakData2DList);  //ITS HEALTHY 
                OnlyTruncationLeft.Stop();           // DELME Execution Time Working

                OnlyTruncationRight.Start();    // DELME Execution Time Working
                _Truncation.TruncationRight(PtmAllow, CandidateProteinListTruncatedRight, CandidateListTruncationRightProcessed, RemainingProteinsRight, peakData2DList);
                OnlyTruncationRight.Stop();           // DELME Execution Time Working

                CandidateProteinListUnModified.AddRange(CandidateListTruncationLeftProcessed);
                CandidateProteinListUnModified.AddRange(CandidateListTruncationRightProcessed);

                CandidateProteinListUnModified = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(CandidateProteinListUnModified, parameters.InsilicoFragType, parameters.HandleIons);

                OnlyTruncation.Stop();          // DELME Execution Time Working

                if (parameters.PtmAllow == "True")
                {
                    Stopwatch OnlyTruncationPtm = Stopwatch.StartNew();    // DELME Execution Time Working

                    RemainingProteinsLeft = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(RemainingProteinsLeft, parameters.InsilicoFragType, parameters.HandleIons);
                    RemainingProteinsRight = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(RemainingProteinsRight, parameters.InsilicoFragType, parameters.HandleIons);

                    var BlindPTMExtractionInfo = _BlindPostTranslationalModificationModule.BlindPTMExtraction(peakData2DList, parameters);  // #MAKE IT COMMON...
                    var RemainingProteinsLeftModified = _BlindPostTranslationalModificationModule.BlindPTMGeneral(RemainingProteinsLeft, peakData2DList, 1, BlindPTMExtractionInfo, parameters, "BlindPTM_Truncation_Left"); //WHy UserHopThreshold = 1???
                    var RemainingProteinsRightModified = _BlindPostTranslationalModificationModule.BlindPTMGeneral(RemainingProteinsRight, peakData2DList, 1, BlindPTMExtractionInfo, parameters, "BlindPTM"); //WHy UserHopThreshold = 1???


                    CandidateProteinsListModifiedLeft = _BlindPostTranslationalModificationModule.PTMTruncation_Modification(RemainingProteinsLeftModified, peakData2DList, parameters, "Truncation_Left_Modification");
                    CandidateProteinsListModifiedRight = _BlindPostTranslationalModificationModule.PTMTruncation_Modification(RemainingProteinsRightModified, peakData2DList, parameters, "Truncation_Right_Modification");

                    OnlyTruncationPtm.Stop();          // DELME Execution Time Working

                }
                else
                {
                    CandidateProteinsListModifiedLeft = new List<ProteinDto>();
                    CandidateProteinsListModifiedRight = new List<ProteinDto>();
                }

                CandidateProteinList.AddRange(CandidateProteinListUnModified);
                CandidateProteinList.AddRange(CandidateProteinsListModifiedLeft);
                CandidateProteinList.AddRange(CandidateProteinsListModifiedRight);

                var FilteredTruncatedList = _Truncation.FilterTruncatedProteins(parameters, CandidateProteinList, PstTags);
                CandidateProteinListTrucnatedwithInsilicoScores = _insilicoFilter.ComputeInsilicoScore(FilteredTruncatedList, peakData2DList, parameters.PeptideTolerance, parameters.PeptideToleranceUnit);

            }
            modulerTimer.Stop();
            executionTimes.TruncationEngineTime = modulerTimer.Elapsed.ToString();
            return CandidateProteinListTrucnatedwithInsilicoScores;
        }


        //Mass Tunner
        private void ExecuteMassTunerModule(SearchParametersDto parameters, MsPeaksDto peakData, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();

            if (parameters.Autotune == "True")
            {
                _wholeProteinMassTuner.TuneWholeProteinMass(peakData, parameters);
            }
            moduleTimer.Stop();
            executionTimes.TunerTime = moduleTimer.Elapsed.ToString();
        }



        //PST: Peptide Sequence Tags
        private List<PstTagList> ExecuteDenovoModule(SearchParametersDto parameters, MsPeaksDto massSpectrometryData, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();
            var pstTags = new List<PstTagList>();
            if (parameters.DenovoAllow == "True")
            {

                pstTags = _pstGenerator.GeneratePeptideSequenceTags(parameters, massSpectrometryData);
                //Logging.DumpPstTags(pstTags);

            }
            moduleTimer.Stop();
            executionTimes.PstTime = moduleTimer.Elapsed.ToString();
            return pstTags;
        }
        //GetCandidateProtein(parameters, massSpectrometryData, PstTags, executionTimes, 

        private CandidateProteinListsDto GetCandidateProtein(SearchParametersDto parameters, MsPeaksDto peakData, List<PstTagList> PstTags, List<ProteinDto> SQLDataBaseProteins, ExecutionTimeDto executionTimes)
        {

            Stopwatch moduleTimer = Stopwatch.StartNew();

            var CandidateProteinListsInfo = _proteinRepository.ExtractProteins(peakData.WholeProteinMolecularWeight, parameters, PstTags, SQLDataBaseProteins);

            moduleTimer.Stop();


            //TimeSpan time = TimeSpan.Parse(executionTimes.InsilicoTime);
            //var totaltime = moduleTimer + time;
            executionTimes.MwFilterTime = moduleTimer.Elapsed.ToString();

            return CandidateProteinListsInfo;
        }

        private List<ProteinDto> UpdateGetCandidateProtein(SearchParametersDto parameters, List<PstTagList> PstTags, List<ProteinDto> candidateProteins, double Experimentalmz)
        {
            List<ProteinDto> UpdatedCandidatedProteinList = new List<ProteinDto>();
            /* WithoutPTM_ParseDatabase.m */
            if (parameters.DenovoAllow == "True")
            {
                // Here just adding PST scores of each protein but those proteins have zero PST score are not removed from candidate protein list but will do in this (_TerminalModifications.EachProteinTerminalModifications(parameters, candidateProteins))
                _pstFilter.ScoreProteinsByPst(PstTags, candidateProteins);
                //Irrespective to WithoutPTM_ParseDatabase.m  there is no need to assign PSTScore = 0 if DenovoAllow is false because we already initialize PSTScore = 0.0
            }

            candidateProteins = _TerminalModifications.EachProteinTerminalModifications(parameters, candidateProteins);
            UpdatedCandidatedProteinList.AddRange(candidateProteins);

            /* WithoutPTM_ParseDatabase.m */

            /* Updated_ParseDatabase.m */

            if (parameters.CysteineChemicalModification != "None" && parameters.MethionineChemicalModification != "None" && parameters.FixedModifications.Count > 0 && parameters.VariableModifications.Count > 0)
            {
                // HERE IT WILL BE PTMs_Generator_Insilico_Generator
                for (int i = 0; i < candidateProteins.Count; i++)
                {
                    List<ProteinDto> ModifiedPtmProtein = _postTranslationalModificationModule.PTMs_Generator_Insilico_Generator(Experimentalmz, candidateProteins[i], parameters);
                    UpdatedCandidatedProteinList.AddRange(ModifiedPtmProtein);
                }
            }

            return UpdatedCandidatedProteinList;
        }

        //ProteoformFinalAlgorithmsWeightage
        private static List<ProteinDto> ExecuteProteoformScoringModule(SearchParametersDto parameters, List<ProteinDto> candidateProteins)
        {
            if (parameters.PstSweight != 0)
            {
                double MaxPstScore = candidateProteins.Max(x => x.PstScore);

                if (MaxPstScore > 1)
                {
                    for (int iter = 0; iter < candidateProteins.Count; iter++)
                    {
                        candidateProteins[iter].PstScore = candidateProteins[iter].PstScore / MaxPstScore;
                    }
                }
            }

            for (int iter = 0; iter < candidateProteins.Count; iter++)
            {
                //candidateProteins[iter].Score = candidateProteins[iter];
                AggregatedScoreByScoringComponents(parameters, candidateProteins[iter]);
            }

            return candidateProteins;
        }

        private static void AggregatedScoreByScoringComponents(SearchParametersDto parameters, ProteinDto Protein)
        {
            Protein.Score = ((parameters.MwSweight * Protein.MwScore) + (parameters.PstSweight * Protein.PstScore) +
                (parameters.InsilicoSweight * Protein.InsilicoScore)) / (parameters.MwSweight + parameters.PstSweight + parameters.InsilicoSweight); //3.0;

            //Score = (pstSweight * PstScore + insilicoSweight * InsilicoScore + mwSweight * MwScore) / (mwSweight + pstSweight + insilicoSweight);
        }


        private List<ProteinDto> RankCandidateProteinsList(List<ProteinDto> candidateProteins)
        {
            candidateProteins = candidateProteins.OrderByDescending(x => x.Score).ToList(); // CandidateProteinList in descending order: According to their Score
            for (int iter = 0; iter < candidateProteins.Count; iter++) // Ranking the Candidate Proteins according to their scores
            {
                candidateProteins[iter].ProteinRank = iter + 1;
            }

            return candidateProteins;
        }
        //SPECTRAL COMPARISON ALGORITHM: 
        private List<ProteinDto> ExecuteSpectralComparisonModule(SearchParametersDto parameters, List<ProteinDto> candidateProteins, List<newMsPeaksDto> peakData2DList, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();
            //if (parameters.InsilicoSweight != 0)

            var CandidateProteinswithInsilicoScores = new List<ProteinDto>();

            //ITS HEALTHY!!! 20200203
            CandidateProteinswithInsilicoScores = _insilicoFilter.ComputeInsilicoScore(candidateProteins, peakData2DList, parameters.PeptideTolerance, parameters.PeptideToleranceUnit);


            //if (parameters.PtmAllow == 0)  // ITS HEALTHY
            //    _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(candidateProteins, parameters.InsilicoFragType, parameters.HandleIons);

            //"Module 7 of 9:  Insilico Filteration.";                                  //FARHAN
            //if (parameters.PtmAllow == 0)  // ITS HEALTHY
            //{
            //    //_insilicoFilter.ComputeInsilicoScore(candidateProteins, massSpectrometryData.Mass, parameters.HopThreshhold); //Commented


            //    _insilicoFilter.ComputeInsilicoScore(candidateProteins, peakData2DList, parameters.PeptideTolerance, parameters.PeptideToleranceUnit, CandidateProteinswithInsilicoScores);
            //    //_insilicoFilter.ComputeInsilicoScore(candidateProteins, massSpectrometryData.Mass, parameters.PeptideTolerance, parameters.PeptideToleranceUnit);
            //}
            moduleTimer.Stop();
            executionTimes.InsilicoTime = moduleTimer.Elapsed.ToString();
            return CandidateProteinswithInsilicoScores;
        }

        //Post Translational Modifcations (PTM)
        private List<ProteinDto> ExecutePostTranslationalModificationsModule(SearchParametersDto parameters, List<ProteinDto> candidateProteins, List<newMsPeaksDto> peakData2DList, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();
            var CandidateProteinListBlindPtmModified = new List<ProteinDto>();
            if (parameters.PtmAllow == "True")
            {
                var BlindPTMExtractionInfo = _BlindPostTranslationalModificationModule.BlindPTMExtraction(peakData2DList, parameters);
                CandidateProteinListBlindPtmModified = _BlindPostTranslationalModificationModule.BlindPTMGeneral(candidateProteins, peakData2DList, 1, BlindPTMExtractionInfo, parameters, "BlindPTM"); //WHy UserHopThreshold = 1??? 
            }
            else
            {
                CandidateProteinListBlindPtmModified = new List<ProteinDto>();
            }

            moduleTimer.Stop();
            executionTimes.PtmTime = moduleTimer.Elapsed.ToString();
            return CandidateProteinListBlindPtmModified;
        }

        private void StoreSearchResults(SearchParametersDto parameters, List<ProteinDto> candidateProteins, ExecutionTimeDto executionTimes, int fileNumber)
        {

            //if (candidateProteins.Count > Constants.NumberOfResultsToStore)                        //ITS HEALTHY.....!!!
            //    candidateProteins = candidateProteins.Take(Constants.NumberOfResultsToStore).ToList<ProteinDto>();

            if (parameters.NumberOfOutputs != "100+")
            {
                int NumberOfOutputs = Convert.ToInt16(parameters.NumberOfOutputs);
                if (candidateProteins.Count > NumberOfOutputs)
                {
                    candidateProteins = candidateProteins.Take(NumberOfOutputs).ToList<ProteinDto>();
                }
            }
            
            var final = new SearchResultsDto(parameters.Queryid, candidateProteins, executionTimes);
            _dataLayer.StoreResults(final, parameters.PeakListFileName[fileNumber], parameters.FileUniqueIdArray[fileNumber], fileNumber, parameters.JobSubmission);
        }

        private void StorePeakListData(string FileUniqueId, List<newMsPeaksDto> peakData2DList, DateTime JobSubmission)
        {
            var peakDataMasses = new List<double>();
            var peakDataIntensities = new List<double>();

            for (int i = 0; i < peakData2DList.Count; i++)
            {
                peakDataMasses.Add(peakData2DList[i].Mass);  // Enhancement MAKE a separate new table
                peakDataIntensities.Add(peakData2DList[i].Intensity);  // Enhancement MAKE a separate new table
            }

            string peakDataMassesString = string.Join(",", peakDataMasses);
            string peakDataIntensitiesString = string.Join(",", peakDataIntensities);

            _dataLayer.StorePeakList(FileUniqueId, peakDataMassesString, peakDataIntensitiesString, JobSubmission);
        }


        //Peak List: Extracting from data file 
        private MsPeaksDto PeakListFileReaderModuleModule(SearchParametersDto parameters, int fileNumber, ExecutionTimeDto executionTimes)
        {
            var moduleTimer = new Stopwatch();
            moduleTimer.Start();
            var peakData = _peakListFileReader.PeakListReader(parameters, fileNumber);

            double maxIntensity = peakData.Intensity.Max();

            //DEL ME////Discuss it with Sir. Because during first time reading file I am protonating (FOR MASSES) & normalizing intensities .. #DISCUSSION1(This discussion is Necessary)
            //DEL ME//Yehi data hr jagha use hona ha to ... humma hr jgha m/z or nomralized intensities he chahya...??? ---#Compare kr lo with SPECTRUM---
            ////*************m/z & normalizing for MS2*************////
            ////After reading peaks(Masses & Intensities) from data file. We converted "*"MS2"*" Masses(monoisotopic masses) into m/z.
            //Formula of m/z: m/z = (M+z)/z  where M is the monoisotopic mass & z is the mass of proton.
            //// Maximum Intensity selected from the peak data file & Intensities of "*"MS2"*" are normalized by dividing the selected maximum intensity
            for (int peakDataindex = 1; peakDataindex <= peakData.Mass.Count - 1; peakDataindex++)//Loop will run only for MS2 so that why its starting from "1".
            {
                if (parameters.MassMode == "M(Neutral)")
                    peakData.Mass[peakDataindex] = peakData.Mass[peakDataindex] + 1.00727647; //monoisotopic to m/z value
                peakData.Intensity[peakDataindex] = peakData.Intensity[peakDataindex] / maxIntensity; //Normalized by dividing the selected maximum intensity 
            }

            moduleTimer.Stop();
            executionTimes.FileReadingTime = moduleTimer.Elapsed.ToString();
            return peakData;
        }

        public List<newMsPeaksDto> peakDataList(MsPeaksDto peakData) // #En Temporary Until Adjustments 
        {
            newMsPeaksDto tempData;
            List<newMsPeaksDto> peakList = new List<newMsPeaksDto>();
            for (int index = 0; index < peakData.Mass.Count; index++)
            {
                tempData = new newMsPeaksDto(peakData.Mass[index], peakData.Intensity[index]);
                peakList.Add(tempData);
            }
            return peakList;
        }

        private MassTunerAndPstCombinedStruct[] ExecuteMassTunerModuleGpu(MsPeaksDto massSpectrometryData, SearchParametersDto parameters, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();
            double[] PeakListMasses = new double[massSpectrometryData.Mass.Count];
            double[] PeakListIntensities = new double[massSpectrometryData.Intensity.Count];
            double[] PeakListIntensitiesForSpectralComp = new double[massSpectrometryData.Intensity.Count];
            for (int i = 0; i < massSpectrometryData.Mass.Count; i++)
            {
                PeakListMasses[i] = massSpectrometryData.Mass[i];
                PeakListIntensities[i] = massSpectrometryData.Intensity[i];
                //if (massSpectrometryData.Intensity[i] < 0.000092)
                //    PeakListIntensitiesForSpectralComp[i] = 0.001;
                //else
                //    PeakListIntensitiesForSpectralComp[i] = 1;
            }
            int PeakListLength = massSpectrometryData.Mass.Count;
            int AutoTune, DenovoAllow;
            if (parameters.Autotune == "True")
                AutoTune = 1;
            else
                AutoTune = 0;
            if (parameters.DenovoAllow == "True")
                DenovoAllow = 1;
            else
                DenovoAllow = 0;

            ParametersToCpp Parameters_To_Cpp = new ParametersToCpp(parameters.MwTolerance, parameters.NeutralLoss, parameters.SliderValue, parameters.HopThreshhold, AutoTune, DenovoAllow, parameters.MinimumPstLength, parameters.MaximumPstLength, parameters.PeptideToleranceUnit, parameters.PeptideTolerance);

            //massSpectrometryData.WholeProteinMolecularWeight = NativeCudaCalls.WholeProteinMassTunerAndPstGpu(PeakListMasses, PeakListIntensities, PeakListLength, Parameters_To_Cpp);

            IntPtr[] pResultsFromGpu = new IntPtr[5000];

            for (int i = 0; i < 5000; i++)
                pResultsFromGpu[i] = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MassTunerAndPstCombinedStruct)));

            int SizeOfDataFromGpu = NativeCudaCalls.WholeProteinMassTunerAndPstGpu(PeakListMasses, PeakListIntensities, PeakListLength, Parameters_To_Cpp, pResultsFromGpu);

            MassTunerAndPstCombinedStruct[] returnArray = new MassTunerAndPstCombinedStruct[SizeOfDataFromGpu];
            for (int i = 0; i < SizeOfDataFromGpu; i++)
            {
                returnArray[i] = (MassTunerAndPstCombinedStruct)Marshal.PtrToStructure(pResultsFromGpu[i], typeof(MassTunerAndPstCombinedStruct));
            }

            moduleTimer.Stop();
            executionTimes.TunerTime = moduleTimer.Elapsed.ToString();

            return returnArray;
        }

        private List<PstTagList> ConvertStructToPstTagList(MassTunerAndPstCombinedStruct[] MassTunerAndPstData)
        {

            //  PstTagList(int cPstTagLength, string cPstTags, double cPstErrorScore, double cPstFrequency)
            int Count = MassTunerAndPstData.Length;

            var PstTags = new List<PstTagList>(Count);
            if (MassTunerAndPstData[0].PstTagLength > 0)  //If there is not tag find into the input file.
            {
                for (int index = 0; index < Count; index++)
                {
                    var Data = MassTunerAndPstData[index];
                    string Tag = "";
                    for (int iter = 0; iter < Data.PstTagLength; iter++)
                    {
                        Tag = Tag + Data.PstTags[iter];
                    }
                    PstTags.Add(new PstTagList(Data.PstTagLength, Tag, Data.PstErrorScore, Data.PstFrequency));
                }
            }
            

            return PstTags;
        }
    }

    // --- GPU Code Below ---   Updated: 20210223
    public static class NativeCudaCalls
    {
        private const string DllFilePath = @"E:\01_PERCEPTRON\GitHub\Code\PerceptronLocalService\x64\Debug\PerceptronCuda.dll";
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void MainInitializer();
        public static void InitializingGpu()
        {
            MainInitializer();
        }
        //private extern static double wholeproteinmasstunerandpst(double[] PeakListMasses, double[] PeakListIntensities, int PeakListLength, [In, Out] ParametersToCpp Parameters);
        //public static double WholeProteinMassTunerAndPstGpu(double[] PeakListMasses, double[] PeakListIntensities, int PeakListLength, [In, Out] ParametersToCpp Parameters)
        //{
        //    return wholeproteinmasstunerandpst(PeakListMasses, PeakListIntensities, PeakListLength, Parameters);
        //}
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int wholeproteinmasstunerandpst(double[] PeakListMasses, double[] PeakListIntensities, int PeakListLength, [In, Out] ParametersToCpp Parameters, IntPtr[] pResultsFromGpu);
        public static int WholeProteinMassTunerAndPstGpu(double[] PeakListMasses, double[] PeakListIntensities, int PeakListLength, [In, Out] ParametersToCpp Parameters, IntPtr[] pResultsFromGpu)
        {
            return wholeproteinmasstunerandpst(PeakListMasses, PeakListIntensities, PeakListLength, Parameters, pResultsFromGpu);
        }

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void insilicospectralcomparisongpu([In, Out] ParametersToCpp Parameters, IntPtr[] candidateProteins, double[] PeakListMasses, double[] PeakListIntensitiesForSpectralComp, int PeakListCount, int candidateProteinsCount);
        public static void InsilicoSpectralComparisonGpu([In, Out] ParametersToCpp Parameters, IntPtr[] candidateProteins, double[] PeakListMasses, double[] PeakListIntensitiesForSpectralComp, int PeakListCount, int candidateProteinsCount)
        {
            insilicospectralcomparisongpu(Parameters, candidateProteins, PeakListMasses, PeakListIntensitiesForSpectralComp, PeakListCount, candidateProteinsCount);
        }

    }
    // --- GPU Code Above ---   Updated: 20210223
}

