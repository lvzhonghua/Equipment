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
        private static SerialPort serialPort_DanPianJi;
        private static SerialPort serialPort_WenKongYi;
        private static SerialPort serialPort_WeiYi;

        public static void SetSerialPort(SerialPort danPianJi, SerialPort wenKongYi, SerialPort weiYi)
        {
            serialPort_DanPianJi = danPianJi;
            serialPort_WenKongYi = wenKongYi;
            serialPort_WeiYi = weiYi;
        }

        public static void OpenDanPianJi()
        {
            serialPort_DanPianJi.Open();
        }

        public static void AddDataReceivedEventHandler_DanPianJi(SerialDataReceivedEventHandler eventHandler)
        {
            serialPort_DanPianJi.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_DanPianJi(SerialDataReceivedEventHandler eventHandler)
        {
            serialPort_DanPianJi.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_DanPianJi(SerialErrorReceivedEventHandler eventHandler)
        {
            serialPort_DanPianJi.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_DanPianJi(SerialErrorReceivedEventHandler eventHandler)
        {
            serialPort_DanPianJi.ErrorReceived -= eventHandler;
        }

        public static void OpenWenKongYi()
        {
            serialPort_WenKongYi.Open();
        }

        public static void AddDataReceivedEventHandler_WenKongYi(SerialDataReceivedEventHandler eventHandler)
        {
            serialPort_WenKongYi.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_WenKongYi(SerialDataReceivedEventHandler eventHandler)
        {
            serialPort_WenKongYi.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_WenKongYi(SerialErrorReceivedEventHandler eventHandler)
        {
            serialPort_WenKongYi.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_WenKongYi(SerialErrorReceivedEventHandler eventHandler)
        {
            serialPort_WenKongYi.ErrorReceived -= eventHandler;
        }

        public static void OpenWeiYi() 
        {
            serialPort_WeiYi.Open();
        }

        public static void AddDataReceivedEventHandler_WeiYi(SerialDataReceivedEventHandler eventHandler)
        {
            serialPort_WeiYi.DataReceived += eventHandler;
        }

        public static void RemoveDataReceivedEventHandler_WeiYi(SerialDataReceivedEventHandler eventHandler)
        {
            serialPort_WeiYi.DataReceived -= eventHandler;
        }

        public static void AddErrorReceivedEventHandler_WeiYi(SerialErrorReceivedEventHandler eventHandler)
        {
            serialPort_WeiYi.ErrorReceived += eventHandler;
        }

        public static void RemoveErrorReceivedEventHandler_WeiYi(SerialErrorReceivedEventHandler eventHandler)
        {
            serialPort_WeiYi.ErrorReceived -= eventHandler;
        }

        public static void CloseDanPianJi()
        {
            serialPort_DanPianJi.Close();
        }

        public static void CloseWenKongYi()
        {
            serialPort_WenKongYi.Close();
        }

        public static void CloseWeiYi()
        {
            serialPort_WeiYi.Close();
        }

        public static bool Write_DianPianJi(byte[] bytes)
        {
            bool success = false;
            try
            {
                serialPort_DanPianJi.Write(bytes,0, bytes.Length);
                success = true;
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"Write_DianPianJi 出现异常：{ex.Message}");
            }

            return success;
        }

        public static bool Write_WenKongYi(byte[] bytes)
        {
            bool success = false;
            try
            {
                serialPort_WenKongYi.Write(bytes, 0, bytes.Length);
                success = true;
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"Write_WenKongYi 出现异常：{ex.Message}");
            }

            return success;
        }

        public static bool Write_WeiYi(byte[] bytes)
        {
            bool success = false;
            try
            {
                serialPort_WeiYi.Write(bytes, 0, bytes.Length);
                success = true;
            }
            catch (Exception ex)
            {
                SampleLoggerOnTextFile.Log($"Write_WeiYi 出现异常：{ex.Message}");
            }

            return success;
        }
    }
}
