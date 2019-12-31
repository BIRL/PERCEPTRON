using System.Collections.Generic;

namespace PerceptronAPI.Models
{
    public class QuerryParameters
    {
        public string QueryId;
        public string UserId;
        public string Title;
        public string ProtDb;
        public string InsilicoFragType;
        public int FilterDb;
        public int MinimumEstLength;
        public int MaximumEstLength;
        public double PtmTolerance;
        public double MwTolerance;
        public double NeutralLoss;  //NeutralLoss Added!!!
        public string MwTolUnit;
        public double HopThreshhold;
        public string HopTolUnit;
        public double GuiMass;
        public List<int> PtmCodeVar;
        public List<int> PtmCodeFix;
        public string FileType;
        public string[] PeakListFile;
        public int Autotune;
        public string HandleIons;
        public double MwSweight;
        public double PstSweight;
        public double InsilicoSweight;
        public int NumberOfOutputs;
        public int DenovoAllow;
        public int PtmAllow;
        public double PSTTolerance; //PSTTolerance Added!!!

        public double PeptideTolerance;
        public string PeptideToleranceUnit;
    }
}