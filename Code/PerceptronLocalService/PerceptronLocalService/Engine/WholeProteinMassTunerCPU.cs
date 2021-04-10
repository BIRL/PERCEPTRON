using System;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using System.Collections.Generic;  //for list
using System.Linq; //for OrderBy

namespace PerceptronLocalService.Engine
{

    //Tuple Sums: Sums of MS1 and MS2s with each other
    public class WholeProteinMassTunerCpu : IWholeProteinMassTuner
    {

        public double TuneWholeProteinMass(List<newMsPeaksDto> peakData2DList, SearchParametersDto parameters)
        {
            double TunnedMass = 0.0;
            double molTolerance = parameters.MwTolerance;
            //////Making a 2D list(peakDatalist) in which Mass & Intensity includes 
            ////var peakDatalist = new List<peakData2Dlist>();
            ////for (int row = 1; row <= peakData.Mass.Count - 1; row++)   //Updated 20210408
            ////{
            ////    var dataforpeakDatalist = new peakData2Dlist(peakData.Mass[row], peakData.Intensity[row]);
            ////    peakDatalist.Add(dataforpeakDatalist);

            ////}
            //////Sort the peakDatalist with respect to the Mass in ascending order
            ////var peakDatalistsort = peakDatalist.OrderBy(n => n.Mass).ToList();

            ////peakDatalistsort.Add(new peakData2Dlist(peakData.Mass[0], peakData.Intensity[0]));   //Added 20210408

            var peakDatalistsort = peakData2DList;
            double IntactProteinMass = peakData2DList[peakData2DList.Count - 1].Mass;

            //FIGURE 5: STEP 2:::: SUMMATIONS FOR EACH M/Z I,J IN MS2 DATA [REF: SPCTRUM PAPER]
            //Description::: Calculate Peak Sums and shortlist those falling within user specified tolerance.

            var summationMassandaverageintensity = new List<newMsPeaksDto>();
            for (int i = 0; i <= peakDatalistsort.Count - 2; i++) //i starts from 0 so that's why "peakDatalistsort.Count - 1" and according to Formula "n-1" gives peakDatalistsort.Count - 2
            {
                for (int j = i + 1; j <= peakDatalistsort.Count - 1; j++)//j starts from 0 so that's why peakDatalistsort.Count - 1 and according to Formula just "n"
                {
                    double cpusummationMassData = peakDatalistsort[i].Mass + peakDatalistsort[j].Mass + parameters.NeutralLoss; //Making Tuple sums of MS2 masses + Neutral Mass Loss if user know the Loss of Mass during experimentation // 20200121 - - NeutralLoss Added 
                    if (IntactProteinMass - molTolerance <= cpusummationMassData && cpusummationMassData <= IntactProteinMass + molTolerance)//Masses & Intensities filter out due to the selected RANGE OF INTACT MASS +/- PROTEIN MASS TOLERANCE
                    {
                        double cpuaverageIntensityData = (peakDatalistsort[i].Intensity + peakDatalistsort[j].Intensity) / 2;
                        var dataforsummationMassandaverageintensity = new newMsPeaksDto(cpusummationMassData, cpuaverageIntensityData);
                        summationMassandaverageintensity.Add(dataforsummationMassandaverageintensity); //Making 2D list for Mass & Intensity
                    }
                }
            }

            if (summationMassandaverageintensity.Count == 0) // LAST UPDATE 20200121
            {
                return TunnedMass;
            }

            //FIGURE 5: STEP 3 Running window have size of Proton (and starts from smallest mass of FIGURE 5: STEP 2 list) [REF: SPCTRUM PAPER]
            summationMassandaverageintensity = summationMassandaverageintensity.OrderBy(n => n.Mass).ToList(); //20200120 - Sorting is being done here!
            double windowposition = summationMassandaverageintensity[0].Mass; //Smallest mass selected from Tuple Sums for creating a window positon
            const double proton = 1.00727647; //Mass of proton

            var windowcapturedelementsold = new List<newMsPeaksDto>();
            var oldindex = new List<int>();   //CHANGE MY NAME...
            var count = new List<int>();   //CHANGE MY NAME...
            double olddiff = 1, newdiff = 0;

            double SlidingWindowValue = parameters.SliderValue;

            SlidingWindowValue = (SlidingWindowValue * IntactProteinMass) / Math.Pow(10, 6);//20200915 - Value for sliding the window  


            int summationMassandaverageintensityindex = summationMassandaverageintensity.Count - 1;
            while (windowposition < summationMassandaverageintensity[summationMassandaverageintensityindex].Mass)
            {
                var newindex = new List<int>();   //CHANGE MY NAME...
                var windowcapturedelementsnew = new List<newMsPeaksDto>();
                for (int i = 0; i <= summationMassandaverageintensityindex; i++)
                {
                    if (windowposition <= summationMassandaverageintensity[i].Mass && summationMassandaverageintensity[i].Mass < windowposition + proton)//#DISCUSSION
                    {
                        var dataforwindowcapturedelementsnew = new newMsPeaksDto(summationMassandaverageintensity[i].Mass, summationMassandaverageintensity[i].Intensity);
                        windowcapturedelementsnew.Add(dataforwindowcapturedelementsnew);
                        newindex.Add(i);
                    }
                    else if (summationMassandaverageintensity[i].Mass >= windowposition + proton)
                    {
                        break;
                    }
                }
                count.Add(windowcapturedelementsnew.Count);
                int a = oldindex.Count;//count.Count;///DEL ME AFTER TESTING!!! :)
                if (windowcapturedelementsnew.Count > windowcapturedelementsold.Count)
                {
                    windowcapturedelementsold = windowcapturedelementsnew;
                    oldindex = newindex;
                }
                else if (windowcapturedelementsnew.Count == windowcapturedelementsold.Count)
                {
                    olddiff = Math.Abs((windowcapturedelementsold[windowcapturedelementsold.Count - 1].Mass) - IntactProteinMass);
                    newdiff = Math.Abs((windowcapturedelementsnew[windowcapturedelementsnew.Count - 1].Mass) - IntactProteinMass);

                    if (olddiff >= newdiff)
                    {
                        windowcapturedelementsold = windowcapturedelementsnew;
                        oldindex = newindex;
                    }
                }
                windowposition = windowposition + SlidingWindowValue;

            }

            int WindowCaptured = windowcapturedelementsold.Count;
            if (WindowCaptured == 0) // Updated 20210410
            {
                return TunnedMass;
            }

            //FIGURE 5: STEP 5:::: Applying formula [REF: SPCTRUM PAPER]
            double[] dataforsumoftunedmassesandintensities = new double[WindowCaptured];//Its tunedmassnominator according to SPECTRUM's formula for TunedMass

            double sumoftunedmassesandintensities = 0;
            double sumoftunedintensities = 0;

            for (int i = 0; i <= windowcapturedelementsold.Count - 1; i++)
            {
                dataforsumoftunedmassesandintensities[i] = windowcapturedelementsold[i].Mass * windowcapturedelementsold[i].Intensity;

                sumoftunedmassesandintensities = sumoftunedmassesandintensities + dataforsumoftunedmassesandintensities[i];
                //Actually, ""intensities are not tuned"" (there isn't any terminology in OUR Mass Tuner Algorithm) according to the algorithm but just for the sake of UNDERSTANDING variable names are "dataforsumoftunedintensities" & "sumoftunedintensities".
                sumoftunedintensities = sumoftunedintensities + windowcapturedelementsold[i].Intensity;//Its tunedmassdenominator according to SPECTRUM's formula for TunedMass
            }
            TunnedMass = sumoftunedmassesandintensities / sumoftunedintensities;// NOW, Tuned Mass will be consider as Intact Mass(WholeProteinMolecularWeight)
            return TunnedMass;
        }


        //public class peakData2Dlist    //Depreciated 20210409
        //{//(double Mass, double Intenstity){
        //    public double Mass { get; set; }
        //    public double Intensity { get; set; }

        //    public peakData2Dlist(double i, double j)  /*!< Parameterized constructor */
        //    {
        //        Mass = i;
        //        Intensity = j;
        //    }
        //    //*** WHAT IS THE PURPOSE OF DEFAULT CONSTRUCTOR?***//
        //    public peakData2Dlist()
        //    {
        //        Mass = 0;
        //        Intensity = 0;
        //    }
        //}
    }
}
