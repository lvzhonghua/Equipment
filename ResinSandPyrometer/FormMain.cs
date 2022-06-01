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
using ResinSandPyrometer.Step;

namespace ResinSandPyrometer
{
    public partial class FormMain : Form
    {
        private SerialPort serialPort_DanPianJi = null;  //单片机串口

        private SerialPort serialPort_WenKongYi = null;  //温控仪串口

        private SerialPort serialPort_WeiYi = null;   //位移传感器串口

        private TemperatureState temperatureState = new TemperatureState();

        private GlobaState globaState = new GlobaState();

        private bool isTemperatureReached = false;//判断温度是否到达到

        private bool isFurnaceZero = false;//加热炉是否在零点

        private bool isMotorZero = false;//加载电机是否在零点

        private DateTime startTime;//开始时间

        private bool isReachedGo = false;

        public float displacementResult = 0f;

        private Steps stepOfSelected = Steps.FirstStep;

        private bool isStartDisplacement = false;

        private bool isReset = false;

        private System.Threading.Timer timerDisplacement = null;

        public FormMain()
        {
            InitializeComponent();
            Setting.Load();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.lblMotorDis.Text = Setting.MotorIdlePath.ToString();
            this.lblHeatDis.Text = Setting.FurnaceFallingDistance.ToString();
            this.txtTargetTemperature.Text = Setting.SettingTemperature.ToString();
            this.lblDispalcementMotorIdlePath.Text = Setting.DisplacementMotorIdlePath.ToString();
            this.btnStartTest.Enabled = false;
            this.btnSaveData.Enabled = false;
            this.timerDisplacement = new System.Threading.Timer(this.DisplacementWrite, null, 0, 200);

            this.OpenInitSeralPort();

            if (this.serialPort_DanPianJi == null) return;
            if (!this.serialPort_DanPianJi.IsOpen) return;

            try
            {
                this.SendLocation();
                Thread.Sleep(500);
                this.serialPort_DanPianJi.Write("#5");//复位
                this.isReset = true;

            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("单片机写入故障：{0}", ex.Message));
            }

            SerialPortManager.SetSerialPort(this.serialPort_DanPianJi, this.serialPort_WenKongYi,this.serialPort_WeiYi);
        }

        private void OpenInitSeralPort()
        {
            int portID_Dan = 3;
            int portID_Wen = 3;
            int portID_Wei = 3;

        InitCOMOne:
            try
            {
                this.InitserialPort_DanPianJi(portID_Dan);
                this.serialPort_DanPianJi.Open();

            }
            catch (IOException)
            {
                if (portID_Dan > 20)
                {
                    MessageBox.Show("串口无法打开", "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                portID_Dan++;
                //跳转
                goto InitCOMOne;
            }
            //catch (Exception ex)
            //{
            //    this.serialPort_Dan.Close();
            //    portID_Dan++;
            //    goto InitCOMOne;
            //}
            this.tsslblDan.Text = portID_Dan.ToString();
            this.tsslblDan.ForeColor = Color.Green;


        //控温仪
        InitCOMTwo:
            try
            {
                if (portID_Wen == portID_Dan)
                {
                    portID_Wen++;
                }
                this.InitSerialPort_Wen(portID_Wen);
                this.serialPort_WenKongYi.Open();
            }
            catch (IOException)
            {
                if (portID_Wen > 20)
                {
                    MessageBox.Show("串口无法打开", "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                portID_Wen++;

                //跳转
                goto InitCOMTwo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("温控仪异常：{0}", ex.Message));
            }

            this.tsslblWen.Text = portID_Wen.ToString();
            this.tsslblWen.ForeColor = Color.Green;
            ////让温控仪开始检查数据

            this.tmCheckTemperature.Enabled = true;
            WindowState = FormWindowState.Maximized;

        ////位移传感器
        InitCOMThree:
            try
            {
                if (portID_Wei == portID_Dan)
                {
                    portID_Wei++;
                }

                if (portID_Wei == portID_Wen)
                {
                    portID_Wei++;
                }

                this.InitSerialPort_Wei(portID_Wei);
                this.serialPort_WeiYi.Open();
            }
            catch (IOException)
            {
                if (portID_Wei > 20)
                {
                    MessageBox.Show("串口无法打开", "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                portID_Wei++;

                //跳转
                goto InitCOMThree;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("位移传感器异常：{0}",ex.Message));
            }
            this.tsslblWei.Text = portID_Wei.ToString();
            //isStartDisplacement = true;
            this.tsslblWei.ForeColor = Color.Green;
        }

        #region 单片机
        //初始化端片机端口
        private void InitserialPort_DanPianJi(int portID)
        {
            this.serialPort_DanPianJi = new SerialPort();
            this.serialPort_DanPianJi.PortName = string.Format("COM{0}", portID);//串口端口
            this.serialPort_DanPianJi.BaudRate = 115200;//波特率
            this.serialPort_DanPianJi.Parity = (Parity)Enum.Parse(typeof(Parity), (string)"None");//奇偶校验
            this.serialPort_DanPianJi.DataBits = 8;//数据位
            this.serialPort_DanPianJi.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
            this.serialPort_DanPianJi.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
            this.serialPort_DanPianJi.ReadTimeout = 2000; ;//超时读取异常
            //串口数据接收事件
            this.serialPort_DanPianJi.DataReceived += new SerialDataReceivedEventHandler(serialPort_DanPianJi_DataReceived);
            //串口出错事件处理
            this.serialPort_DanPianJi.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_DanPianJi_ErrorReceived);
        }

        private void serialPort_DanPianJi_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("单片机串口出错，错误类型：" + e.EventType.ToString(), "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_DanPianJi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);
            //获取串口传递过来数据（从串口读数据）
            byte[] buffer = new byte[5];
            try
            {
                if (!this.serialPort_DanPianJi.IsOpen) return;
                this.serialPort_DanPianJi.Read(buffer, 0, buffer.Length);
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

                    this.globaState.IsEndtest = true;
                    switch (Setting.TestType)
                    {
                        case 0:
                            this.globaState.FirstStepState.InitState();
                            break;
                        case 1:
                            this.globaState.SecondStepState.InitState();
                            break;
                        case 2:
                            this.globaState.ThirdStepState.InitState();
                            break;
                        case 3:
                            this.globaState.FouthStepState.InitState();
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
                    this.Invoke(new Action(delegate() { this.lblReceivedInfo.Text = $"重量：{force}牛"; }));
                    if (force > Setting.SensorMax)
                    {
                        this.Reset();
                        this.Invoke(new Action(delegate(){this.lblStatusTip.Text = "超过力传感器的极限值！！！";}));
                        return;
                    }

                    if (force < -3.1f || force == 0) return;

                    if (this.stepOfSelected == Steps.FourthStep) break;

                    if (this.globaState.IsStarttest && !this.globaState.IsEndtest)
                    {
                        switch (this.stepOfSelected)
                        {
                            case Steps.FirstStep:
                                this.ExecuteFirstStep(point, force);
                                break;
                            case Steps.SecondStep:
                                this.ExecuteSecondStep(point, force);
                                break;
                            case Steps.ThirdStep:
                                this.ExecuteThirdStep(point, force);
                                break;
                            default:
                                break;
                        }
                    }

                    break;
                case Slave_ReplyCode.E:   //力传感器通道错误
                    this.Invoke(new Action(delegate() { this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content;}));
                    this.lblStatusTip.Text = "传感器数据连接中断";
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
                    
                    if (!this.isReachedGo) return;

                    switch (stepOfSelected)
                    {
                        case Steps.FirstStep:
                            Setting.TestType = 0;
                            this.globaState.GoToFirstStep();
                            this.globaState.FirstStepState.Step = Common.FirstStep.加热炉按行程下降;
                            break;
                        case Steps.SecondStep:
                            Setting.TestType = 1;
                            this.globaState.GoToSecondStep();
                            this.globaState.SecondStepState.Step = SecondStep.开始调试并发送指令;
                            break;
                        case Steps.ThirdStep:
                            Setting.TestType = 2;
                            this.globaState.GoToThreeStep();
                            this.globaState.ThirdStepState.Step = ThirdStep.开始测试并发送指令;
                            break;
                        case Steps.FourthStep:
                            Setting.TestType = 3;
                            isStartDisplacement = true;
                            this.globaState.FouthStepState.Step = Common.FourthStep.取预置零点值;
                            break;
                        default:
                            break;
                    }
                    break;

                case Slave_ReplyCode.G:  //加热炉到达预设行程
                    this.Invoke(new Action(delegate () { this.lblReceivedInfo.Text = reply.Code + "  " + reply.Answer_Content; }));

                    switch (this.stepOfSelected)
                    {
                        case Steps.FirstStep:
                            this.globaState.FirstStepState.Step = FirstStep.取预置零点值;
                            break;
                        case Steps.SecondStep:
                            this.globaState.SecondStepState.Step = SecondStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变;
                            this.startTime = DateTime.Now;
                            break;
                        case Steps.ThirdStep:
                            this.globaState.ThirdStepState.Step = ThirdStep.采样数据并绘制压强时间曲线及判断压强值是否突变;
                            this.startTime = DateTime.Now;
                            break;
                        case Steps.FourthStep:
                            if (this.globaState.FouthStepState.IsWaitOver) return;
                           
                            this.globaState.FouthStepState.Step = FourthStep.更新零点值;
                            this.globaState.FouthStepState.IsWaitOver = false;

                            break;
                    }

                    break;
            }
        }

        //条件热稳定性
        private void ExecuteThirdStep(PointF point,float result)
        {
            switch (this.globaState.ThirdStepState.Step)
            {
                case ThirdStep.开始测试并发送指令:
                    this.globaState.TimeCount(2);//2s
                    this.globaState.GetPressZero(result);//零点值
                    if (this.globaState.IsTimeReached)
                    {
                        this.serialPort_DanPianJi.Write("#:1");
                        this.isMotorZero = false;
                        this.globaState.ThirdStepState.Step = ThirdStep.读取传感器采样值并判断预载荷是否达到预定值;
                        this.globaState.IsTimeReached = false;
                    }

                    this.btnSettingFurnaceTemperature.Enabled = false;

                    break;
                case ThirdStep.读取传感器采样值并判断预载荷是否达到预定值:
                    point.Y = (result - this.globaState.PressZero) / Setting.GetArea();
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblPrePressure.Text = Setting.PreloadedPressure.ToString();
                    });
                    if (point.Y > Setting.PreloadedPressure - 0.005f)
                    {
                        this.serialPort_DanPianJi.Write("#:0");
                        this.globaState.ThirdStepState.Step = ThirdStep.继续采样并保证在一定范围内;

                    }
                    break;
                case ThirdStep.继续采样并保证在一定范围内:
                    point.Y = (result - this.globaState.PressZero) / Setting.GetArea();

                    if (point.Y > Setting.PreloadedPressure + 0.005f)
                    {
                        this.serialPort_DanPianJi.Write("#90");
                    }
                    else if (point.Y < Setting.PreloadedPressure - 0.005f)
                    {
                        this.serialPort_DanPianJi.Write("#91");
                    }
                    else
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblPrePressure.Text = point.Y.ToString();
                        });
                        if (!this.globaState.ThirdStepState.IsBPGet)
                        {
                            this.globaState.ThirdStepState.BalancePress = point.Y;
                            this.globaState.ThirdStepState.IsBPGet = true;
                        }
                        if (!this.globaState.ThirdStepState.IsTimeReached)
                        {
                            this.globaState.ThirdStepState.TimeCount(2);//2s
                        }
                        else
                        {
                            this.globaState.ThirdStepState.Step = ThirdStep.加热炉下降;
                        }
                    }
                    break;
                case ThirdStep.加热炉下降:
                    string message = "#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString();
                    SampleLoggerOnTextFile.Log($"ThreeStep.加热炉下降，指令内容：{message}");

                    this.serialPort_DanPianJi.Write(message);
                    this.isFurnaceZero = false;
                    this.globaState.ThirdStepState.Step = ThirdStep.NONE;
                    break;
                case ThirdStep.采样数据并绘制压强时间曲线及判断压强值是否突变:
                    point.Y = (result - this.globaState.PressZero) / Setting.GetArea();
                    point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;

                    this.globaState.ThirdStepState.getBalanceTime(point.X);
                    Setting.Points.Add(point);

                    this.globaState.ThirdStepState.CheckBlancePressChange(point.Y, Setting.PreloadedPressure);//检查抗压强度是否突变
                    if (this.globaState.ThirdStepState.IsBalancePressSudChange)
                    {
                        this.globaState.ThirdStepState.BalanceTime = point.X;
                        this.globaState.ThirdStepState.Step = ThirdStep.突变并返回;
                    }
                    if (point.Y > Setting.PreloadedPressure + 0.005f)
                    {
                        this.serialPort_DanPianJi.Write("#90");
                    }
                    else if (point.Y < Setting.PreloadedPressure - 0.005f)
                    {
                        this.serialPort_DanPianJi.Write("#91");
                    }
                    else
                    {
                        this.Invoke((EventHandler)delegate
                        {

                            this.lblPrePressure.Text = point.Y.ToString();
                        });
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        this.txtBalancePressureTime.Text = BytesOperator.GetOneVaule(this.globaState.ThirdStepState.BalanceTime).ToString();
                        this.chartBalancePress.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到预压强--时间曲线中去
                    });
                    break;
                case ThirdStep.突变并返回:
                    this.globaState.ThirdStepState.Step = ThirdStep.测试结束;
                    break;
                case ThirdStep.测试结束:
                    this.serialPort_DanPianJi.Write("#4");
                    this.globaState.IsEndtest = true;
                    this.globaState.ThirdStepState.InitState();
                    this.globaState.PressZero = 0;
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;
                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    });
                    break;
                default:
                    break;
            }
        }

        //高温膨胀力
        private void ExecuteSecondStep(PointF point,float result)
        {
            switch (this.globaState.SecondStepState.Step)
            {

                case SecondStep.开始调试并发送指令:
                    this.globaState.TimeCount(2);//2s
                    this.globaState.GetPressZero(result);//零点值
                    if (this.globaState.IsTimeReached)
                    {
                        this.serialPort_DanPianJi.Write("#:1");
                        this.isMotorZero = false;
                        this.globaState.SecondStepState.Step = SecondStep.检测预载荷是否为10主机发送指令加载电机停止;
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnSettingFurnaceTemperature.Enabled = false;
                    });

                    //this.lblZeroGl.Text = this.globaState.PressZero.ToString();
                    break;
                case SecondStep.检测预载荷是否为10主机发送指令加载电机停止:
                    point.Y = result - this.globaState.PressZero;

                    if (point.Y > Setting.PreloadedForce - 0.5)
                    {
                        this.serialPort_DanPianJi.Write("#:0");
                        this.globaState.SecondStepState.Step = SecondStep.检测载荷是否为9到11并调节预载荷;
                    }
                    break;

                case SecondStep.检测载荷是否为9到11并调节预载荷:
                    point.Y = result - this.globaState.PressZero;
                    if (point.Y > Setting.PreloadedForce + 0.5)
                    {
                        this.serialPort_DanPianJi.Write("#90");
                    }
                    else if (point.Y < Setting.PreloadedForce - 0.5)
                    {
                        this.serialPort_DanPianJi.Write("#91");
                    }
                    else
                    {
                        this.globaState.SecondStepState.TimeCount(1.5f);
                        this.globaState.SecondStepState.GetPressZero(result);//零点值
                        if (this.globaState.SecondStepState.IsTimeReached)
                        {
                            this.globaState.SecondStepState.Step = SecondStep.加热炉下降;
                        }
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblPrePress.Text = point.Y.ToString();
                        });

                        //this.lblpzero.Text = this.globaState.TwoState.PressZero.ToString();
                    }

                    break;

                case SecondStep.加热炉下降:
                    string message = "#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString();
                    SampleLoggerOnTextFile.Log($"TwoStep.加热炉下降，指令内容：{message}");

                    this.serialPort_DanPianJi.Write(message);
                    this.isFurnaceZero = false;
                    this.globaState.SecondStepState.Step = SecondStep.NONE;
                    break;
                case SecondStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变:
                    point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (result - this.globaState.SecondStepState.PressZero);
                    Setting.Points.Add(point);
                    this.globaState.SecondStepState.GetMaxPress(point);//最大膨胀力和时间
                    this.globaState.SecondStepState.CheckPressSudChange(point.Y);//检查膨胀力是否突变
                    if (this.globaState.SecondStepState.IsPressSudChange)
                    {
                        this.globaState.SecondStepState.Step = SecondStep.结束测试;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.txtPengZhangPower.Text = BytesOperator.GetThreeValue(this.globaState.SecondStepState.MaxPress);
                        this.txtPengZhangTime.Text = BytesOperator.GetThreeValue(this.globaState.SecondStepState.MaxPress_Time);
                        this.chartPengZhang.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到膨胀力--时间曲线中去
                    });

                    break;
                case SecondStep.结束测试:
                    this.serialPort_DanPianJi.Write("#4");
                    this.globaState.IsEndtest = true;
                    this.globaState.IsTimeReached = false;
                    this.globaState.SecondStepState.InitState();
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;

                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    });
                    break;
                default:
                    break;
            }
        }

        // 测试高温抗压强度
        private void ExecuteFirstStep(PointF point,float result)
        {
            switch (this.globaState.FirstStepState.Step)
            {
                case FirstStep.加热炉按行程下降:
                    //string message = "#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + (Setting.FurnaceFallingDistance - 160).ToString();
                    string message = "#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString();

                    SampleLoggerOnTextFile.Log($"FirstStep.加热炉按行程下降，指令内容：{message}");
                    this.serialPort_DanPianJi.Write(message);//加热炉按行程下降

                    this.isFurnaceZero = false;
                    this.globaState.FirstStepState.Step = Common.FirstStep.NONE;
                    break;
                case FirstStep.取预置零点值:
                    this.globaState.FirstStepState.CommandCount(1);
                    this.globaState.FirstStepState.GetPressureZero(result);//零点值

                    if (this.globaState.FirstStepState.IsCommandReached)
                    {
                        this.serialPort_DanPianJi.Write("#:1");
                        this.globaState.FirstStepState.Step = Common.FirstStep.电机上升;
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblTimeCount.Text = this.globaState.FirstStepState.TimeCount(Setting.SoakingTime).ToString();//保温时间和延时
                    });
                    break;
                case FirstStep.电机上升:
                    this.Invoke(new Action(delegate () { this.lblDebugInfo.Text = $"压力：{Math.Abs(this.globaState.FirstStepState.PressureZero - result)}"; }));
                    SampleLoggerOnTextFile.Log($"压力：{ Math.Abs(this.globaState.FirstStepState.PressureZero - result)}");

                    if (Math.Abs(this.globaState.FirstStepState.PressureZero - result) >= 1)
                    {
                        this.serialPort_DanPianJi.Write("#:0");
                        SampleLoggerOnTextFile.Log("接触，电机停止上升");

                        this.globaState.FirstStepState.PressureZeroClear();
                        this.globaState.FirstStepState.Step = Common.FirstStep.电机下降;
                        this.globaState.FirstStepState.IsCommandReached = false;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.lblTimeCount.Text = this.globaState.FirstStepState.TimeCount(Setting.SoakingTime).ToString();//保温时间和延时
                    });
                    break;
                case FirstStep.电机下降:
                    this.serialPort_DanPianJi.Write("#93");
                    this.globaState.FirstStepState.Step = Common.FirstStep.抗压试样保温时间计时并更新零点值;
                    break;
                case FirstStep.抗压试样保温时间计时并更新零点值:
                    if (this.globaState.FirstStepState.countTime < 10)
                    {
                        this.globaState.FirstStepState.CommandCount(1);
                        this.globaState.FirstStepState.GetPressureZero(result);//零点值
                    }
                    
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblTimeCount.Text = this.globaState.FirstStepState.TimeCount(Setting.SoakingTime).ToString();//保温时间和延时
                    });

                    if (this.globaState.FirstStepState.IsTimeReached)
                    {
                        this.serialPort_DanPianJi.Write("#:1");
                        this.isMotorZero = false;
                        this.startTime = DateTime.Now;
                        this.globaState.FirstStepState.Step = Common.FirstStep.采集数据压力是否突变;
                        this.globaState.FirstStepState.IsTimeReached = false;
                    }

                    break;
                case FirstStep.采集数据压力是否突变:
                    point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = Convert.ToSingle((result - this.globaState.FirstStepState.PressureZero) / (Setting.GetArea() * 0.001));
                    Setting.Points.Add(point);

                    this.globaState.FirstStepState.GetMaxPressure(point);//得到最大压力值和对应的时间
                    this.globaState.FirstStepState.CheckPressureSubChange(point.Y);//检查抗压强强值是否过大

                    if (this.globaState.FirstStepState.IsPressureSudChange)//抗压强度值是否突变
                    {
                        this.serialPort_DanPianJi.Write("#:0");
                        this.globaState.FirstStepState.Step = Common.FirstStep.测试结束;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.lblPressure.Text = BytesOperator.GetOneVaule((this.globaState.FirstStepState.MaxPressure)).ToString();
                        this.lblPressureTime.Text = BytesOperator.GetOneVaule(this.globaState.FirstStepState.MaxPreesureTime).ToString();

                        this.chartPressure.Series[0].Points.AddXY(point.X, point.Y);//将点添加到压强--时间曲线中去
                    });
                    break;
                case FirstStep.测试结束:
                    this.serialPort_DanPianJi.Write("#4");
                    this.globaState.IsEndtest = true;

                    this.globaState.FirstStepState.InitState();
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;

                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    });
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region 温控仪
        private void InitSerialPort_Wen(int portID)
        {

            this.serialPort_WenKongYi = new SerialPort();
            this.serialPort_WenKongYi.PortName = string.Format("COM{0}", portID);//串口端口
            this.serialPort_WenKongYi.BaudRate = 19200;//波特率
            this.serialPort_WenKongYi.Parity = (Parity)Enum.Parse(typeof(Parity), (string)"None");//奇偶校验
            this.serialPort_WenKongYi.DataBits = 8;//数据位
            this.serialPort_WenKongYi.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
            this.serialPort_WenKongYi.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
            this.serialPort_WenKongYi.ReadTimeout = 2000; ;//超时读取异常

            //串口数据接收事件
            this.serialPort_WenKongYi.DataReceived += new SerialDataReceivedEventHandler(
                serialPort_WenKongYi_DataReceived);
            //串口出错事件处理
            this.serialPort_WenKongYi.ErrorReceived += new SerialErrorReceivedEventHandler(
                serialPort_Wen_ErrorReceived);
        }

        private void serialPort_Wen_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("温控仪串口出错，错误类型：" + e.EventType.ToString(), "错误信息",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_WenKongYi_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);
            
            byte[] buffer = new byte[10];//从机返回十个字节
            try
            {
                if (!this.serialPort_WenKongYi.IsOpen) return;
                this.serialPort_WenKongYi.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("温控仪读取错误：{0}",ex.Message));
                return;
            }

            byte[] tempBuffer = new byte[2];
            //低字节在前，高字节在后
            tempBuffer[0] = buffer[1];
            tempBuffer[1] = buffer[0];

            float temperature = 0.1f * NumberSystem.BinaryToDecimal_Complement(tempBuffer, 16);

            if (temperature < 10) return;

            if (this.isTemperatureReached == true)
            {
                //温度达到是绿色
                this.lblCurrentTemperature.ForeColor = Color.Green;
                //this.btnStart.Enabled = false;

                if (!this.temperatureState.IsCountSend)
                {
                    this.serialPort_DanPianJi.Write("#6");
                    this.temperatureState.IsCountSend = true;
                }

                this.isTemperatureReached = this.temperatureState.CountTime(temperature, Setting.SettingTemperature);
            }
            else
            {    //温度没达到是红色
                if (this.isTemperatureReached == false) this.lblCurrentTemperature.ForeColor = Color.Red;
                
                this.isTemperatureReached = this.temperatureState.CountTime(temperature, Setting.SettingTemperature);
            }

            this.Invoke(new Action(delegate(){ this.lblCurrentTemperature.Text = temperature.ToString();}));
        }
        #endregion

        #region 位移传感器
        private void InitSerialPort_Wei(int portID)
        {

            this.serialPort_WeiYi = new SerialPort();
            this.serialPort_WeiYi.PortName = string.Format("COM{0}", portID);//串口端口
            this.serialPort_WeiYi.BaudRate = 9600;//波特率
            this.serialPort_WeiYi.Parity = Parity.None;//奇偶校验
            this.serialPort_WeiYi.DataBits = 8;//数据位
            this.serialPort_WeiYi.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
            this.serialPort_WeiYi.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
            this.serialPort_WeiYi.ReadTimeout = 2000; ;//超时读取异常
            Thread threadWei = new Thread(new ThreadStart(delegate ()
              {
                //串口数据接收事件
                this.serialPort_WeiYi.DataReceived += new SerialDataReceivedEventHandler(
                    serialPort_Wei_DataReceived);
                //串口出错事件处理
                this.serialPort_WeiYi.ErrorReceived += new SerialErrorReceivedEventHandler(
                    serialPort_Wei_ErrorReceived);
              }));
            threadWei.IsBackground = true;
            threadWei.Start();
        }

        private void serialPort_Wei_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("位移传感器串口出错，错误类型：" + e.EventType.ToString(), "错误信息",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_Wei_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(10);
            
            byte[] buffer = new byte[9];
            try
            {
                if (!this.serialPort_WeiYi.IsOpen) return;
                this.serialPort_WeiYi.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("位移传感器读取错误：{0}", ex.Message));
                return;
            }

            if (CRC.ToModbusCRC16(buffer) != "0000") return;
            PointF point = new PointF(0f, 0f);
            displacementResult = GetDisplacement(buffer);
            this.Invoke((EventHandler)delegate
            {
                this.lblDisplacement.Text = displacementResult.ToString();
            });
            this.ExecuteFouthStep(point);

        }

        private void ExecuteFouthStep(PointF point)
        {
            switch (this.globaState.FouthStepState.Step)
            {
                case FourthStep.NONE:
                    break;
                case FourthStep.取预置零点值:
                    this.globaState.FouthStepState.TimeCount(1);
                    this.globaState.FouthStepState.GetDisplacementZero(displacementResult);//零点值
                    if (this.globaState.FouthStepState.IsTimeReached)
                    {
                        this.serialPort_DanPianJi.Write("#:1");
                        this.globaState.FouthStepState.Step = Common.FourthStep.电机上升;
                    }
                    break;
                case FourthStep.电机上升:

                    if (Math.Abs(this.globaState.FouthStepState.DisplacementZero - displacementResult) >= 2)
                    {
                        this.serialPort_DanPianJi.Write("#:0");
                        this.globaState.FouthStepState.DisplacementZeroClear();
                        this.globaState.FouthStepState.Step = Common.FourthStep.电机下降;
                        this.globaState.FouthStepState.IsTimeReached = false;
                    }
                    break;
                case FourthStep.电机下降:
                    this.serialPort_DanPianJi.Write("#92");
                    this.globaState.FouthStepState.Step = Common.FourthStep.加热炉下降;
                    break;
                case FourthStep.加热炉下降:
                    this.globaState.FouthStepState.Sleep();
                    if (this.globaState.FouthStepState.IsOK)
                    {
                        string message = "#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString();
                        SampleLoggerOnTextFile.Log($"FourStep.加热炉下降，指令内容：{message}");
                        this.serialPort_DanPianJi.Write(message);

                        this.isFurnaceZero = false;

                        this.globaState.FouthStepState.Step = Common.FourthStep.等待加热炉下降;
                    }
                    break;
                case FourthStep.等待加热炉下降:
                    this.globaState.FouthStepState.Wait();
                    if (this.globaState.FouthStepState.IsWaitOver)
                    {
                        this.globaState.FouthStepState.Step = Common.FourthStep.更新零点值;
                    }
                    break;
                case FourthStep.更新零点值:
                    this.globaState.FouthStepState.TimeCount(2);
                    this.globaState.FouthStepState.GetDisplacementZero(displacementResult);//零点值
                    if (this.globaState.FouthStepState.IsTimeReached)
                    {
                        this.startTime = DateTime.Now;
                        this.globaState.FouthStepState.Step = Common.FourthStep.采集位移数据并绘制曲线以及判断是否突变;
                        this.globaState.FouthStepState.IsTimeReached = false;
                    }
                    break;
                case FourthStep.采集位移数据并绘制曲线以及判断是否突变:
                    point.X = (int)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (float)Math.Round((displacementResult - this.globaState.FouthStepState.DisplacementZero) / Setting.SpecimenHeight * 100, 2);
                    Setting.Points.Add(point);

                    this.globaState.FouthStepState.GetMaxDisplacement(point);//得到最大压力值和对应的时间
                    this.globaState.FouthStepState.CheckDisplacementSubChange(point);
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblMaxExpansionRate.Text = this.globaState.FouthStepState.MaxDisplacement.ToString();
                        this.lblMaxExpansionRateTime.Text = BytesOperator.GetOneVaule(this.globaState.FouthStepState.MaxDisplacementTime).ToString();

                        this.chartExpansionRate.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到膨胀率--时间曲线中去
                    });
                    if (this.globaState.FouthStepState.IsDisplacementSudChange)//抗压强度值是否突变
                    {
                        this.serialPort_DanPianJi.Write("#4");
                        this.globaState.FouthStepState.Step = Common.FourthStep.测试结束;
                    }
                    break;
                case FourthStep.测试结束:
                    this.globaState.IsEndtest = true;
                    this.globaState.FouthStepState.IsTimeReached = false;
                    this.globaState.FouthStepState.InitState();
                    this.Invoke((EventHandler)delegate
                    {
                        //this.tmDisplacement.Stop();
                        //this.tmDisplacement.Enabled = false;
                        isStartDisplacement = false;
                        this.btnSettingFurnaceTemperature.Enabled = true;
                        this.btnSaveData.Enabled = true;
                        this.lblStatusTip.Text = "测试结束，请保存数据";
                    });
                    break;
                default:
                    break;
            }
        }


        private float GetDisplacement(byte[] buffer)
        {
            byte[] integerBytes = new byte[2];
            //低字节在前，高字节在后
            integerBytes[0] = buffer[4];
            integerBytes[1] = buffer[3];

            var h = (integerBytes[1] & 0xf0) >> 4; // 高位

            bool isSign = true;
            if (h == 8)
            {
                integerBytes[1] = Convert.ToByte(integerBytes[1] ^ 0x80);
                isSign = false;
            }

            int integerInt = BitConverter.ToInt16(integerBytes, 0);

            byte[] decimalsBytes = new byte[2];
            decimalsBytes[0] = buffer[6];
            decimalsBytes[1] = buffer[5];

            int decimalsInt = BitConverter.ToInt16(decimalsBytes, 0);

            string distanceStr = string.Format("{0}.{1}", integerInt, decimalsInt);

            float distance = float.Parse(distanceStr);

            string show = distance.ToString("0.00");

            if (isSign)
            {
                return float.Parse(show);
            }
            else
            {
                return 0 - float.Parse(show);
            }
        }
        #endregion

        private void btnSampleInfoSetting_Click(object sender, EventArgs e)
        {
            FormSampleInfo frmSpecimenSetting = new FormSampleInfo();
            DialogResult dialog = frmSpecimenSetting.ShowDialog();
            if (dialog == DialogResult.Cancel) return;

            this.txtSampleNo.Text = Setting.SpecimenNum;
            this.txtSampeName.Text = Setting.SpecimenName;
            this.txtSpecimenDiameter.Text = Setting.SpecimenDiameter.ToString();
            this.txtSpecimenHeight.Text = Setting.SpecimenHeight.ToString();
            this.txtWorkerName.Text = Setting.ExperimentPerson;
            this.txtCompany.Text = Setting.ExperimentUnit;
            this.txtRepeatNumber.Text = Setting.RepeatTimes.ToString();
            this.btnStartTest.Enabled = true;
            if (isReset)
            {
                this.serialPort_DanPianJi.Write("#4");
                isReset = false;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            FormSetting formParameterSetting = new FormSetting();
            DialogResult dialog = formParameterSetting.ShowDialog();
            if (dialog == DialogResult.Cancel) return;

            this.lblMotorDis.Text = Setting.MotorIdlePath.ToString();
            this.lblHeatDis.Text = Setting.FurnaceFallingDistance.ToString();
            this.lblDispalcementMotorIdlePath.Text = Setting.DisplacementMotorIdlePath.ToString();
            this.SendLocation();
        }

        private void SendLocation()
        {
            byte[] sendData = new byte[4];
            sendData[0] = (byte)'#'; // 0x23; // '#'字符
            sendData[1] = (byte)'<'; // 0x3C; // '<'字符
            byte[] numData = new byte[2];
            switch (Setting.TestType)
            {
                case 0:
                case 1:
                case 2:
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_DanPianJi.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                case 3:
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.DisplacementMotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_DanPianJi.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                default:
                    break;
            }
        }

        private void tmCheckTemperature_Tick(object sender, EventArgs e)
        {
            if (this.serialPort_WenKongYi == null) return;
            if (!this.serialPort_WenKongYi.IsOpen) return;
            try
            {
                Command command_GetFurnaceTemperature = CommandGenerator.Generate_GetFurnaceTemperature();
                byte[] buffer = command_GetFurnaceTemperature.Bytes;
                this.serialPort_WenKongYi.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"温控仪写入故障，异常：{ex.Message}");
            }
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            //if (!isFurnaceZero || !isMotorZero) 
            //{
            //    this.adviceLB.Text = "加热炉或者电机不在零点，请复位";
            //    return;
            //}
            
            this.serialPort_DanPianJi.Write("#3");
            
            this.StartTest();
           
            //this.serialPort_Dan.Write("#:0");
            
            //System.Threading.Thread.Sleep(10);

            //string message = "#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString();

            //this.serialPort_Dan.Write(message);
        }

        private void StartTest()
        {
            this.btnSettingFurnaceTemperature.Enabled = false;
            this.lblStatusTip.Text = "";
            this.btnSettings.Enabled = false;
            this.btnSampleInfoSetting.Enabled = false;
            this.isReachedGo = true;

            switch (this.tabLabs.SelectedIndex)
            {
                case 0:
                    this.globaState.GoToFirstStep();
                    this.globaState.FirstStepState.Step = Common.FirstStep.加热炉按行程下降;
                    break;
                case 1:
                    this.globaState.GoToSecondStep();

                    this.globaState.SecondStepState.Step = SecondStep.开始调试并发送指令;
                    break;
                case 2:
                    this.globaState.GoToThreeStep();
                    this.globaState.ThirdStepState.Step = ThirdStep.开始测试并发送指令;
                    break;
                case 3:
                    this.globaState.GoToFourStep();

                    break;
                default:
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
            this.serialPort_DanPianJi.Write("#4");
            //this.isReset = true;
            this.globaState.IsEndtest = true;
            this.isReachedGo = false;
            switch (Setting.TestType)
            {
                case 0:
                    this.globaState.FirstStepState.InitState();
                    break;
                case 1:
                    this.globaState.SecondStepState.InitState();
                    break;
                case 2:
                    this.globaState.ThirdStepState.InitState();
                    break;
                case 3:
                    this.globaState.FouthStepState.InitState();
                    break;
                default:
                    break;
            }

            this.Invoke(new Action(delegate()
            {
                this.lblPressure.Text = "0";
                this.lblPressureTime.Text = "0";
                this.txtPengZhangPower.Text = "0";
                this.txtPengZhangTime.Text = "0";

                this.txtBalancePressureTime.Text = "0";
                //this.lblPrePress.Text = "";
                //this.lblPrePressure.Text = "";
                this.lblMaxExpansionRateTime.Text = "0";
                this.lblMaxExpansionRate.Text = "0";

                this.chartPressure.Series[0].Points.Clear();
                this.chartPengZhang.Series[0].Points.Clear();
                this.chartBalancePress.Series[0].Points.Clear();
                this.chartExpansionRate.Series[0].Points.Clear();

                this.lblTimeCount.Text = Setting.SoakingTime.ToString();
                //this.lblPressZero.Text = "0";
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
            if (!this.globaState.IsEndtest)
            {
                MessageBox.Show("正在进行测试!");
                return;
            }

        InitialSave:

            SaveFileDialog saveDlg = new SaveFileDialog();
            //this.testSample.TstTime = System.DateTime.Now.ToString("f");

            saveDlg.FileName = Setting.TestTime + "." + Setting.SpecimenNum + "." + Setting.GetLabType() + "-" + Setting.RepeatTimes;

            saveDlg.Filter = "文本格式|*.txt";
            string path = string.Empty;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                path = saveDlg.FileName;
                FileStream fileStream = new FileStream(path, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine("Info实验类型:" + Setting.GetLabType());
                streamWriter.WriteLine("Info实验人员:" + Setting.ExperimentPerson);
                streamWriter.WriteLine("Info实验时间:" + Setting.TestTime);
                streamWriter.WriteLine("Info试样编号:" + Setting.SpecimenNum);
                streamWriter.WriteLine("Info试样名称:" + Setting.SpecimenName);

                streamWriter.WriteLine("Info试样尺寸:" + this.txtSpecimenHeight.Text);
                streamWriter.WriteLine("Info设置温度:" + Setting.SettingTemperature + "℃");
                streamWriter.WriteLine("Info来样单位:" + Setting.ExperimentUnit);
                streamWriter.WriteLine("Info重复次数:" + Setting.RepeatTimes);

                switch (Setting.TestType)
                {
                    case 0:
                        streamWriter.WriteLine("Info抗压强度:" + this.lblPressure.Text + "KPa");
                        streamWriter.WriteLine("Info对应时间:" + this.lblPressureTime.Text + "S");
                        streamWriter.WriteLine("Info保温时间:" + Setting.SoakingTime.ToString() + "S");
                        break;
                    case 1:
                        streamWriter.WriteLine("Info膨胀力值:" + this.txtPengZhangPower.Text + "N");
                        streamWriter.WriteLine("Info对应时间:" + this.txtPengZhangTime.Text + "S");
                        streamWriter.WriteLine("Info预载荷值:" + Setting.PreloadedForce.ToString() + "N");
                        break;
                    case 2:
                        streamWriter.WriteLine("Info对应时间:" + this.txtBalancePressureTime.Text + "S");
                        streamWriter.WriteLine("Info设定强度:" + Setting.PreloadedPressure.ToString() + "MPa");
                        break;
                    case 3:
                        streamWriter.WriteLine("Info最大膨胀率:" + this.lblMaxExpansionRate.Text + "%");
                        streamWriter.WriteLine("Info最大膨胀率时间:" + this.lblMaxExpansionRateTime.Text + "S");
                        break;
                }

                foreach (PointF point in Setting.Points)
                {
                    streamWriter.WriteLine("X" + point.X.ToString() + "," + "Y" + point.Y.ToString());
                }

                streamWriter.Close();
                this.btnSettings.Enabled = true;
                MessageBox.Show("数据已保存,请设置相关信息进行再一次测试!");

                this.lblPressure.Text = "0";
                this.lblPressureTime.Text = "0";
                this.txtPengZhangPower.Text = "0";
                this.txtPengZhangTime.Text = "0";

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
                DialogResult result = MessageBox.Show("确定不需要数据吗？？？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.btnSettings.Enabled = true;
                    MessageBox.Show("数据已取消保存，请设置相关信息进行下一次实验");

                    this.lblPressure.Text = "0";
                    this.lblPressureTime.Text = "0";
                    this.txtPengZhangPower.Text = "0";
                    this.txtPengZhangTime.Text = "0";

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

            this.lblTimeCount.Text = "0";
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

            SerialPortManager.RemoveDataReceivedEventHandler_DanPianJi(this.serialPort_DanPianJi_DataReceived);

            FormCalibration frmCalibration = new FormCalibration(this.serialPort_DanPianJi);
            frmCalibration.Revise = Setting.TxtRevise;
            DialogResult dlgResult = frmCalibration.ShowDialog();

            this.globaState.TimeCount(2);

            if (dlgResult == DialogResult.OK)
            {
                Setting.Save("TxtRevise", frmCalibration.Revise.ToString());
                this.isReachedGo = false;
            }

            SerialPortManager.AddDataReceivedEventHandler_DanPianJi(this.serialPort_DanPianJi_DataReceived);
        }

        private void btnBackToZero_Click(object sender, EventArgs e)
        {
            Command command_EndTest = CommandGenerator.Generate_EndTest();
            byte[] buffer = command_EndTest.Bytes;

            this.serialPort_DanPianJi.Write(buffer, 0, buffer.Length);
        }

        private void selectTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                byte[] sendData = new byte[4];
                sendData[0] = (byte)'#'; // 0x23; // '#'字符
                sendData[1] = (byte)'<'; // 0x3C; // '<'字符
                byte[] numData = new byte[2];
                //四选一(抗压强度，热稳定性，膨胀力，高温急热膨胀率)
                switch (this.tabLabs.SelectedIndex)
                {
                    case 0:
                        this.stepOfSelected = Steps.FirstStep;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                        sendData[2] = numData[1];
                        sendData[3] = numData[0];
                        this.serialPort_DanPianJi.Write(sendData, 0, 4);
                        Thread.Sleep(300);
                        break;
                    case 1:
                        this.stepOfSelected = Steps.SecondStep;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                        sendData[2] = numData[1];
                        sendData[3] = numData[0];
                        this.serialPort_DanPianJi.Write(sendData, 0, 4);
                        Thread.Sleep(300);
                        break;
                    case 2:
                        this.stepOfSelected = Steps.ThirdStep;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                        sendData[2] = numData[1];
                        sendData[3] = numData[0];
                        this.serialPort_DanPianJi.Write(sendData, 0, 4);
                        Thread.Sleep(300);
                        break;
                    case 3:
                        this.stepOfSelected = Steps.FourthStep;
                        this.isReachedGo = false;
                        Setting.RepeatTimes = 1;
                        this.txtRepeatNumber.Text = "1";
                        numData = BitConverter.GetBytes(Convert.ToInt32(Setting.DisplacementMotorIdlePath * 2500));
                        sendData[2] = numData[1];
                        sendData[3] = numData[0];
                        this.serialPort_DanPianJi.Write(sendData, 0, 4);
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

        private int clickNumber;//定义温度设置按钮点击次数
        private void btnSettingFurnaceTemperature_Click(object sender, EventArgs e)
        {
            if (this.clickNumber % 2 == 0)
            {
                if (this.temperatureState.IsTemperatureReset)
                {
                    return;
                }
                //手动控制指令
                byte[] handControllBuffer = new byte[]{0x81,0x81,0x43,0x18,0x00,0x00,0x44,0x18};

                //手动控制输出值设置指令
                byte[] handSetOutBufer = new byte[]{0x81,0x81,0x43,0x1A,0x00,0x00,0x44,0x1A};

                if (!this.serialPort_WenKongYi.IsOpen)
                {
                    this.serialPort_WenKongYi.Open();
                }

                this.serialPort_WenKongYi.Write(handControllBuffer, 0, handControllBuffer.Length);
                Thread.Sleep(500);

                this.temperatureState.IsCanStartHeat = true;
                this.temperatureState.IsTemperatureReset = true;
                this.txtTargetTemperature.ReadOnly = false;
                this.btnSettingFurnaceTemperature.Text = "确定";
                this.txtTargetTemperature.ForeColor = Color.White;
                this.serialPort_WenKongYi.Write(handSetOutBufer, 0, handSetOutBufer.Length);
                Setting.Save("SettingTemperature", this.txtTargetTemperature.Text);
            }
            else
            {
                //自动控制指令
                byte[] autorunBuffer = new byte[]{0x81,0x81,0x43,0x18,0x01,0x00,0x45,0x18 };

                if (!this.serialPort_WenKongYi.IsOpen)
                {
                    this.serialPort_WenKongYi.Open();
                }

                if (this.txtTargetTemperature.Text != null && Setting.SettingTemperature > 0 && Setting.SettingTemperature <= 1200)
                {
                    this.serialPort_WenKongYi.Write(NumberSystem.TemperatureToByte8(int.Parse(this.txtTargetTemperature.Text)), 0, 8);
                    Thread.Sleep(500);

                    this.temperatureState = new TemperatureState();
                    this.isTemperatureReached = false;
                    this.txtTargetTemperature.ReadOnly = true;//设定温度为只读
                    this.btnSettingFurnaceTemperature.Text = "设置温度";

                    this.serialPort_WenKongYi.Write(autorunBuffer, 0, autorunBuffer.Length);
                    //cf.ReadConfig("SetTemperature");
                    this.txtTargetTemperature.ForeColor = Color.Yellow;
                    Setting.Save("SettingTemperature", this.txtTargetTemperature.Text);
                    //this.btnStartHeat.Enabled = false ;

                    //cf.SaveConfig("SetTemperature", this.testSample.SetTemperature.ToString());
                }

                else
                {
                    MessageBox.Show("请输入正确的数字");
                    this.txtTargetTemperature.Text = "1000";
                }
                this.btnSettingFurnaceTemperature.Enabled = false;
            }
            this.clickNumber++;

        }

        private void DisplacementWrite(object state)
        {
            if (this.serialPort_WeiYi == null) return;
            if (this.serialPort_WeiYi.IsOpen && isStartDisplacement)
            {
                try
                {
                    Command command_GetDisplacement = CommandGenerator.Generate_GetDisplacement();
                    byte[] buffer = command_GetDisplacement.Bytes;
                    this.serialPort_WeiYi.Write(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    SampleLoggerOnTextFile.Log($"位移传感器写入故障：{ex.Message}");
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (MessageBox.Show("确定要退出程序吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            FormDebug frmDebug = new FormDebug();
            frmDebug.ShowDialog();
        }
    }
}
