namespace CoatingAnalysis
{
    partial class FormOpenFile
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOpenFile));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabView1 = new System.Windows.Forms.TabPage();
            this.tranfChart_Open = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabView2 = new System.Windows.Forms.TabPage();
            this.ventiChart_Open = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabView3 = new System.Windows.Forms.TabPage();
            this.strengthChart_Open = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.ToolStripMenuPrintSet = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuPageSet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPrint = new System.Windows.Forms.ToolStripButton();
            this.infoListBox = new System.Windows.Forms.ListBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.myPrintPreDia = new System.Windows.Forms.PrintPreviewDialog();
            this.myPrintDoc = new System.Drawing.Printing.PrintDocument();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabView1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tranfChart_Open)).BeginInit();
            this.tabView2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ventiChart_Open)).BeginInit();
            this.tabView3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.strengthChart_Open)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(993, 530);
            this.splitContainer1.SplitterDistance = 720;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabView1);
            this.tabControl1.Controls.Add(this.tabView2);
            this.tabControl1.Controls.Add(this.tabView3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(720, 530);
            this.tabControl1.TabIndex = 0;
            // 
            // tabView1
            // 
            this.tabView1.Controls.Add(this.tranfChart_Open);
            this.tabView1.Location = new System.Drawing.Point(4, 29);
            this.tabView1.Name = "tabView1";
            this.tabView1.Padding = new System.Windows.Forms.Padding(3);
            this.tabView1.Size = new System.Drawing.Size(712, 497);
            this.tabView1.TabIndex = 0;
            this.tabView1.Text = "传输特性曲线";
            this.tabView1.UseVisualStyleBackColor = true;
            // 
            // tranfChart_Open
            // 
            chartArea4.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea4.AxisX.LabelStyle.Format = "N1";
            chartArea4.AxisX.MajorGrid.Enabled = false;
            chartArea4.AxisX.Title = "时间(s)";
            chartArea4.AxisX.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea4.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea4.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea4.AxisY.LabelStyle.Format = "N1";
            chartArea4.AxisY.MajorGrid.Enabled = false;
            chartArea4.AxisY.Title = "压强(KPa)";
            chartArea4.AxisY.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartArea4.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea4.BorderColor = System.Drawing.Color.Maroon;
            chartArea4.Name = "ChartArea1";
            this.tranfChart_Open.ChartAreas.Add(chartArea4);
            this.tranfChart_Open.Dock = System.Windows.Forms.DockStyle.Fill;
            legend4.DockedToChartArea = "ChartArea1";
            legend4.Name = "Legend1";
            legend4.Title = "压力-时间图";
            legend4.TitleAlignment = System.Drawing.StringAlignment.Far;
            legend4.TitleBackColor = System.Drawing.Color.White;
            legend4.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tranfChart_Open.Legends.Add(legend4);
            this.tranfChart_Open.Location = new System.Drawing.Point(3, 3);
            this.tranfChart_Open.Name = "tranfChart_Open";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Color = System.Drawing.Color.Red;
            series4.Legend = "Legend1";
            series4.LegendText = "压力";
            series4.MarkerBorderColor = System.Drawing.Color.Red;
            series4.Name = "Series";
            this.tranfChart_Open.Series.Add(series4);
            this.tranfChart_Open.Size = new System.Drawing.Size(706, 491);
            this.tranfChart_Open.TabIndex = 5;
            this.tranfChart_Open.Text = "chart1";
            // 
            // tabView2
            // 
            this.tabView2.Controls.Add(this.ventiChart_Open);
            this.tabView2.Location = new System.Drawing.Point(4, 29);
            this.tabView2.Name = "tabView2";
            this.tabView2.Padding = new System.Windows.Forms.Padding(3);
            this.tabView2.Size = new System.Drawing.Size(712, 497);
            this.tabView2.TabIndex = 1;
            this.tabView2.Text = "透气性曲线";
            this.tabView2.UseVisualStyleBackColor = true;
            // 
            // ventiChart_Open
            // 
            chartArea5.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea5.AxisX.LabelStyle.Format = "N1";
            chartArea5.AxisX.MajorGrid.Enabled = false;
            chartArea5.AxisX.Title = "时间(s)";
            chartArea5.AxisX.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea5.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea5.AxisY.LabelStyle.Format = "N1";
            chartArea5.AxisY.MajorGrid.Enabled = false;
            chartArea5.AxisY.Title = "压强(KPa)";
            chartArea5.AxisY.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartArea5.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.BorderColor = System.Drawing.Color.Maroon;
            chartArea5.Name = "ChartArea1";
            this.ventiChart_Open.ChartAreas.Add(chartArea5);
            this.ventiChart_Open.Dock = System.Windows.Forms.DockStyle.Fill;
            legend5.DockedToChartArea = "ChartArea1";
            legend5.Name = "Legend1";
            legend5.Title = "压力-时间图";
            legend5.TitleAlignment = System.Drawing.StringAlignment.Far;
            legend5.TitleBackColor = System.Drawing.Color.White;
            legend5.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ventiChart_Open.Legends.Add(legend5);
            this.ventiChart_Open.Location = new System.Drawing.Point(3, 3);
            this.ventiChart_Open.Name = "ventiChart_Open";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = System.Drawing.Color.Red;
            series5.Legend = "Legend1";
            series5.LegendText = "压力";
            series5.MarkerBorderColor = System.Drawing.Color.Red;
            series5.Name = "Series";
            this.ventiChart_Open.Series.Add(series5);
            this.ventiChart_Open.Size = new System.Drawing.Size(706, 491);
            this.ventiChart_Open.TabIndex = 4;
            this.ventiChart_Open.Text = "chart1";
            // 
            // tabView3
            // 
            this.tabView3.Controls.Add(this.strengthChart_Open);
            this.tabView3.Location = new System.Drawing.Point(4, 29);
            this.tabView3.Name = "tabView3";
            this.tabView3.Size = new System.Drawing.Size(712, 497);
            this.tabView3.TabIndex = 2;
            this.tabView3.Text = "高温强度曲线";
            this.tabView3.UseVisualStyleBackColor = true;
            // 
            // strengthChart_Open
            // 
            chartArea6.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea6.AxisX.LabelStyle.Format = "N1";
            chartArea6.AxisX.MajorGrid.Enabled = false;
            chartArea6.AxisX.Title = "时间(s)";
            chartArea6.AxisX.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea6.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Lines;
            chartArea6.AxisY.LabelStyle.Format = "N1";
            chartArea6.AxisY.MajorGrid.Enabled = false;
            chartArea6.AxisY.Title = "压强(KPa)";
            chartArea6.AxisY.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            chartArea6.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.BorderColor = System.Drawing.Color.Maroon;
            chartArea6.Name = "ChartArea1";
            this.strengthChart_Open.ChartAreas.Add(chartArea6);
            this.strengthChart_Open.Dock = System.Windows.Forms.DockStyle.Fill;
            legend6.DockedToChartArea = "ChartArea1";
            legend6.Name = "Legend1";
            legend6.Title = "压力-时间图";
            legend6.TitleAlignment = System.Drawing.StringAlignment.Far;
            legend6.TitleBackColor = System.Drawing.Color.White;
            legend6.TitleFont = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.strengthChart_Open.Legends.Add(legend6);
            this.strengthChart_Open.Location = new System.Drawing.Point(0, 0);
            this.strengthChart_Open.Name = "strengthChart_Open";
            this.strengthChart_Open.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Color = System.Drawing.Color.Red;
            series6.Legend = "Legend1";
            series6.LegendText = "压力";
            series6.MarkerBorderColor = System.Drawing.Color.Red;
            series6.Name = "Series";
            this.strengthChart_Open.Series.Add(series6);
            this.strengthChart_Open.Size = new System.Drawing.Size(712, 497);
            this.strengthChart_Open.TabIndex = 5;
            this.strengthChart_Open.Text = "chart1";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.infoListBox);
            this.splitContainer2.Size = new System.Drawing.Size(269, 530);
            this.splitContainer2.SplitterDistance = 42;
            this.splitContainer2.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripButtonOpen,
            this.toolStripButtonPrint});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(269, 42);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuPrintSet,
            this.ToolStripMenuPageSet});
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(84, 39);
            this.toolStripSplitButton1.Text = "设置";
            // 
            // ToolStripMenuPrintSet
            // 
            this.ToolStripMenuPrintSet.Name = "ToolStripMenuPrintSet";
            this.ToolStripMenuPrintSet.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuPrintSet.Text = "打印设置";
            this.ToolStripMenuPrintSet.Click += new System.EventHandler(this.ToolStripMenuPrintSet_Click);
            // 
            // ToolStripMenuPageSet
            // 
            this.ToolStripMenuPageSet.Name = "ToolStripMenuPageSet";
            this.ToolStripMenuPageSet.Size = new System.Drawing.Size(124, 22);
            this.ToolStripMenuPageSet.Text = "页面设置";
            this.ToolStripMenuPageSet.Click += new System.EventHandler(this.ToolStripMenuPageSet_Click);
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(72, 39);
            this.toolStripButtonOpen.Text = "打开";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonPrint
            // 
            this.toolStripButtonPrint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPrint.Image")));
            this.toolStripButtonPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPrint.Name = "toolStripButtonPrint";
            this.toolStripButtonPrint.Size = new System.Drawing.Size(72, 39);
            this.toolStripButtonPrint.Text = "打印";
            this.toolStripButtonPrint.Click += new System.EventHandler(this.toolStripButtonPrint_Click);
            // 
            // infoListBox
            // 
            this.infoListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoListBox.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.infoListBox.FormattingEnabled = true;
            this.infoListBox.ItemHeight = 20;
            this.infoListBox.Location = new System.Drawing.Point(0, 0);
            this.infoListBox.Name = "infoListBox";
            this.infoListBox.Size = new System.Drawing.Size(269, 484);
            this.infoListBox.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // myPrintPreDia
            // 
            this.myPrintPreDia.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.myPrintPreDia.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.myPrintPreDia.ClientSize = new System.Drawing.Size(400, 300);
            this.myPrintPreDia.Enabled = true;
            this.myPrintPreDia.Icon = ((System.Drawing.Icon)(resources.GetObject("myPrintPreDia.Icon")));
            this.myPrintPreDia.Name = "myPrintPreDia";
            this.myPrintPreDia.Visible = false;
            // 
            // myPrintDoc
            // 
            this.myPrintDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.myPrintDoc_PrintPage);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.AutoScroll = true;
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(993, 530);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(993, 555);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // FormOpenFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 555);
            this.Controls.Add(this.toolStripContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormOpenFile";
            this.Text = "文件浏览及打印";
            this.Load += new System.EventHandler(this.FormOpenFile_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabView1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tranfChart_Open)).EndInit();
            this.tabView2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ventiChart_Open)).EndInit();
            this.tabView3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.strengthChart_Open)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PrintPreviewDialog myPrintPreDia;
        private System.Drawing.Printing.PrintDocument myPrintDoc;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart tranfChart_Open;
        private System.Windows.Forms.TabPage tabView2;
        private System.Windows.Forms.DataVisualization.Charting.Chart ventiChart_Open;
        private System.Windows.Forms.TabPage tabView3;
        private System.Windows.Forms.DataVisualization.Charting.Chart strengthChart_Open;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuPrintSet;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuPageSet;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonPrint;
        private System.Windows.Forms.ListBox infoListBox;
    }
}