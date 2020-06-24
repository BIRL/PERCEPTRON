using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PerceptronAPI.Controllers;
using PerceptronAPI.Models;

namespace PerceptronAPI.Utility
{
    public partial class Form1 : Form    /// CHANGE MY NAME
    {

        public Form1()   /// CHANGE MY NAME
        {
            InitializeComponent();
        }

        private List<int> PstIndexFind(string Sequence, List<string> ListPSTTags)
        {
            List<int> PstIndex = new List<int>();

            if (ListPSTTags.Count != 0)
            {
                for (int i = 0; i < ListPSTTags.Count; i++)
                {
                    int index = Sequence.IndexOf(ListPSTTags[i]);
                    PstIndex.Add(index);

                    for (int j = 0; j < ListPSTTags[i].Length - 1; j++)
                    {
                        index++;
                        PstIndex.Add(index);
                    }
                }
            }
            return PstIndex;
        }

        private void nextLineStart(ref int xPoint, ref int PictureBoxWidth, ref int yPoint, ref int yNextLinedist)
        {
            if (xPoint >= this.pictureBox1.Width - 100)
            {
                xPoint = 1;
                yPoint = yPoint + yNextLinedist;
            }
        }

        public void writeOnImage(DetailedProteinHitView RawData) /// CHANGE MY NAME
        {
            /* Data Preparation*/
            var ResultsData = RawData.Results.Results;
            var searchParameters = RawData.searchParameters;

            var peakData = ResultsData.PeakListData.Split(',').Select(double.Parse).ToList();
            var ListPSTTags = ResultsData.PSTTags.Split(',').ToList();
            var PstIndex = PstIndexFind(ResultsData.Sequence, ListPSTTags);

            var LeftMatchedIndex = new List<int>();
            var LeftPeakIndex = new List<int>();
            var LeftType = new List<string>();
            int LeftMatches = 0;
            var InsilicoMassLeft = new List<double>();

            var RightMatchedIndex = new List<int>();
            var RightPeakIndex = new List<int>();
            var RightType = new List<string>();
            int RightMathces = 0;
            var InsilicoMassRight = new List<double>();

            if (ResultsData.LeftMatchedIndex != "")
            {
                LeftMatchedIndex = ResultsData.LeftMatchedIndex.Split(',').Select(int.Parse).ToList();
                LeftPeakIndex = ResultsData.LeftPeakIndex.Split(',').Select(int.Parse).ToList();
                LeftType = ResultsData.LeftType.Split(',').ToList();
                LeftMatches = LeftMatchedIndex.Count;
                InsilicoMassLeft = ResultsData.InsilicoMassLeft.Split(',').Select(double.Parse).ToList();
            }

            if (ResultsData.RightMatchedIndex != "")
            {
                RightMatchedIndex = ResultsData.RightMatchedIndex.Split(',').Select(int.Parse).ToList();
                RightPeakIndex = ResultsData.RightPeakIndex.Split(',').Select(int.Parse).ToList();
                RightType = ResultsData.RightType.Split(',').ToList();
                RightMathces = RightMatchedIndex.Count;
                InsilicoMassRight = ResultsData.InsilicoMassRight.Split(',').Select(double.Parse).ToList();
            }

            int Matches = LeftMatches + RightMathces;
            int TruncationCount = 0;
            


            /* Image */
            var image = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            var font = new Font("TimesNewRoman", 10, FontStyle.Regular, GraphicsUnit.Point);
            var fontSeqNum = new Font("TimesNewRoman", 5, FontStyle.Regular, GraphicsUnit.Point);
            var fontStrikethrough = new Font("TimesNewRoman", 10, FontStyle.Strikeout, GraphicsUnit.Point);
            var fontTruncation = new Font("TimesNewRoman", 15, FontStyle.Regular, GraphicsUnit.Point);
            var fontMasses = new Font("TimesNewRoman", 8, FontStyle.Regular, GraphicsUnit.Point);

            int PictureBoxWidth = this.pictureBox1.Width;

            var graphics = Graphics.FromImage(image);
            graphics.Clear(BackColor);

            /* Image Data Preparation*/
            string LeftTruncation = "]";
            string RightTruncation = "[";
            
            /* Distancing Variables */
            int xPoint = 1;
            int yPoint = 3;
            int xdistBetween2 = 45;
            int yHeaderLinedist = 20;
            int yNextLinedist = 80;
            int yPointReset;
            int xTruncationdist = 30;
            int yTruncationdist = 5;
            //int xMassesdist = 15;
            int yPeakMassdist = 23;
            int yMatchedMassdist = 35;


            string ProteinInfo = " >Protein ID: " + ResultsData.Header;
            graphics.DrawString(ProteinInfo, font, Brushes.Blue, new PointF(xPoint, yPoint));
            ProteinInfo = " >Mass: " + Math.Round(ResultsData.Mw, 4) + "        " + " Score: " + Math.Round(ResultsData.Score, 4) + "        " + " Rank: " + "?????" + "        " + " Matches: " + Matches;

            yPoint = yPoint + yHeaderLinedist; /* Distancing Variables */
            graphics.DrawString(ProteinInfo, font, Brushes.Blue, new PointF(xPoint, yPoint));

            yPoint = yPoint + 10; /* Distancing Variables */
            string Separator = "_____________________________________________________________________________________________________________________________________";
            graphics.DrawString(Separator, font, Brushes.Blue, new PointF(xPoint, yPoint));

            yPoint = yPoint + yHeaderLinedist; /* Distancing Variables */

            yPointReset = yPoint;

            
            string ProteinSequence = ResultsData.Sequence + "ABCDEFGHEKDIELSODJCNFYDHEIWKDNSMFLXOSJ";

            yPoint = yPoint + yHeaderLinedist; /* Distancing Variables */
            string NumString = "";
            int NumCount = 0;

            /* Terminal Modification */

            if (ResultsData.TerminalModification == "NME" || ResultsData.TerminalModification == "NME_Acetylation")
            {
                
                graphics.DrawString("M", fontStrikethrough, Brushes.Red, new PointF(xPoint, yPoint));
                xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */

            }
            /* Left Truncation */
            if (searchParameters.Truncation == 1)
            {
                if (ResultsData.TruncationSite == "Left")
                {
                    for (int indexL = 0; indexL < ResultsData.TruncationIndex; indexL++)
                    {
                        graphics.DrawString(ResultsData.OriginalSequence[indexL].ToString(), fontStrikethrough, Brushes.Red, new PointF(xPoint, yPoint));
                        xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */
                        nextLineStart(ref xPoint, ref PictureBoxWidth, ref yPoint, ref yNextLinedist);
                        TruncationCount++;
                    }
                }
            }



            for (int i = 0; i < ProteinSequence.Length; i++)
            {
                
                nextLineStart(ref xPoint, ref PictureBoxWidth, ref yPoint, ref yNextLinedist);


                /* Peptide Sequence Tag */
                //
                
                if (!PstIndex.Contains(i)) //i >= PstStartIndex[] && i < PstStartIndex[] + ListPSTTags[]
                {
                    graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Black, new PointF(xPoint, yPoint));
                }
                else
                {
                    graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Blue, new PointF(xPoint, yPoint));
                }

                if (LeftMatchedIndex.Count != 0)
                {
                    if (LeftMatchedIndex.Contains(i))
                    {
                        int indexL = LeftMatchedIndex.IndexOf(i);
                        double PeakMass = Math.Round(peakData[LeftPeakIndex[indexL]], 4);



                        graphics.DrawString(LeftTruncation, fontTruncation, Brushes.Black, new PointF(xPoint + xTruncationdist, yPoint - yTruncationdist));
                        graphics.DrawString(PeakMass.ToString(), fontMasses, Brushes.Red, new PointF(xPoint + xTruncationdist, yPoint + yPeakMassdist));
                        graphics.DrawString(Math.Round(InsilicoMassLeft[i], 4).ToString(), fontMasses, Brushes.Green, new PointF(xPoint + xTruncationdist, yPoint + yMatchedMassdist));

                    }

                }
                /////////  DONE DO FOR SYNTAX TESTIN


                //if (RightMatchedIndex.Count != 0)
                //{
                //    if (RightMatchedIndex.Contains(i))
                //    {
                //        int indexR = RightMatchedIndex.IndexOf(i);
                //        double PeakMass = Math.Round(peakData[RightPeakIndex[indexR]], 4);



                //        graphics.DrawString(RightTruncation, fontTruncation, Brushes.Black, new PointF(xPoint - xTruncationdist, yPoint - yTruncationdist));
                //        graphics.DrawString(PeakMass.ToString(), fontMasses, Brushes.Red, new PointF(xPoint + xTruncationdist, yPoint + yPeakMassdist));
                //        graphics.DrawString(Math.Round(InsilicoMassRight[i], 4).ToString(), fontMasses, Brushes.Green, new PointF(xPoint + xTruncationdist, yPoint + yMatchedMassdist));

                //    }

                //}
                /**/


                //NumString = (i + 1 - NumCount).ToString();
                graphics.DrawString(i.ToString(), fontSeqNum, Brushes.Black, new PointF(xPoint + 13, yPoint + 10));
                xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */

            }


            ///////////  DONE DO FOR SYNTAX TESTIN
            ///* Right Truncation */

            //if (searchParameters.Truncation == 1)
            //{
            //    if (ResultsData.TruncationSite == "Right")
            //    {
            //        for (int indexR = 0; indexR < ResultsData.TruncationIndex; indexR++)
            //        {
            //            graphics.DrawString(ResultsData.OriginalSequence[indexR].ToString(), fontStrikethrough, Brushes.Red, new PointF(xPoint, yPoint));
            //            xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */
            //            nextLineStart(ref xPoint, ref PictureBoxWidth, ref yPoint, ref yNextLinedist);
            //            TruncationCount++;
            //        }
            //    }
            //}


            var Original = image;
            this.pictureBox1.Image = image;
            try
            {
                //var image1 = new Bitmap(1000, 500, graphics);   //Resizing Image: Cutting Extra White space
                Rectangle rectangle = new Rectangle(0, 0, this.pictureBox1.Width, yPoint + 100);

                var b = Original.PixelFormat;
                var image1 = Original.Clone(rectangle, Original.PixelFormat);
                image1.Save(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Utility\Ima.jpg");
                image.Save(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Utility\discarded.jpg");
            }
            catch (Exception e)
            {
                int a = 0;
            }

            
            CloseFormWindow();

        }

        private void CloseFormWindow()
        {
            var tmr = new System.Windows.Forms.Timer();
            tmr.Tick += delegate
            {
                this.Close();
            };
            tmr.Interval = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
            tmr.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e) { }
    }
}


/* Reset xPoint & yPoint*/

//xPoint = 1;
//yPoint = yPointReset;


///* Change  */

//for (int i = 0; i < Mseq.Length; i++)
//{

//    xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */

//    if (xPoint >= this.pictureBox1.Width - 100)
//    {
//        xPoint = 1;
//        yPoint = yPoint + yNextLinedist;

//    }
//    NumString = 
//    graphics.DrawString(, font, Brushes.Blue, new PointF(xPoint, yPoint));

//    graphics.DrawString("fds", font, Brushes.Blue, new PointF(xPoint, yPoint));

//}