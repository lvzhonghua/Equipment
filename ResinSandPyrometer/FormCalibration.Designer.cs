namespace ResinSandPyrometer
{
    partial class FormCalibration
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnWeight = new System.Windows.Forms.Button();
            this.txtRevise = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lstInfo = new System.Windows.Forms.ListBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblPressZero = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStandardWeigth = new System.Windows.Forms.TextBox();
            this.lblWeight = new System.Windows.Forms.Label();
            this.btnCalibrate = new System.Windows.Forms.Button();
            this.lblCalibrationWeight = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(187, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 27);
            this.label2.TabIndex = 17;
            this.label2.Text = "Kg";
            // 
            // btnWeight
            // 
            this.btnWeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWeight.Font = new System.Drawing.Font("宋体", 11F);
            this.btnWeight.Location = new System.Drawing.Point(605, 61);
            this.btnWeight.Name = "btnWeight";
            this.btnWeight.Size = new System.Drawing.Size(108, 42);
            this.btnWeight.TabIndex = 16;
            this.btnWeight.Text = "砝码称重";
            this.btnWeight.UseVisualStyleBackColor = true;
            this.btnWeight.Click += new System.EventHandler(this.btnWeight_Click);
            // 
            // txtRevise
            // 
            this.txtRevise.Font = new System.Drawing.Font("宋体", 14F);
            this.txtRevise.Location = new System.Drawing.Point(193, 256);
            this.txtRevise.Name = "txtRevise";
            this.txtRevise.Size = new System.Drawing.Size(102, 29);
            this.txtRevise.TabIndex = 14;
            this.txtRevise.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F);
            this.label1.Location = new System.Drawing.Point(189, 234);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "校正系数:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Font = new System.Drawing.Font("宋体", 11F);
            this.btnOK.Location = new System.Drawing.Point(605, 249);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(108, 41);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "关闭";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Font = new System.Drawing.Font("宋体", 11F);
            this.btnClear.Location = new System.Drawing.Point(605, 11);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 44);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "计算皮重";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lstInfo
            // 
            this.lstInfo.Font = new System.Drawing.Font("宋体", 16F);
            this.lstInfo.FormattingEnabled = true;
            this.lstInfo.IntegralHeight = false;
            this.lstInfo.ItemHeight = 21;
            this.lstInfo.Location = new System.Drawing.Point(8, 11);
            this.lstInfo.Name = "lstInfo";
            this.lstInfo.Size = new System.Drawing.Size(173, 275);
            this.lstInfo.TabIndex = 10;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMessage.Location = new System.Drawing.Point(0, 298);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(725, 21);
            this.lblMessage.TabIndex = 18;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPressZero
            // 
            this.lblPressZero.AutoSize = true;
            this.lblPressZero.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPressZero.ForeColor = System.Drawing.Color.Blue;
            this.lblPressZero.Location = new System.Drawing.Point(189, 51);
            this.lblPressZero.Name = "lblPressZero";
            this.lblPressZero.Size = new System.Drawing.Size(144, 19);
            this.lblPressZero.TabIndex = 19;
            this.lblPressZero.Text = "空载重量：0 Kg";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 14F);
            this.label3.Location = new System.Drawing.Point(189, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 19);
            this.label3.TabIndex = 13;
            this.label3.Text = "砝码重量(Kg)：";
            // 
            // txtStandardWeigth
            // 
            this.txtStandardWeigth.Font = new System.Drawing.Font("宋体", 14F);
            this.txtStandardWeigth.Location = new System.Drawing.Point(193, 195);
            this.txtStandardWeigth.Name = "txtStandardWeigth";
            this.txtStandardWeigth.Size = new System.Drawing.Size(102, 29);
            this.txtStandardWeigth.TabIndex = 14;
            this.txtStandardWeigth.Text = "1";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWeight.ForeColor = System.Drawing.Color.Red;
            this.lblWeight.Location = new System.Drawing.Point(189, 80);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(144, 19);
            this.lblWeight.TabIndex = 19;
            this.lblWeight.Text = "实测重量：0 Kg";
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalibrate.Font = new System.Drawing.Font("宋体", 11F);
            this.btnCalibrate.Location = new System.Drawing.Point(605, 200);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(108, 42);
            this.btnCalibrate.TabIndex = 16;
            this.btnCalibrate.Text = "校正";
            this.btnCalibrate.UseVisualStyleBackColor = true;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // lblCalibrationWeight
            // 
            this.lblCalibrationWeight.AutoSize = true;
            this.lblCalibrationWeight.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCalibrationWeight.ForeColor = System.Drawing.Color.Red;
            this.lblCalibrationWeight.Location = new System.Drawing.Point(189, 111);
            this.lblCalibrationWeight.Name = "lblCalibrationWeight";
            this.lblCalibrationWeight.Size = new System.Drawing.Size(144, 19);
            this.lblCalibrationWeight.TabIndex = 19;
            this.lblCalibrationWeight.Text = "校正重量：0 Kg";
            // 
            // FormCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 319);
            this.Controls.Add(this.lblCalibrationWeight);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.lblPressZero);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCalibrate);
            this.Controls.Add(this.btnWeight);
            this.Controls.Add(this.txtStandardWeigth);
            this.Controls.Add(this.txtRevise);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lstInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCalibration";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCalibration_FormClosing);
            this.Load += new System.EventHandler(this.FormCalibration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnWeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox lstInfo;
        private System.Windows.Forms.TextBox txtRevise;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblPressZero;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStandardWeigth;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Button btnCalibrate;
        private System.Windows.Forms.Label lblCalibrationWeight;
    }
}