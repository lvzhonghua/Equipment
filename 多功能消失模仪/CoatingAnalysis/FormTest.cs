using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;

namespace CoatingAnalysis
{
    public partial class FormTest : Form
    {
        private SerialPort serialPort_Dan = null;

        private byte[] open1 = new byte[3];
        private byte[] open2 = new byte[3];
        private byte[] open3 = new byte[3];
        private byte[] open4 = new byte[3];
        private byte[] open5 = new byte[3];
        private byte[] open6 = new byte[3];
        private byte[] open7 = new byte[3];
        private byte[] close1 = new byte[3];
        private byte[] close2 = new byte[3];
        private byte[] close3 = new byte[3];
        private byte[] close4 = new byte[3];
        public FormTest()
        {
            InitializeComponent();
        }
        public FormTest(SerialPort serialPort_Dan)
        {
            InitializeComponent();
            this.serialPort_Dan = serialPort_Dan;
        }


        private void btnOpen1_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(open1, 0, open1.Length);
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void btnOpen2_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(open2, 0, open2.Length);
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void btnOpen3_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(open3, 0, open3.Length);
        }

        private void btnClose3_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            /*打开阀1*/
            BytesOperator.AppendBuffer(this.open1, Encoding.Default.GetBytes("#3"), 0);
            this.open1[2] = 0x0E;

            /*打开阀2*/
            BytesOperator.AppendBuffer(this.open2, Encoding.Default.GetBytes("#3"), 0);
            this.open2[2] = 0x0D;

            /*打开阀3*/
            BytesOperator.AppendBuffer(this.open3, Encoding.Default.GetBytes("#3"), 0);
            this.open3[2] = 0x0B;

            /*打开阀3+空压机*/
            BytesOperator.AppendBuffer(this.open4, Encoding.Default.GetBytes("#3"), 0);
            this.open4[2] = 0x03;

            /*排气测试*/
            BytesOperator.AppendBuffer(this.open5, Encoding.Default.GetBytes("#3"), 0);
            this.open5[2] = 0x0c;

            /*通气测试*/
            BytesOperator.AppendBuffer(this.open6, Encoding.Default.GetBytes("#3"), 0);
            this.open6[2] = 0x01;

            /*精度测试阀1+阀2*/
            BytesOperator.AppendBuffer(this.open7, Encoding.Default.GetBytes("#3"), 0);
            this.open7[2] = 0x0C;

            /*关闭阀1*/
            BytesOperator.AppendBuffer(this.close1, Encoding.Default.GetBytes("#3"), 0);
            this.close1[2] = 0x0F;

            /*关闭阀2*/
            BytesOperator.AppendBuffer(this.close2, Encoding.Default.GetBytes("#3"), 0);
            this.close2[2] = 0x0F;

            /*关闭阀3*/
            BytesOperator.AppendBuffer(this.close3, Encoding.Default.GetBytes("#3"), 0);
            this.close3[2] = 0x0F;
        }

        private void FormTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void btnOpen4_Click(object sender, EventArgs e)
        {
            DialogResult dalResult = MessageBox.Show("开启前确保空压机与所有阀门断开!", "警告!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (dalResult == DialogResult.Yes)
            {
                this.serialPort_Dan.Write(open4, 0, open4.Length);
            }
        }

        private void btnClose4_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void btnVent_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(open5, 0, open5.Length);
        }

        private void btnCloseVent_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void btnVenting_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(open6, 0, open6.Length);
        }

        private void btnCloseVenting_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(open7, 0, open7.Length);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.serialPort_Dan.Write(close1, 0, close1.Length);
        }

        
        
    }
}
