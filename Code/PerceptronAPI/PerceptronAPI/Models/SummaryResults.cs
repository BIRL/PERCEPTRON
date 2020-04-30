using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PerceptronAPI.Models
{
    public class SummaryResults
    {
        public string ResultId { get; set; }
        public int ProteinRank { get; set; }
        public string ProteinName { get; set; }
        public string ProteinId { get; set; }
        public double MolW { get; set; }
        public string TerminalMods { get; set; }
        public int Mods { get; set; }
        public double Score { get; set; }
    }
}