using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;


namespace PerceptronLocalService.Engine
{
    public class Truncation : ITruncation
    {
        private const double AcetylationWeight = 42.0106;
        //private const double MethionineWeight = 42.0106;
        double MethionineWeight = 131.04049; //RECEIVING so for the #TIMEBEING//= AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;
        //public void PreTruncation(List<ProteinDto> proteinList, MsPeaksDto peakData, SearchParametersDto parameters,
        //    List<ProteinDto> proteinListLeft, List<ProteinDto> proteinListRight)
        public void PreTruncation(SearchParametersDto parameters, List<ProteinDto> proteinList, List<ProteinDto> proteinListLeft, List<ProteinDto> proteinListRight, List<newMsPeaksDto> peakData2DList)
        {
            try
            {
                var DELMELIST = new List<int>();
                int varNone = 0;
                int varNME = 0;
                int NME_A = 0;
                int M_A = 0;


                string[] IndividualModifications = parameters.TerminalModification.Split(',');
                //var terminalModifications = parameters.TerminalModification.Split('-'); // Assign using parameters
                //var none = terminalModifications[0].ToUpper();
                //var nme = terminalModifications[1].ToUpper();
                //var nmeAcetylation = terminalModifications[2].ToUpper();
                //var mAcetylation = terminalModifications[3].ToUpper();
                //var nme = ""; var nmeAcetylation = ""; var mAcetylation = "";

                var proteinExperimentalMw = peakData2DList[0].Mass;

                for (var i = 0; i < proteinList.Count; i++)
                {
                    ///JUST FIVE...
                    //if (proteinList[i].Header == "P04439" || proteinList[i].Header == "P61574")
                    //////if(proteinList[i].Header == "Q8WWY7")


                    //if (proteinList[i].Header == "Q5JRK9")
                    //{


                        var protein = proteinList[i];
                        var newProtein = new ProteinDto(protein);

                        var prtLength = protein.Sequence.Length;
                        var preTruncationIndex = prtLength;
                        var start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + parameters.MwTolerance) / 168) - 1);

                        var leftString = Clone.CloneObject(protein.InsilicoDetails.InsilicoMassLeft);
                        var leftIons = Clone.Decrypt<List<double>>(leftString);

                        var seqString = Clone.CloneObject(protein.Sequence);
                        var sequence = Clone.Decrypt<string>(seqString);

                        var rightString = Clone.CloneObject(protein.InsilicoDetails.InsilicoMassRight);
                        var rightIons = Clone.Decrypt<List<double>>(rightString);


                        for (var m = start; m < prtLength; m++)
                        {
                            if (!(leftIons[m] - proteinExperimentalMw > parameters.MwTolerance)) continue;
                            preTruncationIndex = m;
                            break;
                        }
                        int factor = 0; /// factor is just a constant number for Compensating C# ZERO BASED INDEXING AGAINST SPECTRUM
                        if (preTruncationIndex != prtLength)
                        {
                            factor = 1;
                        }

                        var tmpSeq = sequence.Substring(0, preTruncationIndex + factor);
                        var tmpSeqLength = tmpSeq.Length;
                        newProtein.Sequence = tmpSeq;
                        newProtein.TruncationIndex = preTruncationIndex;
                        leftIons = leftIons.GetRange(0, preTruncationIndex + factor);

                        var molW = leftIons[tmpSeqLength - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;

                        // Right Ion Stuff

                        //var truncationIndex = prtLength - (preTruncationIndex + factor);
                        var truncationIndex = prtLength - preTruncationIndex + factor;
                        if (truncationIndex != 0)
                        {
                            var insilicoTruncationIdxMass = rightIons[truncationIndex - 1];
                            rightIons = rightIons.GetRange(truncationIndex, prtLength - truncationIndex);
                            rightIons = rightIons.Select(x => x - insilicoTruncationIdxMass).ToList();
                        }

                        if (IndividualModifications[0] == "None")   //(none.ToUpper() == "NONE")
                        {

                            varNone = varNone + 1;

                            newProtein.TerminalModification = "None";
                            newProtein.Mw = molW;
                            newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                            newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                            proteinListRight.Add(newProtein);
                        }

                        if (tmpSeq[0] == 'M')
                        {


                            if (IndividualModifications[0] == "NME" || IndividualModifications[1] == "NME") //Its Seems Like hard Code but not in Actual because We know the position of NME will always be either 0 or 1  //// Just checking NME with this method so to avoid conflict between NME and NME_Acetylation     //(nme.ToUpper() == "NME")
                            {
                                varNME = varNME + 1;
                                newProtein = new ProteinDto(protein)
                                {
                                    TerminalModification = "NME",
                                    Mw = molW - MethionineWeight,
                                    InsilicoDetails = { InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList() },
                                    Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) // "-1" Added
                                };
                                proteinListRight.Add(newProtein);
                            }

                            if (parameters.TerminalModification.Contains("NME_Acetylation"))
                            {
                                NME_A = NME_A + 1;
                                bool a = parameters.TerminalModification.Contains("NME_Acetylation");
                                newProtein = new ProteinDto(protein)
                                {
                                    TerminalModification = "NME_Acetylation",
                                    Mw = molW - MethionineWeight + AcetylationWeight,
                                    InsilicoDetails =
                                    {
                                        InsilicoMassLeft =
                                            leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList()
                                    },
                                    Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) // "-1" Added
                                };
                                proteinListRight.Add(newProtein);
                            }
                            if (parameters.TerminalModification.Contains("M_Acetylation"))
                            {
                                M_A = M_A + 1;
                                newProtein = new ProteinDto(protein)
                                {
                                    TerminalModification = "M_Acetylation",
                                    Sequence = tmpSeq,
                                    Mw = molW + AcetylationWeight,
                                    InsilicoDetails = { InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList() }
                                };
                                proteinListRight.Add(newProtein);
                            }
                        }

                        //Save Copy for Left Truncation
                        protein = proteinList[i];
                        newProtein = new ProteinDto(protein);

                        sequence = Clone.Decrypt<string>(seqString);
                        prtLength = sequence.Length;
                        preTruncationIndex = prtLength;
                        rightIons = Clone.Decrypt<List<double>>(rightString);
                        start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + parameters.MwTolerance) / 168)) - 1;

                        for (var m = start; m < prtLength; m++)
                        {
                            if (!(rightIons[m] - proteinExperimentalMw > parameters.MwTolerance)) continue;
                            preTruncationIndex = m;
                            break;
                        }

                        try
                        {
                            truncationIndex = prtLength - (preTruncationIndex + 1);  // + factor

                            if (truncationIndex >0)
                            {
                                //truncationIndex = prtLength - (preTruncationIndex + 1);  // + factor
                                //truncationIndex = prtLength - preTruncationIndex + 1;
                                newProtein.TruncationIndex = truncationIndex;
                                tmpSeq = sequence.Substring(truncationIndex, prtLength - truncationIndex);
                                newProtein.Sequence = tmpSeq;
                                rightIons = rightIons.GetRange(0, preTruncationIndex + 1).ToList();  // + factor

                                //if (truncationIndex >= 0) // (truncationIndex - 1 != 0 && preTruncationIndex != prtLength)
                                //{
                                newProtein.Mw = rightIons[rightIons.Count - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;
                                newProtein.TerminalModification = "None";
                                leftIons = Clone.Decrypt<List<double>>(leftString);

                                double insilicoTruncationIdxMass1 = leftIons[truncationIndex - 1]; //leftIons[truncationIndex - 1]
                                leftIons = leftIons.GetRange(truncationIndex, prtLength - truncationIndex);

                                leftIons = leftIons.Select(x => x - insilicoTruncationIdxMass1).ToList();
                                newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                                newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                                proteinListLeft.Add(newProtein);
                            }
                            else
                            {
                                DELMELIST.Add(i);
                            }
                        }

                        catch (Exception e)
                        {

                            int value = i;
                        }
                        

                    //}
                }
            }
            catch (Exception e)
            {
                int fddas = 0;
            }

        }




        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //if (preTruncationIndex == prtLength)
        //{
        //    //int test = prtLength - (preTruncationIndex + 1);  // NOT REASONABLE...
        //    truncationIndex = prtLength - preTruncationIndex;  // + factor
        //    //truncationIndex = prtLength - preTruncationIndex + 1;
        //    newProtein.TruncationIndex = truncationIndex;
        //    tmpSeq = sequence.Substring(truncationIndex, prtLength - truncationIndex);
        //    newProtein.Sequence = tmpSeq;
        //    rightIons = rightIons.GetRange(0, preTruncationIndex + factor).ToList();
        //}

        // Left Ion Stuff
        //if (truncationIndex - 1 <= 0) continue;
        //if (truncationIndex - 1 != 0) //continue;
        //if (truncationIndex > -1) //IN TESTING PROCESS

       /////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //{
        //    try
        //    {



        //        string[] IndividualModifications = parameters.TerminalModification.Split(',');
        //        //var terminalModifications = parameters.TerminalModification.Split('-'); // Assign using parameters
        //        //var none = terminalModifications[0].ToUpper();
        //        ////var nme = terminalModifications[1].ToUpper();
        //        ////var nmeAcetylation = terminalModifications[2].ToUpper();
        //        ////var mAcetylation = terminalModifications[3].ToUpper();
        //        //var nme = ""; var nmeAcetylation = ""; var mAcetylation = "";

        //        var proteinExperimentalMw = peakData2DList[0].Mass;//.WholeProteinMolecularWeight;

        //        for (var i = 0; i < proteinList.Count; i++)
        //        {

        //            //if (proteinList[i].Header == "P04439")
        //            //{
        //            var protein = proteinList[i];

        //            var newProtein = new ProteinDto(protein);

        //            var prtLength = protein.Sequence.Length;
        //            int preTruncationIndex = prtLength;
        //            //var start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + parameters.MwTolerance) / 168) - 1);

        //            var leftString = Clone.CloneObject(protein.InsilicoDetails.InsilicoMassLeft);
        //            var leftIons = Clone.Decrypt<List<double>>(leftString);

        //            var seqString = Clone.CloneObject(protein.Sequence);
        //            var sequence = Clone.Decrypt<string>(seqString);

        //            var rightString = Clone.CloneObject(protein.InsilicoDetails.InsilicoMassRight);
        //            var rightIons = Clone.Decrypt<List<double>>(rightString);


        //            if (proteinList[i].Header == "P61574")//(preTruncationIndex == prtLength)
        //            {
        //                int aj = 0;
        //            }

        //            preTruncationIndex = FindPreTruncationIndex(proteinExperimentalMw, parameters.MwTolerance, leftIons, prtLength);  // PreTruncationIndex IS BASED ON ZERO INDEXING

        //            //for (var m = start; m < prtLength; m = m + 2)
        //            //{
        //            //    if (!(leftIons[m] - proteinExperimentalMw > parameters.MwTolerance)) continue;
        //            //    preTruncationIndex = m;
        //            //    break;
        //            //}
        //            string tmpSeq = "";
        //            int truncationIndex = 0;
        //            if (preTruncationIndex != prtLength)  //#Jdot
        //            {
        //                tmpSeq = sequence.Substring(0, preTruncationIndex + 1);  //"+1 is due to length" //Substring(int startIndex, int length);   //Protein.TruncatedSequence
        //                leftIons = leftIons.GetRange(0, preTruncationIndex + 1);

        //                truncationIndex = prtLength - preTruncationIndex - 1 - 1; // "-1" is due to ZERO BASED INDEXING of PreTruncation & OTHER "-1" is to Make TruncationIndex, ZERO BASED INDEXED
        //                //We made both PreTruncationIndex and TruncationIndex is ZERO BASED. THEREFORE, it will always be {PreTruncationIndex + TruncationIndex +2 = Protein Sequence Length}
        //                //Main Reason of making both PreTruncationIndex and TruncationIndex ZERO BASED. Because LISTS in C# is ZERO BASED INDEXED.
        //            }

        //            else if (preTruncationIndex == prtLength)  //#Jdot
        //            {
        //                //preTruncationIndex -= 1;
        //                tmpSeq = sequence.Substring(0, preTruncationIndex);  //"+1 is due to length" //Substring(int startIndex, int length);   //Protein.TruncatedSequence
        //                leftIons = leftIons.GetRange(0, preTruncationIndex);

        //                truncationIndex = prtLength - preTruncationIndex; // "-1" is due to ZERO BASED INDEXING of PreTruncation & OTHER "-1" is to Make TruncationIndex, ZERO BASED INDEXED
        //                //We made both PreTruncationIndex and TruncationIndex is ZERO BASED. THEREFORE, it will always be {PreTruncationIndex + TruncationIndex +2 = Protein Sequence Length}
        //                //Main Reason of making both PreTruncationIndex and TruncationIndex ZERO BASED. Because LISTS in C# is ZERO BASED INDEXED.
        //            }


        //            var tmpSeqLength = tmpSeq.Length;
        //            newProtein.Sequence = tmpSeq;
        //            newProtein.TruncationIndex = preTruncationIndex;

        //            var molW = leftIons[tmpSeqLength - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;   // +1.0078250321 + 1.0078250321 + 15.9949146221;

        //            // Right Ion Stuff

        //            if (truncationIndex <= 0)
        //            {
        //                int chekc = 0;
        //            }
        //            if (protein.Header == "P61574")
        //            {
        //                int delme = 0;
        //            }
        //            if (truncationIndex != 0)// (truncationIndex != -1)truncationIndex != -1             //ITH   0  -- ZERO
        //            {
        //                var insilicoTruncationIdxMass = rightIons[truncationIndex];
        //                rightIons = rightIons.GetRange(truncationIndex + 1, prtLength - truncationIndex - 1);
        //                rightIons = rightIons.Select(x => x - insilicoTruncationIdxMass).ToList();
        //            }

        //            if (IndividualModifications[0] == "None")   //(none.ToUpper() == "NONE")
        //            {
        //                newProtein.TerminalModification = "None";
        //                newProtein.Mw = molW;
        //                newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
        //                newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
        //                proteinListRight.Add(newProtein);
        //            }
        //            if (tmpSeq[0] == 'M')
        //            {
        //                if (IndividualModifications[0] == "NME" || IndividualModifications[1] == "NME") //Its Seems Like hard Code but not in Actual because We know the position of NME will always be either 0 or 1  //// Just checking NME with this method so to avoid conflict between NME and NME_Acetylation     //(nme.ToUpper() == "NME")
        //                {
        //                    newProtein = new ProteinDto(protein)
        //                    {
        //                        TerminalModification = "NME",
        //                        Mw = molW - MethionineWeight,
        //                        InsilicoDetails = { InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList() },
        //                        Sequence = tmpSeq.Substring(1, tmpSeqLength - 1) //"-1" is due to length" //Substring(int startIndex, int length); 
        //                    };
        //                    proteinListRight.Add(newProtein);
        //                }

        //                if (parameters.TerminalModification.Contains("NME_Acetylation"))  //(nmeAcetylation.ToUpper() == "NME_ACETYLATION")
        //                {
        //                    newProtein = new ProteinDto(protein)
        //                    {
        //                        TerminalModification = "NME_Acetylation",
        //                        Mw = molW - MethionineWeight + AcetylationWeight,
        //                        InsilicoDetails =
        //                        {
        //                            InsilicoMassLeft =
        //                                leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList()
        //                        },
        //                        Sequence = tmpSeq.Substring(1, tmpSeqLength - 1)
        //                    };
        //                    proteinListRight.Add(newProtein);
        //                }
        //                if (parameters.TerminalModification.Contains("M_Acetylation"))  //(mAcetylation.ToUpper() == "M_ACETYLATION")
        //                {
        //                    newProtein = new ProteinDto(protein)
        //                    {
        //                        TerminalModification = "M_Acetylation",
        //                        Sequence = tmpSeq,
        //                        Mw = molW + AcetylationWeight,
        //                        InsilicoDetails = { InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList() }
        //                    };
        //                    proteinListRight.Add(newProtein);
        //                }
        //            }

        //            //Save Copy for Left Truncation
        //            protein = proteinList[i];
        //            newProtein = new ProteinDto(protein);

        //            sequence = Clone.Decrypt<string>(seqString);
        //            prtLength = sequence.Length;
        //            preTruncationIndex = prtLength;
        //            rightIons = Clone.Decrypt<List<double>>(rightString);

        //            preTruncationIndex = FindPreTruncationIndex(proteinExperimentalMw, parameters.MwTolerance, rightIons, prtLength);  // PreTruncationIndex IS BASED ON ZERO INDEXING
        //            //start = Convert.ToInt32(Math.Ceiling((proteinExperimentalMw + parameters.MwTolerance) / 168)) - 1;

        //            //for (var m = start; m < prtLength; m = m + 2)
        //            //{
        //            //    if (!(rightIons[m] - proteinExperimentalMw > parameters.MwTolerance)) continue;
        //            //    preTruncationIndex = m;
        //            //    break;
        //            //}
        //            truncationIndex = prtLength - preTruncationIndex - 1; //"-1" is for ZERO BASED INDEXING
        //            newProtein.TruncationIndex = truncationIndex;
        //            tmpSeq = sequence.Substring(truncationIndex, prtLength - truncationIndex);
        //            newProtein.Sequence = tmpSeq;
        //            rightIons = rightIons.GetRange(0, preTruncationIndex + 1).ToList();

        //            // Left Ion Stuff
        //            if (truncationIndex - 1 != -1)//(truncationIndex-1 != 0)#ITH // {N/A}truncationIndex is already ZEROBASED INDEXED SO, IGNORING(-1) TruncationIndex-1 ~= 0
        //            {//a.k.a. (truncationIndex - 1 >= 0) 
        //                newProtein.Mw = rightIons[rightIons.Count - 1] + MassAdjustment.H + MassAdjustment.H + MassAdjustment.O;
        //                newProtein.TerminalModification = "None";
        //                leftIons = Clone.Decrypt<List<double>>(leftString);
        //                var insilicoTruncationIdxMass1 = leftIons[truncationIndex - 1];
        //                leftIons = leftIons.GetRange(truncationIndex, prtLength - truncationIndex);
        //                leftIons = leftIons.Select(x => x - insilicoTruncationIdxMass1).ToList();
        //                newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
        //                newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
        //                proteinListLeft.Add(newProtein);
        //            }
        //            //else
        //            //{
        //            //    int indexk = i;
        //            //}

        //            //}
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        int witn = 21;
        //    }
        //}
        public int FindPreTruncationIndex(double IntactMass, double MwTolerance, List<double> Ions, int ProteinSequenceLength)
        {
            int preTruncationIndex = ProteinSequenceLength;
            var start = (int)(Math.Ceiling((IntactMass + MwTolerance) / 168) - 1); //#HARDCODE_DiscussWithSir: One (-1) is HardCoded, due to zeroindexing in C# // Don't know Why 168 

            for (int index = start; index < ProteinSequenceLength; index++)  // Double Stepping REPLACED BY Single Stepping...
            {
                //I think it means: Before Truncation Mass of Protein lie outside the range so, that after truncation it will Fall within the Tolerance
                if (Ions[index] - IntactMass > MwTolerance)  //POTENTIAL BUG: Test Me...
                {
                    preTruncationIndex = index; // FROM HERE ONWARD "PreTruncationIndex" IS BASED ON ZERO INDEXING
                    break;
                }
                //index += 1; //Just needed due to double Stepping like in SPECTRUM{start:2:ProteinLength}
            }
            return preTruncationIndex;
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

        //public void TruncationLeft(List<ProteinDto> proteinList, MsPeaksDto peakData, SearchParametersDto parameters,
        //    List<ProteinDto> proteinListTruncated, List<ProteinDto> proteinListRemaining)
        //{
        //    var proteinExperimentalMw = peakData.WholeProteinMolecularWeight;
        //    const int tol = 2;
        //    for (var index = 1; index < proteinList.Count; index++)
        //    {
        //        var protein = proteinList[index];
        //        var prtLength = protein.Sequence.Length;

        //        // shift experimental Mass by truncation mass
        //        var truncationMass = protein.Mw - proteinExperimentalMw;
        //        var start = Convert.ToInt32(Math.Ceiling(truncationMass / 168)) - 1;

        //        if (truncationMass > 0)
        //        {
        //            // Find Truncation Location and Type
        //            var leftIndex = -1;

        //            for (var m = start; m < prtLength; m++)
        //            {
        //                var diffLeft = protein.InsilicoDetails.InsilicoMassLeft[m] - truncationMass;

        //                if (Math.Abs(diffLeft) <= tol) // No need to worry about Terminal Modifications!)
        //                    leftIndex = m; // This index correspond to the New truncation site located at N-Terminous

        //                if (diffLeft > tol)
        //                    break;

        //            }
        //            if (leftIndex == -1)
        //            {
        //                proteinListRemaining.Add(protein);
        //            }
        //            else
        //            {
        //                protein.Truncation = "Left";
        //                protein.InsilicoDetails.InsilicoMassLeft =
        //                    protein.InsilicoDetails.InsilicoMassLeft.Select(
        //                        x => x - protein.InsilicoDetails.InsilicoMassLeft[leftIndex]).ToList();
        //                protein.InsilicoDetails.InsilicoMassLeft =
        //                    protein.InsilicoDetails.InsilicoMassLeft.GetRange(leftIndex + 1,
        //                        protein.InsilicoDetails.InsilicoMassLeft.Count - leftIndex - 1);
        //                protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(
        //                    0, prtLength - leftIndex - 1); // as this will be the MW of protein - Water
        //                var sequence = protein.Sequence.Substring(leftIndex + 1, prtLength - leftIndex - 1);
        //                protein.Sequence = sequence;

        //                if (sequence.Length < 5)
        //                    continue;

        //                protein.TruncationIndex = protein.TruncationIndex + leftIndex;
        //                protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + 1.0078250321 +
        //                             1.0078250321 + 15.9949146221;
        //                proteinListTruncated.Add(protein);
        //            }
        //        }
        //    }
        //}

        //public void TruncationRight(List<ProteinDto> proteinList, MsPeaksDto peakData, SearchParametersDto parameters,
        //   List<ProteinDto> proteinListTruncated, List<ProteinDto> proteinListRemaining)
        //{
        //    var proteinExperimentalMw = peakData.WholeProteinMolecularWeight;
        //    const int tol = 2;
        //    for (var index = 1; index < proteinList.Count; index++)
        //    {
        //        var protein = proteinList[index];
        //        var prtLength = protein.Sequence.Length;

        //        // shift experimental Mass by truncation mass
        //        var truncationMass = protein.Mw - proteinExperimentalMw;
        //        var start = Convert.ToInt32(Math.Ceiling(truncationMass / 168)) - 1;
        //        if (!(truncationMass > 0)) continue;

        //        // Find Truncation Location and Type
        //        var rightIndex = -1;
        //        for (var m = start; m < prtLength; m++)
        //        {
        //            var diff = protein.InsilicoDetails.InsilicoMassRight[m] - truncationMass;
        //            if (Math.Abs(diff) <= tol) // No need to worry about Terminal Modifications!)
        //                rightIndex = m; // This index correspond to the New truncation site located at N-Terminous
        //            if (diff > tol)
        //                break;
        //        }

        //        if (rightIndex == -1)
        //        {
        //            proteinListRemaining.Add(protein);
        //        }
        //        else
        //        {
        //            var truncationIndex = prtLength - rightIndex;
        //            protein.Truncation = "Right";
        //            protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(0, truncationIndex - 1);
        //            protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.Select(x => x - protein.InsilicoDetails.InsilicoMassRight[rightIndex]).ToList(); // as this will be the MW of protein - Water
        //            protein.InsilicoDetails.InsilicoMassRight =
        //                protein.InsilicoDetails.InsilicoMassRight.GetRange(rightIndex + 1,
        //                    protein.InsilicoDetails.InsilicoMassRight.Count - rightIndex - 1);
        //            var sequence = protein.Sequence.Substring(0, truncationIndex - 1);
        //            protein.Sequence = sequence;

        //            if (sequence.Length < 5)
        //                continue;

        //            protein.TruncationIndex = truncationIndex - 2;
        //            protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;
        //            proteinListTruncated.Add(protein);
        //        }

        //    }
        //}

        //public void TruncationLeftWithMoification(List<ProteinDto> proteinList, MsPeaksDto peakData, SearchParametersDto parameters,
        //    List<ProteinDto> proteinListTruncated)
        //{
        //    var proteinExperimentalMw = peakData.WholeProteinMolecularWeight;
        //    const int tol = 2;
        //    for (var index = 1; index < proteinList.Count; index++)
        //    {
        //        var protein = proteinList[index];
        //        var prtLength = protein.Sequence.Length;

        //        // shift experimental Mass by truncation mass
        //        var truncationMass = protein.Mw - proteinExperimentalMw;
        //        truncationMass = AdjustProteinForTruncation(truncationMass, parameters);

        //        if (truncationMass > 0)
        //        {
        //            // Find Truncation Location and Type
        //            var leftIndex = -1;

        //            var start = Convert.ToInt32(Math.Ceiling(truncationMass / 168)) - 1;
        //            for (var m = start; m < prtLength; m++)
        //            {
        //                var diffLeft = protein.InsilicoDetails.InsilicoMassLeft[m] - truncationMass;

        //                if (Math.Abs(diffLeft) <= tol) // No need to worry about Terminal Modifications!)
        //                    leftIndex = m; // This index correspond to the New truncation site located at N-Terminous

        //                if (diffLeft > tol)
        //                    break;

        //            }
        //            if (leftIndex == -1) continue;

        //            protein.Truncation = "Left";
        //            protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.Select(x => x - protein.InsilicoDetails.InsilicoMassLeft[leftIndex]).ToList();
        //            protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(leftIndex + 1, protein.InsilicoDetails.InsilicoMassLeft.Count - leftIndex - 1);
        //            protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.GetRange(0, prtLength - leftIndex - 1); // as this will be the MW of protein - Water
        //            var sequence = protein.Sequence.Substring(leftIndex + 1, prtLength - leftIndex - 1);
        //            protein.Sequence = sequence;

        //            if (sequence.Length < 5) continue;

        //            protein.TruncationIndex = protein.TruncationIndex + leftIndex;
        //            protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + 1.0078250321 + 1.0078250321 + 15.9949146221;
        //            proteinListTruncated.Add(protein);
        //        }
        //    }
        //}

        //public void TruncationRightWithModification(List<ProteinDto> proteinList, MsPeaksDto peakData, SearchParametersDto parameters,
        //   List<ProteinDto> proteinListTruncated)
        //{
        //    var proteinExperimentalMw = peakData.WholeProteinMolecularWeight;
        //    const int tol = 2;
        //    for (var index = 1; index < proteinList.Count; index++)
        //    {
        //        var protein = proteinList[index];
        //        var prtLength = protein.Sequence.Length;

        //        // shift experimental Mass by truncation mass
        //        var truncationMass = protein.Mw - proteinExperimentalMw;
        //        truncationMass = AdjustProteinForTruncation(truncationMass, parameters);

        //        if (!(truncationMass > 0)) continue;

        //        // Find Truncation Location and Type
        //        var rightIndex = -1;
        //        var start = Convert.ToInt32(Math.Ceiling(truncationMass / 168)) - 1;
        //        for (var m = start; m < prtLength; m++)
        //        {
        //            var diff = protein.InsilicoDetails.InsilicoMassRight[m] - truncationMass;
        //            if (Math.Abs(diff) <= tol) // No need to worry about Terminal Modifications!)
        //                rightIndex = m; // This index correspond to the New truncation site located at N-Terminous
        //            if (diff > tol)
        //                break;
        //        }

        //        if (rightIndex == -1) continue;

        //        var truncationIndex = prtLength - rightIndex;
        //        protein.Truncation = "Right";
        //        protein.InsilicoDetails.InsilicoMassLeft = protein.InsilicoDetails.InsilicoMassLeft.GetRange(0, truncationIndex - 1);
        //        protein.InsilicoDetails.InsilicoMassRight = protein.InsilicoDetails.InsilicoMassRight.Select(x => x - protein.InsilicoDetails.InsilicoMassRight[rightIndex]).ToList(); // as this will be the MW of protein - Water
        //        protein.InsilicoDetails.InsilicoMassRight =
        //            protein.InsilicoDetails.InsilicoMassRight.GetRange(rightIndex + 1,
        //                protein.InsilicoDetails.InsilicoMassRight.Count - rightIndex - 1);
        //        var sequence = protein.Sequence.Substring(0, truncationIndex - 1);
        //        protein.Sequence = sequence;

        //        if (sequence.Length < 5) continue;

        //        protein.TruncationIndex = truncationIndex - 2;
        //        protein.Mw = protein.InsilicoDetails.InsilicoMassRight[protein.Sequence.Length - 1] + 1.0078250321 +
        //                     1.0078250321 + 15.9949146221;
        //        proteinListTruncated.Add(protein);
        //    }
        //}

        //private static double AdjustProteinForTruncation(double truncationMass, SearchParametersDto parameters)
        //{
        //    var insilicoFragmentationType = parameters.InsilicoFragType;

        //    // Fragmentation Type
        //    switch (insilicoFragmentationType.ToUpper())
        //    {
        //        // B and Y Ions
        //        case CleavageTypes.CID:
        //        case CleavageTypes.BIRD:
        //        case CleavageTypes.IMD:
        //        case CleavageTypes.HCD:
        //        case CleavageTypes.SID:
        //            return truncationMass + MassAdjustment.Proton;

        //        // C and Z Ions
        //        case CleavageTypes.ECD:
        //        case CleavageTypes.ETD:
        //            return truncationMass + MassAdjustment.Proton + MassAdjustment.N + 3 * MassAdjustment.H;

        //        // A and X Ions
        //        case CleavageTypes.EDD:
        //        case CleavageTypes.NETD:
        //            return truncationMass + MassAdjustment.Proton - MassAdjustment.CO;
        //    }
        //    return truncationMass;
        //}

    }
}
