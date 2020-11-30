using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public class Evalue
    {
        ProbabilityOfSequence _ProbabilityOfSequence = new ProbabilityOfSequence();
        public void ComputeEvalue(List<ProteinDto> CandidateProteinsList)
        {
            List<double> ProbOfSequence = new List<double>();
            List<int> SpectralMatches = new List<int>();
            
            List<bool> LogicalMatchCounterList = new List<bool>();
            List<bool> SetLogicalMatchCounterList = new List<bool>();  //  Will be used for setting E-Value
            List<int> setind = new List<int>();

            //PrSM_Evalue.m
            for (int i = 0; i < CandidateProteinsList.Count; i++)
            {
                ProbOfSequence.Add(_ProbabilityOfSequence.ProabailityAA(CandidateProteinsList[i].Sequence));
                SpectralMatches.Add(CandidateProteinsList[i].MatchCounter);
            }

            List<int> USpectralMatches = SpectralMatches.Distinct().ToList();
            USpectralMatches.Sort();
            USpectralMatches.Reverse();

            for (int i = 0; i < USpectralMatches.Count; i++)
            {
                int Matches = USpectralMatches[i];
                LogicalMatchCounterList = new List<bool>();
                for (int iteration = 0; iteration < SpectralMatches.Count; iteration++)
                {
                    if (Matches == SpectralMatches[iteration])
                    {
                        LogicalMatchCounterList.Add(true);
                        //LogicalMatchCounterList[iteration] = true;
                    }
                    else
                    {
                        LogicalMatchCounterList.Add(false);
                        //LogicalMatchCounterList.Insert(iteration, false);
                        //LogicalMatchCounterList[iteration] = false;
                    }
                }

                //var tempSetLogicalMatchCounterList = Clone.CloneObject(LogicalMatchCounterList); // Lists are referenced based so, therefore...   //Updated 20201113
                //SetLogicalMatchCounterList = Clone.Decrypt<List<bool>>(tempSetLogicalMatchCounterList); // Lists are referenced based so, therefore...    //Updated 20201113
                SetLogicalMatchCounterList = Clone.DeepClone<List<bool>>(LogicalMatchCounterList);    //Updated 20201113


                List<int> iMinusList = new List<int>();
                if (i > 0)
                {
                    
                    for (int iMinus = i-1 ; iMinus >= 0; iMinus--)
                    {
                        List<bool> tempLogicalMatchCounterList = new List<bool>();  //Updated 20200813
                        iMinusList.Add(iMinus);
                        for (int iteration = 0; iteration < SpectralMatches.Count; iteration++)
                        {
                            if (SpectralMatches[iteration] == USpectralMatches[iMinus])
                            {
                                tempLogicalMatchCounterList.Add(true);
                            }
                            else
                            {
                                tempLogicalMatchCounterList.Add(false);
                            }
                        }

                        for (int index = 0; index < LogicalMatchCounterList.Count; index++)
                        {
                            LogicalMatchCounterList[index] = LogicalMatchCounterList[index] || tempLogicalMatchCounterList[index];
                        }
                            

                    }
                }

                double eval = 0.0;
                int counter = 0;
                for (int j = 0; j < SpectralMatches.Count; j++)
                {
                    if (LogicalMatchCounterList[j] == true)
                    {
                        eval = eval + ProbOfSequence[j];
                        counter = counter + 1;
                    }
                }
                eval = counter * eval;

                for (int iter = 0; iter < SpectralMatches.Count; iter++)
                {
                    if (SetLogicalMatchCounterList[iter] == true)
                    {
                        if (CandidateProteinsList[iter].Truncation == "None")
                        {
                            CandidateProteinsList[iter].Evalue = eval;
                        }
                        else
                        {
                            CandidateProteinsList[iter].Evalue = eval * 0.693;
                        }
                    }
                }
            }
        }
    }
}