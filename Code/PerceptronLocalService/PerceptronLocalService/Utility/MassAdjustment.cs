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
        public const double H = 1.007825035;
        public const double C = 12.0000;
        public const double O = 15.99491463;
        public const double N = 14.003074;
        public const double OH = O + H;
        public const double CO = C + O;
        public const double NH = N + H;
        public const double NH3 = N + H + H + H;
        public const double H2O = H + H + O;
    }
}
