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
            var creationTime = DateTime.Now.ToString(CultureInfo.InvariantCulture);
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
                    parametersDto.SearchParameters = JsonConvert.DeserializeObject<SearchParameter>(jsonData[0].Trim('"'));

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
                    string json = JsonConvert.SerializeObject(jsonData).Replace(@"\",string.Empty);
                    System.IO.File.WriteAllText(@"C:\inetpub\wwwroot\assets\bug_form\"+queryId+".txt", json);
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