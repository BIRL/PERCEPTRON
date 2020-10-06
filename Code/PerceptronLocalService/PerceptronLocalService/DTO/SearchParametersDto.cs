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
        public string MassMode;
        public string InsilicoFragType; // Its Specical Ions
        public string FilterDb;
        public double PtmTolerance;
        public int MinimumPstLength;
        public int MaximumPstLength;        
        public double MwTolerance;
        public string MwTolUnit;
        public double HopThreshhold;
        public string HopTolUnit;

        //public double GuiMass;
        //public string PeptideTolUnit;

        public List<int> PtmCodeVar;  //to be deleted after whole integration
        public List<int> PtmCodeFix;  ////to be deleted after whole integration
        public List<string> VariableModifications;
        public List<string> FixedModifications;
        public string[] FileType;
        public string[] PeakListFileName;
        public string[] PeakListUniqueFileNames;
        public string[] FileUniqueIdArray;

        public string Autotune;
        public string HandleIons;
        public double MwSweight;
        public double PstSweight;
        public double InsilicoSweight;
        public string NumberOfOutputs;
        public string DenovoAllow;
        public string PtmAllow;

        public double NeutralLoss; //  UPDATED: 20200121  -  public double? NeutralLoss; //Added 12Sep2019
        public double PSTTolerance; //Added 11Dec2019

        public double PeptideTolerance;
        public string PeptideToleranceUnit;

        public string TerminalModification;
        public string Truncation;

        public double SliderValue;  //20200121
        public string CysteineChemicalModification;
        public string MethionineChemicalModification;
        public string EmailId;



        public bool HasFixedAndVariableModifications()
        {
            if (PtmCodeFix == null || PtmCodeVar == null) return false;
            return PtmCodeFix.Count != 0 && PtmCodeVar.Count != 0;
        }
    }
}
