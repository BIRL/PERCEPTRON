using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    public class TruncationMessage
    {
        public string TypeOfTruncation(string Truncation)
        {
            var Truncation_Message = "";
            if (Truncation == "Left")
            {
                Truncation_Message = "Truncation at N-Terminal Side";
            }
            else if (Truncation == "Right")
            {
                Truncation_Message = "Truncation at C-Terminal Side";
            }
            else
            {
                Truncation_Message = "No Truncation";
            }
            return Truncation_Message;
        }
    }
}
