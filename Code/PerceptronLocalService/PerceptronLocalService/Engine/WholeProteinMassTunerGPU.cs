using System;
using System.Collections.Generic;
using Cudafy;
using System.Linq;
using Cudafy.Host;
using Cudafy.Translator;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;

namespace PerceptronLocalService.Engine
{
    //Tuple Sums: Sums of MS1 and MS2s with each other
    public class WholeProteinMassTunerGpu : IWholeProteinMassTuner
    {
        private const int NumberOfThreads = 1024;

        public void TuneWholeProteinMass(MsPeaksDto peakData, SearchParametersDto parameters)
        {
            double molTolerance = parameters.MwTolerance;
            //peakData a data extracted from INPUT FILE
         //molTolerance is Intact Mass Tolerance
            //var peakDataMassSort = new int [peakData.Mass.Count]; Make an array of size of peakData.Mass for storing peakData.Mass
            ////////////peakData.Mass.Sort(); // SORTING OF MASSES  WHY SORTING FOR ...????
            //peakData.Intensity.Sort(); //SORTING OF INTENSITIES  -- DON'T SORT INTENSITIES...

            ///////****************************************///////
            ///////*****************STEPS*****************///////
            ///////**************************************///////

/////////////FOR MASSES/////////////
            //sizeofarray = peakData.Mass.Count * peakData.Mass.Count
            // GPU Initializations
            // GPU variable initalizations
            // GPU variable data Preperation
            // Launching GPU Code
            // Post GPU Processing 
            //Converting 2dimensional array{outputarray(summation matrix of masses)} into 1dimensional array for easy to use...
            //Removing null values(which was redundant & GPU give them value = 0 due to its redundancy) {e.g. [0,1] = 1130.638 & [1,0] = 1130.638. So, GPU gives us [0,1] = 1130.638 & [1,0] = 0 to remove the redundancy because  [0,1] and [1,0] are the same}
            // Converting 1D array to 1D list for easy to use in future.. e.g. sorting etc.

/////////////FOR INTENSITIES/////////////
            // GPU variable initalizations
            // GPU variable data Preperation
            // Launching GPU Code
            //Removing null values(which was redundant & GPU give them value = 0 due to its redundancy) {e.g. [0,1] = 1130.638 & [1,0] = 1130.638. So, GPU gives us [0,1] = 1130.638 & [1,0] = 0 to remove the redundancy because  [0,1] and [1,0] are the same}
            //Converting summation to average i.e. dividing by 2
            //Previously size of array(gpuaverageIntensityData) allocated more which is "sizeofarray". So, now resizing the array(gpuaverageIntensityData) to the size of gpuaverageIntensityDataIndex because remain enteries(elements) are empty(=0)



/////////////FURTHER PROCESSING/////////////
            // Now, making 2D-list(gpuprocessedpeaklistdata) from two 1D-lists that are  (1D-list of)GPU processed tuple sums OF masses(gpusummationMassData) & (1D-list of)GPU processed tuple sums OF intensities divided by 2(gpuaverageIntensityData)
            // Sorting gpuprocessedpeaklistdata(2D-list) with respect to the Masses in ascending Order

            //FIGURE 5: STEP 2:::: SUMMATIONS FOR EACH M/Z I,J IN MS2 DATA [REF: SPCTRUM PAPER]
            //Description::: Calculate Peak Sums and shortlist those falling within user specified tolerance.
            //FIGURE 5: STEP 3 Running window have size of Proton (and starts from smallest mass of FIGURE 5: STEP 2 list) [REF: SPCTRUM PAPER]

            //FIGURE 5: STEP 5:::: Applying formula [REF: SPCTRUM PAPER]


            
            var sizeofarray = peakData.Mass.Count * peakData.Mass.Count;

            /////////////FOR MASSES/////////////

            // GPU Initializations
            var gpu = CudafyHost.GetDevice(CudafyModes.Target);
            var arch = gpu.GetArchitecture();
            var km = CudafyTranslator.Cudafy(arch);
            gpu.LoadModule(km);
            // Create a new thread -- (GPU Initializations)
            // GPU variable initalizations
            var lengthGpuArray = sizeofarray; //peak * peak [In the given case (51 * 51 = 2601)]
            var monoisotopicMassPeaks = peakData.Mass.ToArray();
            var peakListLength = new int[1];
            peakListLength[0] = peakData.Mass.Count;
            var gpuoutputarraymasses = new double[peakData.Mass.Count, peakData.Mass.Count];

            // GPU variable data Preperation
            var monoisotopicMassPeaksDevice = gpu.Allocate(monoisotopicMassPeaks);
            var peakListLengthDevice = gpu.Allocate(peakListLength);
            var outputDevice = gpu.Allocate(gpuoutputarraymasses);
            gpu.CopyToDevice(monoisotopicMassPeaks, monoisotopicMassPeaksDevice);
            gpu.CopyToDevice(peakListLength, peakListLengthDevice);
            gpu.CopyToDevice(gpuoutputarraymasses, outputDevice);

            // Launching GPU Code
            var numOfBlocks = (int)Math.Ceiling((double)lengthGpuArray / NumberOfThreads);
            gpu.Launch(numOfBlocks, NumberOfThreads).DeviceGenerateFragmentPairMatrix(monoisotopicMassPeaksDevice, peakListLengthDevice, outputDevice);
            gpu.CopyFromDevice(outputDevice, gpuoutputarraymasses);
            gpu.FreeAll();

            // Post GPU Processing 
            //Converting 2dimensional array{outputarray(summation matrix of masses)} into 1dimensional array for easy to use...
            var gpusummationMassData = new double[sizeofarray];
            int gpusummationMassDataIndex = 0;
            for (int rowwise = 0; rowwise <= peakData.Mass.Count - 1; rowwise++)  // -1 (peakData.Mass.Count - 1) because its starting from 0 (rowwise = 0)
            {
                for (int columnwise = 0; columnwise <= peakData.Mass.Count - 1; columnwise++)//Removing null values(which was redundant & GPU give them value = 0 due to its redundancy) {e.g. [0,1] = 1130.638 & [1,0] = 1130.638. So, GPU gives us [0,1] = 1130.638 & [1,0] = 0 to remove the redundancy because  [0,1] and [1,0] are the same}
                {
                    if (gpuoutputarraymasses[rowwise, columnwise] != 0)
                    {
                        gpusummationMassData[gpusummationMassDataIndex++] = gpuoutputarraymasses[rowwise, columnwise];
                    }
                }
            }
            Array.Resize(ref gpusummationMassData, gpusummationMassDataIndex);  //Previously size of array(gpusummationMassData) allocated more which is "sizeofarray". So, now resizing the array(gpusummationMassData) to the size of gpusummationMassDataIndex because remain enteries(elements) are empty(=0)

            List<double> gpusummationMassDatalist = gpusummationMassData.ToList(); // Converting 1D array to 1D list for easy to use in future.. e.g. sorting etc.

/////////////FOR INTENSITIES/////////////
            // GPU variable initalizations
            var monoisotopicIntensityPeaks = peakData.Intensity.ToArray();
            var intensitypeakListLength = new int[1];
            intensitypeakListLength[0] = peakData.Intensity.Count;
            var gpuoutputarrayintensities = new double[peakData.Mass.Count, peakData.Mass.Count];

            // GPU variable data Preperation
            var monoisotopicIntensityPeaksDevice = gpu.Allocate(monoisotopicIntensityPeaks);
            var intensitypeakListLengthDevice = gpu.Allocate(intensitypeakListLength); //intensitypeakListLengthDevice
            var intensityoutputDevice = gpu.Allocate(gpuoutputarrayintensities); //intensityoutputDevice
            gpu.CopyToDevice(monoisotopicIntensityPeaks, monoisotopicIntensityPeaksDevice);
            gpu.CopyToDevice(intensitypeakListLength, intensitypeakListLengthDevice);
            gpu.CopyToDevice(gpuoutputarrayintensities, intensityoutputDevice);

            // Launching GPU Code
            var numOfBlocksforintensity = (int)Math.Ceiling((double)lengthGpuArray / NumberOfThreads);
            gpu.Launch(numOfBlocksforintensity, NumberOfThreads).DeviceGenerateFragmentPairMatrix(monoisotopicIntensityPeaksDevice, intensitypeakListLengthDevice, intensityoutputDevice);
            gpu.CopyFromDevice(intensityoutputDevice, gpuoutputarrayintensities);
            gpu.FreeAll();

            int gpuaverageIntensityDataIndex = 0;
            var gpuaverageIntensityData = new double[sizeofarray];
            for (int rowwise = 0; rowwise <= peakData.Mass.Count - 1; rowwise++)// -1 (peakData.Mass.Count - 1) because its starting from 0 (rowwise = 0)
            {
                for (int columnwise = 0; columnwise <= peakData.Mass.Count - 1; columnwise++)
                {
                    if (gpuoutputarrayintensities[rowwise, columnwise] != 0)//Removing null values(which was redundant & GPU give them value = 0 due to its redundancy) {e.g. [0,1] = 1130.638 & [1,0] = 1130.638. So, GPU gives us [0,1] = 1130.638 & [1,0] = 0 to remove the redundancy because  [0,1] and [1,0] are the same}
                    {
                        gpuaverageIntensityData[gpuaverageIntensityDataIndex++] = (gpuoutputarrayintensities[rowwise, columnwise]) / 2; //Converting summation to average i.e. dividing by 2
                    }
                }
            }
            Array.Resize(ref gpuaverageIntensityData, gpuaverageIntensityDataIndex);  //Previously size of array(gpuaverageIntensityData) allocated more which is "sizeofarray". So, now resizing the array(gpuaverageIntensityData) to the size of gpuaverageIntensityDataIndex because remain enteries(elements) are empty(=0)


            // Now, making 2D-list(gpuprocessedpeaklistdata) from two 1D-lists that are  (1D-list of)GPU processed tuple sums OF masses(gpusummationMassData) & (1D-list of)GPU processed tuple sums OF intensities divided by 2(gpuaverageIntensityData)
            var gpuprocessedpeaklistdata = new List<gpuprocessedmassandintensity>();
            List<double> gpuprocessedpeaklist = new List<double>();//#NECESSARY
            for (int row = 0; row <= gpuaverageIntensityDataIndex - 1; row++)// -1 (peakData.Mass.Count - 1) because its starting from 0 (rowwise = 0)
            {
                double Masses = gpusummationMassData[row];
                double Intensities = gpuaverageIntensityData[row];
                var dataforgpuprocessedpeaklistdata = new gpuprocessedmassandintensity(Masses, Intensities);
                gpuprocessedpeaklistdata.Add(dataforgpuprocessedpeaklistdata);
            }

            // Sorting gpuprocessedpeaklistdata(2D-list) with respect to the Masses in ascending Order
            var gpuprocessedpeaklistdatasorted = gpuprocessedpeaklistdata.OrderBy(n => n.Masses).ToList(); // for obtaining Ascending Order list
            
            //FIGURE 5: STEP 2:::: SUMMATIONS FOR EACH M/Z I,J IN MS2 DATA [REF: SPCTRUM PAPER]
            //Description::: Calculate Peak Sums and shortlist those falling within user specified tolerance.
            var gpushortlistedpeaklist = new List<gpuprocessedmassandintensity>();

            for (int rowiselistelements = 0; rowiselistelements <= gpusummationMassDataIndex - 1; rowiselistelements++)
            {
                if (peakData.WholeProteinMolecularWeight - molTolerance <= gpuprocessedpeaklistdatasorted[rowiselistelements].Masses + parameters.NeutralLoss && gpuprocessedpeaklistdatasorted[rowiselistelements].Masses + parameters.NeutralLoss  <= peakData.WholeProteinMolecularWeight + molTolerance)//Masses & Intensities filter out due to the selected RANGE OF INTACT MASS +/- PROTEIN MASS TOLERANCE   //Adding Neutral Mass Loss if user know the Loss of Mass during experimentation
                {
                    var dataforgpushortlistedpeaklist = new gpuprocessedmassandintensity(gpuprocessedpeaklistdatasorted[rowiselistelements].Masses, gpuprocessedpeaklistdatasorted[rowiselistelements].Intensities);

                    gpushortlistedpeaklist.Add(dataforgpushortlistedpeaklist);
                }
            }

            if (gpushortlistedpeaklist.Count == 0) //numel(Peak.Sums) == 0 #FutureWork2  // LAST UPDATE 20200121
            {
                peakData.WholeProteinMolecularWeight = 0;
                return;
                //// Featuring Baad ma..#FutureWork1
                //int Tuned_MolWt = 0;
                //int Fragments_SumofMolWt = 0;
                //int Fragments_MaxIntensity = 0;
                //int Histc_Unique_Fragments_MolWt = 0;
                //int Unique_Fragments_Occurrences = 0;
            }
            //FIGURE 5: STEP 3 Running window have size of Proton (and starts from smallest mass of FIGURE 5: STEP 2 list) [REF: SPCTRUM PAPER]
/////////////
            double windowposition = gpushortlistedpeaklist[0].Masses; //Smallest mass selected from Tuple Sums for creating a window positon

            const double proton = 1.00727647; //Mass of proton

            var windowcapturedelementsold = new List<gpuprocessedmassandintensity>();            
            
            var oldindex = new List<int>();   //CHANGE MY NAME...
            var count = new List<int>();   //CHANGE MY NAME...
            double olddiff = 1, newdiff = 0;

            double SlidingWindowValue = 50.0;
            if (parameters.SliderValue != 0)
                SlidingWindowValue = parameters.SliderValue;
            SlidingWindowValue = (SlidingWindowValue * peakData.WholeProteinMolecularWeight) / Math.Pow(10, 6);//20200915 - Value for sliding the window  


            int gpushortlistedpeaklistindex = gpushortlistedpeaklist.Count - 1;
            while (windowposition < gpushortlistedpeaklist[gpushortlistedpeaklistindex].Masses) //window will run on the gpushortlistedpeaklist ///WHY THIS..?}}}window < gpushortlistedpeaklist[gpushortlistedpeaklistindex].Masses{{{
                //windowposition ?? Proper def...
            {
                var newindex = new List<int>();   //CHANGE MY NAME...
                //var windowcapturedelementsnew = new List<double>();
                var windowcapturedelementsnew = new List<gpuprocessedmassandintensity>();
                for (int i = 0; i <= gpushortlistedpeaklistindex; i++)
                {
                    if (gpushortlistedpeaklist[i].Masses >= windowposition && gpushortlistedpeaklist[i].Masses < windowposition + proton)// + proton//#DISCUSSION
                    {
                        var dataforwindowcapturedelementsnew = new gpuprocessedmassandintensity(gpushortlistedpeaklist[i].Masses, gpushortlistedpeaklist[i].Intensities);
                        windowcapturedelementsnew.Add(dataforwindowcapturedelementsnew);
                        int counting = windowcapturedelementsnew.Count;
                        //windowcapturedelementsnew.Add(gpushortlistedpeaklist[i].Masses);
                        newindex.Add(i);
                    }
                    else if (gpushortlistedpeaklist[i].Masses >= windowposition + proton)
                    {
                        break;
                    }
                }
                count.Add(windowcapturedelementsnew.Count);//CHANGE MY NAME...........................................
                if (windowcapturedelementsnew.Count > windowcapturedelementsold.Count)
                {
                    windowcapturedelementsold = windowcapturedelementsnew;
                    oldindex = newindex;
                }
                else if (windowcapturedelementsnew.Count == windowcapturedelementsold.Count)
                {
                    olddiff = Math.Abs((windowcapturedelementsold[windowcapturedelementsold.Count - 1].Masses) - peakData.WholeProteinMolecularWeight);
                    newdiff = Math.Abs((windowcapturedelementsnew[windowcapturedelementsnew.Count - 1].Masses) - peakData.WholeProteinMolecularWeight);

                    if (olddiff >= newdiff)
                    {
                        windowcapturedelementsold = windowcapturedelementsnew;
                        oldindex = newindex;
                    }
                }
                windowposition = windowposition + SlidingWindowValue;
            }

            //FIGURE 5: STEP 5:::: Applying formula [REF: SPCTRUM PAPER]
            double[] dataforsumoftunedmassesandintensities = new double[windowcapturedelementsold.Count];//Its tunedmassnominator according to SPECTRUM's formula for TunedMass
            
            double sumoftunedmassesandintensities = 0;
            double sumoftunedintensities = 0;

            for (int i = 0; i <= windowcapturedelementsold.Count - 1; i++)
            {
                dataforsumoftunedmassesandintensities[i] = windowcapturedelementsold[i].Masses * windowcapturedelementsold[i].Intensities;

                sumoftunedmassesandintensities = sumoftunedmassesandintensities + dataforsumoftunedmassesandintensities[i];
                //Actually, ""intensities are not tuned"" (there isn't any terminology in OUR Mass Tuner Algorithm) according to the algorithm but just for the sake of UNDERSTANDING variable name is "sumoftunedintensities".
                sumoftunedintensities = sumoftunedintensities + windowcapturedelementsold[i].Intensities;//Its tunedmassdenominator according to SPECTRUM's formula for TunedMass
            }
            peakData.WholeProteinMolecularWeight = sumoftunedmassesandintensities / sumoftunedintensities;// NOW, Tuned Mass will be consider as Intact Mass(WholeProteinMolecularWeight)
        }


        public class gpuprocessedmassandintensity
        {
            public double Masses { get; set; }          /*!< Masses: Member Variable */
            public double Intensities { get; set; }    /*!< Intensities: Member Variable */

            public gpuprocessedmassandintensity(double i, double d)    /*!< Parameterized constructor */
            {
                Masses = i;
                Intensities = d;
            }

            public gpuprocessedmassandintensity()  /*!< default constructor */
            {
                Masses = 0;
                Intensities = 0;
            }
        }        
        
        [Cudafy]
        private static void DeviceGenerateFragmentPairMatrix(GThread thread, double[] peaks, int[] len, double[,] output)
        {
            var tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            var s = tid % len[0];
            var e = s + (tid / len[0]);

            if (e >= len[0]) return;

            var dif = peaks[e] + peaks[s];
            output[s, e] = dif;
        }
    }
}