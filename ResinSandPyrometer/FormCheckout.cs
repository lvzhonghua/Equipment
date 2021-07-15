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
    public partial class FormCheckout : Form
    {
        private FormMain formMain;
        private bool istimeReached;

        public bool IstimeReached
        {
            get { return istimeReached; }
            set { istimeReached = value; }
        }

        public FormCheckout()
        {
            InitializeComponent();
        }


        public FormCheckout(FormMain formMain)
        {
            InitializeComponent();
            this.formMain = formMain;

            //    this.TimeCount(2);
            //    this.GetPressZero(formMain.result);


            ////this.TimeCount(10);
            ////this.GetPressZero(formMain.result);


            //this.lstInfo.Items.Add((formMain.result).ToString());
            //if (this.lstInfo.Items.Count != 0)
            //    this.lstInfo.SelectedIndex = this.lstInfo.Items.Count - 1;

        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            this.TimeCount(2);
            this.GetPressZero(formMain.result);

            this.lstInfo.Items.Add(GetThreeValue((formMain.result)).ToString());
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            //this.GetPressZero(formMain.result);
            this.lstInfo.Items.Clear();
            //this.lstInfo.Items.Add((formMain.result).ToString());
            //this.GetPressZero(formMain.result);
            //this.TimeCount(2);
            //this.lstInfo.Items.Add((formMain.result).ToString());

            //this.label3.Text = formMain.result.ToString();
            this.Invoke((EventHandler)delegate
            {
                float b = Convert.ToSingle((formMain.result - this.PressZero * 5) / 9.81);
                this.lstInfo.Items.Add(GetThreeValue(b)).ToString();
                if (this.lstInfo.Items.Count != 0)
                    this.lstInfo.SelectedIndex = this.lstInfo.Items.Count - 1;
            });
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
        private Queue<float> PPressZeroQueue = new Queue<float>();

        //取十个值做平均值
        public void GetPressZero(float pressure)
        {
            if (this.PPressZeroQueue.Count == 5)
            {
                this.PPressZeroQueue.Dequeue();
            }
            this.PPressZeroQueue.Enqueue(pressure);

            float[] zeroArray = PPressZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.PressZero = sum / 5;

        }


        private void btnWeight_Click(object sender, EventArgs e)
        {
            this.Invoke((EventHandler)delegate
            {
                float a = Convert.ToSingle((formMain.result - this.PressZero * 5) / 9.81);
                this.lstInfo.Items.Add(GetThreeValue(a)).ToString();
                if (this.lstInfo.Items.Count != 0)
                    this.lstInfo.SelectedIndex = this.lstInfo.Items.Count - 1;
            });
        }

        //将浮点数保留四位
        public static string GetThreeValue(float value)
        {
            string str1 = string.Format("{0:N3}", value);
            return str1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
