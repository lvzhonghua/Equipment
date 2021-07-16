
namespace ResinSandPyrometer
{
    partial class FormCalcForce
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtBytes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSensorMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSensorMV = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSensibility = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRevise = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtForce = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCalc = new System.Windows.Forms.Button();
            this.lblVoltage = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "原始字节数据：";
            // 
            // txtBytes
            // 
            this.txtBytes.Location = new System.Drawing.Point(114, 14);
            this.txtBytes.Name = "txtBytes";
            this.txtBytes.Size = new System.Drawing.Size(139, 21);
            this.txtBytes.TabIndex = 1;
            this.txtBytes.Text = "09-28-70";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "传感器最大量程：";
            // 
            // txtSensorMax
            // 
            this.txtSensorMax.Location = new System.Drawing.Point(114, 41);
            this.txtSensorMax.Name = "txtSensorMax";
            this.txtSensorMax.Size = new System.Drawing.Size(139, 21);
            this.txtSensorMax.TabIndex = 1;
            this.txtSensorMax.Text = "200";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "供桥电压：";
            // 
            // txtSensorMV
            // 
            this.txtSensorMV.Location = new System.Drawing.Point(114, 68);
            this.txtSensorMV.Name = "txtSensorMV";
            this.txtSensorMV.Size = new System.Drawing.Size(139, 21);
            this.txtSensorMV.TabIndex = 1;
            this.txtSensorMV.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "传感器灵敏度：";
            // 
            // txtSensibility
            // 
            this.txtSensibility.Location = new System.Drawing.Point(114, 95);
            this.txtSensibility.Name = "txtSensibility";
            this.txtSensibility.Size = new System.Drawing.Size(139, 21);
            this.txtSensibility.TabIndex = 1;
            this.txtSensibility.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "校正系数：";
            // 
            // txtRevise
            // 
            this.txtRevise.Location = new System.Drawing.Point(114, 122);
            this.txtRevise.Name = "txtRevise";
            this.txtRevise.Size = new System.Drawing.Size(139, 21);
            this.txtRevise.TabIndex = 1;
            this.txtRevise.Text = "1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(290, 122);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "力量：";
            // 
            // txtForce
            // 
            this.txtForce.Location = new System.Drawing.Point(337, 119);
            this.txtForce.Name = "txtForce";
            this.txtForce.ReadOnly = true;
            this.txtForce.Size = new System.Drawing.Size(161, 21);
            this.txtForce.TabIndex = 1;
            this.txtForce.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(504, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "(牛)";
            // 
            // btnCalc
            // 
            this.btnCalc.Location = new System.Drawing.Point(292, 44);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(39, 23);
            this.btnCalc.TabIndex = 2;
            this.btnCalc.Text = "换算";
            this.btnCalc.UseVisualStyleBackColor = true;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // lblVoltage
            // 
            this.lblVoltage.AutoSize = true;
            this.lblVoltage.Location = new System.Drawing.Point(290, 23);
            this.lblVoltage.Name = "lblVoltage";
            this.lblVoltage.Size = new System.Drawing.Size(0, 12);
            this.lblVoltage.TabIndex = 3;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(337, 92);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.ReadOnly = true;
            this.txtWeight.Size = new System.Drawing.Size(113, 21);
            this.txtWeight.TabIndex = 6;
            this.txtWeight.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(456, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 4;
            this.label8.Text = "(公斤)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(290, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 5;
            this.label9.Text = "重量：";
            // 
            // FormCalcForce
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 156);
            this.Controls.Add(this.txtWeight);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblVoltage);
            this.Controls.Add(this.btnCalc);
            this.Controls.Add(this.txtForce);
            this.Controls.Add(this.txtRevise);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSensibility);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSensorMV);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSensorMax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBytes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCalcForce";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "压力传感器-力计算";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBytes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSensorMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSensorMV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSensibility;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRevise;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtForce;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label lblVoltage;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}