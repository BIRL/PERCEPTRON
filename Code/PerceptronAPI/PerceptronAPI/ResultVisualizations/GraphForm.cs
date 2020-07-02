using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphForm
{
    public partial class FormForGraph : Form
    {
        public FormForGraph()
        {
            InitializeComponent();
            fillChart();
            

        }

        private void Form11(object sender, EventArgs e)
        {

            fillChart();


        }


        private void fillChart()
        {
            //AddXY value in chart1 in series named as Salary  
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Ajay", "10000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Ramesh", "8000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Ankit", "7000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Gurmeet", "10000");
            MassSpectraVisualizationGraph.Series["Experimental_mz"].Points.AddXY("Suresh", "8500");
            //chart title  
            MassSpectraVisualizationGraph.Titles.Add("Mass Spectrum Visualization");

            MassSpectraVisualizationGraph.SaveImage(@"D:\01_PERCEPTRON\gitHub\PERCEPTRON\Code\PerceptronAPI\PerceptronAPI\Utility\mychart.png", ChartImageFormat.Png);
            CloseWindowGraphForm();
            
        }


        private void CloseWindowGraphForm()
        {
            var tmr = new System.Windows.Forms.Timer();
            tmr.Tick += delegate
            {
                this.Close();
            };
            tmr.Interval = (int)TimeSpan.FromSeconds(1).TotalMilliseconds;
            tmr.Start();
        }


    }
}
