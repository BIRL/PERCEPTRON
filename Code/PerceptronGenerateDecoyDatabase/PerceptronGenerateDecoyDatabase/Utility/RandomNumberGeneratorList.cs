using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronGenerateDecoyDatabase.Utility
{
    public class RandomNumberGeneratorList
    {
        public void RandomNoGenerate(int min, int max, int AminoAcidAlphbetsLength)
        {
            Random rand = new Random();
            int RandomNumer;
            var RandomNoList = new List<decimal>(max+1);
            var testRandomNoList = new List<decimal>(max + 1);

            for (int iter = 0; iter < max; iter++)
            {
                //Ref SPECTRUM: Not Multiplying and Ceiling as rand.Next will give integer values
                //From SPECTRUM - AAs( ceil(rand(1,sLength)*numRands));
                var asdf = rand.Next(min, max);
                RandomNoList.Add(((rand.Next(min, max))));
                decimal num = ((asdf / max))*AminoAcidAlphbetsLength;
                testRandomNoList.Add(num);
            }
        }

       
    }
}
