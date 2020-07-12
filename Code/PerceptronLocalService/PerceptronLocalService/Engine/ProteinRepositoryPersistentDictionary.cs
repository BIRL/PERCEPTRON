

/*
 * 
 * WHY I NEED THIS   ""ProteinRepositoryPersistentDictionary.cs"" ????
 *  
 * 
*/



using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    class ProteinRepositoryPersistentDictionary
    {
        private CPersistentDictionary Uniprot = new CPersistentDictionary(@"D:\UploadedData\DIC\DICUniprot.txt");              /*!< Uniprot.txt stores the Uniprot i.e. all annotated proteins along with their fragments and MW. */
        private CPersistentDictionary Ubiquitin = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Ubiquitin.txt");          /*!< Ubiquitin.txt stores the test ubiquitin database. It contains 8 proteins. */
        private CPersistentDictionary Archaea = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Archea.txt");               /*!< Archea.txt stores all the Archea annotated proteins. */
        private CPersistentDictionary Bacteria = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Bacteria.txt");            /*!< Bacteria.txt stores all the Bacteria annotated proteins. */
        private CPersistentDictionary Cellular = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Cellular.txt");            /*!< Cellular.txt stores all the Cellular annotated proteins. */
        private CPersistentDictionary Eukaryota = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Eukaryota.txt");          /*!< Eukaryota.txt stores all the Eukaryota annotated proteins. */
        private CPersistentDictionary Fungi = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Fungi.txt");                  /*!< Fungi.txt stores all the Fungi annotated proteins. */
        private CPersistentDictionary Human = new CPersistentDictionary(@"D:\01_PERCEPTRON\01_PERCEPTRON_FROM_98\DatabasesforDictionary\Human.txt");                  /*!< Human.txt stores all the Human annotated proteins. *///FARHAN's TESTING //--[REPLACED BY]>> D:\UploadedData\DIC\DIC\Human.txt
        private CPersistentDictionary Vertebrates = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Vertebrates.txt");      /*!< Vertebrates.txt stores all the Vertebrates annotated proteins. */
        private CPersistentDictionary Mammals = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Mammals.txt");              /*!< Mammals.txt stores all the Mammals annotated proteins. */
        private CPersistentDictionary Rodents = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Rodents.txt");              /*!< Rodents.txt stores all the Rodents annotated proteins. */
        private CPersistentDictionary Viridiplantae = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Virdiplantae.txt");   /*!< Virdiplantae.txt stores all the Virdiplantae annotated proteins. */
        private CPersistentDictionary Viruses = new CPersistentDictionary(@"D:\UploadedData\DIC\DIC\Viruses.txt");


        //private CPersistentDictionary Uniprot = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Uniprot.txt");              /*!< Uniprot.txt stores the Uniprot i.e. all annotated proteins along with their fragments and MW. */
        //private CPersistentDictionary Ubiquitin = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Ubiquitin.txt");          /*!< Ubiquitin.txt stores the test ubiquitin database. It contains 8 proteins. */
        //private CPersistentDictionary Archaea = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Archea.txt");               /*!< Archea.txt stores all the Archea annotated proteins. */
        //private CPersistentDictionary Bacteria = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Bacteria.txt");            /*!< Bacteria.txt stores all the Bacteria annotated proteins. */
        //private CPersistentDictionary Cellular = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Cellular.txt");            /*!< Cellular.txt stores all the Cellular annotated proteins. */
        //private CPersistentDictionary Eukaryota = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Eukaryota.txt");          /*!< Eukaryota.txt stores all the Eukaryota annotated proteins. */
        //private CPersistentDictionary Fungi = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Fungi.txt");                  /*!< Fungi.txt stores all the Fungi annotated proteins. */
        //private CPersistentDictionary Human = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Human.txt");                  /*!< Human.txt stores all the Human annotated proteins. */
        //private CPersistentDictionary Vertebrates = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Vertebrates.txt");      /*!< Vertebrates.txt stores all the Vertebrates annotated proteins. */
        //private CPersistentDictionary Mammals = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Mammals.txt");              /*!< Mammals.txt stores all the Mammals annotated proteins. */
        //private CPersistentDictionary Rodents = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Rodents.txt");              /*!< Rodents.txt stores all the Rodents annotated proteins. */
        //private CPersistentDictionary Viridiplantae = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Virdiplantae.txt");   /*!< Virdiplantae.txt stores all the Virdiplantae annotated proteins. */
        //private CPersistentDictionary Viruses = new CPersistentDictionary(@"C:\inetpub\wwwroot\DIC\DIC\Viruses.txt");              /*!< Viruses.txt stores all the Viruses annotated proteins. */

        private int CondVar = -1;     /*!< Condition Variable */

        private List<int> UniprotList = new List<int>();         /*!< UniprotList contains the integer masses of all protein in Uniprot. */
        private List<int> UbiquitinList = new List<int>();       /*!< UbiquitinList contains the integer masses of all protein in Ubiquitin.  */
        private List<int> ArchaeaList = new List<int>();         /*!< ArchaeaList contains the integer masses of all protein in Archea. */
        private List<int> BacteriaList = new List<int>();        /*!< BacteriaList contains the integer masses of all protein in Bacteria */
        private List<int> CellularList = new List<int>();        /*!< CellularList contains the integer masses of all protein in Cellular taxonomy. */
        private List<int> EukaryotaList = new List<int>();       /*!< EukaryotaList contains the integer masses of all protein in Eukoryotes. */
        private List<int> FungiList = new List<int>();           /*!< FungiList contains the integer masses of all protein in Fungi. */
        private List<int> HumanList = new List<int>();           /*!< HumanList contains the integer masses of all protein in Human. */
        private List<int> VertebratesList = new List<int>();     /*!< VertebratesList contains the integer masses of all protein in vertebrates.  */
        private List<int> MammalsList = new List<int>();         /*!< MammalsList contains the integer masses of all protein in Mammals. */
        private List<int> RodentsList = new List<int>();         /*!< RodentsList contains the integer masses of all protein in rodents. */
        private List<int> ViridiplantaeList = new List<int>();   /*!< ViridiplantaeList contains the integer masses of all protein in Virdiplantae. */
        private List<int> VirusesList = new List<int>();         /*!< VirusesList contains the integer masses of all protein in Viruses. */

        private static double GetProteinMw(string tempS)
        {
            const double h = 1.0078250321;
            const double o = 15.9949146221;
            const double h2O = h + h + o;
            var tempMw = tempS.Aggregate<char, double>(0, (current, t) => current + AminoAcids.GetAminoAcidMw(t));
            tempMw = tempMw + h2O;
            return tempMw;
        }

        //! Sqlited-Sqlited12 are static member function with no arguments and void return type
        /*!
         *These functions Load the Databse into the dic along with the computation of the MW and the Insilico fragments
         */
        private void Sqlited()
        {
            var counter = 0;
            var tempD = new List<SerializedProteinDataDto>();    /*!<  */
            string line;                /*!<  */
            var tempHeader = "";    /*!<  */
            var tempSequence = "";  /*!<  */
            var path = @"C:\inetpub\wwwroot\Databases\uniprot-all.fasta";   /*!<  */
            var file = new StreamReader(path);     /*!<  */

            while ((line = file.ReadLine()) != null)        /*!<  */
            {
                if (line.Contains('>') && counter > 0)      /*!<  */
                {
                    var tempMw = AminoAcids.GetAminoAcidMw(tempSequence[0]);
                    tempMw = tempSequence.Aggregate(tempMw, (current, t) => current + AminoAcids.GetAminoAcidMw(t));
                    var k = tempHeader.Substring(4, 6);
                    var data1 = new SerializedProteinDataDto(k, tempSequence, tempMw, "", "");
                    tempD.Add(data1);
                    tempHeader = "";
                    tempSequence = "";
                }

                if (line.Contains(" "))
                {
                    tempHeader = tempHeader + line;
                }
                else
                    tempSequence = tempSequence + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();


            while (tempD.Count > 0)
            {
                var left = new List<double>();
                var right = new List<double>();
                var strprotein = tempD[0].Seq;
                var prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (var ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                var insilicoLeft = string.Join(",", left.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
                var insilicoRight = string.Join(",", right.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());

                var res = tempD[0].ID + "&" + tempD[0].MW + "&" + tempD[0].Seq + "&" + insilicoLeft + "&" + insilicoRight + "\n";
                var index = Convert.ToInt32(tempD[0].MW).ToString();
                var tempStr = Uniprot[index].ToString();
                if (tempStr == "")
                {
                    UniprotList.Add(Convert.ToInt32(index));
                    Uniprot.Add(index, "-" + res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + tempStr + "\n";
                    Uniprot.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited1()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\ubiquitin_db.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }



                    string k;
                    if (temp_h.Contains('|'))
                    {
                        k = temp_h.Substring(4, 6);
                    }
                    else
                    {
                        k = temp_h.Substring(1);
                    }

                    //                    string k = temp_h.Substring(4, 6);

                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR;
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Ubiquitin[index].ToString();
                if (xyx == "")
                {
                    UbiquitinList.Add(Convert.ToInt32(index));
                    Ubiquitin.Add(index, "-" + res);
                }
                else
                {

                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString();
                    Ubiquitin.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited2()
        {
            SerializedProteinDataDto data1;
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_archaea.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Archaea[index].ToString();
                if (xyx == "")
                {
                    ArchaeaList.Add(Convert.ToInt32(index));
                    Archaea.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Archaea.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited3()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_bacteria.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Bacteria[index].ToString();
                if (xyx == "")
                {
                    BacteriaList.Add(Convert.ToInt32(index));
                    Bacteria.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Bacteria.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited4()
        {
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Cellular.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    var data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Cellular[index].ToString();
                if (xyx == "")
                {
                    CellularList.Add(Convert.ToInt32(index));
                    Cellular.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Cellular.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited5()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Eukaryota.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Eukaryota[index].ToString();
                if (xyx == "")
                {
                    EukaryotaList.Add(Convert.ToInt32(index));
                    Eukaryota.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Eukaryota.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited6()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Fungi.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Fungi[index].ToString();
                if (xyx == "")
                {
                    FungiList.Add(Convert.ToInt32(index));
                    Fungi.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Fungi.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited7()
        {
            var counter = 0;
            int counter1 = 0;
            var tempD = new List<SerializedProteinDataDto>();
            string line;
            var tempH = "";
            var tempS = "";
            const string path = @"D:\Spectrum\ToolBox\Databases\uniprot_human.fasta";
            var file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {
                    //var tempMw = GetProteinMw(tempS);
                    //var k = tempH.Substring(4, 6);
                    //var data1 = new Data(k, tempS, tempMw, "", "");
                    //tempD.Add(data1);
                    //tempH = "";
                    //tempS = "";


                    /////////////////NME ACETYLATION
                    double tempMw = AminoAcids.GetAminoAcidMw(tempS[1]);

                    for (int i = 2; i < tempS.Length; i++)
                    {
                        tempMw = tempMw + AminoAcids.GetAminoAcidMw(tempS[i]);
                    }
                    tempMw = tempMw + 1.0078250321 + 1.0078250321 + 15.9949146221 + 42.0106;
                    string k = tempH.Substring(4, 6);
                    var data1 = new SerializedProteinDataDto(k, tempS, tempMw, "", "");
                    tempD.Add(data1);

                    tempH = "";
                    tempS = "";
                }

                if (line.Contains(" "))
                    tempH = tempH + line;
                else
                    tempS = tempS + line;
                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            while (tempD.Count > 0)
            {
                counter1++;
                var left = new List<double>();
                var right = new List<double>();
                var strprotein = tempD[0].Seq;
                var prtlength = strprotein.Length;
                //left.Add(AminoAcids.GetMw(strprotein[0]));
                //const double h2O = 1.0078250321 + 1.0078250321 + 15.9949146221;
                //for (var ifragmentationposition = 1; ifragmentationposition < prtlength - 1; ifragmentationposition++)
                //{
                //    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetMw(strprotein[ifragmentationposition]));
                //    right.Add(tempD[0].MW - h2O - left[ifragmentationposition - 1]);
                //}
                //right.Add(tempD[0].MW - h2O - left[left.Count - 1]);

                ////////////////NME Acetylation
                left.Add(AminoAcids.GetAminoAcidMw(strprotein[1]) + 42.0106);
                right.Add(AminoAcids.GetAminoAcidMw(strprotein[prtlength - 1]));
                try
                {
                    for (int ifragmentationposition = 2; ifragmentationposition < prtlength; ifragmentationposition++)
                    {
                        left.Add(left[ifragmentationposition - 2] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                        right.Add(right[ifragmentationposition - 2] + AminoAcids.GetAminoAcidMw(strprotein[prtlength - ifragmentationposition]));
                    }

                }
                catch (Exception)
                {

                    throw;
                }


                var ins = string.Join(",", left.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
                var insR = string.Join(",", right.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray());
                var res = tempD[0].ID + "&" + tempD[0].MW.ToString(CultureInfo.InvariantCulture) + "&" + tempD[0].Seq +
                          "&" + ins + "&" + insR;
                var index = Convert.ToInt32(tempD[0].MW).ToString();
                var xyx = Human[index].ToString();
                if (xyx == "")
                {
                    HumanList.Add(Convert.ToInt32(index));
                    Human.Add(index, "-" + res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx;
                    Human.Add(index, res);
                }
                tempD.RemoveAt(0);
            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited8()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Vertebrates.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Vertebrates[index].ToString();
                if (xyx == "")
                {
                    VertebratesList.Add(Convert.ToInt32(index));
                    Vertebrates.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Vertebrates.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited9()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Mammals.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Mammals[index].ToString();
                if (xyx == "")
                {
                    MammalsList.Add(Convert.ToInt32(index));
                    Mammals.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Mammals.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited10()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Rodents.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Rodents[index].ToString();
                if (xyx == "")
                {
                    RodentsList.Add(Convert.ToInt32(index));
                    Rodents.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Rodents.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited11()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Viridiplantae.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Viridiplantae[index].ToString();
                if (xyx == "")
                {
                    ViridiplantaeList.Add(Convert.ToInt32(index));
                    Viridiplantae.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Viridiplantae.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        private void Sqlited12()
        {
            SerializedProteinDataDto data1 = new SerializedProteinDataDto();
            int counter = 0; int counter1 = 0;
            List<SerializedProteinDataDto> tempD = new List<SerializedProteinDataDto>();
            string line;
            string temp_h = "";
            string temp_s = "";
            string path = @"C:\inetpub\wwwroot\Databases\uniprot_sprot_Viruses.fasta";
            StreamReader file = new StreamReader(path);

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('>') && counter > 0)
                {

                    double temp_MW = AminoAcids.GetAminoAcidMw(temp_s[0]);


                    for (int i = 0; i < temp_s.Length; i++)
                    {
                        temp_MW = temp_MW + AminoAcids.GetAminoAcidMw(temp_s[i]);
                    }

                    string k = temp_h.Substring(4, 6);
                    data1 = new SerializedProteinDataDto(k, temp_s, temp_MW, "", "");
                    tempD.Add(data1);

                    temp_h = "";
                    temp_s = "";


                }

                if (line.Contains(" "))
                {
                    temp_h = temp_h + line;
                }
                else
                    temp_s = temp_s + line;

                counter++;
            }
            tempD = tempD.OrderBy(n => n.MW).ToList();
            List<double> left = new List<double>();
            List<double> right = new List<double>();
            string Ins;
            string InsR;


            while (tempD.Count > 0)
            {
                counter1++;
                Ins = "";
                InsR = "";
                left = new List<double>();
                right = new List<double>();

                var duplicateKeys = tempD.GroupBy(x => x.MW).Where(group => group.Count() > 1).Select(group => group.Key);
                string strprotein = tempD[0].Seq;
                int prtlength = strprotein.Length;

                left.Add(AminoAcids.GetAminoAcidMw(strprotein[0]));
                right.Add(tempD[0].MW);

                for (int ifragmentationposition = 1; ifragmentationposition < prtlength; ifragmentationposition++)
                {
                    left.Add(left[ifragmentationposition - 1] + AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                    right.Add(right[ifragmentationposition - 1] - AminoAcids.GetAminoAcidMw(strprotein[ifragmentationposition]));
                }

                Ins = String.Join(",", left.Select(x => x.ToString()).ToArray());
                InsR = String.Join(",", right.Select(x => x.ToString()).ToArray());

                string res = "";
                res = tempD[0].ID + "&" + tempD[0].MW.ToString() + "&" + tempD[0].Seq + "&" + Ins + "&" + InsR + "\n";
                string index = Convert.ToInt32(tempD[0].MW).ToString();
                string xyx = "";
                xyx = Viruses[index].ToString();
                if (xyx == "")
                {
                    VirusesList.Add(Convert.ToInt32(index));
                    Viruses.Add(index, res);
                }
                else
                {
                    res = res.Substring(0, res.Length - 2) + "|" + xyx.ToString() + "\n";
                    Viruses.Add(index, res);
                }
                tempD.RemoveAt(0);

            }
            file.Close();
            CondVar = 1;
        }
        //! Fasta_Reader is static member function taking 4 arguments and returning a List of proteins
        /*!
         * \param MW a double argument which contains protein MW
         * \param tol a double argument which conatins MW tol
         * \param database a string argument which contains Protein databse name
         * \param FilterDb an integer variable which tells us about user decision regarding filteration of DB on protein MW
         * \return the List of shortlisted proteins
         */
        public List<ProteinDto> ExtractProteinsOLD(double mw, SearchParametersDto parameters, List<PstTagList> PstTags, int CandidateProteinList) // Added "int CandidateList". 20200112
        {
            var tol = parameters.MwTolerance;
            var database = parameters.ProtDb;
            var filterDb = parameters.FilterDb;

            var tempDic = Uniprot;      /*!< Temporary dictionary: It loads the user seleted database in it. */
            var tempList = new List<int>();         /*!< Temporary list: It loads the user selected database's protein falling within user defined mass window. */
            if (filterDb == 0)      /*!< It checks user has asked us to filter the databse using mw window */
                tol = 1000;         /*!< If user hasn't asked to filter the database on mw then we will filter the database on a bigger window of 1 KDa. */

            if (CondVar != 1)      /*!< Check the conditional variable to see if the program is running for first time. if condition true load the databases into dic. */
            {
                //Sqlited();       /*!< Load the DIC. */
                //Sqlited1();      /*!< Load the DIC. */
                //Sqlited2();      /*!< Load the DIC. */
                //Sqlited3();      /*!< Load the DIC. */
                //Sqlited4();      /*!< Load the DIC. */
                //Sqlited5();      /*!< Load the DIC. */
                //Sqlited6();      /*!< Load the DIC. */
                Sqlited7();        /*!< Load the DIC. */
                //Sqlited8();      /*!< Load the DIC. */
                //Sqlited9();      /*!< Load the DIC. */
                //Sqlited10();     /*!< Load the DIC. */
                //Sqlited11();     /*!< Load the DIC. */
                //Sqlited12();     /*!< Load the DIC. */
            }

            /*!<  */
            switch (database)
            {
                case "Uniprot":
                    tempList =
                        UniprotList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    break;
                case "Ubiquitin":
                    tempList =
                        UbiquitinList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Ubiquitin; /*!< Shallow copy of dictionary. */
                    break;
                case "Archaea":
                    tempList =
                        ArchaeaList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Archaea; /*!< Shallow copy of dictionary. */
                    break;
                case "Bacteria":
                    tempList =
                        BacteriaList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Bacteria; /*!< Shallow copy of dictionary. */
                    break;
                case "Cellular":
                    tempList =
                        CellularList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Cellular; /*!< Shallow copy of dictionary. */
                    break;
                case "Eukaryota":
                    tempList =
                        EukaryotaList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Eukaryota; /*!< Shallow copy of dictionary. */
                    break;
                case "Fungi":
                    tempList = FungiList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Fungi; /*!< Shallow copy of dictionary. */
                    break;
                case "Human":
                    tempList = HumanList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Human; /*!< Shallow copy of dictionary. */
                    break;
                case "Vertebrates":
                    tempList =
                        VertebratesList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Vertebrates; /*!< Shallow copy of dictionary. */
                    break;
                case "Mammals":
                    tempList =
                        MammalsList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Mammals; /*!< Shallow copy of dictionary. */
                    break;
                case "Rodents":
                    tempList =
                        RodentsList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Rodents; /*!< Shallow copy of dictionary. */
                    break;
                case "Viridiplantae":
                    tempList =
                        ViridiplantaeList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Viridiplantae; /*!< Shallow copy of dictionary. */
                    break;
                case "Viruses":
                    tempList = VirusesList.FindAll(item => item > Convert.ToInt32(mw - tol) & item < Convert.ToInt32(mw + tol));
                    /*!< Load into temp list. */
                    tempDic = Viruses; /*!< Shallow copy of dictionary. */
                    break;
            }

            /*!
             * A single Protein eg. prot1 string is like this : Header&mw&Seq&C-term(comma-separated)&N-term(comma-separated)
             * Protein at a single index eg p1 are like this: prot1|prot2|prot3
             * Whole protein string: p1-p2-p3
             */

            var proteinList = new List<ProteinDto>();      /*!< Initialize Protein list */
            foreach (var words in tempList.Select(element => tempDic[element.ToString()].ToString()).Where(proteinString => proteinString != "").Select(proteinString => proteinString.Split('|')))
            {
                foreach (var element in words)
                {
                    var first = element.IndexOf("-", StringComparison.Ordinal);       /*!< First index of token  */
                    var last = element.LastIndexOf("-", StringComparison.Ordinal);        /*!< Last index of taken that separates proteins of same mw from the other adjacent Protein */
                    string proteinData;      /*!< Initilaize Protein Data: It will contain the information of a single protein. */
                    if (last > -1)           /*!< If character - exists in the protein string of a single protein */
                    {
                        if (first != last)   /*!< if first and last indices of - are not at the same index. */
                        {
                            proteinData = element.Substring(first, last - 1);   /*!< Get protein Data by omiting the - sign at Start and End */
                        }
                        else                 /*!< otherwise */
                        {
                            proteinData = last == 0 ? element.Substring(1) : element.Substring(0, last - 1);
                        }
                    }
                    else   /*!< otherwise */
                    {
                        proteinData = element.IndexOf("-", StringComparison.Ordinal) > -1 ? element.Substring(first) : element;
                    }
                    var dataPs = proteinData.Split('&');            /*!< split protein Data to get the protein parameters. */

                    if (dataPs.Length != 5) continue;
                    var mWw = Convert.ToDouble(dataPs[1]);    /*!< Experimental protein */
                    var errorScore = MW_filter(mw, tol, mWw, true);                                      /*!< Initiliaze mw Error Score */
                    var tempP = new ProteinDto(dataPs[0], dataPs[2], mWw, errorScore)
                    {
                        InsilicoDetails = new InsilicoObjectDto
                        {
                            InsilicoMassLeft = dataPs[3].Split(',').Select(double.Parse).ToList(),
                            InsilicoMassRight = dataPs[4].Split(',').Select(double.Parse).ToList()
                        }
                    }; /*!< Initialize Protein Object */
                    /*!< Initialize an Insilico object in protein object for storing insilico details */
                    /*!< Parse the C-term ions and store them in protein object. */
                    /*!< Parse the N-term ions and store them in protein object. */
                    proteinList.Add(tempP);    /*!< Add the protein into candidate protein List. */
                }
            }
            return proteinList;                /*!< Return the list of candidate proteins. */
        }

        private double MW_filter(double mw, double tol, double mwExperimental, bool fasta)
        {
            var error = Math.Abs(mwExperimental - mw);      /*!< error calculates the difference b/w theoretical and experimental mw. */
            var errorScore = 1 / Math.Pow(2, error);

            if (fasta)                                  /*!< check condition of fasta variable */
            {
                return errorScore;                             /*!< If MW_filter was called for first time return error score */
            }
            if (error < tol)                                /*!< if error is within user defined tolerance */
                return errorScore;                         /*!< return error score */
            return Constants.OutofBoundModifiedMolecularWeight;                                  /*!< otherwise return -7 to indicate error */
        }

    }

}
