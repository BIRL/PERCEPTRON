namespace GraphForm
{
    partial class FormForGraph
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.MassSpectraVisualizationGraph = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.MassSpectraVisualizationGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // MassSpectraVisualizationGraph
            // 
            this.MassSpectraVisualizationGraph.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.MassSpectraVisualizationGraph.BackImageWrapMode = System.Windows.Forms.DataVisualization.Charting.ChartImageWrapMode.Unscaled;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.Name = "ChartArea1";
            this.MassSpectraVisualizationGraph.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Experimental_mz";
            legend2.Enabled = false;
            legend2.Name = "Theoretical_mz";
            this.MassSpectraVisualizationGraph.Legends.Add(legend1);
            this.MassSpectraVisualizationGraph.Legends.Add(legend2);
            this.MassSpectraVisualizationGraph.Location = new System.Drawing.Point(12, 12);
            this.MassSpectraVisualizationGraph.Margin = new System.Windows.Forms.Padding(2);
            this.MassSpectraVisualizationGraph.Name = "MassSpectraVisualizationGraph";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Experimental_mz";
            series1.Name = "Experimental_mz";
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Theoretical_mz";
            series2.Name = "Theoretical_mz";
            series2.ShadowColor = System.Drawing.Color.Green;
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Experimental_mz";
            series3.Name = "PeakListMasses";
            this.MassSpectraVisualizationGraph.Series.Add(series1);
            this.MassSpectraVisualizationGraph.Series.Add(series2);
            this.MassSpectraVisualizationGraph.Series.Add(series3);
            this.MassSpectraVisualizationGraph.Size = new System.Drawing.Size(869, 388);
            this.MassSpectraVisualizationGraph.TabIndex = 0;
            this.MassSpectraVisualizationGraph.Text = "Mass Spectra";
            this.MassSpectraVisualizationGraph.Visible = false;
            // 
            // FormForGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 412);
            this.Controls.Add(this.MassSpectraVisualizationGraph);
            this.Name = "FormForGraph";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.MassSpectraVisualizationGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart MassSpectraVisualizationGraph;
    }
}

