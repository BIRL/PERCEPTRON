using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    public class ConvertStringListToDoubleList
    {
        public List<double> ConvertStringToDouble(string[] dummySplit)
        {
            List<double> newList = new List<double>(dummySplit.Length);
            double OutDouble = 0.0;
            for (int i = 0; i < dummySplit.Length; i++)
            {
                double.TryParse(dummySplit[i], out OutDouble);
                newList.Add(OutDouble);
            }
            return newList;
        }
    }
}
