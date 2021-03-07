using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.Utility;
namespace PerceptronLocalService.DTO
{ //PstTagsDto.cs Code is not written by using Efficient Approach...
    public class PstTagsDto
    {
        public int startIndex;
        public int endIndex;
        public double startIndexMass;
        public double endIndexMass;
        public double massDifferenceBetweenPeaks;
        public string AminoAcidSymbol;
        public string AminoAcidName;
        public double TagError;
        public double averageIntensity;

        public PstTagsDto(int cstartIndex, int cendIndex, double cstartIndexMass, double cendIndexMass, double cmassDifferenceBetweenPeaks, string cAminoAcidSymbol, string cAminoAcidName, double cTagError, double caverageIntensity) //c for constructor
        //public PstTagsDto(string tag, int start, int end, List<double> tagErrors, List<double> tagIntensities, int tagLength = 1)
        {// c for constructor
            startIndex = cstartIndex;
            endIndex = cendIndex;
            startIndexMass = cstartIndexMass;
            endIndexMass = cendIndexMass;
            massDifferenceBetweenPeaks = cmassDifferenceBetweenPeaks;
            AminoAcidSymbol = cAminoAcidSymbol;
            AminoAcidName = cAminoAcidName;
            TagError = cTagError;
            averageIntensity = caverageIntensity;
        }

        public PstTagsDto(List<PstTagList> cPstTag) //c for constructor
        //public PstTagsDto(string tag, int start, int end, List<double> tagErrors, List<double> tagIntensities, int tagLength = 1)
        {// c for constructor
            var PstTag = Clone.DeepClone<List<PstTagList>>(cPstTag);
        }


    }
    public class Psts
    {
        public int psttaglength;
        public string psttags;
        public double PstTagErrorSum;
        public double rootmeansquareerror;
        public double PstTagIntensity;

        public Psts(int cpsttaglength, string cpsttags, double cPstTagErrorSum, double crootmeansquareerror, double cPstTagIntensity) //This constructor somtime sue to its skeleton... for General Values//c for constructor  
        {
            psttaglength = cpsttaglength;
            psttags = cpsttags;
            PstTagErrorSum = cPstTagErrorSum;
            rootmeansquareerror = crootmeansquareerror;
            PstTagIntensity = cPstTagIntensity;
        }
    }

    public class PstTagList
    {
        public int PstTagLength;
        public string PstTags;
        public double PstErrorScore;
        public double PstFrequency;



        public PstTagList(int cPstTagLength, string cPstTags, double cPstErrorScore, double cPstFrequency)
        {
            PstTagLength = cPstTagLength;
            PstTags = cPstTags;
            PstErrorScore = cPstErrorScore;
            PstFrequency = cPstFrequency;
        }

    }
}

    ///////////////////////////////////////// BBAD MA SMJHNA WALI CHEEZ K MANTA KAISA HOGA AGR NAMES SAME NI HAIN TO...!!!!
//    public class PstTagsDtoGpu
//    {
//        public string AminoAcidTag;
//        public int StartPosition;
//        public int EndPosition;
//        public int TagLength;
//        public double LengthScore;
//        public List<double> ErrorSquaredList;
//        public double ErrorScore;
//        public List<double> IntensityList;
//        public double FrequencyScore;
//        public List<int> TagPositions;

//        public PstTagsDtoGpu(string tag, double sumSquaredError, double sumIntensity)
//        {
//            AminoAcidTag = tag;
//            TagLength = tag.Length;

//            LengthScore = Math.Pow(AminoAcidTag.Length, 2);

//            ErrorScore = Math.Sqrt(sumSquaredError / tag.Length); //RMSE
//            ErrorScore = Math.Exp(-20 * ErrorScore);

//            FrequencyScore = sumIntensity / tag.Length;
//            FrequencyScore = FrequencyScore * LengthScore;
//        }

//        public PstTagsDtoGpu(string tag, int start, int end, List<double> tagErrors, List<double> tagIntensities, int tagLength = 1)
//        {
//            AminoAcidTag = tag;
//            StartPosition = start;
//            EndPosition = end;
//            TagLength = tagLength;

//            LengthScore = Math.Pow(AminoAcidTag.Length, 2);

//            ErrorSquaredList = tagErrors;
//            ErrorScore = ErrorSquaredList.Average(x => Math.Pow(x, 2));
//            ErrorScore = Math.Sqrt(ErrorScore); //RMSE
//            ErrorScore = Math.Exp(-20 * ErrorScore);

//            IntensityList = tagIntensities;
//            FrequencyScore = IntensityList.Average(x => x);
//            FrequencyScore = FrequencyScore * LengthScore;

//            TagPositions = new List<int>();

//        }
//    }

//    public class NewPstTagsDtoGpu  // Will Change it 
//    {
//        public string PstTag;
//        public double PstLength;
//        public double Error; //abhe of Squared sum of error ha {/ lekin ErrorScore chahya
//        public double PstTagIntensity; //abhe of Squared Pst Intensity ha {/ lekin Frequency chahya//Frequency

//        //public double SumofError; //List<double> 
        
//        //public double pstint;

//        public NewPstTagsDtoGpu(string cPstTag, double cPstLength, double cError, double cPstTagIntensity)
//        {
//            PstTag = cPstTag;
//            PstLength = cPstLength;
//            Error = cError;
//            PstTagIntensity = cPstTagIntensity;
//        }


//        //public NewPstTagsDtoGpu(string cpst, List<double> cErrorList, double cpstint)
//        //{
//        //    pst = cpst;
//        //    ErrorList = cErrorList;

//        //    pstint = cpstint;
//        //}


//    }
//}
    