using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    public class MassAdjustment 
    {
        public const double Proton = 1.00727647;
        public const double H = 1.007825035;  // 1.0078250321??
        public const double C = 12.0000;
        public const double O = 15.9949146221;   // 15.99491463
        public const double N = 14.003074;
        public const double OH = O + H;
        public const double CO = C + O;
        public const double NH = N + H;
        public const double NH3 = N + H + H + H;
        public const double H2O = H + H + O;
    }
}

// Engine>TruncationCPU.cs
//    Proton = 1.00727647;
//    H = 1.007825035;
//    C = 12.0000;
//    O = 15.99491463;
//    N = 14.003074;
//    Electron = 0.000549; %#ok<NASGU>
//    OH = O + H;
//    CO = C + O;
//    NH = N + H;
//    NH3 = N+H+H+H;
//    H2O = H+H+O;
