namespace GasDataReader
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSaveData = new System.Windows.Forms.Button();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.tabData = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gasCurveCtrl = new GET3G_PC.Controls.GasCurveCtrl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtGasData = new System.Windows.Forms.TextBox();
            this.btnSaveToImage = new System.Windows.Forms.Button();
            this.tabData.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpen.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpen.Location = new System.Drawing.Point(12, 498);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(125, 33);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "打开文件";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrint.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrint.Location = new System.Drawing.Point(143, 498);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(125, 33);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "打印曲线";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSaveData
            // 
            this.btnSaveData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveData.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveData.Location = new System.Drawing.Point(465, 498);
            this.btnSaveData.Name = "btnSaveData";
            this.btnSaveData.Size = new System.Drawing.Size(125, 33);
            this.btnSaveData.TabIndex = 3;
            this.btnSaveData.Text = "另存数据";
            this.btnSaveData.UseVisualStyleBackColor = true;
            this.btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // tabData
            // 
            this.tabData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabData.Controls.Add(this.tabPage1);
            this.tabData.Controls.Add(this.tabPage2);
            this.tabData.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabData.Location = new System.Drawing.Point(4, 3);
            this.tabData.Name = "tabData";
            this.tabData.SelectedIndex = 0;
            this.tabData.Size = new System.Drawing.Size(799, 489);
            this.tabData.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gasCurveCtrl);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(791, 459);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "发气量曲线";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gasCurveCtrl
            // 
            this.gasCurveCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gasCurveCtrl.CurveName = "[发气量-发气速度]曲线";
            this.gasCurveCtrl.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gasCurveCtrl.Gas = 0F;
            this.gasCurveCtrl.GasInCrement = 0F;
            this.gasCurveCtrl.GasInCrementList = ((System.Collections.Generic.List<float>)(resources.GetObject("gasCurveCtrl.GasInCrementList")));
            this.gasCurveCtrl.GasList = ((System.Collections.Generic.List<float>)(resources.GetObject("gasCurveCtrl.GasList")));
            this.gasCurveCtrl.GasTime = ((long)(0));
            this.gasCurveCtrl.GasTimeList = ((System.Collections.Generic.List<long>)(resources.GetObject("gasCurveCtrl.GasTimeList")));
            this.gasCurveCtrl.Interval = 1;
            this.gasCurveCtrl.Location = new System.Drawing.Point(0, 0);
            this.gasCurveCtrl.Name = "gasCurveCtrl";
            this.gasCurveCtrl.Size = new System.Drawing.Size(779, 448);
            this.gasCurveCtrl.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtGasData);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(791, 459);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "发气量数据";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtGasData
            // 
            this.txtGasData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGasData.Location = new System.Drawing.Point(-4, 0);
            this.txtGasData.Multiline = true;
            this.txtGasData.Name = "txtGasData";
            this.txtGasData.ReadOnly = true;
            this.txtGasData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGasData.Size = new System.Drawing.Size(779, 448);
            this.txtGasData.TabIndex = 0;
            // 
            // btnSaveToImage
            // 
            this.btnSaveToImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveToImage.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveToImage.Location = new System.Drawing.Point(274, 498);
            this.btnSaveToImage.Name = "btnSaveToImage";
            this.btnSaveToImage.Size = new System.Drawing.Size(185, 33);
            this.btnSaveToImage.TabIndex = 2;
            this.btnSaveToImage.Text = "曲线另存为图片";
            this.btnSaveToImage.UseVisualStyleBackColor = true;
            this.btnSaveToImage.Click += new System.EventHandler(this.btnSaveToImage_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 543);
            this.Controls.Add(this.btnSaveToImage);
            this.Controls.Add(this.tabData);
            this.Controls.Add(this.btnSaveData);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "发气量数据查看器";
            this.tabData.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSaveData;
        private System.Drawing.Printing.PrintDocument printDocument;
        private GET3G_PC.Controls.GasCurveCtrl gasCurveCtrl;
        private System.Windows.Forms.TabControl tabData;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtGasData;
        private System.Windows.Forms.Button btnSaveToImage;
    }
}

