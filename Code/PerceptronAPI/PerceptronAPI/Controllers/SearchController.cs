using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using System.Net.Mail;
using System.IO;
using System.IO.Compression;
using PerceptronAPI.Engine;
using GraphForm;
using PerceptronAPI.Models;
using PerceptronAPI.Repository;
using PerceptronAPI.Utility;
using System.Data.Entity.Validation;



namespace PerceptronAPI.Controllers
{
    public class SearchController : ApiController
    {

        readonly IDataAccessLayer _dataLayer;
        public DateTime JobSubmissionTime = DateTime.Now.AddDays(-2);  // Fetching Current Time  //Results will available for 48hrs only

        private void CreateDirectory()
        {
            string MainPathForResults = @"C:\PerceptronApi-tempResultsFolder\";
            if (!(Directory.Exists(MainPathForResults)))
            {
                Directory.CreateDirectory(MainPathForResults);
            }
        }

        public SearchController()
        {
            CreateDirectory();
            _dataLayer = new SqlDatabase();

            //UsersController UserController = new UsersController();
            //var ErrorMessage = UserController.VerfiyingEmailAddress();   // UserController
            



            //AuthenticateUserByFirebase();
            // CHECK TIME AND ADD HERE TO EXPIRE THE RESULTS 


            // CHECK TIME AND ADD HERE TO EXPIRE THE RESULTS 



            //var blob = Database_Download();
            //var Message = Database_Update();
            //Server.MapPath("~");
        }

        [HttpPost]
        [Route("api/search/File_upload")]
        public async Task<HttpResponseMessage> File_upload()
        {
            
            var queryId = Guid.NewGuid().ToString();

            var a = HttpContext.Current.Response.Cookies.Count;

            DateTime time = DateTime.Now;             // Fetching Current Time
            string format = "yyyy/MM/dd HH:mm:ss";
            var creationTime = time.ToString(format); // Formating creationTime and assigning
            const string progress = "0";


            var parametersDto = new SearchParametersDto
            {
                SearchFiles = new List<SearchFile>(),
                SearchQuerry = new SearchQuery(),
                FixedMods = new PtmFixedModification(),
                VarMods = new PtmVariableModification()
            };
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new CustomMultipartFormDataStreamProvider(root);

            try
            {
                
                await Request.Content.ReadAsMultipartAsync(provider);

                var jsonData = provider.FormData.GetValues("Jsonfile");

                if (jsonData != null)
                {
                    parametersDto.SearchParameters = JsonConvert.DeserializeObject<SearchParameter>(jsonData[0].Trim('"'));
                    parametersDto.SearchQuerry = JsonConvert.DeserializeObject<SearchQuery>(jsonData[0].Trim('"'));
                    parametersDto.SearchQuerry.JobSubmission = time;
                    parametersDto.SearchParameters.JobSubmission = time;
                    parametersDto.FixedMods = JsonConvert.DeserializeObject<PtmFixedModification>(jsonData[0].Trim('"'));
                    parametersDto.VarMods = JsonConvert.DeserializeObject<PtmVariableModification>(jsonData[0].Trim('"'));
                }


                if (parametersDto.FixedMods.FixedModifications != "")
                {
                    parametersDto.FixedMods.QueryId = queryId;
                    parametersDto.FixedMods.JobSubmission = time;
                    parametersDto.FixedMods.ModificationId = 1;
                }

                if (parametersDto.VarMods.VariableModifications != "")
                {
                    parametersDto.VarMods.QueryId = queryId;
                    parametersDto.VarMods.JobSubmission = time;
                    parametersDto.VarMods.ModificationId = 1;
                }

                parametersDto.SearchParameters.QueryId = queryId;
                parametersDto.SearchQuerry.QueryId = parametersDto.SearchParameters.QueryId;
                parametersDto.SearchQuerry.Progress = progress;
                parametersDto.SearchQuerry.CreationTime = creationTime;
                parametersDto.SearchQuerry.UserId = parametersDto.SearchParameters.UserId;

                //////////////////
                ///AAAAAAAAAAAAAAAAAAAADDDDDDDDDDDDDDDDDDD
                ///
                InputFileProcessing(queryId, provider.FileData[0].LocalFileName, time, parametersDto);

                var response = _dataLayer.StoreSearchParameters(parametersDto); //Search.ProteinSearch(parametersDto);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (DbEntityValidationException e)    //DbEntityValidationException
            {
                var _DBErrorException = new DBErrorException();
                _DBErrorException.DbEntitiyError(e);
                if (parametersDto.SearchParameters.EmailId != "")
                {
                    //StreamReader ReadPerceptronEmailAddress = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailAddress.txt");
                    //StreamReader ReadPerceptronEmailPassword = new StreamReader(@"C:\01_DoNotEnterPerceptronRelaventInfo\PerceptronEmailPassword.txt");

                    //SendingEmail.SendingEmailMethod(ReadPerceptronEmailAddress.ReadLine(), ReadPerceptronEmailPassword.ReadLine(), parametersDto.SearchParameters.EmailId, parametersDto.SearchParameters.Title, creationTime, "Error");
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
        private void InputFileProcessing(string queryId, string FileName, DateTime time, SearchParametersDto parametersDto)
        {
            var i = 0;
            AddSuffixInName _AddSuffixInName = new AddSuffixInName();
            List<string> InputFileList = new List<string> { FileName };
            if (Path.GetExtension(InputFileList[0]) == ".zip") //Check If file is Zipped
            {
                InputFileList = ZipFileUnzipping(InputFileList); //Unzipping the zipped file.
            }

            for (int index = 0; index < InputFileList.Count; index++)//foreach (var file in provider.FileData)
            {
                string file = InputFileList[index];
                var FileUniqueId = Guid.NewGuid().ToString();
                string FileNameWithUniqueID = _AddSuffixInName.AddSuffix(file, "-ID-" + FileUniqueId); //Updated: To avoid file replacement due to same filenames
                File.Move(file, FileNameWithUniqueID); // Renaming "user's input data file" with "user's input data file + Unique ID (FileUniqueId)"

                i++;
                var x = new SearchFile
                {
                    FileId = i,
                    FileName = file,
                    UniqueFileName = FileNameWithUniqueID,
                    FileType = System.IO.Path.GetExtension(file),
                    QueryId = queryId,
                    FileUniqueId = FileUniqueId,
                    JobSubmission = time
                };
                parametersDto.SearchFiles.Add(x);
            }
            if (parametersDto.SearchFiles.Count == 1)
            {
                parametersDto.SearchParameters.FDRCutOff = "N/A";  //Updated 20210209
            }
        }

        private List<string> ZipFileUnzipping(List<string> InputFileList)
        {
            string ZipFilePath = InputFileList[0];
            var FileName = Path.GetFileNameWithoutExtension(ZipFilePath);
            string FileDirectory = Path.GetDirectoryName(ZipFilePath) + "\\" + FileName;
            string ExtractZipFilePath = Path.GetDirectoryName(ZipFilePath) + "\\" + FileName + "\\";

            if (Directory.Exists(FileDirectory))
                Directory.Delete(FileDirectory, true);

            string ZipFullFileName = ZipFilePath;
            using (var archieve = ZipFile.Open(ZipFullFileName, ZipArchiveMode.Read)) // Creating Zip File
            {
                ZipFile.ExtractToDirectory(ZipFullFileName, ExtractZipFilePath);
            }
            //Extracting the names of the file

            var ZipExtractedFiles = Directory.GetFiles(ExtractZipFilePath); //Reading the contents of Unzipped Folder
            InputFileList = new List<string> ( ZipExtractedFiles );  //Returning the List of Full File Names

            return InputFileList;
        }


        public string SearchQuery(string[] ParameterValues, string VerifiedUser, string FtpRootPath, string FtpUploadedFilePathKeepLocalCopy)
        {
            string Message = "";

            var parametersDto = new SearchParametersDto
            {
                SearchFiles = new List<SearchFile>(),
                SearchQuerry = new SearchQuery(),
                FixedMods = new PtmFixedModification(),
                VarMods = new PtmVariableModification()
            };
            DateTime time = DateTime.Now;             // Fetching Current Time
            string format = "yyyy/MM/dd HH:mm:ss";
            var creationTime = time.ToString(format); // Formating creationTime and assigning
            parametersDto.SearchQuerry.CreationTime = creationTime;
            parametersDto.SearchQuerry.Progress = "0";

            var queryId = Guid.NewGuid().ToString();
            try
            {
                AddSuffixInName _AddSuffixInName = new AddSuffixInName();

                ParametersProcessing(queryId, Message, time, ParameterValues, parametersDto, VerifiedUser);

                string FileNamewExtension = ParameterValues[34];
                string OldFullFileName = "";
                if (VerifiedUser == "True")
                {
                    OldFullFileName = FtpRootPath + ParameterValues[31] + "\\" + FileNamewExtension;
                }
                else
                {
                    OldFullFileName = FtpRootPath + @"Public\" + FileNamewExtension;
                }


                //string FtpUploadedFilePathKeepLocalCopy = @"L:\FtpInputFiles\";
                string NewFullFileName = FtpUploadedFilePathKeepLocalCopy + FileNamewExtension;

                File.Copy(OldFullFileName, NewFullFileName, true);  // Moving Input file from ftproot folder to FtpInputFiles folder

                InputFileProcessing(queryId, NewFullFileName, time, parametersDto);

                var response = _dataLayer.StoreSearchParameters(parametersDto); //Search.ProteinSearch(parametersDto);
                if (VerifiedUser == "True")
                {
                    Message = _dataLayer.StorePerceptronSdkInfo(time, queryId, parametersDto.SearchParameters.Title, ParameterValues[31]);
                }
                else
                {
                    Message = _dataLayer.StorePerceptronSdkInfo(time, queryId, parametersDto.SearchParameters.Title, "Guest");
                }

                //return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (DbEntityValidationException e)
            {
                return Message = "Error while submitting query please try again. Also, if problem still persists then, please report a bug on GitHub.";
            }


            return Message;
        }

        public void ParametersProcessing(string queryId, string Message, DateTime time, string[] ParameterValues, SearchParametersDto parametersDto, string VerifiedUser)
        {
            parametersDto.SearchQuerry.QueryId = queryId;
            parametersDto.SearchQuerry.JobSubmission = time;
            parametersDto.SearchParameters.JobSubmission = time;
            //parametersDto.SearchQuerry.UserId = ParameterValues[32];
            //if (ParameterValues[32] == "")
            //{
            //    throw new ArgumentException("User id cannot be empty!");
            //}
            parametersDto.SearchParameters.NumberOfOutputs = "100";   //In CallingPerceptronApi User cannot decide the Number of output resutls.
            parametersDto.SearchParameters.QueryId = queryId;
            
            if (ParameterValues[0] == "")
            {
                throw new ArgumentException("Query title cannot be empty!");
            }
            parametersDto.SearchParameters.Title = ParameterValues[0];

            if (ParameterValues[1] == "" || Convert.ToDouble(ParameterValues[1]) > 100.0 || Convert.ToDouble(ParameterValues[1]) < 0.0)
            {
                throw new ArgumentException("Invalid FDR value!");
            }
            parametersDto.SearchParameters.FDRCutOff = ParameterValues[1];

            if (ParameterValues[2] != "Human" && ParameterValues[2] != "Ecoli")
            {
                throw new ArgumentException("Invalid Protein Database!");
            }
            parametersDto.SearchParameters.ProteinDatabase = ParameterValues[2];

            
            if (ParameterValues[3] != "M(Neutral)" && ParameterValues[3] != "MH+")
            {
                throw new ArgumentException("Invalid MassMode Input!");
            }
            parametersDto.SearchParameters.MassMode = ParameterValues[3];

            
            if (ParameterValues[4] != "True" && ParameterValues[4] != "False")
            {
                throw new ArgumentException("Invalid FilterDatabase Input!");
            }
            parametersDto.SearchParameters.FilterDb = ParameterValues[4];

            
            if (Convert.ToDouble(ParameterValues[5]) < 0)
            {
                throw new ArgumentException("Invalid ProteinMassTolerance!");
            }
            parametersDto.SearchParameters.MwTolerance = Convert.ToDouble(ParameterValues[5]);

            
            if (Convert.ToDouble(ParameterValues[6]) < 0)
            {
                throw new ArgumentException("Invalid PeptideTolerance!");
            }
            parametersDto.SearchParameters.PeptideTolerance = Convert.ToDouble(ParameterValues[6]);

            
            if (ParameterValues[7] != "ppm" && ParameterValues[7] != "Da" && ParameterValues[7] != "mmu")
            {
                throw new ArgumentException("Invalid PeptideToleranceUnit!");
            }
            parametersDto.SearchParameters.PeptideToleranceUnit = ParameterValues[7];

            if (ParameterValues[8] != "True" && ParameterValues[8] != "False")
            {
                throw new ArgumentException("Invalid TuneIntactProteinMass Input!");
            }
            parametersDto.SearchParameters.Autotune = ParameterValues[8];

            
            if (ParameterValues[9] != "HCD" && ParameterValues[9] != "CID" && ParameterValues[9] != "ECD" && 
                ParameterValues[9] != "ETD" && ParameterValues[9] != "EDD" && ParameterValues[9] != "BIRD" && ParameterValues[9] != "SID" 
                && ParameterValues[9] != "IMD" && ParameterValues[9] != "NETD")
            {
                throw new ArgumentException("Invalid InsilicoFragmentationType!");
            }
            parametersDto.SearchParameters.InsilicoFragType = ParameterValues[9];

            
            if (ParameterValues[9] == "HCD" || ParameterValues[9] == "CID" || ParameterValues[9] == "IMD" || ParameterValues[9] == "BIRD" && ParameterValues[9] == "SID")
            {
                if (ParameterValues[10] != "bo" && ParameterValues[10] != "bstar" && ParameterValues[10] != "yo" && ParameterValues[10] != "ystar" && ParameterValues[10] != "bo,bstar" 
                    && ParameterValues[10] != "bo,yo" && ParameterValues[10] != "bo,ystar" && ParameterValues[10] != "bstar,yo" && ParameterValues[10] != "bstar,ystar" && 
                    ParameterValues[10] != "yo,ystar" && ParameterValues[10] != "bo,bstar,yo" && ParameterValues[10] != "bo,bstar,ystar" && ParameterValues[10] != "bo,yo,ystar" && 
                    ParameterValues[10] != "bstar,yo,ystar" && ParameterValues[10] != "bo,bstar,yo,ystar")
                    throw new ArgumentException("Invalid Types of Special Ions!");
            }
            else if (ParameterValues[9] == "ECD" || ParameterValues[9] != "ETD")
            {
                if (ParameterValues[10] != "zo" && ParameterValues[10] != "zoo" && ParameterValues[10] != "zo,zoo")
                    throw new ArgumentException("Invalid Types of Special Ions!");
            }
            else if (ParameterValues[9] == "NETD" || ParameterValues[9] != "EDD")
            {
                if (ParameterValues[10] != "ao" && ParameterValues[10] != "astar" && ParameterValues[10] != "ao,astar")
                    throw new ArgumentException("Invalid Types of Special Ions!");
            }
            parametersDto.SearchParameters.HandleIons = ParameterValues[10];

            if (ParameterValues[11] != "True" && ParameterValues[11] != "False")
            {
                throw new ArgumentException("Invalid DenovoAllow Input!");
            }
            parametersDto.SearchParameters.DenovoAllow = ParameterValues[11];
            
            if (Convert.ToInt16(ParameterValues[12]) < 2 || Convert.ToInt16(ParameterValues[12]) > 6)
            {
                throw new ArgumentException("Invalid Value for MinimumPeptideSequenceTagLength!");
            }
            parametersDto.SearchParameters.MinimumPstLength = Convert.ToInt16(ParameterValues[12]);
            
            if (Convert.ToInt16(ParameterValues[13]) < Convert.ToInt16(ParameterValues[12]) || Convert.ToInt16(ParameterValues[12]) > 8)
            {
                throw new ArgumentException("MaximumPeptideSequenceTagLength cannot be smaller than MinimumPeptideSequenceTagLength!");
            }
            parametersDto.SearchParameters.MaximumPstLength = Convert.ToInt16(ParameterValues[13]);
            parametersDto.SearchParameters.HopThreshhold = Convert.ToDouble(ParameterValues[14]);
            if (ParameterValues[15] != "Da")
            {
                throw new ArgumentException("Invalid PeptideSequenceTag_Hop_Tolerance_Unit!");
            }
            
            parametersDto.SearchParameters.HopTolUnit = ParameterValues[15];

            if (ParameterValues[17] != "True" && ParameterValues[17] != "False")
            {
                throw new ArgumentException("Invalid Truncation Input!");
            }
            parametersDto.SearchParameters.PSTTolerance = Convert.ToDouble(ParameterValues[16]);
            parametersDto.SearchParameters.Truncation = ParameterValues[17];

            if (ParameterValues[18] != "None" && ParameterValues[18] != "NME" && ParameterValues[18] != "NME_Acetylation" && 
                ParameterValues[18] != "M_Acetylation" && ParameterValues[18] != "None,NME" && ParameterValues[18] != "None,NME_Acetylation" && 
                ParameterValues[18] != "None,M_Acetylation" && ParameterValues[18] != "NME,NME_Acetylation" && ParameterValues[18] != "NME,M_Acetylation" 
                && ParameterValues[18] != "None,NME,NME_Acetylation" && ParameterValues[18] != "None,NME,M_Acetylation" 
                && ParameterValues[18] != "None,NME_Acetylation,M_Acetylation" && ParameterValues[18] != "NME,NME_Acetylation,M_Acetylation" && 
                ParameterValues[18] != "None,NME,NME_Acetylation,M_Acetylation")
            {
                throw new ArgumentException("Invalid TerminalModification!");
            }
            parametersDto.SearchParameters.TerminalModification = ParameterValues[18];

            if (ParameterValues[19] != "True" && ParameterValues[19] != "False")
            {
                throw new ArgumentException("Invalid PostTranslationalModificationsAllow Input!");
            }
            parametersDto.SearchParameters.PtmAllow = ParameterValues[19];
            parametersDto.SearchParameters.PtmTolerance = Convert.ToDouble(ParameterValues[20]);

            if (ParameterValues[23] != "None" && ParameterValues[23] != "MSO" && ParameterValues[23] != "MSONE")
                //&& ParameterValues[23] 
                //!= "None,MSO" && ParameterValues[23] != "None,MSONE" && ParameterValues[23] != "MSO,MSONE" && ParameterValues[23] != "None,MSO,MSONE")
            {
                throw new ArgumentException("Invalid MethionineChemicalModification!");
            }
            
            parametersDto.SearchParameters.MethionineChemicalModification = ParameterValues[23];

            if (ParameterValues[25] != "None" && ParameterValues[25] != "Cys_CAM" && ParameterValues[25] != "Cys_PE" && ParameterValues[25] != "Cys_CM" && ParameterValues[25] != "Cys_PAM")
                //&& ParameterValues[25] != "None,Cys_CAM" && ParameterValues[25] != "None,Cys_PE" && ParameterValues[25] != "None,Cys_CM" && ParameterValues[25] != "None,Cys_PAM" && 
                //ParameterValues[25] != "None,Cys_CAM,Cys_PE" && ParameterValues[25] != "None,Cys_CAM,Cys_CM" && ParameterValues[25] != "None,Cys_CAM,Cys_PAM" && 
                //ParameterValues[25] != "Cys_CAM,Cys_PE,Cys_CM" && ParameterValues[25] != "Cys_CAM,Cys_PE,Cys_PAM" && ParameterValues[25] != "Cys_CAM,Cys_CM,Cys_PAM" &&
                //ParameterValues[25] != "Cys_PE,Cys_CM,Cys_PAM" && ParameterValues[25] != "None,Cys_CAM,Cys_PE,Cys_CM" && ParameterValues[25] != "None,Cys_CAM,Cys_PE,Cys_PAM" && 
                //ParameterValues[25] != "None,Cys_CAM,Cys_CM,Cys_PAM" && ParameterValues[25] != "None,Cys_PE,Cys_CM,Cys_PAM" && ParameterValues[25] != "Cys_CAM,Cys_PE,Cys_CM,Cys_PAM" &&
                //ParameterValues[25] != "None,Cys_CAM,Cys_PE,Cys_CM,Cys_PAM")
            {
                throw new ArgumentException("Invalid CysteineChemicalModification!");
            }
            parametersDto.SearchParameters.CysteineChemicalModification = ParameterValues[25];

            if (Convert.ToDouble(ParameterValues[26]) < 0 || Convert.ToDouble(ParameterValues[26]) > 100)
            {
                throw new ArgumentException("Invalid MwScoringWeightage Value!");
            }
            parametersDto.SearchParameters.MwSweight = Convert.ToDouble(ParameterValues[26]);

            if (Convert.ToDouble(ParameterValues[27]) < 0 || Convert.ToDouble(ParameterValues[27]) > 100)
            {
                throw new ArgumentException("Invalid PeptideSequenceTagScoringWeightage Value!");
            }
            parametersDto.SearchParameters.PstSweight = Convert.ToDouble(ParameterValues[27]);

            
            if (Convert.ToDouble(ParameterValues[28]) < 0 || Convert.ToDouble(ParameterValues[28]) > 100)
            {
                throw new ArgumentException("Invalid InsilicoScoringWeightage Value!");
            }
            parametersDto.SearchParameters.InsilicoSweight = Convert.ToDouble(ParameterValues[28]);


            //bool validEmail = IsValidEmail(ParameterValues[31]);
            if (VerifiedUser == "True")
            {
                parametersDto.SearchParameters.EmailId = ParameterValues[32];
                parametersDto.SearchParameters.UserId = ParameterValues[32];
                parametersDto.SearchQuerry.UserId = ParameterValues[32];
            }
            else        //If  User is a Guest
            {
                if(IsValidEmail(ParameterValues[32]))       //Guest User Give its Email Id
                {
                    parametersDto.SearchParameters.EmailId = ParameterValues[32];
                }
                else                //Guest User does not give its Email Id
                {
                    parametersDto.SearchParameters.EmailId = "";
                }
                
                parametersDto.SearchParameters.UserId = queryId;   // Here UserId is Based on QueryId
            }

            if (ParameterValues[21] != "-")
            {
                parametersDto.FixedMods.QueryId = queryId;
                parametersDto.FixedMods.ModificationId = 1;
                parametersDto.FixedMods.JobSubmission = time;
                parametersDto.FixedMods.FixedModifications = ParameterValues[21];

            }

            if (ParameterValues[22] != "-")
            {
                parametersDto.VarMods.QueryId = queryId;
                parametersDto.VarMods.ModificationId = 1;
                parametersDto.VarMods.JobSubmission = time;
                parametersDto.VarMods.VariableModifications = ParameterValues[22];

            }

        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        [Route("api/search/FDR_Data_upload")]
        public string FDR_Data_upload([FromBody] string input)
        {
            var _FDR = new FDR();
            _FDR.MainFDR();

            return "ABC";
        }


        [HttpPost]
        [Route("api/search/ResultsDownload")]
        public ResultsDownloadDto ResultsDownload([FromBody] string input)  // where input = QueryId
        {
            var TotalTime = new Stopwatch();
            TotalTime.Start();


            var ZipResultFileInfo = _dataLayer.ScanResultFile(input);
            List<byte[]> ListOfFileBlobs = new List<byte[]>();
            string ZipFullFileName = ZipResultFileInfo.ZipFileWithQueryId;
            string ZipFileName = ZipResultFileInfo.ZipFileName;


            using (FileStream fileStream = File.OpenRead(ZipFullFileName))
            {
                byte[] blob = new byte[fileStream.Length];
                fileStream.Read(blob, 0, (int)fileStream.Length);
                ListOfFileBlobs.Add(blob);
            }

            var ResultsDownloadData = new ResultsDownloadDto(ZipFileName, ListOfFileBlobs);
            TotalTime.Stop();
            string time = TotalTime.Elapsed.ToString();
            return ResultsDownloadData;



            /////////GIVEN BELOW --- // ITS HEALTHY WAS IN USED WHEN API COMPILE THE RESUTLS 20201219
            //List<byte[]> ListOfFileBlobs = new List<byte[]>();
            //var ScanInputDataInfo = _dataLayer.ScanInputData(input);
            //var AllResultFilesNames = new List<string>();
            //var TopProteinsOfEachFile = new List<ProteinDto>();


            //string filePath = @"C:\\PerceptronApi-tempResultsFolder\\";
            //string ZipFileName = "Results_" + input + ".zip";
            //string ZipFullFileName = filePath + ZipFileName;

            //WriteResultsFile _WriteResultsFile = new WriteResultsFile();

            ////Loop here
            ////for (int NoOfFiles=0; NoOfFiles< ScanInputDataInfo.FileUniqueIdsList.Count; NoOfFiles++)
            ////{


            //for (int indexofFile = 0; indexofFile < ScanInputDataInfo.FileUniqueIdsList.Count; indexofFile++)
            //{
            //    string FileUniqueId = ScanInputDataInfo.FileUniqueIdsList[indexofFile];
            //    var ScanData = _dataLayer.ScanResultsDownloadData(input, FileUniqueId);    //Scanning File Unique Ids from SearchFiles Table

            //    var SingleResultFileInfo = _WriteResultsFile.ResultFilesWrite(ScanData, filePath, indexofFile, ScanInputDataInfo);   // Writing: All Results files

            //    AllResultFilesNames.Add(SingleResultFileInfo.ResultFileName);
            //    if (SingleResultFileInfo.TopProteinOfResultFile.Count != 0)
            //    {
            //        TopProteinsOfEachFile.Add(SingleResultFileInfo.TopProteinOfResultFile[0]);
            //    }
            //}


            //if (TopProteinsOfEachFile.Count > 1)
            //{
            //    string FileWithPath = filePath + ScanInputDataInfo.searchParameters.Title + ".csv";
            //    string BatchFileName = _WriteResultsFile.WriteBatchResultsFile(FileWithPath, TopProteinsOfEachFile);
            //    AllResultFilesNames.Add(BatchFileName);
            //}

            //var WriteSearchParametersFile = _WriteResultsFile.WriteParametersInTXTFile(ScanInputDataInfo.searchParameters, filePath);
            //AllResultFilesNames.Add(WriteSearchParametersFile);

            //string fullfilename = "";

            //if (File.Exists(ZipFullFileName))
            //    File.Delete(ZipFullFileName); //Deleted Pre-existing file

            //using (var archieve = ZipFile.Open(ZipFullFileName, ZipArchiveMode.Create)) // Creating Zip File
            //{
            //    for (int i = 0; i < AllResultFilesNames.Count; i++)
            //    {
            //        fullfilename = filePath + AllResultFilesNames[i];
            //        archieve.CreateEntryFromFile(fullfilename, Path.GetFileName(fullfilename));   // Adding all results files into the zip file
            //    }
            //}

            //using (FileStream fileStream = File.OpenRead(ZipFullFileName))
            //{
            //    byte[] blob = new byte[fileStream.Length];
            //    fileStream.Read(blob, 0, (int)fileStream.Length);
            //    ListOfFileBlobs.Add(blob);
            //}

            //var ResultsDownloadData = new ResultsDownloadDto(ZipFileName, ListOfFileBlobs);
            //TotalTime.Stop();
            //string time = TotalTime.Elapsed.ToString();
            //return ResultsDownloadData;
            /////////GIVEN ABOVE --- // ITS HEALTHY WAS IN USED WHEN API COMPILE THE RESUTLS 20201219
        }



        [HttpPost]
        [Route("api/search/Post_scan_results")]
        public List<ScanResults> Post_scan_results([FromBody] string input)
        {
            Debug.WriteLine(input);
            var temp = _dataLayer.Scan_Results(input, JobSubmissionTime);
            return temp;
        }

        [HttpPost]
        [Route("api/search/Post_summary_results")]
        public List<SummaryResults> Post_summary_results([FromBody] string input)
        {

            Debug.WriteLine(input);
            string[] values = input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var qid = values[0];
            var fileId = values[1];
            var temp = _dataLayer.Summary_results(qid, fileId, JobSubmissionTime);
            return temp;
        }

        [HttpPost]
        [Route("api/search/Post_detailed_results")]
        public DetailedResults Post_detailed_results([FromBody] string input)
        {
            Debug.WriteLine(input);
            string[] values = input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var qid = values[0];
            var resultid = values[1];
            var temp = _dataLayer.Detailed_Results(qid, resultid, JobSubmissionTime);

            return temp;
        }

        [HttpPost]
        [Route("api/search/Post_DetailedProteinHitView_results")]
        public ResultsVisualizeData Post_DetailedProteinHitView_results([FromBody] string input)
        {
            Debug.WriteLine(input);
            string[] values = input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var qid = values[0];
            var resultid = values[1];
            var rank = values[2];
            //var temp = _dataLayer.DetailedProteinHitView_Results("1", input);


            //ITS A PART OF RESULTS VISUALIZATION ONCE COMPELTED WILL MOVE TO THIS (Post_DetailedProteinHitView_results) METHOD       /////NOW EMBEDDED INTO Post_detailed_results TO AVOID 

            DetailedProteinHitView temp2 = _dataLayer.DetailedProteinHitView_Results(qid, resultid, rank, JobSubmissionTime);

            var MassSpectra = new FormForGraph();
            var InsilicoSpectra = MassSpectra.fillChart(temp2);

            //string NameofFile = "DetailedProteinView_Qid_" + temp2.Results.Results.QueryId + "_Rid_" + temp2.Results.Results.ResultId + ".jpg";
            //var NameOfFileWithFullPath = HttpContext.Current.Server.MapPath("~/App_Data") + "\\Results\\TemporaryResults\\" + NameofFile;


            var ImageForm = new DetailedProteinView();
            bool downloadresults = false;
            var NameOfFileWithFullPath = ImageForm.writeOnImage(temp2, downloadresults);

            //var imgURL = Url.Content(string.Format(@"C:\\inetpub\\wwwroot\\" + NameofFile, NameofFile));  //App_Data/Images/{0}

            FileStream imgStream = File.OpenRead(NameOfFileWithFullPath);
            byte[] blob = new byte[imgStream.Length];
            imgStream.Read(blob, 0, (int)imgStream.Length);
            MsPeaksDto _MsPeaksDto = new MsPeaksDto();
            var PeakListInfo = _MsPeaksDto.PeakListInfo(temp2.PeakListData);    //Formatting the Peak List Info also, in ascending order

            var Data = new ResultsVisualizeData(input, blob, InsilicoSpectra, PeakListInfo);

            return Data;
        }

        //[HttpPost]
        //[Route("api/search/Post_detailed_results/Create_Detailed_Protein_View_Hit")]
        //public string Create_Detailed_Protein_View_Hit([FromBody] string input)
        //{
        //    Debug.WriteLine(input);
        //    var temp = _dataLayer.DetailedProteinHitView_Results("1", input);


        //    ////ITS A PART OF RESULTS VISUALIZATION ONCE COMPELTED WILL MOVE TO THIS (Post_DetailedProteinHitView_results) METHOD       /////NOW EMBEDDED INTO Post_detailed_results TO AVOID 

        //    DetailedProteinHitView temp2 = _dataLayer.DetailedProteinHitView_Results("1", input);
        //    var ImageForm = new DetailedProteinView();
        //    bool downloadresults = false;
        //    var NameofFileWithFullPath = ImageForm.writeOnImage(temp2, downloadresults);
        //    return NameofFileWithFullPath;


        //    //return "This is a testing string...";
        //}



        [HttpPost]
        [Route("api/search/CallingPerceptronApiRegisterUser")]
        public async Task<string> RegisterUser(HttpRequestMessage request)
        {
            var RequestInput = request.Content.ReadAsStringAsync();  //.Content.ToString();  //.
            string input = RequestInput.Result.ToString();
            Debug.WriteLine(input);
            var temp = _dataLayer.GetUserHistory(input, JobSubmissionTime);
            string JobStatus = temp[0].progress;
            if (JobStatus == "Completed")
            {
                var ZipResultFileInfo = _dataLayer.ScanResultFile(input);

                string OldFullFileName = ZipResultFileInfo.ZipFileWithQueryId;
                string NewPath = @"D:\10_PERCEPTRON_Live\ftproot\ResultsReadilyAvailableByFtp\";
                string FileName = Path.GetFileName(OldFullFileName);
                string NewFullFileName = NewPath + FileName;

                File.Move(OldFullFileName, NewFullFileName);  // Moving Input file from ftproot folder to InputFilesByFtp folder

                string FtpPath = @"\ftproot\ResultsReadilyAvailableByFtp\";

                return FtpPath + FileName;
            }
            else
            {
                return JobStatus;
            }


        }



        [HttpPost]
        [Route("api/search/CallingPerceptronApiResults")]
        public async Task<string> CallingPerceptronApiHistory(HttpRequestMessage request)
        {
            var RequestInput = request.Content.ReadAsStringAsync();  //.Content.ToString();  //.
            string input = RequestInput.Result.ToString();
            Debug.WriteLine(input);
            var temp = _dataLayer.GetUserHistory(input, JobSubmissionTime);
            string JobStatus = temp[0].progress;
            if (JobStatus == "Completed")
            {
                var ZipResultFileInfo = _dataLayer.ScanResultFile(input);

                string OldFullFileName = ZipResultFileInfo.ZipFileWithQueryId;
                string NewPath = @"D:\10_PERCEPTRON_Live\ftproot\ResultsReadilyAvailableByFtp\";
                string FileName = Path.GetFileName(OldFullFileName);
                string NewFullFileName = NewPath + FileName;                

                File.Move(OldFullFileName, NewFullFileName);  // Moving Input file from ftproot folder to InputFilesByFtp folder

                string FtpPath = @"\ftproot\ResultsReadilyAvailableByFtp\";

                return FtpPath+FileName;
            }
            else
            {
                return JobStatus;
            }


        }


        [HttpPost]
        [Route("api/search/CallingPerceptronApiResultsDownload")]
        public async Task<string> CallingPerceptronApiResultsDownload(HttpRequestMessage request)
        {
            var RequestInput = request.Content.ReadAsStringAsync();  //.Content.ToString();  //.
            string input = RequestInput.Result.ToString();
            var ZipResultFileInfo = _dataLayer.ScanResultFile(input);

            string FullFileName = "";
            return FullFileName;
        }


        [HttpPost]
        [Route("api/search/Post_history")]
        public List<UserHistory> Post_history([FromBody] string input)
        {
            Debug.WriteLine(input);
            var temp = _dataLayer.GetUserHistory(input, JobSubmissionTime);
            return temp;
        }

        [HttpPost]
        [Route("api/search/stat")]
        public stat stat([FromBody] string input)
        {

            var temp = _dataLayer.stat();
            return temp;
        }



        [HttpPost]
        [Route("api/search/DatabaseUpdate")]
        public async Task<HttpResponseMessage> DatabaseUpdateAsync()
        {
            string StatusMessage = "";
            try
            {

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                var root = HttpContext.Current.Server.MapPath("~/App_Data");
                var provider = new CustomMultipartFormDataStreamProvider(root);

                await Request.Content.ReadAsMultipartAsync(provider);

                var jsonData = provider.FormData.GetValues("Jsonfile");
                var DbUpdateInfo = JsonConvert.DeserializeObject<AdminPanelDto>(jsonData[0].Trim('"'));
                
                FastaReader _FastaReader = new FastaReader();
                string DatabaseName = DbUpdateInfo.UpdateDatabase;  // + "Ecoli2";  //Add here fasta File Name
                string FastaFilePath = provider.FileData[0].LocalFileName;   // @"C:\Users\Administrator\Desktop\";  // Add here fasta file location
                StatusMessage = _FastaReader.MainFastaReader(DatabaseName, FastaFilePath);

            }
            catch(Exception e)   //DbEntityValidationException
            {
                //var _DBErrorException = new DBErrorException();
                //_DBErrorException.DbEntitiyError(e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }



            string response = StatusMessage;
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [HttpPost]
        [Route("api/search/DatabaseDownload")]
        public ResultsDownloadDto DatabaseDownload([FromBody] string DatabaseName)
        {

            var ListOfFileBlobs = new List<byte[]>();
            //string DatabaseName = "Ecoli";  //Add here fasta File Name
            string FastaFilePath = @"D:\10_PERCEPTRON_Live\ftproot\DownloadDatabase\";  // Add here fasta file location
            FastaWriter _FastaWriter = new FastaWriter();
            string PartialName = "DownloadedDatabase_";
            string FullFileName = FastaFilePath + PartialName + DatabaseName + ".fasta";
            string FileName = PartialName + DatabaseName + ".fasta";
            _FastaWriter.MainFastaWriter(DatabaseName, FullFileName);

            using (FileStream fileStream = File.OpenRead(FullFileName))
            {
                byte[] blob = new byte[fileStream.Length];
                fileStream.Read(blob, 0, (int)fileStream.Length);

                //return blob;
                ListOfFileBlobs.Add(blob);
            }
            var DbDownloadData = new ResultsDownloadDto(FileName, ListOfFileBlobs);
            return DbDownloadData;
        }

        [HttpPost]
        [Route("api/search/bug_form")]
        public async Task<HttpResponseMessage> bug_form()
        {
            var queryId = Guid.NewGuid().ToString();
            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new CustomMultipartFormDataStreamProvider(root);
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                var jsonData = provider.FormData.GetValues("Jsonfile");

                if (jsonData != null)
                {
                    string json = JsonConvert.SerializeObject(jsonData).Replace(@"\", string.Empty);
                    System.IO.File.WriteAllText(@"C:\inetpub\wwwroot\assets\bug_form\" + queryId + ".txt", json);
                }

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK, '1');
        }

    }
}