using System.Collections.Generic;

namespace PerceptronLocalService.Utility
{
    public class RemoveMass
    {
        /*    For Time Efficiency --- Alternate of   ".Select" a C# built-in function     Updated 20201201     */
        public List<double> MassRemoval(List<double> IonsList, double RemoveMass)
        {
            int IonsListCount = IonsList.Count;
            List<double> MassRemovedIonsList = new List<double>(IonsListCount);

            for (int iter = 0; iter < IonsListCount; iter++)
            {
                MassRemovedIonsList.Add(IonsList[iter] - RemoveMass);
            }
            return MassRemovedIonsList;
        }
    }
}
