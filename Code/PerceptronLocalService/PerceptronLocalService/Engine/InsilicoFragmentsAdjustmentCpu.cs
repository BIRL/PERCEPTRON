using System.Collections.Generic;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{


    public class InsilicoFragmentsAdjustmentCpu  : IInsilicoFragmentsAdjustment     
    {
        public void adjustForFragmentTypeAndSpecialIons(List<ProteinDto> prot, string clevageType, string ions)
        {
            int del = 0;
            //foreach (var protein in prot)
            //{
            for (int protIndex = 0; protIndex < prot.Count; protIndex++)
            {

               
                //string headerprot = protein.Header;// = "Q5T292";


                adjustProteinForFragmentTypeAndSpecialIons(prot[protIndex], clevageType, ions);
                //string headerprot2 = protein.Header;
            }
        }

        private void adjustProteinForFragmentTypeAndSpecialIons(ProteinDto prot, string clevageType, string handleIon)
        {
            
            var strprotein = prot.Sequence.ToUpper();
            var prtlength = strprotein.Length; //Gives length of Protein


            if (prot.PtmParticulars != null && prot.PtmParticulars.Count != 0)
            {
                for (var seqIndex = 0; seqIndex < prtlength - 1; seqIndex++) //for Fragment
                {

                    foreach (var ptmSite in prot.PtmParticulars)
                    {
                        if (seqIndex >= ptmSite.Index)
                            prot.InsilicoDetails.InsilicoMassLeft[seqIndex] =
                                prot.InsilicoDetails.InsilicoMassLeft[seqIndex] + ptmSite.ModWeight;
                        else
                            prot.InsilicoDetails.InsilicoMassRight[seqIndex] =
                                prot.InsilicoDetails.InsilicoMassRight[seqIndex] + ptmSite.ModWeight;
                    }
                }
            }
            
            string[] IndividualHandleIonArray = handleIon.Split(',');  // We got string of handle ions with comma separated. So, converting it into (string array) with separated ion

            for (int HandleIonIteration = 0; HandleIonIteration < IndividualHandleIonArray.Length; HandleIonIteration++)
            {
                string IndividualHandleIon = IndividualHandleIonArray[HandleIonIteration];

                for (var seqIndex = 0; seqIndex < prtlength - 1; seqIndex++) //for Fragment
                {
                    switch (clevageType.ToUpper())
                    {
                        case CleavageTypes.CID:
                        case CleavageTypes.BIRD:
                        case CleavageTypes.IMD:
                        case CleavageTypes.HCD:
                        case CleavageTypes.SID:



                            switch (IndividualHandleIon)//(handleIon)
                            {
                                case SpecialIons.BO:
                                    MakeAdjustment(prot, MassAdjustment.OH + 2 * MassAdjustment.H, MassAdjustment.Proton, prot.InsilicoDetails.InsilicoMassLeftBo, -MassAdjustment.H2O, prot.InsilicoDetails.InsilicoMassLeft);
                                    break;
                                case SpecialIons.BSTAR:
                                    MakeAdjustment(prot, MassAdjustment.OH + 2 * MassAdjustment.H, MassAdjustment.Proton, prot.InsilicoDetails.InsilicoMassLeftBstar, -MassAdjustment.NH3, prot.InsilicoDetails.InsilicoMassLeft);
                                    break;
                                case SpecialIons.YSTAR:
                                    MakeAdjustment(prot, MassAdjustment.OH + 2 * MassAdjustment.H, MassAdjustment.Proton, prot.InsilicoDetails.InsilicoMassRightYstar, -MassAdjustment.NH3, prot.InsilicoDetails.InsilicoMassRight);
                                    break;
                                case SpecialIons.YO:
                                    MakeAdjustment(prot, MassAdjustment.OH + 2 * MassAdjustment.H, MassAdjustment.Proton, prot.InsilicoDetails.InsilicoMassRightYo, -MassAdjustment.H2O, prot.InsilicoDetails.InsilicoMassRight);
                                    break;
                            }


                            break;

                        case CleavageTypes.ECD:

                        case CleavageTypes.ETD:

                            switch (IndividualHandleIon)//(handleIon)
                            {
                                case SpecialIons.ZD:
                                    MakeAdjustment(prot, MassAdjustment.OH - MassAdjustment.NH, MassAdjustment.Proton + MassAdjustment.N + 3 * MassAdjustment.H, prot.InsilicoDetails.InsilicoMassRightZo, MassAdjustment.Proton, prot.InsilicoDetails.InsilicoMassRight);
                                    break;
                                case SpecialIons.ZDD:
                                    MakeAdjustment(prot, MassAdjustment.OH - MassAdjustment.NH, MassAdjustment.Proton + MassAdjustment.N + 3 * MassAdjustment.H, prot.InsilicoDetails.InsilicoMassRightYo, 2 * MassAdjustment.Proton, prot.InsilicoDetails.InsilicoMassRight);
                                    break;
                            }
                            break;

                        case CleavageTypes.EDD:

                        case CleavageTypes.NETD:


                            switch (IndividualHandleIon)//(handleIon)
                            {
                                case SpecialIons.AO:
                                    MakeAdjustment(prot, MassAdjustment.OH + MassAdjustment.CO, MassAdjustment.Proton - MassAdjustment.CO, prot.InsilicoDetails.InsilicoMassLeftAo, -MassAdjustment.H2O, prot.InsilicoDetails.InsilicoMassLeft);
                                    break;
                                case SpecialIons.ASTAR:
                                    MakeAdjustment(prot, MassAdjustment.OH + MassAdjustment.CO, MassAdjustment.Proton - MassAdjustment.CO, prot.InsilicoDetails.InsilicoMassLeftAstar, -MassAdjustment.NH3, prot.InsilicoDetails.InsilicoMassLeft);
                                    break;
                            }
                            break;
                    }
                }
            }

            
        }
        //STEP 2: Generate theoretical fragments of each protein 
        private static void MakeAdjustment(ProteinDto prot, double rightOffset, double leftOffset, List<double> specialList , double s, List<double> specialDependecy )
        {
            for (var seqIndex = 0; seqIndex < prot.Sequence.Length - 1; seqIndex++) //for Fragment
            {
                prot.InsilicoDetails.InsilicoMassLeft[seqIndex] = prot.InsilicoDetails.InsilicoMassLeft[seqIndex] + leftOffset;                                                                 // ;
                prot.InsilicoDetails.InsilicoMassRight[seqIndex] = prot.InsilicoDetails.InsilicoMassRight[seqIndex] + rightOffset;


                specialList.Add(specialDependecy[seqIndex] + s);
            }
        }
    }
}
