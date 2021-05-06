using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    interface IPeptideSequenceTagScoring
    {
        void ScoreProteinsByPst(SearchParametersDto parameters, List<PstTagList> pstList, List<ProteinDto> mwProt);   //Updated 20210505

    }
}
