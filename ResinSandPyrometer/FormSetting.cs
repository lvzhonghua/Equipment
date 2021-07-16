using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResinSandPyrometer
{
    public partial class FormSetting : Form
    {
        public FormSetting()
        {
            InitializeComponent();
        }

        private void FormParameterSetting_Load(object sender, EventArgs e)
        {
            this.txtMotorIdlePath.Text = Setting.MotorIdlePath.ToString();
            this.cboxMotorLoadingSpeed.Text = Setting.MotorLoadingSpeed.ToString();
            this.txtDisplacementMotorIdlePath.Text = Setting.DisplacementMotorIdlePath.ToString();
            this.cboxFurnaceLiftingSpeed.Text = Setting.FurnaceLiftingSpeed.ToString();
            this.cboxFallingDistance.Text = Setting.FurnaceFallingDistance.ToString();
            this.cboxSaveTemTime.Text = Setting.SoakingTime.ToString();
            this.cboxPreloadedForce.Text = Setting.PreloadedForce.ToString();
            this.cboxPreloadedPressure.Text = Setting.PreloadedPressure.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Setting.SaveParameterSetting(this.txtMotorIdlePath.Text,
                                       this.cboxMotorLoadingSpeed.Text,
                                       this.txtDisplacementMotorIdlePath.Text,
                                       this.cboxFurnaceLiftingSpeed.Text,
                                       this.cboxFallingDistance.Text,
                                       this.cboxSaveTemTime.Text,
                                       this.cboxPreloadedForce.Text,
                                       this.cboxPreloadedPressure.Text);

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            //允许输入数字、小数点、删除键和负号  
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != (char)('.') && e.KeyChar != (char)('-'))
            {
                MessageBox.Show("请输入正确的数字");
                textBox.Text = "";
                e.Handled = true;
            }
            if (e.KeyChar == (char)('-'))
            {
                if (textBox.Text != "")
                {
                    MessageBox.Show("请输入正确的数字");
                    textBox.Text = "";
                    e.Handled = true;
                }
            }
            //小数点只能输入一次  
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text.IndexOf('.') != -1)
            {
                MessageBox.Show("请输入正确的数字");
                textBox.Text = "";
                e.Handled = true;
            }
            //第一位不能为小数点  
            if (e.KeyChar == (char)('.') && ((TextBox)sender).Text == "")
            {
                MessageBox.Show("请输入正确的数字");
                textBox.Text = "";
                e.Handled = true;
            }
            //第一位是0，第二位必须为小数点  
            if (e.KeyChar != (char)('.') && ((TextBox)sender).Text == "0")
            {
                MessageBox.Show("请输入正确的数字");
                textBox.Text = "";
                e.Handled = true;
            }
            //第一位是负号，第二位不能为小数点  
            if (((TextBox)sender).Text == "-" && e.KeyChar == (char)('.'))
            {
                MessageBox.Show("请输入正确的数字");
                textBox.Text = "";
                e.Handled = true;
            }
        }
    }
}
