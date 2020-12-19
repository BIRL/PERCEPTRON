using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    public class NoOfMatchedFragments
    {
        public int NoOfMatchedFragmentsCount(int Matches, List<int> LeftMatchedIndex, List<int> RightMatchedIndex)
        {
            if (LeftMatchedIndex.Count != 0)
            {
                Matches = LeftMatchedIndex.Count;
            }
            else if (RightMatchedIndex.Count != 0)
            {
                Matches = Matches + RightMatchedIndex.Count;
            }
            return Matches;
            
        }
    }
}
