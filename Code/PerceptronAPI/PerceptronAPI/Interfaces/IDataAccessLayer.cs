using System.Collections.Generic;
using PerceptronAPI.Models;

namespace PerceptronAPI
{
    internal interface IDataAccessLayer
    {
        List<ScanResults> Scan_Results(string qid);
        List<SummaryResults> Summary_results(string qid, string fid);
        DetailedResults Detailed_Results(string qid, string rid);
        List<UserHistory> GetUserHistory(string Uid);
        stat stat();
    }
}
