using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IPostTranslationalModificationModule
    {
        List<ProteinDto> PTMs_Generator_Insilico_Generator(double Experimentalmz, ProteinDto protein, SearchParametersDto parameters);

        //List<ProteinDto> ExecutePtmModule(List<ProteinDto> input, MsPeaksDto peakData, SearchParametersDto parameters);
    }
}
