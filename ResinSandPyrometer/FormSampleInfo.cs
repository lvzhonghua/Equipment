using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResinSandPyrometer
{
    public partial class FormSampleInfo : Form
    {
        public FormSampleInfo()
        {
            InitializeComponent();
        }

        private void FormSpecimenSetting_Load(object sender, EventArgs e)
        {
            this.txtPersonName.Text = Setting.ExperimentPerson;
            this.txtUnit.Text = Setting.ExperimentUnit;
            this.txtNum.Text = Setting.SpecimenNum;
            this.txtMat.Text = Setting.SpecimenName;
            this.txtSpecimenDiameter.Text = Setting.SpecimenDiameter.ToString();
            this.txtSpecimenHeight.Text = Setting.SpecimenHeight.ToString();                                                                                                                                        
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Setting.SaveSpecimenSetting(this.txtPersonName.Text,
                                        this.txtUnit.Text,
                                        this.txtNum.Text,
                                        this.txtMat.Text,
                                        this.txtSpecimenDiameter.Text,
                                        this.txtSpecimenHeight.Text,
                                        this.TxtReNum.Text);

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void TxtBox_KeyPress(object sender, KeyPressEventArgs e)
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
