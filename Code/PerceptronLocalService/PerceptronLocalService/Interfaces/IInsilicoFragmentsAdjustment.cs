using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IInsilicoFragmentsAdjustment
    {
        void adjustForFragmentTypeAndSpecialIons(List<ProteinDto> prot, string clevageType, string ions);
    }
}
