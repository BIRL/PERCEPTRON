using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    interface IPeptideSequenceTagScoring
    {
        void ScoreProteinsByPst(List<PstTagList> pstList, List<ProteinDto> mwProt);
  
    }
}
