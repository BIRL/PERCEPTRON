using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronGenerateDecoyDatabase.Engine;
using PerceptronGenerateDecoyDatabase.Utility;

namespace PerceptronGenerateDecoyDatabase
{
    public class Program
    {
        static void Main(string[] args)
        {
            string Path = @"P:\02_PERCEPTRON\FARHAN's_DATA_ZDrive\SPECTRUM_20201219\ToolBox\Databases\GenerateDB\"; //Add here the path
            string InputFileName = "EColi5Proteins";
            string OutputFileName = "Decoy_" + InputFileName;
            ReadFastaFile _ReadFastaFile = new ReadFastaFile();
            var ProteinList = _ReadFastaFile.ReadingFastaFile(Path, InputFileName);

            GenerateDecoyDB _GenerateDecoyDB = new GenerateDecoyDB();
            var ProteinDecoyList = _GenerateDecoyDB.GeneratingDecoyDB(ProteinList);

            WriteFastaFile _WriteFastaFile = new WriteFastaFile();
            _WriteFastaFile.WritingFastaFile(Path, OutputFileName, ProteinDecoyList);

        }
    }
}
