using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class NoOfPtmModifications
    {
        public int NoOfPtmModificationsCount(int NoPtmModificaitons, List<PostTranslationModificationsSiteDto> PtmModifications)
        {
            if (PtmModifications.Count != 0)
            {
                NoPtmModificaitons = PtmModifications.Count;
            }
            return NoPtmModificaitons;
        }
    }
}
