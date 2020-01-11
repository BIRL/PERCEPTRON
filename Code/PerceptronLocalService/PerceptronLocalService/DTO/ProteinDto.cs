using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class ProteinDto
    {
        public string Header;
        public string Sequence;
        public double PstScore;
        public double InsilicoScore;
        public double PtmScore;
        public double Score;
        public double MwScore;
        public double Mw;
        public List<PostTranslationModificationsSiteDto> PtmParticulars;
        public InsilicoObjectDto InsilicoDetails;

        public double MatchesScore;   // Change My Name 
        public int Match;             // Change My Name

        public ProteinDto()
        {
            Header = "";
            Sequence = "";
            PstScore = 0;
            InsilicoScore = 0;
            PtmScore = 0;
            Score = 0;
            MwScore = 0;
            PtmParticulars = new List<PostTranslationModificationsSiteDto>();
            InsilicoDetails = new InsilicoObjectDto();

            MatchesScore = 0.0;
            Match = 0;
        }

        public ProteinDto(string h, string s, double mw, double mwScore)
        {
            Header = h;
            Sequence = s;
            PstScore = 0;
            MwScore = mwScore;
            InsilicoScore = 0;
            PtmScore = 0;
            Score = 0;
            Mw = mw;
            //PtmParticulars = new List<Sites>();

            MatchesScore = 0.0;
            Match = 0;

        }

        public string GetSequence()
        {
            return Sequence;
        }


        public void set_score(double mwSweight, double pstSweight, double insilicoSweight)
        {
            Score = (pstSweight * PstScore / 100 + insilicoSweight * InsilicoScore / 100 + mwSweight * MwScore / 100) / 3.0;
        }
    }
}
