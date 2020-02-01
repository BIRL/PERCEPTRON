using System.Collections.Generic;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{


    public class InsilicoFragmentsAdjustmentCpu : IInsilicoFragmentsAdjustment
    {
        public void adjustForFragmentTypeAndSpecialIons(List<ProteinDto> prot, string clevageType, string ions)
        {
            //int del = 0;//DELME
            //int a;//DELME
            
            for (int protIndex = 0; protIndex < prot.Count; protIndex++)
            {


                //string headerprot = protein.Header;// = "Q5T292";//DELME
                //if (prot[protIndex].Header == "A0A0B4J280")  //"Q3ZAQ7"//DELME
                //    a = 1;//DELME


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

                //for (var seqIndex = 0; seqIndex < prtlength - 1; seqIndex++) //for Fragment
                //{
                switch (clevageType.ToUpper())
                {
                    case CleavageTypes.CID:
                    case CleavageTypes.BIRD:
                    case CleavageTypes.IMD:
                    case CleavageTypes.HCD:
                    case CleavageTypes.SID:

                        if (HandleIonIteration == 0)
                        {
                            MakeAdjustment(prot, MassAdjustment.OH + 2 * MassAdjustment.H, MassAdjustment.Proton);
                        }


                        switch (IndividualHandleIon)//(handleIon)   //IndividualHandleIon
                        {
                            case SpecialIons.BO:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassLeft, prot.InsilicoDetails.InsilicoMassLeftBo, -MassAdjustment.H2O);
                                break;
                            case SpecialIons.BSTAR:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassLeft, prot.InsilicoDetails.InsilicoMassLeftBstar, -MassAdjustment.NH3);
                                break;
                            case SpecialIons.YSTAR:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassRight, prot.InsilicoDetails.InsilicoMassRightYstar, -MassAdjustment.NH3);
                                break;
                            case SpecialIons.YO:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassRight, prot.InsilicoDetails.InsilicoMassRightYo, -MassAdjustment.H2O);
                                break;
                        }


                        break;

                    case CleavageTypes.ECD:
                    case CleavageTypes.ETD:

                        if (HandleIonIteration == 0)
                        {
                            MakeAdjustment(prot, MassAdjustment.OH - MassAdjustment.NH, MassAdjustment.Proton + MassAdjustment.N + 3 * MassAdjustment.H);
                        }

                        switch (IndividualHandleIon)//(handleIon)
                        {
                            case SpecialIons.ZD:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassRight, prot.InsilicoDetails.InsilicoMassRightZo, MassAdjustment.Proton);

                                break;
                            case SpecialIons.ZDD:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassRight, prot.InsilicoDetails.InsilicoMassRightZoo, 2 * MassAdjustment.Proton);    //HERE IS Zoo

                                break;
                        }
                        break;

                    case CleavageTypes.EDD:

                    case CleavageTypes.NETD:

                        if (HandleIonIteration == 0)
                        {
                            MakeAdjustment(prot, MassAdjustment.OH + MassAdjustment.CO, MassAdjustment.Proton - MassAdjustment.CO);
                        }


                        switch (IndividualHandleIon)//(handleIon)
                        {
                            case SpecialIons.AO:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassLeft, prot.InsilicoDetails.InsilicoMassLeftAo, -MassAdjustment.H2O);

                                break;
                            case SpecialIons.ASTAR:

                                MakeAdjustmentInSpecialIons(prot.InsilicoDetails.InsilicoMassLeft, prot.InsilicoDetails.InsilicoMassLeftAstar, -MassAdjustment.NH3);
                                break;
                        }
                        break;
                }
                //}
            }


        }
        //STEP 2: Generate theoretical fragments of each protein 
        private static void MakeAdjustment(ProteinDto prot, double rightOffset, double leftOffset)  // specialDependecy == prot.InsilicoDetails.InsilicoMassLeft  OR specialDependecy == prot.InsilicoDetails.InsilicoMassRight
        {
            ////#FORTHETIMEBEING: Updated 20200115 COMMENTED: PREVIOUSLY Removing Last Entry(MW of Protein - Water). So, now Just For Fragments Now Added: -1
            ////#Update: Updated 20200202:  "-1" is Removed and now its just ".Count"
            for (int index = 0; index < prot.InsilicoDetails.InsilicoMassLeft.Count; index++)  
            {
                prot.InsilicoDetails.InsilicoMassLeft[index] = prot.InsilicoDetails.InsilicoMassLeft[index] + leftOffset;
                prot.InsilicoDetails.InsilicoMassRight[index] = prot.InsilicoDetails.InsilicoMassRight[index] + rightOffset;

            }
        }

        private static void MakeAdjustmentInSpecialIons(List<double> MainIon, List<double> SpecialFragmentIonList, double Offset)
        {
            ////#FORTHETIMEBEING: Updated 20200115 COMMENTED: PREVIOUSLY Removing Last Entry(MW of Protein - Water). So, now Just For Fragments Now Added: -1
            ////#Update: Updated 20200202:  "-1" is Removed and now its just ".Count"
            for (int iter = 0; iter < MainIon.Count; iter++)
            {
                SpecialFragmentIonList.Add(MainIon[iter] + Offset);
            }

        }

        //private static void MakeAdjustmentInSpecialIons(ProteinDto prot, List<double> MainIon, List<double> SpecialFragmentIonList, double Offset)
        //{
        //    for (int iter = 0; iter < MainIon.Count; iter++)
        //    {
        //        SpecialFragmentIonList.Add(MainIon[iter] + Offset);
        //    }

        //}
    }
}


//for (var seqIndex = 0; seqIndex < prot.Sequence.Length - 1; seqIndex++) //for Fragment
//{
//    prot.InsilicoDetails.InsilicoMassLeft[seqIndex] = prot.InsilicoDetails.InsilicoMassLeft[seqIndex] + leftOffset;                                                                 // ;
//    prot.InsilicoDetails.InsilicoMassRight[seqIndex] = prot.InsilicoDetails.InsilicoMassRight[seqIndex] + rightOffset;


//    SpecialFragmentIonList.Add(specialDependecy[seqIndex] + s);
//}
