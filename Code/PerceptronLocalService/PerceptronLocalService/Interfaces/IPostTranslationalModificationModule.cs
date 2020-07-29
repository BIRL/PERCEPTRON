using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IPostTranslationalModificationModule
    {
        void PTMs_Generator_Insilico_Generator(ProteinDto protein, SearchParametersDto parameters);

        //List<ProteinDto> ExecutePtmModule(List<ProteinDto> input, MsPeaksDto peakData, SearchParametersDto parameters);
    }
}
