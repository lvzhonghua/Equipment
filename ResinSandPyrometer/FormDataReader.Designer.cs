namespace ResinSandPyrometer
{
    partial class FormDataReader
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
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataReader));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.infoListBox = new System.Windows.Forms.ListBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.myPrintDoc = new System.Drawing.Printing.PrintDocument();
            this.myPrintPreDoc = new System.Windows.Forms.PrintPreviewDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.infoListBox);
            this.splitContainer1.Panel2.Controls.Add(this.btnPrint);
            this.splitContainer1.Panel2.Controls.Add(this.btnOpen);
            this.splitContainer1.Size = new System.Drawing.Size(1203, 697);
            this.splitContainer1.SplitterDistance = 975;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 1;
            // 
            // chart
            // 
            chartArea1.AxisX.LabelStyle.Format = "N1";
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Title = "时间(s)";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.Title = "抗压强度(MPa)";
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.DockedToChartArea = "ChartArea1";
            legend1.Name = "Legend1";
            legend1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Margin = new System.Windows.Forms.Padding(2);
            this.chart.Name = "chart";
            series1.BorderColor = System.Drawing.Color.White;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Red;
            series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.Red;
            series1.Name = "抗压强度（Kpa）-1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "抗压强度（KPa）-2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "抗压强度（KPa）-3";
            this.chart.Series.Add(series1);
            this.chart.Series.Add(series2);
            this.chart.Series.Add(series3);
            this.chart.Size = new System.Drawing.Size(975, 697);
            this.chart.TabIndex = 1;
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title1.Name = "name1";
            title1.Text = "抗压强度-- 时间";
            this.chart.Titles.Add(title1);
            // 
            // infoListBox
            // 
            this.infoListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoListBox.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.infoListBox.FormattingEnabled = true;
            this.infoListBox.IntegralHeight = false;
            this.infoListBox.ItemHeight = 20;
            this.infoListBox.Location = new System.Drawing.Point(2, 40);
            this.infoListBox.Margin = new System.Windows.Forms.Padding(2);
            this.infoListBox.Name = "infoListBox";
            this.infoListBox.Size = new System.Drawing.Size(225, 657);
            this.infoListBox.TabIndex = 1;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPrint.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrint.Location = new System.Drawing.Point(102, 8);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(97, 28);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "打印文件";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnOpen.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpen.Location = new System.Drawing.Point(11, 8);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(87, 28);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "打开文件";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // myPrintDoc
            // 
            this.myPrintDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.myPrintDoc_PrintPage);
            // 
            // myPrintPreDoc
            // 
            this.myPrintPreDoc.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.myPrintPreDoc.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.myPrintPreDoc.ClientSize = new System.Drawing.Size(400, 300);
            this.myPrintPreDoc.Enabled = true;
            this.myPrintPreDoc.Icon = ((System.Drawing.Icon)(resources.GetObject("myPrintPreDoc.Icon")));
            this.myPrintPreDoc.Name = "myPrePrintDoc";
            this.myPrintPreDoc.Visible = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // FormDataReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 697);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormDataReader";
            this.ShowIcon = false;
            this.Text = "数据查看器";
            this.Load += new System.EventHandler(this.FormOpenFile_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ListBox infoListBox;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnOpen;
        private System.Drawing.Printing.PrintDocument myPrintDoc;
        private System.Windows.Forms.PrintPreviewDialog myPrintPreDoc;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}