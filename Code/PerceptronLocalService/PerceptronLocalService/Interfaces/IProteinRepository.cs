using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    interface IProteinRepository
    {
        CandidateProteinListsDto ExtractProteins(double mw, SearchParametersDto parameters, List<PstTagList> PstTags);
    }
}
