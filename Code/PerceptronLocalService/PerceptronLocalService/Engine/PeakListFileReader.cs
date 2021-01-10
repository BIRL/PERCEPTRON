using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;
using System.Diagnostics;

namespace PerceptronLocalService.Engine
{


    public class PeakListFileReader : IPeakListFileReader
    {
        private void UrwasMzXmlReader(List<string> raw, ref string addressMzXml)
        {
            string line;

            // Read the File and display it line by line.
            //System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Urwa\Desktop\result.mzxml");

            var file = new StreamReader(addressMzXml);
            var mslevel = false;

            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("msLevel=\"2\""))
                    mslevel = true;
                if (mslevel != true) continue;
                if (!line.Contains("compressedLen")) continue;
                var str = line.Split('>')[1];
                str = str.Split('<')[0];
                raw.Add(str);
                mslevel = false;
            }

            file.Close();
        }

        private void Decrypt(ref string addr)
        {
            var rawList = new List<string>();
            UrwasMzXmlReader(rawList, ref addr);        // calling the function

            var mzxmladdr2 = @"C:\Users\Urwa\Desktop\mzxmlTopDown2\";
            for (var t = 0; t < rawList.Count; t++)
            {
                var base64EncodedBytes = Convert.FromBase64String(rawList.ElementAt(t));      // decoding base64
                var peaksCount = base64EncodedBytes.Length / 8;
                var mz = new List<float>();
                var intensity = new List<float>();
                var offset = 0;
                for (var i = 0; i < (peaksCount); i++)          // changing byte order
                {
                    var first4 = new byte[4];
                    var last4 = new byte[4];

                    first4[0] = base64EncodedBytes[offset * 8];
                    first4[1] = base64EncodedBytes[(offset * 8) + 1];
                    first4[2] = base64EncodedBytes[(offset * 8) + 2];
                    first4[3] = base64EncodedBytes[(offset * 8) + 3];

                    last4[0] = base64EncodedBytes[(offset * 8) + 4];
                    last4[1] = base64EncodedBytes[(offset * 8) + 5];
                    last4[2] = base64EncodedBytes[(offset * 8) + 6];
                    last4[3] = base64EncodedBytes[(offset * 8) + 7];

                    Array.Reverse(first4);
                    Array.Reverse(last4);

                    mz.Add(BitConverter.ToSingle(first4, 0));       // adding extracted values
                    intensity.Add(BitConverter.ToSingle(last4, 0));        // adding extracted values
                    offset++;
                }

                var intermed = t + 1;
                addr = mzxmladdr2 + "scan_1.txt";
                var pathname = mzxmladdr2 + "scan_" + intermed + ".txt";     // creating new files

                var path = @pathname;
                if (File.Exists(path))
                    File.Delete(path);

                if (File.Exists(path)) continue;
                {
                    using (var sw = File.CreateText(path))
                    {
                        for (var i = 0; i < mz.Count; i++)
                        {
                            sw.WriteLine(mz.ElementAt(i) + "\t" + intensity.ElementAt(i));
                        }
                    }
                }
            }
        }

        private void mgf_reader(ref string addressFile)
        {
            const string charge = "CHARGE";
            const string pepmass = "PEPMASS";
            var i = 1; //Keeps count of the number of text files to be made
            var fin = new FileStream(addressFile, FileMode.Open);
            var sr = new StreamReader(fin); //converts bytes into strings//26-3-20
            var s = sr.ReadLine();
            var maxCount = 0; //Keeps count of the maximum peaklist file which has been read uptil 'i'
            var maxFilename = ""; //Stores the name of the file containting max peaklist
            var mass = new List<double>();//List for storing masses
            var intensities = new List<double>();//List for storing intensities
            var splitAddress = addressFile.Split('\\');
            var newAddress = "";

            int DELME;

            for (var k = 0; k < (splitAddress.Length) - 1; k++)//stores path //
            {
                newAddress += (splitAddress[k] + '\\');
            }

            //Stores name of mgf file which is being processed--------
            var name = splitAddress[(splitAddress.Length) - 1];
            var item = ".mgf";
            if (name.EndsWith(item))
            {
                name = name.Substring(0, name.LastIndexOf(item));
            }
            //Reads mgf file--------------------------------------------
            while (s != null)//runs till the end of mgf file/ it collects masses and intensities for ONE text file at every iteration
            {
                var pepMass = "";

                if (File.Exists(newAddress + name + "_" + i + ".txt")) //deleting preexisting files
                    File.Delete(newAddress + name + "_" + i + ".txt");

                var fout = new FileStream(newAddress + name + "_" + i + ".txt", FileMode.OpenOrCreate); //making a text file
                var sw = new StreamWriter(fout);
                while (s != "END IONS" && s != null)
                {
                    if (s.Contains(charge))
                    {
                    }

                    if (s.Contains(pepmass)) //stores intact mass
                    {
                        pepMass = (s.Split('='))[1]; //splitting at = sign. stores whatever is after = sign//26-3-20
                        pepMass = string.Format("{0:0.######}", Convert.ToDouble(pepMass));  // Taking pepMass upto 6 digits after decimal  //20200711
                    }

                    if (!s.Contains("BEGIN IONS") && !s.Contains("=")) //stores masses and their intensities
                    {

                        if (s.Contains(' '))
                        {
                            string[] arr = s.Split(' ');
                            arr[0] = string.Format("{0:0.######}", Convert.ToDouble(arr[0]));
                            mass.Add(Convert.ToDouble(arr[0]));
                            arr[1] = string.Format("{0:0.######}", Convert.ToDouble(arr[1]));
                            intensities.Add(Convert.ToDouble(arr[1]));
                        }
                        else if (s.Contains('\t'))
                        {
                            string[] arr = s.Split('\t');
                            arr[0] = string.Format("{0:0.######}", Convert.ToDouble(arr[0]));
                            mass.Add(Convert.ToDouble(arr[0]));
                            arr[1] = string.Format("{0:0.######}", Convert.ToDouble(arr[1]));
                            intensities.Add(Convert.ToDouble(arr[1]));
                        }
                        else
                        {
                            if (s != "")
                            {
                                s = string.Format("{0:0.######}", Convert.ToDouble(s));
                                mass.Add(Convert.ToDouble(s));
                                intensities.Add(-7);
                            }

                        }

                    }
                    s = sr.ReadLine();
                }

                if (pepMass != "") //stores intensity of intact mass as 1
                {
                    sw.WriteLine(pepMass + ' ' + "1");
                }


                for (var x = 0; x < mass.Count; x++) //copies masses and intensities into the text file
                {
                    if (intensities.ElementAt(x) == -7)
                    {
                        sw.WriteLine(mass.ElementAt(x));
                    }
                    else
                    {
                        sw.WriteLine(mass.ElementAt(x) + "\t" + intensities.ElementAt(x));
                    }

                }

                if (mass.Count > maxCount && Convert.ToDouble(pepMass) > 113) //Checking the file for max number of scans and MS1 should be greater than 113. Although no mass of protein is equal to this but we are considering as GG but still its a peptide    //Updated 20210108
                {
                    maxCount = mass.Count;
                    maxFilename = newAddress + name + "_" + i + ".txt";
                }

                mass.Clear();
                intensities.Clear();
                s = sr.ReadLine();
                i++;
                sw.Close();
                fout.Close();

            }
            addressFile = maxFilename; //redirects addressMgf to store the address of the file with max peaklist
        }

        private void mzXML_Reader(ref string addressFile)
        {
            var filepath = Directory.GetCurrentDirectory();
            var navigatepath = Path.GetFullPath(Path.Combine(filepath, "..\\..\\..\\"));  //CHANGE IT WHEN REQUIRED!!! // JUST FOR SAFETY... WHEN VERSION RELEASED THEN, RECHECK IT...
            var MsDeconvConsolePath = Path.GetFullPath(Path.Combine(navigatepath, ".\\PerceptronLocalService\\Tools\\MsDeconvConsole.jar"));  // Navigated to the path where MsDeconvConsole.jar is exists

            try
            {

                System.Diagnostics.Process clientProcess = new Process();
                clientProcess.StartInfo.FileName = "java";//@"C:\Program Files\Java\jre1.8.0_251\bin\java.exe";
                clientProcess.StartInfo.Arguments = @"-jar " + MsDeconvConsolePath + " " + addressFile;// + @"D:\PERCEPTRON_CODE\files\DT4_161116.mzXML";
                clientProcess.Start();
                clientProcess.WaitForExit();
                int code = clientProcess.ExitCode;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Just the Path of MsDeconvConsole.jar");
                string k = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
                
            }

            addressFile = Path.ChangeExtension(addressFile, ".mgf");  //Replacing extension into the filename from .mzXML to .mgf;
            addressFile = Path.GetDirectoryName(addressFile) + "\\"+ Path.GetFileNameWithoutExtension(addressFile) + "_msdeconv" + Path.GetExtension(addressFile);  //For adding "_msdeconv" in filename. Because MsDeconConsole.jar create the output filename with "_msdeconv"
             //return newFileName;
            //mgf_reader(ref address);

        }

        private void mzML_Reader(ref string addressFile)
        {
            var filepath = Directory.GetCurrentDirectory();
            var navigatepath = Path.GetFullPath(Path.Combine(filepath, "..\\..\\..\\"));   // JUST FOR SAFETY... WHEN VERSION RELEASED THEN, RECHECK IT...
            var OpenMSPath = Path.GetFullPath(Path.Combine(navigatepath, ".\\PerceptronLocalService\\Tools\\OpenMS"));  // Navigated to the path where OpenMS folder is exists


            try
            {
                var addressmzXMLFile = addressFile; //file name for mzXML
                addressmzXMLFile = Path.ChangeExtension(addressmzXMLFile, ".mzXML");  //newfilename = newfilename.Replace(".mzML", ".mzXML");

                System.Environment.SetEnvironmentVariable("OPENMS_DATA_PATH", OpenMSPath + "\\share"); //setting environment variable

                bool check = false;

                System.Diagnostics.Process mzmlToMzxml = new Process();
                mzmlToMzxml.StartInfo.FileName = OpenMSPath + "\\FileConverter.exe";
                mzmlToMzxml.StartInfo.Arguments = " -in " + addressFile + " -out " + addressmzXMLFile;

                check = mzmlToMzxml.Start();
                mzmlToMzxml.WaitForExit();
                int code1 = mzmlToMzxml.ExitCode;
                addressFile = addressmzXMLFile;
            }
            catch (Exception e)
            {
                addressFile = "";
            }
        }

        public MsPeaksDto PeakListReader(SearchParametersDto parameters, int fileNumber)//EXTRACTING MASS & INTENSITY FROM FILE(s)
        {

            var intensity = new List<double>();
            var mw = new List<double>();

            var address = Path.Combine(Path.Combine(Constants.PeakListFolder, parameters.PeakListUniqueFileNames[fileNumber])); // 20200509 //Replaced "PeakListFileName" by "PeakListUniqueFileNames"
            //file address with name.. ALL files uploaded at App_Data folder (C:\inetpub\wwwroot\PerceptronAPI\App_Data). And then, PerceptronLocalService starts working on user data..

            

            if (parameters.FileType[fileNumber] == ".mgf")
            {
                mgf_reader(ref address);
            }

            else if (parameters.FileType[fileNumber] == ".mzXML")
            {
                mzXML_Reader(ref address);
                mgf_reader(ref address);
                //Decrypt(ref address);
            }

            else if (parameters.FileType[fileNumber] == ".mzML")
            {
                mzML_Reader(ref address);
                mzXML_Reader(ref address);
                mgf_reader(ref address);
            }

            var counter = 0;
            string line;
            string[] separators = { "\t", " " };
            var file = new StreamReader(address);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains('\t') || line.Contains(' '))
                {
                    var token = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    var a = double.Parse(token[0]);                                                         //NEW STRUCTURED LIST CREATE KR K DIRECT PEAKDATA2DLIST (peakData2Dlist) BNANI HA, FOR COMPUTATIONAL EFFICIENCY 
                    var b = double.Parse(token[1]);
                    mw.Add(a);
                    intensity.Add(b);
                }
                else
                {
                    var a = double.Parse(line);
                    const int b = 0;
                    //mw.Add(a);

                    /////////////////Neutral mass

                    mw.Add(a + 1.0078250321);  // Why adding?? Mass of Hydrogen = 1.007825035
                                               //Because (m+z)/z

                    intensity.Add(b);
                }
                counter++;
            }
            file.Close();
            var mWeight = mw[0];

            return new MsPeaksDto(intensity, mw, mWeight);

        }


    }
}
