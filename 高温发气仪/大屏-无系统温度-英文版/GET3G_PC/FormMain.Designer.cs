namespace GET3G_PC
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panelT = new System.Windows.Forms.Panel();
            this.temperatureCurveCtrl = new GET3G_PC.TemperatureCurveCtrl();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBoxSystemTemprature = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblCurrentRoomTemprature = new System.Windows.Forms.Label();
            this.groupBoxFurnaceTemprature = new System.Windows.Forms.GroupBox();
            this.lblTargetFurnaceTemprature = new System.Windows.Forms.Label();
            this.lblCurrentFurnaceTemprature = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnChangeTemperature = new System.Windows.Forms.Button();
            this.panCustom = new System.Windows.Forms.Panel();
            this.btnCustom_OK = new System.Windows.Forms.Button();
            this.txtCustom = new System.Windows.Forms.TextBox();
            this.btnCustom = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBoxSetting = new System.Windows.Forms.GroupBox();
            this.txtPeople = new System.Windows.Forms.TextBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.txtFactory = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtSampleNumber = new System.Windows.Forms.TextBox();
            this.txtSampleName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtSampleWeight = new System.Windows.Forms.TextBox();
            this.txtRepeat = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnStartGD = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelG = new System.Windows.Forms.Panel();
            this.gasCurveCtrl = new GET3G_PC.Controls.GasCurveCtrl();
            this.btnDrop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnEndGD = new System.Windows.Forms.Button();
            this.groupBoxFT = new System.Windows.Forms.GroupBox();
            this.lblFT = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBoxST = new System.Windows.Forms.GroupBox();
            this.lblST = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBoxGas = new System.Windows.Forms.GroupBox();
            this.lblGas = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            this.timerDelay = new System.Windows.Forms.Timer(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelT.SuspendLayout();
            this.groupBoxSystemTemprature.SuspendLayout();
            this.groupBoxFurnaceTemprature.SuspendLayout();
            this.panCustom.SuspendLayout();
            this.groupBoxSetting.SuspendLayout();
            this.panelG.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxFT.SuspendLayout();
            this.groupBoxST.SuspendLayout();
            this.groupBoxGas.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelT
            // 
            this.panelT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelT.Controls.Add(this.temperatureCurveCtrl);
            this.panelT.Controls.Add(this.btnLoad);
            this.panelT.Controls.Add(this.btnExit);
            this.panelT.Controls.Add(this.groupBoxSystemTemprature);
            this.panelT.Controls.Add(this.groupBoxFurnaceTemprature);
            this.panelT.Controls.Add(this.groupBoxSetting);
            this.panelT.Controls.Add(this.btnStartGD);
            this.panelT.Controls.Add(this.btnStart);
            this.panelT.Location = new System.Drawing.Point(0, 0);
            this.panelT.Name = "panelT";
            this.panelT.Size = new System.Drawing.Size(1360, 685);
            this.panelT.TabIndex = 0;
            // 
            // temperatureCurveCtrl
            // 
            this.temperatureCurveCtrl.CurveName = "Furnace temperature curve";
            this.temperatureCurveCtrl.FurnaceTemperature = 0F;
            this.temperatureCurveCtrl.Location = new System.Drawing.Point(8, 239);
            this.temperatureCurveCtrl.Name = "temperatureCurveCtrl";
            this.temperatureCurveCtrl.Size = new System.Drawing.Size(814, 449);
            this.temperatureCurveCtrl.TabIndex = 28;
            this.temperatureCurveCtrl.TargetFurnaceTemperature = 850F;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoad.Location = new System.Drawing.Point(1206, 430);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(144, 65);
            this.btnLoad.TabIndex = 26;
            this.btnLoad.Text = "View data";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.Location = new System.Drawing.Point(1206, 515);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(144, 65);
            this.btnExit.TabIndex = 21;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBoxSystemTemprature
            // 
            this.groupBoxSystemTemprature.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSystemTemprature.Controls.Add(this.label4);
            this.groupBoxSystemTemprature.Controls.Add(this.lblCurrentRoomTemprature);
            this.groupBoxSystemTemprature.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxSystemTemprature.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBoxSystemTemprature.Location = new System.Drawing.Point(831, 10);
            this.groupBoxSystemTemprature.Name = "groupBoxSystemTemprature";
            this.groupBoxSystemTemprature.Size = new System.Drawing.Size(525, 220);
            this.groupBoxSystemTemprature.TabIndex = 15;
            this.groupBoxSystemTemprature.TabStop = false;
            this.groupBoxSystemTemprature.Text = "Ambient Temperature";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(256, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 64);
            this.label4.TabIndex = 3;
            this.label4.Text = "℃";
            // 
            // lblCurrentRoomTemprature
            // 
            this.lblCurrentRoomTemprature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrentRoomTemprature.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRoomTemprature.Location = new System.Drawing.Point(10, 25);
            this.lblCurrentRoomTemprature.Name = "lblCurrentRoomTemprature";
            this.lblCurrentRoomTemprature.Size = new System.Drawing.Size(246, 106);
            this.lblCurrentRoomTemprature.TabIndex = 25;
            this.lblCurrentRoomTemprature.Text = "888";
            this.lblCurrentRoomTemprature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxFurnaceTemprature
            // 
            this.groupBoxFurnaceTemprature.Controls.Add(this.lblTargetFurnaceTemprature);
            this.groupBoxFurnaceTemprature.Controls.Add(this.lblCurrentFurnaceTemprature);
            this.groupBoxFurnaceTemprature.Controls.Add(this.lblStatus);
            this.groupBoxFurnaceTemprature.Controls.Add(this.btnChangeTemperature);
            this.groupBoxFurnaceTemprature.Controls.Add(this.panCustom);
            this.groupBoxFurnaceTemprature.Controls.Add(this.btnCustom);
            this.groupBoxFurnaceTemprature.Controls.Add(this.label6);
            this.groupBoxFurnaceTemprature.Controls.Add(this.label8);
            this.groupBoxFurnaceTemprature.Controls.Add(this.label5);
            this.groupBoxFurnaceTemprature.Controls.Add(this.label7);
            this.groupBoxFurnaceTemprature.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxFurnaceTemprature.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBoxFurnaceTemprature.Location = new System.Drawing.Point(9, 9);
            this.groupBoxFurnaceTemprature.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxFurnaceTemprature.Name = "groupBoxFurnaceTemprature";
            this.groupBoxFurnaceTemprature.Padding = new System.Windows.Forms.Padding(5);
            this.groupBoxFurnaceTemprature.Size = new System.Drawing.Size(814, 222);
            this.groupBoxFurnaceTemprature.TabIndex = 16;
            this.groupBoxFurnaceTemprature.TabStop = false;
            this.groupBoxFurnaceTemprature.Text = "Furnace Temperature";
            // 
            // lblTargetFurnaceTemprature
            // 
            this.lblTargetFurnaceTemprature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTargetFurnaceTemprature.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetFurnaceTemprature.Location = new System.Drawing.Point(135, 151);
            this.lblTargetFurnaceTemprature.Name = "lblTargetFurnaceTemprature";
            this.lblTargetFurnaceTemprature.Size = new System.Drawing.Size(139, 55);
            this.lblTargetFurnaceTemprature.TabIndex = 25;
            this.lblTargetFurnaceTemprature.Text = "8888";
            this.lblTargetFurnaceTemprature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrentFurnaceTemprature
            // 
            this.lblCurrentFurnaceTemprature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrentFurnaceTemprature.Font = new System.Drawing.Font("Arial", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentFurnaceTemprature.Location = new System.Drawing.Point(135, 24);
            this.lblCurrentFurnaceTemprature.Name = "lblCurrentFurnaceTemprature";
            this.lblCurrentFurnaceTemprature.Size = new System.Drawing.Size(296, 106);
            this.lblCurrentFurnaceTemprature.TabIndex = 25;
            this.lblCurrentFurnaceTemprature.Text = "8888";
            this.lblCurrentFurnaceTemprature.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(730, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 20);
            this.lblStatus.TabIndex = 24;
            this.lblStatus.Visible = false;
            // 
            // btnChangeTemperature
            // 
            this.btnChangeTemperature.Image = global::GET3G_PC.Properties.Resources.refresh_over;
            this.btnChangeTemperature.Location = new System.Drawing.Point(329, 166);
            this.btnChangeTemperature.Name = "btnChangeTemperature";
            this.btnChangeTemperature.Size = new System.Drawing.Size(48, 48);
            this.btnChangeTemperature.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnChangeTemperature, "Save target temperature");
            this.btnChangeTemperature.UseVisualStyleBackColor = true;
            this.btnChangeTemperature.Click += new System.EventHandler(this.btnChangeTemperature_Click);
            // 
            // panCustom
            // 
            this.panCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panCustom.Controls.Add(this.btnCustom_OK);
            this.panCustom.Controls.Add(this.txtCustom);
            this.panCustom.Location = new System.Drawing.Point(437, 166);
            this.panCustom.Name = "panCustom";
            this.panCustom.Size = new System.Drawing.Size(283, 48);
            this.panCustom.TabIndex = 23;
            this.panCustom.Visible = false;
            // 
            // btnCustom_OK
            // 
            this.btnCustom_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCustom_OK.ForeColor = System.Drawing.Color.Black;
            this.btnCustom_OK.Location = new System.Drawing.Point(214, 7);
            this.btnCustom_OK.Name = "btnCustom_OK";
            this.btnCustom_OK.Size = new System.Drawing.Size(64, 32);
            this.btnCustom_OK.TabIndex = 1;
            this.btnCustom_OK.Text = "OK";
            this.btnCustom_OK.UseVisualStyleBackColor = true;
            this.btnCustom_OK.Click += new System.EventHandler(this.btnCustom_OK_Click);
            // 
            // txtCustom
            // 
            this.txtCustom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustom.Location = new System.Drawing.Point(5, 9);
            this.txtCustom.Name = "txtCustom";
            this.txtCustom.Size = new System.Drawing.Size(203, 30);
            this.txtCustom.TabIndex = 0;
            // 
            // btnCustom
            // 
            this.btnCustom.Image = global::GET3G_PC.Properties.Resources.my_docs_Bagg_s;
            this.btnCustom.Location = new System.Drawing.Point(383, 166);
            this.btnCustom.Name = "btnCustom";
            this.btnCustom.Size = new System.Drawing.Size(48, 48);
            this.btnCustom.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnCustom, "Setting target temperature");
            this.btnCustom.UseVisualStyleBackColor = true;
            this.btnCustom.Click += new System.EventHandler(this.btnCustom_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(9, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 27);
            this.label6.TabIndex = 4;
            this.label6.Text = "Setting:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(8, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 27);
            this.label8.TabIndex = 1;
            this.label8.Text = "Current:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(268, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 55);
            this.label5.TabIndex = 5;
            this.label5.Text = "℃";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(426, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 64);
            this.label7.TabIndex = 2;
            this.label7.Text = "℃";
            // 
            // groupBoxSetting
            // 
            this.groupBoxSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSetting.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxSetting.Controls.Add(this.txtPeople);
            this.groupBoxSetting.Controls.Add(this.btnSetting);
            this.groupBoxSetting.Controls.Add(this.txtFactory);
            this.groupBoxSetting.Controls.Add(this.label22);
            this.groupBoxSetting.Controls.Add(this.label21);
            this.groupBoxSetting.Controls.Add(this.label20);
            this.groupBoxSetting.Controls.Add(this.txtSampleNumber);
            this.groupBoxSetting.Controls.Add(this.txtSampleName);
            this.groupBoxSetting.Controls.Add(this.label19);
            this.groupBoxSetting.Controls.Add(this.label13);
            this.groupBoxSetting.Controls.Add(this.label14);
            this.groupBoxSetting.Controls.Add(this.txtSampleWeight);
            this.groupBoxSetting.Controls.Add(this.txtRepeat);
            this.groupBoxSetting.Controls.Add(this.label12);
            this.groupBoxSetting.Controls.Add(this.label11);
            this.groupBoxSetting.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBoxSetting.Location = new System.Drawing.Point(831, 234);
            this.groupBoxSetting.Name = "groupBoxSetting";
            this.groupBoxSetting.Size = new System.Drawing.Size(372, 444);
            this.groupBoxSetting.TabIndex = 19;
            this.groupBoxSetting.TabStop = false;
            this.groupBoxSetting.Text = "Sample parameter setting";
            // 
            // txtPeople
            // 
            this.txtPeople.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPeople.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPeople.Location = new System.Drawing.Point(118, 191);
            this.txtPeople.Name = "txtPeople";
            this.txtPeople.Size = new System.Drawing.Size(248, 31);
            this.txtPeople.TabIndex = 16;
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetting.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnSetting.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetting.Location = new System.Drawing.Point(6, 371);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(339, 65);
            this.btnSetting.TabIndex = 17;
            this.btnSetting.Text = "Modify sample";
            this.btnSetting.UseVisualStyleBackColor = false;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // txtFactory
            // 
            this.txtFactory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFactory.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFactory.Location = new System.Drawing.Point(118, 133);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(248, 31);
            this.txtFactory.TabIndex = 15;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.Location = new System.Drawing.Point(9, 194);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(94, 21);
            this.label22.TabIndex = 14;
            this.label22.Text = "Tester:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(10, 135);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(106, 21);
            this.label21.TabIndex = 13;
            this.label21.Text = "Factory:";
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(286, 263);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(58, 21);
            this.label20.TabIndex = 12;
            this.label20.Text = "Rep.";
            // 
            // txtSampleNumber
            // 
            this.txtSampleNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampleNumber.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSampleNumber.Location = new System.Drawing.Point(118, 77);
            this.txtSampleNumber.Name = "txtSampleNumber";
            this.txtSampleNumber.Size = new System.Drawing.Size(248, 31);
            this.txtSampleNumber.TabIndex = 5;
            // 
            // txtSampleName
            // 
            this.txtSampleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampleName.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSampleName.Location = new System.Drawing.Point(118, 27);
            this.txtSampleName.Name = "txtSampleName";
            this.txtSampleName.Size = new System.Drawing.Size(248, 31);
            this.txtSampleName.TabIndex = 4;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(293, 334);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(22, 21);
            this.label19.TabIndex = 11;
            this.label19.Text = "g";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(10, 77);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 21);
            this.label13.TabIndex = 1;
            this.label13.Text = "No.:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(8, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 21);
            this.label14.TabIndex = 0;
            this.label14.Text = "Name:";
            // 
            // txtSampleWeight
            // 
            this.txtSampleWeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSampleWeight.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSampleWeight.Location = new System.Drawing.Point(118, 334);
            this.txtSampleWeight.Name = "txtSampleWeight";
            this.txtSampleWeight.Size = new System.Drawing.Size(162, 31);
            this.txtSampleWeight.TabIndex = 7;
            this.txtSampleWeight.Text = "1";
            // 
            // txtRepeat
            // 
            this.txtRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRepeat.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRepeat.Location = new System.Drawing.Point(118, 260);
            this.txtRepeat.Name = "txtRepeat";
            this.txtRepeat.Size = new System.Drawing.Size(162, 31);
            this.txtRepeat.TabIndex = 6;
            this.txtRepeat.Text = "1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(9, 263);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(94, 21);
            this.label12.TabIndex = 2;
            this.label12.Text = "Repeat:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(9, 334);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 21);
            this.label11.TabIndex = 3;
            this.label11.Text = "Weight:";
            // 
            // btnStartGD
            // 
            this.btnStartGD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartGD.BackColor = System.Drawing.SystemColors.Control;
            this.btnStartGD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartGD.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStartGD.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartGD.Location = new System.Drawing.Point(1206, 339);
            this.btnStartGD.Name = "btnStartGD";
            this.btnStartGD.Size = new System.Drawing.Size(144, 65);
            this.btnStartGD.TabIndex = 17;
            this.btnStartGD.Text = "Start Testing";
            this.btnStartGD.UseVisualStyleBackColor = false;
            this.btnStartGD.Click += new System.EventHandler(this.btnStartGD_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.SystemColors.Control;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(1206, 247);
            this.btnStart.MaximumSize = new System.Drawing.Size(1024, 607);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(144, 65);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start Slave";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panelG
            // 
            this.panelG.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelG.Controls.Add(this.gasCurveCtrl);
            this.panelG.Controls.Add(this.btnDrop);
            this.panelG.Controls.Add(this.groupBox2);
            this.panelG.Controls.Add(this.btnSave);
            this.panelG.Controls.Add(this.btnPrint);
            this.panelG.Controls.Add(this.btnEndGD);
            this.panelG.Controls.Add(this.groupBoxFT);
            this.panelG.Controls.Add(this.groupBoxST);
            this.panelG.Controls.Add(this.groupBoxGas);
            this.panelG.Location = new System.Drawing.Point(2, 1);
            this.panelG.Name = "panelG";
            this.panelG.Size = new System.Drawing.Size(1357, 689);
            this.panelG.TabIndex = 22;
            this.panelG.Visible = false;
            // 
            // gasCurveCtrl
            // 
            this.gasCurveCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gasCurveCtrl.CurveName = "[Gas-Speed]Curve";
            this.gasCurveCtrl.Gas = 0F;
            this.gasCurveCtrl.GasInCrement = 0F;
            this.gasCurveCtrl.GasInCrementList = ((System.Collections.Generic.List<float>)(resources.GetObject("gasCurveCtrl.GasInCrementList")));
            this.gasCurveCtrl.GasList = ((System.Collections.Generic.List<float>)(resources.GetObject("gasCurveCtrl.GasList")));
            this.gasCurveCtrl.GasTime = ((long)(0));
            this.gasCurveCtrl.GasTimeList = ((System.Collections.Generic.List<long>)(resources.GetObject("gasCurveCtrl.GasTimeList")));
            this.gasCurveCtrl.Interval = 1;
            this.gasCurveCtrl.Location = new System.Drawing.Point(5, 1);
            this.gasCurveCtrl.Name = "gasCurveCtrl";
            this.gasCurveCtrl.Size = new System.Drawing.Size(1149, 682);
            this.gasCurveCtrl.TabIndex = 29;
            // 
            // btnDrop
            // 
            this.btnDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDrop.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDrop.Location = new System.Drawing.Point(1259, 587);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(90, 43);
            this.btnDrop.TabIndex = 28;
            this.btnDrop.Text = "Discard data";
            this.btnDrop.UseVisualStyleBackColor = true;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblTime);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(1160, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 99);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Testing Time mm:ss";
            // 
            // lblTime
            // 
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTime.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(6, 30);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(117, 55);
            this.lblTime.TabIndex = 28;
            this.lblTime.Text = "00:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(1158, 587);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 43);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save Data";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrint.Location = new System.Drawing.Point(1158, 635);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(90, 43);
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "Print Data";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnEndGD
            // 
            this.btnEndGD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEndGD.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEndGD.Location = new System.Drawing.Point(1259, 636);
            this.btnEndGD.Name = "btnEndGD";
            this.btnEndGD.Size = new System.Drawing.Size(90, 43);
            this.btnEndGD.TabIndex = 22;
            this.btnEndGD.Text = "Finish";
            this.btnEndGD.UseVisualStyleBackColor = true;
            this.btnEndGD.Click += new System.EventHandler(this.btnEndGD_Click);
            // 
            // groupBoxFT
            // 
            this.groupBoxFT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFT.Controls.Add(this.lblFT);
            this.groupBoxFT.Controls.Add(this.label9);
            this.groupBoxFT.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxFT.Location = new System.Drawing.Point(1160, 221);
            this.groupBoxFT.Name = "groupBoxFT";
            this.groupBoxFT.Size = new System.Drawing.Size(187, 100);
            this.groupBoxFT.TabIndex = 21;
            this.groupBoxFT.TabStop = false;
            this.groupBoxFT.Text = "Furnace Temperature";
            // 
            // lblFT
            // 
            this.lblFT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblFT.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFT.Location = new System.Drawing.Point(6, 31);
            this.lblFT.Name = "lblFT";
            this.lblFT.Size = new System.Drawing.Size(117, 55);
            this.lblFT.TabIndex = 29;
            this.lblFT.Text = "8888";
            this.lblFT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(129, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 27);
            this.label9.TabIndex = 3;
            this.label9.Text = "℃";
            // 
            // groupBoxST
            // 
            this.groupBoxST.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxST.Controls.Add(this.lblST);
            this.groupBoxST.Controls.Add(this.label10);
            this.groupBoxST.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxST.Location = new System.Drawing.Point(1160, 337);
            this.groupBoxST.Name = "groupBoxST";
            this.groupBoxST.Size = new System.Drawing.Size(187, 85);
            this.groupBoxST.TabIndex = 20;
            this.groupBoxST.TabStop = false;
            this.groupBoxST.Text = "System Temperature";
            this.groupBoxST.Visible = false;
            // 
            // lblST
            // 
            this.lblST.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblST.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblST.Location = new System.Drawing.Point(6, 22);
            this.lblST.Name = "lblST";
            this.lblST.Size = new System.Drawing.Size(117, 55);
            this.lblST.TabIndex = 30;
            this.lblST.Text = "888";
            this.lblST.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(131, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 29);
            this.label10.TabIndex = 4;
            this.label10.Text = "℃";
            // 
            // groupBoxGas
            // 
            this.groupBoxGas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxGas.Controls.Add(this.lblGas);
            this.groupBoxGas.Controls.Add(this.label18);
            this.groupBoxGas.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBoxGas.Location = new System.Drawing.Point(1160, 11);
            this.groupBoxGas.Name = "groupBoxGas";
            this.groupBoxGas.Size = new System.Drawing.Size(189, 93);
            this.groupBoxGas.TabIndex = 19;
            this.groupBoxGas.TabStop = false;
            this.groupBoxGas.Text = "Gas";
            // 
            // lblGas
            // 
            this.lblGas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGas.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGas.Location = new System.Drawing.Point(6, 19);
            this.lblGas.Name = "lblGas";
            this.lblGas.Size = new System.Drawing.Size(117, 55);
            this.lblGas.TabIndex = 28;
            this.lblGas.Text = "8888";
            this.lblGas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point(129, 41);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(54, 30);
            this.label18.TabIndex = 27;
            this.label18.Text = "ml/g";
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // timerClock
            // 
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            // 
            // timerDelay
            // 
            this.timerDelay.Interval = 20000;
            this.timerDelay.Tick += new System.EventHandler(this.timerDelay_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 690);
            this.ControlBox = false;
            this.Controls.Add(this.panelT);
            this.Controls.Add(this.panelG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GET-Ⅲ Intelligent gas generation tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panelT.ResumeLayout(false);
            this.groupBoxSystemTemprature.ResumeLayout(false);
            this.groupBoxSystemTemprature.PerformLayout();
            this.groupBoxFurnaceTemprature.ResumeLayout(false);
            this.groupBoxFurnaceTemprature.PerformLayout();
            this.panCustom.ResumeLayout(false);
            this.panCustom.PerformLayout();
            this.groupBoxSetting.ResumeLayout(false);
            this.groupBoxSetting.PerformLayout();
            this.panelG.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxFT.ResumeLayout(false);
            this.groupBoxST.ResumeLayout(false);
            this.groupBoxGas.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel panelT;
		private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.GroupBox groupBoxSystemTemprature;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBoxSetting;
		private System.Windows.Forms.TextBox txtSampleWeight;
		private System.Windows.Forms.TextBox txtRepeat;
		private System.Windows.Forms.TextBox txtSampleNumber;
		private System.Windows.Forms.TextBox txtSampleName;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Button btnStartGD;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Panel panelG;
		private System.Windows.Forms.Button btnEndGD;
		private System.Windows.Forms.GroupBox groupBoxFT;
        private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox groupBoxST;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBoxGas;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSave;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtPeople;
        private System.Windows.Forms.TextBox txtFactory;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Timer timerClock;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnDrop;
        private System.Windows.Forms.Timer timerDelay;
        private TemperatureCurveCtrl temperatureCurveCtrl;
        private GET3G_PC.Controls.GasCurveCtrl gasCurveCtrl;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label lblCurrentRoomTemprature;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblFT;
        private System.Windows.Forms.Label lblST;
        private System.Windows.Forms.Label lblGas;
        private System.Windows.Forms.GroupBox groupBoxFurnaceTemprature;
        private System.Windows.Forms.Label lblTargetFurnaceTemprature;
        private System.Windows.Forms.Label lblCurrentFurnaceTemprature;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnChangeTemperature;
        private System.Windows.Forms.Panel panCustom;
        private System.Windows.Forms.Button btnCustom_OK;
        private System.Windows.Forms.TextBox txtCustom;
        private System.Windows.Forms.Button btnCustom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
    }
}

