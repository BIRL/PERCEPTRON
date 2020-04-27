//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PerceptronAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SearchParameter
    {
        public string QueryId { get; set; }
        public string EmailId { get; set; }
        public string Title { get; set; }
        public string ProtDb { get; set; }
        public string InsilicoFragType { get; set; }
        public int FilterDb { get; set; }
        public double PtmTolerance { get; set; }
        public int MinimumPstLength { get; set; }
        public int MaximumPstLength { get; set; }
        public double MwTolerance { get; set; }
        public string MwTolUnit { get; set; }
        public double HopThreshhold { get; set; }
        public string HopTolUnit { get; set; }
        public double GuiMass { get; set; }
        public int Autotune { get; set; }
        public string HandleIons { get; set; }
        public double MwSweight { get; set; }
        public double PstSweight { get; set; }
        public double InsilicoSweight { get; set; }
        public int NumberOfOutputs { get; set; }
        public int DenovoAllow { get; set; }
        public int PtmAllow { get; set; }
        public double NeutralLoss { get; set; }
        public double PSTTolerance { get; set; }
        public double PeptideTolerance { get; set; }
        public string PeptideToleranceUnit { get; set; }
        public int Truncation { get; set; }
        public string TerminalModification { get; set; }
        public double SliderValue { get; set; }
        public string CysteineChemicalModification { get; set; }
        public string MethionineChemicalModification { get; set; }
    }
}
