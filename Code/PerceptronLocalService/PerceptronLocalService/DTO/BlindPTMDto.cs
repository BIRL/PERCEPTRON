using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    public class BlindPTMDto
    {
        public int sizeHopInfo;
        public List<string> HopInfoName = new List<string>();
        public List<char> HopInfoAA = new List<char>();
        public List<double> HopInfoEnd = new List<double>();
        public List<double> HopInfoStart = new List<double>();

        public BlindPTMDto(int csizeHopInfo, List<string> cHopInfoName, List<char> cHopInfoAA, List<double> cHopInfoEnd, List<double> cHopInfoStart)
        {
            sizeHopInfo = csizeHopInfo;
            HopInfoName = cHopInfoName;
            HopInfoAA = cHopInfoAA;
            HopInfoEnd = cHopInfoEnd;
            HopInfoStart = cHopInfoStart;
        }



    }
}
