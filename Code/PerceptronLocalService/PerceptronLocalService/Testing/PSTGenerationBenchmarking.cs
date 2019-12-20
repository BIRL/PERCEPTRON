using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Engine;

namespace PerceptronLocalService.Testing
{
    public static class PstGenerationBenchmarking
    {
        private const string DirectoryPath = @"D:\Benchmarking\TopPIC data\PeakSize";

        public static void Benchnmark()
        {
            string[] fileEntries = Directory.GetFiles(DirectoryPath);
            var numberOfPeaklistFiles = fileEntries.Length;
            var executionTimesGpu = new List<string>();
            var executionTimesCpu = new List<string>();
            var pstCountCpu = new List<int>();
            var pstCountGpu = new List<int>();
            var peakCount = new List<int>();
            var parameters = new SearchParametersDto()
            {
                HopThreshhold = 0.1,
                PeakListFileName = fileEntries,
                FileType = fileEntries.Select(x => Path.GetExtension(DirectoryPath + "//" + x)).ToArray(),
                MinimumPstLength = 1,
                MaximumPstLength = 10

            };

            for (var fileNumber = 0; fileNumber < numberOfPeaklistFiles; fileNumber++)
            {
                Console.WriteLine("Started processing: "+fileNumber +"  at: "+ DateTime.Now);
                var massSpectrometryData = PeakListFileReaderModuleModule(parameters, fileNumber);
                peakCount.Add(massSpectrometryData.Mass.Count);
                var timer = new Stopwatch();
               
                timer.Start();              
                pstCountCpu.Add(ExecuteDenovoModuleCpu(parameters, massSpectrometryData));
                timer.Stop();
                executionTimesCpu.Add(timer.Elapsed.TotalMilliseconds.ToString());

                timer.Restart();
                pstCountGpu.Add(ExecuteDenovoModuleGpu(parameters, massSpectrometryData));
                timer.Stop();
                executionTimesGpu.Add(timer.Elapsed.TotalMilliseconds.ToString());
            }

            DumpResults(fileEntries, executionTimesGpu, executionTimesCpu, pstCountCpu, pstCountGpu, peakCount);

        }

        private static int ExecuteDenovoModuleCpu(SearchParametersDto parameters, MsPeaksDto massSpectrometryData)
        {
            var pstGenerator = new PstGeneratorCpu();
            var pstTags = pstGenerator.GeneratePeptideSequenceTags(parameters, massSpectrometryData);
            return pstTags.Count;

        }
        private static int ExecuteDenovoModuleGpu(SearchParametersDto parameters, MsPeaksDto massSpectrometryData)
        {
            var pstGenerator = new PstGeneratorGpu();
            var pstTags = pstGenerator.GeneratePeptideSequenceTags(parameters, massSpectrometryData);
            return pstTags.Count;
        }

        private static MsPeaksDto PeakListFileReaderModuleModule(SearchParametersDto parameters, int fileNumber)
        {
            var peakListFileReader = new PeakListFileReader();
            var peakData = peakListFileReader.PeakListReader(parameters, fileNumber);                     
            return peakData;
        }

        public static void DumpResults(string[] files, List<string> gpuTimes, List<string> cpuTimes, List<int> cpuCount, List<int> gpuCount, List<int> peakCount)
        {
            var dir = @"D:\Benchmarking\PstGeneration\PeaklistSize";

            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = dir + "\\PstGenerationTime.xls";
            Application oXL;
            _Workbook oWB;
            _Worksheet oSheet;
            Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            try
            {
                //Start Excel and get Application object.
                oXL = new Microsoft.Office.Interop.Excel.Application();
                //oXL.Visible = true;

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "File";
                oSheet.Cells[1, 2] = "Cpu Time";
                oSheet.Cells[1, 3] = "Gpu Time";
                oSheet.Cells[1, 4] = "Cpu Count";
                oSheet.Cells[1, 5] = "Gpu Count";
                oSheet.Cells[1, 6] = "Peak Count";

                for (int i = 0; i < files.Length; i++)
                {
                    oSheet.Cells[i + 2, 1] = files[i];
                    oSheet.Cells[i + 2, 2] = cpuTimes[i];
                    oSheet.Cells[i + 2, 3] = gpuTimes[i];
                    oSheet.Cells[i + 2, 4] = cpuCount[i];
                    oSheet.Cells[i + 2, 5] = gpuCount[i];
                    oSheet.Cells[i + 2, 6] = peakCount[i];
                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception e)
            {

            }
            //...

        }
    }
}
