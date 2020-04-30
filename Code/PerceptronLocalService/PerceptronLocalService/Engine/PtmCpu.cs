using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    class PtmCpu : IPostTranslationalModificationModule
    {
        private readonly IInsilicoFilter _insilicoFilter;      

        public PtmCpu()
        {
            _insilicoFilter = new InsilicoFilterCpu();         
        }

        private IEnumerable<int[]> Combinations(int m, int n) // nCr = nCm
        {
            var result = new int[m];
            var stack = new Stack<int>();
            stack.Push(0);

            while (stack.Count > 0)
            {
                var index = stack.Count - 1;
                var value = stack.Pop();

                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return result;
                    break;
                }
            }
        }
       
        private void EvaulateModificationSites(string protSequence,  List<PostTranslationModificationsSiteDto> filtered, List<int> ptmCode, double tol = 0)
        {
            // Runs only the PTMS that the user selected

            //List<Sites> filtered_sites = new List<Sites>();
            var dummy = new List<PostTranslationModificationsSiteDto>();

            foreach (var a in ptmCode)
            {
                switch (a)
                {
                    case 1:
                        dummy = Acetylation.Acetylation_A(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 2:
                        dummy = Acetylation.Acetylation_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 3:
                        dummy = Acetylation.Acetylation_M(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 4:
                        dummy = Acetylation.Acetylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 5:
                        dummy = Amidation.Amidation_F(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 6:
                        dummy = Hydroxylation.Hydroxylation_P(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 7:
                        dummy = Methylation.Methylation_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 8:
                        dummy = Methylation.Methylation_R(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 9:
                        dummy = Glycosylation.N_linked_glycosylation_N(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 10:
                        dummy = Glycosylation.O_linked_glycosylation_T(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 11:
                        dummy = Glycosylation.O_linked_glycosylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 12:
                        dummy = Phosphorylation.Phosphorylation_S(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 13:
                        dummy = Phosphorylation.Phosphorylation_T(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 14:
                        dummy = Phosphorylation.Phosphorylation_Y(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    case 15:
                        dummy = Ubiquitination.Ubiquitination_K(protSequence, tol);
                        //filter(dummy, filtered, tol);
                        foreach (var b in dummy)
                            filtered.Add(b);
                        break;
                    default:
                        // idle
                        break;
                }
            }
        }
      
        private void GenerateModifiedProteins(List<PostTranslationModificationsSiteDto> filteredSites, List<int> combos,
            ProteinDto parent, List<ProteinDto> shortProt, List<double> peakList, SearchParametersDto parameters)
        {

            var dummyMw = parent.Mw; // MW of the original sequence
            double dummyPtmScore = 0; // ptm score of the unmodified sequence
            
            var sitesInfo = new List<PostTranslationModificationsSiteDto>();

            foreach (var i in combos)
            {
                if (i != 777)
                {
                    dummyMw += filteredSites.ElementAt(i).ModWeight;
                    // accumulates the mod weight of all the sites in the current combination
                    dummyPtmScore += filteredSites.ElementAt(i).Score; //accumlates the ptm score
                    sitesInfo.Add(filteredSites.ElementAt(i));

                }
                else if (i == 777)
                {                 

                    var temProtein = new ProteinDto()
                    {
                        Mw = dummyMw,
                        PtmScore = dummyPtmScore,
                        Sequence = parent.Sequence,
                        Header = parent.Header,
                        PstScore = parent.PstScore,
                        InsilicoScore = parent.InsilicoScore,
                        MwScore = parent.MwScore,
                        Score = parent.Score,
                        InsilicoDetails = parent.InsilicoDetails,
                        PtmParticulars = sitesInfo
                    };

                    InsilicoFragmentationPtmCpu.insilico_fragmentation(temProtein, parameters.InsilicoFragType, parameters.HandleIons);
                              
                   // _insilicoFilter.ComputeInsilicoScore(new List<ProteinDto>{temProtein}, peakList, parameters.HopThreshhold);
                    //temProtein.set_score(parameters.MwSweight, parameters.PstSweight, parameters.InsilicoSweight);
                    shortProt.Add(temProtein);

                    dummyMw = parent.Mw;
                    dummyPtmScore = 0;
                    sitesInfo = new List<PostTranslationModificationsSiteDto>();
                }
            }
        }

        private double ComputeVariableModificationsScore(ProteinDto protein, List<ProteinDto> shortlisted, List<double> peakList, SearchParametersDto parameters)
        {          
            var filteredSitesA = new List<PostTranslationModificationsSiteDto>();
            EvaulateModificationSites(protein.Sequence, filteredSitesA, parameters.PtmCodeVar, parameters.PtmTolerance); // filtered sites is currently empty
            var sortedList = filteredSitesA.OrderBy(o => o.Score).ToList();
                       
            var combos = new List<string>();
            var siteIndices = new List<int>();
            for (var j = 1; j <= Constants.MaximumVariableModifications; j++) 
            {
                foreach (var comb in Combinations(j, sortedList.Count))
                {
                    var dummy = string.Empty;
                    foreach (var t in comb)
                    {
                        dummy += t.ToString();
                        siteIndices.Add(t); // separates indices
                    }
                    combos.Add(dummy);
                    siteIndices.Add(777);
                }
            }

            if (sortedList.Count > 0)
                GenerateModifiedProteins(filteredSitesA, siteIndices, protein, shortlisted, peakList, parameters);

            return 0;
        }

        private double ComputeFixedModificationScore(ProteinDto parentPro, List<ProteinDto> modifiedProteins, List<double> peakList, SearchParametersDto parameters)
        {           
            var filteredSites = new List<PostTranslationModificationsSiteDto>();
            EvaulateModificationSites(parentPro.Sequence, filteredSites, parameters.PtmCodeFix);
            filteredSites = filteredSites.OrderBy(o => o.Score).ToList();

            var siteIndices = Enumerable.Range(0, filteredSites.Count).ToList();
            siteIndices.Add(777);

            if (filteredSites.Count > 0)
                GenerateModifiedProteins(filteredSites, siteIndices, parentPro, modifiedProteins, peakList, parameters);
            return 0;
        }

        public List<ProteinDto> ExecutePtmModule(List<ProteinDto> proteinList, MsPeaksDto peakData, SearchParametersDto parameters)
        {
            var modifiedProteins = new List<ProteinDto>();
            if (parameters.PtmCodeVar.Count != 0)
            {
                foreach (var protein in proteinList)
                {
                    protein.PtmScore = ComputeVariableModificationsScore(protein, modifiedProteins, peakData.Mass, parameters);

                }
            }
            if (parameters.PtmCodeFix.Count != 0)
            {
                foreach (var protein in proteinList)
                {
                    protein.PtmScore = ComputeFixedModificationScore(protein, modifiedProteins, peakData.Mass, parameters);
                }
            }

            proteinList.AddRange(modifiedProteins);
            return proteinList;
        }
    }
}
