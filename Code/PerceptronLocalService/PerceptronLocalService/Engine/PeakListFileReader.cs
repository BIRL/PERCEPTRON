using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PerceptronLocalService.Interfaces;
using PerceptronLocalService.DTO;
using PerceptronLocalService.Utility;

namespace PerceptronLocalService.Engine
{


    public class PeakListFileReader : IPeakListFileReader
    {
        private void MzXmlReader(List<string> raw, ref string addressMzXml)
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
            MzXmlReader(rawList, ref addr);        // calling the function

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

        private void mgf_reader(ref string addressMgf)
        {
            const string charge = "CHARGE";
            const string pepmass = "PEPMASS";
            var i = 1;
            var fin = new FileStream(addressMgf, FileMode.Open);
            var sr = new StreamReader(fin);
            var s = sr.ReadLine();
            var maxCount = 1;
            var maxFilename = "";
            var mass = new List<double>();
            var intensities = new List<double>();
            var splitAddress = addressMgf.Split('\\');
            var newAddress = "";


            for (var k = 0; k < (splitAddress.Length) - 1; k++)
            {
                newAddress += (splitAddress[k] + '\\');
            }

            while (s != null)
            {
                var pepMass = "";

                if (File.Exists(newAddress + "scan" + i + ".txt"))
                    File.Delete(newAddress + "scan" + i + ".txt");

                var fout = new FileStream(newAddress + "scan" + i + ".txt", FileMode.OpenOrCreate);
                var sw = new StreamWriter(fout);
                while (s != "END IONS" && s != null)
                {
                    if (s.Contains(charge))
                    {
                    }

                    if (s.Contains(pepmass))
                    {
                        pepMass = (s.Split('='))[1];
                    }

                    if (!s.Contains("BEGIN IONS") && !s.Contains("="))
                    {

                        if (s.Contains(' '))
                        {
                            string[] arr = s.Split(' ');
                            mass.Add(Convert.ToDouble(arr[0]));
                            intensities.Add(Convert.ToDouble(arr[1]));
                        }
                        else if (s.Contains('\t'))
                        {
                            string[] arr = s.Split('\t');
                            mass.Add(Convert.ToDouble(arr[0]));
                            intensities.Add(Convert.ToDouble(arr[1]));
                        }
                        else
                        {
                            if (s != "")
                            {
                                mass.Add(Convert.ToDouble(s));
                                intensities.Add(-7);
                            }

                        }

                    }
                    s = sr.ReadLine();
                }

                sw.WriteLine(pepMass);

                for (var x = 0; x < mass.Count; x++)
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

                if (mass.Count + 1 > maxCount)
                {
                    maxFilename = "scan" + i + ".txt";
                    maxCount = mass.Count + 1;
                }

                mass.Clear();
                intensities.Clear();
                s = sr.ReadLine();
                i++;
                sw.Close();
                fout.Close();

            }
            addressMgf = newAddress + maxFilename;
        }

        public MsPeaksDto PeakListReader(SearchParametersDto parameters, int fileNumber)//EXTRACTING MASS & INTENSITY FROM FILE(s)
        {

            var intensity = new List<double>();
            var mw = new List<double>();

            var address = Path.Combine(Path.Combine(Constants.PeakListFolder, parameters.PeakListUniqueFileNames[fileNumber])); // 20200509 //Replaced "PeakListFileName" by "PeakListUniqueFileNames"
            //file address with name.. ALL files uploaded at App_Data folder (C:\inetpub\wwwroot\PerceptronAPI\App_Data). And then, PerceptronLocalService starts working on user data..

            if (parameters.FileType[fileNumber] == ".mzXML")//INSERT HERE THE SUPPORT OF .mzML
            {
                Decrypt(ref address);
            }

            if (parameters.FileType[fileNumber] == ".mgf")
            {
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
