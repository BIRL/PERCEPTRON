using System.Collections.Generic;

namespace PerceptronAPI.Models
{
    public class ProteinDto
    {
        public string FileName;
        public string Header;
        public string TerminalModification;
        public string Sequence;
        public string TruncationSite;
        public int TruncationIndex;
        public double Score;
        public double Mw;
        public int NoOfPtmModifications;
        public int NoOfFragmentsMatched;
        public double ElapsedTime;
        public double Evalue;

        public ProteinDto()
        {
            FileName = "";
            Header = "";
            TerminalModification = "";
            Sequence = "";
            TruncationSite = "";
            TruncationIndex = 0;
            Score = 0.0;
            Mw = 0.0;
            NoOfPtmModifications = 0;
            NoOfFragmentsMatched = 0;
            ElapsedTime = 0.0;
            Evalue = 0.0;
        }

        public ProteinDto(ProteinDto Protein)
        {

        }

        public ProteinDto(string cFileName, string cHeader, string cTerminalModification, string cSequence, string cTruncationSite, int cTruncationIndex, double cScore,
            double cMw, int cNoOfPtmModifications, int cNoOfFragmentsMatched, double cElapsedTime, double cEvalue)
        {
            FileName = cFileName;
            Header = cHeader;
            TerminalModification = cTerminalModification;
            Sequence = cSequence;
            TruncationSite = cTruncationSite;
            TruncationIndex = cTruncationIndex;
            Score = cScore;
            Mw = cMw;
            NoOfPtmModifications = cNoOfPtmModifications;
            NoOfFragmentsMatched = cNoOfFragmentsMatched;
            ElapsedTime = cElapsedTime;
            Evalue = cEvalue;
        }
      
        //public double EstScore;
        //public double InsilicoScore;
        //public double PtmScore;

        //public double MwScore;
        
        //public List<Sites> PtmParticulars;
        //public InsilicoObj InsilicoDetails;

        //public Proteins()
        //{
        //    Header = "";
        //    Sequence = "";
        //    EstScore = 0;
        //    InsilicoScore = 0;
        //    PtmScore = 0;
        //    Score = 0;
        //    MwScore = 0;
        //    PtmParticulars = new List<Sites>();
        //    InsilicoDetails = new InsilicoObj();
        //}

        //public Proteins(string h, string s, double mw, double mwScore)
        //{
        //    Header = h;
        //    Sequence = s;
        //    EstScore = 0;
        //    MwScore = mwScore;
        //    InsilicoScore = 0;
        //    PtmScore = 0;
        //    Score = 0;
        //    Mw = mw;
        //    //PtmParticulars = new List<Sites>();
        //}
    }
}