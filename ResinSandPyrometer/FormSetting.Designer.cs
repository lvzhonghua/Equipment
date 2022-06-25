namespace ResinSandPyrometer
{
    partial class FormSetting
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
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboxPreloadedForce = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboxSaveTemTime = new System.Windows.Forms.ComboBox();
            this.cboxFurnaceLiftingSpeed = new System.Windows.Forms.ComboBox();
            this.cboxFallingDistance = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboxPreloadedPressure = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMotorIdlePath = new System.Windows.Forms.TextBox();
            this.txtDisplacementMotorIdlePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxMotorLoadingSpeed = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(149, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "预载荷压强(MPa):";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(6, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 21);
            this.label11.TabIndex = 8;
            this.label11.Text = "预载荷力(N):";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.cboxPreloadedForce);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.ForeColor = System.Drawing.Color.Red;
            this.groupBox4.Location = new System.Drawing.Point(241, 179);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(206, 64);
            this.groupBox4.TabIndex = 63;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "高温膨胀力试验参数：";
            // 
            // cboxPreloadedForce
            // 
            this.cboxPreloadedForce.Font = new System.Drawing.Font("宋体", 12F);
            this.cboxPreloadedForce.FormattingEnabled = true;
            this.cboxPreloadedForce.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14"});
            this.cboxPreloadedForce.Location = new System.Drawing.Point(137, 30);
            this.cboxPreloadedForce.Name = "cboxPreloadedForce";
            this.cboxPreloadedForce.Size = new System.Drawing.Size(63, 24);
            this.cboxPreloadedForce.TabIndex = 45;
            this.cboxPreloadedForce.Text = "5";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 21);
            this.label4.TabIndex = 8;
            this.label4.Text = "保温时间(s):";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.cboxSaveTemTime);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.Red;
            this.groupBox3.Location = new System.Drawing.Point(15, 179);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(220, 64);
            this.groupBox3.TabIndex = 62;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "抗压强度试验参数：";
            // 
            // cboxSaveTemTime
            // 
            this.cboxSaveTemTime.Font = new System.Drawing.Font("宋体", 12F);
            this.cboxSaveTemTime.FormattingEnabled = true;
            this.cboxSaveTemTime.Items.AddRange(new object[] {
            "60",
            "90",
            "120",
            "150",
            "180",
            "210",
            "240",
            "270",
            "300",
            "330",
            "360",
            "390",
            "420",
            "450",
            "480",
            "510",
            "540",
            "570",
            "600"});
            this.cboxSaveTemTime.Location = new System.Drawing.Point(139, 30);
            this.cboxSaveTemTime.Name = "cboxSaveTemTime";
            this.cboxSaveTemTime.Size = new System.Drawing.Size(65, 24);
            this.cboxSaveTemTime.TabIndex = 42;
            this.cboxSaveTemTime.Text = "300";
            // 
            // cboxFurnaceLiftingSpeed
            // 
            this.cboxFurnaceLiftingSpeed.Font = new System.Drawing.Font("宋体", 12F);
            this.cboxFurnaceLiftingSpeed.FormattingEnabled = true;
            this.cboxFurnaceLiftingSpeed.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80",
            "90",
            "100"});
            this.cboxFurnaceLiftingSpeed.Location = new System.Drawing.Point(231, 30);
            this.cboxFurnaceLiftingSpeed.Name = "cboxFurnaceLiftingSpeed";
            this.cboxFurnaceLiftingSpeed.Size = new System.Drawing.Size(63, 24);
            this.cboxFurnaceLiftingSpeed.TabIndex = 42;
            this.cboxFurnaceLiftingSpeed.Text = "10";
            // 
            // cboxFallingDistance
            // 
            this.cboxFallingDistance.Font = new System.Drawing.Font("宋体", 12F);
            this.cboxFallingDistance.FormattingEnabled = true;
            this.cboxFallingDistance.Items.AddRange(new object[] {
            "150",
            "152",
            "154",
            "156",
            "158",
            "160",
            "162",
            "164",
            "166",
            "168"});
            this.cboxFallingDistance.Location = new System.Drawing.Point(230, 67);
            this.cboxFallingDistance.Name = "cboxFallingDistance";
            this.cboxFallingDistance.Size = new System.Drawing.Size(63, 24);
            this.cboxFallingDistance.TabIndex = 41;
            this.cboxFallingDistance.Text = "165";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 21);
            this.label3.TabIndex = 8;
            this.label3.Text = "加热炉升降速度(mm/min):";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(6, 67);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(219, 30);
            this.label13.TabIndex = 34;
            this.label13.Text = "加热炉设定下降距离（mm):";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.cboxPreloadedPressure);
            this.groupBox5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox5.ForeColor = System.Drawing.Color.Red;
            this.groupBox5.Location = new System.Drawing.Point(453, 179);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(237, 64);
            this.groupBox5.TabIndex = 64;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "条件热稳定性试验参数：";
            // 
            // cboxPreloadedPressure
            // 
            this.cboxPreloadedPressure.Font = new System.Drawing.Font("宋体", 12F);
            this.cboxPreloadedPressure.FormattingEnabled = true;
            this.cboxPreloadedPressure.Items.AddRange(new object[] {
            "0.05",
            "0.1",
            "0.15",
            "0.2",
            "0.25",
            "0.3",
            "0.35",
            "0.4"});
            this.cboxPreloadedPressure.Location = new System.Drawing.Point(161, 30);
            this.cboxPreloadedPressure.Name = "cboxPreloadedPressure";
            this.cboxPreloadedPressure.Size = new System.Drawing.Size(65, 24);
            this.cboxPreloadedPressure.TabIndex = 48;
            this.cboxPreloadedPressure.Text = "0.05";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cboxFurnaceLiftingSpeed);
            this.groupBox2.Controls.Add(this.cboxFallingDistance);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Red;
            this.groupBox2.Location = new System.Drawing.Point(388, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 111);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "加热炉参数：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMotorIdlePath);
            this.groupBox1.Controls.Add(this.txtDisplacementMotorIdlePath);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cboxMotorLoadingSpeed);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point(15, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 141);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "电机参数：";
            // 
            // txtMotorIdlePath
            // 
            this.txtMotorIdlePath.Location = new System.Drawing.Point(212, 30);
            this.txtMotorIdlePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtMotorIdlePath.Name = "txtMotorIdlePath";
            this.txtMotorIdlePath.Size = new System.Drawing.Size(65, 26);
            this.txtMotorIdlePath.TabIndex = 37;
            this.txtMotorIdlePath.Text = "8";
            this.txtMotorIdlePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_KeyPress);
            // 
            // txtDisplacementMotorIdlePath
            // 
            this.txtDisplacementMotorIdlePath.Location = new System.Drawing.Point(212, 101);
            this.txtDisplacementMotorIdlePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtDisplacementMotorIdlePath.Name = "txtDisplacementMotorIdlePath";
            this.txtDisplacementMotorIdlePath.Size = new System.Drawing.Size(65, 26);
            this.txtDisplacementMotorIdlePath.TabIndex = 36;
            this.txtDisplacementMotorIdlePath.Text = "2.5";
            this.txtDisplacementMotorIdlePath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_KeyPress);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 21);
            this.label2.TabIndex = 8;
            this.label2.Text = "电机空载行程（mm):";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(5, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 30);
            this.label6.TabIndex = 34;
            this.label6.Text = "膨胀率空载行程（mm):";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 30);
            this.label1.TabIndex = 34;
            this.label1.Text = "电机加载速度（mm/min):";
            // 
            // cboxMotorLoadingSpeed
            // 
            this.cboxMotorLoadingSpeed.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMotorLoadingSpeed.FormattingEnabled = true;
            this.cboxMotorLoadingSpeed.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboxMotorLoadingSpeed.Location = new System.Drawing.Point(212, 67);
            this.cboxMotorLoadingSpeed.Name = "cboxMotorLoadingSpeed";
            this.cboxMotorLoadingSpeed.Size = new System.Drawing.Size(65, 24);
            this.cboxMotorLoadingSpeed.TabIndex = 35;
            this.cboxMotorLoadingSpeed.Text = "5";
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("宋体", 12F);
            this.btnCancel.Location = new System.Drawing.Point(574, 261);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 32);
            this.btnCancel.TabIndex = 57;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 12F);
            this.btnOK.Location = new System.Drawing.Point(453, 261);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 32);
            this.btnOK.TabIndex = 58;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 303);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "参数设定";
            this.Load += new System.EventHandler(this.FormParameterSetting_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.ComboBox cboxPreloadedForce;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.ComboBox cboxSaveTemTime;
        public System.Windows.Forms.ComboBox cboxFurnaceLiftingSpeed;
        public System.Windows.Forms.ComboBox cboxFallingDistance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.ComboBox cboxPreloadedPressure;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cboxMotorLoadingSpeed;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDisplacementMotorIdlePath;
        private System.Windows.Forms.TextBox txtMotorIdlePath;
    }
}