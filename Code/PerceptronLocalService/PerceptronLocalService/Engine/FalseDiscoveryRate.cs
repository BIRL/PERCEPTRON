using System;
using System.Collections.Generic;
using System.Linq;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;
using System.Data;
using System.Text.RegularExpressions;

namespace PerceptronLocalService.Engine
{
    public class FalseDiscoveryRate //: IFalseDiscoveryRate
    {
        public List<FalseDiscoveryRateDto> FDR(double FDRCutOff, List<FalseDiscoveryRateDto> TargetList, List<FalseDiscoveryRateDto> DecoyList)
        {
            int TargetListCount = TargetList.Count();   // Updated 20210213
            var ResultsOfFDR = new List<FalseDiscoveryRateDto>(TargetList.Count);
            List<Tuple<string, double>> Decoy = new List<Tuple<string, double>>(); //this was a bug that i am not removing for now. it should be DecoyData.Rows.Count
            double[] Threshold = new double[TargetList.Count];
            for (int i = 0; i < DecoyList.Count; i++)
            {
                var tuple = Tuple.Create(DecoyList[i].FileName, DecoyList[i].Score);
                Decoy.Add(tuple);
            }
            Decoy.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            double EvalueThreshold = Math.Pow(10, -10);
            var eValCount = 0;
            for (int iter = 0; iter < TargetList.Count; iter++)
            {
                if (TargetList[iter].Evalue <= EvalueThreshold)
                {
                    eValCount = eValCount + 1;
                }

            }


            List<Tuple<string, double>> Target = new List<Tuple<string, double>>(); //this was a bug that i am not removing for now. it should be DecoyData.Rows.Count
            for (int i = 0; i < TargetList.Count; i++)
            {
                var tuple = Tuple.Create(TargetList[i].FileName, TargetList[i].Score);
                Target.Add(tuple);
                Threshold[i] = TargetList[i].Score;
            }
            Target.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            FDRCutOff = FDRCutOff / 100;
            List<decimal> FDR_Estimation = new List<decimal>(); // Updated 20210213

            for (int i = 0; i < TargetList.Count; i++)
            {
                List<Tuple<string, double>> TargetShortListed = new List<Tuple<string, double>>();
                List<Tuple<string, double>> DecoyShortListed = new List<Tuple<string, double>>();
                var NumOfTarget = 0;
                var NumOfDecoy = 0;
                int TargetCount = 0;
                int DecoyCount = 0;

                for (int j = 0; j < Decoy.Count; j++)  //No need of this {{ (Decoy.Count) / 2 }}  BUT ONLY THIS {{ Decoy.Count }}
                {
                    if (Decoy[j].Item2 > Threshold[i])
                    {
                        DecoyShortListed.Add(Decoy[j]);
                    }
                }
                for (int k = 0; k < Target.Count; k++)  //No need of this {{ (Target.Count) / 2 }}  BUT ONLY THIS {{ Target.Count }}
                {
                    if (Target[k].Item2 >= Threshold[i])
                    {
                        TargetShortListed.Add(Target[k]);
                    }
                }

                if (DecoyShortListed.Count == 0 || TargetShortListed.Count == 0)
                {
                    NumOfTarget = TargetShortListed.Count;// / 2;
                    NumOfDecoy = DecoyShortListed.Count;// / 2;
                }

                if (DecoyShortListed.Count != 0 && TargetShortListed.Count != 0)
                {

                    var Target_Elements = TargetShortListed.Where(p => DecoyShortListed.All(p2 => p2.Item1 != p.Item1));
                    var Decoy_Elements = DecoyShortListed.Where(p => TargetShortListed.All(p2 => p2.Item1 != p.Item1));
                    NumOfTarget = Target_Elements.Count();
                    NumOfDecoy = Decoy_Elements.Count();
                    var CommonElements = TargetShortListed.Select(s1 => s1.Item1).ToList().Intersect(DecoyShortListed.Select(s1 => s1.Item1).ToList()).ToList();

                    for (int index = 0; index < CommonElements.Count; index++)
                    {
                        double DecoyScore = 0;
                        double TargetScore = 0;
                        for (int ind1 = 0; ind1 < DecoyShortListed.Count; ind1++)
                        {
                            if (DecoyShortListed[ind1].Item1 == CommonElements[index])
                            {
                                DecoyScore = DecoyShortListed[ind1].Item2;
                                break;
                            }
                        }
                        for (int ind2 = 0; ind2 < TargetShortListed.Count; ind2++)
                        {
                            if (TargetShortListed[ind2].Item1 == CommonElements[index])
                            {
                                TargetScore = TargetShortListed[ind2].Item2;
                                break;
                            }
                        }
                        if (TargetScore >= DecoyScore)
                        {
                            TargetCount = TargetCount + 1;
                        }
                        else
                        {
                            DecoyCount = DecoyCount + 1;
                        }
                    }
                }
                decimal FDRValue = (Convert.ToDecimal((2 * DecoyCount) + NumOfDecoy) / Convert.ToDecimal(NumOfTarget + TargetCount + DecoyCount));  // Updated 20210213
                FDR_Estimation.Add(FDRValue);
                if (FDRValue > Convert.ToDecimal(FDRCutOff))    // Updated 20210213
                    break;
            }


            TargetList.RemoveAt(TargetList.Count - 1);  // Removing Last entry means ---- Ref in SPECTRUM: {{  numel(FDR_estimation)-1  }}
            FDR_Estimation.RemoveAt(FDR_Estimation.Count - 1);
            int NoOfUniqueProteins = TargetList.Select(x => x.Header).Distinct().ToList().Count;
            //SKIPPING HERE THIS {{ protein = TargetDataWithFDR.ProteinHeader; }} FROM SPECTRUM WILL Just Return No of Proteins
            var tempResultsOfFDR = new FalseDiscoveryRateDto(TargetListCount, TargetList, FDR_Estimation, TargetList.Count, NoOfUniqueProteins, eValCount);   // Updated 20210213
            ResultsOfFDR.Add(tempResultsOfFDR);
            return ResultsOfFDR;
        }
    }
}



//List<object> unique_protein = protein.Distinct().ToList();
//int protein_count = unique_protein.Count;

//var complete_targetfile_with_fdr = target_filepath.Replace(".csv", "_FDR.csv");

//StreamWriter sw = new StreamWriter(complete_targetfile_with_fdr, false);
////CREATING CSV FILE
//try
//{
//    int columnCount = TargetDataWithFDR.Columns.Count;

//    for (int i = 0; i < columnCount; i++)
//    {
//        sw.Write(TargetDataWithFDR.Columns[i]);

//        if (i < columnCount - 1)
//        {
//            sw.Write(",");
//        }
//    }

//    sw.Write(sw.NewLine);

//    foreach (DataRow dr in TargetDataWithFDR.Rows)
//    {
//        for (int i = 0; i < columnCount; i++)
//        {
//            if (!Convert.IsDBNull(dr[i]))
//            {
//                sw.Write(dr[i].ToString());
//            }

//            if (i < columnCount - 1)
//            {
//                sw.Write(",");
//            }
//        }

//        sw.Write(sw.NewLine);
//    }


//}
//catch (Exception ex)
//{
//    throw ex;
//}

//var expProteinString = @"Total protein count reported by Experiment: " + Convert.ToString(sorted_target.Rows.Count);
//var evalProteinString = @"Total protein count reported by Experiment with E-value greater than 1E-10: " + Convert.ToString(evalue_count);
//var TotalProteinString = @"Total protein reported with: " + Convert.ToString(cutoff_actual) + @"% FDR: " + Convert.ToString(protein.Count);
//var ProteinString = @"Unique proteins count reported with " + Convert.ToString(cutoff_actual) + @"% FDR: " + Convert.ToString(protein_count);

//sw.WriteLine();
//sw.WriteLine(expProteinString);
//sw.WriteLine(evalProteinString);
//sw.WriteLine(TotalProteinString);
//sw.WriteLine(ProteinString);
//sw.Close();























//var ListOFDRPoteins = new List<ProteinDto>();
//return ListOFDRPoteins;






//BACK IN COMPUTEFDR.M
//DataTable TargetDataWithFDR = new DataTable();
//foreach (string header in headers_decoy)
//{
//    TargetDataWithFDR.Columns.Add(header.Trim());
//}
//int counter = 1;
//foreach (DataRow dr in sorted_target.Rows)
//{
//    if (counter < FDR_estimation.Count)
//    {
//        TargetDataWithFDR.Rows.Add(dr.ItemArray);
//        counter++;
//    }
//    else
//    {
//        break;
//    }
//}

//TargetDataWithFDR.Columns.Add("FDR", typeof(System.Double));
//int count = 0;
//foreach (DataRow row in TargetDataWithFDR.Rows)
//{
//    row["FDR"] = FDR_estimation[count];
//    count++;
//}

//List<object> protein = new List<object>();
//foreach (DataRow row in TargetDataWithFDR.Rows)
//{
//    protein.Add(row["Protein Header"]);
//}