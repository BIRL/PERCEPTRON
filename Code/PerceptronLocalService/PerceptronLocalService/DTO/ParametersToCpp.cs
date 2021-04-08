using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.DTO
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct ParametersToCpp
    {
        public double MwTolerance;
        public double NeutralLoss;
        public double SliderValue;
        public double HopThreshhold;
        public int Autotune;
        public int DenovoAllow;
        public int MinimumPstLength;
        public int MaximumPstLength;
        public string PeptideToleranceUnit;
        public double PeptideTolerance;
        public double PSTTolerance;

        public ParametersToCpp(double MwTolerance, double NeutralLoss, double SliderValue, double HopThreshhold,
            int Autotune, int DenovoAllow, int MinimumPstLength, int MaximumPstLength, string PeptideToleranceUnit,
            double PeptideTolerance, double PSTTolerance)
        {
            this.MwTolerance = MwTolerance;
            this.NeutralLoss = NeutralLoss;
            this.SliderValue = SliderValue;
            this.HopThreshhold = HopThreshhold;
            this.Autotune = Autotune;
            this.DenovoAllow = DenovoAllow;
            this.MinimumPstLength = MinimumPstLength;
            this.MaximumPstLength = MaximumPstLength;
            this.PeptideToleranceUnit = PeptideToleranceUnit;
            this.PeptideTolerance = PeptideTolerance;
            this.PSTTolerance = PSTTolerance;
        }
    }
}
