using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IInsilicoFragmentsAdjustment
    {
        List<ProteinDto> adjustForFragmentTypeAndSpecialIons(List<ProteinDto> prot, string clevageType, string ions);
    }
}
