using PerceptronLocalService.DTO;
using System.Collections.Generic;

namespace PerceptronLocalService.Interfaces
{
    public interface IWholeProteinMassTuner
    {
        double TuneWholeProteinMass(List<newMsPeaksDto> peakData2DList, SearchParametersDto parameters);
    }
}
