using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class CandidateProteinListsDto
    {
        public List<ProteinDto> CandidateProteinList = new List<ProteinDto>();
        public List<ProteinDto> CandidateProteinListTruncated = new List<ProteinDto>();

        public CandidateProteinListsDto()
        {
            CandidateProteinList = new List<ProteinDto>();
            CandidateProteinListTruncated = new List<ProteinDto>();
        }
    }
}
