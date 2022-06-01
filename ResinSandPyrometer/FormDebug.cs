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
using System.IO;

namespace ResinSandPyrometer
{
    public partial class FormDebug : Form
    {
        private SerialPort serial_Temperature = null;       //温控仪串口
        private SerialPort serial_Displacement = null;      //位移传感器
        private SerialPort serial_Slave = null;                 //下位机

        public FormDebug()
        {
            InitializeComponent();
        }

        private void Serial_Temperature_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);

            byte[] buffer = new byte[10];//从机返回十个字节

            try
            {
                if (!this.serial_Temperature.IsOpen) return;

                this.serial_Temperature.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：温控仪读取错误：{ ex.Message}\r\n");
                return;
            }

            CommandAndReply.Temperature_Reply reply = CommandAndReply.Temperature_ReplyAnalyzer.Analyse(buffer);

            this.Invoke(new Action(() => 
            {
                this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：{reply}\r\n");
            }));
        }

        private void btnSend_Temperature_Click(object sender, EventArgs e)
        {
            if (this.serial_Temperature == null || this.serial_Temperature.IsOpen == false) 
            {
                this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：温控仪串口未连接\r\n");
                return;
            };

            if (string.IsNullOrEmpty(this.cboCommand_Temperature.Text.Trim()))
            {
                this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：请选定指令\r\n");
                return;
            }

            string commandName = this.cboCommand_Temperature.Text.Trim();

            CommandAndReply.Command command= null;

            switch (commandName)
            {
                default:
                case "查询":
                    command = CommandAndReply.CommandGenerator.Generate_GetFurnaceTemperature();
                    break;
                case "设定目标炉温：20度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(20);
                    break;
                case "设定目标炉温：30度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(30);
                    break;
                case "设定目标炉温：50度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(50);
                    break;
                case "设定目标炉温：100度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(100);
                    break;
                case "设定目标炉温：250度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(250);
                    break;
                case "设定目标炉温：500度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(500);
                    break;
                case "设定目标炉温：1000度":
                    command = CommandAndReply.CommandGenerator.Generate_SetFurnaceTargetTemperature(1000);
                    break;
                case "停止温控仪":
                    command = CommandAndReply.CommandGenerator.Generate_StopTemperatureControl();
                    break;
                case "启动温控仪":
                    command = CommandAndReply.CommandGenerator.Generate_StartTemperatureControl();
                    break;
            }

            CommandAndReply.CommandExecutor.Send(this.serial_Temperature, command);
            this.txtLog_Temperature.AppendText($"\r\n{DateTime.Now.ToLongTimeString()}：指令原始数据：{BitConverter.ToString(command.Bytes)}\r\n");
        }

        private void btnConnect_Temperature_Click(object sender, EventArgs e)
        {
            string portName = this.txtCOM_Temperature.Text.Trim();
            this.serial_Temperature = new SerialPort();
            this.serial_Temperature.PortName = portName;                //COM口
            this.serial_Temperature.BaudRate = 19200;                     //波特率
            this.serial_Temperature.Parity = Parity.None;                   //校验位
            this.serial_Temperature.DataBits = 8;                             //数据位
            this.serial_Temperature.StopBits = StopBits.Two;             //停止位

            this.serial_Temperature.DataReceived += Serial_Temperature_DataReceived;
            this.serial_Temperature.Open();

            this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：成功连接温控仪\r\n");
        }

        private void btnDisconnect_Temperature_Click(object sender, EventArgs e)
        {
            if (this.serial_Temperature == null) return;
            if (this.serial_Temperature.IsOpen)
            {
                this.serial_Temperature.Close();
                this.serial_Temperature.DataReceived -= this.Serial_Temperature_DataReceived;

                this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：断开温控仪\r\n");
            }
        }

        private void btnConnect_Displacement_Click(object sender, EventArgs e)
        {
            string portName = this.txtCOM_Displacement.Text.Trim();
            this.serial_Displacement = new SerialPort();
            this.serial_Displacement.PortName = portName;                //COM口
            this.serial_Displacement.BaudRate = 9600;                       //波特率
            this.serial_Displacement.Parity = Parity.None;                   //校验位
            this.serial_Displacement.DataBits = 8;                             //数据位
            this.serial_Displacement.StopBits = StopBits.One;             //停止位

            this.serial_Displacement.DataReceived += Serial_Displacement_DataReceived; ;

            this.serial_Displacement.Open();

            this.txtLog_Displacement.AppendText($"{DateTime.Now.ToLongTimeString()}：成功连接位移传感器\r\n");
        }

        private void Serial_Displacement_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(10);

            byte[] buffer = new byte[9];
            try
            {
                if (!this.serial_Displacement.IsOpen) return;

                this.serial_Displacement.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                this.txtLog_Displacement.AppendText($"{DateTime.Now.ToLongTimeString()}：位移传感器读取错误：{ ex.Message}\r\n");
                return;
            }

            if (Common.CRC.ToModbusCRC16(buffer) != "0000") return;

            CommandAndReply.Displacement_Reply reply = CommandAndReply.Displacement_ReplyAnalyzer.Analyse(buffer);

            this.Invoke(new Action(() => 
            {
                this.txtLog_Displacement.AppendText($"{DateTime.Now.ToLongTimeString()}：{reply}\r\n");
            }));
        }

        private void btnDisconnect_Displacement_Click(object sender, EventArgs e)
        {
            if (this.serial_Displacement == null) return;
            if (this.serial_Displacement.IsOpen)
            {
                this.serial_Displacement.Close();
                this.serial_Displacement.DataReceived -= this.Serial_Displacement_DataReceived;

                this.txtLog_Temperature.AppendText($"{DateTime.Now.ToLongTimeString()}：断开温控仪\r\n");
            }
        }

        private void btnSend_Displacement_Click(object sender, EventArgs e)
        {
            if (this.serial_Displacement == null || this.serial_Displacement.IsOpen == false)
            {
                this.txtLog_Displacement.AppendText($"{DateTime.Now.ToLongTimeString()}：位移传感器串口未连接\r\n");
                return;
            };

            if (string.IsNullOrEmpty(this.cboCommand_Displacement.Text.Trim()))
            {
                this.txtLog_Displacement.AppendText($"{DateTime.Now.ToLongTimeString()}：请选定指令\r\n");
                return;
            }

            string commandName = this.cboCommand_Displacement.Text.Trim();

            CommandAndReply.Command command = null;

            switch (commandName)
            {
                default:
                case "查询":
                    command = CommandAndReply.CommandGenerator.Generate_GetDisplacement();
                    break;
            }

            CommandAndReply.CommandExecutor.Send(this.serial_Displacement, command);
            this.txtLog_Displacement.AppendText($"\r\n{DateTime.Now.ToLongTimeString()}：指令原始数据：{BitConverter.ToString(command.Bytes)}\r\n");
        }

        private void btnConnect_Slave_Click(object sender, EventArgs e)
        {
            string portName = this.txtCOM_Slave.Text.Trim();
            this.serial_Slave = new SerialPort();
            this.serial_Slave.PortName = portName;                //COM口
            this.serial_Slave.BaudRate = 115200;                    //波特率
            this.serial_Slave.Parity = Parity.None;                   //校验位
            this.serial_Slave.DataBits = 8;                             //数据位
            this.serial_Slave.StopBits = StopBits.One;             //停止位

            this.serial_Slave.DataReceived += Serial_Slave_DataReceived;

            this.serial_Slave.Open();

            this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：成功连接下位机\r\n");
        }

        private void Serial_Slave_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);
            //获取串口传递过来数据（从串口读数据）
            byte[] buffer = new byte[5];
            try
            {
                if (!this.serial_Slave.IsOpen) return;
                this.serial_Slave.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：下位机读取错误：{ ex.Message}\r\n");
                return;
            }

            CommandAndReply.Slave_Reply reply = CommandAndReply.Slave_ReplyAnalyzer.Analyse(buffer);

            this.Invoke(new Action(() =>
            {
                this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：回应原始数据：{BitConverter.ToString(buffer)}\r\n");
                this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：回应代码：{reply.Code} 内容：{ reply.Answer_Content}\r\n");
            }));

            switch (reply.Code)
            {
                case CommandAndReply.Slave_ReplyCode.Answer:
                case CommandAndReply.Slave_ReplyCode.R:
                case CommandAndReply.Slave_ReplyCode.C:
                case CommandAndReply.Slave_ReplyCode.O: //力传感器通道正常
                case CommandAndReply.Slave_ReplyCode.K:   //加载电机到下限位
                case CommandAndReply.Slave_ReplyCode.L:  //加载电机到下限位（零位）
                case CommandAndReply.Slave_ReplyCode.P: //加载电机到达预设行程
                case CommandAndReply.Slave_ReplyCode.G:  //加热炉到达预设行程
                    break;
                case CommandAndReply.Slave_ReplyCode.F:
                    float force = (float)reply.Data_Double;

                    this.Invoke(new Action(() =>
                    {
                        this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：重量：{ force}牛\r\n");
                    }));

                    break;
                case CommandAndReply.Slave_ReplyCode.E:   //力传感器通道错误
                    this.Invoke(new Action(() =>
                    {
                        this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：传感器数据连接中断\r\n");
                    }));
                    break;
            }
        }

        private void btnDisconnect_Slave_Click(object sender, EventArgs e)
        {
            if (this.serial_Slave == null) return;
            if (this.serial_Slave.IsOpen)
            {
                this.serial_Slave.Close();
                this.serial_Slave.DataReceived -= Serial_Slave_DataReceived;

                this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：断开下位机\r\n");
            }
        }

        private void btnSend_Slave_Click(object sender, EventArgs e)
        {
            if (this.serial_Slave == null || this.serial_Slave.IsOpen == false)
            {
                this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：下位机串口未连接\r\n");
                return;
            };

            if (string.IsNullOrEmpty(this.cboCommand_Slave.Text.Trim()))
            {
                this.txtLog_Slave.AppendText($"{DateTime.Now.ToLongTimeString()}：请选定指令\r\n");
                return;
            }

            string commandName = this.cboCommand_Slave.Text.Trim();

            CommandAndReply.Command command = null;

            switch (commandName)
            {
                default:
                case "查询":
                    command = CommandAndReply.CommandGenerator.Generate_CheckEquipment();
                    break;
                case "炉子归零":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceToZero();
                    break;
                case "炉子到默认工作位":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend();
                    break;
                case "炉子到工作位0":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._0);
                    break;
                case "炉子到工作位1":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._1);
                    break;
                case "炉子到工作位2":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._2);
                    break;
                case "炉子到工作位3":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._3);
                    break;
                case "炉子到工作位4":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._4);
                    break;
                case "炉子到工作位5":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._5);
                    break;
                case "炉子到工作位6":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._6);
                    break;
                case "炉子到工作位7":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._7);
                    break;
                case "炉子到工作位8":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._8);
                    break;
                case "炉子到工作位9":
                    command = CommandAndReply.CommandGenerator.Generate_FurnaceDesend(CommandAndReply.FurnaceDesendSpeed._4, CommandAndReply.FurnaceDesendDistance._9);
                    break;
                case "托盘上升一步":
                    command = CommandAndReply.CommandGenerator.Generate_MotorStep(CommandAndReply.MotorUpOrDown.上升);
                    break;
                case "托盘下降一步":
                    command = CommandAndReply.CommandGenerator.Generate_MotorStep(CommandAndReply.MotorUpOrDown.下降);
                    break;
                case "托盘归零":
                    command = CommandAndReply.CommandGenerator.Generate_MotorToZero();
                    break;
                case "设置托盘上升速度(默认值)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorSpeedSetting(CommandAndReply.MotorSpeed._4);
                    break;
                case "设置托盘上升行程(2mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(2);
                    break;
                case "设置托盘上升行程(4mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(4);
                    break;
                case "设置托盘上升行程(6mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(6);
                    break;
                case "设置托盘上升行程(8mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(8);
                    break;
                case "设置托盘上升行程(10mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(20);
                    break;
                case "设置托盘上升行程(12mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(12);
                    break;
                case "设置托盘上升行程(14mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(14);
                    break;
                case "设置托盘上升行程(16mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(16);
                    break;
                case "设置托盘上升行程(18mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(18);
                    break;
                case "设置托盘上升行程(20mm)":
                    command = CommandAndReply.CommandGenerator.Generate_MotorDistanceSetting(20);
                    break;
                case "力传感器当前值查询":
                    command = CommandAndReply.CommandGenerator.Generate_GetForce();
                    break;
                case "设置测试方式(高温抗压强度)":
                    command = CommandAndReply.CommandGenerator.Generate_SetTestType(CommandAndReply.TestType.高温抗压强度测试);
                    break;
                case "设置测试方式(热稳定性)":
                    command = CommandAndReply.CommandGenerator.Generate_SetTestType(CommandAndReply.TestType.热稳定性测试);
                    break;
                case "设置测试方式(高温膨胀力)":
                    command = CommandAndReply.CommandGenerator.Generate_SetTestType(CommandAndReply.TestType.高温膨胀力);
                    break;
                case "单片机复位":
                    command = CommandAndReply.CommandGenerator.Generate_Reset();
                    break;
                case "开始测试":
                    command = CommandAndReply.CommandGenerator.Generate_BeginTest();
                    break;
                case "结束测试":
                    command = CommandAndReply.CommandGenerator.Generate_EndTest();
                    break;
                case "禁止/允许发送传感器零点数据":
                    command = CommandAndReply.CommandGenerator.Generate_EnableSendZeroData();
                    break;
                case "炉温到达设定温度":
                    command = CommandAndReply.CommandGenerator.Generate_TemperatureReached();
                    break;
            }

            CommandAndReply.CommandExecutor.Send(this.serial_Slave, command);
            this.txtLog_Slave.AppendText($"\r\n{DateTime.Now.ToLongTimeString()}：指令原始数据：{BitConverter.ToString(command.Bytes)}\r\n");
        }

        private void btnCalcForce_Click(object sender, EventArgs e)
        {
            FormCalcForce frmCalcForce = new FormCalcForce();
            frmCalcForce.ShowDialog();
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            if (this.serial_Slave == null || this.serial_Slave.IsOpen == false)
            {
                MessageBox.Show("下位机串口未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            };

            this.serial_Slave.DataReceived -= this.Serial_Slave_DataReceived;

            FormCalibration frmCalibration = new FormCalibration(this.serial_Slave);

            DialogResult dlgResult = frmCalibration.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                Common.Setting.Save("TxtRevise", frmCalibration.Revise.ToString());
            }

            this.serial_Slave.DataReceived += this.Serial_Slave_DataReceived;
        }

        //private void AutoDetectSerialPort(out string portName_Slave, out string portName_Temperature)
        //{
            

        //}

        private void btnAutoDetect_Click(object sender, EventArgs e)
        {
            this.txtAutoDetect.Text = "";

            int serialPort_Start = 3;

            //先找到可用的串口
            List<string> serialPortNames_Avalible = new List<string>();
            for (int index = serialPort_Start; index <= serialPort_Start + 20; index++)
            {
                string portName = $"COM{index}";
                SerialPort serialPort = new SerialPort();
                serialPort.PortName = portName;                //COM口
                serialPort.BaudRate = 115200;                    //波特率
                serialPort.Parity = Parity.None;                   //校验位
                serialPort.DataBits = 8;                             //数据位
                serialPort.StopBits = StopBits.One;             //停止位

                try
                {
                    serialPort.Open();
                    serialPortNames_Avalible.Add(portName);

                    serialPort.Close();
                }
                catch (IOException) //串口不可用
                {
                    this.txtAutoDetect.AppendText($"{portName} 不可用\r\n");
                }
                catch (UnauthorizedAccessException) //串口被其他程序占用
                {
                    this.txtAutoDetect.AppendText($"{portName} 可用，但被其他程序占用\r\n");
                    serialPortNames_Avalible.Add(portName);
                }
            }

            this.txtAutoDetect.AppendText("\r\n可用串口：\r\n");

            foreach (string portName in serialPortNames_Avalible)
            {
                this.txtAutoDetect.AppendText($"\t{portName}\r\n");
            }

            foreach (string portName in serialPortNames_Avalible)
            {
                SerialPort serialPort = new SerialPort();
                serialPort.PortName = portName;                //COM口
                serialPort.BaudRate = 115200;                    //波特率
                serialPort.Parity = Parity.None;                   //校验位
                serialPort.DataBits = 8;                             //数据位
                serialPort.StopBits = StopBits.One;             //停止位
                serialPort.DataReceived += SerialPort_DataReceived;

                try
                {
                    serialPort.Open();

                    CommandAndReply.Command commandCheck = CommandAndReply.CommandGenerator.Generate_CheckEquipment();
                    CommandAndReply.CommandExecutor.Send(serialPort, commandCheck);
                }
                catch (IOException) //串口不可用
                {
                }
                catch (UnauthorizedAccessException) //串口被其他程序占用
                {
                }
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);

            SerialPort serialPort = (SerialPort)sender;

            byte[] buffer = new byte[5];
            try
            {
                if (!serialPort.IsOpen) return;
                serialPort.Read(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                serialPort.Close();
                return;
            }

            CommandAndReply.Slave_Reply reply = CommandAndReply.Slave_ReplyAnalyzer.Analyse(buffer);

            if (reply.Answer_Content == "仪器查询成功")
            {
                this.Invoke(new Action(() => 
                {
                    this.txtAutoDetect.AppendText("\r\n");
                    this.txtAutoDetect.AppendText($"{serialPort.PortName} 为单片机串口\r\n");
                }));
            }

            serialPort.Close();
        }
    }
}
