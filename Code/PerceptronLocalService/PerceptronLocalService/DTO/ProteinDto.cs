using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class ProteinDto
    {
        public string Header;
        public string OriginalSequence;
        public string Sequence;
        public double PstScore;
        public double PtmScore;
        public double Score;
        public double MwScore;
        public double Mw;
        public List<PostTranslationModificationsSiteDto> PtmParticulars;
        public InsilicoObjectDto InsilicoDetails;

        public int TruncationIndex;
        public string Truncation;
        public string TruncatedSequence;
        public double TruncatedMolecaularWeight;
        public string TerminalModification;

        //Insilico Scoring
        public double InsilicoScore;
        public int MatchCounter;
        public List<int> LeftMatchedIndex;
        public List<int> RightMatchedIndex;
        public List<int> LeftPeakIndex;
        public List<int> RightPeakIndex;
        public List<string> LeftType;
        public List<string> RightType;
        public double Evalue;

        public ProteinDto()
        {
            Header = "";
            OriginalSequence = "";
            Sequence = "";
            PstScore = 0;
            InsilicoScore = 0;
            PtmScore = 0;
            Score = 0;
            MwScore = 0;
            PtmParticulars = new List<PostTranslationModificationsSiteDto>();
            InsilicoDetails = new InsilicoObjectDto();

            MatchCounter = 0;
            Truncation = "None";
            TruncationIndex = -1;
            TruncatedSequence = "";
            TruncatedMolecaularWeight = 0;
            TerminalModification = "";
        }

        public ProteinDto(string h, string s, double mw, double mwScore)
        {
            Header = h;
            OriginalSequence = s;
            Sequence = s;
            PstScore = 0;
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
            Score = (pstSweight * PstScore + insilicoSweight * InsilicoScore + mwSweight * MwScore) / (mwSweight + pstSweight + insilicoSweight);
        }
        //public void set_score(double mwSweight, double pstSweight, double insilicoSweight)
        //{
        //    Score = (pstSweight * PstScore / 100 + insilicoSweight * InsilicoScore / 100 + mwSweight * MwScore / 100) / 3.0;
        //}
    }
}
