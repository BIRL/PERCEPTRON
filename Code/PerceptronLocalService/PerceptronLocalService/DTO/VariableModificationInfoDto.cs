using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class VariableModificationInfoDto
    {
        public int Occurrences;
        public string Site;
        public double MolecularWeight;


        public VariableModificationInfoDto()
        {
            Occurrences = 0;
            Site = "";
            MolecularWeight = 0.0;

        }


        public VariableModificationInfoDto(int cOccurrences, string cSite, double cMolecularWeight)
        {
            Occurrences = cOccurrences;
            Site = cSite;
            MolecularWeight = cMolecularWeight;

        }
    }
}
