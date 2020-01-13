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
            _postTranslationalModificationModule = new PtmCpu();
            _insilicoFragmentsAdjustment = new InsilicoFragmentsAdjustmentCpu();
            _insilicoFilter = new InsilicoFilterCpu();
            _peakListFileReader = new PeakListFileReader();

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
                //System.Threading.Thread.Sleep(10000);                 /*UNCOMMENT ME....*/
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

        
        public static void Send_Results_Link(SearchParametersDto p)
        {
            var emailaddress = p.UserId;
            using (var mm = new MailMessage("perceptron@lums.edu.pk", emailaddress))
            {
                string BaseUrl = "http://perceptron.lums.edu.pk/";
                mm.Subject = "Perceptron: Protein Search Results";
                var body = "Dear User,";
                body += "<br /><br /> The results for protein search query submitted at " + DateTime.Now.ToString() + " with job title \"" +
                        p.Title + "\" have been completed. The complete results are available at";
                body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                body += "<br />If you need help check out the Getting Started Guide and our Video Tutorials. If you encounter any kind of problems, please contact us at <a href=\'" + BaseUrl + "/index.html#/contact'> link</a>.";
                body += "<br />Thank You for using Perceptron.";
                body += "<br />The PERCEPTRON Team";
                body += "<br />Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                mm.Body = body;
            
                mm.IsBodyHtml = true;
                var networkCred = new NetworkCredential("perceptron@lums.edu.pk", "********");
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
            var numberOfPeaklistFiles = parameters.PeakListFileName.Length;  //Number of files uploaded by user

            for (var fileNumber = 0; fileNumber < numberOfPeaklistFiles; fileNumber++)
            {
                //Logging.CreatePeakFileDirectory(fileNumber);

                try
                {

                    var executionTimes = new ExecutionTimeDto();
                    var pipeLineTimer = new Stopwatch();
                    pipeLineTimer.Start();

                    //Step 0 - Read the Peaklist File Read the Peaklist File Read the Peaklist File Read the Peaklist File Read the Peaklist File Read the Peaklist File Read the Peaklist File
                    var massSpectrometryData = PeakListFileReaderModuleModule(parameters, fileNumber, executionTimes);
                    //  Logging.DumpMsData(massSpectrometryData);


                    //Step 1 - 1st Algorithm - Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner Mass Tuner 
                    var old = massSpectrometryData.WholeProteinMolecularWeight;
                    ExecuteMassTunerModule(parameters, massSpectrometryData, executionTimes);
                    if (massSpectrometryData.WholeProteinMolecularWeight == 0)
                        massSpectrometryData.WholeProteinMolecularWeight = old;
                    //Logging.DumpMwTunerResult(massSpectrometryData);


                    //Step  - 2nd Algorithm - Peptide Sequence Tags (PSTs)    PST PST PST PSTPST PST PST PSTPST PST PST PSTPST PST PST PSTPST PST PST PSTPST PST PST PSTPST PST PST PST
                    if (parameters.DenovoAllow == 1)
                    {
                        var PstTags = ExecuteDenovoModule(parameters, massSpectrometryData, executionTimes);
                    }
                    var t = 0;
                    double score = 0;
                    string lol;
                    //Logging.DumpModifiedProteins(candidateProteins);


                    List<newMsPeaksDto> peakData2DList = peakDataList(massSpectrometryData); //Temporary
                    //Step 2 - 1st Candidate Protein List  --- (In SPECTRUM: Score_Mol_Weight{Adding scores with respect to the Mass difference with Intact Mass})
                    int SimpleCandidateProteinList = 0; // For selecting Simple Candidate Protein List

                    var candidateProteins = GetCandidateProtein(parameters, massSpectrometryData, executionTimes, SimpleCandidateProteinList);
                    double mass, error, mw_score;
                    for (var i = 0; i < candidateProteins.Count; ++i)
                    {
                        mass = candidateProteins[i].Mw;
                        error = Math.Abs(mass - massSpectrometryData.WholeProteinMolecularWeight);
                        if (error == 0)
                            mw_score = 1;
                        else
                            mw_score = 1 / (Math.Pow(2, error));
                        candidateProteins[i].MwScore = mw_score; //FARHAN!!! HERE VALUES MAY VARY BUT ITS ROUNDING OFF AT 10th PLACE...
                    }
                    //Logging.DumpCandidateProteins(candidateProteins);

                    ///////////////////////////////////////////////////////////////////////
                    //Step 2B: Getting Candidate Protein List Truncated -- 

                    int TruncatedCandidateProteinList = 1;
                    var CandidateProteinListTruncated = GetCandidateProtein(parameters, massSpectrometryData, executionTimes, TruncatedCandidateProteinList);

                    ///////////////////////////////////////////////////////////////////////


                    



                    //************************************************************************************************//
                    //******************ITS ORIGINAL******************//                    //************************************************//
                    //************************************************************************************************//

                    ////Step X - ??? Algorithm - Post Translational Modifications (PTMs)    {{*****FARHAN!!! FOR THE TIME BEING ITS PTM IS AFTER INSILICO COMPARISON*****}}
                    ////string candidateProteins = "xyz";
                    //candidateProteins = ExecutePostTranslationalModificationsModule(parameters, candidateProteins, massSpectrometryData, executionTimes);
                    //for (var i = 0; i < candidateProteins.Count; ++i)
                    //{
                    //    if (candidateProteins[i].PstScore > score)
                    //    {
                    //        score = candidateProteins[i].PstScore;
                    //        lol = candidateProteins[i].Header;
                    //        t = i;
                    //    }
                    //}
                    //************************************************************************************************//


                    //Step 4 - ??? Algorithm - Spectral Comparison
                    ExecuteSpectralComparisonModule(parameters, candidateProteins, peakData2DList, executionTimes);


                    //Step Next -- Truncation
                    //parameters.Truncation = 1;

                    
                    //ExecuteTruncationModule(parameters);





                    //Logging.DumpInsilicoScores(candidateProteins);
                    score = 0;
                    for (var i = 0; i < candidateProteins.Count; ++i)
                    {
                        if (candidateProteins[i].PstScore + candidateProteins[i].MwScore + candidateProteins[i].PtmScore > score)
                        {
                            score = candidateProteins[i].PstScore + candidateProteins[i].MwScore + candidateProteins[i].PtmScore;
                            lol = candidateProteins[i].Header;
                            t = i;
                        }
                    }

                    candidateProteins = ExecuteProteoformScoringModule(parameters, candidateProteins);
                    //Logging.DumpTotalScores(candidateProteins);

                    pipeLineTimer.Stop();
                    executionTimes.TotalTime = pipeLineTimer.Elapsed.ToString();
                    StoreSearchResults(parameters, candidateProteins, executionTimes, fileNumber);


                }
                catch (Exception r)
                {
                    string k = r.Message;
                    System.Diagnostics.Debug.WriteLine(r.Message);
                }

                //Logging.DumpTotalTime(executionTimes);
                //Logging.ExitPeakFileDirectory();
            }

            Send_Results_Link(parameters);
            _dataLayer.Set_Progress(parameters.Queryid, 100);

        }

        //Mass Tunner
        private void ExecuteMassTunerModule(SearchParametersDto parameters, MsPeaksDto peakData, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();

            if (parameters.Autotune == 1)
            {
                //double a = parameters.NeutralLoss;
                _wholeProteinMassTuner.TuneWholeProteinMass(peakData, parameters.MwTolerance);
            }
            moduleTimer.Stop();
            executionTimes.TunerTime = moduleTimer.Elapsed.ToString();
        }


        //PST: Peptide Sequence Tags
        private List<PstTagList> ExecuteDenovoModule(SearchParametersDto parameters, MsPeaksDto massSpectrometryData, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();

            var pstTags = _pstGenerator.GeneratePeptideSequenceTags(parameters, massSpectrometryData);

            //Logging.DumpPstTags(pstTags);


            //////////if (candidateProteins.Count > 0)                                                          #MAJOR_MODIFICATIONS
            //////////{
            //////////    _pstFilter.ScoreProteinsByPst(pstTags, candidateProteins);  
            //////////}

            //Logging.DumpPstScores(candidateProteins);
            // pstTags.Clear();                                  #MAJOR_MODIFICATIONS
            moduleTimer.Stop();
            executionTimes.PstTime = moduleTimer.Elapsed.ToString();
            return pstTags;
        }

        private List<ProteinDto> GetCandidateProtein(SearchParametersDto parameters, MsPeaksDto peakData,ExecutionTimeDto executionTimes, int CandidateList) // Added "int CandidateList". 20200112
        {

            Stopwatch moduleTimer = Stopwatch.StartNew();

            var listOfProteins = _proteinRepository.ExtractProteins(peakData.WholeProteinMolecularWeight, parameters, CandidateList);// Added "int CandidateList". 20200112

            moduleTimer.Stop();
            
            if (CandidateList == 1)
            {
                TimeSpan time = TimeSpan.Parse(executionTimes.InsilicoTime);
                //var totaltime = moduleTimer + time;
                
            }
            executionTimes.InsilicoTime = moduleTimer.Elapsed.ToString();
            return listOfProteins;
        }

        private static List<ProteinDto> ExecuteProteoformScoringModule(SearchParametersDto parameters, List<ProteinDto> candidateProteins)
        {
            List<ProteinDto> candidateProteins1 = new List<ProteinDto>();
            //"Module 8 of 9:  Evaluating Final Scores.";                                  //FARHAN
            foreach (var protein in candidateProteins)
            {
                protein.set_score(parameters.MwSweight, parameters.PstSweight, parameters.InsilicoSweight);  //MwSweight is Intact Protein Mass Score Weightage
            }
            double score = 0;
            string lol;
            for (var i = 0; i < candidateProteins.Count; ++i)
            {
                if (candidateProteins[i].Score > score)
                {

                    score = candidateProteins[i].Score;
                    lol = candidateProteins[i].Header;
                }
            }
            candidateProteins1 = candidateProteins.OrderByDescending(x => x.Score).ToList();
            return candidateProteins1;

        }

        //SPECTRAL COMPARISON ALGORITHM: 
        private void ExecuteSpectralComparisonModule(SearchParametersDto parameters, List<ProteinDto> candidateProteins,
            List<newMsPeaksDto> peakData2DList, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();


            if (parameters.PtmAllow == 0)
                _insilicoFragmentsAdjustment.adjustForFragmentTypeAndSpecialIons(candidateProteins, parameters.InsilicoFragType, parameters.HandleIons);

            //"Module 7 of 9:  Insilico Filteration.";                                  //FARHAN
            if (parameters.PtmAllow == 0)
            {
                //_insilicoFilter.ComputeInsilicoScore(candidateProteins, massSpectrometryData.Mass, parameters.HopThreshhold); //Commented

                //newProgram Object = new peakData2DList();
                //Object.peakDataList = new List<peakData2DList>();
                //var peakList = new List<peakData2DList>();
                //peakList = peakData2DList.peakDataList(massSpectrometryData);

                _insilicoFilter.ComputeInsilicoScore(candidateProteins, peakData2DList, parameters.PeptideTolerance, parameters.PeptideToleranceUnit);
                //_insilicoFilter.ComputeInsilicoScore(candidateProteins, massSpectrometryData.Mass, parameters.PeptideTolerance, parameters.PeptideToleranceUnit);
            }
            moduleTimer.Stop();
            executionTimes.InsilicoTime = moduleTimer.Elapsed.ToString();
        }

        private List<ProteinDto> ExecutePostTranslationalModificationsModule(SearchParametersDto parameters, List<ProteinDto> candidateProteins,
            MsPeaksDto massSpectrometryData, ExecutionTimeDto executionTimes)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();
            List<ProteinDto> proteoformsList;

            if (parameters.PtmAllow == 1)
            {
                proteoformsList = _postTranslationalModificationModule.ExecutePtmModule(candidateProteins,
                   massSpectrometryData, parameters);

                //_insilicoFilter.ComputeInsilicoScore(proteoformsList, massSpectrometryData.Mass, parameters.HopThreshhold);//Commented
                /* #CFTTB
                 * _insilicoFilter.ComputeInsilicoScore(candidateProteins, massSpectrometryData.Mass, parameters.PeptideTolerance, parameters.PeptideToleranceUnit);
                 */
                _molecularWeightModule.FilterModifiedProteinsByWholeProteinMass(parameters, proteoformsList,
               massSpectrometryData);
            }

            else
            {
                proteoformsList = candidateProteins;
                foreach (var protein in proteoformsList)
                {
                    protein.PtmParticulars = new List<PostTranslationModificationsSiteDto>();
                }
            }

            moduleTimer.Stop();
            executionTimes.PtmTime = moduleTimer.Elapsed.ToString();
            return proteoformsList;
        }

        //private List<ProteinDto> ExecuteTruncationModule(SearchParametersDto parameters)
        //{
        //    if (parameters.Truncation == "true")
        //    {
        //        int abc = 0;
        //    }

        //}

        private void StoreSearchResults(SearchParametersDto parameters, List<ProteinDto> candidateProteins, ExecutionTimeDto executionTimes, int fileNumber)
        {
            if (candidateProteins.Count > Constants.NumberOfResultsToStore)
                candidateProteins = candidateProteins.Take(Constants.NumberOfResultsToStore).ToList<ProteinDto>();

            var final = new SearchResultsDto(parameters.Queryid, candidateProteins, executionTimes);
            _dataLayer.StoreResults(final, parameters.PeakListFileName[fileNumber], fileNumber);
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
