using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{

    public class PTMModificationsInfoDto
    {
        public int Occurrences;
        public string Site;
        public double MolecularWeight;
        public List<PTMModificationsInfoDto> ListPTMModificationsInfo = new List<PTMModificationsInfoDto>();

        public PTMModificationsInfoDto()
        {
            Occurrences = 0;
            Site = "";
            MolecularWeight = 0.0;
            ListPTMModificationsInfo = new List<PTMModificationsInfoDto>();

        }

        public PTMModificationsInfoDto(int cOccurrences, string cSite, double cMolecularWeight)
        {
            Occurrences = cOccurrences;
            Site = cSite;
            MolecularWeight = cMolecularWeight;

        }

    }
}

//public class PTMModificationsInfoDto
//{
//    public List<FixedModificationInfoDto> FixedModificationInfo = new List<FixedModificationInfoDto>();
//    public List<VariableModificationInfoDto> VariableModificationInfo = new List<VariableModificationInfoDto>();

//}
