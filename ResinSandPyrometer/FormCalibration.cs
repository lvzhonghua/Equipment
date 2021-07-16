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
    public partial class FormCalibration : Form
    {
        public int Revise
        {
            get { return int.Parse(this.txtRevise.Text.Trim()); }
            set { this.txtRevise.Text = value.ToString(); }
        }

        private bool istimeReached;

        public bool IstimeReached
        {
            get { return istimeReached; }
            set { istimeReached = value; }
        }

        public FormCalibration()
        {
            InitializeComponent();
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            this.TimeCount(2);
            //this.GetPressZero(formMain.result);

            //this.lstInfo.Items.Add(GetThreeValue((formMain.result)).ToString());
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            //this.GetPressZero(formMain.result);
            //this.lstInfo.Items.Clear();
            //this.lstInfo.Items.Add((formMain.result).ToString());
            //this.GetPressZero(formMain.result);
            //this.TimeCount(2);
            //this.lstInfo.Items.Add((formMain.result).ToString());

            //this.label3.Text = formMain.result.ToString();
            //this.Invoke((EventHandler)delegate
            //{
            //    float b = Convert.ToSingle((formMain.result - this.PressZero * 5) / 9.81);
            //    this.lstInfo.Items.Add(GetThreeValue(b)).ToString();
            //    if (this.lstInfo.Items.Count != 0)
            //        this.lstInfo.SelectedIndex = this.lstInfo.Items.Count - 1;
            //});
            //this.lstInfo.Items.Add((formMain.result).ToString());
        }

        private int countTime = 0;//收到指令的个数

        public void TimeCount(int timeCount)
        {
            countTime++;
            if (countTime == 5 * timeCount)
            {

                countTime = 0;
            }
        }

        //定义零点值
        private float pressZero = 0f;

        public float PressZero
        {
            get { return pressZero; }
            set { pressZero = value; }
        }
        private Queue<float> pressZeroQueue = new Queue<float>();

        //取十个值做平均值
        public void GetPressZero(float pressure)
        {
            if (this.pressZeroQueue.Count == 5)
            {
                this.pressZeroQueue.Dequeue();
            }

            this.pressZeroQueue.Enqueue(pressure);

            float[] zeroArray = pressZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.PressZero = sum / 5;

        }

        private void btnWeight_Click(object sender, EventArgs e)
        {
            //float a = Convert.ToSingle((formMain.result - this.PressZero * 5) / 9.81);
            //this.lstInfo.Items.Add(GetThreeValue(a)).ToString();
            //if (this.lstInfo.Items.Count != 0)  this.lstInfo.SelectedIndex = this.lstInfo.Items.Count - 1;
        }

        //将浮点数保留四位
        public static string GetThreeValue(float value)
        {
            string strValue = string.Format("{0:N3}", value);
            return strValue;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
