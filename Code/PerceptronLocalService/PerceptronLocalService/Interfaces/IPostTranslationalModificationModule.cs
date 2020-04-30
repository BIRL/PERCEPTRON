using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IPostTranslationalModificationModule
    {
        List<ProteinDto> ExecutePtmModule(List<ProteinDto> input, MsPeaksDto peakData, SearchParametersDto parameters);
    }
}
