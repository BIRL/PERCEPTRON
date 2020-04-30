using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{
    

    public class MwModule : IMolecularWeightModule
    {
        public void FilterModifiedProteinsByWholeProteinMass(SearchParametersDto parameters, List<ProteinDto> shortlistedProteins,
           MsPeaksDto peakData)
        {
            if (parameters.HasFixedAndVariableModifications())
            {
                if (shortlistedProteins.Count > 1)
                {
                    for (var p = 1; p < shortlistedProteins.Count; p++)
                    {
                        double buffer = MW_filter(peakData.WholeProteinMolecularWeight,
                            parameters.MwTolerance,
                            shortlistedProteins.ElementAt(p).Mw, false);
                        if ((int)buffer == Constants.OutofBoundModifiedMolecularWeight)
                            shortlistedProteins.Remove(shortlistedProteins.ElementAt(p));
                        else
                            shortlistedProteins.ElementAt(p).MwScore = buffer;
                    }
                }
            }
        }

        private double MW_filter(double mw, double tol, double mwExperimental, bool fasta)
        {
            var error = Math.Abs(mwExperimental - mw);      /*!< error calculates the difference b/w theoretical and experimental mw. */
            var errorScore = 1 / Math.Pow(2, error);

            if (fasta)                                  /*!< check condition of fasta variable */
            {
                return errorScore;                             /*!< If MW_filter was called for first time return error score */
            }
            if (error < tol)                                /*!< if error is within user defined tolerance */
                return errorScore;                         /*!< return error score */
            return Constants.OutofBoundModifiedMolecularWeight;                                  /*!< otherwise return -7 to indicate error */
        }
    }
}
