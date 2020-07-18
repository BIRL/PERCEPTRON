using System.Collections.Generic;
using PerceptronAPI.Models;
using PerceptronAPI.Engine;

namespace PerceptronAPI
{
    internal interface IDataAccessLayer
    {
        List<ScanResults> Scan_Results(string qid);
        List<SummaryResults> Summary_results(string qid, string fid);
        DetailedResults Detailed_Results(string qid, string rid);
        DetailedProteinHitView DetailedProteinHitView_Results(string qid, string rid);
        List<UserHistory> GetUserHistory(string Uid);
        stat stat();
        void StoringCompiledResults(List<ResultsDownloadDataCompile> CompiledResults);
        SearchParameter GetSearchParmeters(string qid);
        //MassSpectrumImagedetail GetImagePathMassSpectrum(string qid);
    }
}
