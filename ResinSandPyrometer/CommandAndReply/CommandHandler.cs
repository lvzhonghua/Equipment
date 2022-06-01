using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 指令处理器
    /// </summary>
    public class CommandHandler
    {
        public static Command CreateCommand(CommandCode commandCode, string data_String)
        {
            Command command = new Command();
            command.Code = commandCode;
            command.Data_String = data_String;
           
            switch (command.Code)
            {
                case CommandCode.GetForce:
                    command.Code_String = "#0";
                    break;
                case CommandCode.SetTestType:
                    command.Code_String = "#1";
                    break;
                case CommandCode.Reset:
                    command.Code_String = "#2";
                    break;
                case CommandCode.BeginTest:
                    command.Code_String = "#3";
                    break;
                case CommandCode.EndTest:
                    command.Code_String = "#4";
                    break;
                case CommandCode.EnableSendZeroData:
                    command.Code_String = "#5";
                    break;
                case CommandCode.TemperatureReached:
                    command.Code_String = "#6";
                    break;
                case CommandCode.CheckEquipment:
                    command.Code_String = "#7";
                    break;
                case CommandCode.MotorTest:
                    command.Code_String = "#8";
                    break;
                case CommandCode.MotorStep:
                    command.Code_String = "#9";
                    break;
                case CommandCode.MotorRunOrStop:
                    command.Code_String = "#:";
                    break;
                case CommandCode.MotorToZero:
                    command.Code_String = "#;";
                    break;
                case CommandCode.MotorDistanceSetting:
                    command.Code_String = "#<";
                    break;
                case CommandCode.MotorSpeedSetting:
                    command.Code_String = "#=";
                    break;
                case CommandCode.FurnaceDesend:
                    command.Code_String = "#>";
                    break;
                case CommandCode.FurnaceToZero:
                    command.Code_String = "#?";
                    break;
                case CommandCode.StartTemperatureControl:
                    command.Bytes = new byte[] { 0x81, 0x81, 0x43, 0x1B, 0x00, 0x00, 0x44, 0x1B };
                    break;
                case CommandCode.StopTemperatureControl:
                    command.Bytes = new byte[] { 0x81, 0x81, 0x43, 0x1B, 0x01, 0x00, 0x45, 0x1B };
                    break;
                case CommandCode.GetFurnaceTemperature: //温控仪查询指令
                    command.Bytes = new byte[] { 0x81, 0x81, 0x52, 0x00, 0x00, 0x00, 0x53, 0x00 }; 
                    break;
                case CommandCode.SetFurnaceTargetTemperature:   //设定炉温目标温度
                    int target = int.Parse(data_String);
                    command.Bytes = new byte[8];
                    command.Bytes[0] = 0x81;
                    command.Bytes[1] = 0x81;
                    command.Bytes[2] = 0x43;
                    command.Bytes[3] = 0x00;
                    command.Bytes[4] = (byte)(target * 10 % 256);//二进制低字节
                    command.Bytes[5] = (byte)(target * 10 / 256);//二进制高字节
                    command.Bytes[6] = (byte)((target * 10 + 68) % 256);
                    command.Bytes[7] = (byte)((target * 10 + 68) / 256);
                    break;
                case CommandCode.GetDisplacement:   //位移传感器查询位置
                    command.Bytes = new byte[] { 0x01, 0x04, 0x00, 0x04, 0x00, 0x02, 0x30, 0x0A };
                    break;
            }

            if (string.IsNullOrEmpty(command.Data_String) == false)
            {
                command.Data = Encoding.ASCII.GetBytes(command.Data_String);
            }

            if (string.IsNullOrEmpty(command.Code_String) == false)
            {
                command.Code_Bytes = Encoding.ASCII.GetBytes(command.Code_String);
            }

            if (command.Code == CommandCode.SetFurnaceTargetTemperature ||
                command.Code == CommandCode.StartTemperatureControl ||
                command.Code == CommandCode.StopTemperatureControl)  //温控仪相关指令
            {
                return command;
            }

            List<byte> byteList = new List<byte>();
            if (command.Code_Bytes != null) byteList.AddRange(command.Code_Bytes);
            if (command.Data != null) byteList.AddRange(command.Data);

            if (byteList.Count > 0) command.Bytes = byteList.ToArray();

            return command;
        }

    }
}
