using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ResinSandPyrometer.Common
{
    /// <summary>
    /// 串口管理器
    /// </summary>
    public class SerialPortManager
    {
        public static SerialPort SerialPort_Slave;              //单片机串口
        public static SerialPort SerialPort_Temperature;    //温控仪串口
        public static SerialPort SerialPort_Displacement;   //温控仪串口

        public static void OpenSerial_Slave(string portName)
        {
            try
            {
                SerialPort_Slave = new SerialPort();
                SerialPort_Slave.PortName = portName;//串口端口
                SerialPort_Slave.BaudRate = 115200;//波特率
                SerialPort_Slave.Parity = (Parity)Enum.Parse(typeof(Parity), (string)"None");//奇偶校验
                SerialPort_Slave.DataBits = 8;//数据位
                SerialPort_Slave.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
                SerialPort_Slave.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
                SerialPort_Slave.ReadTimeout = 2000; //超时读取异常

                SerialPort_Slave.Open();
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerialPort_Slave方法 打开单片机串口 出现异常：{ex.Message}");
            }
            
        }

        public static void AddDataReceivedEventHandler_Slave(SerialDataReceivedEventHandler eventHandler)
        {
            SerialPort_Slave.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_Slave(SerialDataReceivedEventHandler eventHandler)
        {
            SerialPort_Slave.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_Slave(SerialErrorReceivedEventHandler eventHandler)
        {
            SerialPort_Slave.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_Slave(SerialErrorReceivedEventHandler eventHandler)
        {
            SerialPort_Slave.ErrorReceived -= eventHandler;
        }

        public static void OpenSerial_Temperature(string portName)
        {
            //由于温控仪与位移传感器共用一个串口，所以必须必须关闭一个，才能打开另一个
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

            try
            {
                SerialPort_Temperature = new SerialPort();
                SerialPort_Temperature.PortName = portName;                //COM口
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

        public static void OpenSerial_Displacement(string portName) 
        {
            //由于温控仪与位移传感器共用一个串口，所以必须必须关闭一个，才能打开另一个

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

            try
            {
                SerialPort_Displacement = new SerialPort();
                SerialPort_Displacement.PortName = portName; //串口
                SerialPort_Displacement.BaudRate = 9600;//波特率
                SerialPort_Displacement.Parity = Parity.None;//奇偶校验
                SerialPort_Displacement.DataBits = 8;//数据位
                SerialPort_Displacement.StopBits = (StopBits)Enum.Parse(typeof(StopBits), (string)"One");//停止位 
                SerialPort_Displacement.Handshake = (Handshake)Enum.Parse(typeof(Handshake), (string)"None");//握手协议即流控制方式
                SerialPort_Displacement.ReadTimeout = 2000;//超时读取异常

                SerialPort_Displacement.Open();

            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"OpenSerial_Displacement方法 打开位移串口串口 出现异常：{ex.Message}");
            }

        }

        public static void AddDataReceivedEventHandler_Temperature(SerialDataReceivedEventHandler eventHandler)
        {
            SerialPort_Temperature.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_Temperature(SerialDataReceivedEventHandler eventHandler)
        {
            SerialPort_Temperature.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_Temperature(SerialErrorReceivedEventHandler eventHandler)
        {
            SerialPort_Temperature.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_Temperature(SerialErrorReceivedEventHandler eventHandler)
        {
            SerialPort_Temperature.ErrorReceived -= eventHandler;
        }

        public static void AddDataReceivedEventHandler_Displacement(SerialDataReceivedEventHandler eventHandler)
        {
            SerialPort_Displacement.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_Displacement(SerialDataReceivedEventHandler eventHandler)
        {
            SerialPort_Displacement.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_Displacement(SerialErrorReceivedEventHandler eventHandler)
        {
            SerialPort_Displacement.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_Displacement(SerialErrorReceivedEventHandler eventHandler)
        {
            SerialPort_Displacement.ErrorReceived -= eventHandler;
        }

        public static void CloseSlave()
        {
            SerialPort_Slave.Close();
        }

        public static void CloseTemperature()
        {
            SerialPort_Temperature.Close();
        }

        public static void CloseDisplacement()
        {
            SerialPort_Displacement.Close();
        }

        public static bool Write_Slave(byte[] bytes)
        {
            bool success = false;
            try
            {
                SerialPort_Slave.Write(bytes,0, bytes.Length);
                success = true;
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"向单片机串口写数据 出现异常：{ex.Message}");
            }

            return success;
        }

        public static bool Write_Temperature(byte[] bytes)
        {
            bool success = false;
            try
            {
                SerialPort_Temperature.Write(bytes, 0, bytes.Length);
                success = true;
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"向温控仪串口写数据 出现异常：{ex.Message}");
            }

            return success;
        }

        public static bool Write_Displacement(byte[] bytes)
        {
            bool success = false;
            try
            {
                SerialPort_Displacement.Write(bytes, 0, bytes.Length);
                success = true;
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"向位移传感器串口写数据 出现异常：{ex.Message}");
            }

            return success;
        }
    }
}
