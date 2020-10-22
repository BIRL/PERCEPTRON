using System.Collections.Generic;
namespace PerceptronAPI.Models
{
    public class ScanResults
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string ProteinId { get; set; }
        public double Score { get; set; }
        public double MolW { get; set; }
        public string Truncation { get; set; }
        public int Frags { get; set; }
        public int Mods { get; set; }
        public string FileUniqueId { get; set; }
        //public string SearchModeMessage = "BatchMode"; //Initializing with BatchMode Message but it will be updated if query will be the Single Mode Search.
    }
}