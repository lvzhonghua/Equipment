using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

using ResinSandPyrometer.Common;

namespace ResinSandPyrometer.Common
{
    /// <summary>
    /// 串口管理器
    /// </summary>
    public class SerialPortManager
    {
        /// <summary>
        /// 单片机串口名称
        /// </summary>
        public static string Slave_COM { get { return SettingReaderAndWriter.ReadAppSetting("Slave_COM"); }  }

        /// <summary>
        /// 温控仪串口名称
        /// </summary>
        public static string Temperature_COM { get { return SettingReaderAndWriter.ReadAppSetting("Temperature_COM"); } }

        /// <summary>
        /// 位移传感器串口名称
        /// </summary>
        public static string Displacement_COM { get { return SettingReaderAndWriter.ReadAppSetting("Displacement_COM"); } }

        public static SerialPort SerialPort_Slave;              //单片机串口
        public static SerialPort SerialPort_Temperature;    //温控仪串口
        public static SerialPort SerialPort_Displacement;   //温控仪串口

        public static void OpenSerial_Slave()
        {
            if (SerialPort_Slave != null && SerialPort_Slave.IsOpen) return;
            try
            {
                SerialPort_Slave = new SerialPort();
                SerialPort_Slave.PortName = Slave_COM;             //串口端口
                SerialPort_Slave.BaudRate = 115200;                  //波特率
                SerialPort_Slave.Parity = Parity.None;                  //奇偶校验
                SerialPort_Slave.DataBits = 8;                            //数据位
                SerialPort_Slave.StopBits = StopBits.One;            //停止位 
                SerialPort_Slave.Handshake = Handshake.None;    //握手协议即流控制方式
                SerialPort_Slave.ReadTimeout = 2000;                 //超时读取异常

                SerialPort_Slave.Open();
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerialPort_Slave方法 打开单片机串口 出现异常：{ex.Message}");
            }
            
        }

        public static void AddDataReceivedEventHandler_Slave(SerialDataReceivedEventHandler eventHandler)
        {
            if (SerialPort_Slave == null) return;
            SerialPort_Slave.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_Slave(SerialDataReceivedEventHandler eventHandler)
        {
            if (SerialPort_Slave == null) return;
            SerialPort_Slave.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_Slave(SerialErrorReceivedEventHandler eventHandler)
        {
            if (SerialPort_Slave == null) return;
            SerialPort_Slave.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_Slave(SerialErrorReceivedEventHandler eventHandler)
        {
            if (SerialPort_Slave == null) return;
            SerialPort_Slave.ErrorReceived -= eventHandler;
        }

        public static void OpenSerial_Temperature(ref bool isExists)
        {
            //由于温控仪与位移传感器共用一个串口，所以必须关闭一个，才能打开另一个

            isExists = false;

            try
            {
                if (SerialPort_Displacement != null && SerialPort_Displacement.IsOpen == true)
                {
                    SerialPort_Displacement.Close();
                }
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerial_Temperature方法 关闭位移传感器串口 出现异常：{ex.Message}");
            }

            if (SerialPortManager.SerialPort_Temperature != null && SerialPortManager.SerialPort_Temperature.IsOpen)
            {
                isExists = true;
                return;
            }

            try
            {
                SerialPort_Temperature = new SerialPort();
                SerialPort_Temperature.PortName = Temperature_COM;     //COM口
                SerialPort_Temperature.BaudRate = 19200;                     //波特率
                SerialPort_Temperature.Parity = Parity.None;                   //校验位
                SerialPort_Temperature.DataBits = 8;                             //数据位
                SerialPort_Temperature.StopBits = StopBits.Two;             //停止位

                SerialPort_Temperature.Open();
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerial_Temperature方法 打开温控仪串口 出现异常：{ex.Message}");
            }
        }

        public static void OpenSerial_Displacement(ref bool isExists) 
        {
            //由于温控仪与位移传感器共用一个串口，所以必须关闭一个，才能打开另一个

            isExists = false;

            try
            {
                if (SerialPort_Temperature != null && SerialPort_Temperature.IsOpen == true)
                {
                    SerialPort_Temperature.Close();
                }
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerial_Displacement方法 关闭温控仪串口 出现异常：{ex.Message}");
            }

            if (SerialPortManager.SerialPort_Displacement != null && SerialPortManager.SerialPort_Displacement.IsOpen)
            {
                isExists = true;
                return;
            }

            try
            {
                SerialPort_Displacement = new SerialPort();
                SerialPort_Displacement.PortName = Displacement_COM;     //串口
                SerialPort_Displacement.BaudRate = 9600;                         //波特率
                SerialPort_Displacement.Parity = Parity.None;                     //奇偶校验
                SerialPort_Displacement.DataBits = 8;                               //数据位
                SerialPort_Displacement.StopBits = StopBits.One;               //停止位 

                SerialPort_Displacement.Open();

            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerial_Displacement方法 打开位移串口串口 出现异常：{ex.Message}");
            }

        }

        public static void AddDataReceivedEventHandler_Temperature(SerialDataReceivedEventHandler eventHandler)
        {
            if (SerialPort_Temperature == null) return;
            SerialPort_Temperature.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_Temperature(SerialDataReceivedEventHandler eventHandler)
        {
            if (SerialPort_Temperature == null) return;
            SerialPort_Temperature.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_Temperature(SerialErrorReceivedEventHandler eventHandler)
        {
            if (SerialPort_Temperature == null) return;
            SerialPort_Temperature.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_Temperature(SerialErrorReceivedEventHandler eventHandler)
        {
            if (SerialPort_Temperature == null) return;
            SerialPort_Temperature.ErrorReceived -= eventHandler;
        }

        public static void AddDataReceivedEventHandler_Displacement(SerialDataReceivedEventHandler eventHandler)
        {
            if (SerialPort_Displacement == null) return;
            SerialPort_Displacement.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_Displacement(SerialDataReceivedEventHandler eventHandler)
        {
            if (SerialPort_Displacement == null) return;
            SerialPort_Displacement.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_Displacement(SerialErrorReceivedEventHandler eventHandler)
        {
            if (SerialPort_Displacement == null) return;
            SerialPort_Displacement.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_Displacement(SerialErrorReceivedEventHandler eventHandler)
        {
            if (SerialPort_Displacement == null) return;
            SerialPort_Displacement.ErrorReceived -= eventHandler;
        }

        public static void CloseSlave()
        {
            if (SerialPort_Slave == null) return;
            SerialPort_Slave.Close();
            SerialPort_Slave = null;
        }

        public static void CloseTemperature()
        {
            if (SerialPort_Temperature == null) return;
            SerialPort_Temperature.Close();
            SerialPort_Temperature = null;
        }

        public static void CloseDisplacement()
        {
            if (SerialPort_Displacement == null) return;
            SerialPort_Displacement.Close();
            SerialPort_Displacement = null;
        }
    }
}
