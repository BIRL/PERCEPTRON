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
        public InsilicoObjectDto InsilicoDetails = new InsilicoObjectDto();


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

            InsilicoDetails.InsilicoMassLeft.AddRange(protein.InsilicoDetails.InsilicoMassLeft);
            InsilicoDetails.InsilicoMassRight.AddRange(protein.InsilicoDetails.InsilicoMassRight);
            InsilicoDetails.InsilicoMassLeftAo.AddRange(protein.InsilicoDetails.InsilicoMassLeftAo);
            InsilicoDetails.InsilicoMassLeftBo.AddRange(protein.InsilicoDetails.InsilicoMassLeftBo);
            InsilicoDetails.InsilicoMassLeftAstar.AddRange(protein.InsilicoDetails.InsilicoMassLeftAstar);
            InsilicoDetails.InsilicoMassRightYo.AddRange(protein.InsilicoDetails.InsilicoMassRightYo);
            InsilicoDetails.InsilicoMassRightYstar.AddRange(protein.InsilicoDetails.InsilicoMassRightYstar);
            InsilicoDetails.InsilicoMassRightZo.AddRange(protein.InsilicoDetails.InsilicoMassRightZo);
            InsilicoDetails.InsilicoMassRightZoo.AddRange(protein.InsilicoDetails.InsilicoMassRightZoo);

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

//public ProteinDto(ProteinDto protein) // Not Useful constructor  contains Reference based of Insilico DTO
//{
//    Header = protein.Header;
//    OriginalSequence = protein.OriginalSequence;
//    Sequence = protein.Sequence;
//    PstScore = protein.PstScore;
//    InsilicoScore = protein.InsilicoScore;
//    PtmScore = protein.PtmScore;
//    Score = protein.Score;
//    MwScore = protein.MwScore;
//    Mw = protein.Mw;


//    PtmParticulars = new List<PostTranslationModificationsSiteDto>();

//    PtmParticulars.AddRange(protein.PtmParticulars);

//    BlindPtmLocalizationInfo = new BlindPtmInfo(protein.BlindPtmLocalizationInfo.Start,
//    protein.BlindPtmLocalizationInfo.End, protein.BlindPtmLocalizationInfo.Mass);

//    InsilicoDetails = new InsilicoObjectDto(protein.InsilicoDetails);

//    TruncationIndex = protein.TruncationIndex;
//    TerminalModification = protein.TerminalModification;
//    Truncation = protein.Truncation;
//    TruncatedSequence = protein.TruncatedSequence;
//    TruncatedMolecaularWeight = protein.TruncatedMolecaularWeight;
//    InsilicoScore = protein.InsilicoScore;
//    MatchCounter = protein.MatchCounter;

//    LeftMatchedIndex = protein.LeftMatchedIndex;
//    RightMatchedIndex = protein.RightMatchedIndex;
//    LeftPeakIndex = protein.LeftPeakIndex;
//    RightPeakIndex = protein.RightPeakIndex;
//    LeftType = protein.LeftType;
//    RightType = protein.RightType;

//    PstTagsWithComma = protein.PstTagsWithComma;

//}




//InsilicoDetails = new InsilicoObjectDto
//{
//    InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft,
//    InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight,
//    InsilicoMassLeftAo = protein.InsilicoDetails.InsilicoMassLeftAo,
//    InsilicoMassLeftBo = protein.InsilicoDetails.InsilicoMassLeftBo,
//    InsilicoMassLeftAstar = protein.InsilicoDetails.InsilicoMassLeftAstar,
//    InsilicoMassLeftBstar = protein.InsilicoDetails.InsilicoMassLeftBstar,
//    InsilicoMassRightYo = protein.InsilicoDetails.InsilicoMassRightYo,
//    InsilicoMassRightYstar = protein.InsilicoDetails.InsilicoMassRightYstar,
//    InsilicoMassRightZo = protein.InsilicoDetails.InsilicoMassRightZo,
//    InsilicoMassRightZoo = protein.InsilicoDetails.InsilicoMassRightZoo,

//};

//var temp = protein.InsilicoDetails;

//var tempLeft = new List<double>();
//tempLeft.AddRange(temp.InsilicoMassLeft);
//temp.InsilicoMassLeft = tempLeft;

//var tempRight = new List<double>();
//tempRight.AddRange(temp.InsilicoMassRight);
//temp.InsilicoMassRight = tempRight;

//var tempInsilicoMassLeftAo = new List<double>();
//tempInsilicoMassLeftAo.AddRange(temp.InsilicoMassLeftAo);
//temp.InsilicoMassLeftAo = tempInsilicoMassLeftAo;

//var tempInsilicoMassLeftBo = new List<double>();
//tempInsilicoMassLeftBo.AddRange(temp.InsilicoMassLeftBo);
//temp.InsilicoMassLeftBo = tempInsilicoMassLeftBo;

//var tempInsilicoMassLeftAstar = new List<double>();
//tempInsilicoMassLeftAstar.AddRange(temp.InsilicoMassLeftAstar);
//temp.InsilicoMassLeftAstar = tempInsilicoMassLeftAstar;


//var tempInsilicoMassLeftBstar = new List<double>();
//tempInsilicoMassLeftBstar.AddRange(temp.InsilicoMassLeftBstar);
//temp.InsilicoMassLeftBstar = tempInsilicoMassLeftBstar;

//var tempInsilicoMassRightYo = new List<double>();
//tempInsilicoMassRightYo.AddRange(temp.InsilicoMassRightYo);
//temp.InsilicoMassRightYo = tempInsilicoMassRightYo;

//var tempInsilicoMassRightYstar = new List<double>();
//tempInsilicoMassRightYstar.AddRange(temp.InsilicoMassRightYstar);
//temp.InsilicoMassRightYstar = tempInsilicoMassRightYstar;

//var tempInsilicoMassRightZo = new List<double>();
//tempInsilicoMassRightZo.AddRange(temp.InsilicoMassRightZo);
//temp.InsilicoMassRightZo = tempInsilicoMassRightZo;

//var tempInsilicoMassRightZoo = new List<double>();
//tempInsilicoMassRightZoo.AddRange(temp.InsilicoMassRightZoo);
//temp.InsilicoMassRightZoo = tempInsilicoMassRightZoo;
