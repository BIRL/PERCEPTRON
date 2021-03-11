using System;
using System.Collections.Generic;
using PerceptronLocalService.Utility;
using System.Diagnostics;

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


        public ProteinDto(ProteinDto protein)   //Simple    //...................WITH NEW ProteinDto
        {
            Header = protein.Header;
            OriginalSequence = protein.OriginalSequence;
            Sequence = protein.Sequence;
            PstScore = protein.PstScore;
            InsilicoScore = protein.InsilicoScore;
            PtmScore = protein.PtmScore;
            Score = protein.Score;
            MwScore = protein.MwScore;
            Mw = protein.Mw;


            PtmParticulars = new List<PostTranslationModificationsSiteDto>();

            PtmParticulars.AddRange(protein.PtmParticulars);

            BlindPtmLocalizationInfo = new BlindPtmInfo(protein.BlindPtmLocalizationInfo.Start,
            protein.BlindPtmLocalizationInfo.End, protein.BlindPtmLocalizationInfo.Mass);


            InsilicoDetails = new InsilicoObjectDto(protein.InsilicoDetails);

            TruncationIndex = protein.TruncationIndex;
            TerminalModification = protein.TerminalModification;
            Truncation = protein.Truncation;
            TruncatedSequence = protein.TruncatedSequence;
            TruncatedMolecaularWeight = protein.TruncatedMolecaularWeight;
            InsilicoScore = protein.InsilicoScore;
            MatchCounter = protein.MatchCounter;

            LeftMatchedIndex = protein.LeftMatchedIndex;
            RightMatchedIndex = protein.RightMatchedIndex;
            LeftPeakIndex = protein.LeftPeakIndex;
            RightPeakIndex = protein.RightPeakIndex;
            LeftType = protein.LeftType;
            RightType = protein.RightType;

            PstTagsWithComma = protein.PstTagsWithComma;

        }

        

        public static ProteinDto GetCopy(ProteinDto protein)
        {
            return Clone.DeepClone<ProteinDto>(protein);
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


        public void set_score(double mwSweight, double pstSweight, double insilicoSweight)   // Should be Discarded One Code Clean
        {
            Score = ((mwSweight * MwScore) + (pstSweight * PstScore) + (insilicoSweight * InsilicoScore)) / (mwSweight + pstSweight + insilicoSweight); //3.0;

        }
    }
}