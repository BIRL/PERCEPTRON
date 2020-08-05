using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IBlindPTMModule
    {
        BlindPTMDto BlindPTMExtraction(List<newMsPeaksDto> peakData2DList, SearchParametersDto parameters);
        List<ProteinDto> BlindPTMGeneral(List<ProteinDto> CandidateProtList, List<newMsPeaksDto> peakData2DList, double UserHopThreshold, BlindPTMDto BlindPTMExtractionInfo, SearchParametersDto parameters, string TypeOfFunction);
        List<ProteinDto> BlindPTMLocalization(List<ProteinDto> Matches, double IntactMass, SearchParametersDto parameters);
        List<ProteinDto> PTMTruncation_Modification(List<ProteinDto> CandidateProtListInput, List<newMsPeaksDto> peakData2DList, SearchParametersDto parameters, string FunctionType);
    }
}
