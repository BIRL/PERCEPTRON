using System.Collections.Generic;

namespace PerceptronAPI.Models
{
    public class Proteins
    {
        public string Header;
        public string Sequence;
        public double EstScore;
        public double InsilicoScore;
        public double PtmScore;
        public double Score;
        public double MwScore;
        public double Mw;
        public List<Sites> PtmParticulars;
        public InsilicoObj InsilicoDetails;

        public Proteins()
        {
            Header = "";
            Sequence = "";
            EstScore = 0;
            InsilicoScore = 0;
            PtmScore = 0;
            Score = 0;
            MwScore = 0;
            PtmParticulars = new List<Sites>();
            InsilicoDetails = new InsilicoObj();
        }

        public Proteins(string h, string s, double mw, double mwScore)
        {
            Header = h;
            Sequence = s;
            EstScore = 0;
            MwScore = mwScore;
            InsilicoScore = 0;
            PtmScore = 0;
            Score = 0;
            Mw = mw;
            //PtmParticulars = new List<Sites>();
        }

        public string GetSequence()
        {
            return Sequence;
        }


        public void set_score(double mwSweight, double pstSweight, double insilicoSweight)
        {
            Score = (pstSweight * EstScore + insilicoSweight * InsilicoScore + mwSweight * MwScore) / 3.0;
        }
    }
}