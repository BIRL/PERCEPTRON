using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class MSONE_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();

        public List<PostTranslationModificationsSiteDto> MSONE(string proteinSequence, double ptmTolerance)
        {
            var modName = "Sulfone";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'M';

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalMeth = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // stores the amino acids found
            var sub_sequence = new List<char>();

            // for loop run for as many times as there are characters in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'M')
                {
                    totalMeth = totalMeth + 1;
                    score = 1;
                    sub_sequence.Add(proteinSequence[i]);
                }
                array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, sub_sequence));
            }
            return array;
        }
    }
}
