using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    public class TruncationIndexed  //C# is Zero Indexed so this class is for removing Zero Indexing by adding 1
    {
        public int TruncationIndxing(int TruncationIndex)
        {
            if (TruncationIndex != -1)
            {
                TruncationIndex = TruncationIndex + 1;
            }

            return TruncationIndex;
        }
    }
}
