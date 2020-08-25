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
using Newtonsoft.Json;
using PerceptronAPI.Models;
using PerceptronAPI.Engine;


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

            
            var InsilicoDetails = new InsilicoMassIons(RawData.Results.Results);

            var Results = RawData.Results.Results;

            var InsilicoSpectra = InsilicoSpectraPrep(Results, InsilicoDetails, RawData.searchParameters.InsilicoFragType, PeakListMasses, PeakListIntensities);

            CloseWindowGraphForm();
            return InsilicoSpectra;

        }

        //private InsilicoMassIons InsilicoMassDataPrep(SearchResult Results)
        //{
        //    var InsilicoDetail = new InsilicoMassIons();
        //    InsilicoDetail.InsilicoMassLeftIons = Results.InsilicoMassLeft.Split(',').Select(double.Parse).ToList();
        //    InsilicoDetail.InsilicoMassRightIons = Results.InsilicoMassRight.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassLeftAo != "")
        //        InsilicoDetail.InsilicoMassLeftAo = Results.InsilicoMassLeftAo.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassLeftBo != "")
        //        InsilicoDetail.InsilicoMassLeftBo = Results.InsilicoMassLeftBo.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassLeftAstar != "")
        //        InsilicoDetail.InsilicoMassLeftAstar = Results.InsilicoMassLeftAstar.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassLeftBstar != "")
        //        InsilicoDetail.InsilicoMassLeftBstar = Results.InsilicoMassLeftBstar.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassRightYo != "")
        //        InsilicoDetail.InsilicoMassRightYo = Results.InsilicoMassRightYo.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassRightYstar != "")
        //        InsilicoDetail.InsilicoMassRightYstar = Results.InsilicoMassRightYstar.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassRightZo != "")
        //        InsilicoDetail.InsilicoMassRightZo = Results.InsilicoMassRightZo.Split(',').Select(double.Parse).ToList();

        //    if (Results.InsilicoMassRightZoo != "")
        //        InsilicoDetail.InsilicoMassRightZoo = Results.InsilicoMassRightZoo.Split(',').Select(double.Parse).ToList();

        //    return InsilicoDetail;
        //}

        private AssembleInsilicoSpectra InsilicoSpectraPrep(SearchResult Results, InsilicoMassIons InsilicoDetails, string FragmentationType, List<double> PeakListMasses, List<double> PeakListIntensities)
        {

            var _ExtractInsilicoMass = new ExtractInsilicoMass();
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
                    ListIndices.Add(LeftMatchedIndex[index] + 1);    // +1 is for to over comming Zero Indexing

                    tempTheoretical_mz = _ExtractInsilicoMass.ExtractInsilicoLeftMass(index, ListFragIon, FragmentationType, LeftType[index], LeftMatchedIndex[index], InsilicoDetails);
                    tempTheoretical_mz = Math.Round(tempTheoretical_mz, 4);
                    ListTheoretical_mz.Add(tempTheoretical_mz);

                    tempExperimental_mz = Math.Round( PeakListMasses[LeftPeakIndex[index]] , 4 );
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

                    ListIndices.Add(InsilicoDetails.InsilicoMassLeftIons.Count - RightMatchedIndex[index] + 1);    // +1 is for to over comming Zero Indexing

                    tempTheoretical_mz = _ExtractInsilicoMass.ExtractInsilicoRightMass(index, ListFragIon, FragmentationType, RightType[index], RightMatchedIndex[index], InsilicoDetails);
                    tempTheoretical_mz = Math.Round(tempTheoretical_mz, 4);
                    ListTheoretical_mz.Add(tempTheoretical_mz);

                    tempExperimental_mz = Math.Round ( PeakListMasses[RightPeakIndex[index]] , 4 );
                    ListExperimental_mz.Add(Math.Round(tempExperimental_mz, 4));

                    tempError = Math.Abs(tempExperimental_mz - tempTheoretical_mz);
                    ListAbsError.Add(Math.Round(tempError, 4));
                }

            }

            var InsilicoSpectra = new AssembleInsilicoSpectra(ListIndices, ListFragIon, ListExperimental_mz, ListTheoretical_mz, ListAbsError);
            return InsilicoSpectra;

        }

        //private void ExtractInsilicoLeftMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassIons InsilicoDetails, ref double tempTheoretical_mz)
        //{
        //    if (Type == "Left")
        //    {
        //        if (FragmentationType == "ECD" || FragmentationType == "ETD")
        //        {
        //            ListFragIon.Add("C");
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftIons[MatchedIndex];
        //        }
                    
        //        else if (FragmentationType == "EDD" || FragmentationType == "NETD")
        //        {
        //            ListFragIon.Add("A");
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftIons[MatchedIndex];
        //        }
        //        else
        //        {
        //            ListFragIon.Add("B");
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftIons[MatchedIndex];
        //        }
                    
        //    }
        //    else
        //    {
        //        ListFragIon.Add(Type); /////CHECKING CHECKING WHETHER BUG .... EXISTS....?????

        //        if (Type == "A'")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftAo[MatchedIndex];
        //        }
        //        else if (Type == "B'")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftBo[MatchedIndex];
        //        }
        //        else if (Type == "A*")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftAstar[MatchedIndex];
        //        }
        //        else if (Type == "B*")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassLeftBstar[MatchedIndex];
        //        }

        //    }
        //}
        //private void ExtractInsilicoRightMass(int index, List<string> ListFragIon, string FragmentationType, string Type, int MatchedIndex, InsilicoMassIons InsilicoDetails, ref double tempTheoretical_mz)
        //{
        //    if (Type == "Right")
        //    {
        //        if (FragmentationType == "ECD" || FragmentationType == "ETD")
        //        {
        //            ListFragIon.Add("Z");
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
        //        }
        //        else if (FragmentationType == "EDD" || FragmentationType == "NETD")
        //        {
        //            ListFragIon.Add("X");
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
        //        }
        //        else
        //        {
        //            ListFragIon.Add("Y");
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightIons[MatchedIndex];
        //        }
        //    }
        //    else
        //    {

        //        ListFragIon.Add(Type);

        //        if (Type == "Y'")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightYo[MatchedIndex];
        //        }
        //        else if (Type == "Z'")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightZo[MatchedIndex];
        //        }
        //        else if (Type == "Z''")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightZoo[MatchedIndex];
        //        }
        //        else if (Type == "Y*")
        //        {
        //            tempTheoretical_mz = InsilicoDetails.InsilicoMassRightYstar[MatchedIndex];
        //        }
        //    }
        //}

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
