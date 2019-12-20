using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Engine;

namespace PerceptronLocalService.Testing
{

    public static class PstScoringBenchmarking
    {
        private const string DirectoryPath = @"D:\Benchmarking\TopPIC data\ETD";

        public static void Benchnmark()
        {
            string[] fileEntries = Directory.GetFiles(DirectoryPath);
            var numberOfPeaklistFiles = fileEntries.Length;
            var executionTimesGpu = new List<string>();
            var executionTimesCpu = new List<string>();
            var pstCount = new List<int>();               
            var proteinCount = new List<int>();

            var parameters = new SearchParametersDto()
            {
                HopThreshhold = 0.1,
                PeakListFileName = fileEntries,
                FileType = fileEntries.Select(x => Path.GetExtension(DirectoryPath + "//" + x)).ToArray(),
                MinimumPstLength = 1,
                MaximumPstLength = 10,
                FilterDb = 0,
                ProtDb = "Ecoli"

            };

            for (var fileNumber = 200; fileNumber < numberOfPeaklistFiles; fileNumber+=400)
            {

                var massSpectrometryData = PeakListFileReaderModuleModule(parameters, fileNumber);
                
                var timer = new Stopwatch();
                var pstTags = ExecuteDenovoModuleCpu(parameters, massSpectrometryData);
               

                var proteins = GetCandidateProtein(parameters, massSpectrometryData);

                for (var pro = 100; pro < proteins.Count; pro += 50)
                {
                    pstCount.Add(pstTags.Count);
                    proteinCount.Add(pro);

                    var filteredproteins = proteins.Take(pro).ToList();
                    timer.Start();
                    var pstFilterCpu = new PstFilterCpu();
                    pstFilterCpu.ScoreProteinsByPst(pstTags, filteredproteins);
                    timer.Stop();
                    executionTimesCpu.Add(timer.Elapsed.TotalMilliseconds.ToString());

                    filteredproteins = proteins.Take(pro).ToList();
                    timer.Restart();
                    timer.Start();
                    var pstFilterGpu = new PstFilterGpu();
                    pstFilterGpu.ScoreProteinsByPst(pstTags, filteredproteins);
                    timer.Stop();
                    executionTimesGpu.Add(timer.Elapsed.TotalMilliseconds.ToString());

                }

               

               
            }

            DumpResults(executionTimesGpu, executionTimesCpu, pstCount, proteinCount);

        }

        private static List<PstTagsDto> ExecuteDenovoModuleCpu(SearchParametersDto parameters, MsPeaksDto massSpectrometryData)
        {
            var pstGenerator = new PstGeneratorCpu();
            var pstTags = pstGenerator.GeneratePeptideSequenceTags(parameters, massSpectrometryData);
            return pstTags;

        }
        private static int ExecuteDenovoModuleGpu(SearchParametersDto parameters, MsPeaksDto massSpectrometryData)
        {
            var pstGenerator = new PstGeneratorGpu();
            var pstTags = pstGenerator.GeneratePeptideSequenceTags(parameters, massSpectrometryData);
            return pstTags.Count;
        }
        private static List<ProteinDto> GetCandidateProtein(SearchParametersDto parameters, MsPeaksDto peakData)
        {
            Stopwatch moduleTimer = Stopwatch.StartNew();
            var proteinRepository = new ProteinRepositorySql();
            var listOfProteins = proteinRepository.ExtractProteins(peakData.WholeProteinMolecularWeight, parameters);

            moduleTimer.Stop();
            return listOfProteins;
        }

        private static MsPeaksDto PeakListFileReaderModuleModule(SearchParametersDto parameters, int fileNumber)
        {
            var peakListFileReader = new PeakListFileReader();
            var peakData = peakListFileReader.PeakListReader(parameters, fileNumber);
            return peakData;
        }

        public static void DumpResults( List<string> gpuTimes, List<string> cpuTimes, List<int> pstCount, List<int> proteinCount)
        {
            var dir = @"D:\Benchmarking\PstGeneration\CID Results";

            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = dir + "\\PstScoringTime.xls";
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

                oSheet.Cells[1, 1] = "Cpu Time";
                oSheet.Cells[1, 2] = "Gpu Time";
                oSheet.Cells[1, 3] = "Pst Count";
                oSheet.Cells[1, 4] = "Protein Count";
                

                for (int i = 0; i < gpuTimes.Count; i++)
                {                    
                    oSheet.Cells[i + 2, 1] = cpuTimes[i];
                    oSheet.Cells[i + 2, 2] = gpuTimes[i];
                    oSheet.Cells[i + 2, 3] = pstCount[i];
                    oSheet.Cells[i + 2, 4] = proteinCount[i];
                   
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
