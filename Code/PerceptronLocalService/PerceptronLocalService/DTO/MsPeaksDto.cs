using System.Collections.Generic;

namespace PerceptronLocalService.DTO
{
    public class MsPeaksDto
    {
        public List<double> Intensity;
        public List<double> Mass;
        public double WholeProteinMolecularWeight;

        public MsPeaksDto()
        {
            Intensity = new List<double>();
            Mass = new List<double>();
        }

        public MsPeaksDto(List<double> intensity, List<double> massList, double mw )
        {
            Intensity = intensity;
            Mass = massList;
            WholeProteinMolecularWeight = mw;
        }
    }
}
