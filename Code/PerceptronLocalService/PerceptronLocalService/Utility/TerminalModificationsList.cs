using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Diagnostics;

namespace PerceptronLocalService.Utility
{
    /* Added 20201207  -- For Time Efficiancy  */
    public class TerminalModificationsList
    {
        public List<string> TerminalModifications(string TerminalModifications)
        {

            Stopwatch t = new Stopwatch();
            t.Start();
            var IndividualModifications = TerminalModifications.Split(',');
            var TerminalModificationsList = new List<string>() { "", "", "", ""};
            
            if (IndividualModifications.Contains("None"))
            {
                TerminalModificationsList[0] = "None";
            }

            if (IndividualModifications.Contains("NME"))
            {
                TerminalModificationsList[1] = "NME";
            }

            if (IndividualModifications.Contains("NME_Acetylation"))
            {
                TerminalModificationsList[2] = "NME_Acetylation";
            }

            if (IndividualModifications.Contains("M_Acetylation"))
            {
                TerminalModificationsList[3] = "M_Acetylation";
            }

            t.Stop();
            return TerminalModificationsList;
        }
    }
}
