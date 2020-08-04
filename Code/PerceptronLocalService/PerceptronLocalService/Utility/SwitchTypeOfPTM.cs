using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Utility
{
    class SwitchTypeOfPTM
    {
        Acetylation_CPU _Acetylation = new Acetylation_CPU();
        Amidation_CPU _Amidation = new Amidation_CPU();
        Hydroxylation_CPU _Hydroxylation = new Hydroxylation_CPU();
        Methylation_CPU _Methylation = new Methylation_CPU();
        Glycosylation_CPU _Glycosylation = new Glycosylation_CPU();
        Phosphorylation_CPU _Phosphorylation = new Phosphorylation_CPU();

        public List<PostTranslationModificationsSiteDto> SwitchToTypeOfPTM(string TypeOfModification, string ProteinSequence, double Tolerance)
        {
            //For the time being its here. Otherwise it should moved to the Utility for the Common Use (PTM CPU & PTM GPU).


            var ModificationSite = new List<PostTranslationModificationsSiteDto>();
            switch (TypeOfModification)
            {
                case "Acetylation_A":
                    ModificationSite = _Acetylation.Acetylation_A(ProteinSequence, Tolerance);
                    break;
                case "Acetylation_K":
                    ModificationSite = _Acetylation.Acetylation_K(ProteinSequence, Tolerance);
                    break;
                case "Acetylation_M":
                    ModificationSite = _Acetylation.Acetylation_M(ProteinSequence, Tolerance);
                    break;
                case "Acetylation_S":
                    ModificationSite = _Acetylation.Acetylation_S(ProteinSequence, Tolerance);
                    break;
                case "Amidation_F":
                    ModificationSite = _Amidation.Amidation_F(ProteinSequence, Tolerance);
                    break;
                case "Hydroxylation_P":
                    ModificationSite = _Hydroxylation.Hydroxylation_P(ProteinSequence, Tolerance);
                    break;
                case "Methylation_K":
                    ModificationSite = _Methylation.Methylation_K(ProteinSequence, Tolerance);
                    break;
                case "Methylation_R":
                    ModificationSite = _Methylation.Methylation_R(ProteinSequence, Tolerance);
                    break;
                case "N_Linked_Glycosylation_N":
                    ModificationSite = _Glycosylation.N_Linked_Glycosylation_N(ProteinSequence, Tolerance);
                    break;
                case "O_Linked_Glycosylation_T":
                    ModificationSite = _Glycosylation.O_Linked_Glycosylation_T(ProteinSequence, Tolerance);
                    break;
                case "O_Linked_Glycosylation_S":
                    ModificationSite = _Glycosylation.O_Linked_Glycosylation_S(ProteinSequence, Tolerance);
                    break;
                case "Phosphorylation_S":
                    ModificationSite = _Phosphorylation.Phosphorylation_S(ProteinSequence, Tolerance);
                    break;
                case "Phosphorylation_T":
                    ModificationSite = _Phosphorylation.Phosphorylation_T(ProteinSequence, Tolerance);
                    break;
                case "Phosphorylation_Y":
                    ModificationSite = _Phosphorylation.Phosphorylation_Y(ProteinSequence, Tolerance);
                    break;
            }
            return ModificationSite;
        }
    }
}
