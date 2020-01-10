﻿namespace PerceptronAPI.Models
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
    }
}