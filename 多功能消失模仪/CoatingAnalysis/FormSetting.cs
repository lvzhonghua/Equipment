using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoatingAnalysis
{
    public partial class FormSetting : Form
    {
        public string PersonName
        {
            get
            {
                return this.TestPersonTB.Text.ToString();
            }
        }

        public string TestFrom
        {
            get
            {
                return this.TestFromTB.Text.ToString();
            }
        }

        public DateTime TestTime
        {
            get
            {
                return this.dateTimePicker.Value;
            }
        }

        public int TestNum
        {
            get
            {
                return Convert.ToInt32(this.TestNumTB.Text.ToString());
            }
        }

        public float TestDepth
        {
            get
            {
                return (float)Convert.ToDouble(this.TestDepthTB.Text);
            }
        }

        public string TestMaterial
        {
            get
            {
                return this.TestNameTB.Text.ToString();
            }
        }
        public FormSetting()
        {
            InitializeComponent();
        }

        private void settingCertainBtn_Click(object sender, EventArgs e)
        {
            #region 确定名称
            if (this.TestPersonTB.Text.Length > 4)
            {
                MessageBox.Show("姓名应小于4个字", "提示");
                return;
            }
            else if (TestNumTB.Text.Trim() == "")
            {
                MessageBox.Show("姓名应小于4个字", "提示");
                return;
            }
            #endregion

            #region 确定编号大于0并且小于10000
            try
            {
                if (Convert.ToInt32(this.TestNumTB.Text.Trim()) > 99999999 || Convert.ToInt32(this.TestNumTB.Text.Trim()) <= 0)
                {
                    MessageBox.Show("试样编号应大于0并不大于8位数", "提示");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请输入正确的数字", "提示");
            }
            #endregion

            #region 确定试样厚度应大于0mm并小于20mm
            try
            {
                if (Convert.ToDouble(this.TestDepthTB.Text.Trim()) > 20 || Convert.ToInt32(this.TestNumTB.Text.Trim()) < 0)
                {
                    MessageBox.Show("试样编号应大于0mm并小于20mm", "提示");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请输入正确的数字", "提示");
            }
            #endregion

            #region 确定涂料名称
            if (this.TestNameTB.Text.Length > 12)
            {
                MessageBox.Show("涂料名称应小于12个字", "提示");
                return;
            }
            else if (TestNameTB.Text.Trim() == "")
            {
                MessageBox.Show("请输入涂料名称", "提示");
                return;
            }
            #endregion

            #region 确定涂料来源
            if (this.TestFromTB.Text.Length > 12)
            {
                MessageBox.Show("涂料来源应小于12个字", "提示");
                return;
            }
            else if (TestFromTB.Text.Trim() == "")
            {
                MessageBox.Show("请输入涂料来源", "提示");
                return;
            }
            #endregion

            this.DialogResult = DialogResult.OK;
        }

        private void settingCancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
