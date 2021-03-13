using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Engine
{
    public class FastaReader
    {
        public static List<List<ProteinDto>> FetchingSqlDatabaseProteins()
        {
            var AllDatabasesOfProteins = new List<List<ProteinDto>>();

            string CurrentDirectory = Directory.GetCurrentDirectory();
            string DatabasesFolder = CurrentDirectory.Replace("\\bin\\Debug", "\\ProteinDatabasesInFastaFormat\\");
            List<string> DatabaseFileNames = new List<string>() { "Human.fasta", "HumanDecoyDB.fasta", "EColi.fasta", "EColiDecoyDB.fasta" };
            
            for (int index = 0; index < DatabaseFileNames.Count; index++)
            {
                string FastaFullFileName = DatabasesFolder + DatabaseFileNames[index];
                AllDatabasesOfProteins.Add(EachFileCandidateProteinsFetching(FastaFullFileName));
            }
            return AllDatabasesOfProteins;
        }

        public static List<ProteinDto> EachFileCandidateProteinsFetching(string FastaFullFileName)
        {
            try
            {
                //Stopwatch Time = Stopwatch.StartNew();
                //Time.Start();
                

                //var tempD = new List<FastaProteinDataDto>();

                var FastaFile = new StreamReader(FastaFullFileName);
                var ReadPeripheralFastaFile = new StreamReader(FastaFullFileName); // Reading same file but for using ReadLine() method separately...!

                var FastaProteinInfo = new List<ProteinDto>();
                int FastaFileLineCount = File.ReadLines(FastaFullFileName).Count();  //ReadLines is more computationally efficient than ReadAllLines  &  ReadLines() creates an enumerator on the file, reading it line-by-line (actually using StreamReader. ReadLine() ).

                string NextLine = ReadPeripheralFastaFile.ReadLine();

                string tempHeader;
                string tempFastaHeader;
                string tempSequence;

                int FileReadingIteration = 0;

                /* FROM HERE; START READING FILE  */
                while (FileReadingIteration < FastaFileLineCount)// For Reading Full fasta file till end...
                {
                    tempHeader = "";
                    tempFastaHeader = "";
                    tempSequence = "";
                    string FastaFileLine;
                    //string NextLine;

                    while (true)
                    {
                        FileReadingIteration += 1;
                        FastaFileLine = FastaFile.ReadLine();

                        switch (FastaFileLine.Contains(">")) //Updated 20201215    --- Replaced this ">sp|" to ">"   ---- //ReadLine for Reading Lines Line By Line
                        {
                            case true:

                                /*Uniprot Accession Number have 6 to 10 alphanumrical characters...*/
                                /* https://www.uniprot.org/help/accession_numbers  */
                                //FastaFileLine = ">sp|ABCDEFGHIJKL123|NUD|||||4B"; //I am Just for testing

                                int FirstVerticalBar = FastaFileLine.IndexOf("|");              //Updated 20201215 
                                int SecondVerticalBar = FastaFileLine.IndexOf("|", FirstVerticalBar + 1);              //Updated 20201215 

                                tempHeader = FastaFileLine.Substring(FirstVerticalBar + 1, SecondVerticalBar - FirstVerticalBar - 1); //4: is starting Position(BUT NOT INCLUDED) & 6 is number of characters should be extracted
                                tempFastaHeader = FastaFileLine.Replace("'", " ");
                                tempFastaHeader = tempFastaHeader.Replace(",", " ");
                                ////if (FastaFileLine[10] != '|') //If Accession Number Length is >6    // ITS HEALTHY...
                                ////{
                                ////    int LengthofAccessionNumber = FastaFileLine.IndexOf('|', 9) - 4; //4 is due to {>sp|}
                                ////    tempHeader = FastaFileLine.Substring(4, LengthofAccessionNumber);
                                ////}
                                break;

                            case false:
                                tempSequence = tempSequence + FastaFileLine;//FastaFullFileName
                                break;
                        }
                        try
                        {
                            NextLine = ReadPeripheralFastaFile.ReadLine();
                            if (NextLine.Contains(">"))  //Updated 20201215    --- Replaced this ">sp|" to ">"   ----
                            { break; }
                        }
                        catch (Exception) //Last Line will be empty. So, NextLine will be null & Exception will break the loop
                        {
                            break;
                        }
                    }
                    GetSequenceInfoData(tempHeader, tempFastaHeader, tempSequence, FastaProteinInfo);

                }
                //FastaProteinInfo = FastaProteinInfo.OrderByDescending(n => n.MolecularWeight).ToList();  //Sort By Descending Order         //ITS HEALTHY

                //GetConnectionString(FastaProteinInfo, DatabaseToBeUpdated);   //ITS HEALTHY

                FastaFile.Close();
                //Time.Stop();
                int delme = 1;

                return FastaProteinInfo;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //private static void GetConnectionString(List<FastaProteinDataDto> FastaProteinInfo, string DatabaseToBeUpdated)
        //{

        //    try
        //    {
        //        string ConnetionString = "Data Source=****-II;Initial Catalog=HumanDecoy;Integrated Security=True";

        //        //ConfigurationManager.ConnectionStrings["EcoliConnectionStringName"].ConnectionString;
        //        SqlConnection Connection = new SqlConnection(ConnetionString);
        //        Connection.Open();

        //        var QueryInfo = "";

        //        for (int index = 0; index < FastaProteinInfo.Count; index++)
        //        {

        //            QueryInfo = "Insert INTO " + DatabaseToBeUpdated + ".dbo.ProteinInfoes (ID,MW, Seq, Insilico, InsilicoR, FastaHeader) Values ('"
        //             + FastaProteinInfo[index].ID + "'," + FastaProteinInfo[index].MolecularWeight + ",'" + FastaProteinInfo[index].Sequence + "','" + FastaProteinInfo[index].InsilicoLeft + "','" + FastaProteinInfo[index].InsilicoRight + "','" + FastaProteinInfo[index].FastaHeader + "')";

        //            var Command = new SqlCommand(QueryInfo, Connection);
        //            Command.ExecuteNonQuery();
        //        }
        //        //db.SaveChanges();

        //        Connection.Close();
        //    }

        //    catch (Exception e)
        //    {
        //        throw;
        //    }
        //}

        public static void GetSequenceInfoData(string Header, string FastaHeader,  string Sequence, List<ProteinDto> FastaProteinInfo)       //   string Path, string FileName,
        {//This method will calculate Insilico Left & Right Ion Fragments
            

            int SequenceLength = Sequence.Length;
            double LeftIon;
            double LeftIonMass = 0;
            var LeftIonArray = new List<double>(SequenceLength);
            string LeftIonString;

            double RightIon;
            double RightIonMass = 0;
            var RightIonArray = new List<double>(SequenceLength);
            string RightIonString;
            double mass;

            for (int index = 0; index < SequenceLength; index++)
            {

                LeftIon = AminoAcidMasses.TryGetValue(Sequence[index], out mass) ? mass : mass;   //LeftIon Starts from starting of the sequence 
                LeftIonMass = LeftIonMass + LeftIon;
                LeftIonArray.Add(LeftIonMass);        // LeftIonArray[index] = LeftIonMass;


                RightIon = AminoAcidMasses.TryGetValue(Sequence[SequenceLength - 1 - index], out mass) ? mass : mass; // RightIon starting from the End of the Sequence
                RightIonMass = RightIonMass + RightIon;
                RightIonArray.Add(RightIonMass);   // RightIonArray[index] = RightIonMass;
            }

            //LeftIonString = String.Join(",", LeftIonArray);
            //RightIonString = String.Join(",", RightIonArray);
            double WholeProteinMass = RightIonMass + 2 * (1.0078250321) + 15.9949146221; // RightIonMass + H2O

            var temp = new ProteinDto(Header, WholeProteinMass, Sequence, LeftIonArray, RightIonArray);
            FastaProteinInfo.Add(temp);

        }

        public static readonly Dictionary<char, double> AminoAcidMasses = new Dictionary<char, double>()
        {
        // GIVEN BELOW AMINO ACID LIST IS MODIFIED ACCORDING TO SPECTRUM>ENGINE>AA_MW_ARRAY.M 
        {'A', 71.03711},
        {'B', 114.5349}, //B = avg(D,N)
        {'C', 103.00919},
        {'D', 115.02694},
        {'E', 129.04259},
        {'F', 147.06841},
        {'G', 57.02146},
        {'H', 137.05891},
        {'I', 113.08406},
        {'J', 0},
        {'K', 128.09496},
        {'L', 113.08406},
        {'M', 131.04049},
        {'N', 114.04293},
        {'O', 255.158295},
        {'P', 97.05276},
        {'Q', 128.05858},
        {'R', 156.10111},
        {'S', 87.03203},
        {'T', 101.04768},
        {'U', 168.964203},
        {'V', 99.06841},
        {'W', 186.07931},
        {'X', 110},
        {'Y', 163.06333},
        {'Z', 128.5506}  //Z = avg(E,Q)
        };

    }
}

