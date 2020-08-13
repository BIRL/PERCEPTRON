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
using System.IO;

using System.Web;

namespace PerceptronAPI.Engine
{
    public partial class DetailedProteinView : Form    /// CHANGE MY NAME
    {

        public DetailedProteinView()   /// CHANGE MY NAME
        {
            InitializeComponent();
        }

        public string writeOnImage(DetailedProteinHitView RawData, bool downloadresults) /// CHANGE MY NAME
        {
            /* Data Preparation*/
            var ResultsData = RawData.Results.Results;
            var searchParameters = RawData.searchParameters;
            var PeakListData = RawData.PeakListData;

            var PeakListMasses = PeakListData.PeakListMasses.Split(',').Select(double.Parse).ToList();
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
            int ProteinRank = ResultsData.ProteinRank;

            var BlindPtm = new BlindPtmInfo(ResultsData.BlindPtmLocalization);
            var BlindPtmLocalizationInfo = ResultsData.BlindPtmLocalization.Split(',').ToList<string>();

            var PtmSites = new List<PostTranslationModificationsSiteDto>();
            if (RawData.Results.PtmSitesInfo.Index != null)
            {
                PostTranslationModificationsSiteDto _PostTranslationModificationsSiteDto = new PostTranslationModificationsSiteDto();
                var PtmSitesInfo = new PostTranslationModificationsSiteDto(RawData.Results.PtmSitesInfo);

                var tempInfo = _PostTranslationModificationsSiteDto.ProcessPtmSiteInfo(PtmSitesInfo);
                PtmSites.AddRange(tempInfo);
                PtmSites = PtmSites.OrderBy(x => x.SiteIndex).ToList();
            }
                


            //BlindPtmLocalizationStart = 5; BlindPtmLocalizationEnd = 10; BlindPtmLocalizationMass = 54343.09;   // Just for testing

            string NameofFileWithFullPath = "";
            string NameofFile = "";
            /* Image */
            using (var image = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height))
            {
                var font = new Font("TimesNewRoman", 10, FontStyle.Regular, GraphicsUnit.Point);
                var fontSeqNum = new Font("TimesNewRoman", 5, FontStyle.Regular, GraphicsUnit.Point);
                var fontStrikethrough = new Font("TimesNewRoman", 10, FontStyle.Strikeout, GraphicsUnit.Point);
                var fontTruncation = new Font("TimesNewRoman", 15, FontStyle.Regular, GraphicsUnit.Point);
                var fontMasses = new Font("TimesNewRoman", 8, FontStyle.Regular, GraphicsUnit.Point);
                var fontBlindPtmLocalization = new Font("TimesNewRoman", 8, FontStyle.Bold, GraphicsUnit.Point);

                int PictureBoxWidth = this.pictureBox1.Width;

                var graphics = Graphics.FromImage(image);
                graphics.Clear(BackColor);

                /* Image Data Preparation*/
                string LeftTruncation = "]";
                string RightTruncation = "[";

                /* Distancing Variables */
                int xPoint = 20;
                int xHeaderPoint = 1;
                int yPoint = 3;
                int xdistBetween2 = 60;
                int yHeaderLinedist = 20;
                int yNextLinedist = 100;
                int yNextTruncatedLinedist = 30;
                int xLeftTruncationdist = 20;
                int xRightTruncationdist = 70;
                int yTruncationdist = 5;
                //int xMassesdist = 15;
                int yLeftPeakMassdist = 23;
                int yLeftMatchedMassdist = 35;
                int yRightPeakMassdist = 43;
                int yRightMatchedMassdist = 55;


                string ProteinInfo = " >Protein ID: " + ResultsData.Header;
                graphics.DrawString(ProteinInfo, font, Brushes.Blue, new PointF(xHeaderPoint, yPoint));
                ProteinInfo = " >Mass: " + Math.Round(ResultsData.Mw, 4) + "        " + " Score: " + Math.Round(ResultsData.Score, 4) + "        " + " Rank: " + ProteinRank + "        " + " Matches: " + Matches;

                yPoint = yPoint + yHeaderLinedist; /* Distancing Variables */
                graphics.DrawString(ProteinInfo, font, Brushes.Blue, new PointF(xHeaderPoint, yPoint));

                yPoint = yPoint + 10; /* Distancing Variables */
                string Separator = "_____________________________________________________________________________________________________________________________________";
                graphics.DrawString(Separator, font, Brushes.Blue, new PointF(xHeaderPoint, yPoint));

                yPoint = yPoint + yHeaderLinedist; /* Distancing Variables */




                string ProteinSequence = ResultsData.Sequence;// +"ABCDEFGHEKDIELSODJCNFYDHEIWKDNSMFLXOSJ";

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
                if (searchParameters.Truncation == "True")
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


                    
                    //
                    if (ListPSTTags[0] != "")    /* Peptide Sequence Tag */
                    {
                        if (!PstIndex.Contains(i)) //i >= PstStartIndex[] && i < PstStartIndex[] + ListPSTTags[]
                        {
                            graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Black, new PointF(xPoint, yPoint));
                        }
                        else
                        {
                            graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Blue, new PointF(xPoint, yPoint));
                        }
                    }
                    else
                    {
                        if (searchParameters.PtmAllow == "True" && BlindPtm.BlindPtmLocalizationStart != -1)   /* BlindPtm */
                        {
                            if (i >= BlindPtm.BlindPtmLocalizationStart && i < BlindPtm.BlindPtmLocalizationEnd)
                            {
                                graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Magenta, new PointF(xPoint, yPoint));
                                if (i == BlindPtm.BlindPtmLocalizationStart)
                                {
                                    graphics.DrawString(Math.Round(BlindPtm.BlindPtmLocalizationMass, 4).ToString(), fontMasses, Brushes.Red, new PointF(xPoint, yPoint - yLeftPeakMassdist));
                                }
                            }
                            else                                                                        /* Simple */
                            {
                                graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Black, new PointF(xPoint, yPoint));
                            }
                        }
                        else                                                                            /* Simple */
                        {
                            graphics.DrawString(ProteinSequence[i].ToString(), font, Brushes.Black, new PointF(xPoint, yPoint));
                        }
                        
                    }

                   
                    if (LeftMatchedIndex.Count != 0)
                    {

                        if (LeftMatchedIndex.Contains(i))
                        {
                            int indexL = LeftMatchedIndex.IndexOf(i);
                            double PeakMass = Math.Round(PeakListMasses[LeftPeakIndex[indexL]], 4);

                            graphics.DrawString(LeftTruncation, fontTruncation, Brushes.Black, new PointF(xPoint + xLeftTruncationdist, yPoint - yTruncationdist));
                            graphics.DrawString(PeakMass.ToString(), fontMasses, Brushes.Red, new PointF(xPoint, yPoint + yLeftPeakMassdist));
                            graphics.DrawString(Math.Round(InsilicoMassLeft[i], 4).ToString(), fontMasses, Brushes.Green, new PointF(xPoint, yPoint + yLeftMatchedMassdist));
                        }
                    }

                    if (RightMatchedIndex.Count != 0)
                    {
                        for (int index = 0; index < RightMatchedIndex.Count; index++)
                        {
                            int RightTruncationIndexPosition = ProteinSequence.Length - RightMatchedIndex[index];

                            if (RightTruncationIndexPosition == i)
                            {

                                double PeakMass = Math.Round(PeakListMasses[RightPeakIndex[index]], 4);
                                double RightMatchedMass = Math.Round(InsilicoMassRight[RightMatchedIndex[index]], 4);


                                graphics.DrawString(RightTruncation, fontTruncation, Brushes.Black, new PointF(xPoint - xRightTruncationdist, yPoint - yTruncationdist));
                                graphics.DrawString(PeakMass.ToString(), fontMasses, Brushes.Red, new PointF(xPoint - xRightTruncationdist, yPoint + yRightPeakMassdist));
                                graphics.DrawString(RightMatchedMass.ToString(), fontMasses, Brushes.Green, new PointF(xPoint - xRightTruncationdist, yPoint + yRightMatchedMassdist));
                            }
                        }

                    }

                    for (int indexPtmSites = 0; indexPtmSites < PtmSites.Count; indexPtmSites++)
                    {
                        if (PtmSites[indexPtmSites].SiteIndex == i)
                        {
                            string ModSymbol = ModificationSymbol(PtmSites[indexPtmSites].ModName);
                            graphics.DrawString(ModSymbol, fontMasses, Brushes.Red, new PointF(xPoint, yPoint - yLeftPeakMassdist - 5));
                        }
                        
                    }

                    NumString = (i + 1).ToString();  //- NumCount
                    graphics.DrawString(NumString.ToString(), fontSeqNum, Brushes.Black, new PointF(xPoint + 13, yPoint + 10));
                    xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */
                }


                /* Right Truncation */
                if (searchParameters.Truncation == "True")
                {
                    if (ResultsData.TruncationSite == "Right")
                    {
                        int startIndex;
                        if (ResultsData.TerminalModification == "NME" || ResultsData.TerminalModification == "NME_Acetylation")
                        {
                            startIndex = ResultsData.TruncationIndex + 2;  // No, Zero Indexing
                        }
                        else
                        {
                            startIndex = ResultsData.TruncationIndex + 1;
                        }
                        for (int indexR = startIndex; indexR < ResultsData.OriginalSequence.Length; indexR++)
                        {
                            graphics.DrawString(ResultsData.OriginalSequence[indexR].ToString(), fontStrikethrough, Brushes.Red, new PointF(xPoint, yPoint));
                            xPoint = xPoint + xdistBetween2;/////////////////////////////////yPoint = yPoint + 20; /* Distancing Variables */
                            nextLineStart(ref xPoint, ref PictureBoxWidth, ref yPoint, ref yNextLinedist);
                            TruncationCount++;
                        }
                    }
                }


                var Original = image;
                this.pictureBox1.Image = image;
                try
                {

                    Rectangle rectangle = new Rectangle(0, 0, this.pictureBox1.Width, yPoint + 100);

                    var imageSave = Original.Clone(rectangle, Original.PixelFormat);  // Resizing Image: Copying Image and removing white (empty) space

                    //var filepath = Directory.GetCurrentDirectory();
                    var navigatepath = HttpContext.Current.Server.MapPath("~/App_Data");  //Path.GetFullPath(Path.Combine(filepath, "..\\..\\"));
                    var DirectoryPath = "";

                    //var abds = HttpContext.Current.Server.MapPath("~/App_Data/Results/TemporaryResults");

                    if (downloadresults == false)//downloadresults = false; // Setting flag for Images will be stored at "App_Data" Folder
                        DirectoryPath = navigatepath + "\\Results\\TemporaryResults\\";  // Navigated to the path where Temporary Files should be created
                    else //downloadresults = true; // Setting flag for Images will be stored at "\ResultsDownload\Results File Images" permanent(ResultsDownload) folders
                        DirectoryPath = Path.GetFullPath(Path.Combine(navigatepath, ".\\inetpub\\wwwroot\\assets\\ResultsDownload\\ResultsFileImages\\"));  // Navigated to the path where Files should be created

                    //NameOfFileWithPathDirectoryPath = @"D:\\01_PERCEPTRON\\gitHub\\PERCEPTRON\\Code\\PerceptronFrontEnd\\src\\assets\\ResultsFileImages\\";   //Overriding DirectoryPath...
                    //D:\\01_PERCEPTRON\\gitHub\\PERCEPTRON\\Code\\PerceptronFrontEnd\\assets\\ResultsFileImages\\     //Overriding DirectoryPath..
                    NameofFile = "DetailedProteinView_Qid_" + ResultsData.QueryId + "_Rid_" + ResultsData.ResultId + ".jpg";
                    //DirectoryPath = @"D:\\01_PERCEPTRON\\gitHub\\PERCEPTRON\\Code\\PerceptronFrontEnd\\assets\\ResultsFileImages\\";   //Overriding DirectoryPath..

                    //DirectoryPath = Path.GetFullPath(Path.Combine(navigatepath, ".\\inetpub\\wwwroot\\"));      //Overriding DirectoryPath..


                    DirectoryPath = @"C:\\PerceptronApi-tempResultsFolder\\";
                    NameofFileWithFullPath = DirectoryPath + NameofFile;
                    imageSave.Save(NameofFileWithFullPath);

                    //image.Save(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Utility\discarded.jpg");      \src\assets\ResultsFileImages
                    imageSave.Dispose(); // Releases all resources used
                }
                catch (Exception e)
                {
                    int a = 0;  //Generate EMail or popup if something wrong... For the User
                }



                CloseFormWindow();

                // Releases all resources used by
                font.Dispose();
                fontSeqNum.Dispose();
                fontStrikethrough.Dispose();
                fontTruncation.Dispose();
                fontMasses.Dispose();
                image.Dispose();
                graphics.Dispose();
                
            }
            return NameofFileWithFullPath;
        }

        private string ModificationSymbol(string modification)
        {
            string symbol = "";

            switch (modification)
            {
                case "Phosphorylation":
                    return symbol = "\u03D6";
                case "DiMethylation":
                    return symbol = "\u03C9";

                case "Methylation":
                    return symbol = "\u03C8";

                case "Acetylation":
                    return symbol = "\u03B1";

                case "Hydroxylation":
                    return symbol = "\u03B8";

                case "DiHydroxylation":
                    return symbol = "\u2294";

                case "O-Linked-Glycosylation":
                    return symbol = "\u2295";

                case "Sulfoxide":
                    return symbol = "\u00D8";

                case "Glutathionylation":
                    return symbol = "\u2207";

                case "Methylation_K":
                    return symbol = "\u0394";

                case "S-Nitrosylation":
                    return symbol = "\u2663";

                case "Palmitoylation":
                    return symbol = "\u2665";

                case "Formylation":
                    return symbol = "\u2660";

                case "Nitration":
                    return symbol = "\u25CA";

                case "N-Linked-Glycosylation":
                    return symbol = "\u211C";

                case "Sulfone":
                    return symbol = "\u03B6";

                case "Pyruvate-S":
                    return symbol = "\u22A5";

                case "Pyrrolidone-Aarboxylic-Acid":
                    return symbol = "\u2228";

                case "Gamma-Carboxyglutamic-Acid":
                    return symbol = "\u03C9";

                default:
                    return symbol;
            }


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
            if (xPoint >= this.pictureBox1.Width - 70)
            {
                xPoint = 20;
                yPoint = yPoint + yNextLinedist;
            }
        }
        //private void nextTruncatedLineStart(ref int xPoint, ref int PictureBoxWidth, ref int yPoint, ref int yNextTruncatedLinedist)
        //{
        //    if (xPoint >= this.pictureBox1.Width - 70)
        //    {
        //        xPoint = 20;
        //        yPoint = yPoint + yNextTruncatedLinedist;
        //    }
        //}

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




