using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Configuration;

using System.IO;
using System.IO.Ports;

using System.Threading;
using System.Diagnostics;

using System.Xml;
using System.Xml.Serialization;


using GET3G_PC.Controls;

namespace GET3G_PC
{
	public partial class FormMain : Form
	{

        private List<GasData> gasDatas = new List<GasData>();

        private float maxGas = 0;
        private long maxGasTime =0;
        private float maxGasIncrement = 0;
        private string saveMaxGasIncrement;
        private long nCount = 0;                          //计时用
        private bool isFirstToGD = true;                //判断是否是发气量的第一个数据
        private long timeFurnaceT = 0;                  //炉温稳定时间

        //private long timeSystemT = 0;                   //系统温度稳定时间

		//private List<ReceivedData> datasReceived = new List<ReceivedData>();
        //private List<ReceivedData> pidReceived = new List<ReceivedData>(); 
		private SerialPort serialPort = null;
		private AppSettingsReader settingsReader = new AppSettingsReader();		
        private List<byte> bufferOfReceived = new List<byte>();

        int iCount = 0;             //实验次数标记
		
        //private PIDController furnacepidController = new PIDController();
		//炉温目标值调节部分
		//float TemperatureTarget;
		//炉温控制
		float furnaceTemperatureULast;
		float furnaceTemperatureTarget;					    //设定炉温
		float furnaceTemperatureCurrent = 0;				//当前炉温
		float furnaceTemperatureLast;						//上次炉温
		float furnaceTemperatureLastLast;					//上上次炉温
        float furnaceTemperatureOutPutValue;
        float furnaceTemperatureUDelta=0;


		//系统温度控制
        //float systemTemperatureULast;
        //float systemTemperatureTarget;					//设定系统温度
        //float systemTemperatureCurrent = 0;				//当前系统温度
        //float systemTemperatureLast;				    //上次系统温度
        //float systemTemperatureLastLast;				//上上次系统温度
        //float systemTemperatureUDelta;
		//发气量计算相关参数
		float sampleWeight=1.0f;							//试样重量
        bool isVG_0 = false;                            //判断是否可以收VG_i
        float nCompress = 0;                            //初始化接到的压力值总数
        float[] gasArray = new float[5];                //存compress的数组
        long nGas = 0;                                  //记录初始化时传来compress的个数

        float VG_0 = 0f;									//初次压力传感器传出值
		float VG_i = 0f;									//第i次压力传感器传出值
       
		float gasLast = 0f;										//上一次发气量

		private bool isWorking = false;
        private bool isDisplayTemperature = true;                   //控制是否向画板传数据
       
		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			CheckForIllegalCrossThreadCalls = false;
            this.btnStartGD.Enabled = false;

            this.txtFactory.Enabled         = false;
            this.txtPeople.Enabled          = false;
            this.txtRepeat.Enabled          = false;
            this.txtSampleName.Enabled      = false;
            this.txtSampleNumber.Enabled    = false;
            this.txtSampleWeight.Enabled    = false;

            this.txtSampleWeight.Text=(string)settingsReader.GetValue("SampleWeight",typeof(string));
            this.txtSampleName.Text = (string)settingsReader.GetValue("SampleName", typeof(string));
            this.txtSampleNumber.Text = (string)settingsReader.GetValue("SampleNumber", typeof(string));
            this.txtFactory.Text = (string)settingsReader.GetValue("Factory", typeof(string));
            this.txtPeople.Text = (string)settingsReader.GetValue("People", typeof(string));
            
            this.lblCurrentFurnaceTemprature.BackColor = Color.Black;
            this.lblCurrentFurnaceTemprature.ForeColor = Color.Red;
            this.lblCurrentRoomTemprature.BackColor = Color.Black;
            this.lblCurrentRoomTemprature.ForeColor = Color.Red;
            this.lblTargetFurnaceTemprature.BackColor = Color.Black;
            this.lblTargetFurnaceTemprature.ForeColor = Color.LawnGreen;

            this.lblTime.BackColor = Color.Black;
            this.lblTime.ForeColor = Color.Red;

            this.lblFT.BackColor = Color.Black;
            this.lblFT.ForeColor = Color.Red;
            this.lblGas.BackColor = Color.Black;
            this.lblGas.ForeColor = Color.Red;
            this.lblST.BackColor = Color.Black;
            this.lblST.ForeColor = Color.Red;
           
            this.lblTargetFurnaceTemprature.Text = (string)settingsReader.GetValue("SettingValue", typeof(string));
           
            //this.InitValues();

			int portID = 3;

		InitCOM:		//自适应连接COM口			
			try
			{
				//初始化串口
				this.InitSerialPort(portID);

				//打开串口
				this.serialPort.Open();

                Command cmd = new Command(CommandCode._0, "0D");
                byte[] bytes = cmd.ToBytes();
                this.serialPort.Write(bytes, 0, bytes.Length);
                
			}
			catch (IOException)
			{
				if (portID > 20)
				{
					MessageBox.Show("USB口无法打开", "连接错误",
												MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				portID++;

				//跳转语句
				goto InitCOM;

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }

		private void InitValues()
		{   
			this.furnaceTemperatureTarget =int.Parse(this.lblTargetFurnaceTemprature.Text.Trim());
			this.furnaceTemperatureLast = this.furnaceTemperatureTarget;
			this.furnaceTemperatureLastLast = this.furnaceTemperatureTarget;
			this.furnaceTemperatureULast = (float)settingsReader.GetValue("Furnace_850_U0",typeof(float));
			
            //this.systemTemperatureTarget = (float)settingsReader.GetValue("SystemTemperatureTarget",typeof(float));
            //this.systemTemperatureLast = this.systemTemperatureTarget;
            //this.systemTemperatureLastLast = this.systemTemperatureTarget;
            //this.systemTemperatureULast = (float)settingsReader.GetValue("System_U0", typeof(float));
		}

		private void InitSerialPort(int portID)
		{
			this.serialPort = new SerialPort();

			this.serialPort.PortName = string.Format("COM{0}", portID);
			this.serialPort.BaudRate = (int)settingsReader.GetValue("BaudRate", typeof(int));
			this.serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), (string)settingsReader.GetValue("Parity", typeof(string)));
			this.serialPort.DataBits = (int)settingsReader.GetValue("DataBits", typeof(int)); ;
			this.serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)settingsReader.GetValue("StopBits", typeof(string)));
			this.serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)settingsReader.GetValue("Handshake", typeof(string)));
			this.serialPort.ReadTimeout = (int)settingsReader.GetValue("ReadTimeout", typeof(int));

            ////串口出错事件处理
            //this.serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);

            ////串口数据接收事件
            //this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
		}

		void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
            //MessageBox.Show("USB口出错，错误类型：" + e.EventType.ToString(), "错误信息",
            //                            MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //serialPort对象的第一个参数是它本身的引用，第二个参数则是SerialDataReceivedEventArgs对象的一个实例,它是由事件传送的参数 
                System.Threading.Thread.Sleep(45);	//延时，以防止单片机传输过来的数据不完整

                if (!this.serialPort.IsOpen) return;

                byte[] buffer = new byte[8];

                this.serialPort.Read(buffer, 0, buffer.Length);

                Command cmdTemp = new Command(buffer);

                string dataValue = string.Empty;

                this.furnaceTemperatureCurrent = this.furnaceTemperatureTarget;

                this.lblStatus.Text = cmdTemp.Code.ToString();

                switch (cmdTemp.Code)
                {
                    case CommandCode.TF:	//炉温数据

                        this.isWorking = true;

                        if (this.isWorking)
                        {
                           
                            this.btnChangeTemperature.Enabled = false;
                            this.btnCustom.Enabled = false;
                            this.panCustom.Visible = false;

                            this.btnStart.Text = "停止从机";
                        }
                        else
                        {
                            
                            this.btnChangeTemperature.Enabled = true;
                            this.btnCustom.Enabled = true;

                            this.btnStart.Text = "启动从机";
                        }

                        PIDController furnacepidController = new PIDController(Controller.Furnace_850);

                        int targetTempratureTemp = int.Parse(this.lblTargetFurnaceTemprature.Text);
                        if (targetTempratureTemp <= 750)
                        {
                            furnacepidController = new PIDController(Controller.Furnace_700);
                        }
                        if (targetTempratureTemp > 750 && targetTempratureTemp <= 950)
                        {
                            furnacepidController = new PIDController(Controller.Furnace_850);
                        }
                        if (targetTempratureTemp > 950)
                        {
                            furnacepidController = new PIDController(Controller.Furnace_1000);
                        }

                        #region 废弃代码
                        //if (this.lblTargetFurnaceTemprature.Text == "1000") furnacepidController = new PIDController(Controller.Furnace_1000);
                        //if (this.lblTargetFurnaceTemprature.Text == "850") furnacepidController = new PIDController(Controller.Furnace_850);
                        //if (this.lblTargetFurnaceTemprature.Text == "700") furnacepidController = new PIDController(Controller.Furnace_700);
                        //if (this.lblTargetFurnaceTemprature.Text == "900") furnacepidController = new PIDController(Controller.Furnace_900);
                        #endregion


                        float roomTemperature = DataConvert.ConvertToRoomTemperature(NumberSystem.BinaryToDecimal_Complement(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 0, 2), 16));

                        this.lblCurrentRoomTemprature.Text = string.Format("{0:00.0}", roomTemperature);

                        float roomV = DataConvert.ConvertTemperatureToV(roomTemperature);
                        float v = DataConvert.ConvertToThermocouple(NumberSystem.BinaryToDecimal(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 2, 3), 16));
                        float furnaceTemperatureV = roomV + v;
                        float furnaceTemperature = DataConvert.ConvertVToTemperature(furnaceTemperatureV);

                        if (Convert.ToInt32(furnaceTemperature) == this.furnaceTemperatureTarget)
                        {
                            timeFurnaceT++;
                        }
                        else
                        {
                            timeFurnaceT = 0;
                            this.lblCurrentFurnaceTemprature.ForeColor = Color.Red;
                        }
                        if (timeFurnaceT == 150)
                        {
                            this.lblCurrentFurnaceTemprature.ForeColor = Color.LawnGreen;
                        }

                        if (this.isDisplayTemperature == true)
                        {
                            this.temperatureCurveCtrl.TargetFurnaceTemperature = this.furnaceTemperatureTarget;
                            this.temperatureCurveCtrl.FurnaceTemperature = furnaceTemperature;
                            this.temperatureCurveCtrl.DisplayTemperatureCurve();
                        }

                        this.furnaceTemperatureCurrent = furnaceTemperature;
                        byte furnaceControlValue = furnacepidController.OutPut(this.furnaceTemperatureTarget,
                                                                                  this.furnaceTemperatureCurrent,
                                                                                  this.furnaceTemperatureLast,
                                                                                  this.furnaceTemperatureLastLast,
                                                                                  ref this.furnaceTemperatureULast,
                                                                                  ref this.furnaceTemperatureUDelta);

                        this.furnaceTemperatureOutPutValue = furnaceControlValue;
                        this.furnaceTemperatureLastLast = this.furnaceTemperatureLast;
                        this.furnaceTemperatureLast = this.furnaceTemperatureCurrent;

                        //发送炉温控制量
                        byte[] furnaceControlCmdBuffer = new byte[4];
                        BytesOperator.AppendBuffer(furnaceControlCmdBuffer, Encoding.Default.GetBytes("#1"), 0);
                        BytesOperator.AppendBuffer(furnaceControlCmdBuffer, new byte[] { furnaceControlValue }, 2);
                        furnaceControlCmdBuffer[3] = 0x0D;
                        this.serialPort.Write(furnaceControlCmdBuffer, 0, furnaceControlCmdBuffer.Length);
                        dataValue = (Convert.ToInt32(furnaceTemperature)).ToString();

                        this.lblCurrentFurnaceTemprature.Text = dataValue;
                        this.lblFT.Text = dataValue;          //添加的语句
                        break;
                    #region 废弃代码-系统温度控制
                    //case CommandCode.TS:	//系统温度数据

                    //    this.isWorking = true;

                    //    if (this.isWorking)
                    //    {
                    //        this.btnDown.Enabled = false;
                    //        this.btnChangeTemperature.Enabled = false;
                    //        this.btnStart.Text = "停止从机";
                    //    }
                    //    else
                    //    {
                    //        this.btnDown.Enabled = true;
                    //        this.btnChangeTemperature.Enabled = true;
                    //        this.btnStart.Text = "启动从机";
                    //    }

                    //float systemTemperature = DataConvert.ConvertToSystemTemperature(NumberSystem.BinaryToDecimal(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 0, 3), 16));
                    //dataValue = (Convert.ToInt32(systemTemperature)).ToString();
                    //if (Convert.ToInt32(systemTemperature) == 105)
                    //{
                    //    timeSystemT++;
                    //}
                    //else
                    //{
                    //    timeSystemT = 0;
                    //    this.lblCurrentSystemTemprature.ForeColor = Color.Red;
                    //}
                    //if (timeSystemT == 150)
                    //{
                    //    this.lblCurrentSystemTemprature.ForeColor = Color.LawnGreen;
                    //}

                    //this.systemTemperatureCurrent = systemTemperature;

                    //PIDController systemPIDController = new PIDController(Controller.System);
                    //byte systemControlValue = systemPIDController.OutPut(this.systemTemperatureTarget,
                    //                                                        this.systemTemperatureCurrent,
                    //                                                        this.systemTemperatureLast,
                    //                                                        this.systemTemperatureLastLast,
                    //                                                        ref this.systemTemperatureULast, ref this.systemTemperatureUDelta);

                    //this.systemTemperatureLastLast = this.systemTemperatureLast;
                    //this.systemTemperatureLast = this.systemTemperatureCurrent;

                    //if (this.isDisplayTemperature == true)
                    //{
                    //    this.temperatureCurveCtrl.SystemTemperature = systemTemperature;
                    //    this.temperatureCurveCtrl.DisplayTemperatureCurve();
                    //}

                    //发送系统控制温度
                    //byte[] systemControlCmdBuffer = new byte[4];
                    //BytesOperator.AppendBuffer(systemControlCmdBuffer, Encoding.Default.GetBytes("#2"), 0);
                    //BytesOperator.AppendBuffer(systemControlCmdBuffer, new byte[] { systemControlValue }, 2);
                    //systemControlCmdBuffer[3] = 0x0D;

                    ////以下注释程序为向串口写系统温度控制量
                    //this.serialPort.Write(systemControlCmdBuffer, 0, systemControlCmdBuffer.Length);

                    //this.lblCurrentSystemTemprature.Text = dataValue;
                    //this.lblST.Text = dataValue;          //添加的语句
                    //break;
                    #endregion
                    case CommandCode.START:	//START 开始发气量采集                   
                        dataValue = "开始发气量采集";
                        this.isVG_0 = true;

                        //只当界面为“显示温度数据”时，
                        //才开始进入“发气量数据”界面，开始发气量数据采集
                        byte[] postBackCmdBuffer = new byte[1];

                        if (this.panelT.Visible == true)
                        {
                            //发送回馈指令，可以接受“发气量数据”，反馈指令内容为“0x0D”
                            postBackCmdBuffer[0] = 0x0D;
                            this.Invoke(new Del_ChangeViewToGas(this.ChangeViewToGas));
                        }
                        else
                        {
                            //发送回馈指令，不可以接受“发气量数据”，反馈指令内容为“0xAA”
                            postBackCmdBuffer[0] = 0xAA;
                            this.isVG_0 = false;
                        }

                        this.serialPort.Write(postBackCmdBuffer, 0, postBackCmdBuffer.Length);

                        break;
                    case CommandCode.END:	 //END 结束发气量采集
                        dataValue = "结束发气量采集";

                        this.Invoke(new Del_EndGasTest(this.EndGasTest));

                        this.isVG_0 = false;
                        break;

                    case CommandCode.GD:	//发气量采集数据
                        //添加
                        GasData gasData = new GasData();

                        long time = NumberSystem.BinaryToDecimal(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 0, 2), 16);
                        long compressTemp = NumberSystem.BinaryToDecimal_Complement(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 2, 3), 16);
                        float compress = DataConvert.ConvertToThermocouple_Compress(compressTemp);
                        if (isVG_0 == false)
                        {
                            switch (nGas % 5)
                            {
                                case 0: gasArray[0] = compress;
                                    break;
                                case 1: gasArray[1] = compress;
                                    break;
                                case 2: gasArray[2] = compress;
                                    break;
                                case 3: gasArray[3] = compress;
                                    break;
                                case 4: gasArray[4] = compress;
                                    break;
                            }
                            for (int i = 0; i < 5; i++)
                                nCompress += gasArray[i];
                            this.VG_0 = nCompress / 5;
                            nGas++;
                            nCompress = 0;
                        }
                        if (isVG_0 == true)
                        {
                            float gas = 0;
                            nGas = 0;
                            this.VG_i = compress;
                            sampleWeight = float.Parse(this.txtSampleWeight.Text);

                            int targetTemprature = int.Parse(this.lblTargetFurnaceTemprature.Text);
                            if (targetTemprature <= 750)
                            {
                                gas = DataConvert.ConvertVToGas_700(this.VG_i, this.VG_0, this.sampleWeight, time);
                            }
                            if (targetTemprature > 750 && targetTemprature <= 950)
                            {
                                gas = DataConvert.ConvertVToGas_850(this.VG_i, this.VG_0, this.sampleWeight, time);
                            }
                            if (targetTemprature > 950)
                            {
                                gas = DataConvert.ConvertVToGas_1000(this.VG_i, this.VG_0, this.sampleWeight, time);
                            }

                            #region 废弃代码
                            //if (this.lblTargetFurnaceTemprature.Text == "850")
                            //{
                            //    gas = DataConvert.ConvertVToGas_850(this.VG_i, this.VG_0, this.sampleWeight, time);
                            //}

                            //if (this.lblTargetFurnaceTemprature.Text == "1000")
                            //{
                            //    gas = DataConvert.ConvertVToGas_1000(this.VG_i, this.VG_0, this.sampleWeight, time);
                            //}

                            //if (this.lblTargetFurnaceTemprature.Text == "700")
                            //{
                            //    gas = DataConvert.ConvertVToGas_700(this.VG_i, this.VG_0, this.sampleWeight, time);
                            //}

                            //if (this.lblTargetFurnaceTemprature.Text == "900")
                            //{
                            //    gas = DataConvert.ConvertVToGas_900(this.VG_i, this.VG_0, this.sampleWeight, time);
                            //}
                            #endregion

                            if (this.maxGas < gas)                         //找出最大发气量及时间
                            {
                                this.maxGas = gas;
                                this.maxGasTime = time;
                            }

                            if (isFirstToGD == true)                       //是否是第一次接发气数据
                            {
                                this.gasLast = gas;
                                this.isFirstToGD = false;
                            }
                            dataValue = gas.ToString();
                            this.lblGas.Text = dataValue;
                            gasData.GasDatas = dataValue;
                            gasData.Time = time;
                            gasData.GasIncrementDatas = ((gas - this.gasLast) / (int)settingsReader.GetValue("Gas_DrawData_Interval", typeof(int))).ToString();
                            gasDatas.Add(gasData);

                            //显示“发气量/发气速度”曲线
                            this.gasCurveCtrl.GasTime = time;

                            lblTime.BackColor = Color.Black;
                            lblTime.ForeColor = Color.Red;

                            this.gasCurveCtrl.Gas = gas;
                            //this.gasCurveCtrl.Interval = (int)settingsReader.GetValue("Gas_DrawData_Interval", typeof(int));
                            this.gasCurveCtrl.GasInCrement = (gas - this.gasLast) / (int)settingsReader.GetValue("Gas_DrawData_Interval", typeof(int));
                            this.gasCurveCtrl.DisplayGasCurve();

                            if (this.maxGasIncrement < (gas - this.gasLast) / (int)settingsReader.GetValue("Gas_DrawData_Interval", typeof(int)))
                            {
                                this.maxGasIncrement = (gas - this.gasLast) / (int)settingsReader.GetValue("Gas_DrawData_Interval", typeof(int));
                            }
                            this.gasLast = gas;

                        }
                        break;
                    case CommandCode.E0:	//E0：热电偶通道开路错误	E1：压力传感器通道开路错误	E2：系统温度传感器通道开路错误
                        dataValue = "热电偶通道开路错误";
                        this.lblCurrentFurnaceTemprature.Text = "Err0";
                        MessageBox.Show("热电偶通道开路错误", "请检查硬件错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case CommandCode.E1:
                        dataValue = "压力传感器通道开路错误";
                        this.lblCurrentFurnaceTemprature.Text = "Err1";
                        MessageBox.Show("压力传感器通道开路错误", "请检查硬件错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case CommandCode.E2:
                        dataValue = "系统温度传感器通道开路错误";
                        this.lblCurrentFurnaceTemprature.Text = "Err2";
                        MessageBox.Show("系统温度传感器通道开路错误", "请检查硬件错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case CommandCode.O0:	//OK0：热电偶通道正常		OK1：压力传感器通道正常	OK2：系统温度传感器通道正常	
                        dataValue = "热电偶通道正常";

                        break;
                    case CommandCode.O1:
                        dataValue = "压力传感器通道正常";

                        break;
                    case CommandCode.O2:
                        dataValue = "系统温度传感器通道正常";

                        break;
                    case CommandCode._0:	//仪器在线查询指令返回结果
                        dataValue = "仪器在线";
                        break;
                    case CommandCode._1:	//发送加热炉控制指令返回结果
                        dataValue = "加热炉温控制指令正常执行";
                        break;
                    case CommandCode._2:	//发送系统控制指令返回结果
                        dataValue = "系统控制指令正常执行";
                        break;
                    case CommandCode._3:	//请求重新发送上一个压力采样数据
                        dataValue = "重新发送上一个压力采样数据";
                        break;
                    case CommandCode._4:	//开始实验指令返回结果
                        dataValue = "开始实验指令正常执行";
                        break;
                    case CommandCode._5:	//结束实验指令返回结果
                        dataValue = "结束实验指令正常执行";

                        break;
                    case CommandCode._6:	//开始发送数据指令返回结果
                        dataValue = "开始发送数据指令正常执行";
                        break;
                    case CommandCode._E:	//指令出错
                        dataValue = "指令执行出错";
                        break;

                    #region 废弃代码
                    /*
                default:
                    float roomTemperature_InitData = DataConvert.ConvertToRoomTemperature(NumberSystem.BinaryToDecimal_Complement(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 0, 2), 16));

                    float roomV_InitData = DataConvert.ConvertTemperatureToV(roomTemperature_InitData);
                    float v_InitData = DataConvert.ConvertToThermocouple(NumberSystem.BinaryToDecimal(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 2, 3), 16));
                    float furnaceTemperatureV_InitData = roomV_InitData + v_InitData;
                    float furnaceTemperature_InitData = DataConvert.ConvertVToTemperature(furnaceTemperatureV_InitData);
                    furnaceTemperature_InitData = Convert.ToInt32(furnaceTemperature_InitData);
                    //this.lblCurrentFurnaceTemprature.Text = furnaceTemperature_InitData.ToString();

                    //float systemTemperature_InitData = DataConvert.ConvertToSystemTemperature(NumberSystem.BinaryToDecimal(BytesOperator.CutBuffer(cmdTemp.DataBuffer, 5, 3), 16));
                    //systemTemperature_InitData = Convert.ToInt32(systemTemperature_InitData);
                    //this.lblCurrentSystemTemprature.Text=systemTemperature_InitData.ToString();

                    break;
                     * */
                    #endregion
                }
            }
            catch (TimeoutException)
            {

            }

        }

        private delegate void Del_ChangeViewToGas();

        private void ChangeViewToGas()
        {
            this.panelT.Visible = false;
            this.panelG.Visible = true;
            this.timerClock.Enabled = true;

            this.btnPrint.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnDrop.Enabled = false;
            this.btnEndGD.Enabled = true;
            
            this.isDisplayTemperature =false;
            this.gasCurveCtrl.ResetGasDatas();
            this.btnStartGD.Enabled = false;
            this.timerDelay.Enabled =false;
        }

//////////////////////////////按键相关//////////////////////////////////////////

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			//程序关闭时，如果从机在向主机发送数据，
			//主机将自动向从机发送停止指令
			try
			{
                //关闭炉温控制  
                byte[] furnaceControlCmdBuffer = new byte[4];
                BytesOperator.AppendBuffer(furnaceControlCmdBuffer, Encoding.Default.GetBytes("#1"), 0);
                BytesOperator.AppendBuffer(furnaceControlCmdBuffer, new byte[] { 0x00 }, 2);
                furnaceControlCmdBuffer[3] = 0x0D;
                this.serialPort.Write(furnaceControlCmdBuffer, 0, furnaceControlCmdBuffer.Length);
                System.Threading.Thread.Sleep(45);

                //关闭系统温度控制
                byte[] systemControlCmdBuffer = new byte[4];
                BytesOperator.AppendBuffer(systemControlCmdBuffer, Encoding.Default.GetBytes("#2"), 0);
                BytesOperator.AppendBuffer(systemControlCmdBuffer, new byte[] { 0x00 }, 2);
                systemControlCmdBuffer[3] = 0x0D;

                //以下注释程序为向串口写系统温度控制量
                this.serialPort.Write(systemControlCmdBuffer, 0, systemControlCmdBuffer.Length);
                System.Threading.Thread.Sleep(65);

				if (this.isWorking)
				{
					Command cmd = new Command(CommandCode._6, "0D");						//停止从机发送数据
					byte[] bytes = cmd.ToBytes();
					this.serialPort.Write(bytes, 0, bytes.Length);                   
                }

				if (this.serialPort.IsOpen)
				{
					this.serialPort.Close();
				}
			}
			catch
			{
			}

            System.Threading.Thread.Sleep(65);
            Application.Exit(); 
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			//丢弃串口缓冲中的所有数据
            try
            {
				this.serialPort.DiscardInBuffer();
				this.serialPort.DiscardOutBuffer();
                this.InitValues();

				//开始采集数据，仪器开始工作
				Command cmd = new Command(CommandCode._6, "0D");
				byte[] bytes = cmd.ToBytes();
				this.serialPort.Write(bytes, 0, bytes.Length);

				this.isWorking = !this.isWorking;

				if (this.isWorking)
				{
                    //串口出错事件处理
                    this.serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);

                    //串口数据接收事件
                    this.serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                    this.btnChangeTemperature.Enabled = false;
                    this.btnCustom.Enabled = false;
                    this.panCustom.Visible = false;

                    this.btnStart.Text = "停止从机";

					this.temperatureCurveCtrl.ResetFurnaceTemperatures();

                    this.btnStartGD.Enabled = true;
				}
				else
                {
                    this.lblStatus.Text = "";

                    //串口出错事件处理
                    this.serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);

                    //串口数据接收事件
                    this.serialPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);

                    this.btnChangeTemperature.Enabled=true;
                    this.btnCustom.Enabled = true;

					this.btnStart.Text = "启动从机";

                    //关闭炉温控制  
                    byte[] furnaceControlCmdBuffer = new byte[4];
                    BytesOperator.AppendBuffer(furnaceControlCmdBuffer, Encoding.Default.GetBytes("#1"), 0);
                    BytesOperator.AppendBuffer(furnaceControlCmdBuffer, new byte[] { 0x00 }, 2);
                    furnaceControlCmdBuffer[3] = 0x0D;
                    this.serialPort.Write(furnaceControlCmdBuffer, 0, furnaceControlCmdBuffer.Length);
                    System.Threading.Thread.Sleep(45);
                    //关闭系统温度控制
                    byte[] systemControlCmdBuffer = new byte[4];
                    BytesOperator.AppendBuffer(systemControlCmdBuffer, Encoding.Default.GetBytes("#2"), 0);
                    BytesOperator.AppendBuffer(systemControlCmdBuffer, new byte[] { 0x00 }, 2);
                    systemControlCmdBuffer[3] = 0x0D;

                    //以下注释程序为向串口写系统温度控制量
                    this.serialPort.Write(systemControlCmdBuffer, 0, systemControlCmdBuffer.Length);
                    System.Threading.Thread.Sleep(65);
				}
			}
			catch (InvalidOperationException)
			{
                MessageBox.Show("操作错误，可能是主机和从机连接断开。\r\n请检查USB口连接线。",
										   "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch
			{
			}
		}

		private void btnStartGD_Click(object sender, EventArgs e)
        {           
            try
            {                
                this.isVG_0 = true;
                this.timerClock.Enabled = true;
                this.timerClock.Interval = 1000;

                Command cmd = new Command(CommandCode._4, "0D");
                byte[] bytes = cmd.ToBytes();
                this.serialPort.Write(bytes, 0, bytes.Length);
                
                this.panelT.Visible = false ;
                this.panelG.Visible = true ;

                this.btnEndGD.Enabled =true;
                this.btnPrint.Enabled = false;
                this.btnSave.Enabled = false;
                this.btnDrop.Enabled = false;
                this.btnStartGD.Enabled = false;
                this.isDisplayTemperature =false;
                this.gasCurveCtrl.ResetGasDatas();
                this.timerDelay.Enabled =false;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("操作错误，可能是主机和从机连接断开。\r\n请检查USB口连接线。",
                                           "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }       
        
        }

        private void btnPrintResult_Click(object sender, EventArgs e)
		{
			//打印实验结果
			
		}

        private void btnEndGD_Click(object sender, EventArgs e)
        {
            this.EndGasTest();
        }

        private delegate void Del_EndGasTest();

        private void EndGasTest()
        {
            try
            {
                this.isVG_0 = false;
                this.isFirstToGD = true;
               
                Command cmd = new Command(CommandCode._5, "0D");
                byte[] bytes = cmd.ToBytes();
                this.serialPort.Write(bytes, 0, bytes.Length);

                this.btnEndGD.Enabled = false;
                this.btnDrop.Enabled = true;
                this.btnSave.Enabled = true;
                this.btnPrint.Enabled = true;
                groupBoxGas.Text = "最大发气量";
                this.lblGas.Text = this.maxGas.ToString();
                this.groupBox2.Text = "最大发气量时间 mm:ss";
                //this.lblTime.Text = (string.Format("{0:00}", this.maxGasTime * 2 / 60) + ":" + string.Format("{0:00}", this.maxGasTime * 2 % 60)).ToString();

                this.lblTime.Text = (string.Format("{0:00}", this.maxGasTime / 60) + ":" + string.Format("{0:00}", this.maxGasTime % 60)).ToString();
                this.saveMaxGasIncrement= this.maxGasIncrement.ToString("0.00");
                this.maxGas = 0;
                this.maxGasTime = 0;
                this.maxGasIncrement = 0;
                this.temperatureCurveCtrl.DisplayTemperatureCurve();

                this.isDisplayTemperature =true;
                this.btnStart.Enabled = true;
                iCount++;
                this.txtRepeat.Text = iCount.ToString();
                nCount = 0;
                this.timerClock.Enabled = false;
                
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("操作错误，可能是主机和从机连接断开。\r\n请检查USB口连接线。",
                                           "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
            }
        }
    
		private void btnChangeTemperature_Click(object sender, EventArgs e)
		{
            this.btnStart.Enabled =true;
            this.btnStartGD.Enabled =true;
            
            this.furnaceTemperatureTarget = int.Parse(this.lblTargetFurnaceTemprature.Text.Trim());
            this.furnaceTemperatureLast = this.furnaceTemperatureTarget;
            this.furnaceTemperatureLastLast = this.furnaceTemperatureTarget;

            this.lblTargetFurnaceTemprature.ForeColor = Color.LawnGreen;
		    ConfigClass config=new ConfigClass();
            config.SetConfigName("GET3G_PC.exe.config");
            config.SaveConfig("SettingValue",this.lblTargetFurnaceTemprature.Text);
        
        }
  


        //保存数据函数
        private void Save(List<GasData> gas,string filename)
        {                
                FileStream fileStream = new FileStream(filename, FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                string temp = string.Format("实验时间:{0}\r\n试样名称:{1}\r\n试样编号:{2}\r\n实验单位:{3}\r\n实验人员:{4}\r\n重复试验次数:{5}\r\n试样重量:{6}\r\n实验温度设定值:{7}\r\n最大发气量:{8}  最大发气时间:{9}\r\n",DateTime.Now, txtSampleName.Text, txtSampleNumber.Text,txtFactory.Text,txtPeople.Text,txtRepeat.Text, txtSampleWeight.Text,this.lblTargetFurnaceTemprature.Text,this.lblGas.Text,this.lblTime.Text);
                streamWriter.WriteLine(temp);
                string tempString = string.Format("时间\t\t发气量\t\t发气速度\r\n");
                streamWriter.Write(tempString);
                foreach (GasData gasArray in gasDatas)
                {                    
                        temp = string.Format("{0} s\t\t{1} ml/g\t{2:0.00} ml/g/s", gasArray.Time * 2, gasArray.GasDatas, gasArray.GasIncrementDatas);
                        streamWriter.WriteLine(temp);                    
                }

                streamWriter.Close();
                fileStream.Close(); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "试样参数及发气量(*.gas)|*.gas";

            GasDataToPrint gasDataToPrint = new GasDataToPrint();

            gasDataToPrint.GasTimeList = this.gasCurveCtrl.GasTimeList;
            gasDataToPrint.GasList = this.gasCurveCtrl.GasList;
            gasDataToPrint.GasInCrementList = this.gasCurveCtrl.GasInCrementList;
            gasDataToPrint.RectToPrint = this.gasCurveCtrl.ClientRectangle;
            gasDataToPrint.SampleName = this.txtSampleName.Text;
            gasDataToPrint.SampleNumber = this.txtSampleNumber.Text;
            gasDataToPrint.SampleWeight = this.txtSampleWeight.Text;
            gasDataToPrint.SampleRepeat = this.txtRepeat.Text;
            gasDataToPrint.Gas = this.lblGas.Text;
            gasDataToPrint.GasIncrement = this.saveMaxGasIncrement;
            //gasDataToPrint.TestTime = this.lblTime.Text;
            gasDataToPrint.TestTime = DateTime.Now.ToString();
            gasDataToPrint.FurnaceTargetTemperature = this.lblTargetFurnaceTemprature.Text;
            gasDataToPrint.Factory = this.txtFactory.Text;
            gasDataToPrint.People = this.txtPeople.Text;

            DateTime dtNow = DateTime.Now;

            saveDlg.FileName = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}-{6}",                                                             
                                                             dtNow.Year,
                                                             dtNow.Month,
                                                             dtNow.Day,
                                                             dtNow.Hour,
                                                             dtNow.Minute,
                                                             dtNow.Second,
                                                             gasDataToPrint.SampleNumber
                                                             );

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                string filename = saveDlg.FileName;
                                
                //将gasDataToPrint对象序列化
                FileStream stream = new FileStream(filename,FileMode.Create);
                XmlSerializer serializer = new XmlSerializer(typeof(GasDataToPrint));
                serializer.Serialize(stream, gasDataToPrint);
                stream.Close();

                //this.Save(this.gasDatas,filename);

                //清空数据
                this.gasDatas.Clear();
            }
            //界面切换
            this.groupBox2.Text = "实验时间 mm:ss";
            this.groupBoxGas.Text = "发气量";
            this.panelT.Visible = true;

            this.panelG.Visible = false;
            this.timerDelay.Enabled =true;
        }
      
        private string LoadDatas(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);

            string Datas = streamReader.ReadToEnd();
            
            streamReader.Close();
            fileStream.Close();
            return Datas;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "试样参数及发气量(*.txt)|*.txt";

            this.printDocument.DefaultPageSettings.Landscape = true;

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = this.printDocument;

            printPreviewDialog.Show(this);

            //界面切换
            this.groupBox2.Text = "实验时间 mm:ss";
            this.groupBoxGas.Text = "发气量";
            this.panelT.Visible = true;

            this.panelG.Visible = false;
            this.timerDelay.Enabled = true;
            DialogResult dalResult = MessageBox.Show("是否保存打印数据？", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dalResult != DialogResult.Yes) return;

            this.btnSave_Click(sender, e);
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //打印
            GasDataToPrint dataPrinter = new GasDataToPrint();
            //绘制曲线部分
            dataPrinter.GasTimeList = this.gasCurveCtrl.GasTimeList;
            dataPrinter.GasList = this.gasCurveCtrl.GasList;
            dataPrinter.GasInCrementList = this.gasCurveCtrl.GasInCrementList;
            dataPrinter.RectToPrint = this.gasCurveCtrl.ClientRectangle;
            //写文字部分
            dataPrinter.SampleName = this.txtSampleName.Text;
            dataPrinter.SampleNumber = this.txtSampleNumber.Text;
            dataPrinter.SampleWeight = this.txtSampleWeight.Text;
            dataPrinter.SampleRepeat = this.txtRepeat.Text;
            dataPrinter.Gas = this.lblGas.Text;
            dataPrinter.GasIncrement = this.saveMaxGasIncrement;
            dataPrinter.TestTime = this.lblTime.Text;
            dataPrinter.FurnaceTargetTemperature = this.lblTargetFurnaceTemprature.Text;
            dataPrinter.Factory = this.txtFactory.Text;
            dataPrinter.People = this.txtPeople.Text;
            dataPrinter.DrawGasDatas(e.Graphics);
           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {            
            this.lblTime.Text = (string.Format("{0:00}", nCount / 60) + ":" + string.Format("{0:00}", nCount % 60)).ToString();
            nCount++;
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            if (this.btnSetting.Text == "修改试样参数")
            {
                this.btnSetting.Text = "确  定";
                this.txtFactory.Enabled = true;
                this.txtPeople.Enabled = true;
                this.txtRepeat.Enabled = true;
                this.txtSampleName.Enabled = true;
                this.txtSampleNumber.Enabled = true;
                this.txtSampleWeight.Enabled = true;
            }
            else if (this.btnSetting.Text == "确  定")
            {
                if (this.txtFactory.Text.Length > 12)
                {
                    MessageBox.Show("公司名称应小于12个字", "提示");
                    return;
                }
                if (this.txtSampleNumber.Text.Length > 12)
                {
                    MessageBox.Show("试样编号应小于12个字", "提示");
                    return;
                }
                if(this.txtSampleName.Text.Length>12)
                {
                    MessageBox.Show("试样名称应小于12个字", "提示");
                    return;
                }
                this.btnSetting.Text = "修改试样参数";               
                this.txtFactory.Enabled = false;
                this.txtPeople.Enabled = false;
                this.txtRepeat.Enabled = false;
                this.txtSampleName.Enabled = false;
                this.txtSampleNumber.Enabled = false;
                this.txtSampleWeight.Enabled = false;
                ConfigClass config = new ConfigClass();
                config.SetConfigName("GET3G_PC.exe.config");
                config.SaveConfig("SampleName", this.txtSampleName.Text);
                config.SaveConfig("SampleNumber", this.txtSampleNumber.Text);
                config.SaveConfig("Factory", this.txtFactory.Text);
                config.SaveConfig("People", this.txtPeople.Text);
                config.SaveConfig("SampleWeight", this.txtSampleWeight.Text);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo Info = new ProcessStartInfo();
                string fileName = Application.ExecutablePath.Substring(0,Application.ExecutablePath.LastIndexOf(@"\")) + @"\GasDataReader.exe";
                Info.FileName =  fileName;
                Process Proc = Process.Start(Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动数据查看器程序出错：" + ex.Message, "错误提示",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
            }
            
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            this.gasDatas.Clear();
            this.groupBox2.Text = "实验时间 mm:ss";
            this.groupBoxGas.Text = "发气量";
            this.panelT.Visible = true;
            this.timerDelay.Enabled = true;
            this.panelG.Visible = false;
            iCount--;
            
        }

        private void timerDelay_Tick(object sender, EventArgs e)
        {
            this.btnStartGD.Enabled = true;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dalResult = MessageBox.Show("确定要退出程序吗？", "确认信息", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dalResult != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnCustom_Click(object sender, EventArgs e)
        {
            this.panCustom.Visible = true;
            this.txtCustom.Text = this.lblTargetFurnaceTemprature.Text;
        }

        private void btnCustom_OK_Click(object sender, EventArgs e)
        {
            try
            {
                int targetTemprature = int.Parse(this.txtCustom.Text);

                if (targetTemprature > 1050)
                {
                    MessageBox.Show("目标温度不能超过摄氏1050度", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.txtCustom.Focus();
                    return;
                }

                this.lblTargetFurnaceTemprature.Text = this.txtCustom.Text;
                this.lblTargetFurnaceTemprature.ForeColor = Color.Red;
                this.panCustom.Visible = false;
            }
            catch (FormatException)
            {
                MessageBox.Show("请输入正确的数字", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtCustom.Focus();
            }
        }

    }
}
