using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace PerceptronAPI.Controllers
{
    public class HomeController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("")]
        public HttpResponseMessage Index()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(
                    "<strong>Perceptron is working fine.</strong>",
                    Encoding.UTF8,
                    "text/html"
                )
            };
        }
    }
}