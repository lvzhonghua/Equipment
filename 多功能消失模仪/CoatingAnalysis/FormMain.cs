using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoatingAnalysis
{
    public partial class FormMain : Form
    {
        ConfigFile cf = new ConfigFile("HDoit.Instrument.CoatingAnalysis");
        private bool isTempReached = false;//判断温度是否达到
        private bool isTempReached1 = false;//判断温度是否低于极限值
        private DateTime startTime;//开始的时间
        private int setTemOfTest = 900;//开始设置的温度
        private bool sleepTime_Two = true;//true还未休息10s,false表示已经休息完10s
        private bool isFirstTestStart = true;//是否第一次开始实验
        private byte[] checkTemBuffer = new byte[]{
            0x81,0x81,0x52,0x00,0x00,0x00,0x53,0x00};//温控仪查询温度的说明
        private TestSample testSample = new TestSample(900);//声明一个测试样本
        private GlobaState globalState = new GlobaState();//声明一个全局状态
        private TemperatureState temState = new TemperatureState();
        private SerialPort serialPort_Dan = null;//声明单片机的串口
        private AppSettingsReader settingsReader = new AppSettingsReader();
        private EnumOfCmd enumOfCmd = EnumOfCmd.NONE;

        private String errorInfo1 = "";//端口1错误类型
        private String errorInfo2 = "";//端口1错误类型

        private bool singleStep = false;//是否激活单步操作
        /*压力传感器回归方程参数*/
        private float a1;
        private float b1;
        private float a2;
        private float b2;

        private bool beVented = false;


        private Queue<String> cmdQueue = new Queue<string>();

        private float dData = 0.5f;
        private float hData = 0.5f;

        //高温通气性测试的停止10s
        private void SleepTenSeconds_Two()
        {
            Thread.Sleep(10000);
            if (this.sleepTime_Two)
            {
                this.sleepTime_Two = false;
                this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.延时2s并记录Pb;
            }
        }

        private void CloseVavle()
        {
            Thread.Sleep(5000);
            if (this.serialPort_Dan.IsOpen)
            {
                byte[] buffer = new byte[3];
                BytesOperator.AppendBuffer(buffer, Encoding.Default.GetBytes("#3"), 0);
                buffer[2] = 0x0F;
                this.cmdQueue.Enqueue("关闭所有阀");
                this.serialPort_Dan.Write(buffer, 0, buffer.Length);
            }
        }


        //初始化单片机的端口
        private void InitSerialPort_Dan(int portID)
        {
            //this.serialPort_Dan = new SerialPort();
            //this.serialPort_Dan.PortName = string.Format("COM{0}", portID);
            this.serialPort_Dan.BaudRate = (int)settingsReader.GetValue("BaudRate_Dan", typeof(int));//波特率 位/秒
            this.serialPort_Dan.Parity = (Parity)Enum.Parse(typeof(Parity), (string)settingsReader.GetValue("Parity_Dan", typeof(string)));//奇偶校验
            this.serialPort_Dan.DataBits = (int)settingsReader.GetValue("DataBits_Dan", typeof(int)); //数据位
            this.serialPort_Dan.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)settingsReader.GetValue("StopBits_Dan", typeof(string)));//停止位
            this.serialPort_Dan.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)settingsReader.GetValue("Handshake_Dan", typeof(string)));//握手协议即流控制方式
            this.serialPort_Dan.ReadTimeout = (int)settingsReader.GetValue("ReadTimeout_Dan", typeof(int));//超时读取异常

            //串口数据接收事件
            this.serialPort_Dan.DataReceived += new 
                SerialDataReceivedEventHandler(serialPort_Dan_DataReceived);
            //串口出错事件处理
            this.serialPort_Dan.ErrorReceived += new 
                SerialErrorReceivedEventHandler(serialPort_Dan_ErrorReceived);
        }

        //单片机出错信息显示
        private void serialPort_Dan_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            String lastError = e.EventType.ToString();
            if (this.errorInfo1 == lastError)
            {
                this.receivedInfoLB.Items.Add(this.errorInfo1);
            }else
            {
                this.errorInfo1 = lastError;
                MessageBox.Show("串口出错，错误类型：" + e.EventType.ToString(), "错误信息",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        //单片机的接收数据事件
        private void serialPort_Dan_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //serialPort对象的第一个参数是它本身的引用，第二个参数则是SerialDataReceivedEventArgs对象的一个实例,它是由事件传送的参数 
            System.Threading.Thread.Sleep(25);	//延时，以防止单片机传输过来的数据不完整
            if (!this.serialPort_Dan.IsOpen) return;
            PointF point = new PointF(0, 0);
            byte[] buffer = new byte[5];

            this.serialPort_Dan.Read(buffer, 0, buffer.Length);

            CommandInterpreter command = new CommandInterpreter(buffer);
            #region 收到的指令
            switch (command.GetFirstCode())
            {
                #region 按键指令R
                case CommandCode.R:
                    this.globalState.GotoFirstStep();
                    //this.serialPort_Dan.Write("#21");
                    //this.cmdQueue.Enqueue("发送#21跳转至测试指令");

                    this.Invoke((EventHandler)delegate
                    {
                        this.selectTabControl.SelectedIndex = 0;
                        this.btnCancelTest.Enabled = true;
                        this.startBtn.Enabled = false;
                        this.setMatBtn.Enabled = false;
                        this.adviceInfoLbl.Text = "正在测试中！！";
                        this.receivedInfoLB.Items.Add("R----------");
                    });
                    break;
                #endregion

                #region 第一个传感器数据D
                case CommandCode.D:
                    float resultD = (float)(command.GetData() *this.b1* 4.65661f * Math.Pow(10,-6) + a1);//1.168289*Math.Pow(10,-6)

                    if (!this.globalState.IsStartTest)//开始前的零点值
                    {
                        this.dData = resultD;
                        this.globalState.GetFirstZeroPress(resultD);
	                }
                    else if (this.globalState.IsStartTest && !this.globalState.IsEndTest&&this.globalState.StepNum == StepEnum.FirstStep)
                    {
                        switch (this.globalState.OneState.EnumOfFS)
                        {
                            case EnumOfFirstStep.开始测试并打开阀1:
                                this.serialPort_Dan.Write(this.globalState.OneState.OpenBuffer, 0
                                    , this.globalState.OneState.OpenBuffer.Length);
                                this.cmdQueue.Enqueue("打开阀一");
                                this.globalState.OneState.EnumOfFS = EnumOfFirstStep.绘制压力时间曲线;
                                this.startTime = DateTime.Now;
                                break;
                            case EnumOfFirstStep.绘制压力时间曲线:
                                point.X = (float)((DateTime.Now - this.startTime).TotalSeconds);
                                point.Y = resultD - this.globalState.FirstPressZero;

                                //将数据点添加至传输特性图中
                                this.testSample.TransPoints.Add(point);

                                
                                this.testSample.GetTranMaxPressure(point);
                                
                                this.globalState.OneState.CheckPressIsZero(point.Y,this.globalState.FirstPressZero);
                                //判断是否达到终点
                                if (this.globalState.OneState.IsPressZero)
                                {
                                    this.globalState.OneState.EnumOfFS = EnumOfFirstStep.关闭所有阀;
                                }

                                //显示最大压力、最大压力时间、排出时间、画对应的压力-时间曲线
                                this.Invoke((EventHandler)delegate
                                {
                                    this.transPressLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.TransMaxPress);
                                    this.transPressTimeLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.TranMaxPrees_Time);
                                    if (this.testSample.TestResult.TranEndTime>=100)
                                        this.transEndTimeLbl.Text = this.testSample.TestResult.TranEndTime.ToString().Substring(0,3);
                                    else
                                        this.transEndTimeLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.TranEndTime);
                                    this.transChart.Series[0].Points.AddXY(point.X, point.Y);
                                });
                                break;
                            case EnumOfFirstStep.关闭所有阀:
                                this.serialPort_Dan.Write(this.globalState.OneState.CloseBuffer, 0
                                    , this.globalState.OneState.CloseBuffer.Length);

                                this.cmdQueue.Enqueue("关闭所有阀");
                                //根据是否激活单步操作判断
                                if (this.singleStep) {
                                    this.globalState.OneState.EnumOfFS = EnumOfFirstStep.单步结束测试;
                                }
                                else{
                                    this.globalState.OneState.EnumOfFS = EnumOfFirstStep.转换至高温透气性;
                                }
                                break;
                            case EnumOfFirstStep.转换至高温透气性:
                                this.serialPort_Dan.Write(this.globalState.OneState.ChangeTestBuffer, 0
                                    , this.globalState.OneState.ChangeTestBuffer.Length);
                                Thread.Sleep(100);
                                this.cmdQueue.Enqueue("转换至测试步骤2");
                                this.globalState.OneState.EnumOfFS = EnumOfFirstStep.NONE;
                                this.globalState.GotoSecondStep();
                                this.Invoke((EventHandler)delegate
                                {
                                    this.selectTabControl.SelectedIndex = 1;
                                });
                                break;
                            case EnumOfFirstStep.单步结束测试:
                                this.globalState.GotoEndTest();
                                this.Invoke((EventHandler)delegate
                                {
                                    this.ventBtn.Enabled = false;
                                    this.setMatBtn.Enabled = false;
                                    this.btnCancelTest.Enabled = false;
                                    this.adviceInfoLbl.Text = "测试结束，请保存数据";
                                });
                                this.serialPort_Dan.Write("#5");
                                this.cmdQueue.Enqueue("停止测试");
                                break;
                            default:
                                break;
                        }
                    }
                    else { }

                    this.Invoke((EventHandler)delegate
                    {
                        this.receivedInfoLB.Items.Add("D" + " " + resultD.ToString());
                        if (this.receivedInfoLB.Items.Count != 0)
                            this.receivedInfoLB.SelectedIndex = this.receivedInfoLB.Items.Count - 1;
                    });
                    break;
                #endregion

                #region 第二个传感器数据H
                case CommandCode.H:
                    float resultH = (float)(command.GetData() *this.b2* 9.31323f * Math.Pow(10,-6)+ this.a2);//1.180661*Math.Pow(10,-5)

                    if (!this.globalState.IsStartTest)//未开始测试前
                    {
                        this.hData = resultH;
                        this.globalState.GetSecondZeroPress(resultH);
                    }
                    #region 第二步测试
                    else if (this.globalState.IsStartTest && this.globalState.StepNum == StepEnum.SecondStep)//测试过程中
                    {
                        point.Y = resultH - this.globalState.SecondPressZero;
                        switch (this.globalState.TwoState.EnumOfSS)
                        {
                            case EnumOfSecondStep.打开阀3及压缩机:
                                this.serialPort_Dan.Write(this.globalState.TwoState.OpenBuffer, 0,
                                    this.globalState.TwoState.OpenBuffer.Length);
                                this.cmdQueue.Enqueue("打开阀3及压缩机");
                                this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.第一次关闭所有阀;
                                break;
                            case EnumOfSecondStep.第一次关闭所有阀:
                                //检查压力是否大于10KPa
                                this.globalState.TwoState.CheckPressReach(point.Y);
                                //判断压力是否达到10KPa
                                if (this.globalState.TwoState.IsPressReached)
                                {
                                    this.serialPort_Dan.Write(this.globalState.TwoState.CloseBuffer, 0,
                                        this.globalState.TwoState.CloseBuffer.Length);
                                    this.cmdQueue.Enqueue("关闭所有阀");
                                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.延时2s并记录Pb;
                                }
                                break;
                            case EnumOfSecondStep.延时2s并记录Pb:
                                Thread t1 = new Thread(new ThreadStart(this.SleepTenSeconds_Two));
                                if (sleepTime_Two)
                                {
                                    t1.Start();
                                    t1.IsBackground = true;
                                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.NONE;
                                }
                                else
                                {
                                    t1.Abort();
                                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.打开阀2;
                                }
                                break;
                            case EnumOfSecondStep.打开阀2:
                                this.globalState.Pb = point.Y;
                                this.serialPort_Dan.Write(this.globalState.TwoState.OpenTwoBuffer, 0,
                                    this.globalState.TwoState.OpenTwoBuffer.Length);
                                this.cmdQueue.Enqueue("打开阀2");
                                this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.绘制压力时间曲线;
                                this.startTime = DateTime.Now;
                                break;
                            case EnumOfSecondStep.绘制压力时间曲线:
                                point.X = (float)((DateTime.Now - this.startTime).TotalSeconds);
                                //添加高温透气性压力时间点
                                this.testSample.VentiPoints.Add(point);
                                
                                this.testSample.GetTe(this.globalState.Pb, point);
                                //检查压力是否到达零点
                                this.globalState.TwoState.CheckPressIsZero(point.Y);
                                this.testSample.TestResult.TranEndTime = point.X;

                                if (this.globalState.TwoState.IsPressZero)
                                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.第二次关闭所有阀;
                                this.Invoke((EventHandler)delegate
                                {
                                    this.ventiNowPressLbl.Text = BytesOperator.GetFourValue(point.Y);
                                    this.ventiChart.Series[0].Points.AddXY(point.X, point.Y);
                                });
                                break;
                            case EnumOfSecondStep.第二次关闭所有阀:
                                this.serialPort_Dan.Write(this.globalState.TwoState.CloseBuffer, 0,
                                    this.globalState.TwoState.CloseBuffer.Length);
                                this.cmdQueue.Enqueue("第二次关闭所有阀");
                                //计算并得到得到透气率
                                this.testSample.GetVentina(this.globalState.Pb);
                                this.Invoke((EventHandler)delegate
                                {
                                    if (this.testSample.TestResult.Ventiratio.ToString().Length > 4)
                                        this.ventiLbl.Text = this.testSample.TestResult.Ventiratio.ToString().Substring(0, 4);
                                    this.label6.Visible = true;
                                    this.ventiLbl.Visible = true;
                                });

                                if (this.singleStep)
                                {
                                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.单步结束测试;
                                }
                                else {
                                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.高温强度测试;
                                }
                                
                                break;
                            case EnumOfSecondStep.高温强度测试:
                                this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.NONE;
                                this.globalState.GotoThridStep();
                                this.Invoke((EventHandler)delegate
                                {
                                    this.selectTabControl.SelectedIndex = 2;
                                });
                                break;
                            case EnumOfSecondStep.单步结束测试:
                                this.globalState.GotoEndTest();
                                this.Invoke((EventHandler)delegate
                                {
                                    this.ventBtn.Enabled = false;
                                    this.setMatBtn.Enabled = false;
                                    this.btnCancelTest.Enabled = false;
                                    this.adviceInfoLbl.Text = "测试结束，请保存数据";
                                });
                                this.serialPort_Dan.Write("#5");
                                this.cmdQueue.Enqueue("停止测试");
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                    #region 第三步测试
                    else if (this.globalState.IsStartTest && this.globalState.StepNum == StepEnum.ThirdStep)
                    {
                        switch (this.globalState.ThreeState.EnumOfTS)
                        {
                            case EnumOfThirdStep.打开阀2和3以及空压机:
                                this.serialPort_Dan.Write(this.globalState.ThreeState.OpenBuffer, 0,
                                    this.globalState.ThreeState.OpenBuffer.Length);
                                this.globalState.ThreeState.EnumOfTS = EnumOfThirdStep.绘制压力时间曲线;
                                this.cmdQueue.Enqueue("打开阀2和阀3及压缩机");
                                this.startTime = DateTime.Now;
                                break;
                            case EnumOfThirdStep.绘制压力时间曲线:
                                point.X = (float)((DateTime.Now - this.startTime).TotalSeconds);
                                point.Y = resultH - this.globalState.SecondPressZero;

                                this.globalState.ThreeState.GetMaxPress(point);
                                this.testSample.StrengthPoints.Add(point);

                                this.testSample.TestResult.StrengthMaxPress = this.globalState.ThreeState.MaxPressure;
                                this.testSample.TestResult.StrengthMaxPress_Time = this.globalState.ThreeState.MaxPressureTime;
                                
                                //this.globalState.ThreeState.CheckPressSubChange(point.Y);
                                this.globalState.ThreeState.CheckPressSubChange(point);
                                this.globalState.ThreeState.CheckLarge(point.Y);
                                this.globalState.ThreeState.CheckTestBroken(point.Y);

                                if (this.globalState.ThreeState.IsPressSudChange || this.globalState.ThreeState.IsPressureTooLarge || this.globalState.ThreeState.IsTestBroken)
                                {
                                    this.globalState.ThreeState.EnumOfTS = EnumOfThirdStep.关闭空压机;
                                    this.serialPort_Dan.Write(this.globalState.ThreeState.CloseKongBuffer, 0,
                                        this.globalState.ThreeState.CloseKongBuffer.Length);
                                    this.cmdQueue.Enqueue("关闭所有阀");
                                }
                                this.Invoke((EventHandler)delegate
                                {
                                    this.strengthPressLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.StrengthMaxPress);
                                    this.strengthPressTimeLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.StrengthMaxPress_Time);
                                    this.strengthChart.Series[0].Points.AddXY(point.X, point.Y);
                                });
                                break;
                            case EnumOfThirdStep.关闭空压机:
                                point.X = (float)((DateTime.Now - this.startTime).TotalSeconds);
                                point.Y = resultH - this.globalState.SecondPressZero;
                                this.testSample.StrengthPoints.Add(point);
                                this.globalState.ThreeState.CheckVentEnough();
                                if (this.globalState.ThreeState.IsVentEnough)
                                    this.globalState.ThreeState.EnumOfTS = EnumOfThirdStep.关闭所有阀;

                                this.Invoke((EventHandler)delegate
                                {
                                    this.strengthPressLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.StrengthMaxPress);
                                    this.strengthPressTimeLbl.Text = BytesOperator.GetFourValue(this.testSample.TestResult.StrengthMaxPress_Time);
                                    this.strengthChart.Series[0].Points.AddXY(point.X, point.Y);
                                });
                                break;
                            case EnumOfThirdStep.关闭所有阀:
                                this.serialPort_Dan.Write(this.globalState.ThreeState.CloseBuffer, 0,
                                    this.globalState.ThreeState.CloseBuffer.Length);

                                this.cmdQueue.Enqueue("关闭所有阀");
                                this.globalState.ThreeState.EnumOfTS = EnumOfThirdStep.测试终点;
                                break;
                            case EnumOfThirdStep.测试终点:
                                this.globalState.GotoEndTest();
                                this.Invoke((EventHandler)delegate
                                {
                                    this.ventBtn.Enabled = false;
                                    this.setMatBtn.Enabled = false;
                                    this.btnCancelTest.Enabled = false;
                                    this.adviceInfoLbl.Text = "测试结束，请保存数据";
                                });
                                this.serialPort_Dan.Write("#5");
                                this.cmdQueue.Enqueue("停止测试");
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion
                    else {}
                    this.Invoke((EventHandler)delegate
                    {
                        this.receivedInfoLB.Items.Add("H " + resultH.ToString());
                        if (this.receivedInfoLB.Items.Count != 0)
                            this.receivedInfoLB.SelectedIndex = this.receivedInfoLB.Items.Count - 1;
                    });

                    break;
                #endregion

                #region 收到#指令
                case CommandCode.Sharp:
                   //this.adviceInfoLbl.Text =  this.cmdQueue.Dequeue();
                    Thread wait3s = new Thread(new ThreadStart(CloseVavle));
                    switch (this.enumOfCmd)
	                {
                        case EnumOfCmd.第一次关闭所有阀:
                            wait3s.IsBackground = true;
                            wait3s.Start();
                            this.enumOfCmd = EnumOfCmd.打开阀一;
                            break;
                        case EnumOfCmd.打开阀一 :
                            wait3s.Abort();
                            byte[] bufferCmd = new byte[3];
                            BytesOperator.AppendBuffer(bufferCmd, Encoding.Default.GetBytes("#3"), 0);
                            bufferCmd[2] = 0x0E;
                            this.serialPort_Dan.Write(bufferCmd, 0, bufferCmd.Length);
                            this.enumOfCmd = EnumOfCmd.第二次关闭所有阀;
                            this.cmdQueue.Enqueue("打开阀1");
                            break;
                        case EnumOfCmd.第二次关闭所有阀:
                            wait3s.Start();
                            wait3s.IsBackground = true;
                            this.enumOfCmd = EnumOfCmd.检查气压;
                            this.adviceInfoLbl.Text = "检查气道压力中...";
                            break;
                        case EnumOfCmd.检查气压:
                            //wait3s.Abort();
                            this.Invoke((EventHandler)delegate
                            {
                                if (this.hData > 0.5f || this.dData > 0.5f)
                                {
                                    this.adviceInfoLbl.Text = "通道气压过高，请保证通道通畅并排气！";
                                    this.ventBtn.Enabled = true;
                                }
                                else
                                {
                                    this.enumOfCmd = EnumOfCmd.激活开始按钮;
                                    this.serialPort_Dan.Write("#7");
                                    this.cmdQueue.Enqueue("发送#7测试指令");
                                    this.adviceInfoLbl.Text = "测试准备中...";
                                }
                            });
                            break;
                        case EnumOfCmd.激活开始按钮:
                            this.enumOfCmd = EnumOfCmd.NONE;
                            this.Invoke((EventHandler)delegate
                            {
                                this.serialPort_Dan.Write("#21");
                                this.ventBtn.Enabled = false;
                                this.startBtn.Enabled = true;
                                if (this.isFirstTestStart)
                                {
                                    this.adviceInfoLbl.Text = "请进行测试！";
                                }
                                else
                                {
                                    this.adviceInfoLbl.Text = "测试准备完成。";
                                }
                            });
                            break;
                        case EnumOfCmd.开始测试:
                            this.serialPort_Dan.Write("#4");
                            this.cmdQueue.Enqueue("发送#4开始测试指令");
                            this.enumOfCmd = EnumOfCmd.NONE;
                            break;
                        
                        case EnumOfCmd.关闭所有阀:
                            this.serialPort_Dan.Write(this.globalState.OneState.CloseBuffer, 0, this.globalState.OneState.CloseBuffer.Length);
                            this.cmdQueue.Enqueue("关闭所有阀");
                            this.enumOfCmd = EnumOfCmd.NONE;

                            this.globalState.InitState();
                            this.Invoke((EventHandler)delegate
                            {
                                this.btnCancelTest.Enabled = false;
                                //this.ventBtn.Enabled = true;
                                this.setMatBtn.Enabled = true;
                            });
                            break;
                        default:
                            break;
	                }
                    this.Invoke((EventHandler)delegate
                    {
                        this.receivedInfoLB.Items.Add("#000");
                        if (this.receivedInfoLB.Items.Count != 0)
                            this.receivedInfoLB.SelectedIndex = this.receivedInfoLB.Items.Count - 1;
                    });
                    break;
                #endregion
                default:
                    break;
            }
            #endregion
        }

        private SerialPort serialPort_Wen = null;
        private void InitSerialPort_Wen(int portID)
        {
            //this.serialPort_Wen = new SerialPort();
            //this.serialPort_Wen.PortName = string.Format("COM{0}", portID);
            this.serialPort_Wen.BaudRate = (int)settingsReader.GetValue("BaudRate_Wen", typeof(int));
            this.serialPort_Wen.Parity = (Parity)Enum.Parse(typeof(Parity), (string)settingsReader.GetValue("Parity_Wen", typeof(string)));
            this.serialPort_Wen.DataBits = (int)settingsReader.GetValue("DataBits_Wen", typeof(int)); ;
            this.serialPort_Wen.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)settingsReader.GetValue("StopBits_Wen", typeof(string)));
            this.serialPort_Wen.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)settingsReader.GetValue("Handshake_Wen", typeof(string)));
            this.serialPort_Wen.ReadTimeout = (int)settingsReader.GetValue("ReadTimeout_Wen", typeof(int));

            //串口数据接收事件
            this.serialPort_Wen.DataReceived += new SerialDataReceivedEventHandler(
                serialPort_Wen_DataReceived);
            //串口出错事件处理
            this.serialPort_Wen.ErrorReceived += new SerialErrorReceivedEventHandler(
                serialPort_Wen_ErrorReceived);
        }

        private void serialPort_Wen_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            String lastError = e.EventType.ToString();
            if (this.errorInfo2 == lastError)
            {
                this.Invoke((EventHandler)delegate
               {
                   this.receivedInfoLB.Items.Add(this.errorInfo2);
               });
            }
            else
            {
                this.errorInfo2 = lastError;
                MessageBox.Show("串口出错，错误类型：" + e.EventType.ToString(), "错误信息",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void serialPort_Wen_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(75);
            if (!this.serialPort_Wen.IsOpen) return;
            byte[] buffer = new byte[10];
            if (this.serialPort_Wen.IsOpen)
            {
                this.serialPort_Wen.Read(buffer, 0, buffer.Length);
            }

            byte[] temBuffer = new byte[2];
            temBuffer[0] = buffer[1];
            temBuffer[1] = buffer[0];

            float temperature = 0.1f*TransCoding.BinaryToDecimal_Complement(temBuffer, 16);
            if (temperature > 1000f || temperature < 3f)
            {
                return;
            }


            if (!this.isTempReached && !this.startHeatBtn.Enabled && !this.resetTemBtn.Enabled&&!this.isTempReached1)
            {
                this.nowTemLbl.ForeColor = Color.Red;
                this.beVented = false;
                this.isTempReached = this.temState.CountTime(temperature, this.testSample.SetTemperature);
            }
            else
            {
                this.isTempReached1 = this.temState.CountTime1(temperature, this.testSample.SetTemperature);
                this.nowTemLbl.ForeColor = Color.Green;
            }

            this.Invoke((EventHandler)delegate
            {
                //if (temperature.ToString().Length < 3)
                //{
                    this.nowTemLbl.Text=temperature.ToString();
                //}else{
                  //  this.nowTemLbl.Text = temperature.ToString().Substring(0,4);
                //}
                if (this.temState.TimeCount <= 60)
                    this.temReachProBar.Value = this.temState.TimeCount;
            });

            /*自动排气*/
            if (this.temState.TimeCount == 59 && !this.globalState.IsStartTest&&!this.beVented)
            {
                this.VentInit();
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {

            try
            {
                this.a1 = (float)settingsReader.GetValue("a1", typeof(float));
                this.b1 = (float)settingsReader.GetValue("b1", typeof(float));
                this.a2 = (float)settingsReader.GetValue("a2", typeof(float));
                this.b2 = (float)settingsReader.GetValue("b2", typeof(float));
            }
            catch (Exception)
            {
                MessageBox.Show("请检查配置文件的回归方程参数!", "参数错误");
                return;
            }

            int portID_Dan = 2;
            int portID_Wen = 3;
            //找寻单片机对应端口
            portID_Dan=this.SearchPort_Dan(portID_Dan);

            //找寻控温仪对应端口
            this.SearchPort_Wen(portID_Dan,portID_Wen);

            
            
        }

        private void SearchPort_Wen(int portID_Dan,int portID_Wen)
        {
            for (; portID_Wen < 20; portID_Wen++)
            {
                try
                {
                    if (portID_Wen == portID_Dan)
                    {
                        portID_Wen++;
                    }
                    this.serialPort_Wen = new SerialPort("COM" + portID_Wen.ToString());
                    this.serialPort_Wen.Open();
                }
                catch (IOException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //初始化温控仪串口
                this.InitSerialPort_Wen(portID_Wen);

                this.wenLbl.Text = portID_Wen.ToString();
                this.tmCheckTem.Enabled = true;
                return;
            }
            MessageBox.Show("未找到控温仪串口", "连接错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private int SearchPort_Dan(int portID_Dan)
        {
            for ( ; portID_Dan < 20; portID_Dan++)
            {
                try
                {
                    this.serialPort_Dan = new SerialPort("COM" + portID_Dan.ToString());
                    this.serialPort_Dan.Open();
                }
                catch (IOException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return -1;
                }
                //端口初始化
                this.InitSerialPort_Dan(portID_Dan);

                this.danLbl.Text = portID_Dan.ToString();
                this.serialPort_Dan.Write("#5");
                this.enumOfCmd = EnumOfCmd.关闭所有阀;
                this.cmdQueue.Enqueue("关闭所有阀");
                this.receivedInfoLB.Items.Add("发送关闭所有阀指令");
                return portID_Dan;
            }
            MessageBox.Show("未找到单片机串口", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 0;
        }

        private void resetTemBtn_Click(object sender, EventArgs e)
        {
            if (this.globalState.IsStartTest)
            {
                MessageBox.Show("正在进行测试，无法进行温度设置");
                return;
            }
            //if (this.temState.IsTemReset )
            //{
            //    return;
            //}
            if (!this.serialPort_Wen.IsOpen)
            {
                try
                {
                    this.serialPort_Wen.Open();
                }
                catch (Exception)
                {
                    MessageBox.Show("无法开启温度控制器，请检查设备连接。","提示");
                    return;
                } 
            }
            byte[] handControllBuffer = new byte[]{
                0x81,0x81,0x43,0x18,0x00,0x00,0x44,0x18
            };

            byte[] handSetOutBufer = new byte[]{
                0x81,0x81,0x43,0x1A,0x00,0x00,0x44,0x1A
            };

            

            this.serialPort_Wen.Write(handControllBuffer, 0, handControllBuffer.Length);

            Thread.Sleep(400);

            //this.temState.IsTemReset = true;
            this.temState.IsCanStartHeat = true;
            this.setTemTxt.ReadOnly = false;
            this.resetTemBtn.Enabled = false;
            this.startHeatBtn.Enabled= true;
            this.setTemTxt.ForeColor = Color.White;
            this.serialPort_Wen.Write(handSetOutBufer, 0, handSetOutBufer.Length);

            this.adviceInfoLbl.Text = "请设置温度并点击开始加热！";
        }

        private void startHeatBtn_Click(object sender, EventArgs e)
        {
            byte[] autorunBuffer = new byte[]{
            0x81,0x81,0x43,0x18,0x01,0x00,0x45,0x18 };

            if (!this.serialPort_Wen.IsOpen)
                this.serialPort_Wen.Open();
            try
            {
                this.setTemOfTest = Convert.ToInt32(this.setTemTxt.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("请输入正确的数字");
                this.setTemTxt.Text = "900";
                return;
            }

            
            if (this.setTemTxt.Text != null && this.setTemOfTest > 0 && this.setTemOfTest < 1200)
            {
                this.tmCheckTem.Enabled = false;
                this.testSample.SetTemperature = this.setTemOfTest;

                this.serialPort_Wen.Write(TransCoding.TemperatureToByte8(this.testSample.SetTemperature), 0, 8);

                Thread.Sleep(400);

                this.temState = new TemperatureState();
                this.isTempReached = false;
                this.setTemTxt.ReadOnly = true;

                this.serialPort_Wen.Write(autorunBuffer, 0, autorunBuffer.Length);
                this.tmCheckTem.Enabled = true;
                this.startHeatBtn.Enabled = false;
                this.setTemTxt.ForeColor = Color.Yellow;
                this.ventBtn.Enabled = true;

                this.adviceInfoLbl.Text = "请等待加热，温度稳定排气后开始测试。";
                cf.SaveConfig("SetTemperature", this.testSample.SetTemperature.ToString());
            }
            else
            {
                
            }
            //this.isFirstTestStart = true;
        }

        private void setMatBtn_Click(object sender, EventArgs e)
        {
            this.testSample.PersonName = cf.ReadConfig("PersonName") ;
            this.testSample.TestNum = Convert.ToInt32(cf.ReadConfig("TestNumber"));
            this.testSample.TestDepth = (float)Convert.ToDouble(cf.ReadConfig("TestDepth"));
            this.testSample.TestName = cf.ReadConfig("TestName");
            this.testSample.TestFrom = cf.ReadConfig("TestFrom");
            this.testSample.TestTime = cf.ReadConfig("TestTime");

            FormSetting formSetting = new FormSetting();
            formSetting.TestPersonTB.Text = this.testSample.PersonName;
            formSetting.TestDepthTB.Text = this.testSample.TestDepth.ToString();
            formSetting.TestNumTB.Text = this.testSample.TestNum.ToString();
            formSetting.TestNameTB.Text = this.testSample.TestName;
            formSetting.TestFromTB.Text = this.testSample.TestFrom;
            DialogResult dlgResult = formSetting.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                this.testSample.SetTemperature = Convert.ToInt32(cf.ReadConfig("SetTemperature"));
                this.setTemTxt.Text = this.testSample.SetTemperature.ToString();
                this.testSample.PersonName = formSetting.PersonName;
                this.testSample.TestTime = formSetting.TestTime.ToString("yyyy-MM-dd");
                this.testSample.TestNum = formSetting.TestNum;
                this.testSample.TestName = formSetting.TestMaterial;
                this.testSample.TestDepth = formSetting.TestDepth;
                this.testSample.TestFrom = formSetting.TestFrom;
                this.personTxt.Text = this.testSample.PersonName;
                this.testFromTxt.Text = this.testSample.TestFrom;
                this.testNameTxt.Text = this.testSample.TestName;
                this.testNumTxt.Text = this.testSample.TestNum.ToString();
                this.testDepthTxt.Text = this.testSample.TestDepth + "mm";
                if (this.isFirstTestStart == true)
                {
                    this.resetTemBtn.Enabled = true;
                    //this.temState.IsTemReset = false;
                    this.startBtn.Enabled = false;
                    this.ventBtn.Enabled = false;
                    this.startBtn.Enabled = false;
                }
                if (this.isFirstTestStart == false)
                {
                    this.startBtn.Enabled = true;
                    this.resetTemBtn.Enabled = true;
                    /*自动排气*/
                    this.beVented = false;
                    this.VentInit();
                    //this.ventBtn.Enabled = true;
                }
                this.singleStepBtn.Enabled = true;  
                cf.SaveConfig("PersonName", this.testSample.PersonName);
                cf.SaveConfig("TestNumber",this.testSample.TestNum.ToString());
                cf.SaveConfig("TestDepth", this.testSample.TestDepth.ToString());
                cf.SaveConfig("TestName", this.testSample.TestName);
                cf.SaveConfig("TestFrom", this.testSample.TestFrom);
                cf.SaveConfig("TestTime", this.testSample.TestTime);
                if (!this.serialPort_Wen.IsOpen)
                {
                    this.adviceInfoLbl.Text = "未连接温度控制器";
                    return;
                }
                if (this.isFirstTestStart == true)
                {
                    this.adviceInfoLbl.Text = "信息设置完成，请点击设置温度按钮";
                }else{
                    this.adviceInfoLbl.Text = "信息设置完成，如不重设温度可直接开始测试";
                }
            }          
        }

        private void opnBrwBtn_Click(object sender, EventArgs e)
        {
            FormOpenFile formOpenFile = new FormOpenFile();
            DialogResult dialogResult = formOpenFile.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

            }
        }

        private void saveFileBtn_Click(object sender, EventArgs e)
        {
            if (!this.globalState.IsEndTest)
            {
                MessageBox.Show("还未进行测试！");
                return;
            }

            
        InitialSave:
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.FileName = this.testSample.TestTime+"."+this.testSample.TestNum.ToString();
            saveDlg.Filter = "文本格式|*.txt";
            string path = string.Empty;
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                path = saveDlg.FileName;
                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("Info实验人员:" + this.testSample.PersonName);
                sw.WriteLine("Info实验时间:" + this.testSample.TestTime);
                sw.WriteLine("Info试样编号:" + this.testSample.TestNum);
                sw.WriteLine("Info涂料名称:" + this.testSample.TestName);
                sw.WriteLine("Info试样厚度:" + this.testSample.TestDepth+"mm");
                sw.WriteLine("Info设置温度:" + this.testSample.SetTemperature + "℃");
                sw.WriteLine("Info来样单位:" + this.testSample.TestFrom);
                //保存传输特性最大的压强
                if (this.testSample.TestResult.TransMaxPress.ToString().Length>4)
                    sw.WriteLine("Info传输特性:最大压强" + this.testSample.TestResult.TransMaxPress.ToString().Substring(0, 4)+"KPa");
                else
                    sw.WriteLine("Info传输特性:最大压强" + this.testSample.TestResult.TransMaxPress.ToString() + "KPa");
                //保存传输特性最大压强对应的时间
                if (this.testSample.TestResult.TranMaxPrees_Time>=100)
                    sw.WriteLine("Info传输特性:最大压强时间" + this.testSample.TestResult.TranMaxPrees_Time.ToString().Substring(0, 3)+"s");
                else if (this.testSample.TestResult.TranMaxPrees_Time.ToString().Length > 4)
                    sw.WriteLine("Info传输特性:最大压强时间" + this.testSample.TestResult.TranMaxPrees_Time.ToString().Substring(0, 4) + "s");
                else
                    sw.WriteLine("Info传输特性:最大压强时间" + this.testSample.TestResult.TranMaxPrees_Time.ToString() + "s");
                //保存传输特性的排除时间
                if (this.testSample.TestResult.TranEndTime >= 100)
                    sw.WriteLine("Info高温透气性:排出时间" + this.testSample.TestResult.TranEndTime.ToString().Substring(0, 3) + "s");
                else if (this.testSample.TestResult.TranEndTime.ToString().Length>4)
                    sw.WriteLine("Info高温透气性:排出时间" + this.testSample.TestResult.TranEndTime.ToString().Substring(0,4) + "s");
                else
                    sw.WriteLine("Info高温透气性:排出时间" + this.testSample.TestResult.TranEndTime.ToString()+ "s");
                //保存高温透气率
                if (this.testSample.TestResult.Ventiratio >= 1000)
                    sw.WriteLine("Info高温透气性:透气率" + this.testSample.TestResult.Ventiratio.ToString().Substring(0, 6) + "cm²/Pa.min");
                else if (this.testSample.TestResult.Ventiratio >= 100)
                    sw.WriteLine("Info高温透气性:透气率" + this.testSample.TestResult.Ventiratio.ToString().Substring(0, 5) + "cm²/Pa.min");
                else if (this.testSample.TestResult.Ventiratio.ToString().Length>4)
                    sw.WriteLine("Info高温透气性:透气率" + this.testSample.TestResult.Ventiratio.ToString().Substring(0, 4) + "cm²/Pa.min");
                else
                    sw.WriteLine("Info高温透气性:透气率" + this.testSample.TestResult.Ventiratio.ToString() + "cm²/Pa.min");
                //保存涂层最大强度
                if (this.testSample.TestResult.StrengthMaxPress.ToString().Length > 4)
                    sw.WriteLine("Info高温强度:最大压强" + this.testSample.TestResult.StrengthMaxPress.ToString().Substring(0, 4) + "KPa");
                else
                    sw.WriteLine("Info高温强度:最大压强" + this.testSample.TestResult.StrengthMaxPress.ToString() + "KPa");
                //保存最大强度对应的时间
                if (this.testSample.TestResult.StrengthMaxPress_Time.ToString().Length > 4)
                    sw.WriteLine("Info高温强度:最大压强时间" + this.testSample.TestResult.StrengthMaxPress_Time.ToString().Substring(0, 4) + "s");
                else
                    sw.WriteLine("Info高温强度:最大压强时间" + this.testSample.TestResult.StrengthMaxPress_Time.ToString() + "s");
                foreach (PointF point in this.testSample.TransPoints)
                {
                    sw.WriteLine("TpX" + point.X.ToString() + "Y" + point.Y.ToString());
                }
                foreach (PointF point in this.testSample.VentiPoints)
                {
                    sw.WriteLine("VpX" + point.X.ToString() + "Y" + point.Y.ToString());
                }
                foreach (PointF point in this.testSample.StrengthPoints)
                {
                    sw.WriteLine("SpX" + point.X.ToString() + "Y" + point.Y.ToString());
                }
                sw.Close();
                this.setMatBtn.Enabled = true;
                this.adviceInfoLbl.Text = "数据已保存，设置相关信息可进行再一次测试。";

                this.transPressLbl.Text = "----";
                this.transPressTimeLbl.Text = "----";
                this.transEndTimeLbl.Text = "----";
                this.ventiLbl.Text = "----";
                this.ventiNowPressLbl.Text = "----";
                this.strengthPressLbl.Text = "----";
                this.strengthPressTimeLbl.Text = "----";

                this.testSample.CleanPoint();
                this.transChart.Series[0].Points.Clear();
                this.ventiChart.Series[0].Points.Clear();
                this.strengthChart.Series[0].Points.Clear();
                this.testSample.TestNum++;
            }
            else
            {
                DialogResult dalResult = MessageBox.Show("是否清除数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dalResult == DialogResult.Yes)
                {
                    this.setMatBtn.Enabled = true;
                    this.adviceInfoLbl.Text = "数据已清除，设置样品信息后可重新测试。";

                    this.transPressLbl.Text = "----";
                    this.transPressTimeLbl.Text = "----";
                    this.transEndTimeLbl.Text = "----";
                    this.ventiLbl.Text = "----";
                    this.ventiNowPressLbl.Text = "----";
                    this.strengthPressLbl.Text = "----";
                    this.strengthPressTimeLbl.Text = "----";

                    this.testSample.CleanPoint();
                    this.transChart.Series[0].Points.Clear();
                    this.ventiChart.Series[0].Points.Clear();
                    this.strengthChart.Series[0].Points.Clear();
                }
                else
                {
                    goto InitialSave;
                }
            }
            

            if (!this.singleStep)
            {
                this.singleStepBtn.Enabled = true;
            }

            this.ventBtn.Enabled = false ;
            this.startBtn.Enabled = false;
            this.globalState.IsEndTest = true ;
        }


        private void VentInit()
        {
            if (this.beVented)
            {
                return;
            }
            this.adviceInfoLbl.Text = "排气中！";
            byte[] buffer = new byte[3];
            BytesOperator.AppendBuffer(buffer, Encoding.Default.GetBytes("#3"), 0);

            buffer[2] = 0x0c;
            this.serialPort_Dan.Write(buffer, 0, buffer.Length);
            this.cmdQueue.Enqueue("打开阀1和阀2");
            this.enumOfCmd = EnumOfCmd.第二次关闭所有阀;

            this.sleepTime_Two = true;
            this.beVented = true;
            this.Invoke((EventHandler)delegate
            {
                this.ventBtn.Enabled = false;
                this.globalState.InitState();
                this.transChart.Series[0].Points.Clear();
                this.ventiChart.Series[0].Points.Clear();
                this.strengthChart.Series[0].Points.Clear();
            });
            
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.ventBtn.Enabled = false;
            this.btnCancelTest.Enabled = true;
            this.startBtn.Enabled = false;
            this.setMatBtn.Enabled = false;
            if (!isFirstTestStart && this.resetTemBtn.Enabled)
            { this.resetTemBtn.Enabled = false; }

            //this.serialPort_Dan.Write("#4");
            //this.enumOfCmd = EnumOfCmd.开始测试;
            //this.cmdQueue.Enqueue("发送#4开始测试指令");

            this.isFirstTestStart = false;

            /*是否激活单步测试*/
            if(!this.singleStep){
                this.singleStepBtn.Enabled = false;
                this.globalState.GotoFirstStep();
                this.selectTabControl.SelectedIndex = 0;
                this.enumOfCmd = EnumOfCmd.开始测试;
                this.serialPort_Dan.Write("#21");
            }
            else
            {
                if (this.selectTabControl.SelectedIndex == 0)
                {
                    this.globalState.GotoFirstStep();
                    this.selectTabControl.SelectedIndex = 0;
                    this.enumOfCmd = EnumOfCmd.开始测试;
                    this.serialPort_Dan.Write("#21");
                }
                else if (this.selectTabControl.SelectedIndex == 1)
                {
                    this.enumOfCmd = EnumOfCmd.开始测试;
                    this.globalState.GotoSecondStep();
                    this.serialPort_Dan.Write("#22");
                    this.globalState.OneState.EnumOfFS = EnumOfFirstStep.NONE;
                }
                else if (this.selectTabControl.SelectedIndex == 2)
                {
                    this.globalState.GotoThridStep();
                    this.enumOfCmd = EnumOfCmd.开始测试;
                    this.serialPort_Dan.Write("#22");
                    this.globalState.TwoState.EnumOfSS = EnumOfSecondStep.NONE;
                }
            }
            this.adviceInfoLbl.Text = "正在测试中！！";
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                //关闭炉温控制  
                byte[] closeBufferWhenClosed = new byte[3];
                BytesOperator.AppendBuffer(closeBufferWhenClosed, Encoding.Default.GetBytes("#3"), 0);

                closeBufferWhenClosed[2] = 0x0F;
                this.serialPort_Dan.Write(closeBufferWhenClosed, 0, closeBufferWhenClosed.Length);

                this.cmdQueue.Enqueue("关闭所有阀");
                if (this.serialPort_Dan.IsOpen)
                {
                    this.serialPort_Dan.Close();
                }
                this.tmCheckTem.Enabled = false;
                if (this.serialPort_Wen.IsOpen)
                {
                    this.serialPort_Wen.Close();
                }
            }
            catch
            {
            }

            System.Threading.Thread.Sleep(65);
            Application.Exit(); 
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dalResult = MessageBox.Show("确定要退出程序吗？", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dalResult != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void tmCheckTem_Tick(object sender, EventArgs e)
        {
            if (this.serialPort_Wen != null && this.serialPort_Wen.IsOpen)
                this.serialPort_Wen.Write(this.checkTemBuffer, 0, this.checkTemBuffer.Length);
        }

        private void btnCancelTest_Click(object sender, EventArgs e)
        {
            this.enumOfCmd = EnumOfCmd.关闭所有阀;
            this.serialPort_Dan.Write("#5");
            this.cmdQueue.Enqueue("发送强制停止指令");
            this.adviceInfoLbl.Text = "测试已停止";
            this.ventBtn.Enabled = true;
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            if (!(!this.globalState.IsStartTest || this.globalState.IsEndTest))
            {
                MessageBox.Show("测试进行中！无法调试。");
                return;
            }

            if (this.serialPort_Dan.IsOpen)
            {
                FormTest formTest = new FormTest(this.serialPort_Dan);
                formTest.ShowDialog();

            }
            else
            {
                MessageBox.Show("未找到单片机串口!");
            }
        }

        private void ventBtn_Click(object sender, EventArgs e)
        {
            /*分析前的排气初始化*/
            if (this.temReachProBar.Value < 6)//60
            {
                MessageBox.Show("温度未稳定，请稍等！");
                return;
            }
            
            this.beVented = false;
            VentInit();
        }

        private void singleStepBtn_Click(object sender, EventArgs e)
        {
            /*激活单步操作*/
            if (!(!this.globalState.IsStartTest || this.globalState.IsEndTest))
            {
                MessageBox.Show("测试进行中！无法更改。");
                return;
            }
            this.singleStep = !this.singleStep;
            if (singleStep)
            {
                this.singleStepBtn.Image = HDoit.Instrument.CoatingAnalysis.Properties.Resources.motors_on2_48;
                this.adviceInfoLbl.Text = "启用单步测试!请选择相应的曲线图开始测试。";
            }
            else
            {
                this.singleStepBtn.Image = HDoit.Instrument.CoatingAnalysis.Properties.Resources.motors_off2_48;
                this.adviceInfoLbl.Text = "已取消单步测试!";
            }
        }

        private void selectTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
