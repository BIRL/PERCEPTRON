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
                case "Propionamidation":    //#UP
                case "Cys_PAM":
                    return ModWeight = 174.04631;
                case "Pyruvate-S":
                    return ModWeight = -17.0265;
                case "Pyruvate-C":
                    return ModWeight = 70.0055;
                case "Amidation":         //Site = 'F'    //#UP
                    return ModWeight = -0.9840;
                case "Citrullination":
                    return ModWeight = 0.984016;
                case "Methylation":           //Site = 'R' || 'K'    //#UP
                    return ModWeight = 14.0157;
                case "Hydroxylation":         //Site = 'P'    //#UP
                    return ModWeight = 15.9949;
                case "Sulfoxide":
                    return ModWeight = 15.9949;    //Updated 20210105 Bug Fix
                case "MSO":                   //Site = 'M'    //#UP
                    return ModWeight = 147.0354;
                case "Formylation":
                    return ModWeight = 27.9949;
                case "DiMethylation":
                    return ModWeight = 28.0313;
                case "S-Nitrosylation":
                    return ModWeight = 28.9902;
                case "Sulfone":                         //is it MSONE    //#UP
                case "MSONE":
                    return ModWeight = 31.9898;
                case "DiHydroxylation":
                    return ModWeight = 31.9898;
                case "TriMethylation":
                    return ModWeight = 42.047;
                case "Acetylation":       //Site = 'S' || 'M' || 'K'  || 'A'     //#UP
                    return ModWeight = 42.0106;
                case "Gamma-Carboxyglutamic-Acid":
                    return ModWeight = 43.9898;
                case "Nitration":
                    return ModWeight = 44.9851;
                case "Phosphorylation":       //Site = 'Y'    //#UP
                    return ModWeight = 79.9663;
                case "Pyrrolidone-Aarboxylic-Acid":
                    return ModWeight = -17.0265;
                case "O-linked-Glycosylation":  //O_Linked_Glycosylation  //Site = 'S'     //#UP
                    return ModWeight = 203.0794;
                case "Palmitoylation":        //Site = 'Y'
                    return ModWeight = 238.23;
                case "Glutathionylation":         //Site = 'C'
                    return ModWeight = 305.068;
                case "N-linked-Glycosylation":// N_Linked_Glycosylation      //#UP
                    return ModWeight = 317.122;
                case "Carboxyamidomethylation":
                case "Cys_CAM":   //Carboxyamidomethylation //#UP
                    return ModWeight = 160.03065;
                case "Carboxymethylation":
                case "Cys_CM":                               //#UP
                    return ModWeight = 161.01466;
                case "Pyridylethylation":         //Site = 'C'
                case "Cys_PE":                               //#UP
                    return ModWeight = 208.067039;
                default:
                    return ModWeight;
            }
        }
    }
}





