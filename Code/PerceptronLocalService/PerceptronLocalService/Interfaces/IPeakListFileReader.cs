using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IPeakListFileReader
    {
        MsPeaksDto PeakListReader(SearchParametersDto parameters, int fileNumber);
    }
}