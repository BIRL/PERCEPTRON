using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;

namespace PerceptronAPI.Engine
{
    public class FDR
    {
        public void MainFDR()
        {
            try
            {
                //var e_value_threshold;//= 10^-10
                //Converting TARGET FILE to DATATABLE and sorting it.
                var target_filepath = @"D:\Others\FDR script\Interpreting results\Target.csv";
                StreamReader sr_target = new StreamReader(target_filepath);
                string[] headers_target = sr_target.ReadLine().Split(',');
                DataTable TargetData = new DataTable();
                foreach (string header in headers_target)
                {
                    TargetData.Columns.Add(header.Trim());
                }

                while (!sr_target.EndOfStream)
                {
                    string[] rows = Regex.Split(sr_target.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = TargetData.NewRow();
                    for (int i = 0; i < headers_target.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    TargetData.Rows.Add(dr);
                }
                //Sorting 

                DataView dv_target = TargetData.DefaultView;
                dv_target.Sort = "Score desc";
                DataTable sorted_target = dv_target.ToTable();
                //END OF SORTING TARGET FILE

                //Converting DECOY FILE to DATATABLE and sorting it.
                StreamReader sr_decoy = new StreamReader(@"D:\Others\FDR script\Interpreting results\Decoy.csv");
                string[] headers_decoy = sr_decoy.ReadLine().Split(',');
                DataTable DecoyData = new DataTable();
                foreach (string header in headers_decoy)
                {
                    DecoyData.Columns.Add(header.Trim());
                }

                while (!sr_decoy.EndOfStream)
                {
                    string[] rows = Regex.Split(sr_decoy.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = DecoyData.NewRow();
                    for (int i = 0; i < headers_decoy.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    DecoyData.Rows.Add(dr);
                }
                //Sorting
                DataView dv_decoy = DecoyData.DefaultView;
                dv_decoy.Sort = "Score desc";
                DataTable sorted_decoy = dv_decoy.ToTable();
                //END OF SORTING DECOY FILE

                //MAKING DECOY TUPLE ARRAY
                Tuple<object, double>[] Decoy = new Tuple<object, double>[TargetData.Rows.Count]; //this was a bug that i am not removing for now. it should be DecoyData.Rows.Count
                for (int i = 0; i < TargetData.Rows.Count; i++)
                {
                    var d_score = Math.Round(Convert.ToDouble(sorted_decoy.Rows[i][6]), 4);
                    var tuple = Tuple.Create(sorted_decoy.Rows[i][0], d_score);
                    Decoy[i] = tuple;
                }//END OF MAKING DECOY TUPLE ARRAY

                //HAVE TO DO SORT ROWS?????

                var evalue_threshold = Math.Pow(10, -10);
                var evalue_count = 0;

                //MAKING TARGET TUPLE ARRAY
                Tuple<object, double>[] Target = new Tuple<object, double>[TargetData.Rows.Count];
                for (int i = 0; i < TargetData.Rows.Count; i++)
                {
                    var t_score = Math.Round(Convert.ToDouble(sorted_target.Rows[i][6]), 4);
                    var tuple = Tuple.Create(sorted_target.Rows[i][0], t_score);
                    Target[i] = tuple;

                    if (Convert.ToDouble(sorted_target.Rows[i][11]) <= evalue_threshold)
                    {
                        evalue_count = evalue_count + 1;
                    }
                }//END OF MAKING TARGET TUPLE ARRAY

                //HAVE TO DO SORT ROWS???

                //Replicating THRESHOLD
                double[] threshold = new double[TargetData.Rows.Count];
                for (int i = 0; i < TargetData.Rows.Count; i++)
                {
                    threshold[i] = Target[i].Item2;
                }

                //HAVE TO SORT ROWS OF TARGET???
                double cutoff = 1; //WILL GET FROM USER LATER ON.
                double cutoff_actual = cutoff;

                //CODE FOR FDR.M STARTS HERE

                List<double> FDR_estimation = new List<double>();
                cutoff = cutoff / 100.0;
                Console.WriteLine(threshold.Length);

                for (int i = 0; i < threshold.Length; i++)
                {
                    List<Tuple<object, double>> shortlisted_target = new List<Tuple<object, double>>();
                    List<Tuple<object, double>> shortlisted_decoy = new List<Tuple<object, double>>();
                    double TO = 0;
                    double DO = 0;
                    double TB = 0;
                    double DB = 0;

                    for (int j = 0; j < (Decoy.Length); j++)
                    {
                        if (Decoy[j].Item2 > threshold[i])
                        {
                            shortlisted_decoy.Add(Decoy[j]);
                        }
                    }
                    for (int k = 0; k < (Target.Length); k++)
                    {
                        if (Target[k].Item2 >= threshold[i])
                        {
                            shortlisted_target.Add(Target[k]);
                        }
                    }

                    if (!shortlisted_decoy.Any())
                    {
                        TO = (2 * shortlisted_target.Count) / 2.0;
                        DO = (2 * shortlisted_decoy.Count) / 2.0;
                    }

                    if (!shortlisted_target.Any())
                    {
                        TO = (2 * shortlisted_target.Count) / 2;
                        DO = (2 * shortlisted_decoy.Count) / 2;
                    }

                    if (shortlisted_decoy.Any() && shortlisted_target.Any())
                    {
                        List<object> TO_elements = new List<object>();
                        for (int x = 0; x < shortlisted_target.Count; x++)
                        {
                            bool check = false;
                            for (int y = 0; y < shortlisted_decoy.Count; y++)
                            {
                                if ((shortlisted_target[x].Item1).Equals(shortlisted_decoy[y].Item1))
                                {
                                    check = true;
                                    break;
                                }
                            }
                            if (check == false)
                            {
                                TO_elements.Add(shortlisted_target[x].Item1);
                            }
                        }

                        List<object> DO_elements = new List<object>();
                        for (int x = 0; x < shortlisted_decoy.Count; x++)
                        {
                            bool check = false;
                            for (int y = 0; y < shortlisted_target.Count; y++)
                            {
                                if ((shortlisted_decoy[x].Item1).Equals(shortlisted_target[y].Item1))
                                {
                                    check = true;
                                    break;
                                }
                            }
                            if (check == false)
                            {
                                DO_elements.Add(shortlisted_decoy[x].Item1);
                            }
                        }
                        TO = TO_elements.Count;
                        DO = DO_elements.Count;

                        List<object> Common = new List<object>();
                        for (int x = 0; x < shortlisted_decoy.Count; x++)
                        {
                            for (int y = 0; y < shortlisted_target.Count; y++)
                            {
                                //Console.WriteLine((shortlisted_decoy[x].Item1).Equals(shortlisted_target[y].Item1));
                                if ((shortlisted_decoy[x].Item1).Equals(shortlisted_target[y].Item1))
                                {
                                    Common.Add(shortlisted_decoy[x].Item1);
                                    break;
                                }
                            }
                        }

                        for (int idx = 0; idx < Common.Count; idx++)
                        {
                            double decoy_score = 0;
                            double target_score = 0;

                            for (int j = 0; j < shortlisted_decoy.Count; j++)
                            {
                                if ((shortlisted_decoy[j].Item1).Equals(Common[idx]))
                                {
                                    decoy_score = shortlisted_decoy[j].Item2;
                                    break;
                                }
                            }
                            for (int j = 0; j < shortlisted_target.Count; j++)
                            {
                                if ((shortlisted_target[j].Item1).Equals(Common[idx]))
                                {
                                    target_score = shortlisted_target[j].Item2;
                                    break;
                                }
                            }

                            if (target_score >= decoy_score)
                            {
                                TB = TB + 1;
                            }
                            else
                            {
                                DB = DB + 1;
                            }
                        }
                    }

                    double FDRxx = (2 * DB + DO) / (TO + TB + DB);
                    FDR_estimation.Add(FDRxx);
                    //FDR_estimation.Add(Math.Round(FDRxx,4)); //not sure if i am suppose to round this off
                    if (FDRxx > cutoff)
                    {
                        break;
                    }
                }
                //END OF FDR.M

                //BACK IN COMPUTEFDR.M
                DataTable TargetDataWithFDR = new DataTable();
                foreach (string header in headers_decoy)
                {
                    TargetDataWithFDR.Columns.Add(header.Trim());
                }
                int counter = 1;
                foreach (DataRow dr in sorted_target.Rows)
                {
                    if (counter < FDR_estimation.Count)
                    {
                        TargetDataWithFDR.Rows.Add(dr.ItemArray);
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                }

                TargetDataWithFDR.Columns.Add("FDR", typeof(System.Double));
                int count = 0;
                foreach (DataRow row in TargetDataWithFDR.Rows)
                {
                    row["FDR"] = FDR_estimation[count];
                    count++;
                }

                List<object> protein = new List<object>();
                foreach (DataRow row in TargetDataWithFDR.Rows)
                {
                    protein.Add(row["Protein Header"]);
                }

                List<object> unique_protein = protein.Distinct().ToList();
                int protein_count = unique_protein.Count;

                var complete_targetfile_with_fdr = target_filepath.Replace(".csv", "_FDR.csv");

                StreamWriter sw = new StreamWriter(complete_targetfile_with_fdr, false);
                //CREATING CSV FILE
                try
                {
                    int columnCount = TargetDataWithFDR.Columns.Count;

                    for (int i = 0; i < columnCount; i++)
                    {
                        sw.Write(TargetDataWithFDR.Columns[i]);

                        if (i < columnCount - 1)
                        {
                            sw.Write(",");
                        }
                    }

                    sw.Write(sw.NewLine);

                    foreach (DataRow dr in TargetDataWithFDR.Rows)
                    {
                        for (int i = 0; i < columnCount; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                sw.Write(dr[i].ToString());
                            }

                            if (i < columnCount - 1)
                            {
                                sw.Write(",");
                            }
                        }

                        sw.Write(sw.NewLine);
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }

                var expProteinString = @"Total protein count reported by Experiment: " + Convert.ToString(sorted_target.Rows.Count);
                var evalProteinString = @"Total protein count reported by Experiment with E-value greater than 1E-10: " + Convert.ToString(evalue_count);
                var TotalProteinString = @"Total protein reported with: " + Convert.ToString(cutoff_actual) + @"% FDR: " + Convert.ToString(protein.Count);
                var ProteinString = @"Unique proteins count reported with " + Convert.ToString(cutoff_actual) + @"% FDR: " + Convert.ToString(protein_count);

                sw.WriteLine();
                sw.WriteLine(expProteinString);
                sw.WriteLine(evalProteinString);
                sw.WriteLine(TotalProteinString);
                sw.WriteLine(ProteinString);
                sw.Close();
            }
            catch (Exception e)
            {

            }

        }
    }
}