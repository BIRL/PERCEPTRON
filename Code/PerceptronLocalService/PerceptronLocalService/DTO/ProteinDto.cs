using System;
using System.Collections.Generic;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.DTO
{
    [Serializable]
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


        public ProteinDto(string cHeader, double cWholeProteinMass, string cSequence, List<double> InsilicoLeftIons, List<double> InsilicoRightIons)  // , string FastaHeader   ---Wait for FastaHeader
        {

            Header = cHeader;
            OriginalSequence = cSequence;
            Sequence = cSequence;
            PstScore = 0.0;
            InsilicoScore = 0.0;
            PtmScore = 0.0;
            Score = 0.0;
            MwScore = 0.0;
            Mw = cWholeProteinMass;
            PtmParticulars = new List<PostTranslationModificationsSiteDto>();
            BlindPtmLocalizationInfo = new BlindPtmInfo();
            InsilicoDetails = new InsilicoObjectDto(InsilicoLeftIons, InsilicoRightIons);




            Truncation = "None";  //20200126
            TruncationIndex = -1;
            TruncatedSequence = "";
            TruncatedMolecaularWeight = 0;
            TerminalModification = "None";

            InsilicoScore = 0.0;
            MatchCounter = 0;

            LeftMatchedIndex = new List<int>();
            RightMatchedIndex = new List<int>();
            LeftPeakIndex = new List<int>();
            RightPeakIndex = new List<int>();
            LeftType = new List<string>();
            RightType = new List<string>();

            PstTagsWithComma = "";
        }



        public ProteinDto(ProteinDto protein)             ///ITS HEALTHY..........!!!!!!!!
        {

            var ProteinClone = Clone.DeepClone<ProteinDto>(protein);


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


        public static ProteinDto GetCopy(ProteinDto ProteinClone)  //Updated 20210304
        {
            ProteinDto ProteinInfo = new ProteinDto();

            ProteinInfo.Header = ProteinClone.Header;
            ProteinInfo.OriginalSequence = ProteinClone.OriginalSequence;
            ProteinInfo.Sequence = ProteinClone.Sequence;
            ProteinInfo.PstScore = ProteinClone.PstScore;
            ProteinInfo.InsilicoScore = ProteinClone.InsilicoScore;
            ProteinInfo.PtmScore = ProteinClone.PtmScore;
            ProteinInfo.Score = ProteinClone.Score;
            ProteinInfo.MwScore = ProteinClone.MwScore;
            ProteinInfo.Mw = ProteinClone.Mw;
            ProteinInfo.PtmParticulars = new List<PostTranslationModificationsSiteDto>(ProteinClone.PtmParticulars);
            ProteinInfo.BlindPtmLocalizationInfo = new BlindPtmInfo(
                ProteinClone.BlindPtmLocalizationInfo.Start,
                ProteinClone.BlindPtmLocalizationInfo.End,
                ProteinClone.BlindPtmLocalizationInfo.Mass);

            ProteinInfo.InsilicoDetails = new InsilicoObjectDto(ProteinClone.InsilicoDetails);

            ProteinInfo.TruncationIndex = ProteinClone.TruncationIndex;
            ProteinInfo.TerminalModification = ProteinClone.TerminalModification;
            ProteinInfo.Truncation = ProteinClone.Truncation;
            ProteinInfo.TruncatedSequence = ProteinClone.TruncatedSequence;
            ProteinInfo.TruncatedMolecaularWeight = ProteinClone.TruncatedMolecaularWeight;
            ProteinInfo.InsilicoScore = ProteinClone.InsilicoScore;
            ProteinInfo.MatchCounter = ProteinClone.MatchCounter;

            ProteinInfo.LeftMatchedIndex = ProteinClone.LeftMatchedIndex;
            ProteinInfo.RightMatchedIndex = ProteinClone.RightMatchedIndex;
            ProteinInfo.LeftPeakIndex = ProteinClone.LeftPeakIndex;
            ProteinInfo.RightPeakIndex = ProteinClone.RightPeakIndex;
            ProteinInfo.LeftType = ProteinClone.LeftType;
            ProteinInfo.RightType = ProteinClone.RightType;

            ProteinInfo.PstTagsWithComma = ProteinClone.PstTagsWithComma;
            return ProteinInfo;
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


        // Should be Discarded One Code Clean   // Should be Discarded One Code Clean
        // Alternate of "set_score"  is in Perceptron.cs with name of  "AggregatedScoreByScoringComponents"
        public void set_score(double mwSweight, double pstSweight, double insilicoSweight)   // Should be Discarded One Code Clean
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