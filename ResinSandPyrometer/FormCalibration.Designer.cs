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
            this.btnZero = new System.Windows.Forms.Button();
            this.txtRevise = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCE = new System.Windows.Forms.Button();
            this.lstInfo = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(187, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 27);
            this.label2.TabIndex = 17;
            this.label2.Text = "Kg";
            // 
            // btnWeight
            // 
            this.btnWeight.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWeight.Location = new System.Drawing.Point(312, 99);
            this.btnWeight.Name = "btnWeight";
            this.btnWeight.Size = new System.Drawing.Size(88, 42);
            this.btnWeight.TabIndex = 16;
            this.btnWeight.Text = "称重";
            this.btnWeight.UseVisualStyleBackColor = true;
            this.btnWeight.Click += new System.EventHandler(this.btnWeight_Click);
            // 
            // btnZero
            // 
            this.btnZero.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZero.Location = new System.Drawing.Point(312, 1);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(88, 42);
            this.btnZero.TabIndex = 15;
            this.btnZero.Text = "取零点";
            this.btnZero.UseVisualStyleBackColor = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            // 
            // txtRevise
            // 
            this.txtRevise.Font = new System.Drawing.Font("宋体", 14F);
            this.txtRevise.Location = new System.Drawing.Point(187, 154);
            this.txtRevise.Name = "txtRevise";
            this.txtRevise.Size = new System.Drawing.Size(102, 29);
            this.txtRevise.TabIndex = 14;
            this.txtRevise.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F);
            this.label1.Location = new System.Drawing.Point(187, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "校正系数:";
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(312, 147);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(88, 41);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "关闭";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCE
            // 
            this.btnCE.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCE.Location = new System.Drawing.Point(312, 49);
            this.btnCE.Name = "btnCE";
            this.btnCE.Size = new System.Drawing.Size(88, 44);
            this.btnCE.TabIndex = 11;
            this.btnCE.Text = "清零";
            this.btnCE.UseVisualStyleBackColor = true;
            this.btnCE.Click += new System.EventHandler(this.btnCE_Click);
            // 
            // lstInfo
            // 
            this.lstInfo.Font = new System.Drawing.Font("宋体", 16F);
            this.lstInfo.FormattingEnabled = true;
            this.lstInfo.ItemHeight = 21;
            this.lstInfo.Location = new System.Drawing.Point(8, 11);
            this.lstInfo.Name = "lstInfo";
            this.lstInfo.Size = new System.Drawing.Size(173, 172);
            this.lstInfo.TabIndex = 10;
            // 
            // FormCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 194);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnWeight);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.txtRevise);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCE);
            this.Controls.Add(this.lstInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormCalibration";
            this.Text = "校验";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnWeight;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCE;
        private System.Windows.Forms.ListBox lstInfo;
        private System.Windows.Forms.TextBox txtRevise;
    }
}