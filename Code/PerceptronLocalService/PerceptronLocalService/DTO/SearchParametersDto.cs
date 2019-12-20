using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class SearchParametersDto
    {
        public string Queryid;
        public string UserId;
        public string Title;
        public string ProtDb;
        public string InsilicoFragType;
        public int FilterDb;
        public double PtmTolerance;
        public int MinimumPstLength;
        public int MaximumPstLength;        
        public double MwTolerance;
        public string MwTolUnit;
        public double HopThreshhold;
        public string HopTolUnit;
        public double GuiMass;
        public double PeptideTolerance;
        public string PeptideTolUnit;

        public List<int> PtmCodeVar;
        public List<int> PtmCodeFix;
        public string[] FileType;
        public string[] PeakListFileName;

        public int Autotune;
        public string HandleIons;
        public double MwSweight;
        public double PstSweight;
        public double InsilicoSweight;
        public int NumberOfOutputs;
        public int DenovoAllow;
        public int PtmAllow;
        public double? NeutralLoss; //Added 12Sep2019
        public double PSTTolerance; //Added 11Dec2019


        public bool HasFixedAndVariableModifications()
        {
            if (PtmCodeFix == null || PtmCodeVar == null) return false;
            return PtmCodeFix.Count != 0 && PtmCodeVar.Count != 0;
        }
    }
}
