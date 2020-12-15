using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronGenerateDecoyDatabase.DTO;

namespace PerceptronGenerateDecoyDatabase.Utility
{
    public class WriteFastaFile
    {
        public void WritingFastaFile(string Path, string OutputFileName, List<FastaProteinInfo> ProteinDecoyList)
        {
            int a = 1;

            string FileWithPath = Path + OutputFileName + ".fasta";
            if (File.Exists(FileWithPath))
                File.Delete(FileWithPath); //Deleted Pre-existing file

            var fout = new FileStream(FileWithPath, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fout);

            for (int iter = 0; iter < ProteinDecoyList.Count; iter++)
            {
                sw.WriteLine(ProteinDecoyList[iter].Header);
                sw.WriteLine(ProteinDecoyList[iter].Sequence);
            }

            sw.Close();

        }
    }
}
