using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PerceptronAPI.Models;
using PerceptronAPI.Repository;
using PerceptronAPI.Utility;

namespace PerceptronAPI.Engine
{
    public class FastaReader
    {
        SqlDatabase _dataLayer = new SqlDatabase();

        AminoAcids _AminoAcids = new AminoAcids();

        public string MainFastaReader(string DatabaseName, string FastaFullFileName)
        {

            try
            {
                Stopwatch Time = Stopwatch.StartNew();
                var tempD = new List<FastaReaderProteinDataDto>();
                //FastaFilePath = @"C:\Users\Administrator\Desktop\";  // Add here fasta file location
                //DatabaseName = "Human";  //Add here fasta File Name
                //string FastaFullFileName = FastaFilePath + DatabaseName + ".fasta";
                //var ExcelFileName = FastaFilePath + DatabaseName + ".xlsx";


                var FastaFile = new StreamReader(FastaFullFileName);
                var ReadPeripheralFastaFile = new StreamReader(FastaFullFileName); // Reading same file but for using ReadLine() method separately...!

                var FastaProteinInfo = new List<FastaReaderProteinDataDto>();
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
                    string ProteinDescription = "";
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
                                tempFastaHeader = FastaFileLine.Replace("'", " ");   //Removing Single Quote sign to avoid issues while implementing SQL Query 
                                tempFastaHeader = tempFastaHeader.Replace(",", " ");   //Removing Single Quote sign to avoid issues while implementing SQL Query 
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
                    GetSequenceInfoData(tempHeader, tempFastaHeader, DatabaseName, tempSequence, FastaProteinInfo);

                }
                FastaProteinInfo = FastaProteinInfo.OrderByDescending(n => n.MolecularWeight).ToList();  //Sort By Descending Order

                string Message = _dataLayer.UpdatingDatabase(DatabaseName, FastaProteinInfo);

                FastaFile.Close();
                Time.Stop();
                return Message;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<FastaReaderProteinDataDto> GetSequenceInfoData(string Header, string ProteinDescription, string FileName, string Sequence, List<FastaReaderProteinDataDto> FastaProteinInfo)
        {//This method will calculate Insilico Left & Right Ion Fragments

            int SequenceLength = Sequence.Length;
            double LeftIon;
            double LeftIonMass = 0;
            double[] LeftIonArray = new double[SequenceLength];
            string LeftIonString;

            double RightIon;
            double RightIonMass = 0;
            double[] RightIonArray = new double[SequenceLength];
            string RightIonString;
            double mass;

            for (int index = 0; index < SequenceLength; index++)
            {

                LeftIon = _AminoAcids.AminoAcidMasses.TryGetValue(Sequence[index], out mass) ? mass : mass;   //LeftIon Starts from starting of the sequence 
                LeftIonMass = LeftIonMass + LeftIon;
                LeftIonArray[index] = LeftIonMass;

                RightIon = _AminoAcids.AminoAcidMasses.TryGetValue(Sequence[SequenceLength - 1 - index], out mass) ? mass : mass; // RightIon starting from the End of the Sequence
                RightIonMass = RightIonMass + RightIon;
                RightIonArray[index] = RightIonMass;
            }

            LeftIonString = String.Join(",", LeftIonArray);
            RightIonString = String.Join(",", RightIonArray);
            double WholeProteinMass = RightIonMass + 2 * (1.0078250321) + 15.9949146221; // RightIonMass + H2O

            var temp = new FastaReaderProteinDataDto(Header, ProteinDescription, WholeProteinMass, Sequence, LeftIonString, RightIonString);
            FastaProteinInfo.Add(temp);


            return FastaProteinInfo;
        }
    }
}