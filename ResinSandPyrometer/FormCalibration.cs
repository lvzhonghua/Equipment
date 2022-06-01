using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace ResinSandPyrometer
{
    public partial class FormCalibration : Form
    {
        private string work = "计算皮重";

        private SerialPort serialPort;

        private Queue<float> pressZeroQueue = new Queue<float>();

        private Queue<float> weightQueue = new Queue<float>();

        private float revise = 1f;
        public float Revise
        {
            get { return this.revise; }
            set 
            {
                this.revise = value;
                this.txtRevise.Text = this.revise.ToString();
            }
        }
        //皮重
        private float pressZero = 0f;

        public float PressZero
        {
            get { return this.pressZero; }
            set { this.pressZero = value; }
        }

        private float weight = 0;

        public float Weight 
        {
            get { return this.weight; }
            set { this.weight = value; }
        }


        public FormCalibration(SerialPort serialPort)
        {
            InitializeComponent();

            this.serialPort = serialPort;
            this.serialPort.DataReceived += this.SerialPort_DataReceived;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.work = "计算皮重";

            if (this.pressZeroQueue.Count < 10) return;

            float[] array = this.pressZeroQueue.ToArray();

            this.CountPressZero();

            this.lstInfo.Items.Clear();
            foreach (var item in array)
            {
                this.lstInfo.Items.Add($"{item:0.000}");
            }

            this.lblPressZero.Text = $"空载重量：{this.pressZero:0.000} Kg";
        }

        //取十个值做平均值
        public void CountPressZero()
        {
            float[] zeroArray = this.pressZeroQueue.ToArray();

            float sum = 0;

            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }

            this.pressZero = sum / zeroArray.Length;
        }

        //取十个值做平均值
        public void CountWeight()
        {
            float[] weigthArray = this.weightQueue.ToArray();

            float sum = 0;

            for (int i = 0; i < weigthArray.Length; i++)
            {
                sum += weigthArray[i];
            }

            this.weight = sum / weigthArray.Length;
        }

        private void btnWeight_Click(object sender, EventArgs e)
        {
            this.work = "砝码称重";

            if (this.weightQueue.Count < 10)
            {
                MessageBox.Show("取十次平均，数据不足，稍等后再次点击该按钮","提示",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            float[] array = this.weightQueue.ToArray();

            this.CountWeight();

            this.lstInfo.Items.Clear();
            foreach (var item in array)
            {
                this.lstInfo.Items.Add($"{item:0.000}");
            }

            this.lblWeight.Text = $"实测重量：{this.weight:0.000} Kg";

            this.lblCalibrationWeight.Text = $"校正重量：{this.weight * this.revise:0.000} Kg";
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

        private void FormCalibration_Load(object sender, EventArgs e)
        {
            Common.Setting.Load();

            this.Revise = Common.Setting.TxtRevise;

            //下位机复位指令
            CommandAndReply.Command command_Reset = CommandAndReply.CommandGenerator.Generate_Reset();
            CommandAndReply.CommandExecutor.Send(this.serialPort, command_Reset);

            Thread.Sleep(2000);

            //设置空载行程（默认2.5毫米）
            CommandAndReply.Command command_MotorDistanceSetting = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(2.5f);
            CommandAndReply.CommandExecutor.Send(this.serialPort, command_MotorDistanceSetting);

            Thread.Sleep(50);

            //发送开始测试指令
            CommandAndReply.Command command_BeginTest = CommandAndReply.CommandGenerator.Generate_BeginTest();
            CommandAndReply.CommandExecutor.Send(this.serialPort, command_BeginTest);
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);

            //获取串口传递过来数据（从串口读数据）
            byte[] buffer = new byte[5];
            try
            {
                if (!this.serialPort.IsOpen) return;
                this.serialPort.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(()=> 
                {
                    this.lblMessage.Text = $"{DateTime.Now.ToLongTimeString()}：读取数据异常：{ex.Message}";
                }));
                
                return;
            }

            try
            {
                CommandAndReply.Slave_Reply reply = CommandAndReply.Slave_ReplyAnalyzer.Analyse(buffer);

                switch (reply.Code)
                {
                    case CommandAndReply.Slave_ReplyCode.Answer:
                    case CommandAndReply.Slave_ReplyCode.R:
                    case CommandAndReply.Slave_ReplyCode.C:
                    case CommandAndReply.Slave_ReplyCode.O: //力传感器通道正常
                    case CommandAndReply.Slave_ReplyCode.K: //加载电机到下限位
                    case CommandAndReply.Slave_ReplyCode.L:  //加载电机到下限位（零位）
                    case CommandAndReply.Slave_ReplyCode.P: //加载电机到达预设行程
                    case CommandAndReply.Slave_ReplyCode.G:  //加热炉到达预设行程
                        break;
                    case CommandAndReply.Slave_ReplyCode.F:
                        long longTemp = Common.NumberSystem.BinaryToDecimal_Complement(reply.Data);

                        float force = (float)Utilities.GetForceFromVoltage((float)longTemp,
                                                                                             Common.Setting.SensorMax,
                                                                                             Common.Setting.SensorMV,
                                                                                             Common.Setting.SensorSys);

                        switch (this.work)
                        {
                            default:
                            case "计算皮重":
                                if (this.pressZeroQueue.Count >= 10)
                                {
                                    this.pressZeroQueue.Dequeue();
                                }
                                this.pressZeroQueue.Enqueue(force / 9.8f);
                                break;
                            case "砝码称重":
                                if (this.weightQueue.Count >= 10)
                                {
                                    this.weightQueue.Dequeue();
                                }
                                this.weightQueue.Enqueue(force / 9.8f - this.pressZero);
                                break;
                        }

                        this.Invoke(new Action(() =>
                        {
                            this.lblMessage.Text = $"{DateTime.Now.ToLongTimeString()}：数据：{BitConverter.ToString(reply.Data)}，重量：{force:0.000}牛，{force / 9.8f:0.000}公斤\r\n";
                        }));

                        break;
                    case CommandAndReply.Slave_ReplyCode.E:   //力传感器通道错误
                        this.Invoke(new Action(() =>
                        {
                            this.lblMessage.Text = $"{DateTime.Now.ToLongTimeString()}：传感器数据连接中断\r\n";
                        }));
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            if (this.pressZero == 0) return;
            if (this.weight == 0) return;

            float standardWeight = float.Parse(this.txtStandardWeigth.Text.Trim());
            if (standardWeight == 0) return;

            this.Revise = standardWeight / this.weight;

            Common.Setting.TxtRevise = this.revise;

            Common.Setting.Save("TxtRevise", this.revise.ToString());
        }

        private void FormCalibration_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.serialPort.DataReceived -= this.SerialPort_DataReceived;

            //实验结束指令
            CommandAndReply.Command command_EndTest = CommandAndReply.CommandGenerator.Generate_EndTest();
            CommandAndReply.CommandExecutor.Send(this.serialPort, command_EndTest);
        }
    }
}
