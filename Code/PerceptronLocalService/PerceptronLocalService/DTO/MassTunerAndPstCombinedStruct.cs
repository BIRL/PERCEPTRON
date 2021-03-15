using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MassTunerAndPstCombinedStruct
    {
        public int PstTagLength;
        public double PstErrorScore;
        public double PstFrequency;
        public double MassTuner;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] PstTags;
        //public MassTunerAndPstCombinedStruct (int PstTagLength, char[] PstTags, double PstErrorScore, double PstFrequency, double MassTuner)
        //{
        //    this.PstTagLength = PstTagLength;
        //    this.PstTags = PstTags;
        //    this.PstErrorScore = PstErrorScore;
        //    this.PstFrequency = PstFrequency;
        //    this.MassTuner = MassTuner;
        //}
    }
}
