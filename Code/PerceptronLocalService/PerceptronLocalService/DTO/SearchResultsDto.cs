using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class SearchResultsDto
    {
        public string QueryId;
        public List<ProteinDto> FinalProt;
        public ExecutionTimeDto Times;

        public SearchResultsDto(string qId, List<ProteinDto> prt, ExecutionTimeDto t)
        {
            QueryId = qId;
            FinalProt = prt;
            Times = t;
        }
        public SearchResultsDto()
        {
            FinalProt = new List<ProteinDto>();
            Times = new ExecutionTimeDto();
        }
    }
}
