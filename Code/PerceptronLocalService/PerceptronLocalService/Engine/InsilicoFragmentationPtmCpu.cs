using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Engine
{
    public class InsilicoFragmentationPtmCpu //: IInsilicoFragmentation
         
    {
        public static void  insilico_fragmentation(ProteinDto prot, string clevageType, string ions)
        {
            
            Frag1(prot, clevageType, ions);
            
        }

        private static void Frag1(ProteinDto prot, string clevageType, string handleIon)
        {
            const double proton = 1.00727647;
            const double h = 1.007825035;
            const double c = 12.0000;
            const double o = 15.99491463;
            const double n = 14.003074;
            const double oh = o + h;
            const double co = c + o;
            const double nh = n + h;
            const double nh3 = n + h + h + h;
            const double h2O = h + h + o;
            var strprotein = prot.Sequence.ToUpper();
            var prtlength = strprotein.Length; //Gives length of Protein

            for (var seqIndex = 0; seqIndex < prtlength - 1; seqIndex++) //for Fragment
            {
                if (prot.PtmParticulars == null) continue;
                if (prot.PtmParticulars.Count == 0) continue;
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

            for (var seqIndex = 0; seqIndex < prtlength - 1; seqIndex++) //for Fragment
            {
                switch (clevageType)
                {
                    case "CID": //WHY CID in caps and in small letters(cid)???
                    case "cid":
                    case "bird":
                    case "BIRD":
                    case "imd":
                    case "IMD":
                    case "HCD":
                    case "hcd":
                    case "SID":
                    case "sid":
                        prot.InsilicoDetails.InsilicoMassLeft[seqIndex] = prot.InsilicoDetails.InsilicoMassLeft[seqIndex] + proton;
                        prot.InsilicoDetails.InsilicoMassRight[seqIndex] = prot.InsilicoDetails.InsilicoMassRight[seqIndex] + oh + 2 * h;
                        switch (handleIon)
                        {
                            case "bo":
                                prot.InsilicoDetails.InsilicoMassLeftBo.Add(prot.InsilicoDetails.InsilicoMassLeft[seqIndex] - h2O);
                                break;
                            case "bstar":
                                prot.InsilicoDetails.InsilicoMassLeftBstar.Add(prot.InsilicoDetails.InsilicoMassLeft[seqIndex] - nh3);
                                break;
                            case "ystar":
                                prot.InsilicoDetails.InsilicoMassRightYstar.Add(prot.InsilicoDetails.InsilicoMassRight[seqIndex] - nh3);
                                break;
                            case "yo":
                                prot.InsilicoDetails.InsilicoMassRightYo.Add(prot.InsilicoDetails.InsilicoMassRight[seqIndex] - h2O);
                                break;
                        }
                        break;

                    case "ECD":
                    case "ecd":
                    case "ETD":
                    case "etd":
                        prot.InsilicoDetails.InsilicoMassLeft[seqIndex] = prot.InsilicoDetails.InsilicoMassLeft[seqIndex] + proton + n + 3 * h;
                        prot.InsilicoDetails.InsilicoMassRight[seqIndex] = prot.InsilicoDetails.InsilicoMassRight[seqIndex] + oh - nh;
                        switch (handleIon)
                        {
                            case "zd":
                                prot.InsilicoDetails.InsilicoMassRightZo.Add(prot.InsilicoDetails.InsilicoMassRight[seqIndex] + proton);
                                break;
                            case "zdd":
                                prot.InsilicoDetails.InsilicoMassRightZoo.Add(prot.InsilicoDetails.InsilicoMassRight[seqIndex] + 2 * proton);
                                break;
                        }
                        break;

                    case "EDD":
                    case "edd":
                    case "NETD":
                    case "netd":
                        prot.InsilicoDetails.InsilicoMassLeft[seqIndex] = prot.InsilicoDetails.InsilicoMassLeft[seqIndex] + proton - co;
                        prot.InsilicoDetails.InsilicoMassRight[seqIndex] = prot.InsilicoDetails.InsilicoMassRight[seqIndex] + oh + co;
                        switch (handleIon)
                        {
                            case "ao":
                                prot.InsilicoDetails.InsilicoMassLeftAo.Add(prot.InsilicoDetails.InsilicoMassLeft[seqIndex] - h2O);
                                break;
                            case "astar":
                                prot.InsilicoDetails.InsilicoMassLeftAstar.Add(prot.InsilicoDetails.InsilicoMassLeft[seqIndex] - nh3);
                                break;
                        }
                        break;
                }
            }
        }


    }
}