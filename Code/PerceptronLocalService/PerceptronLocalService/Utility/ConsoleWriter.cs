using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    public class ConsoleWriter
    {
        public static void printPST(List<PstTagsDto> pstTags)
        {
            foreach (var tag in pstTags)
            {
                //Console.WriteLine(tag.StartPosition+"--"+tag.EndPosition+"  : "+tag.AminoAcidTag);
            }
        }
    }
}
