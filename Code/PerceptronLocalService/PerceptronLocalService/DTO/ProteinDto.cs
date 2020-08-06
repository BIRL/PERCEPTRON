using System.Collections.Generic;
using PerceptronLocalService.Utility;

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
        public BlindPtmInfo BlindPtmLocalizationInfo;
        public InsilicoObjectDto InsilicoDetails;


        public int TruncationIndex;
        public string Truncation; //20200126
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
        public int ProteinRank;
        public string PstTagsWithComma;

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
            Mw = 0; // 20200122
            PtmParticulars = new List<PostTranslationModificationsSiteDto>();
            BlindPtmLocalizationInfo = new BlindPtmInfo();
            InsilicoDetails = new InsilicoObjectDto();

            MatchCounter = 0;
            Truncation = "None";  //20200126
            TruncationIndex = -1;
            TruncatedSequence = "";
            TruncatedMolecaularWeight = 0;
            TerminalModification = "None";

            PstTagsWithComma = "";

        }

        public ProteinDto(ProteinDto protein) 
        {
            var proteinInfoClone = Clone.CloneObject(protein); // Lists are referenced based so, therefore...
            ProteinDto ProteinClone = Clone.Decrypt<ProteinDto>(proteinInfoClone); // Lists are referenced based so, therefore...

            Header = ProteinClone.Header;
            OriginalSequence = ProteinClone.OriginalSequence;
            Sequence = ProteinClone.Sequence;
            PstScore = ProteinClone.PstScore;
            InsilicoScore = ProteinClone.InsilicoScore;
            PtmScore = ProteinClone.PtmScore;
            Score = ProteinClone.Score;
            MwScore = ProteinClone.MwScore;
            Mw = ProteinClone.Mw;
            PtmParticulars = new List<PostTranslationModificationsSiteDto>(ProteinClone.PtmParticulars);
            BlindPtmLocalizationInfo = ProteinClone.BlindPtmLocalizationInfo;
            InsilicoDetails = new InsilicoObjectDto(ProteinClone.InsilicoDetails);

            TruncationIndex = ProteinClone.TruncationIndex;
            TerminalModification = ProteinClone.TerminalModification;
            Truncation = ProteinClone.Truncation;
            TruncatedSequence = ProteinClone.TruncatedSequence;
            TruncatedMolecaularWeight = ProteinClone.TruncatedMolecaularWeight;
            InsilicoScore = ProteinClone.InsilicoScore;
            MatchCounter = ProteinClone.MatchCounter;

            LeftMatchedIndex = ProteinClone.LeftMatchedIndex;
            RightMatchedIndex = ProteinClone.RightMatchedIndex;
            LeftPeakIndex = ProteinClone.LeftPeakIndex;
            RightPeakIndex = ProteinClone.RightPeakIndex;
            LeftType = ProteinClone.LeftType;
            RightType = ProteinClone.RightType;

            PstTagsWithComma = ProteinClone.PstTagsWithComma;

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
            Score = ((mwSweight * MwScore) + (pstSweight * PstScore) + (insilicoSweight * InsilicoScore)) / (mwSweight + pstSweight + insilicoSweight); //3.0;

            //Score = (pstSweight * PstScore + insilicoSweight * InsilicoScore + mwSweight * MwScore) / (mwSweight + pstSweight + insilicoSweight);
        }
        //public void set_score(double mwSweight, double pstSweight, double insilicoSweight)
        //{
        //    Score = (pstSweight * PstScore / 100 + insilicoSweight * InsilicoScore / 100 + mwSweight * MwScore / 100) / 3.0;
        //}
    }
}
