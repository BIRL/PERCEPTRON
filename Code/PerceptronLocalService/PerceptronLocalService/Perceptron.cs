using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using PerceptronLocalService.Engine;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Models;
using PerceptronLocalService.Repository;
//using PerceptronLocalService.Testing;
using PerceptronLocalService.Utility;
/////For getting GPU version/////
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

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


        public void Start()
        {
            while (true)
            {

                var pendingJobs = _dataLayer.ServerStatus();
                var pendingJobsParameters = pendingJobs.Select(element => _dataLayer.GetParameters(element.QueryId));
                foreach (var searchParameters in pendingJobsParameters)
                {
                    //Send_Results_Link(searchParameters);
                    //_dataLayer.Set_Progress(searchParameters.Queryid, 100);
                    PerformSearch(searchParameters);
                }
                //System.Threading.Thread.Sleep(10000);
            }

        }

        public void Stop()
        {
            //ignored
        }


        //public void Send_Results_Link(SearchParametersDto p)
        //{
        //    var creationtime = _dataLayer.GetCreationTime(p.Queryid);
        //    var emailaddress = p.UserId;

        //    using (var mm = new MailMessage("perceptron@lums.edu.pk", emailaddress))
        //    {
        //        mm.Subject = "Perceptron: Protein Search Results";
        //        var body = "Dear User,";
        //        body += "<br /><br /> The results for protein search query submitted at " + creationtime + " with job title \""
        //                + "<span style='font-weight:bold;font-size:15px;'>" + p.Title + "</span>" + "\" have been computed. To see the results please click on following link(s).<br />";
        //        body += "&nbsp;&nbsp;&nbsp;&nbsp;<a href=\'http://perceptron.lums.edu.pk/index.html#/scans/" + p.Queryid + " \'>Click here to see your Results of Protein Identification.</a>";
        //        body += "<br /><br />Thank You for using Perceptron.";
        //        body += "<br />Best Regards,";
        //        body += "<br/><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family:calibri;font-weight:bold;font-size:12.5px;color:rgb(90, 60, 140);'>Team Proteomics,</span>";
        //        body += "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family:calibri;font-size:12.5px;color:rgb(90, 60, 140);'>Biomedical Informatics Research Laboratory (BIRL),<br/>" +
        //            "</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Department of Biology, SBA School of Science and Engineering," +
        //            "</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Lahore University of Management Sciences (LUMS), Lahore, Pakistan>" +
        //            "</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Voice:<a href=\'http://biolabs.lums.edu.pk/BIRL/\'> +92 321 4255171</a> | Fax: +92 42 3560 8317" +
        //            "</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Email: <a href=\'http://perceptron@lums.edu.pk/\'>perceptron@lums.edu.pk</a>, <a href=\'http://chaudhary.safee.ullah@gmail.com/\'>chaudhary.safee.ullah@gmail.com</a>" +
        //            "</br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Web:<a href=\'http://biolabs.lums.edu.pk/BIRL/\'> biolabs.lums.edu.pk/BIRL</a></span>";
        //        mm.Body = body;
        //        mm.IsBodyHtml = true;

        //        var networkCred = new NetworkCredential("perceptron@lums.edu.pk", "LUMSProT@comBio");
        //        var smtp = new SmtpClient
        //        {
        //            Host = "smtp.office365.com",
        //            EnableSsl = true,
        //            UseDefaultCredentials = true,
        //            Credentials = networkCred,
        //            Port = 587
        //        };
        //        try
        //        {
        //            smtp.Send(mm);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.WriteLine(e.Message);
        //        }
        //    }
        //}

        //"<a href=\'" + BaseUrl + "/index.html#/getting >Getting Started</a>"
        public static void Sending_Email(SearchParametersDto p, int EmailMsg)
        {
            var emailaddress = p.EmailId;
            using (var mm = new MailMessage("dummyemail@lums.edu.pk", emailaddress))
            {
                string BaseUrl = "https://perceptron.lums.edu.pk/";

                if (EmailMsg == 1)
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
                else if (EmailMsg == -1) // Email Msg for Something Wrong With Entered Query
                {
                    mm.Subject = "PERCEPTRON: Protein Search Results";
                    var body = "Dear User,";
                    body += "<br/><br/> Search couldn't complete for protein search query submitted at " + DateTime.Now.ToString() + " with job title \"" +
                            p.Title + "\" Please check your search parameters and data file.";
                    //body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                    body += "</br> If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If problem still persists, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";

                    body += "</br></br>Thank You for using Perceptron.";
                    body += "</br><b>The PERCEPTRON Team</b>";
                    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                    mm.Body = body;
                }
                else if (EmailMsg == -2)
                {
                    mm.Subject = "PERCEPTRON: Invalid Parameters";
                    var body = "Dear User,";
                    body += "<br/><br/> Search couldn't complete for protein search query submitted at " + DateTime.Now.ToString() + " with job title \"" +
                            p.Title + "\" Either MS1 or MS2 are not accurate enough to perform Mass Tuning. Deactivate auto-tune option in main GUI to proceed.";
                    //body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                    body += "</br> If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If problem still persists, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";

                    body += "</br></br>Thank You for using Perceptron.";
                    body += "</br><b>The PERCEPTRON Team</b>";
                    body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                    mm.Body = body;
                }


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


        private void PerformSearch(SearchParametersDto parameters)
        {
            //Logging.CreateDirectory();
            //Logging.DumpParameters(parameters);

            //var counter = 0;
            int EmailMsg = 0;  // EmailMsg == 1 Means All Good & == -1 Means Somthing Wrong & == -2 Means Invalid Parameters (for Mass Tuner)
            var numberOfPeaklistFiles = parameters.PeakListFileName.Length;  //Number of files uploaded by user

            for (var fileNumber = 0; fileNumber < numberOfPeaklistFiles; fileNumber++)
            {
                //Logging.CreatePeakFileDirectory(fileNumber);

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
                    ExecuteMassTunerModule(parameters, massSpectrometryData, executionTimes);
                    if (massSpectrometryData.WholeProteinMolecularWeight == 0)
                    {
                        //massSpectrometryData.WholeProteinMolecularWeight = old;/// UNCOMMENT IT!! If Mass Tuner gives tunned mass = 0 etc. then, use the Peak list file Intact mass 
                        EmailMsg = -2;
                        //Sending_Email(parameters, EmailMsg); // EmailMsg = -2 where -2 is for Invalid Parameters etc. //Send Email we will use Intact Mass 20200805
                        break;

                    }

                    //Logging.DumpMwTunerResult(massSpectrometryData);


                    //Step  - 2nd Algorithm - Peptide Sequence Tags (PSTs)
                    var PstTags = new List<PstTagList>();
                    PstTags = ExecuteDenovoModule(parameters, massSpectrometryData, executionTimes);



                    //Logging.DumpModifiedProteins(candidateProteins);

                    List<newMsPeaksDto> peakData2DList = peakDataList(massSpectrometryData); //Another "Peak data" storing List //Temporary

                    //Step 2 - (1st)Candidate Protein List (Simple) & Candidate Protein List Truncated  --- (In SPECTRUM: Score_Mol_Weight{Adding scores with respect to the Mass difference with Intact Mass})
                    var candidateProteins = new List<ProteinDto>();
                    var CandidateProteinListTruncated = new List<ProteinDto>();


                    //Fetching Candidate Proteins From User Selected DataBase
                    ////// SHOULD USE THIS........""  List<newMsPeaksDto> peakData2DList  ""
                    var CandidateProteinListsInfo = GetCandidateProtein(parameters, massSpectrometryData, PstTags, executionTimes);
                    candidateProteins = CandidateProteinListsInfo.CandidateProteinList;
                    CandidateProteinListTruncated = CandidateProteinListsInfo.CandidateProteinListTruncated;

                    //Score Proteins on Intact Protein Mass  (Adding scores with respect to the Mass difference with Intact Mass)
                    ScoringByMolecularWeight(parameters, massSpectrometryData.WholeProteinMolecularWeight, candidateProteins); // Scoring for Simple Candidate Protein List

                    //Logging.DumpCandidateProteins(candidateProteins);

                    //////UpdatedParse_database.m
                    candidateProteins = UpdateGetCandidateProtein(parameters, PstTags, candidateProteins, peakData2DList[0].Mass);
                    if (candidateProteins.Count == 0 && CandidateProteinListTruncated.Count == 0 && parameters.EmailId != "") // Its Beacuse Data File Having not Enough Info(Number of MS2s are vary few)
                    {
                        EmailMsg = -1;
                        //Sending_Email(parameters, EmailMsg);
                        break;
                    }

                    candidateProteins = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(candidateProteins, parameters.InsilicoFragType, parameters.HandleIons);

                    // Blind PTM Algos (BlindPTMExtraction & BlindPTMGeneral)
                    var CandidateProteinListBlindPtmModified = new List<ProteinDto>();
                    CandidateProteinListBlindPtmModified = ExecutePostTranslationalModificationsModule(parameters, candidateProteins, peakData2DList, executionTimes);
                    candidateProteins.AddRange(CandidateProteinListBlindPtmModified);

                    //Step 4 - ??? Algorithm - Spectral Comparison
                    var CandidateProteinswithInsilicoScores = new List<ProteinDto>();
                    CandidateProteinswithInsilicoScores = ExecuteSpectralComparisonModule(parameters, candidateProteins, peakData2DList, executionTimes);

                    //BlindPTMLocalization: Localizing Unknown mass shift
                    CandidateProteinswithInsilicoScores = _BlindPostTranslationalModificationModule.BlindPTMLocalization(CandidateProteinswithInsilicoScores, peakData2DList[0].Mass, parameters);


                    //Logging.DumpInsilicoScores(candidateProteins);

                    //Executing Truncation 
                    var CandidateProteinListTrucnatedwithInsilicoScores = Truncation_Engine(parameters, CandidateProteinListTruncated, PstTags, peakData2DList);

                    var FinalCandidateProteinListforFinalScoring = new List<ProteinDto>();

                    FinalCandidateProteinListforFinalScoring.AddRange(CandidateProteinswithInsilicoScores);
                    FinalCandidateProteinListforFinalScoring.AddRange(CandidateProteinListTrucnatedwithInsilicoScores);


                    //CandidateProteinswithInsilicoScores = ExecuteProteoformScoringModule(parameters, CandidateProteinswithInsilicoScores); Its Healthy Just List Name Changed
                    FinalCandidateProteinListforFinalScoring = ExecuteProteoformScoringModule(parameters, FinalCandidateProteinListforFinalScoring);

                    //Logging.DumpTotalScores(candidateProteins);

                    pipeLineTimer.Stop();
                    executionTimes.TotalTime = pipeLineTimer.Elapsed.ToString();

                    StoreSearchResults(parameters, FinalCandidateProteinListforFinalScoring, executionTimes, fileNumber);
                    StorePeakListData(parameters.FileUniqueIdArray[fileNumber], peakData2DList);
                }
                catch (Exception r)
                {
                    if (parameters.EmailId != "")
                    {
                        EmailMsg = -1;
                        //Sending_Email(parameters, EmailMsg);
                    }

                    string k = r.Message;
                    System.Diagnostics.Debug.WriteLine(r.Message);
                }

                //Logging.DumpTotalTime(executionTimes);
                //Logging.ExitPeakFileDirectory();
            }

            if (numberOfPeaklistFiles >= 1 && EmailMsg != -2 && EmailMsg != -1 && parameters.EmailId != "")
            {
                EmailMsg = 1;
                Sending_Email(parameters, EmailMsg);
            }
            else if (numberOfPeaklistFiles == 0 && EmailMsg != -2 && EmailMsg != -1 && parameters.EmailId != "")
            {
                EmailMsg = -1;
                Sending_Email(parameters, EmailMsg);
            }

            _dataLayer.Set_Progress(parameters.Queryid, 100);

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

        private List<ProteinDto> Truncation_Engine(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncated, List<PstTagList> PstTags, List<newMsPeaksDto> peakData2DList)
        {
            Stopwatch modulerTimer = Stopwatch.StartNew();

            var CandidateProteinListUnModified = new List<ProteinDto>();
            var CandidateProteinListTrucnatedwithInsilicoScores = new List<ProteinDto>();
            var CandidateProteinList = new List<ProteinDto>();
            if (parameters.Truncation == "True")
            {

                var CandidateProteinListTruncatedLeft = new List<ProteinDto>();
                var CandidateProteinListTruncatedRight = new List<ProteinDto>();

                var CandidateListTruncationLeftProcessed = new List<ProteinDto>();  // Processed by TruncationLeft
                var RemainingProteinsLeft = new List<ProteinDto>();  // Processed by TruncationLeft

                var CandidateListTruncationRightProcessed = new List<ProteinDto>();  // Processed by TruncationRight
                var RemainingProteinsRight = new List<ProteinDto>();  // Processed by TruncationRight


                var CandidateProteinsListModifiedLeft = new List<ProteinDto>();
                var CandidateProteinsListModifiedRight = new List<ProteinDto>();

                _Truncation.PreTruncation(parameters, CandidateProteinListTruncated, CandidateProteinListTruncatedLeft, CandidateProteinListTruncatedRight, peakData2DList);

                _Truncation.TruncationLeft(parameters, CandidateProteinListTruncatedLeft, CandidateListTruncationLeftProcessed, RemainingProteinsLeft, peakData2DList);  //ITS HEALTHY 

                _Truncation.TruncationRight(parameters, CandidateProteinListTruncatedRight, CandidateListTruncationRightProcessed, RemainingProteinsRight, peakData2DList);

                CandidateProteinListUnModified.AddRange(CandidateListTruncationLeftProcessed);
                CandidateProteinListUnModified.AddRange(CandidateListTruncationRightProcessed);

                CandidateProteinListUnModified = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(CandidateProteinListUnModified, parameters.InsilicoFragType, parameters.HandleIons);


                if (parameters.PtmAllow == "True")
                {
                    RemainingProteinsLeft = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(RemainingProteinsLeft, parameters.InsilicoFragType, parameters.HandleIons);
                    RemainingProteinsRight = _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(RemainingProteinsRight, parameters.InsilicoFragType, parameters.HandleIons);

                    var BlindPTMExtractionInfo = _BlindPostTranslationalModificationModule.BlindPTMExtraction(peakData2DList, parameters);  // #MAKE IT COMMON...
                    var RemainingProteinsLeftModified = _BlindPostTranslationalModificationModule.BlindPTMGeneral(RemainingProteinsLeft, peakData2DList, 1, BlindPTMExtractionInfo, parameters, "BlindPTM_Truncation_Left"); //WHy UserHopThreshold = 1???
                    var RemainingProteinsRightModified = _BlindPostTranslationalModificationModule.BlindPTMGeneral(RemainingProteinsRight, peakData2DList, 1, BlindPTMExtractionInfo, parameters, "BlindPTM"); //WHy UserHopThreshold = 1???


                    CandidateProteinsListModifiedLeft = _BlindPostTranslationalModificationModule.PTMTruncation_Modification(RemainingProteinsLeftModified, peakData2DList, parameters, "Truncation_Left_Modification");
                    CandidateProteinsListModifiedRight = _BlindPostTranslationalModificationModule.PTMTruncation_Modification(RemainingProteinsRightModified, peakData2DList, parameters, "Truncation_Right_Modification");
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
            modulerTimer.Stop(); // #FutureWork: Will be added into FE also for Truncation Time
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

        private CandidateProteinListsDto GetCandidateProtein(SearchParametersDto parameters, MsPeaksDto peakData, List<PstTagList> PstTags, ExecutionTimeDto executionTimes)
        {

            Stopwatch moduleTimer = Stopwatch.StartNew();

            var CandidateProteinListsInfo = _proteinRepository.ExtractProteins(peakData.WholeProteinMolecularWeight, parameters, PstTags);

            moduleTimer.Stop();


            //TimeSpan time = TimeSpan.Parse(executionTimes.InsilicoTime);
            //var totaltime = moduleTimer + time;
            //executionTimes.MwFilterTime = moduleTimer.Elapsed.ToString();

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
            
            var tempList = _TerminalModifications.EachProteinTerminalModifications(parameters, candidateProteins);
            UpdatedCandidatedProteinList.AddRange(tempList);
            
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
            List<ProteinDto> candidateProteins1 = new List<ProteinDto>();

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

            //"Module 8 of 9:  Evaluating Final Scores.";                                  //FARHAN

            //for (int iter = 0; iter < candidateProteins.Count; iter++)
            //{
            //    candidateProteins[iter].Score = candidateProteins[iter]
            //}


            for (int i = 0; i < candidateProteins.Count; i++)
            {
                candidateProteins[i].set_score(parameters.MwSweight, parameters.PstSweight, parameters.InsilicoSweight);
            }

            //foreach (var protein in candidateProteins)
            //{
            //    protein.set_score(parameters.MwSweight, parameters.PstSweight, parameters.InsilicoSweight);  //MwSweight is Intact Protein Mass Score Weightage
            //}
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //double score = 0;
            //string lol;
            //for (var i = 0; i < candidateProteins.Count; ++i)
            //{

            //    if (candidateProteins[i].Score > score)
            //    {

            //        score = candidateProteins[i].Score;
            //        lol = candidateProteins[i].Header;
            //    }
            //}

            candidateProteins1 = candidateProteins.OrderByDescending(x => x.Score).ToList();
            return candidateProteins1;
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

            if (candidateProteins.Count > parameters.NumberOfOutputs)
                candidateProteins = candidateProteins.Take(parameters.NumberOfOutputs).ToList<ProteinDto>();

            var final = new SearchResultsDto(parameters.Queryid, candidateProteins, executionTimes);
            _dataLayer.StoreResults(final, parameters.PeakListFileName[fileNumber], parameters.FileUniqueIdArray[fileNumber], fileNumber);
        }

        private void StorePeakListData(string FileUniqueId, List<newMsPeaksDto> peakData2DList)
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

            _dataLayer.StorePeakList(FileUniqueId, peakDataMassesString, peakDataIntensitiesString);
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
    }
}

