using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PerceptronAPI.Models;
using Newtonsoft.Json;


namespace GraphForm
{
    public partial class FormForGraph : Form
    {
        public FormForGraph()
        {
            InitializeComponent();
        }

        //private void Form11(object sender, EventArgs e)
        //{
        //    fillChart();

        //}

        public AssembleInsilicoSpectra fillChart(DetailedProteinHitView RawData)
        {
            /* Preparation of Data */
            List<double> PeakListMasses = RawData.PeakListData.PeakListMasses.Split(',').Select(double.Parse).ToList();
            List<double> PeakListIntensities = RawData.PeakListData.PeakListIntensities.Split(',').Select(double.Parse).ToList();
            var InsilicoDetails = InsilicoMassDataPrep(RawData.Results.Results);

            var Results = RawData.Results.Results;

            var InsilicoSpectra = InsilicoSpectraPrep(Results, InsilicoDetails, RawData.searchParameters.InsilicoFragType, PeakListMasses, PeakListIntensities);


            //For the Time Being Converting into Json
            //var json = JsonConvert.SerializeObject(InsilicoSpectra);
            //return json;


            //var abc = InsilicoSpectra.ListExperimental_mz;
            //Chart chart = new Chart();
            //chart.DataSource = InsilicoSpectra.ListExperimental_mz;

            //for (int i = 0; i < InsilicoSpectra.ListExperimental_mz.Count; i++)
            //{
            //    MassSpectraVisualizationGraph.Series[0].Points.AddXY(InsilicoSpectra.ListExperimental_mz[i], 1);
            //    MassSpectraVisualizationGraph.Series[1].Points.AddXY(InsilicoSpectra.ListTheoretical_mz[i], 1);

            //}
            //MassSpectraVisualizationGraph.Series[0].ChartType = SeriesChartType.Point;
            //MassSpectraVisualizationGraph.Series[1].ChartType = SeriesChartType.Line;
            //for (int i = 0; i < PeakListMasses.Count; i++)
            //{
            //    MassSpectraVisualizationGraph.Series[2].Points.AddXY(PeakListMasses[i], 1);
            //}
            ////MassSpectraVisualizationGraph.Series[2].ChartType = SeriesChartType.Point;

            ////chart title  
            //MassSpectraVisualizationGraph.Titles.Add("Mass Spectrum Visualization");

            //MassSpectraVisualizationGraph.SaveImage(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Engine\mychart.png", ChartImageFormat.Png);
            CloseWindowGraphForm();
            return InsilicoSpectra;

        }

        private InsilicoMassIons InsilicoMassDataPrep(SearchResult Results)
        {
            var InsilicoDetail = new InsilicoMassIons();
            InsilicoDetail.InsilicoMassLeftIons = Results.InsilicoMassLeft.Split(',').Select(double.Parse).ToList();
            InsilicoDetail.InsilicoMassRightIons = Results.InsilicoMassRight.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftAo != "")
                InsilicoDetail.InsilicoMassLeftAo = Results.InsilicoMassLeftAo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftBo != "")
                InsilicoDetail.InsilicoMassLeftBo = Results.InsilicoMassLeftBo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftAstar != "")
                InsilicoDetail.InsilicoMassLeftAstar = Results.InsilicoMassLeftAstar.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassLeftBstar != "")
                InsilicoDetail.InsilicoMassLeftBstar = Results.InsilicoMassLeftBstar.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightYo != "")
                InsilicoDetail.InsilicoMassRightYo = Results.InsilicoMassRightYo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightYstar != "")
                InsilicoDetail.InsilicoMassRightYstar = Results.InsilicoMassRightYstar.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightZo != "")
                InsilicoDetail.InsilicoMassRightZo = Results.InsilicoMassRightZo.Split(',').Select(double.Parse).ToList();

            if (Results.InsilicoMassRightZoo != "")
                InsilicoDetail.InsilicoMassRightZoo = Results.InsilicoMassRightZoo.Split(',').Select(double.Parse).ToList();

            return InsilicoDetail;
        }

        private AssembleInsilicoSpectra InsilicoSpectraPrep(SearchResult Results, InsilicoMassIons InsilicoDetails, string FragmentationType, List<double> PeakListMasses, List<double> PeakListIntensities)
        {
            var ListIndices = new List<int>();
            var ListFragIon = new List<string>();
            var ListExperimental_mz = new List<double>();
            var ListTheoretical_mz = new List<double>();
            var ListAbsError = new List<double>();

            double tempTheoretical_mz = 0.0;
            double tempExperimental_mz;
            double tempError;


            if (Results.LeftMatchedIndex != "")
            {
                var LeftMatchedIndex = Results.LeftMatchedIndex.Split(',').Select(int.Parse).ToList();
                var LeftPeakIndex = Results.LeftPeakIndex.Split(',').Select(int.Parse).ToList();
                var LeftType = Results.LeftType.Split(',').ToList();
                //string Site = LeftType;

                for (int index = 0; index < LeftMatchedIndex.Count; index++)
                {
                    ListIndices.Add(LeftMatchedIndex[index]);

                    ExtractInsilicoLeftMass(index, ListFragIon, FragmentationType, LeftType[index], LeftMatchedIndex[index], InsilicoDetails, Results, ref tempTheoretical_mz);
                    ListTheoretical_mz.Add(Math.Round(tempTheoretical_mz, 4));

                    tempExperimental_mz = PeakListMasses[LeftPeakIndex[index]];
                    ListExperimental_mz.Add(Math.Round(tempExperimental_mz, 4));

                    tempError = Math.Abs(tempExperimental_mz - tempTheoretical_mz);
                    ListAbsError.Add(Math.Round(tempError, 4));
                }
            }
            if (Results.RightMatchedIndex != "")
            {
                var RightMatchedIndex = Results.RightMatchedIndex.Split(',').Select(int.Parse).ToList();
                var RightPeakIndex = Results.RightPeakIndex.Split(',').Select(int.Parse).ToList();
                var RightType = Results.RightType.Split(',').ToList();

                for (int index = RightMatchedIndex.Count - 1; index > -1; index--)  //int index = 0; index < RightMatchedIndex.Count; index++
                {

                    ListIndices.Add(InsilicoDetails.InsilicoMassLeftIons.Count - RightMatchedIndex[index] - 1); // -1 is for Zero Indexing

                    ExtractInsilicoRightMass(index, ListFragIon, FragmentationType, RightType[index], RightMatchedIndex[index], InsilicoDetails, Results, ref tempTheoretical_mz);
                    ListTheoretical_mz.Add(Math.Round(tempTheoretical_mz, 4));

                    tempExperimental_mz = PeakListMasses[RightPeakIndex[index]];
                    ListExperimental_mz.Add(Math.Round(tempExperimental_mz, 4));

                    tempError = Math.Abs(tempExperimental_mz - tempTheoretical_mz);
                    ListAbsError.Add(Math.Round(tempError, 4));
                }

            }

            var InsilicoSpectra = new AssembleInsilicoSpectra(ListIndices, ListFragIon, ListExperimental_mz, ListTheoretical_mz, ListAbsError);
            return InsilicoSpectra;

        }

        private void ExtractInsilicoLeftMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassIons InsilicoDetails, SearchResult Results, ref double tempTheoretical_mz)
        {
            if (Type == "Left")
            {
                if (FragmentationType == "ECD" || FragmentationType == "ETD")
                    ListFragIon.Add("C");
                else if (FragmentationType == "EDD" || FragmentationType == "NETD")
                    ListFragIon.Add("A");
                else
                    ListFragIon.Add("B");
            }
            else
            {
                ListFragIon.Add(Type); /////CHECKING CHECKING WHETHER BUG .... EXISTS....?????

                if (Type == "A'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftAo[MatchedIndex];
                }
                else if (Type == "B'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftBo[MatchedIndex];
                }
                else if (Type == "A*")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftAstar[MatchedIndex];
                }
                else if (Type == "B*")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftBstar[MatchedIndex];
                }

            }
        }
        private void ExtractInsilicoRightMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassIons InsilicoDetails, SearchResult Results, ref double tempTheoretical_mz)
        {
            if (Type == "Right")
            {
                if (FragmentationType == "ECD" || FragmentationType == "ETD")
                {
                    ListFragIon.Add("Z");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
                }
                else if (FragmentationType == "EDD" || FragmentationType == "NETD")
                {
                    ListFragIon.Add("X");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
                }
                else
                {
                    ListFragIon.Add("Y");
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
                }
            }
            else
            {

                ListFragIon.Add(Type);

                if (Type == "Y'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightYo[MatchedIndex];
                }
                else if (Type == "Z'")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightZo[MatchedIndex];
                }
                else if (Type == "Z''")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightZoo[MatchedIndex];
                }
                else if (Type == "Y*")
                {
                    tempTheoretical_mz = InsilicoDetails.InsilicoMassRightYstar[MatchedIndex];
                }
            }
        }

        private void CloseWindowGraphForm()
        {
            var tmr = new System.Windows.Forms.Timer();
            tmr.Tick += delegate
            {
                this.Close();
            };
            tmr.Interval = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
            tmr.Start();
        }


    }
}
