using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;
using PerceptronGenerateDecoyDatabase.DTO;
using System.Data.SqlClient;

namespace PerceptronGenerateDecoyDatabase.Engine
{
    public class ReadFastaFile
    {
        //string FastaFilePath = @"C:\Users\Administrator\Desktop\";  // Add here fasta file location
        //string FileName = "Human";  //Add here fasta File Name

        public List<FastaProteinInfo> ReadingFastaFile(string FastaFilePath, string InputFileName)
        {
            try
            {
                Stopwatch Time = Stopwatch.StartNew();
                var tempD = new List<FastaProteinDataDto>();

                var ProteinList = new List<FastaProteinInfo>();


                string FastaFullFileName = FastaFilePath + InputFileName + ".fasta";

                //var ExcelFileName = FastaFilePath + FileName + ".xlsx";


                var FastaFile = new StreamReader(FastaFullFileName);
                var ReadPeripheralFastaFile = new StreamReader(FastaFullFileName); // Reading same file but for using ReadLine() method separately...!

                var FastaProteinInfo = new List<FastaProteinDataDto>();
                int FastaFileLineCount = File.ReadLines(FastaFullFileName).Count();  //ReadLines is more computationally efficient than ReadAllLines  &  ReadLines() creates an enumerator on the file, reading it line-by-line (actually using StreamReader. ReadLine() ).

                string NextLine = ReadPeripheralFastaFile.ReadLine();

                string tempHeader;
                string tempSequence;

                int FileReadingIteration = 0;

                /* FROM HERE; START READING FILE  */
                while (FileReadingIteration < FastaFileLineCount)// For Reading Full fasta file till end...
                {
                    tempHeader = "";
                    tempSequence = "";
                    string FastaFileLine;
                    //string NextLine;

                    

                    while (true)
                    {
                        FileReadingIteration += 1;
                        FastaFileLine = FastaFile.ReadLine();

                        switch (FastaFileLine.Contains(">sp|")) //ReadLine for Reading Lines Line By Line
                        {
                            case true:
                                tempHeader = FastaFileLine;
                                break;
                                
                            case false:
                                tempSequence = tempSequence + FastaFileLine;//FastaFullFileName
                                break;
                        }
                        try
                        {
                            NextLine = ReadPeripheralFastaFile.ReadLine();
                            if (NextLine.Contains(">sp|")) {

                                /*Three Copies Required for Each Protein Therefore, below Code Used*/
                                var tempProtein1 = new FastaProteinInfo(tempHeader, tempSequence);
                                var tempProtein2 = new FastaProteinInfo(tempHeader, tempSequence);
                                var tempProtein3 = new FastaProteinInfo(tempHeader, tempSequence);
                                ProteinList.Add(tempProtein1);
                                ProteinList.Add(tempProtein2);
                                ProteinList.Add(tempProtein3);
                                break;
                            }
                        }
                        catch (Exception) //Last Line will be empty. So, NextLine will be null & Exception will break the loop
                        {

                            /*Three Copies Required for Each Protein Therefore, below Code Used*/
                            //For Adding Last Protein
                            var tempProtein1 = new FastaProteinInfo(tempHeader, tempSequence);
                            var tempProtein2 = new FastaProteinInfo(tempHeader, tempSequence);
                            var tempProtein3 = new FastaProteinInfo(tempHeader, tempSequence);
                            ProteinList.Add(tempProtein1);
                            ProteinList.Add(tempProtein2);
                            ProteinList.Add(tempProtein3);

                            break;
                        }
                    }
                    //GetSequenceInfoData(tempHeader, Path, FileName, tempSequence, FastaProteinInfo);

                }

                FastaFile.Close();

                Time.Stop();

                return ProteinList;
            }
            catch (Exception e)
            {
                throw;
            }

        }
    }
}
