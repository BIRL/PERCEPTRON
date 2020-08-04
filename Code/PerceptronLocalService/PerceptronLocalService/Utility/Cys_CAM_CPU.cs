using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class Cys_CAM_CPU
    {
        ModificationMWShift ModificationTableClass = new ModificationMWShift();

        public List<PostTranslationModificationsSiteDto> Cys_CAM(string proteinSequence, double Tolerance)
        {
            var modName = "Carboxyamidomethylation";
            double modWeight = ModificationTableClass.ModificationMWShiftTable(modName);
            var site = 'C';

            //SetsiteDetect(site);

            // variable score is initialized
            double score = 0;

            // Variable to store total number of Lysine
            var totalCys = 0;

            // list "array" creation
            var array = new List<PostTranslationModificationsSiteDto>();

            // for loop run for as many times as there are characters(Amino Acids) in the protein sequence
            for (var i = 0; i < proteinSequence.Length; i++)
            {
                if (proteinSequence[i] == 'C')
                {
                    var AminoAcid = new List<char>();
                    totalCys = totalCys + 1;
                    score = 1;
                    AminoAcid.Add(proteinSequence[i]);  /// no need

                    array.Add(new PostTranslationModificationsSiteDto(i, score, modWeight, modName, site, AminoAcid));
                }

            }
            return array;
        }
    }
}
