using System;
using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IDataAccessLayer
    {
        //string GetEmailFromUserId(string id);
        //void InsertLeftInsilicoMasses(string pid, double mw, string ions);
        //void InsertRightIsilicoMasses(string pid, double mw, string ions);

        void StoreZipResultsForDownload(string Queryid, string ZipFileName, string ZipFileWithQueryId, DateTime? JobSubmission);
        void StorePeakList(string FileUniqueId, string peakDataMassesString, string peakDataIntensitiesString, DateTime? JobSubmission);
        //string GetCreationTime(string qid);   // HOLD FOR KNOW..
        string StoreResults(SearchResultsDto res, string fileName, string FileUniqueId, int fileId, DateTime? JobSubmission);
        List<SearchQueryDto> ServerStatus();
        SearchParametersDto GetParameters(string qid);
        //void GetFiles(SearchParametersDto qp);
        int Set_Progress(string qid, int p);
    }
}
