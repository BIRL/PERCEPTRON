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

        public void fillChart(DetailedProteinHitView RawData)
        {
            /* Preparation of Data */
            List<double> PeakListMasses = RawData.PeakListData.PeakListMasses.Split(',').Select(double.Parse).ToList();
            List<double> PeakListIntensities = RawData.PeakListData.PeakListIntensities.Split(',').Select(double.Parse).ToList();
            var InsilicoDetails = InsilicoMassDataPrep(RawData.Results.Results);

            var Results = RawData.Results.Results;

            var InsilicoSpectra = InsilicoSpectraPrep(Results, InsilicoDetails, RawData.searchParameters.InsilicoFragType, PeakListMasses, PeakListIntensities);


            //AddXY value in chart1 in series named as Salary  
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Test", "10000");

            //chart title  
            MassSpectraVisualizationGraph.Titles.Add("Mass Spectrum Visualization");

            MassSpectraVisualizationGraph.SaveImage(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Utility\mychart.png", ChartImageFormat.Png);
            CloseWindowGraphForm();

        }

        private InsilicoMassSiteFragIon InsilicoMassDataPrep(SearchResult Results)
        {
            var InsilicoDetail = new InsilicoMassSiteFragIon();
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

        private AssembleInsilicoSpectra InsilicoSpectraPrep(SearchResult Results, InsilicoMassSiteFragIon InsilicoDetails, string FragmentationType, List<double> PeakListMasses, List<double> PeakListIntensities)
        {
            var ListIndices = new List<int>();
            var ListFragIon = new List<string>();
            var ListExperimental_mz = new List<double>();
            var ListTheoretical_mz = new List<double>();
            var ListAbsError = new List<double>();

            double tempTheoretical_mz;
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

                    tempTheoretical_mz = ExtractInsilicoLeftMass(index, ListFragIon, FragmentationType, LeftType[index], LeftMatchedIndex[index], InsilicoDetails, Results);
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
                string Site = "Right";
                for (int index = 0; index < RightMatchedIndex.Count; index++)  //(int index = RightMatchedIndex.Count; index > RightMatchedIndex.Count; index--)
                {

                    ListIndices.Add(RightMatchedIndex[index]);

                    tempTheoretical_mz = ExtractInsilicoRightMass(index, ListFragIon, FragmentationType, RightType[index], RightMatchedIndex[index], InsilicoDetails, Results);
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

        private double ExtractInsilicoLeftMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassSiteFragIon InsilicoDetails, SearchResult Results)
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
                    return InsilicoDetails.InsilicoMassLeftAo[MatchedIndex];
                }
                else if (Type == "B'")
                {
                    return InsilicoDetails.InsilicoMassLeftBo[MatchedIndex];
                }
                else if (Type == "A*")
                {
                    return InsilicoDetails.InsilicoMassLeftAstar[MatchedIndex];
                }
                else if (Type == "B*")
                {
                    return InsilicoDetails.InsilicoMassLeftBstar[MatchedIndex];
                }

            }
            return 0.0; // It will never ever called
        }
        private double ExtractInsilicoRightMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassSiteFragIon InsilicoDetails, SearchResult Results)
        {
            if (Type == "Right")
            {
                if (FragmentationType == "ECD" || FragmentationType == "ETD")
                    ListFragIon.Add("Z");
                else if (FragmentationType == "EDD" || FragmentationType == "NETD")
                    ListFragIon.Add("X");
                else
                    ListFragIon.Add("Y");
            }
            else
            {
                //name = [name; Match.RightType{RightIndex,1}];
                ListFragIon.Add(Type); //Concept not clear ..................Concept not clear ..................Concept not clear ..................

                if (Type == "Y'")
                {
                    return InsilicoDetails.InsilicoMassRightYo[MatchedIndex];
                }
                else if (Type == "Z'")
                {
                    return InsilicoDetails.InsilicoMassRightZo[MatchedIndex];
                }
                else if (Type == "Z''")
                {
                    return InsilicoDetails.InsilicoMassRightZoo[MatchedIndex];
                }
                else if (Type == "Y*")
                {
                    return InsilicoDetails.InsilicoMassRightYstar[MatchedIndex];
                }
            }
            return 0.0; // It will never ever called
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
