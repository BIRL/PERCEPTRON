using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IDataAccessLayer
    {
        //string GetEmailFromUserId(string id);
        //void InsertLeftInsilicoMasses(string pid, double mw, string ions);
        //void InsertRightIsilicoMasses(string pid, double mw, string ions);
        string StoreResults(SearchResultsDto res, string fileName, int fileId);
        List<SearchQueryDto> ServerStatus();
        SearchParametersDto GetParameters(string qid);
        //void GetFiles(SearchParametersDto qp);
        int Set_Progress(string qid, int p);
    }
}
