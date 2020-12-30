using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net.Mail;
using System.IO;
using System.IO.Compression;
//using System.IO.Compression.FileSystem;
using PerceptronAPI.Engine;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using GraphForm;
using System.Web.UI.WebControls;
using PerceptronAPI.Models;
using PerceptronAPI.Repository;
using PerceptronAPI.Utility;


namespace PerceptronAPI.Controllers
{
    public class SearchController : ApiController
    {

        readonly IDataAccessLayer _dataLayer;

        public SearchController()
        {
            _dataLayer = new SqlDatabase();

            //var blob = Database_Download();
            //var Message = Database_Update();
            //Server.MapPath("~");
        }
        //public string Get_progress(string em)
        //{
        //    return Search.Progress_reporter(em);
        //}

        [HttpPost]
        [Route("api/search/File_upload")]
        public async Task<HttpResponseMessage> File_upload()
        {
            AddSuffixInName _AddSuffixInName = new AddSuffixInName();
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
                var i = 0;
                await Request.Content.ReadAsMultipartAsync(provider);

                var jsonData = provider.FormData.GetValues("Jsonfile");

                if (jsonData != null)
                {
                    parametersDto.SearchParameters = JsonConvert.DeserializeObject<SearchParameter>(jsonData[0].Trim('"'));
                    parametersDto.SearchQuerry = JsonConvert.DeserializeObject<SearchQuery>(jsonData[0].Trim('"'));
                    parametersDto.FixedMods = JsonConvert.DeserializeObject<PtmFixedModification>(jsonData[0].Trim('"'));
                    parametersDto.VarMods = JsonConvert.DeserializeObject<PtmVariableModification>(jsonData[0].Trim('"'));
                }


                if (parametersDto.FixedMods.FixedModifications != "")
                {
                    parametersDto.FixedMods.QueryId = queryId;
                    parametersDto.FixedMods.ModificationId = 1;
                }

                if (parametersDto.VarMods.VariableModifications != "")
                {
                    parametersDto.VarMods.QueryId = queryId;
                    parametersDto.VarMods.ModificationId = 1;
                }

                parametersDto.SearchParameters.QueryId = queryId;
                parametersDto.SearchQuerry.QueryId = parametersDto.SearchParameters.QueryId;
                parametersDto.SearchQuerry.Progress = progress;
                parametersDto.SearchQuerry.CreationTime = creationTime;
                parametersDto.SearchQuerry.UserId = parametersDto.SearchParameters.UserId;

                List<string> InputFileList = new List<string> { provider.FileData[0].LocalFileName };
                if (Path.GetExtension(InputFileList[0]) == ".zip") //Check If file is Zipped
                {
                    InputFileList = ZipFileUnzipping(InputFileList, parametersDto.SearchParameters, provider.FileData); //Unzipping the zipped file.
                }

                for (int index = 0; index < InputFileList.Count; index++)//foreach (var file in provider.FileData)
                {
                    string file = InputFileList[index];
                    var FileUniqueId = Guid.NewGuid().ToString();
                    string FileNameWithUniqueID = _AddSuffixInName.AddSuffix(file, "-ID-" + FileUniqueId); //Updated: To avoid file replacement due to same filenames
                    System.IO.File.Move(file, FileNameWithUniqueID); // Renaming "user's input data file" with "user's input data file + Unique ID (FileUniqueId)"

                    i++;
                    var x = new SearchFile
                    {
                        FileId = i,
                        FileName = file,
                        UniqueFileName = FileNameWithUniqueID,
                        FileType = System.IO.Path.GetExtension(file),
                        QueryId = queryId,
                        FileUniqueId = FileUniqueId
                    };
                    parametersDto.SearchFiles.Add(x);
                }
                var response = _dataLayer.StoreSearchParameters(parametersDto); //Search.ProteinSearch(parametersDto);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                if (parametersDto.SearchParameters.EmailId != "")
                {
                    Sending_Email(parametersDto, creationTime);
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        private List<string> ZipFileUnzipping(List<string> InputFileList, SearchParameter parameters, Collection<MultipartFileData> ProviderFileData)
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

        [HttpPost]
        [Route("api/search/FDR_Data_upload")]
        public string FDR_Data_upload([FromBody] string input)
        {
            var _FDR = new FDR();
            _FDR.MainFDR();

            return "ABC";
        }


        [HttpPost]
        [Route("api/search/Calling_API")]
        public async Task<HttpResponseMessage> Calling_API(HttpRequestMessage request)
        {

            try
            {
                var queryId = Guid.NewGuid().ToString();
                var result = await request.Content.ReadAsStringAsync();
                
                string[] ParameterValues = result.Split(":".ToCharArray());

                ParametersProcessing(queryId, ParameterValues);
                


            }
            catch (Exception e)
            { 
                int fsadsa = 1;
            }

            var response = "";
            return Request.CreateResponse(HttpStatusCode.OK, response);

        }
        public void ParametersProcessing(string queryId, string[] ParameterValues)
        {
            var parametersDto = new SearchParametersDto
            {
                SearchFiles = new List<SearchFile>(),
                SearchQuerry = new SearchQuery(),
                FixedMods = new PtmFixedModification(),
                VarMods = new PtmVariableModification()
            };

            parametersDto.SearchParameters.QueryId = queryId;
            parametersDto.SearchParameters.Title = ParameterValues[0];
            parametersDto.SearchParameters.FDRCutOff = ParameterValues[1];
            parametersDto.SearchParameters.ProteinDatabase = ParameterValues[2];
            parametersDto.SearchParameters.MassMode = ParameterValues[3];


            parametersDto.SearchParameters.FilterDb = ParameterValues[4];
            parametersDto.SearchParameters.MwTolerance = Convert.ToDouble(ParameterValues[5]);
            parametersDto.SearchParameters.PeptideTolerance = Convert.ToDouble(ParameterValues[6]);
            parametersDto.SearchParameters.PeptideToleranceUnit = ParameterValues[7];
            parametersDto.SearchParameters.Autotune = ParameterValues[8];
            parametersDto.SearchParameters.InsilicoFragType = ParameterValues[9];
            parametersDto.SearchParameters.HandleIons = ParameterValues[10];
            parametersDto.SearchParameters.DenovoAllow = ParameterValues[11];

            parametersDto.SearchParameters.MinimumPstLength = Convert.ToInt16(ParameterValues[12]);
            parametersDto.SearchParameters.MaximumPstLength = Convert.ToInt16(ParameterValues[13]);
            parametersDto.SearchParameters.HopThreshhold = Convert.ToDouble(ParameterValues[14]);

            parametersDto.SearchParameters.PSTTolerance = Convert.ToDouble(ParameterValues[16]);
            parametersDto.SearchParameters.Truncation = ParameterValues[17];
            parametersDto.SearchParameters.TerminalModification = ParameterValues[18];
            parametersDto.SearchParameters.PtmAllow = ParameterValues[19];


            parametersDto.SearchParameters.PtmTolerance = Convert.ToDouble(ParameterValues[20]);
            parametersDto.SearchParameters.MethionineChemicalModification = ParameterValues[23];
            parametersDto.SearchParameters.CysteineChemicalModification = ParameterValues[24];
            parametersDto.SearchParameters.MwSweight = Convert.ToDouble(ParameterValues[25]);

            parametersDto.SearchParameters.PstSweight = Convert.ToDouble(ParameterValues[26]);
            parametersDto.SearchParameters.InsilicoSweight = Convert.ToDouble(ParameterValues[27]);
            parametersDto.SearchParameters.EmailId = ParameterValues[28];
            parametersDto.SearchParameters.UserId = ParameterValues[29];


            if (ParameterValues[21] != "")
            {
                parametersDto.FixedMods.QueryId = queryId;
                parametersDto.FixedMods.ModificationId = 1;
                parametersDto.FixedMods.FixedModifications = ParameterValues[21];

            }

            if (ParameterValues[22] != "")
            {
                parametersDto.VarMods.QueryId = queryId;
                parametersDto.VarMods.ModificationId = 1;
                parametersDto.VarMods.VariableModifications = ParameterValues[22];

            }

        }



        [HttpPost]
        [Route("api/search/Calling_API2")]
        public async Task<HttpResponseMessage> Calling_API2()
        {
            AddSuffixInName _AddSuffixInName = new AddSuffixInName();
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
                new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new CustomMultipartFormDataStreamProvider(root);

            try
            {
                var i = 0;
                //await Request.Content.ReadAsMultipartAsync(provider);

                var jsonData = provider.FormData.GetValues("Jsonfile");

                if (jsonData != null)
                {
                    parametersDto.SearchParameters = JsonConvert.DeserializeObject<SearchParameter>(jsonData[0].Trim('"'));
                    parametersDto.SearchQuerry = JsonConvert.DeserializeObject<SearchQuery>(jsonData[0].Trim('"'));
                    parametersDto.FixedMods = JsonConvert.DeserializeObject<PtmFixedModification>(jsonData[0].Trim('"'));
                    parametersDto.VarMods = JsonConvert.DeserializeObject<PtmVariableModification>(jsonData[0].Trim('"'));
                }


                if (parametersDto.FixedMods.FixedModifications != "")
                {
                    parametersDto.FixedMods.QueryId = queryId;
                    parametersDto.FixedMods.ModificationId = 1;
                }

                if (parametersDto.VarMods.VariableModifications != "")
                {
                    parametersDto.VarMods.QueryId = queryId;
                    parametersDto.VarMods.ModificationId = 1;
                }

                parametersDto.SearchParameters.QueryId = queryId;
                parametersDto.SearchQuerry.QueryId = parametersDto.SearchParameters.QueryId;
                parametersDto.SearchQuerry.Progress = progress;
                parametersDto.SearchQuerry.CreationTime = creationTime;
                parametersDto.SearchQuerry.UserId = parametersDto.SearchParameters.UserId;

                List<string> InputFileList = new List<string> { provider.FileData[0].LocalFileName };
                if (Path.GetExtension(InputFileList[0]) == ".zip") //Check If file is Zipped
                {
                    InputFileList = ZipFileUnzipping(InputFileList, parametersDto.SearchParameters, provider.FileData); //Unzipping the zipped file.
                }

                for (int index = 0; index < InputFileList.Count; index++)//foreach (var file in provider.FileData)
                {
                    string file = InputFileList[index];
                    var FileUniqueId = Guid.NewGuid().ToString();
                    string FileNameWithUniqueID = _AddSuffixInName.AddSuffix(file, "-ID-" + FileUniqueId); //Updated: To avoid file replacement due to same filenames
                    System.IO.File.Move(file, FileNameWithUniqueID); // Renaming "user's input data file" with "user's input data file + Unique ID (FileUniqueId)"

                    i++;
                    var x = new SearchFile
                    {
                        FileId = i,
                        FileName = file,
                        UniqueFileName = FileNameWithUniqueID,
                        FileType = System.IO.Path.GetExtension(file),
                        QueryId = queryId,
                        FileUniqueId = FileUniqueId
                    };
                    parametersDto.SearchFiles.Add(x);
                }
                var response = _dataLayer.StoreSearchParameters(parametersDto); //Search.ProteinSearch(parametersDto);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                if (parametersDto.SearchParameters.EmailId != "")
                {
                    Sending_Email(parametersDto, creationTime);
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
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
            var temp = _dataLayer.Scan_Results(input);
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
            var temp = _dataLayer.Summary_results(qid, fileId);
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
            var temp = _dataLayer.Detailed_Results(qid, resultid);

            return temp;
        }

        [HttpPost]
        [Route("api/search/Post_DetailedProteinHitView_results")]
        public ResultsVisualizeData Post_DetailedProteinHitView_results([FromBody] string input)
        {
            Debug.WriteLine(input);
            //var temp = _dataLayer.DetailedProteinHitView_Results("1", input);


            //ITS A PART OF RESULTS VISUALIZATION ONCE COMPELTED WILL MOVE TO THIS (Post_DetailedProteinHitView_results) METHOD       /////NOW EMBEDDED INTO Post_detailed_results TO AVOID 

            DetailedProteinHitView temp2 = _dataLayer.DetailedProteinHitView_Results("1", input);

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
        [Route("api/search/Post_history")]
        public List<UserHistory> Post_history([FromBody] string input)
        {
            Debug.WriteLine(input);
            var temp = _dataLayer.GetUserHistory(input);
            return temp;
        }

        [HttpPost]
        [Route("api/search/stat")]
        public stat stat([FromBody] string input)
        {

            var temp = _dataLayer.stat();
            return temp;
        }

        [HttpGet]      //DEL ME //DEL ME  //DEL ME //DEL ME //DEL ME //DEL ME 
        [Route("api/search/statGet")]
        public stat statGet([FromBody] string input)
        {

            var temp = _dataLayer.stat();
            return temp;
        }



        [HttpPost]
        [Route("api/search/Database_Update")]
        public string Database_Update()
        {
            FastaReader _FastaReader = new FastaReader();
            string DatabaseName = "Ecoli";  //Add here fasta File Name
            string FastaFilePath = @"C:\Users\Administrator\Desktop\";  // Add here fasta file location
            string Message = _FastaReader.MainFastaReader(DatabaseName, FastaFilePath);
            return Message;
        }

        [HttpPost]
        [Route("api/search/Database_Download")]
        public byte[] Database_Download()
        {
            string DatabaseName = "Ecoli";  //Add here fasta File Name
            string FastaFilePath = @"C:\PerceptronApi-tempResultsFolder\";  // Add here fasta file location
            FastaWriter _FastaWriter = new FastaWriter();
            string FullFileName = FastaFilePath + "Download_" + DatabaseName + ".fasta";
            _FastaWriter.MainFastaWriter(DatabaseName, FullFileName);

            using (FileStream fileStream = File.OpenRead(FullFileName))
            {
                byte[] blob = new byte[fileStream.Length];
                fileStream.Read(blob, 0, (int)fileStream.Length);

                return blob;
            }
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



        public static void Sending_Email(SearchParametersDto p, string CreationTime)//, int EmailMsg)
        {
            var emailaddress = p.SearchParameters.EmailId;
            using (var mm = new MailMessage("perceptron@lums.edu.pk", emailaddress))
            {
                string BaseUrl = "https://perceptron.lums.edu.pk/";


                //if (EmailMsg == -1) // Email Msg for Something Wrong With Entered Query
                //{
                mm.Subject = "PERCEPTRON: Protein Search Results";
                var body = "Dear User,";
                body += "<br/><br/> Search couldn't complete for protein search query submitted at " + CreationTime + " with job title \"" +
                        p.SearchParameters.Title + "\" Please check your search parameters and data file.";
                //body += "&nbsp;<a href=\'" + BaseUrl + "/index.html#/scans/" + p.Queryid + " \'>link</a>.";
                body += "</br> If you need help check out the <a href=\'" + BaseUrl + "/index.html#/getting \'>Getting Started</a> guide and our <a href=\'https://www.youtube.com/playlist?list=PLaNVq-kFOn0Z_7b-iL59M_CeV06JxEXmA'>Video Tutorials</a>. If problem still persists, please <a href=\'" + BaseUrl + "/index.html#/contact'> contact</a> us.";

                body += "</br></br>Thank You for using Perceptron.";
                body += "</br><b>The PERCEPTRON Team</b>";
                body += "</br>Biomedical Informatics Research Laboratory (BIRL), Lahore University of Management Sciences (LUMS), Pakistan";
                mm.Body = body;
                //}  //I'M COMMENTED

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
    }
}