using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class PTMModificationsInfoDto
    {
        public List<FixedModificationInfoDto> FixedModificationInfo = new List<FixedModificationInfoDto>();
        public List<VariableModificationInfoDto> VariableModificationInfo = new List<VariableModificationInfoDto>();
        

        //public PTMModificationsInfoDto()
        //{
        //    FixedModificationInfoDto FixedModificationInfo = new FixedModificationInfoDto();
        //    VariableModificationInfoDto VariableModificationInfo = new VariableModificationInfoDto();
        //}

    }
}
