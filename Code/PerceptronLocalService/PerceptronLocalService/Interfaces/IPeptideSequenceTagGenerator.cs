using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    interface IPeptideSequenceTagGenerator
    {
        List<PstTagList> GeneratePeptideSequenceTags(SearchParametersDto parameters, MsPeaksDto peakData);
    }
}
