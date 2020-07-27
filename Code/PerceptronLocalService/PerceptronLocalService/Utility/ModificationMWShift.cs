using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronLocalService.Utility
{
    public class ModificationMWShift
    {

        public double ModificationMWShiftTable(string Mod) //Gives the weight of different types of modifications
        {
            double ModWeight = 0.0;

            switch (Mod)
            {
                case "Propionamidation":
                case "Cys_PAM":
                    return ModWeight = 174.04631;
                case "Pyruvate-S":
                    return ModWeight = -17.0265;
                case "Pyruvate-C":
                    return ModWeight = 70.0055;
                case "Amidation":         //Site = 'F'
                    return ModWeight = -0.984016;
                case "Citrullination":
                    return ModWeight = 0.984016;
                case "Methylation":           //Site = 'R' || 'K'
                    return ModWeight = 14.0156;
                case "Hydroxylation":         //Site = 'P'
                    return ModWeight = 15.9949;
                case "Sulfoxide":
                case "MSO":                   //Site = 'M'
                    return ModWeight = 15.9949;
                case "Formylation":
                    return ModWeight = 27.9949;
                case "DiMethylation":
                    return ModWeight = 28.0313;
                case "S-Nitrosylation":
                    return ModWeight = 28.9902;
                case "Sulfone":
                    return ModWeight = 31.9898;
                case "DiHydroxylation":
                    return ModWeight = 31.9898;
                case "TriMethylation":
                    return ModWeight = 42.047;
                case "Acetylation":       //Site = 'S' || 'M' || 'K'  || 'A'
                    return ModWeight = 42.0106;
                case "Gamma-Carboxyglutamic-Acid":
                    return ModWeight = 43.9898;
                case "Nitration":
                    return ModWeight = 44.9851;
                case "Phosphorylation":       //Site = 'Y'
                    return ModWeight = 79.9663;
                case "Pyrrolidone-Aarboxylic-Acid":
                    return ModWeight = -17.0265;
                case "O-linked-Glycosylation":    //Site = 'S'
                    return ModWeight = 203.0794;
                case "Palmitoylation":        //Site = 'Y'
                    return ModWeight = 238.23;
                case "Glutathionylation":         //Site = 'C'
                    return ModWeight = 305.068;
                case "N-linked-Glycosylation":
                    return ModWeight = 317.122;
                case "Carboxymethylation":        //In SPECTRUM its name is Cys_CAM                     //Site = 'C'
                case "Cys_CAM":
                    return ModWeight = 160.03065;
                case "Pyridylethylation":         //Site = 'C'
                    return ModWeight = 208.067039;
                default:
                    return ModWeight;
            }
        }
    }
}
