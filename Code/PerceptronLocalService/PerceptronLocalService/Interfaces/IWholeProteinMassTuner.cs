using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IWholeProteinMassTuner
    {
        void TuneWholeProteinMass(MsPeaksDto peakData, SearchParametersDto parameters);
        
    }
}
