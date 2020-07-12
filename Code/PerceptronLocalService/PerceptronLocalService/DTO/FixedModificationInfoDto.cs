using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class FixedModificationInfoDto
    {
        public int Occurrences;
        public string Site;
        public double MolecularWeight;

        public FixedModificationInfoDto()
        {
            Occurrences = 0;
            Site = "";
            MolecularWeight = 0.0;

        }

        public FixedModificationInfoDto(int cOccurrences, string cSite, double cMolecularWeight)
        {
            Occurrences = cOccurrences;
            Site = cSite;
            MolecularWeight = cMolecularWeight;

        }

    }
}
