using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PerceptronLocalService.DTO;
using Microsoft.Office.Interop;
using Microsoft.Office.Interop.Excel;

namespace PerceptronLocalService.Testing
{
    public static class Logging
    {
        private static string _directory;
        private static string _rootDirectory;
        public static void CreateDirectory()
        {
            
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            _directory = @"D:\Benchmarking\" + timeStamp;
            _rootDirectory = _directory;

            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public static void CreatePeakFileDirectory(int fileNumber)
        {


            _directory = _directory + "\\file_" + fileNumber;

            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public static void DumpParameters(SearchParametersDto parameters)
        {
            string output = JsonConvert.SerializeObject(parameters);
            using (StreamWriter file =
            new StreamWriter(_directory+"\\Prameters.txt", true))
            {
                file.Write(output);
            }
        }

        public static void DumpMsData(MsPeaksDto msData)
        {
            
            using (StreamWriter file =
            new StreamWriter(_directory + "\\molecular_weight.txt", true))
            {
                file.Write(msData.WholeProteinMolecularWeight);
            }

            Application xlApp = new Application();
            if (xlApp == null) return; 

            string excelFile = _directory + "\\PeakListData.xls";
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
                oWB = (Microsoft.Office.Interop.Excel._Workbook) (oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet) oWB.ActiveSheet;

                //Add table headers going cell by cell.
                oSheet.Cells[1, 1] = "Peak";
                oSheet.Cells[1, 2] = "Intensity";

                for (int i = 0; i < msData.Mass.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = msData.Mass[i];
                    oSheet.Cells[i + 2, 2] = msData.Intensity[i];

                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {
                
            }
            //...

        }

        public static void DumpMwTunerResult(MsPeaksDto msData)
        {

            using (StreamWriter file =
                new StreamWriter(_directory + "\\tuned_molecular_weight.txt", true))
            {
                file.Write(msData.WholeProteinMolecularWeight);
            }
        }

        public static void DumpCandidateProteins(List<ProteinDto> proteinsList)
        {

           

            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\CandidateProteins.xls";
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
                oSheet.Cells[1, 1] = "Header";
                oSheet.Cells[1, 2] = "MW";

                for (int i = 0; i < proteinsList.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = proteinsList[i].Header;
                    oSheet.Cells[i + 2, 2] = proteinsList[i].Mw;

                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {

            }
            //...

        }

        public static void DumpPstTags(List<PstTagsDto> tags)
        {



            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\PstTags.xls";
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
                oSheet.Cells[1, 1] = "Tag";
                oSheet.Cells[1, 2] = "Start";
                oSheet.Cells[1, 3] = "End";
                oSheet.Cells[1, 4] = "Error Score";
                oSheet.Cells[1, 5] = "Frequency Score";

                for (int i = 0; i < tags.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = tags[i].AminoAcidTag;
                    oSheet.Cells[i + 2, 2] = tags[i].StartPosition;
                    oSheet.Cells[i + 2, 3] = tags[i].EndPosition;
                    oSheet.Cells[i + 2, 4] = tags[i].ErrorScore;
                    oSheet.Cells[i + 2, 5] = tags[i].FrequencyScore;

                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {

            }
            //...

        }

        public static void DumpPstScores(List<ProteinDto> proteinsList)
        {



            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\PstScores.xls";
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
                oSheet.Cells[1, 1] = "Header";
                oSheet.Cells[1, 2] = "Pst Score";

                for (int i = 0; i < proteinsList.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = proteinsList[i].Header;
                    oSheet.Cells[i + 2, 2] = proteinsList[i].PstScore;

                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {

            }
            //...

        }

        public static void DumpModifiedProteins(List<ProteinDto> proteinsList)
        {
            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\ModifiedProteins.xls";
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
                oSheet.Cells[1, 1] = "Header";
                oSheet.Cells[1, 2] = "Mod Count";
                oSheet.Cells[1, 3] = "Ptm Score";


                //oSheet.Cells[1, 4] = "Mod#";                
                //oSheet.Cells[1, 5] = "AminoAcid";
                //oSheet.Cells[1, 6] = "Index";
                //oSheet.Cells[1, 7] = "ModName"; 
                //oSheet.Cells[1, 8] = "ModWeight"; 
                //oSheet.Cells[1, 9] = "Score";
                //oSheet.Cells[1, 10] = "Site";

                int row = 2;

                for (int i = 0; i < proteinsList.Count; i++)
                {
                    oSheet.Cells[row, 1] = proteinsList[i].Header;
                    oSheet.Cells[row, 2] = proteinsList[i].PtmParticulars.Count;
                    oSheet.Cells[row, 3] = proteinsList[i].PtmScore;

                    //for (int j = 0; j < proteinsList[i].PtmParticulars.Count; j++)
                    //{
                    //    oSheet.Cells[row, 4] = j + 1;
                    //    oSheet.Cells[row, 5] = proteinsList[i].PtmParticulars[j].AminoAcid;
                    //    oSheet.Cells[row, 6] = proteinsList[i].PtmParticulars[j].Index;
                    //    oSheet.Cells[row, 7] = proteinsList[i].PtmParticulars[j].ModName;
                    //    oSheet.Cells[row, 8] = proteinsList[i].PtmParticulars[j].ModWeight;
                    //    oSheet.Cells[row, 9] = proteinsList[i].PtmParticulars[j].Score;
                    //    oSheet.Cells[row, 10] = proteinsList[i].PtmParticulars[j].Site;
                    //    row++;
                    //}

                    //if (proteinsList[i].PtmParticulars.Count == 0)
                        row++;

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
                Console.WriteLine(e.Message);
            }
            //...

        }

        public static void DumpInsilicoScores(List<ProteinDto> proteinsList)
        {



            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\InsilicoScores.xls";
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
                oSheet.Cells[1, 1] = "Header";
                oSheet.Cells[1, 2] = "Mod Count";
                oSheet.Cells[1, 3] = "Insilico Score";

                for (int i = 0; i < proteinsList.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = proteinsList[i].Header;
                    oSheet.Cells[i + 2, 2] = proteinsList[i].PtmParticulars.Count;
                    oSheet.Cells[i + 2, 3] = proteinsList[i].InsilicoScore;

                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {

            }
            //...

        }

        public static void DumpTotalScores(List<ProteinDto> proteinsList)
        {

            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\TotalScores.xls";
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
                oSheet.Cells[1, 1] = "Header";
                oSheet.Cells[1, 2] = "Mod Count";
                oSheet.Cells[1, 3] = "Tot Score";
                oSheet.Cells[1, 4] = "Ins Score";
                oSheet.Cells[1, 5] = "Pst Score";
                oSheet.Cells[1, 6] = "Ptm Score";
                oSheet.Cells[1, 7] = "Mw Score";
               

                for (int i = 0; i < proteinsList.Count; i++)
                {
                    oSheet.Cells[i + 2, 1] = proteinsList[i].Header;
                    oSheet.Cells[i + 2, 2] = proteinsList[i].PtmParticulars.Count;
                    oSheet.Cells[i + 2, 3] = proteinsList[i].Score;
                    oSheet.Cells[i + 2, 4] = proteinsList[i].InsilicoScore;
                    oSheet.Cells[i + 2, 5] = proteinsList[i].PstScore;
                    oSheet.Cells[i + 2, 6] = proteinsList[i].PtmScore;
                    oSheet.Cells[i + 2, 7] = proteinsList[i].MwScore;

                }



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {

            }
            //...

        }

        public static void DumpTotalTime(ExecutionTimeDto time)
        {



            Application xlApp = new Application();
            if (xlApp == null) return;

            string excelFile = _directory + "\\Times.xls";
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
                oSheet.Cells[1, 1] = "TotalTime";
                oSheet.Cells[1, 2] = "FileReadingTime";
                oSheet.Cells[1, 3] = "InsilicoTime";
                oSheet.Cells[1, 4] = "MwFilterTime";
                oSheet.Cells[1, 5] = "PstTime";
                oSheet.Cells[1, 6] = "PtmTime";
                oSheet.Cells[1, 7] = "TunerTime";

                
                oSheet.Cells[2, 1] = time.TotalTime;
                oSheet.Cells[2, 2] = time.FileReadingTime;
                oSheet.Cells[2, 3] = time.InsilicoTime;
                oSheet.Cells[2, 4] = time.MwFilterTime;
                oSheet.Cells[2, 5] = time.PstTime;
                oSheet.Cells[2, 6] = time.PtmTime;
                oSheet.Cells[2, 7] = time.TunerTime;
                  

                



                oXL.Visible = false;
                oXL.UserControl = false;
                oWB.SaveAs(excelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault,
                    Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                oWB.Close();
            }
            catch (Exception)
            {

            }
            //...

        }

        public static void ExitPeakFileDirectory()
        {
            _directory = _rootDirectory;
        }

        public static void DumpError(string error)
        {

            using (StreamWriter file =
                new StreamWriter(_directory + "\\error.txt", true))
            {
                file.Write(error);
            }
        }
    }
}
