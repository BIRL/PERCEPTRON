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

                foreach (var file in provider.FileData)
                {
                    var FileUniqueId = Guid.NewGuid().ToString();
                    string FileNameWithUniqueID = _AddSuffixInName.AddSuffix(file.LocalFileName, "-ID-" + FileUniqueId); //Updated: To avoid file replacement due to same filenames
                    System.IO.File.Move(file.LocalFileName, FileNameWithUniqueID); // Renaming "user's input data file" with "user's input data file + Unique ID (FileUniqueId)"

                    i++;
                    var x = new SearchFile
                    {
                        FileId = i,
                        FileName = file.LocalFileName,
                        UniqueFileName = FileNameWithUniqueID,
                        FileType = System.IO.Path.GetExtension(file.LocalFileName),
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

        //[HttpPost]
        //[Route("api/search/Calling_API")]
        //public async Task<HttpResponseMessage> Calling_API_Processing()
        //{
        //    var queryId = Guid.NewGuid().ToString();

        //    var a = HttpContext.Current.Response.Cookies.Count;

        //    DateTime time = DateTime.Now;             // Fetching Current Time
        //    string format = "yyyy/MM/dd HH:mm:ss";
        //    var creationTime = time.ToString(format); // Formating creationTime and assigning
        //    const string progress = "0";


        //    var parametersDto = new SearchParametersDto
        //    {
        //        SearchFiles = new List<SearchFile>(),
        //        SearchQuerry = new SearchQuery(),
        //        FixedMods = new PtmFixedModification(),
        //        VarMods = new PtmVariableModification()
        //    };
        //    // Check if the request contains multipart/form-data.
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }

        //    var root = HttpContext.Current.Server.MapPath("~/App_Data");
        //    var provider = new CustomMultipartFormDataStreamProvider(root);

        //    try
        //    {
        //        var i = 0;
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        var jsonData = provider.FormData.GetValues("Jsonfile");

        //        if (jsonData != null)
        //        {
        //            parametersDto.SearchParameters = JsonConvert.DeserializeObject<SearchParameter>(jsonData[0].Trim('"'));
        //            parametersDto.SearchQuerry = JsonConvert.DeserializeObject<SearchQuery>(jsonData[0].Trim('"'));
        //            parametersDto.FixedMods = JsonConvert.DeserializeObject<PtmFixedModification>(jsonData[0].Trim('"'));
        //            parametersDto.VarMods = JsonConvert.DeserializeObject<PtmVariableModification>(jsonData[0].Trim('"'));
        //        }


        //        if (parametersDto.FixedMods.FixedModifications != "")
        //        {
        //            parametersDto.FixedMods.QueryId = queryId;
        //            parametersDto.FixedMods.ModificationId = 1;
        //        }

        //        if (parametersDto.VarMods.VariableModifications != "")
        //        {
        //            parametersDto.VarMods.QueryId = queryId;
        //            parametersDto.VarMods.ModificationId = 1;
        //        }

        //        parametersDto.SearchParameters.QueryId = queryId;
        //        parametersDto.SearchQuerry.QueryId = parametersDto.SearchParameters.QueryId;
        //        parametersDto.SearchQuerry.Progress = progress;
        //        parametersDto.SearchQuerry.CreationTime = creationTime;
        //        parametersDto.SearchQuerry.UserId = parametersDto.SearchParameters.UserId;

        //        foreach (var file in provider.FileData)
        //        {
        //            var FileUniqueId = Guid.NewGuid().ToString();
        //            string FileNameWithUniqueID = AddSuffix(file.LocalFileName, "-ID-" + FileUniqueId); //Updated: To avoid file replacement due to same filenames
        //            System.IO.File.Move(file.LocalFileName, FileNameWithUniqueID); // Renaming "user's input data file" with "user's input data file + Unique ID (FileUniqueId)"

        //            i++;
        //            var x = new SearchFile
        //            {
        //                FileId = i,
        //                FileName = file.LocalFileName,
        //                UniqueFileName = FileNameWithUniqueID,
        //                FileType = System.IO.Path.GetExtension(file.LocalFileName),
        //                QueryId = queryId,
        //                FileUniqueId = FileUniqueId
        //            };
        //            parametersDto.SearchFiles.Add(x);
        //        }
        //        var response = Search.ProteinSearch(parametersDto);
        //        return Request.CreateResponse(HttpStatusCode.OK, response);
        //    }
        //    catch (Exception e)
        //    {
        //        if (parametersDto.SearchParameters.EmailId != "")
        //        {
        //            Sending_Email(parametersDto, creationTime);
        //        }
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        //    }
        //}




        [HttpPost]
        [Route("api/search/ResultsDownload")]
        public ResultsDownloadDto ResultsDownload([FromBody] string input)
        {
            List<byte[]> ListOfFileBlobs = new List<byte[]>();
            var ScanData = _dataLayer.ScanResultsDownloadData(input);    //Scanning File Unique Ids from SearchFiles Table

            string filePath = @"C:\\PerceptronApi-tempResultsFolder\\";
            string ZipFileName = filePath + "Results_" + input + ".zip";

            WriteResultsFile _WriteResultsFile = new WriteResultsFile();
            var AllResultFilesNames = _WriteResultsFile.ResultFilesWrite(ScanData, filePath);   // Writing: All Results files 

            string fullfilename = "";

            if (File.Exists(ZipFileName))
                File.Delete(ZipFileName); //Deleted Pre-existing file

            using (var archieve = ZipFile.Open(ZipFileName, ZipArchiveMode.Create)) // Creating Zip File
            {
                for (int i = 0; i < AllResultFilesNames.Count; i++)
                {
                    fullfilename = filePath + AllResultFilesNames[i];
                    archieve.CreateEntryFromFile(fullfilename, Path.GetFileName(fullfilename));   // Adding all results files into the zip file
                }
            }

            using (FileStream fileStream = File.OpenRead(ZipFileName))
            {
                byte[] blob = new byte[fileStream.Length];
                fileStream.Read(blob, 0, (int)fileStream.Length);
                ListOfFileBlobs.Add(blob);
            }

            var ResultsDownloadData = new ResultsDownloadDto(ZipFileName, ListOfFileBlobs);
            return ResultsDownloadData;
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
            //string[] values = input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //var qid = values[0];
            //var resultid = values[1];
            var temp = _dataLayer.Detailed_Results("1", input);

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

        //[HttpPost]
        //[Route("api/search/FASTA_File_upload")]  //    
        //public async Task<HttpResponseMessage> FASTA_File_upload()

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