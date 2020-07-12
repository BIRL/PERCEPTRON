using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class PTMDataDto
    {  //20200707
        public string ModName;
        public string AminoAcidName;
        public double End;
        public double Start;
        public int ThrI;

        public PTMDataDto(string cModName, string cAminoAcidName, double cEnd, double cStart, int cThrI)
        {

            ModName = cModName;
            AminoAcidName = cAminoAcidName;
            End = cEnd;
            Start = cStart;
            ThrI = cThrI;

        }
    }
}

