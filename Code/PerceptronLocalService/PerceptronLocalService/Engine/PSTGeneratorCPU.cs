using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;
using System.Collections;
using System.Text; // For ArrayList
using PerceptronLocalService.Engine;

namespace PerceptronLocalService.Engine
{
    public class PstGeneratorCpu : IPeptideSequenceTagGenerator
    {
        public double[] AminoAcidMasses = { 57.0214600000000, 71.0371100000000, 87.0320300000000, 97.0527600000000, 99.0684100000000, 101.047680000000, 103.009190000000, 113.084060000000, 114.042930000000, 115.026940000000, 128.058580000000, 128.094960000000, 129.042590000000, 131.040490000000, 137.058910000000, 147.068410000000, 156.101110000000, 163.063330000000, 168.964203000000, 186.079310000000, 255.158295000000 };

        public string[] AminoAcidSymbol = { "G", "A", "S", "P", "V", "T", "C", "L", "N", "D", "Q", "K", "E", "M", "H", "F", "R", "Y", "U", "W", "O" };

        public string[] AminoAcidName = { "Gly", "Ala", "Ser", "Pro", "Val", "Thr", "Cys", "Leu", "Asn", "Asp", "Gln", "Lys", "Glu", "Met", "His", "Phe", "Arg", "Tyr", "Sec", "Trp", "Pyl" };

        // Extracts a list Peptide Sequence Tags from the MS/MS peaklist
        public List<PstTagList> GeneratePeptideSequenceTags(SearchParametersDto parameters, MsPeaksDto peakData)
        {

            List<PstTagList> FinalPstTags = new List<PstTagList>();

            //Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            var peakDatalist = new List<peakData2Dlist>();
            for (int row = 0; row <= peakData.Mass.Count - 1; row++)
            {
                var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
                peakDatalist.Add(dataforpeakDatalist);

            }
            //Sort the peakDatalist with respect to the Mass in ascending order
            var peakDatalistsort = peakDatalist.OrderBy(n => n.Mass).ToList();

            var singleLengthPstTagList = GenerateSingleLengthPstList(parameters, peakDatalistsort);   // This method will extract Single Length PST Tags 
            var multipleLenghtTagList = GenerateMultipleLenghtPstList(parameters, singleLengthPstTagList); // This method will extract Multiple Length PST Tags 

            //var pstTagsdata = multipleLenghtTagList; // Multiple Tags
            //////// ************* FARHAN!!! WHAT IF THERE WILL BE NO PST TAGS FOUND THEN, WHAT COULD HAPPENED...?????
            if (multipleLenghtTagList.Count != 0)// Here filtering out tags according to MIN & MAX threshold of PST lengths...
            {


                //List<List<PstTagsDto>> TrimPstTagsList = TrimPstTags(multipleLenghtTagList, parameters);   // Break the larger Tags into all possible smaller tags &&& Filtering the Tags according to Minimum-Maximum Range Length of PST
                //var PstTagList = PstTagInfoList(TrimPstTagsList, parameters);  //Calculating  PST Tag Error, PST intensity, & Root Mean Square Error etc. && Finding the Unique PSTs: Remove all other redundant PSTs but keep only one having lowest Root Mean Square Error(RMSE)
                var PstTagList = PstTagInfoList(multipleLenghtTagList, parameters); //Calculating  PST Tag Error, PST intensity, & Root Mean Square Error etc.   //Updated 20210305

                //Finding the Unique PSTs: Remove all other redundant PSTs but keep only one having lowest Root Mean Square Error(RMSE)
                //If 2 or more Tags are same AT SAME POSITION then keep ONLY just one
                List<Psts> FirstUniquePstTagInfoList = UniquePsts(PstTagList);

                var AccomodatePsts = AccomodateIsoforms(FirstUniquePstTagInfoList, parameters);

                List<Psts> SecondUniquePstTagInfoList = UniquePsts(AccomodatePsts);

                FinalPstTags = FilteredPsts(SecondUniquePstTagInfoList, parameters);
            }
            return FinalPstTags;
        }

        private List<PstTagsDto> GenerateSingleLengthPstList(SearchParametersDto parameters, List<peakData2Dlist> peakDatalistsort)
        {
            // SINGLE LENGTH PSTs
            var GenerateSingleLengthPstList = new List<PstTagsDto>();
            //m/z(of MS2) value differences for each Fragment-pair

            //startIndex and endIndex are selecting upper triangle(in Matrix) for calculation to avoid Mirror Image Values {Means: startIndex = 1(means peak 1) and endIndex = 2 (means peak 2) is Equal to startIndex = 2(means peak 2) and endIndex = 1 (means peak 1) WHEN GIVING differences}
            for (var home_peakIndexsingle = 0; home_peakIndexsingle <= peakDatalistsort.Count - 2; home_peakIndexsingle++) // startIndex=0 so that's why its "-1" and according to formula "n-1" which gives us peakData.Mass.Count - 2 //Starting from peak 1(startIndex = 0)
            {
                // for each element of peaklist after ith element
                for (var hop_peakIndexsingle = home_peakIndexsingle + 1; hop_peakIndexsingle <= peakDatalistsort.Count - 1; hop_peakIndexsingle++) //endIndex starts from 0 so that's why peakData.Mass.Count - 1 and according to Formula just "n"
                {
                    var massDifferenceBetweenPeaks = peakDatalistsort[hop_peakIndexsingle].Mass - peakDatalistsort[home_peakIndexsingle].Mass; //Mass difference between two peaks

                    for (int AminoAcidIndex = 0; AminoAcidIndex <= AminoAcidMasses.Length - 1; AminoAcidIndex++) //Taking 21 amino acids
                    {
                        var TagError = massDifferenceBetweenPeaks - AminoAcidMasses[AminoAcidIndex];      // Updated 20200920
                        var absTagError = Math.Abs(TagError);      // Updated 20200920

                        if (absTagError <= parameters.HopThreshhold)      // Updated 20200920  -- Changed TagError to absTagError
                        {
                            var aminoAcid = AminoAcidSymbol[AminoAcidIndex];
                            double avgIntensityList = (peakDatalistsort[home_peakIndexsingle].Intensity + peakDatalistsort[hop_peakIndexsingle].Intensity) / 2;

                            var singleLengthPstTag = new PstTagsDto(home_peakIndexsingle, hop_peakIndexsingle, peakDatalistsort[home_peakIndexsingle].Mass, peakDatalistsort[hop_peakIndexsingle].Mass, massDifferenceBetweenPeaks, AminoAcidSymbol[AminoAcidIndex], AminoAcidName[AminoAcidIndex], TagError, avgIntensityList);

                            GenerateSingleLengthPstList.Add(singleLengthPstTag);
                        }
                        else
                        {
                            if (TagError < -parameters.HopThreshhold)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return GenerateSingleLengthPstList;
        }
        private List<List<PstTagsDto>> GenerateMultipleLenghtPstList(SearchParametersDto parameters, List<PstTagsDto> singleLengthPstTagList)
        { //FIGURE 6: STEP 3:::: OBTAIN AMINO ACIDS CORRESPONDING TO FRAGMENT-PAIR DIFFERENCES
            //Hops having equal starting peak and ending peak values were joined together to form PST ladders
            int HopPeak;
            int minusonesingleTagsFound = singleLengthPstTagList.Count; // - 1;
            List<List<PstTagsDto>> DoubleTagPstTags = new List<List<PstTagsDto>>();


            //HERE FIRST GENERATING Duplicate Tags
            for (int HomePeak = 0; HomePeak < minusonesingleTagsFound - 1; HomePeak++)//HERE IS THE  STARTING OF MAKING MORE THAN ON PST TAGS...
            {

                for (int startIndexmultiple = 0; startIndexmultiple < minusonesingleTagsFound - 1; startIndexmultiple++)
                {
                    HopPeak = startIndexmultiple + 1;
                    {

                        if (singleLengthPstTagList[HomePeak].endIndex == singleLengthPstTagList[HopPeak].startIndex)  // CHECKING "ENDPOSITION" AND "STARTPOSITION" OF SINGLE LENGTH TAGS
                        {
                            List<PstTagsDto> TempList = new List<PstTagsDto>();
                            TempList.Add(new PstTagsDto(singleLengthPstTagList[HomePeak].startIndex, singleLengthPstTagList[HomePeak].endIndex,
                            singleLengthPstTagList[HomePeak].startIndexMass, singleLengthPstTagList[HomePeak].endIndexMass,
                            singleLengthPstTagList[HomePeak].massDifferenceBetweenPeaks, singleLengthPstTagList[HomePeak].AminoAcidSymbol,
                            singleLengthPstTagList[HomePeak].AminoAcidName, singleLengthPstTagList[HomePeak].TagError,
                            singleLengthPstTagList[HomePeak].averageIntensity)); //Home Peak will be attached with everyone...

                            TempList.Add(new PstTagsDto(singleLengthPstTagList[HopPeak].startIndex, singleLengthPstTagList[HopPeak].endIndex,
                            singleLengthPstTagList[HopPeak].startIndexMass, singleLengthPstTagList[HopPeak].endIndexMass,
                            singleLengthPstTagList[HopPeak].massDifferenceBetweenPeaks, singleLengthPstTagList[HopPeak].AminoAcidSymbol,
                            singleLengthPstTagList[HopPeak].AminoAcidName, singleLengthPstTagList[HopPeak].TagError,
                            singleLengthPstTagList[HopPeak].averageIntensity)); //Home Peak will be attached with everyone...

                            DoubleTagPstTags.Add(TempList);
                        }
                    }
                    if (singleLengthPstTagList[HomePeak].endIndex < singleLengthPstTagList[HopPeak].startIndex)
                    {
                        break;
                    }
                }
            }
            //AFTER GENERATING DUPLICATE TAGS NOW GENERATING MULTIPLE TAGS
            var MultipleTags = MultipleTagsGeneration(DoubleTagPstTags, parameters, 2, singleLengthPstTagList);


            //REMOVE TAGS WHICH ARE LESS THAN THE USER DEFINED MINIMUM PST TAG LENGTH
            var MultipleTagsProcessed = new List<List<PstTagsDto>>();
            for (int iPst = 0; iPst < MultipleTags.Count; iPst++)
            {
                if (parameters.MinimumPstLength <= MultipleTags[iPst].Count)
                {
                    MultipleTagsProcessed.Add(MultipleTags[iPst]);
                }

            }
            
            //var PstListSting = new List<string>();  //DELME
            //for (int iPst = 0; iPst < MultipleTagsProcessed.Count; iPst++)
            //{
            //    string Tag = "";
            //    for (int iter = 0; iter < MultipleTagsProcessed[iPst].Count; iter++)
            //    {
            //        Tag = Tag + MultipleTagsProcessed[iPst][iter].AminoAcidSymbol;
            //    }
            //    PstListSting.Add(Tag);
            //}
            return MultipleTagsProcessed;
        }

        private List<List<PstTagsDto>> MultipleTagsGeneration(List<List<PstTagsDto>> DoubleTagPstTags, SearchParametersDto parameters, int TagLength, List<PstTagsDto> singleLengthPstTagList)
        {
            for (int HopPeakIndex = 0; HopPeakIndex < DoubleTagPstTags.Count; HopPeakIndex++)
            {
                TagLength = DoubleTagPstTags[HopPeakIndex].Count;

                int DoubleTagEndIndex = DoubleTagPstTags[HopPeakIndex][TagLength - 1].endIndex;
                for (int SinglePstHomePeakIndex = 0; SinglePstHomePeakIndex < singleLengthPstTagList.Count; SinglePstHomePeakIndex++)
                {
                    if (DoubleTagEndIndex == singleLengthPstTagList[SinglePstHomePeakIndex].startIndex)  // CHECKING "ENDPOSITION" AND "STARTPOSITION" OF SINGLE LENGTH TAGS
                    {
                        if (parameters.MaximumPstLength > TagLength)
                        {
                            var temp = new List<PstTagsDto>(DoubleTagPstTags[HopPeakIndex]);

                            temp.Add(new PstTagsDto(singleLengthPstTagList[SinglePstHomePeakIndex].startIndex, singleLengthPstTagList[SinglePstHomePeakIndex].endIndex,
                            singleLengthPstTagList[SinglePstHomePeakIndex].startIndexMass, singleLengthPstTagList[SinglePstHomePeakIndex].endIndexMass,
                            singleLengthPstTagList[SinglePstHomePeakIndex].massDifferenceBetweenPeaks, singleLengthPstTagList[SinglePstHomePeakIndex].AminoAcidSymbol,
                            singleLengthPstTagList[SinglePstHomePeakIndex].AminoAcidName, singleLengthPstTagList[SinglePstHomePeakIndex].TagError,
                            singleLengthPstTagList[SinglePstHomePeakIndex].averageIntensity));

                            DoubleTagPstTags.Add(temp);
                        }
                    }
                    if (DoubleTagEndIndex < singleLengthPstTagList[SinglePstHomePeakIndex].startIndex)
                    {
                        break;
                    }
                }
            }

            var MultipleTags = new List<List<PstTagsDto>>(DoubleTagPstTags); //Because of referenced based deep clonning (here more precisely because of safe side).
            return MultipleTags;
        }

        public static List<List<PstTagsDto>> TrimPstTags(List<List<PstTagsDto>> multipleLenghtTagList, SearchParametersDto parameters)
        {
            //Break the larger Tags into all possible smaller tags
            //E.g. larger tag (SDTI)         divided into smaller tags         S,SD,SDT,SDTI
            //Break the larger Tags into all possible smaller tags
            // This breakage of PST is for Filtering the Tags according to Minimum-Maximum Range Length of PST

            List<List<PstTagsDto>> breakmultipleLenghtTags = new List<List<PstTagsDto>>();
            for (int outerindex = 0; outerindex <= multipleLenghtTagList.Count - 1; outerindex++)  // outer index is the index of big list in list of lists
            {
                int numberofelements = multipleLenghtTagList[outerindex].Count;
                var getcombinations = GetConsecutiveNumberCombination(numberofelements);  // this method will take just a Count(number of entries) and find its combinations


                for (int outerindex_getcombinations = 0; outerindex_getcombinations <= getcombinations.Count - 1; outerindex_getcombinations++) //0 1 2 
                {
                    List<PstTagsDto> temp = new List<PstTagsDto>();
                    for (int innerindex_getcombinations = 0; innerindex_getcombinations <= getcombinations[outerindex_getcombinations].Count - 1; innerindex_getcombinations++) // 0
                    {
                        int ExtractInnerIndex = getcombinations[outerindex_getcombinations][innerindex_getcombinations];

                        var subtempData = multipleLenghtTagList[outerindex][ExtractInnerIndex];

                        temp.Add(subtempData);
                    }
                    breakmultipleLenghtTags.Add(temp);
                }
            }

            ///////////////DELME  /// POINT-1
            var Tag2 = new List<string>();
            for (int i = 0; i < breakmultipleLenghtTags.Count; i++)
            {
                string tag = "";
                for (int j = 0; j < breakmultipleLenghtTags[i].Count; j++)
                {
                    tag = String.Concat(tag, breakmultipleLenghtTags[i][j].AminoAcidSymbol);
                }
                Tag2.Add(tag);

            }
            ///////////////DELME

            //Filtering the PST Tags:
            List<List<PstTagsDto>> filteredmultipleLenghtTags = new List<List<PstTagsDto>>(); //Filtering the Tags according to Minimum-Maximum Range Length of PST
            for (int i = 0; i <= breakmultipleLenghtTags.Count - 1; i++)
            {
                if (parameters.MaximumPstLength >= breakmultipleLenghtTags[i].Count && breakmultipleLenghtTags[i].Count >= parameters.MinimumPstLength)
                {
                    filteredmultipleLenghtTags.Add(breakmultipleLenghtTags[i]);
                }
            }


            ///////////////DELME  /// POINT-3
            var Tag3 = new List<string>();
            for (int i = 0; i < filteredmultipleLenghtTags.Count; i++)
            {
                string tag = "";
                for (int j = 0; j < filteredmultipleLenghtTags[i].Count; j++)
                {
                    tag = String.Concat(tag, filteredmultipleLenghtTags[i][j].AminoAcidSymbol);
                }
                Tag3.Add(tag);

            }
            ///////////////DELME
            ///
            return filteredmultipleLenghtTags;
        }

        public static List<List<int>> GetConsecutiveNumberCombination(int numberofelements)
        {
            List<int> listofnumbers = new List<int>(); // Generating list, range of 0 to numberofelements
            for (int indexlist = 0; indexlist <= numberofelements - 1; indexlist++)
            {
                listofnumbers.Add(indexlist);
            }

            var consecutivenumbers = new List<List<int>>();

            double count = Math.Pow(2, listofnumbers.Count);
            for (int i = 1; i <= count - 1; i++) // This loop is making all statistics' Combinations of list elements but just storing that numbers which are consecutives
            {
                string int2binary = Convert.ToString(i, 2).PadLeft(listofnumbers.Count, '0');
                var subresults = new List<int>();
                for (int j = 0; j < int2binary.Length; j++)
                {
                    if (int2binary[j] == '1')
                    {
                        subresults.Add(listofnumbers[j]);
                    }
                }
                var temporarylist = subresults;
                bool isConsecutive = !temporarylist.Select((z, j) => z - j).Distinct().Skip(1).Any();//Checking for consecutive numbers
                if (isConsecutive == true)
                    consecutivenumbers.Add(subresults); //Just collecting that numbers which are consecutives
            }
            return consecutivenumbers;
        }

        public static List<Psts> PstTagInfoList(List<List<PstTagsDto>> LadderList, SearchParametersDto parameters)
        {
            string psttags;
            int psttaglength;
            double PstTagErrorSum;
            double pstintensity;
            double rootmeansquareerror;


            var psttaginfolist = new List<Psts>();

            for (int indexouterLadderList = 0; indexouterLadderList <= LadderList.Count - 1; indexouterLadderList++)
            {
                psttaglength = LadderList[indexouterLadderList].Count;
                psttags = "";
                PstTagErrorSum = 0;
                pstintensity = 0;

                for (int indexinnerLadderList = 0; indexinnerLadderList <= LadderList[indexouterLadderList].Count - 1; indexinnerLadderList++)
                {
                    psttags = psttags + LadderList[indexouterLadderList][indexinnerLadderList].AminoAcidSymbol;

                    PstTagErrorSum = PstTagErrorSum + Math.Pow(LadderList[indexouterLadderList][indexinnerLadderList].TagError, 2);
                    pstintensity = pstintensity + LadderList[indexouterLadderList][indexinnerLadderList].averageIntensity;  //averageIntensity= mean intensity of 2 peaks(home peak and hop peak)
                }
                rootmeansquareerror = (Math.Pow(PstTagErrorSum, .5) / psttaglength) * 10;
                pstintensity = pstintensity / psttaglength;                                             ///Panga Modlsdajl
                var psttaginfo = new Psts(psttaglength, psttags, PstTagErrorSum, rootmeansquareerror, pstintensity);

                psttaginfolist.Add(psttaginfo);

            }

            return psttaginfolist;
        }

        public List<PstTagList> FilteredPsts(List<Psts> UniquePstTagInfoList, SearchParametersDto parameters) //Filter tags according to fulltag error threshold
        {
            List<PstTagList> FinalPstTagList = new List<PstTagList>();

            for (int indexUniquePstTagInfoList = 0; indexUniquePstTagInfoList <= UniquePstTagInfoList.Count - 1; indexUniquePstTagInfoList++)
            {
                // Filtering tags According to PST Tolerance & Scoring Tag Length
                if (UniquePstTagInfoList[indexUniquePstTagInfoList].rootmeansquareerror <= parameters.PSTTolerance)
                {
                    var ScoreError = Math.Exp(-2 * UniquePstTagInfoList[indexUniquePstTagInfoList].rootmeansquareerror);

                    double ScoreForLength = Math.Pow(UniquePstTagInfoList[indexUniquePstTagInfoList].psttaglength, 2);
                    double PstTagFrequency = UniquePstTagInfoList[indexUniquePstTagInfoList].PstTagIntensity * ScoreForLength;

                    var tempList = new PstTagList(UniquePstTagInfoList[indexUniquePstTagInfoList].psttaglength, UniquePstTagInfoList[indexUniquePstTagInfoList].psttags, ScoreError, PstTagFrequency);
                    FinalPstTagList.Add(tempList);
                }
            }
            return FinalPstTagList;
        }


        public static List<Psts> UniquePsts(List<Psts> psttaginfolist)
        {
            /* //Updated 20201228  BELOW */
            //PST SAME RMSE ALSO, SAME then, we select that PST having more intensity
            var uniquepst = psttaginfolist.Select(checktag => checktag.psttags).Distinct().ToList();
            uniquepst.Sort();  //Sorting based on alphabetic order just for simplicity  //Updated 20201228
            psttaginfolist = psttaginfolist.OrderBy(x => x.psttags).ToList(); //Sorting based on alphabetic order just for simplicity // Updated 20201228 //This will store data about Unique Pst Tag Information

            var UniquePstTagInfoList = new List<Psts>(uniquepst.Count);
            try
            {


                if (uniquepst.Count != psttaginfolist.Count)
                {
                    //int firstIndex = 0;
                    int secondIndexForBreakLoop = -1;  //-1 is just for initlization because psttaginfolist.Count cannot be -1 
                                                       //while (true)
                    string LastTag = "";
                    for (int firstIndex = 0; firstIndex < psttaginfolist.Count - 1; firstIndex++)
                    {
                        var firstErrorSum = psttaginfolist[firstIndex].PstTagErrorSum;
                        var firstTag = psttaginfolist[firstIndex].psttags;

                        if (LastTag == firstTag)  // Used to avoid same tag comparison in firstIndex loop
                        {
                            continue;
                        }
                        //int secondIndex = firstIndex + 1;
                        LastTag = firstTag;

                        bool AddEntry = true;  //We want to add just one Tag Info into the UniquePstTagInfoList therefore, applying filter in form of this...
                        bool ErrorSumChecked = false; //This boolean value will be used if firstErrorSum < secondErrorSum  OR  firstErrorSum > secondErrorSum already checked then, intensity will not be checked in next iterations 
                        for (int secondIndex = firstIndex + 1; secondIndex < psttaginfolist.Count; secondIndex++)
                        {
                            secondIndexForBreakLoop = secondIndex;
                            var secondErrorSum = psttaginfolist[secondIndex].PstTagErrorSum;
                            var secondTag = psttaginfolist[secondIndex].psttags;

                            if (AddEntry == true)
                            {
                                UniquePstTagInfoList.Add(psttaginfolist[firstIndex]);
                                AddEntry = false;
                            }

                            if (firstTag == secondTag)
                            {
                                if (firstErrorSum < secondErrorSum)
                                {
                                    UniquePstTagInfoList[UniquePstTagInfoList.Count - 1] = psttaginfolist[firstIndex];
                                    ErrorSumChecked = true;
                                    //psttaginfolist.RemoveAt(secondIndex);
                                }
                                else if (firstErrorSum > secondErrorSum)
                                {
                                    UniquePstTagInfoList[UniquePstTagInfoList.Count - 1] = psttaginfolist[secondIndex];
                                    ErrorSumChecked = true;
                                    //psttaginfolist.RemoveAt(firstIndex);
                                }
                                else // (firstErrorSum == secondErrorSum)
                                {
                                    if (psttaginfolist[firstIndex].PstTagIntensity <= psttaginfolist[secondIndex].PstTagIntensity && ErrorSumChecked == false)// || psttaginfolist[firstIndex].PstTagIntensity == psttaginfolist[secondIndex].PstTagIntensity)
                                    {
                                        UniquePstTagInfoList[UniquePstTagInfoList.Count - 1] = psttaginfolist[firstIndex];

                                        //psttaginfolist[firstIndex] = psttaginfolist[secondIndex];
                                        //psttaginfolist.RemoveAt(secondIndex);
                                    }
                                    else if (psttaginfolist[firstIndex].PstTagIntensity > psttaginfolist[secondIndex].PstTagIntensity && ErrorSumChecked == false)
                                    {
                                        UniquePstTagInfoList[UniquePstTagInfoList.Count - 1] = psttaginfolist[secondIndex];

                                    }
                                }
                            }
                            else  //WHEN TAG HAS NOT REPETITION THEN, ADD IT INTO      UniquePstTagInfoList    AS IT IS...
                            {
                                //if(UpdateEntry == true)
                                //{
                                //    UniquePstTagInfoList.Add(psttaginfolist[firstIndex]);
                                //}
                                firstIndex = secondIndex - 1;

                                if (firstIndex == psttaginfolist.Count - 2 && firstTag != secondTag) //Updated 20210105 Bug Fix
                                {
                                    UniquePstTagInfoList.Add(psttaginfolist[secondIndex]);
                                }
                                break;
                            }

                        }
                        if (secondIndexForBreakLoop == psttaginfolist.Count)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    UniquePstTagInfoList = psttaginfolist; //Updated 20201230
                }
            }
            catch (Exception e)
            {
                int fsadf = 1;
            }
            return UniquePstTagInfoList;
        }

        public static List<Psts> AccomodateIsoforms(List<Psts> FinalPstTagList, SearchParametersDto parameters)
        {
            char[] ResidueForReplacement = { 'L', 'D', 'N', 'E', 'Q' };
            string newResidue;
            //List<Psts> NewAccomodatedPstTagList = new List<Psts>();

            for (int indexFinalPstTagList = 0; indexFinalPstTagList <= FinalPstTagList.Count - 1; indexFinalPstTagList++)   //Pura Chlana ha islya...
            {
                List<Psts> RowofFinalPstTagList = new List<Psts>();
                RowofFinalPstTagList.Add(FinalPstTagList[indexFinalPstTagList]);



                for (int indexResidueAA = 0; indexResidueAA <= ResidueForReplacement.Length - 1; indexResidueAA++)                //ResidueForReplacement wise Chlana ha islya 
                {
                    char OldResidue = ResidueForReplacement[indexResidueAA];
                    string BeforeAccomodatePst = RowofFinalPstTagList[0].psttags;

                    if (BeforeAccomodatePst.Contains(OldResidue))
                    {
                        newResidue = " ";

                        if (OldResidue == 'L')//Here I think Switch Case will be more better....!!!!
                            newResidue = "I";
                        else if (OldResidue == 'D')
                            newResidue = "B";
                        else if (OldResidue == 'N')
                            newResidue = "B";
                        else if (OldResidue == 'E')
                            newResidue = "Z";
                        else if (OldResidue == 'Q' && parameters.HopThreshhold <= 1.5)
                            newResidue = "Z";
                        else if (OldResidue == 'Q' && parameters.HopThreshhold > 1.5)
                            newResidue = "K";


                        for (int iter = 0; iter <= BeforeAccomodatePst.Length - 1; iter++)
                        {
                            if (BeforeAccomodatePst[iter] == OldResidue)
                            {
                                var ResidueRemoved = BeforeAccomodatePst.Remove(iter, 1);
                                var ResidueInserted = ResidueRemoved.Insert(iter, newResidue);

                                var AccomodatedPstTag = ResidueInserted;
                                var tempD = new Psts(FinalPstTagList[indexFinalPstTagList].psttaglength,
                                    AccomodatedPstTag, FinalPstTagList[indexFinalPstTagList].PstTagErrorSum,
                                    FinalPstTagList[indexFinalPstTagList].rootmeansquareerror, FinalPstTagList[indexFinalPstTagList].PstTagIntensity);

                                FinalPstTagList.Add(tempD);   //Updated 20210305  // Bug Removed Now will ZRL, ZRI, ERI, ZRI against ERL Tag
                                BeforeAccomodatePst = AccomodatedPstTag;// Just for using same PST Tag for further Accommodation.......
                            }

                        }
                    }
                }
            }
            //FinalPstTagList.AddRange(NewAccomodatedPstTagList);
            return FinalPstTagList;
        }
    }




    public class peakData2Dlist
    {//(double Mass, double Intenstity){
        public double Mass { get; set; }
        public double Intensity { get; set; }

        public peakData2Dlist(double i, double j)  /*!< Parameterized constructor */
        {
            Mass = i;
            Intensity = j;
        }
        //*** WHAT IS THE PURPOSE OF DEFAULT CONSTRUCTOR?***//
        public peakData2Dlist()
        {
            Mass = 0;
            Intensity = 0;
        }
    }

}