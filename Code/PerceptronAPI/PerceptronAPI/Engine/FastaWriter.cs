using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PerceptronAPI.Models;
using PerceptronAPI.Repository;

namespace PerceptronAPI.Engine
{
    public class FastaWriter
    {
        SqlDatabase _dataLayer = new SqlDatabase();

        public void MainFastaWriter(string DatabaseName, string FastaFilePath)
        {
            Stopwatch Time = Stopwatch.StartNew();

            List<FastaWriterProteinDataDto> FastaWriterProteinInfo = _dataLayer.ReadingDataBase(DatabaseName);

            if (FastaWriterProteinInfo.Count > 0)
            {
                if (File.Exists(FastaFilePath))
                    File.Delete(FastaFilePath); //Deleted Pre-existing file

                var fout = new FileStream(FastaFilePath, FileMode.OpenOrCreate);
                var sw = new StreamWriter(fout);

                for (int index = 0; index < FastaWriterProteinInfo.Count; index++)
                {
                    sw.WriteLine(FastaWriterProteinInfo[index].ProteinDescription);
                    sw.WriteLine(FastaWriterProteinInfo[index].Sequence);
                }
                sw.Close();
            }
        }

    }
}