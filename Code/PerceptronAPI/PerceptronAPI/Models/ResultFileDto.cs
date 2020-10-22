using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class ResultFileDto
    {
        public List<ProteinDto> TopProteinOfResultFile = new List<ProteinDto>();
        public string ResultFileName;

        public ResultFileDto(List<ProteinDto> cTopProteinOfResultFile, string cResultFileName)
        {
            TopProteinOfResultFile = cTopProteinOfResultFile;
            ResultFileName = cResultFileName;
        }
   }
}