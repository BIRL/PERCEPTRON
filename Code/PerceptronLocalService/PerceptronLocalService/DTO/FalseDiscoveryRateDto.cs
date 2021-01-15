using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class FalseDiscoveryRateDto
    {
        public string FileName;
        public string Header;
        public string TerminalModification;
        public string Sequence;
        public string Truncation;
        public int TruncationIndex;
        public double Score;
        public double Mw;
        public int NumOfModifications;
        public int MatchedFragments;
        public string RunTime;
        public double Evalue;
        public List<decimal> FdrValue = new List<decimal>();    // Updated 20210213
        public List<FalseDiscoveryRateDto> BatchTargetList = new List<FalseDiscoveryRateDto>();
        public int EvalueCount;  // EvalueCount is different to the evalue i.e. the evalue of protein (Evalue)
        public int NoOfProteins;
        public int NoOfUniqueProteins;
        public int TargetListCount;   // Updated 20210213

        public FalseDiscoveryRateDto(string cFileName, string cHeader, string cTerminalModification, string cSequence, string cTruncation, int cTruncationIndex, double cScore, double cMw, int cNumOfModifications, int cMatchedFragments, string cRunTime, double cEvalue)
        {
            FileName = cFileName;
            Header = cHeader;
            TerminalModification = cTerminalModification;
            Sequence = cSequence;
            Truncation = cTruncation;
            TruncationIndex = cTruncationIndex;
            Score = cScore;
            Mw = cMw;
            NumOfModifications = cNumOfModifications;
            MatchedFragments = cMatchedFragments;
            RunTime = cRunTime;
            Evalue = cEvalue;
        }

        public FalseDiscoveryRateDto(int cTargetListCount, List<FalseDiscoveryRateDto> cBatchTargetList, List<decimal> cFdrValue, int cNoOfProteins, int cNoOfUniqueProteins, int cEvalueCount)   // Updated 20210213
        {
            TargetListCount = cTargetListCount;   // Updated 20210213
            BatchTargetList = cBatchTargetList;
            FdrValue = cFdrValue;
            NoOfProteins = cNoOfProteins;
            NoOfUniqueProteins = cNoOfUniqueProteins;
            EvalueCount = cEvalueCount;
        }

    }
}
