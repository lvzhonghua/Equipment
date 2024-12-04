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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDataReader));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnOpenFiles = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.lstData = new System.Windows.Forms.ListBox();
            this.dlgPrintPreview = new System.Windows.Forms.PrintPreviewDialog();
            this.printDoc = new System.Drawing.Printing.PrintDocument();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lstData);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1016, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 749);
            this.panel1.TabIndex = 0;
            // 
            // chartData
            // 
            chartArea3.AxisX.LabelStyle.Format = "N1";
            chartArea3.AxisX.MajorGrid.Enabled = false;
            chartArea3.AxisX.Title = "时间(s)";
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea3.AxisY.MajorGrid.Enabled = false;
            chartArea3.AxisY.Title = "抗压强度(MPa)";
            chartArea3.Name = "ChartArea1";
            this.chartData.ChartAreas.Add(chartArea3);
            this.chartData.Dock = System.Windows.Forms.DockStyle.Fill;
            legend3.DockedToChartArea = "ChartArea1";
            legend3.Name = "Legend1";
            legend3.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chartData.Legends.Add(legend3);
            this.chartData.Location = new System.Drawing.Point(0, 0);
            this.chartData.Margin = new System.Windows.Forms.Padding(2);
            this.chartData.Name = "chartData";
            series7.BorderColor = System.Drawing.Color.White;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Color = System.Drawing.Color.Red;
            series7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series7.Legend = "Legend1";
            series7.MarkerBorderColor = System.Drawing.Color.Red;
            series7.Name = "抗压强度（Kpa）-1";
            series7.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series7.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.Name = "抗压强度（KPa）-2";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Legend = "Legend1";
            series9.Name = "抗压强度（KPa）-3";
            this.chartData.Series.Add(series7);
            this.chartData.Series.Add(series8);
            this.chartData.Series.Add(series9);
            this.chartData.Size = new System.Drawing.Size(1016, 749);
            this.chartData.TabIndex = 2;
            title3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            title3.Name = "标题";
            title3.Text = "标题";
            this.chartData.Titles.Add(title3);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenFiles,
            this.toolStripSeparator1,
            this.btnPrint});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(246, 28);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnOpenFiles
            // 
            this.btnOpenFiles.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenFiles.Image")));
            this.btnOpenFiles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenFiles.Name = "btnOpenFiles";
            this.btnOpenFiles.Size = new System.Drawing.Size(94, 25);
            this.btnOpenFiles.Text = "打开文件";
            this.btnOpenFiles.Click += new System.EventHandler(this.btnOpenFiles_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(94, 25);
            this.btnPrint.Text = "打印报告";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lstData
            // 
            this.lstData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstData.Font = new System.Drawing.Font("宋体", 12F);
            this.lstData.FormattingEnabled = true;
            this.lstData.IntegralHeight = false;
            this.lstData.ItemHeight = 16;
            this.lstData.Location = new System.Drawing.Point(0, 28);
            this.lstData.Name = "lstData";
            this.lstData.Size = new System.Drawing.Size(246, 721);
            this.lstData.TabIndex = 1;
            // 
            // dlgPrintPreview
            // 
            this.dlgPrintPreview.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.dlgPrintPreview.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.dlgPrintPreview.ClientSize = new System.Drawing.Size(400, 300);
            this.dlgPrintPreview.Document = this.printDoc;
            this.dlgPrintPreview.Enabled = true;
            this.dlgPrintPreview.Icon = ((System.Drawing.Icon)(resources.GetObject("dlgPrintPreview.Icon")));
            this.dlgPrintPreview.Name = "myPrePrintDoc";
            this.dlgPrintPreview.Visible = false;
            // 
            // printDoc
            // 
            this.printDoc.DocumentName = "试验报告";
            this.printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDoc_PrintPage);
            // 
            // FormDataReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 749);
            this.Controls.Add(this.chartData);
            this.Controls.Add(this.panel1);
            this.Name = "FormDataReader";
            this.ShowIcon = false;
            this.Text = "数据查看器";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox lstData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnOpenFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        private System.Windows.Forms.PrintPreviewDialog dlgPrintPreview;
        private System.Drawing.Printing.PrintDocument printDoc;
    }
}