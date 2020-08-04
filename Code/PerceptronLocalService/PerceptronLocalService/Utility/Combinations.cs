using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    class Combinations
    {


        public List<List<List<int>>> GetAllCombination(int numberofelements)
        {
            List<int> listofnumbers = new List<int>(); // Generating list, range of 0 to numberofelements
            for (int indexlist = 0; indexlist <= numberofelements - 1; indexlist++)
            {
                listofnumbers.Add(indexlist);
            }

            var consecutivenumbers = new List<List<int>>();

            double count = Math.Pow(2, listofnumbers.Count);
            for (int i = 1; i <= count - 1; i++) // This loop is making all statistics' Combinations of list elements but just storing that numbers which are consecutives
            {
                string int2binary = Convert.ToString(i, 2).PadLeft(listofnumbers.Count, '0');
                var subresults = new List<int>();
                for (int j = 0; j < int2binary.Length; j++)
                {
                    if (int2binary[j] == '1')
                    {
                        subresults.Add(listofnumbers[j]);
                    }
                }
                //var temporarylist = subresults;
                //bool isConsecutive = !temporarylist.Select((z, j) => z - j).Distinct().Skip(1).Any();//Checking for consecutive numbers
                //if (isConsecutive == true)
                consecutivenumbers.Add(subresults); //Just collecting that numbers which are consecutives
            }

            //Combinning lists of same count into CombinedConsecutiveNumList
            var CombinedConsecutiveNumList = new List<List<List<int>>>();

            int distinctnumber = (from x in consecutivenumbers select x.Count).Distinct().Count();  //Check distinct number of counts into the list (consecutivenumbers)
            for (int i = 0; i < distinctnumber; i++)  // Looping on the distinct number of counts
            {
                var templist = (from x in consecutivenumbers where x.Count == i + 1 select x).ToList();//Grouping the sublists having same number of counts
                CombinedConsecutiveNumList.Insert(i, templist);
            }
            return CombinedConsecutiveNumList;
        }
    }
}
