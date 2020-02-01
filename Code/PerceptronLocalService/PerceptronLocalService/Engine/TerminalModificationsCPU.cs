using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.Engine;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    public class TerminalModificationsCPU : ITerminalModifications
    {

        public List<ProteinDto> EachProteinTerminalModifications(SearchParametersDto parameters, List<ProteinDto> candidateProteins)
        {
            
            
            var tempCandidateProteins = new List<ProteinDto>();
            for (int index = 0; index < candidateProteins.Count; index++)
            {

                //Preparing Protein Info
                var protein = candidateProteins[index];
                var tempprotein = new ProteinDto(protein);

                //BELOW: Just for Safe Level
                var leftString = Clone.CloneObject(tempprotein.InsilicoDetails.InsilicoMassLeft);
                var leftIons = Clone.Decrypt<List<double>>(leftString);

                var seqString = Clone.CloneObject(tempprotein.Sequence);
                var sequence = Clone.Decrypt<string>(seqString);

                var rightString = Clone.CloneObject(tempprotein.InsilicoDetails.InsilicoMassRight);
                var rightIons = Clone.Decrypt<List<double>>(rightString);
                //ABOVE: Just for Safe Level

                double molW = tempprotein.InsilicoDetails.InsilicoMassLeft.Count - 1;
                int tmpSeqLength = sequence.Length;

                TerminalModifications(molW, leftIons, rightIons, sequence, tmpSeqLength, parameters, protein, tempCandidateProteins);
            }

            candidateProteins = tempCandidateProteins;
            return candidateProteins;
        }


        public void TerminalModifications(double molW, List<double> leftIons, List<double> rightIons, string tempseq, int tmpSeqLength, SearchParametersDto parameters, ProteinDto tempprotein, List<ProteinDto> tempCandidateProteins)
        {
            double AcetylationWeight = MassAdjustment.AcetylationWeight;
            double MethionineWeight = 131.04049; //RECEIVING so for the #TIMEBEING//= AminoAcidInfo.AminoAcidMasses.TryGetValue('M', out MethionineWeight) ? MethionineWeight : MethionineWeight;

            string[] IndividualModifications = parameters.TerminalModification.Split(',');

            var DELMECandiList = new List<ProteinDto> ();


            if (IndividualModifications[0] == "None")
            {
                var newProtein = new ProteinDto(tempprotein);
                newProtein.TerminalModification = "None";
                newProtein.Mw = molW;
                newProtein.InsilicoDetails.InsilicoMassLeft = leftIons;
                newProtein.InsilicoDetails.InsilicoMassRight = rightIons;
                tempCandidateProteins.Add(newProtein);
            }

            if (tempseq[0] == 'M')
            {
                if (IndividualModifications[0] == "NME" || IndividualModifications[1] == "NME") //Its Seems Like hard Code but not in Actual because We know the position of NME will always be either 0 or 1  //// Just checking NME with this method so to avoid conflict between NME and NME_Acetylation
                {
                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "NME",
                        Mw = molW - MethionineWeight,
                        InsilicoDetails =
                        {
                            InsilicoMassLeft = leftIons.Select(x => x - MethionineWeight).ToList(),
                            InsilicoMassRight = rightIons.ToList()
                        },
                        Sequence = tempseq.Substring(1, tmpSeqLength - 1) // "-1" Added
                    };
                    tempCandidateProteins.Add(newProtein);
                }

                if (parameters.TerminalModification.Contains("NME_Acetylation"))
                {

                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "NME_Acetylation",
                        Mw = molW - MethionineWeight + AcetylationWeight,
                        InsilicoDetails =
                        {
                            InsilicoMassLeft =
                                leftIons.Select(x => x - MethionineWeight + AcetylationWeight).ToList(),
                            InsilicoMassRight = rightIons.ToList()
                        },
                        Sequence = tempseq.Substring(1, tmpSeqLength - 1) // "-1" Added
                    };
                    tempCandidateProteins.Add(newProtein);
                }
                if (parameters.TerminalModification.Contains("M_Acetylation"))
                {
                    var newProtein = new ProteinDto(tempprotein)
                    {
                        TerminalModification = "M_Acetylation",
                        Sequence = tempseq,
                        Mw = molW + AcetylationWeight,
                        InsilicoDetails =
                        {
                            InsilicoMassLeft = leftIons.Select(x => x + AcetylationWeight).ToList(),
                            InsilicoMassRight = rightIons.ToList()
                        }
                    };
                    tempCandidateProteins.Add(newProtein);
                }
            }



            

        }
    }
}
