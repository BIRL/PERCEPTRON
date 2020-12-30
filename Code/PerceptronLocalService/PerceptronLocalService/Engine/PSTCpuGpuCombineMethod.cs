using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Engine;

namespace PerceptronLocalService.Engine
{
    class PSTCpuGpuCombineMethod
    {
        public static List<List<PstTagsDto>> TrimPstTags(List<List<PstTagsDto>> multipleLenghtTagList, SearchParametersDto parameters)
        {
            ///////////////DELME  /// POINT-0
            var Tag = new List<string>();
            for (int i = 0; i < multipleLenghtTagList.Count; i++)
            {
                string tag = "";
                for (int j = 0; j < multipleLenghtTagList[i].Count; j++)
                {
                    tag = String.Concat(tag, multipleLenghtTagList[i][j].AminoAcidSymbol);
                }
                Tag.Add(tag);

            }
            ///////////////DELME


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

        public static List<PstTagList> PstTagInfoList(List<List<PstTagsDto>> LadderList, SearchParametersDto parameters)
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

            ///////////////DELME  /// POINT-3
            var Tag4 = new List<string>();
            for (int i = 0; i < psttaginfolist.Count; i++)
            {
                //string tag = "";
                //for (int j = 0; j < psttaginfolist[i].Count; j++)
                //{
                //    tag = String.Concat(tag, psttaginfolist[i][j].AminoAcidSymbol);
                //}
                //tag = String.Concat(tag, psttaginfolist[i].psttags);
                Tag4.Add(psttaginfolist[i].psttags);

            }
            ///////////////DELME 


            //Finding the Unique PSTs: Remove all other redundant PSTs but keep only one having lowest Root Mean Square Error(RMSE)
            //If 2 or more Tags are same then also keep just one

            /* //Updated 20201228  BELOW */
            //PST SAME RMSE ALSO, SAME then, we select that PST having more intensity
            var uniquepst = psttaginfolist.Select(checktag => checktag.psttags).Distinct().ToList();
            uniquepst.Sort();  //Sorting based on alphabetic order just for simplicity  //Updated 20201228
            psttaginfolist = psttaginfolist.OrderBy(x=>x.psttags).ToList(); //Sorting based on alphabetic order just for simplicity // Updated 20201228 //This will store data about Unique Pst Tag Information

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
                                    UniquePstTagInfoList[UniquePstTagInfoList.Count-1] = psttaginfolist[firstIndex];
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
            /* //Updated 20201228  ABOVE */

            /////////* //Updated 20201214  BELOW */
            //////////PST SAME RMSE ALSO, SAME then, we select that PST having more intensity
            ////////var uniquepst = psttaginfolist.Select(test => test.psttags).Distinct().ToList();
            ////////var UniquePstTagInfoList = psttaginfolist; //This will store data about Unique Pst Tag Information
            ////////if (uniquepst.Count() != psttaginfolist.Count())
            ////////{
            ////////    int firstIndex = 0;
            ////////    while (true)
            ////////    {
            ////////        var firstErrorSum = UniquePstTagInfoList[firstIndex].PstTagErrorSum;
            ////////        var firstTag = UniquePstTagInfoList[firstIndex].psttags;
            ////////        int secondIndex = firstIndex + 1;
            ////////        while (secondIndex < UniquePstTagInfoList.Count())
            ////////        {
            ////////            var secondErrorSum = UniquePstTagInfoList[secondIndex].PstTagErrorSum;
            ////////            var secondTag = UniquePstTagInfoList[secondIndex].psttags;

            ////////            /////DELME 
            ////////            if (firstTag == "GLE")
            ////////            {
            ////////                int eas = 1;
            ////////            }
            ////////            if (secondTag == "GLE")
            ////////            {
            ////////                int waithere = 0;

            ////////            }
            ////////            /////DELME 


            ////////            if (firstTag == secondTag)
            ////////            {
            ////////                if (firstErrorSum < secondErrorSum)
            ////////                {
            ////////                    UniquePstTagInfoList.RemoveAt(secondIndex);
            ////////                }
            ////////                else if (firstErrorSum > secondErrorSum)
            ////////                {
            ////////                    UniquePstTagInfoList.RemoveAt(firstIndex);
            ////////                }
            ////////                else // (firstErrorSum == secondErrorSum)
            ////////                {
            ////////                    if (UniquePstTagInfoList[firstIndex].PstTagIntensity < UniquePstTagInfoList[secondIndex].PstTagIntensity || UniquePstTagInfoList[firstIndex].PstTagIntensity == UniquePstTagInfoList[secondIndex].PstTagIntensity)
            ////////                    {
            ////////                        UniquePstTagInfoList[firstIndex] = UniquePstTagInfoList[secondIndex];
            ////////                        UniquePstTagInfoList.RemoveAt(secondIndex);
            ////////                    }
            ////////                    else
            ////////                    {
            ////////                        UniquePstTagInfoList.RemoveAt(secondIndex);
            ////////                    }
            ////////                }
            ////////            }
            ////////            else
            ////////            {
            ////////                secondIndex++;
            ////////            }
            ////////        }
            ////////        if (firstTag == UniquePstTagInfoList[firstIndex].psttags)
            ////////            firstIndex++;
            ////////        if (firstIndex >= UniquePstTagInfoList.Count() - 1)
            ////////        {
            ////////            break;
            ////////        }
            ////////    }
            ////////}
            /////////* //Updated 20201214  ABOVE */

            //var uniquepst = psttaginfolist.Select(test => test.psttags).Distinct().ToList();
            //var UniquePstTagInfoList = new List<Psts>(); //This will store data about Unique Pst Tag Information    ////////IF POSSIBLE ASSIGN THE SIZE OF "uniquepst.Count"

            //for (int indexuniquepst = 0; indexuniquepst <= uniquepst.Count - 1; indexuniquepst++)  // This loop will run on the Unique(distinct) PSTTAGS
            //{
            //    var uniquepsttag = uniquepst[indexuniquepst]; //Each Unique(distinct) PSTTAG will be taken for selecting all corresponding data against this PSTTAG 

            //    var UniquePstTagMinError = (from c in psttaginfolist
            //                                group c by c.psttags into g
            //                                where g.Key == uniquepsttag
            //                                select new { psttags = g.Key, MinPstTagErrorSum = g.Select(m => m.PstTagErrorSum).Min() }).ToList(); // PSTTAG SELECTED; Select That list's Row which have Minimum "PST TAG ERROR SUM"

            //    for (int indexpsttaginfolist = 0; indexpsttaginfolist <= psttaginfolist.Count - 1; indexpsttaginfolist++)
            //    {
            //        if (psttaginfolist[indexpsttaginfolist].psttags == uniquepsttag && psttaginfolist[indexpsttaginfolist].PstTagErrorSum == UniquePstTagMinError[0].MinPstTagErrorSum)
            //        {
            //            var temporaryData = psttaginfolist[indexpsttaginfolist];
            //            UniquePstTagInfoList.Add(psttaginfolist[indexpsttaginfolist]);
            //            break;  //If 2 or more Tags are same then, keep just one AND BREAK THE LOOP...
            //        }
            //    }
            //}


            /////////////DEL ME POINT-6
            ///
            var TagName = new List<string>(UniquePstTagInfoList.Count);
            for (int i = 0; i < UniquePstTagInfoList.Count; i++)
            {
                TagName.Add(UniquePstTagInfoList[i].psttags);
            }

            /// 
            /////////////DEL ME 

            //Filter tags according to fulltag error threshold
            var FilteredTagList = new List<Psts>();
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


        public static List<PstTagList> AccomodateIsoforms(List<PstTagList> FinalPstTagList, SearchParametersDto parameters)
        {
            char[] ResidueForReplacement = { 'L', 'D', 'N', 'E', 'Q' };
            string newResidue;
            List<PstTagList> NewAccomodatedPstTagList = new List<PstTagList>();

            for (int indexFinalPstTagList = 0; indexFinalPstTagList <= FinalPstTagList.Count - 1; indexFinalPstTagList++)   //Pura Chlana ha islya...
            {
                List<PstTagList> RowofFinalPstTagList = new List<PstTagList>();
                RowofFinalPstTagList.Add(FinalPstTagList[indexFinalPstTagList]);



                for (int indexResidueAA = 0; indexResidueAA <= ResidueForReplacement.Length - 1; indexResidueAA++)                //ResidueForReplacement wise Chlana ha islya 
                {
                    char OldResidue = ResidueForReplacement[indexResidueAA];
                    string BeforeAccomodatePst = RowofFinalPstTagList[0].PstTags;

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
                                var tempD = new PstTagList(FinalPstTagList[indexFinalPstTagList].PstTagLength, AccomodatedPstTag, FinalPstTagList[indexFinalPstTagList].PstErrorScore, FinalPstTagList[indexFinalPstTagList].PstFrequency);

                                NewAccomodatedPstTagList.Add(tempD);
                                BeforeAccomodatePst = AccomodatedPstTag;// Just for using same PST Tag for further Accommodation.......
                            }

                        }
                    }
                }
            }
            FinalPstTagList.AddRange(NewAccomodatedPstTagList);
            return FinalPstTagList;
        }
    }
}
