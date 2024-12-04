using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResinSandPyrometer.CommandAndReply;
using ResinSandPyrometer.Common;
using ResinSandPyrometer.Lab;

namespace ResinSandPyrometer
{
    public partial class FormMain : Form
    {
        private TemperatureState temperatureState = new TemperatureState();

        private GlobaState globaState = new GlobaState();

        private bool isTemperatureReached = false;//判断温度是否到达到

        private bool isFurnaceZero = false;//加热炉是否在零点

        private bool isMotorZero = false;//加载电机是否在零点

        private DateTime startTime;//开始时间

        private bool isReachedGo = false;

        private Labs labOfSelected = Labs.FirstLab;

        private bool isStartDisplacement = false;

        private bool isReset = false;

        private string fileNameForFirstLab = string.Empty;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //加载配置
            Setting.Load();

            this.lblMotorDis.Text = Setting.MotorIdlePath.ToString();
            this.lblHeatDis.Text = Setting.FurnaceFallingDistance.ToString();
            this.txtTargetTemperature.Text = Setting.SettingTemperature.ToString();
            this.lblDispalcementMotorIdlePath.Text = Setting.DisplacementMotorIdlePath.ToString();
            this.txtRepeatNumber.Text = "1";
            #region 2023-01-08
            //this.btnStartTest.Enabled = false;
            this.btnStartTest.Enabled = true;
            #endregion

            this.btnSaveData.Enabled = false;

            this.lblPrePress.Text = Setting.PreloadedForce.ToString();

            this.InitSerialPort_Slave();
            this.InitSerialPort_Temperature();
            //this.InitSerialPort_Displacement();

            try
            {
                this.SendLocation();
                Thread.Sleep(500);

                //复位
                #region 2023-01-09
                //Command command = CommandGenerator.Generate_EnableSendZeroData();
                //CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                #endregion

                this.isReset = true;

                //窗台打开时，获取当前的炉温，并显示
                this.CheckCurrentTemperature();

                //显示试样信息
                this.ShowSampleInfo();

            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("单片机写入故障：{0}", ex.Message));
            }
        }

        private void ShowSampleInfo()
        {
            this.txtSampleNo.Text = Setting.SpecimenNum;
            this.txtSampeName.Text = Setting.SpecimenName;
            this.txtInnerDiameter.Text = Setting.InnerDiameter.ToString();
            this.txtSpecimenDiameter.Text = Setting.SpecimenDiameter.ToString();
            this.txtSpecimenHeight.Text = Setting.SpecimenHeight.ToString();
            this.txtWorkerName.Text = Setting.ExperimentPerson;
            this.txtCompany.Text = Setting.ExperimentUnit;
            this.txtRepeatNumber.Text = Setting.RepeatTimes.ToString();
        }

        /// <summary>
        /// 采集当前炉温
        /// </summary>
        private void CheckCurrentTemperature()
        {
            if (SerialPortManager.SerialPort_Temperature == null || SerialPortManager.SerialPort_Temperature.IsOpen == false) return;

            Command command = CommandGenerator.Generate_GetFurnaceTemperature();
            CommandExecutor.Send(SerialPortManager.SerialPort_Temperature, command);
        }

        private void InitSerialPort_Slave()
        {
            this.tlblPort_Slave.Text = SerialPortManager.Slave_COM;
            this.tlblPort_Slave.ForeColor = Color.Green;

            this.WindowState = FormWindowState.Maximized;

            SerialPortManager.OpenSerial_Slave();
            SerialPortManager.AddDataReceivedEventHandler_Slave(this.serialPort_Slave_DataReceived);
            SerialPortManager.AddErrorReceivedEventHandler_Slave(this.serialPort_Slave_ErrorReceived);
        }

        private void InitSerialPort_Temperature()
        {
            this.Invoke(new Action(() => 
            {
                this.tlblPort_Temperature.Text = SerialPortManager.Temperature_COM;
                this.tlblPort_Temperature.ForeColor = Color.Green;

                this.tmCheckTemperature.Enabled = true;
            }));

            bool isExists = false;

            SerialPortManager.OpenSerial_Temperature(ref isExists);
            if (isExists == false)
            {
                SerialPortManager.AddDataReceivedEventHandler_Temperature(this.serialPort_Temperature_DataReceived);
                SerialPortManager.AddErrorReceivedEventHandler_Temperature(this.serialPort_Temperature_ErrorReceived);
            }
        }

        #region 单片机
        private void serialPort_Slave_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("单片机串口出错，错误类型：" + e.EventType.ToString(), "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_Slave_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(20);
            //获取串口传递过来数据（从串口读数据）
            byte[] buffer = new byte[5];
            try
            {
                if (SerialPortManager.SerialPort_Slave.IsOpen == false) return;
                SerialPortManager.SerialPort_Slave.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("单片机读取错误：{0}",ex.Message));
                return;
            }

            PointF point = new PointF(0f, 0f);

            Slave_Reply reply = Slave_ReplyAnalyzer.Analyse(buffer);

            switch (reply.Code)
            {
                case Slave_ReplyCode.Answer:
                    this.Invoke(new Action(delegate () { this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content; }));
                    break;

                case Slave_ReplyCode.R:
                    this.Invoke(new Action(delegate ()
                    {
                        this.StartTest();
                        this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content;
                    }));
                    break;

                case Slave_ReplyCode.C:
                    this.Invoke(new Action(delegate () {this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content;}));

                    this.globaState.IsEndTest = true;
                    switch (Setting.TestType)
                    {
                        case 0:
                            this.globaState.FirstLabState.InitState();
                            break;
                        case 1:
                            this.globaState.SecondLabState.InitState();
                            break;
                        case 2:
                            this.globaState.ThirdLabState.InitState();
                            break;
                        case 3:
                            this.globaState.FouthLabState.InitState();
                            break;
                        default:
                            break;
                    }

                    this.Invoke(new Action(delegate()
                    {
                        this.btnSettings.Enabled = true;
                        this.btnSampleInfoSetting.Enabled = true;
                        this.btnStartTest.Enabled = true;
                        this.btnSettingFurnaceTemperature.Enabled = true;
                    }));
                    break;

                case Slave_ReplyCode.F:
                    float force = (float)reply.Data_Double;

                    this.Invoke(new Action(delegate() { this.lblReceivedInfo.Text = $"重量:{force:0.00}牛"; }));

                    if (force > Setting.SensorMax)
                    {
                        this.Reset();
                        this.Invoke(new Action(delegate () { this.lblStatusTip.Text = "超过力传感器的极限值！！！"; }));
                        return;
                    }

                    if (force < -3.1f || force == 0) return;

                    if (this.labOfSelected == Labs.FourthLab) break;

                    if (this.globaState.IsStartTest && !this.globaState.IsEndTest)
                    {
                        switch (this.labOfSelected)
                        {
                            case Labs.FirstLab:
                                this.ExecuteFirstLab(point, force);
                                break;
                            case Labs.SecondLab:
                                this.ExecuteSecondLab(point, force);
                                break;
                            case Labs.ThirdLab:
                                this.ExecuteThirdLab(point, force);
                                break;
                            default:
                                break;
                        }
                    }
                    break;

                case Slave_ReplyCode.E:   //力传感器通道错误
                    this.Invoke(new Action(delegate() 
                    { 
                        this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content;
                        this.lblStatusTip.Text = "传感器数据连接中断";
                    }));
                    break;

                case Slave_ReplyCode.O: //力传感器通道正常
                    this.Invoke(new Action(delegate () { this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content;}));
                    break;

                case Slave_ReplyCode.K:   //加载电机到下限位
                    this.Invoke(new Action(delegate() { this.lblStatusTip.Text = reply.Answer_Content; }));
                    this.isFurnaceZero = (reply.IsError == false);
                    break;
                   
                case Slave_ReplyCode.L:  //加载电机到下限位（零位）
                    this.Invoke(new Action(delegate () { this.lblStatusTip.Text = reply.Answer_Content; }));
                    this.isMotorZero = (reply.IsError == false);
                    break;

                case Slave_ReplyCode.P: //加载电机到达预设行程
                    this.Invoke(new Action(delegate () { this.lblStatusTip.Text = reply.Answer_Content; }));

                    if (this.isReachedGo == false) return;

                    switch (this.labOfSelected)
                    {
                        case Labs.FirstLab:
                            Setting.TestType = 0;
                            this.globaState.GoToFirstLab();
                            this.globaState.FirstLabState.Step = FirstLabStep.加热炉按行程下降;
                            break;
                        case Labs.SecondLab:
                            Setting.TestType = 1;
                            this.globaState.GoToSecondLab();
                            this.globaState.SecondLabState.Step = SecondLabStep.开始调试并发送指令;
                            break;
                        case Labs.ThirdLab:
                            Setting.TestType = 2;
                            this.globaState.GoToThreeStep();
                            this.globaState.ThirdLabState.Step = ThirdLabStep.开始测试并发送指令;
                            break;
                        case Labs.FourthLab:
                            Setting.TestType = 3;
                            this.isStartDisplacement = true;
                            #region 2023-01-07 修改第四个实验的过程
                            //this.globaState.FouthLabState.Step = FourthLabStep.取预置零点值;
                            this.globaState.FouthLabState.Step = FourthLabStep.加热炉下降;

                            //try
                            //{
                            //    Command command = CommandGenerator.Generate_GetDisplacement();
                            //    CommandExecutor.Send(SerialPortManager.SerialPort_Displacement, command);
                            //}
                            //catch (Exception ex)
                            //{
                            //    SampleLoggerOnTextFile.Log($"位移传感器写入故障：{ex.Message}");
                            //}

                            #endregion
                            break;
                    }
                    break;

                case Slave_ReplyCode.G:  //加热炉到达预设行程
                    this.Invoke(new Action(delegate () { this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content; }));

                    switch (this.labOfSelected)
                    {
                        case Labs.FirstLab:
                            this.globaState.FirstLabState.Step = FirstLabStep.取预置零点值;
                            break;
                        case Labs.SecondLab:
                            #region 2023-01-08
                            //this.globaState.SecondStepState.Step = SecondLabStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变;
                            this.globaState.SecondLabState.Step = SecondLabStep.第二次取零点;
                            #endregion
                            this.startTime = DateTime.Now;
                            break;
                        case Labs.ThirdLab:
                            this.globaState.ThirdLabState.Step = ThirdLabStep.采样数据并绘制压强时间曲线及判断压强值是否突变;
                            this.startTime = DateTime.Now;
                            break;
                        case Labs.FourthLab:
                            if (this.globaState.FouthLabState.IsWaitOver) return;

                            #region 2023-01-07 修改第四个实验的过程
                            //this.globaState.FouthLabState.Step = FourthLabStep.更新零点值;
                            this.globaState.FouthLabState.Step = FourthLabStep.取预置零点值;
                            #endregion

                            this.globaState.FouthLabState.IsWaitOver = false;

                            break;
                    }
                    break;
            }
        }

        //高温急热膨胀率
        private void ExecuteFouthLab(PointF point, float displacement)
        {
            Command command = null;
            switch (this.globaState.FouthLabState.Step)
            {
                case FourthLabStep.NONE:
                    break;

                #region 2023-01-07 废弃代码
                //case FourthLabStep.取预置零点值:
                //    this.globaState.FouthLabState.TimeCount(1);
                //    this.globaState.FouthLabState.GetDisplacementZero(displacement);//零点值
                //    if (this.globaState.FouthLabState.IsTimeReached == true)
                //    {
                //        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.运行);
                //        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                //        this.globaState.FouthLabState.Step = FourthLabStep.电机上升;
                //    }

                //    break;

                //case FourthLabStep.电机上升:
                //    if (Math.Abs(this.globaState.FouthLabState.DisplacementZero - displacement) >= 2)
                //    {
                //        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.停止);
                //        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                //        this.globaState.FouthLabState.DisplacementZeroClear();
                //        this.globaState.FouthLabState.Step = FourthLabStep.电机下降;
                //        this.globaState.FouthLabState.IsTimeReached = false;
                //    }

                //    break;
                //case FourthLabStep.电机下降:
                //    command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.下降一毫米);
                //    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                //    this.globaState.FouthLabState.Step = FourthLabStep.加热炉下降;
                //    break;
                #endregion

                case FourthLabStep.加热炉下降:
                    this.globaState.FouthLabState.Sleep();
                    if (this.globaState.FouthLabState.IsOK)
                    {
                        string desendSpeed = $"_{Setting.FurnaceLiftingSpeed / 10 - 1}";
                        string desendDistance = $"_{(Setting.FurnaceFallingDistance - 150) / 2}";

                        command = CommandGenerator.Generate_FurnaceDesend((FurnaceDesendSpeed)Enum.Parse(typeof(FurnaceDesendSpeed), desendSpeed),
                                                                                                         (FurnaceDesendDistance)Enum.Parse(typeof(FurnaceDesendDistance), desendDistance));
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        SampleLoggerOnTextFile.Log($"FourStep.加热炉下降");

                        this.isFurnaceZero = false;

                        this.globaState.FouthLabState.Step = FourthLabStep.等待加热炉下降;
                    }
                    break;

                case FourthLabStep.等待加热炉下降:
                    this.globaState.FouthLabState.Wait();
                    if (this.globaState.FouthLabState.IsWaitOver)
                    {
                        #region 2023-01-07 修改实验流程
                        this.globaState.FouthLabState.DisplacementZeroClear();
                        #endregion

                        this.globaState.FouthLabState.Step = FourthLabStep.取预置零点值;
                    }
                    break;

                case FourthLabStep.取预置零点值:
                    this.globaState.FouthLabState.TimeCount(1);
                    this.globaState.FouthLabState.GetDisplacementZero(displacement);//零点值
                    if (this.globaState.FouthLabState.IsTimeReached)
                    {
                        this.startTime = DateTime.Now;
                        this.globaState.FouthLabState.Step = FourthLabStep.采集位移数据并绘制曲线以及判断是否突变;
                        this.globaState.FouthLabState.IsTimeReached = false;
                    }
                    break;

                case FourthLabStep.采集位移数据并绘制曲线以及判断是否突变:
                    //point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (float)Math.Round((displacement - this.globaState.FouthLabState.DisplacementZero) / Setting.SpecimenHeight * 100, 2);
                    Setting.Points.Add(point);

                    this.globaState.FouthLabState.GetMaxDisplacement(point);//得到最大压力值和对应的时间
                    this.globaState.FouthLabState.CheckDisplacementSubChange(point);

                    this.Invoke(new Action(() =>
                    {
                        this.lblMaxExpansionRate.Text = this.globaState.FouthLabState.MaxDisplacement.ToString();
                        this.lblMaxExpansionRateTime.Text = BytesOperator.GetOneVaule(this.globaState.FouthLabState.MaxDisplacementTime).ToString();

                        //this.chartExpansionRate.Series[0].Points.AddXY(point.X, point.Y);//将点添加到膨胀率--时间曲线中去
                        this.chartExpansionRate.Series[0].Points.AddY(point.Y);//将点添加到膨胀率--时间曲线中去
                    }));

                    if (this.globaState.FouthLabState.IsDisplacementSudChange)//抗压强度值是否突变
                    {
                        command = CommandGenerator.Generate_EndTest();
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        this.globaState.FouthLabState.Step = FourthLabStep.测试结束;
                    }
                    break;

                case FourthLabStep.测试结束:
                    this.globaState.IsEndTest = true;
                    this.globaState.FouthLabState.IsTimeReached = false;
                    this.globaState.FouthLabState.InitState();
                    this.Invoke(new Action(() =>
                    {
                        this.isStartDisplacement = false;
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;
                        this.lblStatusTip.Text = "测试结束，请保存数据";

                    }));

                    this.InitSerialPort_Temperature();

                    break;
            }
        }

        //条件热稳定性
        private void ExecuteThirdLab(PointF point,float force)
        {
            Command command = null;
            switch (this.globaState.ThirdLabState.Step)
            {
                case ThirdLabStep.开始测试并发送指令:
                    this.globaState.TimeCount(2);//2s
                    this.globaState.GetPressureZero(force);//零点值
                    if (this.globaState.IsTimeReached)
                    {
                        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.运行);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        this.isMotorZero = false;
                        this.globaState.ThirdLabState.Step = ThirdLabStep.读取传感器采样值并判断预载荷是否达到预定值;
                        this.globaState.IsTimeReached = false;
                    }

                    this.Invoke(new Action(() =>
                    {
                        this.btnSettingFurnaceTemperature.Enabled = false;
                    }));
                    break;

                case ThirdLabStep.读取传感器采样值并判断预载荷是否达到预定值:
                    point.Y = (force - this.globaState.PressureZero) / Setting.GetArea();
                    this.Invoke(new Action(()=> { this.lblPrePressure.Text = Setting.PreloadedPressure.ToString();}));

                    if (point.Y > Setting.PreloadedPressure - 0.005f)
                    {
                        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.停止);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        this.globaState.ThirdLabState.Step = ThirdLabStep.继续采样并保证在一定范围内;
                    }

                    break;

                case ThirdLabStep.继续采样并保证在一定范围内:
                    point.Y = (force - this.globaState.PressureZero) / Setting.GetArea();

                    #region 2023-01-08 
                    //if (point.Y > Setting.PreloadedPressure + 0.005f)
                    //{
                    //    command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.下降);
                    //    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    //}
                    //else if (point.Y < Setting.PreloadedPressure - 0.005f)
                    //{
                    //    command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.上升);
                    //    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    //}
                    //else
                    //{
                    //    this.Invoke(new Action(() => {this.lblPrePressure.Text = point.Y.ToString();}));

                    //    if (!this.globaState.ThirdLabState.IsBPGet)
                    //    {
                    //        this.globaState.ThirdLabState.BalancePress = point.Y;
                    //        this.globaState.ThirdLabState.IsBPGet = true;
                    //    }
                    //    if (!this.globaState.ThirdLabState.IsTimeReached)
                    //    {
                    //        this.globaState.ThirdLabState.TimeCount(2);//2s
                    //    }
                    //    else
                    //    {
                    //        this.globaState.ThirdLabState.Step = ThirdLabStep.加热炉下降;
                    //    }
                    //}

                    if (point.Y >= Setting.PreloadedPressure * 0.7f)
                    {
                        this.Invoke(new Action(() => { this.lblPrePressure.Text = point.Y.ToString(); }));

                        if (!this.globaState.ThirdLabState.IsBPGet)
                        {
                            this.globaState.ThirdLabState.BalancePress = point.Y;
                            this.globaState.ThirdLabState.IsBPGet = true;
                        }
                        if (!this.globaState.ThirdLabState.IsTimeReached)
                        {
                            this.globaState.ThirdLabState.TimeCount(2);//2s
                        }
                        else
                        {
                            this.globaState.ThirdLabState.Step = ThirdLabStep.加热炉下降;
                        }
                    }

                    #endregion

                    break;

                case ThirdLabStep.加热炉下降:
                    string desendSpeed = $"_{Setting.FurnaceLiftingSpeed / 10 - 1}";
                    string desendDistance = $"_{(Setting.FurnaceFallingDistance - 150) / 2}";

                    command = CommandGenerator.Generate_FurnaceDesend((FurnaceDesendSpeed)Enum.Parse(typeof(FurnaceDesendSpeed), desendSpeed),
                                                                                                     (FurnaceDesendDistance)Enum.Parse(typeof(FurnaceDesendDistance), desendDistance));
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    SampleLoggerOnTextFile.Log($"ThreeLab.加热炉下降");

                    this.isFurnaceZero = false;
                    this.globaState.ThirdLabState.Step = ThirdLabStep.NONE;

                    break;
                case ThirdLabStep.采样数据并绘制压强时间曲线及判断压强值是否突变:
                    point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (force - this.globaState.PressureZero) / Setting.GetArea();

                    this.globaState.ThirdLabState.GetBalanceTime(point.X);
                    Setting.Points.Add(point);

                    this.globaState.ThirdLabState.CheckBlancePressureChange(point.Y, Setting.PreloadedPressure);//检查抗压强度是否突变
                    if (this.globaState.ThirdLabState.IsBalancePressureSudChange)
                    {
                        this.globaState.ThirdLabState.BalanceTime = point.X;
                        this.globaState.ThirdLabState.Step = ThirdLabStep.突变并返回;
                    }
                    if (point.Y > Setting.PreloadedPressure + 0.005f)
                    {
                        command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.下降);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    }
                    else if (point.Y < Setting.PreloadedPressure - 0.005f)
                    {
                        command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.上升);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    }
                    else
                    {
                        this.Invoke(new Action(()=> { this.lblPrePressure.Text = point.Y.ToString(); }));
                    }
                    
                    this.Invoke(new Action(() =>
                    {
                        //this.txtBalancePressureTime.Text = BytesOperator.GetOneVaule(this.globaState.ThirdLabState.BalanceTime).ToString();
                        this.txtBalancePressureTime.Text = ((int)this.globaState.ThirdLabState.BalanceTime).ToString();

                        //将点添加到预压强--时间曲线中去
                        this.chartBalancePress.Series[0].Points.AddXY(point.X, point.Y);
                    }));

                    break;
                case ThirdLabStep.突变并返回:
                    this.globaState.ThirdLabState.Step = ThirdLabStep.测试结束;
                    break;

                case ThirdLabStep.测试结束:
                    command = CommandGenerator.Generate_EndTest();
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    this.globaState.IsEndTest = true;
                    this.globaState.ThirdLabState.InitState();
                    this.globaState.PressureZero = 0;
                    this.Invoke(new Action(()=>
                    {
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;
                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    }));

                    break;
                default:
                    break;
            }

            this.Invoke(new Action(() =>
            {
                //this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{Math.Abs(this.globaState.PressureZero - force):0.00}牛";
                this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛";
            }));
        }

        //高温膨胀力
        private void ExecuteSecondLab(PointF point, float force)
        {
            Command command = null;
            switch (this.globaState.SecondLabState.Step)
            {
                case SecondLabStep.开始调试并发送指令:
                    this.globaState.TimeCount(2);//2s
                    this.globaState.GetPressureZero(force);//零点值
                    if (this.globaState.IsTimeReached)
                    {
                        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.运行);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        this.isMotorZero = false;
                        this.globaState.SecondLabState.Step = SecondLabStep.检测预载荷是否为10主机发送指令加载电机停止;
                    }
                    this.Invoke(new Action(() => { this.btnSettingFurnaceTemperature.Enabled = false; }));

                    this.Invoke(new Action(() =>
                    {
                        //this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{force - this.globaState.PressureZero:0.00}牛";
                        this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛";
                    }));

                    break;
                case SecondLabStep.检测预载荷是否为10主机发送指令加载电机停止:
                    point.Y = force - this.globaState.PressureZero;

                    if (point.Y > Setting.PreloadedForce - 0.5)
                    {
                        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.停止);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        this.globaState.SecondLabState.Step = SecondLabStep.检测载荷是否为9到11并调节预载荷;
                    }

                    this.Invoke(new Action(() =>
                    {
                        //this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{force - this.globaState.PressureZero:0.00}牛";
                        this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛";
                    }));

                    break;

                case SecondLabStep.检测载荷是否为9到11并调节预载荷:
                    point.Y = force - this.globaState.PressureZero;
                    if (point.Y > Setting.PreloadedForce + 0.5)
                    {
                        command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.下降);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    }
                    else if (point.Y < Setting.PreloadedForce - 0.5)
                    {
                        command = CommandGenerator.Generate_MotorStep(MotorUpOrDown.上升);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    }
                    else
                    {
                        #region 2023-01-08
                        //this.globaState.SecondStepState.TimeCount(1.5f);
                        #endregion

                        #region 2023-01-08
                        //this.globaState.SecondStepState.GetPressureZero(force);//零点值
                        //if (this.globaState.SecondStepState.IsTimeReached)
                        //{
                        //    this.globaState.SecondStepState.Step = SecondLabStep.加热炉下降;
                        //}

                        this.globaState.SecondLabState.Step = SecondLabStep.加热炉下降;
                        #endregion
                        
                        this.Invoke(new Action(() => { this.lblPrePress.Text = point.Y.ToString(); }));
                    }

                    this.Invoke(new Action(() =>
                    {
                        //this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{force - this.globaState.PressureZero:0.00}牛";
                        this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛";
                    }));

                    break;

                case SecondLabStep.加热炉下降:
                    string desendSpeed = $"_{Setting.FurnaceLiftingSpeed / 10 - 1}";
                    string desendDistance = $"_{(Setting.FurnaceFallingDistance - 150) / 2}";

                    command = CommandGenerator.Generate_FurnaceDesend((FurnaceDesendSpeed)Enum.Parse(typeof(FurnaceDesendSpeed), desendSpeed),
                                                                                                     (FurnaceDesendDistance)Enum.Parse(typeof(FurnaceDesendDistance), desendDistance));
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    SampleLoggerOnTextFile.Log($"SecondLab.加热炉下降");

                    this.isFurnaceZero = false;
                    this.globaState.SecondLabState.Step = SecondLabStep.NONE;

                    this.Invoke(new Action(() =>
                    {
                        //this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{force - this.globaState.PressureZero:0.00}牛";
                        this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛";
                    }));

                    break;

                #region 2023-01-08
                case SecondLabStep.第二次取零点:
                    this.globaState.SecondLabState.TimeCount(2f);
                    if (this.globaState.SecondLabState.IsTimeReached)
                    {
                        this.globaState.SecondLabState.GetPressureZero(force);//零点值
                        if (this.globaState.SecondLabState.IsPressureZero)
                        {
                            this.globaState.SecondLabState.Step = SecondLabStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变;
                        }
                    }

                    this.Invoke(new Action(() =>
                    {
                        //this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{force - this.globaState.PressureZero:0.00}牛";
                        this.lblDebugInfo.Text = $"皮重:{this.globaState.PressureZero:0.00}牛";
                    }));

                    break;

                #endregion

                case SecondLabStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变:
                    //point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (force - this.globaState.SecondLabState.PressureZero);
                    Setting.Points.Add(point);
                    this.globaState.SecondLabState.GetMaxPress(point);//最大膨胀力和时间
                    this.globaState.SecondLabState.CheckPressureSudChange(point.Y);//检查膨胀力是否突变
                    if ((DateTime.Now - this.startTime).TotalSeconds >= 60 && this.globaState.SecondLabState.IsPressureSudChange)
                    {
                        this.globaState.SecondLabState.Step = SecondLabStep.结束测试;
                    }

                    this.Invoke(new Action(() => 
                    {
                        #region 2023-01-09
                        //this.lblPengZhangPower.Text = BytesOperator.GetThreeValue(this.globaState.SecondLabState.MaxPress);
                        this.lblPengZhangPower.Text = $"{this.globaState.SecondLabState.MaxPress:0.00}";
                        #endregion

                        #region 2023-01-08
                        //this.lblPengZhangTime.Text = BytesOperator.GetThreeValue(this.globaState.SecondStepState.MaxPress_Time);
                        this.lblPengZhangTime.Text = $"{this.globaState.SecondLabState.MaxPress_Time:0}";
                        #endregion

                        //this.chartPengZhang.Series[0].Points.AddXY(point.X, point.Y);//将点添加到膨胀力--时间曲线中去
                        this.chartPengZhang.Series[0].Points.AddY(point.Y);//将点添加到膨胀力--时间曲线中去
                    }));

                    this.Invoke(new Action(() =>
                    {
                        //this.lblDebugInfo.Text = $"第二次零点:{this.globaState.SecondLabState.PressureZero:0.00}牛，原始值:{force:0.00}牛，压力:{force - this.globaState.SecondLabState.PressureZero:0.00}牛";
                        this.lblDebugInfo.Text = $"第二次零点:{this.globaState.SecondLabState.PressureZero:0.00}牛";
                    }));

                    break;
                case SecondLabStep.结束测试:
                    command = CommandGenerator.Generate_EndTest();
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    this.globaState.IsEndTest = true;
                    this.globaState.IsTimeReached = false;
                    this.globaState.SecondLabState.InitState();

                    this.Invoke(new Action(() =>
                    {
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;

                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    }));
                    break;
                default:
                    break;
            }
        }

        // 测试高温抗压强度
        private void ExecuteFirstLab(PointF point, float force)
        {
            Command command = null;
            switch (this.globaState.FirstLabState.Step)
            {
                case FirstLabStep.加热炉按行程下降:
                    string desendSpeed = $"_{Setting.FurnaceLiftingSpeed / 10 - 1}";
                    string desendDistance = $"_{(Setting.FurnaceFallingDistance - 150) / 2}";

                    command = CommandGenerator.Generate_FurnaceDesend((FurnaceDesendSpeed)Enum.Parse(typeof(FurnaceDesendSpeed), desendSpeed),
                                                                                                     (FurnaceDesendDistance)Enum.Parse(typeof(FurnaceDesendDistance), desendDistance));
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    SampleLoggerOnTextFile.Log($"FirstStep.加热炉按行程下降");

                    this.isFurnaceZero = false;
                    this.globaState.FirstLabState.Step = FirstLabStep.NONE;

                    break;

                case FirstLabStep.取预置零点值:
                    this.globaState.FirstLabState.CommandCount(1);
                    this.globaState.FirstLabState.GetPressureZero(force);

                    if (this.globaState.FirstLabState.IsCommandReached)
                    {
                        this.globaState.FirstLabState.Step = FirstLabStep.抗压试样保温时间计时;
                    }
                    this.Invoke(new Action(() => { this.lblTimeCount.Text = this.globaState.FirstLabState.TimeCount(Setting.SoakingTime).ToString(); })); //保温时间和延时

                    SampleLoggerOnTextFile.Log($"FirstStep.取预置零点值");
                    break;

                case FirstLabStep.抗压试样保温时间计时:
                    this.Invoke(new Action(() => { this.lblTimeCount.Text = this.globaState.FirstLabState.TimeCount(Setting.SoakingTime).ToString(); })); //保温时间和延时

                    if (this.globaState.FirstLabState.IsTimeReached)
                    {
                        this.isMotorZero = false;
                        this.startTime = DateTime.Now;
                        //this.globaState.FirstLabState.Step = FirstLabStep.托盘快速上升2mm;
                        this.globaState.FirstLabState.Step = FirstLabStep.托盘缓慢上升;
                        this.globaState.FirstLabState.IsTimeReached = false;
                    }

                    SampleLoggerOnTextFile.Log($"FirstStep.抗压试样保温时间计时");
                    break;

                //case FirstLabStep.托盘快速上升2mm:
                    
                //    command = CommandAndReply.CommandGenerator.Generate_MotorTest(CommandAndReply.MotorTestType.加载电机上升);
                //    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                //    this.globaState.FirstLabState.Step = FirstLabStep.托盘缓慢上升;

                //    this.Invoke(new Action(() => { this.lblTimeCount.Text = this.globaState.FirstLabState.TimeCount(Setting.SoakingTime).ToString(); })); //保温时间和延时

                //    SampleLoggerOnTextFile.Log($"FirstStep.托盘快速上升2mm");
                //    break;

                case FirstLabStep.托盘缓慢上升:
                    command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.运行);
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    this.globaState.FirstLabState.Step = FirstLabStep.采集数据压力是否突变;

                    SampleLoggerOnTextFile.Log($"FirstStep.托盘缓慢上升");
                    break;

                case FirstLabStep.采集数据压力是否突变:
                    //point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;

                    #region 2023-01-09
                    //point.Y = (force - this.globaState.FirstLabState.PressureZero) / Setting.GetArea() * Setting.PresureRate;
                    point.Y = (force + Setting.Preload / 1000f * Setting.G - this.globaState.FirstLabState.PressureZero) / Setting.GetArea() * Setting.PresureRate;
                    #endregion

                    Setting.Points.Add(point);

                    this.globaState.FirstLabState.GetMaxPressure(point);//得到最大压力值和对应的时间
                    this.globaState.FirstLabState.CheckPressureSubChange(point.Y);//检查抗压强强值是否过大

                    #region 2023-01-09
                    //if ((DateTime.Now - this.startTime).TotalSeconds >= 60 && this.globaState.FirstLabState.IsPressureSudChange)//抗压强度值是否突变
                    if (this.globaState.FirstLabState.IsPressureSudChange)//抗压强度值是否突变
                    #endregion
                    {
                        command = CommandGenerator.Generate_MotorRunOrStop(MotorRunOrStop.停止);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                        this.globaState.FirstLabState.Step = FirstLabStep.测试结束;
                    }

                    this.Invoke(new Action(() =>
                    {
                        this.lblPressure.Text = BytesOperator.GetOneVaule((this.globaState.FirstLabState.MaxPressure)).ToString();
                        this.lblPressureTime.Text = BytesOperator.GetOneVaule(this.globaState.FirstLabState.MaxPreesureTime).ToString();

                        //this.chartPressure.Series[0].Points.AddXY(point.X, point.Y);//将点添加到压强--时间曲线中去
                        this.chartPressure.Series[0].Points.AddY(point.Y);//将点添加到压强--时间曲线中去

                    }));

                    SampleLoggerOnTextFile.Log($"FirstStep.采集数据压力是否突变");
                    break;

                case FirstLabStep.测试结束:
                    command = CommandGenerator.Generate_EndTest();
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    this.globaState.IsEndTest = true;

                    this.globaState.FirstLabState.InitState();
                    this.Invoke(new Action(() =>
                    {
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;

                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    }));

                    SampleLoggerOnTextFile.Log($"FirstStep.测试结束");

                    break;
            }

            this.Invoke(new Action(() =>
            {
                //this.lblDebugInfo.Text = $"皮重:{this.globaState.FirstLabState.PressureZero:0.00}牛，原始值:{force:0.00}牛，去皮重:{Math.Abs(this.globaState.FirstLabState.PressureZero - force):0.00}牛";
                this.lblDebugInfo.Text = $"皮重:{this.globaState.FirstLabState.PressureZero:0.00}牛";
            }));
            //////if (this.globaState.FirstLabState.Step != FirstLabStep.测试结束)
            //////{
            //////    //////////////////////
            //////    ForceLogger.WriteForceForFirstLab(this.fileNameForFirstLab, force, this.globaState.FirstLabState.PressureZero, force - this.globaState.FirstLabState.PressureZero);
            //////    //////////////////////
            //////}
        }
        #endregion

        #region 温控仪
        private void serialPort_Temperature_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("温控仪串口出错，错误类型：" + e.EventType.ToString(), "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_Temperature_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);
            
            byte[] buffer = new byte[10];//从机返回十个字节
            try
            {
                if (!SerialPortManager.SerialPort_Temperature.IsOpen) return;
                SerialPortManager.SerialPort_Temperature.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("温控仪读取错误：{0}",ex.Message));
                return;
            }

            Temperature_Reply reply = Temperature_ReplyAnalyzer.Analyse(buffer);
            float temperature = reply.Temperature;

            #region 2023-01-06 取消强行配置炉温
            //float settingTemperature = reply.SettingTemperature;

            //if (settingTemperature != Setting.SettingTemperature)
            //{
            //    Command command = CommandGenerator.Generate_SetFurnaceTargetTemperature(Setting.SettingTemperature);
            //    CommandExecutor.Send(SerialPortManager.SerialPort_Temperature, command);
            //}
            #endregion

            if (this.isTemperatureReached == true)
            {
                //温度达到是绿色
                this.lblCurrentTemperature.ForeColor = Color.Green;

                if (this.temperatureState.IsCountSend == false)
                {
                    Command command = CommandGenerator.Generate_TemperatureReached();
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                    this.temperatureState.IsCountSend = true;
                }

                this.isTemperatureReached = this.temperatureState.CountTime(temperature, Setting.SettingTemperature);
            }
            else
            {   
                //温度没达到是红色
                if (this.isTemperatureReached == false) this.lblCurrentTemperature.ForeColor = Color.Red;

                this.isTemperatureReached = this.temperatureState.CountTime(temperature, Setting.SettingTemperature);
            }

            #region 2023-01-06 温度显示整数
            this.Invoke(new Action(() => { this.lblCurrentTemperature.Text = ((int)temperature).ToString(); }));
            #endregion

        }
        #endregion

        #region 位移传感器
        private void InitSerialPort_Displacement()
        {
            this.Invoke(new Action(() =>
            {
                this.tlblPort_Displacement.Text = SerialPortManager.Displacement_COM;
                this.tlblPort_Displacement.ForeColor = Color.Green;
            }));

            bool isExists = false;

            SerialPortManager.OpenSerial_Displacement(ref isExists);

            if (isExists == false)
            {
                SerialPortManager.AddDataReceivedEventHandler_Displacement(new SerialDataReceivedEventHandler(serialPort_Displacement_DataReceived));
                SerialPortManager.AddErrorReceivedEventHandler_Displacement(new SerialErrorReceivedEventHandler(serialPort_Displacement_ErrorReceived));
            }
        }

        private void serialPort_Displacement_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("位移传感器串口出错，错误类型：" + e.EventType.ToString(), "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_Displacement_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(10);
            
            byte[] buffer = new byte[9];
            try
            {
                if (!SerialPortManager.SerialPort_Displacement.IsOpen) return;
                SerialPortManager.SerialPort_Displacement.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("位移传感器读取错误：{0}", ex.Message));
                return;
            }

            if (CRC.ToModbusCRC16(buffer) != "0000") return;
            Displacement_Reply reply = Displacement_ReplyAnalyzer.Analyse(buffer);
            this.Invoke(new Action(() => 
            {
                this.lblDisplacement.Text = $"位移:{reply.Displacement:0.00}mm";
                this.lblDebugInfo.Text = $"零点:{this.globaState.FouthLabState.DisplacementZero:0.00}mm，距离:{reply.Displacement - this.globaState.FouthLabState.DisplacementZero:0.00}mm";
            }));

            PointF point = new PointF(0f, 0f);
            this.ExecuteFouthLab(point, reply.Displacement);
        }

        #endregion

        private void btnSampleInfoSetting_Click(object sender, EventArgs e)
        {
            FormSampleInfo frmSpecimenSetting = new FormSampleInfo();
            DialogResult dialog = frmSpecimenSetting.ShowDialog();
            if (dialog == DialogResult.Cancel) return;

            this.txtSampleNo.Text = Setting.SpecimenNum;
            this.txtSampeName.Text = Setting.SpecimenName;
            this.txtInnerDiameter.Text = Setting.InnerDiameter.ToString();
            this.txtSpecimenDiameter.Text = Setting.SpecimenDiameter.ToString();
            this.txtSpecimenHeight.Text = Setting.SpecimenHeight.ToString();
            this.txtWorkerName.Text = Setting.ExperimentPerson;
            this.txtCompany.Text = Setting.ExperimentUnit;
            this.txtRepeatNumber.Text = Setting.RepeatTimes.ToString();
            this.btnStartTest.Enabled = true;

            #region 2023-01-06
            //if (this.isReset)
            //{
            //    Command command = CommandGenerator.Generate_EndTest();
            //    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

            //    this.isReset = false;
            //}
            #endregion
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FormSetting formParameterSetting = new FormSetting();
            DialogResult dialog = formParameterSetting.ShowDialog();
            if (dialog == DialogResult.Cancel) return;

            Setting.Load();

            this.lblMotorDis.Text = Setting.MotorIdlePath.ToString();
            this.lblHeatDis.Text = Setting.FurnaceFallingDistance.ToString();
            this.lblDispalcementMotorIdlePath.Text = Setting.DisplacementMotorIdlePath.ToString();
            this.lblPrePress.Text = Setting.PreloadedForce.ToString();
            this.SendLocation();
        }

        private void SendLocation()
        {
            Command command = null;
            switch (Setting.TestType)
            {
                case 0:
                case 1:
                case 2:
                    command = CommandGenerator.Generate_MotorDistanceSetting(Setting.MotorIdlePath);
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    Thread.Sleep(300);
                    break;
                case 3:
                    command = CommandGenerator.Generate_MotorDistanceSetting(Setting.DisplacementMotorIdlePath);
                    CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                    Thread.Sleep(300);
                    break;
            }
        }

        private void tmCheckTemperature_Tick(object sender, EventArgs e)
        {
            this.CheckCurrentTemperature();

            #region 2023-01-09

            if (this.globaState.IsStartTest && !this.globaState.IsEndTest && this.labOfSelected == Labs.ThirdLab)
            {
                if (this.globaState.ThirdLabState.Step == ThirdLabStep.采样数据并绘制压强时间曲线及判断压强值是否突变)
                {
                    float balanceTime = (float)(DateTime.Now - this.startTime).TotalSeconds;
                    this.txtBalancePressureTime.Text = ((int)balanceTime).ToString();
                }
            }

            #endregion
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            float currentTemperature = float.Parse(this.lblCurrentTemperature.Text);
            float settingTemperature = Setting.SettingTemperature;

            //if (currentTemperature <= settingTemperature * 0.95f || currentTemperature >= settingTemperature * 1.05f)
            //{
            //    DialogResult dlgResult = MessageBox.Show("当前炉温测量值不在设定炉温的范围内（上下5%的浮动），确定要开始实验吗？",
            //                                                                  "询问",
            //                                                                  MessageBoxButtons.YesNo,
            //                                                                  MessageBoxIcon.Question);

            //    if (dlgResult == DialogResult.No) return;
            //}

            Command command = CommandGenerator.Generate_BeginTest();
            CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

            //this.fileNameForFirstLab = ForceLogger.CreateLogFileForFirsLab();

            this.StartTest();
        }

        private void StartTest()
        {
            this.btnSettingFurnaceTemperature.Enabled = false;
            this.lblStatusTip.Text = "";
            this.btnSettings.Enabled = false;
            this.btnSampleInfoSetting.Enabled = false;
            this.isReachedGo = true;
            this.lblTimeCount.Text = Setting.SoakingTime.ToString();

            Setting.Points.Clear();

            switch (this.tabLabs.SelectedIndex)
            {
                case 0:
                    this.globaState.GoToFirstLab();
                    this.globaState.FirstLabState.Step = FirstLabStep.NONE;
                    this.chartPressure.Series[0].Points.Clear();
                    this.tmCheckTemperature.Start();
                    this.tmGetDisplacement.Stop();
                    break;

                case 1:
                    this.globaState.GoToSecondLab();
                    this.globaState.SecondLabState.Step = SecondLabStep.开始调试并发送指令;
                    this.chartPengZhang.Series[0].Points.Clear();
                    this.tmCheckTemperature.Start();
                    this.tmGetDisplacement.Stop();

                    this.lblPrePress.Text = Setting.PreloadedForce.ToString();

                    break;

                case 2:
                    this.globaState.GoToThreeStep();
                    this.globaState.ThirdLabState.Step = ThirdLabStep.开始测试并发送指令;
                    this.chartBalancePress.Series[0].Points.Clear();
                    this.tmCheckTemperature.Start();
                    this.tmGetDisplacement.Stop();
                    break;

                case 3:
                    this.InitSerialPort_Displacement();
                    this.globaState.GoToFourStep();
                    this.chartExpansionRate.Series[0].Points.Clear();
                    this.tmCheckTemperature.Stop();
                    this.tmGetDisplacement.Start();
                    break;
            }
            Setting.InitResult(this.tabLabs.SelectedIndex);
            this.btnStartTest.Enabled = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.Reset();
        }

        /// <summary>
        /// 复位
        /// </summary>
        private void Reset()
        {
            this.isStartDisplacement = false;

            Command command = CommandGenerator.Generate_EndTest();
            CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

            this.globaState.IsEndTest = true;
            this.isReachedGo = false;
            switch (Setting.TestType)
            {
                case 0:
                    this.globaState.FirstLabState.InitState();
                    break;
                case 1:
                    this.globaState.SecondLabState.InitState();
                    break;
                case 2:
                    this.globaState.ThirdLabState.InitState();
                    break;
                case 3:
                    this.globaState.FouthLabState.InitState();
                    break;
                default:
                    break;
            }

            this.Invoke(new Action(delegate()
            {
                this.lblPressure.Text = "0";
                this.lblPressureTime.Text = "0";
                this.lblPengZhangPower.Text = "0";
                this.lblPengZhangTime.Text = "0";

                this.txtBalancePressureTime.Text = "0";
                this.lblMaxExpansionRateTime.Text = "0";
                this.lblMaxExpansionRate.Text = "0";

                this.chartPressure.Series[0].Points.Clear();
                this.chartPengZhang.Series[0].Points.Clear();
                this.chartBalancePress.Series[0].Points.Clear();
                this.chartExpansionRate.Series[0].Points.Clear();

                this.lblTimeCount.Text = Setting.SoakingTime.ToString();
                this.lblPrePress.Text = Setting.PreloadedForce.ToString();
                this.lblPrePressure.Text = Setting.PreloadedPressure.ToString();

                this.btnSettings.Enabled = true;
                this.btnSampleInfoSetting.Enabled = true;
                this.btnStartTest.Enabled = true;
                this.btnSettingFurnaceTemperature.Enabled = true;
            }));

            Setting.Points.Clear();            
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (this.globaState.IsEndTest == false)
            {
                MessageBox.Show("正在进行测试!");
                return;
            }

        InitialSave:

            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.FileName = $"{Setting.TestTime}.{Setting.SpecimenNum}.{Setting.GetLabType()}-{Setting.RepeatTimes}";

            saveDlg.Filter = "文本格式|*.txt";
            string path = string.Empty;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                path = saveDlg.FileName;
                FileStream fileStream = new FileStream(path, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine($"Info实验类型:{Setting.GetLabType()}");
                streamWriter.WriteLine($"Info实验人员:{Setting.ExperimentPerson}");
                streamWriter.WriteLine($"Info实验时间:{Setting.TestTime}");
                streamWriter.WriteLine($"Info试样编号:{Setting.SpecimenNum}");
                streamWriter.WriteLine($"Info试样名称:{Setting.SpecimenName}");

                streamWriter.WriteLine($"Info试样尺寸:{this.txtSpecimenHeight.Text}");
                streamWriter.WriteLine($"Info设置温度:{Setting.SettingTemperature}℃");
                streamWriter.WriteLine($"Info来样单位:{Setting.ExperimentUnit}");
                streamWriter.WriteLine($"Info重复次数:{Setting.RepeatTimes}");

                switch (Setting.TestType)
                {
                    case 0:
                        streamWriter.WriteLine($"Info抗压强度:{this.lblPressure.Text}KPa");
                        streamWriter.WriteLine($"Info对应时间:{this.lblPressureTime.Text}S");
                        streamWriter.WriteLine($"Info保温时间:{Setting.SoakingTime}S");
                        break;
                    case 1:
                        streamWriter.WriteLine($"Info膨胀力值:{this.lblPengZhangPower.Text}N");
                        streamWriter.WriteLine($"Info对应时间:{this.lblPengZhangTime.Text}S");
                        streamWriter.WriteLine($"Info预载荷值:{Setting.PreloadedForce}N");
                        break;
                    case 2:
                        streamWriter.WriteLine($"Info对应时间:{this.txtBalancePressureTime.Text}S");
                        streamWriter.WriteLine($"Info设定强度:{Setting.PreloadedPressure}MPa");
                        break;
                    case 3:
                        streamWriter.WriteLine($"Info最大膨胀率:{this.lblMaxExpansionRate.Text}%");
                        streamWriter.WriteLine($"Info最大膨胀率时间:{this.lblMaxExpansionRateTime.Text}S");
                        break;
                }

                foreach (PointF point in Setting.Points)
                {
                    streamWriter.WriteLine($"X{point.X},Y{point.Y}");
                }

                streamWriter.Close();
                this.btnSettings.Enabled = true;
                MessageBox.Show("数据已保存,请设置相关信息进行再一次测试!");

                this.lblPressure.Text = "0";
                this.lblPressureTime.Text = "0";
                this.lblPengZhangPower.Text = "0";
                this.lblPengZhangTime.Text = "0";

                this.txtBalancePressureTime.Text = "0";
                this.lblPrePress.Text = "";
                this.lblPrePressure.Text = "";

                this.lblMaxExpansionRateTime.Text = "0";
                this.lblMaxExpansionRate.Text = "0";

                Setting.Points.Clear();
                this.chartPressure.Series[0].Points.Clear();
                this.chartPengZhang.Series[0].Points.Clear();
                this.chartBalancePress.Series[0].Points.Clear();
                this.chartExpansionRate.Series[0].Points.Clear();
                Setting.RepeatTimes++;
                this.txtRepeatNumber.Text = Setting.RepeatTimes.ToString();

            }
            else
            {
                DialogResult result = MessageBox.Show("确定不需要数据吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.btnSettings.Enabled = true;
                    MessageBox.Show("数据已取消保存，请设置相关信息进行下一次实验");

                    this.lblPressure.Text = "0";
                    this.lblPressureTime.Text = "0";
                    this.lblPengZhangPower.Text = "0";
                    this.lblPengZhangTime.Text = "0";

                    this.txtBalancePressureTime.Text = "0";
                    this.lblPrePress.Text = "";
                    this.lblPrePressure.Text = "";
                    this.lblMaxExpansionRateTime.Text = "0";
                    this.lblMaxExpansionRate.Text = "0";

                    Setting.Points.Clear();
                    this.chartPressure.Series[0].Points.Clear();
                    this.chartPengZhang.Series[0].Points.Clear();
                    this.chartBalancePress.Series[0].Points.Clear();
                    this.chartExpansionRate.Series[0].Points.Clear();
                }
                else
                {
                    goto InitialSave;
                }
            }
            this.lblTimeCount.Text = Setting.SoakingTime.ToString();

            this.lblPressure.Text = "0";
            this.lblPressureTime.Text = "0";

            this.lblPrePress.Text = Setting.PreloadedForce.ToString();
            this.lblPrePressure.Text = Setting.PreloadedPressure.ToString();
            this.btnSettings.Enabled = true;
            this.btnSampleInfoSetting.Enabled = true;
            this.btnStartTest.Enabled = true;
            this.btnSettingFurnaceTemperature.Enabled = true;
            this.btnSaveData.Enabled = false;
            this.isReachedGo = false;
        }

        private void btnDataReader_Click(object sender, EventArgs e)
        {
            FormDataReader frmDataReader = new FormDataReader();
            frmDataReader.ShowDialog();
        }

        private void btnCalibration_Click(object sender, EventArgs e)
        {  
            this.isReachedGo = true;

            SerialPortManager.RemoveDataReceivedEventHandler_Slave(this.serialPort_Slave_DataReceived);

            FormCalibration frmCalibration = new FormCalibration(SerialPortManager.SerialPort_Slave);
            frmCalibration.Revise = Setting.TxtRevise;
            DialogResult dlgResult = frmCalibration.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                Setting.Save("TxtRevise", frmCalibration.Revise.ToString());
                this.isReachedGo = false;
            }

            SerialPortManager.AddDataReceivedEventHandler_Slave(this.serialPort_Slave_DataReceived);
        }

        private void btnBackToZero_Click(object sender, EventArgs e)
        {
            Command command_EndTest = CommandGenerator.Generate_EndTest();
            CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command_EndTest);
        }

        private void selectTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Command command = null;
                //四选一(抗压强度，热稳定性，膨胀力，高温急热膨胀率)
                switch (this.tabLabs.SelectedIndex)
                {
                    case 0:
                        this.tmGetDisplacement.Stop();
                        this.InitSerialPort_Temperature();

                        this.labOfSelected = Labs.FirstLab;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        command = CommandGenerator.Generate_MotorDistanceSetting(Setting.MotorIdlePath);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                        Thread.Sleep(300);
                        break;
                    case 1:
                        this.tmGetDisplacement.Stop();
                        this.InitSerialPort_Temperature();

                        this.labOfSelected = Labs.SecondLab;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        command = CommandGenerator.Generate_MotorDistanceSetting(Setting.MotorIdlePath);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                        Thread.Sleep(300);
                        break;
                    case 2:
                        this.tmGetDisplacement.Stop();
                        this.InitSerialPort_Temperature();

                        this.labOfSelected = Labs.ThirdLab;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        command = CommandGenerator.Generate_MotorDistanceSetting(Setting.MotorIdlePath);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);
                        Thread.Sleep(300);
                        break;
                    case 3:
                        ////this.InitSerialPort_Displacement();

                        this.labOfSelected = Labs.FourthLab;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        command = CommandGenerator.Generate_MotorDistanceSetting(Setting.DisplacementMotorIdlePath);
                        CommandExecutor.Send(SerialPortManager.SerialPort_Slave, command);

                        Thread.Sleep(300);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"程序出现异常：{ex.Message}");
            }
        }

        /// <summary>
        /// 设定目标炉温
        /// </summary>
        /// <param name="targetTemperature">目标炉温</param>
        private void SetTargetTemperature(int targetTemperature)
        {
            this.InitSerialPort_Temperature();

            Command command = CommandGenerator.Generate_SetFurnaceTargetTemperature(targetTemperature);

            CommandExecutor.Send(SerialPortManager.SerialPort_Temperature, command);

            Setting.Save("SettingTemperature", this.txtTargetTemperature.Text);
            Setting.Load();
        }

        private void btnSettingFurnaceTemperature_Click(object sender, EventArgs e)
        {
            try
            {
                int targetTemperature = int.Parse(this.txtTargetTemperature.Text.Trim());

                if (targetTemperature < 500 || targetTemperature > 1200)
                {
                    MessageBox.Show("请输入介于500到1200的整数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                this.SetTargetTemperature(targetTemperature);

                MessageBox.Show("目标炉温已设定", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (FormatException)
            {
                MessageBox.Show("请输入介于500到1200的整数","异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (MessageBox.Show("确定要退出程序吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            DialogResult dlgResult = MessageBox.Show("启动调试功能，正在进行的流程将会终止，确定要进行该操作么？",
                                                                          "提示",
                                                                          MessageBoxButtons.YesNo,
                                                                          MessageBoxIcon.Question);

            if (dlgResult == DialogResult.No) return;

            if (SerialPortManager.SerialPort_Slave != null)
            {
                SerialPortManager.RemoveDataReceivedEventHandler_Slave(this.serialPort_Slave_DataReceived);
                SerialPortManager.RemoveErrorReceivedEventHandler_Slave(this.serialPort_Slave_ErrorReceived);
                SerialPortManager.CloseSlave();
            }

            if (SerialPortManager.SerialPort_Temperature != null)
            {
                SerialPortManager.RemoveDataReceivedEventHandler_Temperature(this.serialPort_Temperature_DataReceived);
                SerialPortManager.RemoveErrorReceivedEventHandler_Temperature(this.serialPort_Temperature_ErrorReceived);
                SerialPortManager.CloseTemperature();
            }

            if (SerialPortManager.SerialPort_Displacement != null)
            {
                SerialPortManager.RemoveDataReceivedEventHandler_Displacement(this.serialPort_Displacement_DataReceived);
                SerialPortManager.RemoveErrorReceivedEventHandler_Displacement(this.serialPort_Displacement_ErrorReceived);
                SerialPortManager.CloseDisplacement();
            }

            FormDebug frmDebug = new FormDebug();
            frmDebug.ShowDialog();

            SerialPortManager.AddDataReceivedEventHandler_Slave(this.serialPort_Slave_DataReceived);
            SerialPortManager.AddErrorReceivedEventHandler_Slave(this.serialPort_Slave_ErrorReceived);
            SerialPortManager.AddDataReceivedEventHandler_Temperature(this.serialPort_Temperature_DataReceived);
            SerialPortManager.AddErrorReceivedEventHandler_Temperature(this.serialPort_Temperature_ErrorReceived);
            SerialPortManager.AddDataReceivedEventHandler_Displacement(this.serialPort_Displacement_DataReceived);
            SerialPortManager.AddErrorReceivedEventHandler_Displacement(this.serialPort_Displacement_ErrorReceived);

        }

        private void tmGetDisplacement_Tick(object sender, EventArgs e)
        {
            if (SerialPortManager.SerialPort_Displacement == null) return;
            if (SerialPortManager.SerialPort_Displacement.IsOpen == false) return;
            
            if (this.isStartDisplacement == false) return;

            try
            {
                Command command = CommandGenerator.Generate_GetDisplacement();
                CommandExecutor.Send(SerialPortManager.SerialPort_Displacement, command);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"位移传感器写入故障：{ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormDataReader_Old frmDataReader = new FormDataReader_Old();
            frmDataReader.ShowDialog();
        }
    }
}
