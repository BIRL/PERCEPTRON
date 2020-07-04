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
        private double ExtractInsilicoMass(int index, string Site, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, SearchResult Results)
        {
            if (Site == "Left")
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
                        return Results.InsilicoMassLeftAo[MatchedIndex];
                    }
                    else if (Type == "B'")
                    {
                        return Results.InsilicoMassLeftBo[MatchedIndex];
                    }
                    else if (Type == "A*")
                    {
                        return Results.InsilicoMassLeftAstar[MatchedIndex];
                    }
                    else if (Type == "B*")
                    {
                        return Results.InsilicoMassLeftBstar[MatchedIndex];
                    }

                }

            }
            else if (Site == "Right")
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
                else{
                    //name = [name; Match.RightType{RightIndex,1}];
                    ListFragIon.Add(Type); //Concept not clear ..................Concept not clear ..................Concept not clear ..................

                    if (Type == "Y'")
                    {
                        return Results.InsilicoMassRightYo[MatchedIndex];
                    }
                    else if (Type == "Z'")
                    {
                        return Results.InsilicoMassRightZo[MatchedIndex];
                    }
                    else if (Type == "Z''")
                    {
                        return Results.InsilicoMassRightZoo[MatchedIndex];
                    }
                    else if (Type == "Y*")
                    {
                        return Results.InsilicoMassRightYstar[MatchedIndex];
                    }
                }
            }
            double NothingIsReturned = 0.0;
            return NothingIsReturned;

        }


        private AssembleInsilicoSpectra InsilicoSpectraPrep(SearchResult Results, string FragmentationType, List<double> PeakListMasses, List<double> PeakListIntensities)
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
                var LeftPeakIndex = Results.LeftMatchedIndex.Split(',').Select(int.Parse).ToList();
                var LeftType = Results.LeftType.Split(',').ToList();
                string Site = "Left";

                for (int index = 0; index < LeftMatchedIndex.Count; index++)
                {
                    ListIndices.Add(LeftMatchedIndex[index]);

                    tempTheoretical_mz = ExtractInsilicoMass(index, Site, ListFragIon, FragmentationType, LeftType[index], LeftMatchedIndex[index], Results);
                    ListTheoretical_mz.Add(tempTheoretical_mz);

                    tempExperimental_mz = PeakListMasses[Results.LeftPeakIndex[index]];
                    ListExperimental_mz.Add(tempExperimental_mz);

                    tempError = Math.Abs(tempExperimental_mz - tempTheoretical_mz);


                }
            }
            if (Results.RightMatchedIndex != "")
            {
                var RightMatchedIndex = Results.RightMatchedIndex.Split(',').Select(int.Parse).ToList();
                var RightPeakIndex = Results.RightMatchedIndex.Split(',').Select(int.Parse).ToList();
                var RightType = Results.RightType.Split(',').ToList();
                string Site = "Right";
                for (int index = 0; index < RightMatchedIndex.Count; index++)
                {

                    ListIndices.Add(RightMatchedIndex[index]);

                    tempTheoretical_mz = ExtractInsilicoMass(index, Site, ListFragIon, FragmentationType, RightType[index], RightMatchedIndex[index], Results);
                    ListTheoretical_mz.Add(tempTheoretical_mz);

                    tempExperimental_mz = PeakListMasses[Results.RightPeakIndex[index]];
                    ListExperimental_mz.Add(tempExperimental_mz);

                    tempError = Math.Abs(tempExperimental_mz - tempTheoretical_mz);
                    ListAbsError.Add(tempError);
                }

            }

            var InsilicoSpectra = new AssembleInsilicoSpectra(ListIndices, ListFragIon, ListExperimental_mz, ListTheoretical_mz, ListAbsError);
            return InsilicoSpectra;

        }

        public void fillChart(DetailedProteinHitView RawData)
        {
            /* Preparation of Data */

            List<double> PeakListMasses = RawData.PeakListData.PeakListMasses.Split(',').Select(double.Parse).ToList();
            List<double> PeakListIntensities = RawData.PeakListData.PeakListIntensities.Split(',').Select(double.Parse).ToList();

            var Results = RawData.Results.Results;
            //List<double> Experimental_mz = RawData.Results.Results.

            var InsilicoSpectra = InsilicoSpectraPrep(Results, RawData.searchParameters.InsilicoFragType, PeakListMasses, PeakListIntensities);


            //AddXY value in chart1 in series named as Salary  
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Ajay", "10000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Ramesh", "8000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Ankit", "7000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Gurmeet", "10000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Suresh", "8500");
            //chart title  
            MassSpectraVisualizationGraph.Titles.Add("Mass Spectrum Visualization");

            MassSpectraVisualizationGraph.SaveImage(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Utility\mychart.png", ChartImageFormat.Png);
            CloseWindowGraphForm();

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
