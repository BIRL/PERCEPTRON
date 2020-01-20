using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;
using System.Text.RegularExpressions;

namespace PerceptronLocalService.Engine
{
    public class Truncation : ITruncation
    {
        public void PreTruncation(SearchParametersDto parameters, List<ProteinDto> CandidateProteinListTruncated, List<ProteinDto> CandidateProteinListTruncatedLeft, List<ProteinDto> CandidateProteinListTruncatedRight, List<newMsPeaksDto> peakData2DList)
        {
            try
            {


                string[] IndividualModifications = parameters.TerminalModification.Split(',');
                int NumOfModifications = IndividualModifications.Length;

                double IntactMass = peakData2DList[0].Mass;
                double AcetylationWeight = 42.0106;
                double MassOfMethionine = AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MassOfMethionine) ? MassOfMethionine : MassOfMethionine;
                var DELMELIST = new List<int>();

                for (int indexProteinList = 0; indexProteinList < CandidateProteinListTruncated.Count; indexProteinList++)
                {
                    string tempSequence;
                    int tempSequenceLength;
                    //if (CandidateProteinListTruncated[indexProteinList].Header == "P04439")  // DELME
                    //{
                    //    int wait;                                                             // DELME

                    //InsilicoLeftIons and InsilicoRightIons are not 
                    var Protein = CandidateProteinListTruncated[indexProteinList];
                    string Sequence = Protein.Sequence;
                    int ProteinLength = Sequence.Length;


                    //var InsilicoLeftIons = Protein.InsilicoDetails.InsilicoMassLeft;
                    //var InsilicoRightIons = Protein.InsilicoDetails.InsilicoMassRight;
                    var LeftIonsBackUp = new List<double>();
                    var RightIonsBackUp = new List<double>();

                    LeftIonsBackUp.AddRange(Protein.InsilicoDetails.InsilicoMassLeft);
                    RightIonsBackUp.AddRange(Protein.InsilicoDetails.InsilicoMassRight);

                    //Left Ions
                    var LeftIons = new List<double>();
                    LeftIons.AddRange(LeftIonsBackUp);

                    int PreTruncationIndex = FindPreTruncationIndex(IntactMass, parameters.MwTolerance, LeftIons, ProteinLength);


                    Protein.TruncationIndex = PreTruncationIndex;                                                                                    //~~~~~~~~~~// Its PerTrucnation Index means before Truncation...
                    tempSequence = Protein.Sequence.Substring(0, PreTruncationIndex);  //Protein.TruncatedSequence 
                    tempSequenceLength = tempSequence.Length;

                    Protein.Sequence = tempSequence;

                    //////////////////////////////////////////////////// #FORTHETIMEBEING: Once We will start Concatenation of List of every Algortihm then, Remove the Below
                    LeftIons.RemoveRange(Protein.TruncationIndex, LeftIons.Count - Protein.TruncationIndex);
                    double NewMolecularWeight = LeftIons[tempSequenceLength - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;   // 6302P vs 629S #DECIMALDIFF $AFFECTING ALL MolW

                    //// Right Ions 
                    var RightIons = new List<double>();
                    RightIons.AddRange(RightIonsBackUp);

                    int TruncationIndex = ProteinLength - PreTruncationIndex - 1; // "-1" is due to ZERO BASED INDEXING in C#

                    if (TruncationIndex != 0)
                    {
                        double InsilicoTruncationIndexMass = RightIons[TruncationIndex];
                        RightIons.RemoveRange(0, TruncationIndex + 1); // + 1
                        RightIons = ElementwiseListOperation(RightIons, -InsilicoTruncationIndexMass);
                    }

                    if (IndividualModifications[0] == "None")
                    {

                        Protein.Mw = NewMolecularWeight;
                        Protein.InsilicoDetails.InsilicoMassLeft = LeftIons;
                        Protein.InsilicoDetails.InsilicoMassRight = RightIons;

                        CandidateProteinListTruncatedRight.Add(Protein);
                    }


                    if (tempSequence[0] == 'M')
                    {

                        if (IndividualModifications[0] == "NME" || IndividualModifications[1] == "NME") //Its Seems Like hard Code but no in Actual because We know the position of NME will always be either 0 or 1  //// Just checking NME with this method so to avoid conflict between NME and NME_Acetylation
                        {


                            Protein.TerminalModification = "NME";
                            Protein.Mw = NewMolecularWeight - MassOfMethionine;

                            Protein.InsilicoDetails.InsilicoMassLeft = ElementwiseListOperation(LeftIons, -MassOfMethionine);
                            Protein.Sequence = tempSequence.Substring(1, tempSequenceLength - 1);
                            CandidateProteinListTruncatedRight.Add(Protein);
                        }

                        if (parameters.TerminalModification.Contains("NME_Acetylation"))
                        {
                            Protein.TerminalModification = "NME_Acetylation";
                            Protein.Mw = NewMolecularWeight - MassOfMethionine + AcetylationWeight;
                            Protein.InsilicoDetails.InsilicoMassLeft = ElementwiseListOperation(LeftIons, (-MassOfMethionine + AcetylationWeight));
                            Protein.Sequence = tempSequence.Substring(1, tempSequenceLength - 1);
                            CandidateProteinListTruncatedRight.Add(Protein);
                        }

                        if (parameters.TerminalModification.Contains("M_Acetylation"))
                        {
                            Protein.TerminalModification = "M_Acetylation";
                            Protein.Sequence = tempSequence;
                            Protein.Mw = NewMolecularWeight + AcetylationWeight;
                            Protein.InsilicoDetails.InsilicoMassLeft = ElementwiseListOperation(LeftIons, AcetylationWeight);
                            CandidateProteinListTruncatedRight.Add(Protein);

                        }

                    }


                    RightIons = new List<double>();
                    RightIons.AddRange(RightIonsBackUp);

                    PreTruncationIndex = FindPreTruncationIndex(IntactMass, parameters.MwTolerance, RightIons, ProteinLength);

                    //#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE
                    //PreTruncationIndex = PreTruncationIndex + 3; // #HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE   //ITS ORIGINAL#1 
                    //#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE
                    //(NOT APPLICABLE DUE TO 2 HARDCODINGS)Right side k lehaz sa TruncationIndex is small according to SPECTRUM(252) IN PERCEPTRON(253)
                    TruncationIndex = ProteinLength - PreTruncationIndex + 1;

                    //TEST 
                    if (CandidateProteinListTruncatedRight.Count == 188)
                    {
                        int DELMEE = 1;
                    }
                    //TEST
                    //Left Ions
                    Protein.TruncationIndex = TruncationIndex;

                    tempSequence = Sequence.Substring(TruncationIndex, ProteinLength - TruncationIndex);
                    Protein.Sequence = tempSequence;
                    tempSequenceLength = tempSequence.Length;

                    //PreTruncationIndex = PreTruncationIndex - 1;//"-1"#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE#HARDCODE #HARDCODE ITS ORIGINAL#2

                    //////////////////////////////************************************
                    //////////////////////////////************************************
                    //START FROM BELOW,,,,,,,,,,..........
                    //           TEST ME FROM HERE ..................
                    /////////////////************************************

                    RightIons.RemoveRange(PreTruncationIndex, ProteinLength - PreTruncationIndex);

                    if (TruncationIndex - 1 != 0)
                    {
                        //Protein.Mw = RightIons[tempSequenceLength - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221; //"-1"#HARDCODE #HARDCODE#HARDCODE #HARDCODE //ITS ORIGINAL#3
                        Protein.Mw = RightIons[tempSequenceLength - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221; //"-1"#HARDCODE #HARDCODE#HARDCODE #HARDCODE
                        Protein.TerminalModification = "None";
                        LeftIons = new List<double>();
                        LeftIons.AddRange(LeftIonsBackUp);
                        double InsilicoTruncationIndexMass = LeftIons[TruncationIndex - 1];

                        //LeftIons.RemoveRange(TruncationIndex, ProteinLength - TruncationIndex); //#SEE IS "TruncationIndex - 1" HARDCODE OR NOT
                        LeftIons.RemoveRange(0, ProteinLength - PreTruncationIndex);
                        LeftIons = ElementwiseListOperation(LeftIons, InsilicoTruncationIndexMass);

                        Protein.InsilicoDetails.InsilicoMassLeft = LeftIons;
                        Protein.InsilicoDetails.InsilicoMassRight = RightIons;

                        CandidateProteinListTruncatedLeft.Add(Protein);
                    }

                    int a = tempSequence.Length;




                    //}
                }
            }
            catch(Exception)
            {
                var asd = CandidateProteinListTruncatedLeft;
            }





        }
        public int FindPreTruncationIndex(double IntactMass, double MwTolerance, List<double> Ions, int ProteinSequenceLength)
        {
            int PreTruncationIndex = ProteinSequenceLength;
            var start = (int)(Math.Ceiling((IntactMass + MwTolerance) / 168) - 1); //#HARDCODE_DiscussWithSir: One (-1) is HardCoded, due to zeroindexing in C# // Don't know Why 168 

            for (int index = start; index < ProteinSequenceLength; index++)  // Double Stepping
            {
                //I think it means: Before Truncation Mass of Protein lie outside the range so, that after truncation it will Fall within the Tolerance
                if (Ions[index] - IntactMass > MwTolerance)   //POTENTIAL BUG: Test Me...
                {
                    PreTruncationIndex = index;
                    break;

                }
                index += 1; //Just needed due to double Stepping like in SPECTRUM{start:2:ProteinLength}
            }


            return PreTruncationIndex;
        }
        public List<double> ElementwiseListOperation(List<double> Ions, double Offset)
        {
            var newList = new List<double>();
            for (int iter = 0; iter < Ions.Count; iter++)
            {

                newList.Add(Ions[iter] + Offset);
            }
            return newList;

        }

        public void TruncationLeft(List<ProteinDto> CandidateProteinListTruncatedLeft, List<newMsPeaksDto>  peakData2DList)
        {

        }

        public void TruncationRight(List<ProteinDto> CandidateProteinListTruncatedLeft, List<newMsPeaksDto>  peakData2DList)
        {

        }

    }
}

//Protein.InsilicoDetails.InsilicoMassLeft;
//tempSequence.Ge GetRange(TruncationIndex + 1, PreTruncationIndex); //GetRange(int index, int count)


//for (int iter = 0; iter < RightIons.Count; iter++)
//{
//    tempRightIons.Add(RightIons[iter] - InsilicoTruncationIndexMass);

//}
//RightIons = tempRightIons;


//RightIons = RightIons.GetRange(TruncationIndex + 1, PreTruncationIndex); //GetRange(int index, int count)
//RightIons = RightIons - InsilicoTruncationIndexMass;

//ElementwiseListOperation(tempRightIons, iteration, RightIons, InsilicoTruncationIndexMass);



///////////////////////////ITS HEALTHY///////////////////////////
//for (int index = start; index < Protein.Sequence.Length; index++)  // Double Stepping
//{


//    DELMELIST.Add(index);                                                         // DELME
//    //I think it means: Before Truncation Mass of Protein lie outside the range so, that after truncation it will Fall within the Tolerance
//    if (LeftIons[index] - IntactMass > parameters.MwTolerance)   //POTENTIAL BUG: Test Me...
//    {
//        PreTruncationIndex = index;
//        break;

//    }
//    index += 1; //Just needed due to double Stepping like in SPECTRUM{start:2:ProteinLength}
//}