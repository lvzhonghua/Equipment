using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 指令执行器
    /// </summary>
    class CommandExecutor
    {
        public static void Send(SerialPort serialPort, Command command)
        {
            if (serialPort == null || serialPort.IsOpen == false) throw new Exception("串口未连接，无法发送指令");

            byte[] bytes = command.Bytes;
            if (bytes == null) throw new Exception("指令内容为空，无法发送指令");

            serialPort.Write(bytes,0, bytes.Length);
        }
    }
}
