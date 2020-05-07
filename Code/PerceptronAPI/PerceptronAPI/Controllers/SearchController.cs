using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using PerceptronAPI.Models;
using PerceptronAPI.Repository;
using PerceptronAPI.ServiceLayer;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Net.Mail;

namespace PerceptronAPI.Controllers
{
   public class SearchController : ApiController
    {

        readonly IDataAccessLayer _dataLayer;

        public SearchController()
        {
            _dataLayer = new SqlDatabase();
        }
        public string Get_progress(string em)
        {
            return Search.Progress_reporter(em);
        }

        [HttpPost]
        [Route("api/search/File_upload")]
        public async Task<HttpResponseMessage> File_upload()
        {
            var queryId = Guid.NewGuid().ToString();

            var a = HttpContext.Current.Response.Cookies.Count;

            //var creationTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);  // Updated
            DateTime time = DateTime.Now;             // Fetching Current Time
            string format = "yyyy/MM/dd HH:mm:ss";
            var creationTime = time.ToString(format); // Formating creationTime and assigning
            const string progress = "0";


            var parametersDto = new SearchParametersDto
            {
                SearchFiles = new List<SearchFile>(),
                SearchQuerry = new SearchQuery(),
                FixedMods = new List<PtmFixedModification>(),
                VarMods = new List<PtmVariableModification>()
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
                    //ErrorInfo = JsonConvert.DeserializeObject<BasicJobInfo>(jsonData[0].Trim('"'));
                    parametersDto.SearchParameters = JsonConvert.DeserializeObject<SearchParameter>(jsonData[0].Trim('"'));
                    parametersDto.SearchQuerry = JsonConvert.DeserializeObject<SearchQuery>(jsonData[0].Trim('"'));
                }

                //parametersDto.SearchParameters.DenovoAllow = 1;
                //parametersDto.SearchParameters.PtmAllow = 1;
                //parametersDto.SearchParameters.FilterDb = 1;
                

                parametersDto.SearchParameters.QueryId = queryId;
                parametersDto.SearchQuerry.QueryId = parametersDto.SearchParameters.QueryId;
                parametersDto.SearchQuerry.Progress = progress;
                parametersDto.SearchQuerry.CreationTime = creationTime;
                parametersDto.SearchQuerry.UserId = parametersDto.SearchParameters.UserId;

                foreach (var file in provider.FileData)
                {
                    i++;
                    var x = new SearchFile
                    {
                        FileId = i,
                        FileName = file.LocalFileName,
                        FileType = System.IO.Path.GetExtension(file.LocalFileName),
                        QueryId = queryId
                    };
                    parametersDto.SearchFiles.Add(x);
                }
                var response = Search.ProteinSearch(parametersDto);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception e)
            {
                if (parametersDto.SearchParameters.EmailId != null || creationTime != "")
                {
                    Sending_Email(parametersDto, creationTime);
                }
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
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
                var networkCred = new NetworkCredential("perceptron@lums.edu.pk", "*****");
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