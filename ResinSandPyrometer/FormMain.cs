using ResinSandPyrometer.Common;
using ResinSandPyrometer.Step;
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

namespace ResinSandPyrometer
{
    public partial class FormMain : Form
    {
        private SerialPort serialPort_Dan = null;//声明单片机串口

        //定义温控仪
        private SerialPort serialPort_Wen = null;

        private SerialPort serialPort_Wei = null;

        private TemperatureState temState = new TemperatureState();

        private GlobaState globaState = new GlobaState();

        private bool isTemReached = false;//判断温度是否到达到

        private bool isFurnaceZero = false;//加热炉是否在零点

        private bool isMotorZero = false;//加载电机是否在零点

        private DateTime startTime;//开始时间

        private bool isReachedGo = false;

        public float result;

        public float displacementResult = 0f;

        private EnumOfSetPara enumOfSetPara = EnumOfSetPara.NONE;

        private byte[] checkTemBuffer = new byte[] { 0x81, 0x81, 0x52, 0x00, 0x00, 0x00, 0x53, 0x00 };//温控仪查询指令;

        private byte[] checkDisplacementBuffer = new byte[]{
            0x01,0x04,0x00,0x04,0x00,0x02,0x30,0x0A};//位移传感器查询位置的说明

        private StepEnum selectIndex = StepEnum.FirstStep;

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
            this.setTemTxt.Text = Setting.SettingTemperature.ToString();
            this.lblDispalcementMotorIdlePath.Text = Setting.DisplacementMotorIdlePath.ToString();
            this.btnStart.Enabled = false;
            this.btnSave.Enabled = false;
            timerDisplacement = new System.Threading.Timer(DisplacementWrite, null, 0, 200);
            this.OpenInitseralPort();

            if (this.serialPort_Dan == null) return;
            if (!this.serialPort_Dan.IsOpen) return;

            try
            {
                this.SendLocation();
                Thread.Sleep(500);
                this.serialPort_Dan.Write("#5");//复位
                this.isReset = true;

            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("单片机写入故障：{0}", ex.Message));
                return;
            }

        }

        

        private void OpenInitseralPort()
        {
            int portID_Dan = 3;
            int portID_Wen = 3;
            int portID_Wei = 3;

        InitCOMOne:
            try
            {
                this.InitserialPort_Dan(portID_Dan);
                this.serialPort_Dan.Open();

            }
            catch (IOException)
            {
                if (portID_Dan > 20)
                {
                    MessageBox.Show("串口无法打开", "连接错误",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.serialPort_Wen.Open();
            }
            catch (IOException)
            {
                if (portID_Wen > 20)
                {
                    MessageBox.Show("串口无法打开", "连接错误",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                portID_Wen++;

                //跳转
                goto InitCOMTwo;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("温控仪：{0}", ex.Message));
            }

            this.tsslblWen.Text = portID_Wen.ToString();
            this.tsslblWen.ForeColor = Color.Green;
            ////让温控仪开始检查数据

            this.tmCheckTem.Enabled = true;
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
                this.serialPort_Wei.Open();
            }
            catch (IOException)
            {
                if (portID_Wei > 20)
                {
                    MessageBox.Show("串口无法打开", "连接错误",
                                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                portID_Wei++;

                //跳转
                goto InitCOMThree;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("位移传感器：{0}",ex.Message));
            }
            this.tsslblWei.Text = portID_Wei.ToString();
            //isStartDisplacement = true;
            this.tsslblWei.ForeColor = Color.Green;
        }

        #region 单片机
        //初始化端片机端口
        private void InitserialPort_Dan(int portID)
        {
            this.serialPort_Dan = new SerialPort();
            this.serialPort_Dan.PortName = string.Format("COM{0}", portID);//串口端口
            this.serialPort_Dan.BaudRate = 115200;//波特率
            this.serialPort_Dan.Parity = (Parity)Enum.Parse(typeof(Parity), (string)"None");//奇偶校验
            this.serialPort_Dan.DataBits = 8;//数据位
            this.serialPort_Dan.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
            this.serialPort_Dan.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
            this.serialPort_Dan.ReadTimeout = 2000; ;//超时读取异常
            //串口数据接收事件
            this.serialPort_Dan.DataReceived += new SerialDataReceivedEventHandler(serialPort_Dan_DataReceived);
            //串口出错事件处理
            this.serialPort_Dan.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_Dan_ErrorReceived);
        }

        private void serialPort_Dan_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("单片机串口出错，错误类型：" + e.EventType.ToString(), "错误信息",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_Dan_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);
            //获取串口传递过来数据（从串口读数据）1111
            byte[] buffer = new byte[5];
            try
            {
                if (!this.serialPort_Dan.IsOpen) return;
                this.serialPort_Dan.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("单片机读取错误：{0}",ex.Message));
                return;
            }

            PointF point = new PointF(0f, 0f);

            Command temCom = new Command(buffer);


            switch (temCom.CmdCode)
            {
                case CommandCode.R:

                    //if (!isFurnaceZero || !isMotorZero)
                    //{
                    //    this.adviceLB.Text = "加热炉或者电机不在零点，请复位";
                    //    return;
                    //}
                    
                    this.Invoke((EventHandler)delegate
                    {
                        this.StartTest();
                        this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();
                    });

                    break;
                case CommandCode.C:
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();
                    });

                    this.globaState.IsEndtest = true;
                    switch (Setting.TestType)
                    {
                        case 0:
                            this.globaState.OneState.InitState();
                            break;
                        case 1:
                            this.globaState.TwoState.InitState();
                            break;
                        case 2:
                            this.globaState.ThreeState.InitState();
                            break;
                        case 3:
                            this.globaState.FourState.InitState();
                            break;
                        default:
                            break;
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        this.setDeviceBtn.Enabled = true;
                        this.setParaBtn.Enabled = true;
                        this.btnStart.Enabled = true;
                        this.btnStartHeat.Enabled = true;
                    });
                    break;

                case CommandCode.F:

                    result = (float)(temCom.GetData() * Setting.TxtRevise * Setting.SensorMax * (2500 / Setting.SensorMV) / (8388607 * 128 * Setting.SensorSys));
                    //this.lblMessage.Text = (( Convert.ToSingle(cf.ReadConfig("SensorMax"))) / (Convert.ToSingle(cf.ReadConfig("SensorMV")) * Convert.ToSingle(cf.ReadConfig("SensorSys")))).ToString();

                    if (result > Setting.SensorMax)
                    {
                        this.Restoration();
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblReceivedInfo.Text = temCom.CmdCode + "  " + result.ToString()+" "+ temCom.GetDataString();
                            this.adviceLB.Text = "超过力传感器的极限值！！！";
                        });
                        return;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = temCom.CmdCode + " " + result.ToString();
                    });

                    if (result < -3.1f || result == 0) return;

                    if (this.selectIndex == StepEnum.FourStep) break;

                    if (this.globaState.IsStarttest && !this.globaState.IsEndtest){
                        switch (this.selectIndex)
                        {
                            case StepEnum.FirstStep:
                                this.FirstStep(point, result);
                                break;
                            case StepEnum.TwoStep:
                                this.TwoStep(point, result);
                                break;
                            case StepEnum.ThreeStep:
                                this.ThreeStep(point, result);
                                break;
                            default:
                                break;
                        }

                    }

                    break;
                case CommandCode.E:
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();

                    });
                    this.adviceLB.Text = "传感器数据连接中断";
                    break;
                case CommandCode.O:
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();

                    });
                    break;

                case CommandCode.K:

                    //if (isReset)
                    //{
                    //    isReset = false;
                    //    this.SendLocation();
                    //    Thread.Sleep(500);
                    //    //this.serialPort_Dan.Write("#5");
                    //}
                    if (temCom.GetDataString() == "000")
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();
                            this.adviceLB.Text = "加热炉到零位";
                        });
                        
                        this.isFurnaceZero = true;
                        
                    }
                    else if(temCom.GetDataString() == "100")
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();
                            this.adviceLB.Text = "加热炉到下限位";
                        });
                        this.isFurnaceZero = false;
                    }
                    
                    break;
                case CommandCode.L:
                    
                    if (temCom.GetDataString() == "100")
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();

                            this.adviceLB.Text = "加载电机到上限位";
                        });
                        this.isMotorZero = false;
                        
                    }
                    else if(temCom.GetDataString() == "000")
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();

                            this.adviceLB.Text = "加载电机到零位";
                        });
                        this.isMotorZero = true;
                    }
                    break;

                case CommandCode.P:

                    this.Invoke((EventHandler)delegate
                    {
                        this.adviceLB.Text = "";
                        this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();
                        if (this.btnJiao.Text == "标定")
                        {

                        }
                    });

                    if (!isReachedGo) return;

                    switch (selectIndex)
                    {
                        case StepEnum.FirstStep:
                            Setting.TestType = 0;
                            this.globaState.GoToFirstStep();
                            this.globaState.OneState.EnumFirst = EnumOfFirstStep.加热炉按行程下降;
                            break;
                        case StepEnum.TwoStep:
                            Setting.TestType = 1;
                            this.globaState.GoToSecondStep();
                            this.globaState.TwoState.EnumTwo = EnumOfTwoStep.开始调试并发送指令;
                            break;
                        case StepEnum.ThreeStep:
                            Setting.TestType = 2;
                            this.globaState.GoToThreeStep();
                            this.globaState.ThreeState.EnumThree = EnumOfThreeStep.开始测试并发送指令;
                            break;
                        case StepEnum.FourStep:
                            Setting.TestType = 3;
                            isStartDisplacement = true;
                            this.globaState.FourState.EnumFour = EnumOfFourStep.取预置零点值;
                            break;
                        default:
                            break;
                    }
                    break;


                case CommandCode.G:
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = temCom.CmdCode + "  " + temCom.GetDataString();

                    });
                    switch (this.selectIndex)
                    {
                        case StepEnum.FirstStep:
                            this.globaState.OneState.EnumFirst = EnumOfFirstStep.取预置零点值;
                            break;
                        case StepEnum.TwoStep:
                            this.globaState.TwoState.EnumTwo = EnumOfTwoStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变;
                            this.startTime = DateTime.Now;
                            break;
                        case StepEnum.ThreeStep:
                            this.globaState.ThreeState.EnumThree = EnumOfThreeStep.采样数据并绘制压强时间曲线及判断压强值是否突变;
                            this.startTime = DateTime.Now;
                            break;
                        case StepEnum.FourStep:
                            if (this.globaState.FourState.IsWaitOver) return;
                           
                            this.globaState.FourState.EnumFour = EnumOfFourStep.更新零点值;
                            this.globaState.FourState.IsWaitOver = false;

                            break;
                        default:
                            break;
                    }

                    break;

                case CommandCode.Sharp:
                    switch (this.enumOfSetPara)
                    {
                        case EnumOfSetPara.设置加载电机加载速度:
                            this.serialPort_Dan.Write("#=" + Setting.MotorLoadingSpeed);
                            this.enumOfSetPara = EnumOfSetPara.设置完毕;
                            break;
                        case EnumOfSetPara.设置完毕:
                            this.enumOfSetPara = EnumOfSetPara.NONE;
                            this.btnStart.Enabled = true;
                            break;
                        default:
                            break;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = "#" + temCom.GetDataString();

                    });
                    break;
                //case CommandCode._7:
                //    this.Invoke((EventHandler)delegate
                //    {
                //        this.lblReceivedInfo.Text = temCom.GetDataString();

                //    });
                //    Thread.Sleep(20);
                //    this.serialPort_Dan.Write("#5");

                //    break;
                case CommandCode.Error:
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblReceivedInfo.Text = "?  " + temCom.GetDataString();

                    });
                    break;
                default:
                    break;

            }
        }

        //条件热稳定性
        private void ThreeStep(PointF point,float result)
        {
            switch (this.globaState.ThreeState.EnumThree)
            {
                case EnumOfThreeStep.开始测试并发送指令:
                    this.globaState.TimeCount(2);//2s
                    this.globaState.GetPressZero(result);//零点值
                    if (this.globaState.IsTimeReached)
                    {
                        this.serialPort_Dan.Write("#:1");
                        this.isMotorZero = false;
                        this.globaState.ThreeState.EnumThree = EnumOfThreeStep.读取传感器采样值并判断预载荷是否达到预定值;
                        this.globaState.IsTimeReached = false;
                    }

                    this.btnStartHeat.Enabled = false;

                    break;
                case EnumOfThreeStep.读取传感器采样值并判断预载荷是否达到预定值:
                    point.Y = (result - this.globaState.PressZero) / Setting.GetArea();
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblPrePressure.Text = Setting.PreloadedPressure.ToString();
                    });
                    if (point.Y > Setting.PreloadedPressure - 0.005f)
                    {
                        this.serialPort_Dan.Write("#:0");
                        this.globaState.ThreeState.EnumThree = EnumOfThreeStep.继续采样并保证在一定范围内;

                    }
                    break;
                case EnumOfThreeStep.继续采样并保证在一定范围内:
                    point.Y = (result - this.globaState.PressZero) / Setting.GetArea();

                    if (point.Y > Setting.PreloadedPressure + 0.005f)
                    {
                        this.serialPort_Dan.Write("#90");
                    }
                    else if (point.Y < Setting.PreloadedPressure - 0.005f)
                    {
                        this.serialPort_Dan.Write("#91");
                    }
                    else
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblPrePressure.Text = point.Y.ToString();
                        });
                        if (!this.globaState.ThreeState.IsBPGet)
                        {
                            this.globaState.ThreeState.BalancePress = point.Y;
                            this.globaState.ThreeState.IsBPGet = true;
                        }
                        if (!this.globaState.ThreeState.IsTimeReached)
                        {
                            this.globaState.ThreeState.TimeCount(2);//2s
                        }
                        else
                        {
                            this.globaState.ThreeState.EnumThree = EnumOfThreeStep.加热炉下降;
                        }
                    }
                    break;
                case EnumOfThreeStep.加热炉下降:
                    this.serialPort_Dan.Write("#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString());
                    this.isFurnaceZero = false;
                    this.globaState.ThreeState.EnumThree = EnumOfThreeStep.NONE;
                    break;
                case EnumOfThreeStep.采样数据并绘制压强时间曲线及判断压强值是否突变:
                    point.Y = (result - this.globaState.PressZero) / Setting.GetArea();
                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;

                    this.globaState.ThreeState.getBalanceTime(point.X);
                    Setting.Points.Add(point);

                    this.globaState.ThreeState.CheckBlancePressChange(point.Y, Setting.PreloadedPressure);//检查抗压强度是否突变
                    if (this.globaState.ThreeState.IsBalancePressSudChange)
                    {
                        this.globaState.ThreeState.BalanceTime = point.X;
                        this.globaState.ThreeState.EnumThree = EnumOfThreeStep.突变并返回;
                    }
                    if (point.Y > Setting.PreloadedPressure + 0.005f)
                    {
                        this.serialPort_Dan.Write("#90");
                    }
                    else if (point.Y < Setting.PreloadedPressure - 0.005f)
                    {
                        this.serialPort_Dan.Write("#91");
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

                        this.txtBalancePressureTime.Text = BytesOperator.GetOneVaule(this.globaState.ThreeState.BalanceTime).ToString();
                        this.chartBalancePress.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到预压强--时间曲线中去
                    });
                    break;
                case EnumOfThreeStep.突变并返回:
                    
                    this.globaState.ThreeState.EnumThree = EnumOfThreeStep.测试结束;
                    break;
                case EnumOfThreeStep.测试结束:
                    this.serialPort_Dan.Write("#4");
                    this.globaState.IsEndtest = true;
                    this.globaState.ThreeState.InitState();
                    this.globaState.PressZero = 0;
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnStartHeat.Enabled = true;
                        this.btnSave.Enabled = true;
                        this.adviceLB.Text = "测试结束，请保存数据";
                    });
                    break;
                default:
                    break;
            }
        }

        //高温膨胀力
        private void TwoStep(PointF point,float result)
        {
            switch (this.globaState.TwoState.EnumTwo)
            {

                case EnumOfTwoStep.开始调试并发送指令:
                    this.globaState.TimeCount(2);//2s
                    this.globaState.GetPressZero(result);//零点值
                    if (this.globaState.IsTimeReached)
                    {
                        this.serialPort_Dan.Write("#:1");
                        this.isMotorZero = false;
                        this.globaState.TwoState.EnumTwo = EnumOfTwoStep.检测预载荷是否为10主机发送指令加载电机停止;
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnStartHeat.Enabled = false;
                    });

                    //this.lblZeroGl.Text = this.globaState.PressZero.ToString();
                    break;
                case EnumOfTwoStep.检测预载荷是否为10主机发送指令加载电机停止:
                    point.Y = result - this.globaState.PressZero;

                    if (point.Y > Setting.PreloadedForce - 0.5)
                    {
                        this.serialPort_Dan.Write("#:0");
                        this.globaState.TwoState.EnumTwo = EnumOfTwoStep.检测载荷是否为9到11并调节预载荷;
                    }
                    break;

                case EnumOfTwoStep.检测载荷是否为9到11并调节预载荷:
                    point.Y = result - this.globaState.PressZero;
                    if (point.Y > Setting.PreloadedForce + 0.5)
                    {
                        this.serialPort_Dan.Write("#90");
                    }
                    else if (point.Y < Setting.PreloadedForce - 0.5)
                    {
                        this.serialPort_Dan.Write("#91");
                    }
                    else
                    {
                        this.globaState.TwoState.TimeCount(1.5f);
                        this.globaState.TwoState.GetPressZero(result);//零点值
                        if (this.globaState.TwoState.IsTimeReached)
                        {
                            this.globaState.TwoState.EnumTwo = EnumOfTwoStep.加热炉下降;
                        }
                        this.Invoke((EventHandler)delegate
                        {
                            this.lblPrePress.Text = point.Y.ToString();
                        });

                        //this.lblpzero.Text = this.globaState.TwoState.PressZero.ToString();
                    }

                    break;

                case EnumOfTwoStep.加热炉下降:
                    this.serialPort_Dan.Write("#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString());
                    this.isFurnaceZero = false;
                    this.globaState.TwoState.EnumTwo = EnumOfTwoStep.NONE;
                    break;
                case EnumOfTwoStep.采集数据并绘制膨胀力和时间曲线及膨胀力是否突变:
                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (result - this.globaState.TwoState.PressZero);
                    Setting.Points.Add(point);
                    this.globaState.TwoState.GetMaxPress(point);//最大膨胀力和时间
                    this.globaState.TwoState.CheckPressSudChange(point.Y);//检查膨胀力是否突变
                    if (this.globaState.TwoState.IsPressSudChange)
                    {
                        this.globaState.TwoState.EnumTwo = EnumOfTwoStep.结束测试;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.txtPengZhangPower.Text = BytesOperator.GetThreeValue(this.globaState.TwoState.MaxPress);
                        this.txtPengZhangTime.Text = BytesOperator.GetThreeValue(this.globaState.TwoState.MaxPress_Time);
                        this.chartPengZhang.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到膨胀力--时间曲线中去
                    });

                    break;
                case EnumOfTwoStep.结束测试:
                    this.serialPort_Dan.Write("#4");
                    this.globaState.IsEndtest = true;
                    this.globaState.IsTimeReached = false;
                    this.globaState.TwoState.InitState();
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnStartHeat.Enabled = true;
                        this.btnSave.Enabled = true;

                        this.adviceLB.Text = "测试结束，请保存数据";
                    });
                    break;
                default:
                    break;
            }
        }

        // 测试高温抗压强度
        private void FirstStep(PointF point,float result)
        {
            switch (this.globaState.OneState.EnumFirst)
            {
                case EnumOfFirstStep.加热炉按行程下降:

                    this.serialPort_Dan.Write("#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + (Setting.FurnaceFallingDistance - 160).ToString());//加热炉按行程下降
                    this.isFurnaceZero = false;
                    this.globaState.OneState.EnumFirst = EnumOfFirstStep.NONE;
                    break;
                case EnumOfFirstStep.取预置零点值:
                    this.globaState.OneState.CommandCount(1);
                    this.globaState.OneState.GetPressureZero(result);//零点值

                    if (this.globaState.OneState.IsCommandReached)
                    {
                        this.serialPort_Dan.Write("#:1");
                        this.globaState.OneState.EnumFirst = EnumOfFirstStep.电机上升;
                    }
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblTimeCount.Text = this.globaState.OneState.TimeCount(Setting.SoakingTime).ToString();//保温时间和延时
                    });
                    break;
                case EnumOfFirstStep.电机上升:

                    if (Math.Abs(this.globaState.OneState.PressureZero - result) >= 5)
                    {
                        this.serialPort_Dan.Write("#:0");
                        this.globaState.OneState.PressureZeroClear();
                        this.globaState.OneState.EnumFirst = EnumOfFirstStep.电机下降;
                        this.globaState.OneState.IsCommandReached = false;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.lblTimeCount.Text = this.globaState.OneState.TimeCount(Setting.SoakingTime).ToString();//保温时间和延时
                    });
                    break;
                case EnumOfFirstStep.电机下降:
                    this.serialPort_Dan.Write("#93");
                    this.globaState.OneState.EnumFirst = EnumOfFirstStep.抗压试样保温时间计时并更新零点值;
                    break;
                case EnumOfFirstStep.抗压试样保温时间计时并更新零点值:
                    if (this.globaState.OneState.countTime < 10)
                    {
                        this.globaState.OneState.CommandCount(1);
                        this.globaState.OneState.GetPressureZero(result);//零点值
                    }
                    
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblTimeCount.Text = this.globaState.OneState.TimeCount(Setting.SoakingTime).ToString();//保温时间和延时
                    });

                    if (this.globaState.OneState.IsTimeReached)
                    {
                        this.serialPort_Dan.Write("#:1");
                        this.isMotorZero = false;
                        this.startTime = DateTime.Now;
                        this.globaState.OneState.EnumFirst = EnumOfFirstStep.采集数据压力是否突变;
                        this.globaState.OneState.IsTimeReached = false;
                    }

                    break;
                case EnumOfFirstStep.采集数据压力是否突变:

                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = Convert.ToSingle((result - this.globaState.OneState.PressureZero) / (Setting.GetArea() * 0.001));
                    Setting.Points.Add(point);

                    this.globaState.OneState.GetMaxPressure(point);//得到最大压力值和对应的时间
                    this.globaState.OneState.CheckPressureSubChange(point.Y);//检查抗压强强值是否过大

                    if (this.globaState.OneState.IspressureSudChange)//抗压强度值是否突变
                    {
                        this.serialPort_Dan.Write("#:0");
                        this.globaState.OneState.EnumFirst = EnumOfFirstStep.测试结束;
                    }

                    this.Invoke((EventHandler)delegate
                    {
                        this.lblPressure.Text = BytesOperator.GetOneVaule((this.globaState.OneState.MaxPressure)).ToString();
                        this.lblPressureTime.Text = BytesOperator.GetOneVaule(this.globaState.OneState.MaxPreesureTime).ToString();

                        this.chartPressure.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到压强--时间曲线中去
                    });
                    break;
                case EnumOfFirstStep.测试结束:
                    this.serialPort_Dan.Write("#4");
                    this.globaState.IsEndtest = true;

                    this.globaState.OneState.InitState();
                    this.Invoke((EventHandler)delegate
                    {
                        this.btnStartHeat.Enabled = true;
                        this.btnSave.Enabled = true;

                        this.adviceLB.Text = "测试结束，请保存数据";
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

            this.serialPort_Wen = new SerialPort();
            this.serialPort_Wen.PortName = string.Format("COM{0}", portID);//串口端口
            this.serialPort_Wen.BaudRate = 19200;//波特率
            this.serialPort_Wen.Parity = (Parity)Enum.Parse(typeof(Parity), (string)"None");//奇偶校验
            this.serialPort_Wen.DataBits = 8;//数据位
            this.serialPort_Wen.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
            this.serialPort_Wen.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
            this.serialPort_Wen.ReadTimeout = 2000; ;//超时读取异常

            //串口数据接收事件
            this.serialPort_Wen.DataReceived += new SerialDataReceivedEventHandler(
                serialPort_Wen_DataReceived);
            //串口出错事件处理
            this.serialPort_Wen.ErrorReceived += new SerialErrorReceivedEventHandler(
                serialPort_Wen_ErrorReceived);
        }

        private void serialPort_Wen_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("温控仪串口出错，错误类型：" + e.EventType.ToString(), "错误信息",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void serialPort_Wen_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(40);
            
            byte[] buffer = new byte[10];//从机返回十个字节
            try
            {
                if (!this.serialPort_Wen.IsOpen) return;
                this.serialPort_Wen.Read(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("温控仪读取错误：{0}",ex.Message));
                return;
            }
            byte[] temBuffer = new byte[2];
            //低字节在前，高字节在后
            temBuffer[0] = buffer[1];
            temBuffer[1] = buffer[0];

            float temperature = 0.1f * NumberSystem.BinaryToDecimal_Complement(temBuffer, 16);

            if (temperature < 10)
            {

                return;
            }

            //this.temState.ChecktemperatureSubChange(temperature);
            //if (this.temState.IstemperatureSudChange)
            //{

            //    this.temState.IstemperatureSudChange = false;

            //}
            if (this.isTemReached)
            {
                //温度达到是绿色
                this.nowTemLbl.ForeColor = Color.Green;
                //this.btnStart.Enabled = false;

                if (!this.temState.IsCountSend)
                {
                    this.serialPort_Dan.Write("#6");
                    this.temState.IsCountSend = true;

                }

                this.isTemReached = this.temState.CountTime(temperature, Setting.SettingTemperature);

            }
            else
            {    //温度没达到是红色
                if (!isTemReached)
                {
                    this.nowTemLbl.ForeColor = Color.Red;

                }
                this.isTemReached = this.temState.CountTime(temperature, Setting.SettingTemperature);

            }

            this.Invoke((EventHandler)delegate
            {
                this.nowTemLbl.Text = temperature.ToString();

            });
        }
        #endregion

        #region 位移传感器
        private void InitSerialPort_Wei(int portID)
        {

            this.serialPort_Wei = new SerialPort();
            this.serialPort_Wei.PortName = string.Format("COM{0}", portID);//串口端口
            this.serialPort_Wei.BaudRate = 9600;//波特率
            this.serialPort_Wei.Parity = Parity.None;//奇偶校验
            this.serialPort_Wei.DataBits = 8;//数据位
            this.serialPort_Wei.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
            this.serialPort_Wei.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
            this.serialPort_Wei.ReadTimeout = 2000; ;//超时读取异常
            Thread threadWei = new Thread(new ThreadStart(delegate ()
              {
                //串口数据接收事件
                this.serialPort_Wei.DataReceived += new SerialDataReceivedEventHandler(
                    serialPort_Wei_DataReceived);
                //串口出错事件处理
                this.serialPort_Wei.ErrorReceived += new SerialErrorReceivedEventHandler(
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
                if (!this.serialPort_Wei.IsOpen) return;
                this.serialPort_Wei.Read(buffer, 0, buffer.Length);
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
            this.FourStepD(point);

        }

        private void FourStepD(PointF point)
        {
            switch (this.globaState.FourState.EnumFour)
            {
                case EnumOfFourStep.NONE:
                    break;
                case EnumOfFourStep.取预置零点值:
                    this.globaState.FourState.TimeCount(1);
                    this.globaState.FourState.GetDisplacementZero(displacementResult);//零点值
                    if (this.globaState.FourState.IsTimeReached)
                    {
                        this.serialPort_Dan.Write("#:1");
                        this.globaState.FourState.EnumFour = EnumOfFourStep.电机上升;
                    }
                    break;
                case EnumOfFourStep.电机上升:

                    if (Math.Abs(this.globaState.FourState.DisplacementZero - displacementResult) >= 2)
                    {
                        this.serialPort_Dan.Write("#:0");
                        this.globaState.FourState.DisplacementZeroClear();
                        this.globaState.FourState.EnumFour = EnumOfFourStep.电机下降;
                        this.globaState.FourState.IsTimeReached = false;
                    }
                    break;
                case EnumOfFourStep.电机下降:
                    this.serialPort_Dan.Write("#92");
                    this.globaState.FourState.EnumFour = EnumOfFourStep.加热炉下降;
                    break;
                case EnumOfFourStep.加热炉下降:
                    this.globaState.FourState.Sleep();
                    if (this.globaState.FourState.IsOK)
                    {
                        this.serialPort_Dan.Write("#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString());
                        this.isFurnaceZero = false;

                        this.globaState.FourState.EnumFour = EnumOfFourStep.等待加热炉下降;
                    }
                    break;
                case EnumOfFourStep.等待加热炉下降:
                    this.globaState.FourState.Wait();
                    if (this.globaState.FourState.IsWaitOver)
                    {
                        this.globaState.FourState.EnumFour = EnumOfFourStep.更新零点值;
                    }
                    break;
                case EnumOfFourStep.更新零点值:
                    this.globaState.FourState.TimeCount(2);
                    this.globaState.FourState.GetDisplacementZero(displacementResult);//零点值
                    if (this.globaState.FourState.IsTimeReached)
                    {
                        this.startTime = DateTime.Now;
                        this.globaState.FourState.EnumFour = EnumOfFourStep.采集位移数据并绘制曲线以及判断是否突变;
                        this.globaState.FourState.IsTimeReached = false;
                    }
                    break;
                case EnumOfFourStep.采集位移数据并绘制曲线以及判断是否突变:
                    point.X = (float)(DateTime.Now - this.startTime).TotalSeconds;
                    point.Y = (float)Math.Round((displacementResult - this.globaState.FourState.DisplacementZero) / Setting.SpecimenHeight * 100, 2);
                    Setting.Points.Add(point);

                    this.globaState.FourState.GetMaxDisplacement(point);//得到最大压力值和对应的时间
                    this.globaState.FourState.CheckDisplacementSubChange(point);
                    this.Invoke((EventHandler)delegate
                    {
                        this.lblMaxExpansionRate.Text = this.globaState.FourState.MaxDisplacement.ToString();
                        this.lblMaxExpansionRateTime.Text = BytesOperator.GetOneVaule(this.globaState.FourState.MaxDisplacementTime).ToString();

                        this.chartExpansionRate.Series[0].Points.AddXY(point.X, point.Y);//讲点添加到膨胀率--时间曲线中去
                    });
                    if (this.globaState.FourState.IsDisplacementSudChange)//抗压强度值是否突变
                    {
                        this.serialPort_Dan.Write("#4");
                        this.globaState.FourState.EnumFour = EnumOfFourStep.测试结束;
                    }
                    break;
                case EnumOfFourStep.测试结束:

                    this.globaState.IsEndtest = true;
                    this.globaState.FourState.IsTimeReached = false;
                    this.globaState.FourState.InitState();
                    this.Invoke((EventHandler)delegate
                    {
                        //this.tmDisplacement.Stop();
                        //this.tmDisplacement.Enabled = false;
                        isStartDisplacement = false;
                        this.btnStartHeat.Enabled = true;
                        this.btnSave.Enabled = true;
                        this.adviceLB.Text = "测试结束，请保存数据";
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

        private void setParaBtn_Click(object sender, EventArgs e)
        {
            FormSpecimenSetting frmSpecimenSetting = new FormSpecimenSetting();
            DialogResult dialog = frmSpecimenSetting.ShowDialog();
            if (dialog == DialogResult.Cancel) return;

            this.txbNum.Text = Setting.SpecimenNum;
            this.txbMat.Text = Setting.SpecimenName;
            this.txtSpecimenDiameter.Text = Setting.SpecimenDiameter.ToString();
            this.txtSpecimenHeight.Text = Setting.SpecimenHeight.ToString();
            this.txbPersonName.Text = Setting.ExperimentPerson;
            this.txbFrom.Text = Setting.ExperimentUnit;
            this.txbReNum.Text = Setting.RepeatTimes.ToString();
            this.btnStart.Enabled = true;
            if (isReset)
            {
                this.serialPort_Dan.Write("#4");
                isReset = false;
            }
        }

        private void setDeviceBtn_Click(object sender, EventArgs e)
        {
            FormParameterSetting formParameterSetting = new FormParameterSetting();
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
            sendData[0] = 0x23;
            sendData[1] = 0x3C;
            byte[] numData = new byte[2];
            switch (Setting.TestType)
            {
                case 0:
                case 1:
                case 2:
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_Dan.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                case 3:
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.DisplacementMotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_Dan.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                default:
                    break;
            }
        }

        private void tmCheckTem_Tick(object sender, EventArgs e)
        {
            if (this.serialPort_Wen == null) return;
            if (!this.serialPort_Wen.IsOpen) return;
            try
            {
                this.serialPort_Wen.Write(this.checkTemBuffer, 0, this.checkTemBuffer.Length);
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log(string.Format("温控仪写入故障：{0}", ex.Message));
                return;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //if (!isFurnaceZero || !isMotorZero) 
            //{
            //    this.adviceLB.Text = "加热炉或者电机不在零点，请复位";
            //    return;
            //}
            this.serialPort_Dan.Write("#3");
            this.StartTest();
            //this.serialPort_Dan.Write("#:0");
            //System.Threading.Thread.Sleep(10);
            //this.serialPort_Dan.Write("#>" + (Setting.FurnaceLiftingSpeed / 10 - 1).ToString() + ((Setting.FurnaceFallingDistance - 150) / 2).ToString());
        }

        private void StartTest()
        {
            this.btnStartHeat.Enabled = false;
            this.adviceLB.Text = "";
            this.setDeviceBtn.Enabled = false;
            this.setParaBtn.Enabled = false;
            this.isReachedGo = true;

            switch (this.selectTabControl.SelectedIndex)
            {
                case 0:
                    this.globaState.GoToFirstStep();
                    this.globaState.OneState.EnumFirst = EnumOfFirstStep.加热炉按行程下降;
                    break;
                case 1:
                    this.globaState.GoToSecondStep();

                    this.globaState.TwoState.EnumTwo = EnumOfTwoStep.开始调试并发送指令;
                    break;
                case 2:
                    this.globaState.GoToThreeStep();
                    this.globaState.ThreeState.EnumThree = EnumOfThreeStep.开始测试并发送指令;
                    break;
                case 3:
                    this.globaState.GoToFourStep();

                    break;
                default:
                    break;
            }
            Setting.InitResult(this.selectTabControl.SelectedIndex);
            this.btnStart.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.Restoration();
        }

        /// <summary>
        /// 复位
        /// </summary>
        private void Restoration()
        {
            isStartDisplacement = false;
            this.serialPort_Dan.Write("#4");
            //this.isReset = true;
            this.globaState.IsEndtest = true;
            this.isReachedGo = false;
            switch (Setting.TestType)
            {
                case 0:
                    this.globaState.OneState.InitState();
                    break;
                case 1:
                    this.globaState.TwoState.InitState();
                    break;
                case 2:
                    this.globaState.ThreeState.InitState();
                    break;
                case 3:
                    this.globaState.FourState.InitState();
                    break;
                default:
                    break;
            }

            this.lblPressure.Text = "0";
            this.lblPressureTime.Text = "0";
            this.txtPengZhangPower.Text = "0";
            this.txtPengZhangTime.Text = "0";

            this.txtBalancePressureTime.Text = "0";
            //this.lblPrePress.Text = "";
            //this.lblPrePressure.Text = "";
            this.lblMaxExpansionRateTime.Text = "0";
            this.lblMaxExpansionRate.Text = "0";

            Setting.Points.Clear();
            this.chartPressure.Series[0].Points.Clear();
            this.chartPengZhang.Series[0].Points.Clear();
            this.chartBalancePress.Series[0].Points.Clear();
            this.chartExpansionRate.Series[0].Points.Clear();

            this.lblTimeCount.Text = Setting.SoakingTime.ToString();
            //this.lblPressZero.Text = "0";
            this.lblPrePress.Text = Setting.PreloadedForce.ToString();
            this.lblPrePressure.Text = Setting.PreloadedPressure.ToString();

            this.setDeviceBtn.Enabled = true;
            this.setParaBtn.Enabled = true;
            this.btnStart.Enabled = true;
            this.btnStartHeat.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.globaState.IsEndtest)
            {
                MessageBox.Show("正在进行测试!");
                return;
            }

        InitialSave:

            SaveFileDialog saveDlg = new SaveFileDialog();
            //this.testSample.TstTime = System.DateTime.Now.ToString("f");

            saveDlg.FileName = Setting.TstTime + "." + Setting.SpecimenNum + "." + Setting.GetTypeStr() + "-" + Setting.RepeatTimes;

            saveDlg.Filter = "文本格式|*.txt";
            string path = string.Empty;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                path = saveDlg.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Info实验类型:" + Setting.GetTypeStr());
                sw.WriteLine("Info实验人员:" + Setting.ExperimentPerson);
                sw.WriteLine("Info实验时间:" + Setting.TstTime);
                sw.WriteLine("Info试样编号:" + Setting.SpecimenNum);
                sw.WriteLine("Info试样名称:" + Setting.SpecimenName);

                sw.WriteLine("Info试样尺寸:" + this.txtSpecimenHeight.Text);
                sw.WriteLine("Info设置温度:" + Setting.SettingTemperature + "℃");
                sw.WriteLine("Info来样单位:" + Setting.ExperimentUnit);
                sw.WriteLine("Info重复次数:" + Setting.RepeatTimes);

                switch (Setting.TestType)
                {
                    case 0:
                        sw.WriteLine("Info抗压强度:" + this.lblPressure.Text + "KPa");
                        sw.WriteLine("Info对应时间:" + this.lblPressureTime.Text + "S");
                        sw.WriteLine("Info保温时间:" + Setting.SoakingTime.ToString() + "S");
                        break;
                    case 1:
                        sw.WriteLine("Info膨胀力值:" + this.txtPengZhangPower.Text + "N");
                        sw.WriteLine("Info对应时间:" + this.txtPengZhangTime.Text + "S");
                        sw.WriteLine("Info预载荷值:" + Setting.PreloadedForce.ToString() + "N");
                        break;
                    case 2:
                        sw.WriteLine("Info对应时间:" + this.txtBalancePressureTime.Text + "S");
                        sw.WriteLine("Info设定强度:" + Setting.PreloadedPressure.ToString() + "MPa");
                        break;
                    case 3:
                        sw.WriteLine("Info最大膨胀率:" + this.lblMaxExpansionRate.Text + "%");
                        sw.WriteLine("Info最大膨胀率时间:" + this.lblMaxExpansionRateTime.Text + "S");
                        break;
                }



                foreach (PointF point in Setting.Points)
                {
                    sw.WriteLine("X" + point.X.ToString() + "," + "Y" + point.Y.ToString());
                }

                sw.Close();
                this.setDeviceBtn.Enabled = true;
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
                this.txbReNum.Text = Setting.RepeatTimes.ToString();

            }
            else
            {
                DialogResult result = MessageBox.Show("确定不需要数据吗？？？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    this.setDeviceBtn.Enabled = true;
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
            this.setDeviceBtn.Enabled = true;
            this.setParaBtn.Enabled = true;
            this.btnStart.Enabled = true;
            this.btnStartHeat.Enabled = true;
            this.btnSave.Enabled = false;
            this.isReachedGo = false;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            FormOpenFile frmOpenFile = new FormOpenFile();

            DialogResult dlgResult = frmOpenFile.ShowDialog();
            
        }

        private void btnJiao_Click(object sender, EventArgs e)
        {
            ////串口数据接收事件
            //this.serialPort_Dan.DataReceived += new SerialDataReceivedEventHandler(serialPort_Dan_DataReceived);
            byte[] sendData = new byte[4];
            sendData[0] = 0x23;
            sendData[1] = 0x3C;
            byte[] numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
            sendData[2] = numData[1];
            sendData[3] = numData[0];
            this.serialPort_Dan.Write(sendData, 0, 4);
            Thread.Sleep(50);
            this.serialPort_Dan.Write("#3");
            this.isReachedGo = true;

            FormCheckout fjy = new FormCheckout(this);
            //FormJiaoyan fjy = new FormJiaoyan(ReceivedInfoLB);
            fjy.TxtJiaoZheng.Text = Setting.TxtRevise.ToString();
            DialogResult dlgResult = fjy.ShowDialog();

            //this.globaState.TimeCount(2);
            //FormJiaoyan fjy = new FormJiaoyan(this);
            ////FormJiaoyan fjy = new FormJiaoyan(ReceivedInfoLB);
            //fjy.TxtJiaoZheng.Text = cf.ReadConfig("TxtJiaoZheng");
            //DialogResult dlgResult = fjy.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                Setting.Save("TxtRevise", fjy.TxtJiaoZheng.Text);
                this.isReachedGo = false;

            }
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            //this.serialPort_Dan.Write("#91");
            this.serialPort_Dan.Write("#4");
        }

        private void selectTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte[] sendData = new byte[4];
            sendData[0] = 0x23;
            sendData[1] = 0x3C;
            byte[] numData = new byte[2];
            //四选一(抗压强度，热稳定性，膨胀力，高温急热膨胀率)
            switch (this.selectTabControl.SelectedIndex)
            {
                case 0:
                    this.selectIndex = StepEnum.FirstStep;
                    this.isReachedGo = false;
                    Setting.RepeatTimes = 1;
                    this.txbReNum.Text = "1";
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_Dan.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                case 1:
                    this.selectIndex = StepEnum.TwoStep;
                    this.isReachedGo = false;
                    Setting.RepeatTimes = 1;
                    this.txbReNum.Text = "1";
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_Dan.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                case 2:
                    this.selectIndex = StepEnum.ThreeStep;
                    this.isReachedGo = false;
                    Setting.RepeatTimes = 1;
                    this.txbReNum.Text = "1";
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.MotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_Dan.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                case 3:
                    this.selectIndex = StepEnum.FourStep;
                    this.isReachedGo = false;
                    Setting.RepeatTimes = 1;
                    this.txbReNum.Text = "1";
                    numData = BitConverter.GetBytes(Convert.ToInt32(Setting.DisplacementMotorIdlePath * 2500));
                    sendData[2] = numData[1];
                    sendData[3] = numData[0];
                    this.serialPort_Dan.Write(sendData, 0, 4);
                    Thread.Sleep(300);
                    break;
                default:
                    break;
            }
        }

        int num;//定义温度设置按钮点击次数
        private void btnStartHeat_Click(object sender, EventArgs e)
        {
            if (num % 2 == 0)
            {
                if (this.temState.IsTemReset)
                {
                    return;
                }
                //手动控制指令
                byte[] handControllBuffer = new byte[]{
                0x81,0x81,0x43,0x18,0x00,0x00,0x44,0x18
                };

                //手动控制输出值设置指令
                byte[] handSetOutBufer = new byte[]{
                0x81,0x81,0x43,0x1A,0x00,0x00,0x44,0x1A
                };

                if (!this.serialPort_Wen.IsOpen)
                {
                    this.serialPort_Wen.Open();
                }

                this.serialPort_Wen.Write(handControllBuffer, 0, handControllBuffer.Length);
                Thread.Sleep(500);

                this.temState.IsCanStartHeat = true;
                this.temState.IsTemReset = true;
                this.setTemTxt.ReadOnly = false;
                this.btnStartHeat.Text = "确定";
                this.setTemTxt.ForeColor = Color.White;
                this.serialPort_Wen.Write(handSetOutBufer, 0, handSetOutBufer.Length);
                Setting.Save("SettingTemperature", this.setTemTxt.Text);
            }
            else
            {
                //自动控制指令
                byte[] autorunBuffer = new byte[]{
            0x81,0x81,0x43,0x18,0x01,0x00,0x45,0x18 };

                if (!this.serialPort_Wen.IsOpen)
                {
                    this.serialPort_Wen.Open();
                }


                if (this.setTemTxt.Text != null && Setting.SettingTemperature > 0 && Setting.SettingTemperature <= 1200)
                {
                    this.serialPort_Wen.Write(NumberSystem.TemperatureToByte8(int.Parse(this.setTemTxt.Text)), 0, 8);
                    Thread.Sleep(500);

                    this.temState = new TemperatureState();
                    this.isTemReached = false;
                    this.setTemTxt.ReadOnly = true;//设定温度为只读
                    this.btnStartHeat.Text = "设置温度";

                    this.serialPort_Wen.Write(autorunBuffer, 0, autorunBuffer.Length);
                    //cf.ReadConfig("SetTemperature");
                    this.setTemTxt.ForeColor = Color.Yellow;
                    Setting.Save("SettingTemperature", this.setTemTxt.Text);
                    //this.btnStartHeat.Enabled = false ;


                    //cf.SaveConfig("SetTemperature", this.testSample.SetTemperature.ToString());
                }

                else
                {
                    MessageBox.Show("请输入正确的数字");
                    this.setTemTxt.Text = "1000";
                }
                this.btnStartHeat.Enabled = false;
            }
            num++;

        }

        private void DisplacementWrite(object state)
        {
            if (this.serialPort_Wei == null) return;
            if (this.serialPort_Wei.IsOpen && isStartDisplacement)
            {
                try
                {
                    this.serialPort_Wei.Write(this.checkDisplacementBuffer, 0, this.checkDisplacementBuffer.Length);
                }
                catch (Exception ex)
                {
                    SampleLoggerOnTextFile.Log(string.Format("位移传感器写入故障：{0}", ex.Message));
                    return;
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (MessageBox.Show("确定要退出程序吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No);
        }
    }
}
