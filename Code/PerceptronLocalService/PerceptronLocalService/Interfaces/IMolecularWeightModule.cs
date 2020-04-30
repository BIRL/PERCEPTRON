using System.Collections.Generic;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Interfaces
{
    public interface IMolecularWeightModule
    {
       // List<ProteinDto> Fasta_Reader(double mw, SearchParametersDto parameters);
       // double MW_filter(double mw, double tol, double mwExperimental, bool fasta);
        void FilterModifiedProteinsByWholeProteinMass(SearchParametersDto parameters, List<ProteinDto> shortlistedProteins, MsPeaksDto peakData);
    }
}
