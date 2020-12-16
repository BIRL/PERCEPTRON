using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PerceptronLocalService.DTO
{
    public class ResultsDownloadToBeWrite
    {
        public string FileName;
        public List<ProteinDto> CandidateList;
        public ProteinDto Protein;
        public string Time;

        public ResultsDownloadToBeWrite()
        {
            FileName = "";
            CandidateList = new List<ProteinDto>();
        }

        public ResultsDownloadToBeWrite(string cFileName, List<ProteinDto> cCandidateList)
        {
            FileName = cFileName;
            CandidateList = cCandidateList;
        }


        public ResultsDownloadToBeWrite(string cFileName, ProteinDto cProtein, string cTime)
        {
            FileName = cFileName;
            Protein = cProtein;
            Time = cTime;
        }

    }
}
