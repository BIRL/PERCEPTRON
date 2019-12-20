using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IInsilicoFilter
    {
        void ComputeInsilicoScore(List<ProteinDto> proteinList, List<double> peakList, double tol);
    }
}
