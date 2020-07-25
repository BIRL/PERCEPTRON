using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IInsilicoFilter
    {
        List<ProteinDto> ComputeInsilicoScore(List<ProteinDto> proteinList, List<newMsPeaksDto> peakData2DList, double tol, string pepUnit);
    }
}
