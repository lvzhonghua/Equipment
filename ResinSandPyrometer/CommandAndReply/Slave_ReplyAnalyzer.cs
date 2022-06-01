using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ResinSandPyrometer.Common;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 下位机回应解析器
    /// </summary>
    public class Slave_ReplyAnalyzer
    {
        private static CommandCode GetRelatedCommandCode(byte[] buffer)
        {
            CommandCode commandCode = CommandCode.None;

            byte firstByte = buffer[0];
            byte secondByte = buffer[1];
            char firstChar = (char)((int)firstByte);
            char secondChar = (char)((int)secondByte);

            switch (firstChar)
            {
                case 'F':
                    commandCode = CommandCode.GetForce;
                    break;
                case 'P':
                    commandCode = CommandCode.BeginTest;
                    break;
                case 'K':
                case 'L':
                    commandCode = CommandCode.EndTest;
                    break;
                case 'G':
                    ////////////////commandCode = CommandCode.FurnaceSpeedSetting;
                    break;
                case 'Z':
                    commandCode = CommandCode.CheckEquipment;
                    break;
            }

            switch (secondChar)
            {
                case '0':
                    if (firstChar == '?') commandCode = CommandCode.GetForce;
                    break;
                case '1':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.SetTestType;
                    break;
                case '2':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.Reset;
                    break;
                case '3':
                    if(firstChar == '?') commandCode = CommandCode.BeginTest;
                    break;
                case '4':
                    if (firstChar == '?') commandCode = CommandCode.EndTest;
                    break;
                case '5':
                    if(firstChar == '?') commandCode = CommandCode.EnableSendZeroData;
                    break;
                case '6':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.TemperatureReached;
                    break;
                case '7':
                    if (firstChar == '?') commandCode = CommandCode.CheckEquipment;
                    break;
                case '8':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.MotorTest;
                    break;
                case '9':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.MotorStep;
                    break;
                case ':':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.MotorRunOrStop;
                    break;
                case ';':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.MotorToZero;
                    break;
                case '<':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.MotorDistanceSetting;
                    break;
                case '=':
                    if (firstChar == '?' || firstChar == '#') commandCode = CommandCode.MotorSpeedSetting;
                    break;
                case '>':
                    ////////////if (firstChar == '?') commandCode = CommandCode.FurnaceDistanceSetting;
                    break;
                case '?':
                    if(firstChar == '?')commandCode = CommandCode.FurnaceToZero;
                    break;
            }

            return commandCode;
        }

        private static void SetAnswer_Content(ref Slave_Reply reply)
        {
            switch (reply.RelatedCommandCode)
            {
                case CommandCode.GetForce:
                    reply.Answer_Content = reply.IsError ? "力传感器取值出现异常" : $"力传感器值：{reply.Data_Double} 牛";
                    break;
                case CommandCode.SetTestType:
                    reply.Answer_Content = reply.IsError ? "设置测试方法出现异常" : "设置测试方式成功";
                    break;
                case CommandCode.Reset:
                    reply.Answer_Content = reply.IsError ? "单片机复位出现异常" : "单片机复位成功";
                    break;
                case CommandCode.BeginTest:
                    reply.Answer_Content = reply.IsError ? "开始测试出现异常" : "开始测试";
                    break;
                case CommandCode.EndTest:
                    reply.Answer_Content = reply.IsError ? "结束测试出现异常" : "结束测试";
                    break;
                case CommandCode.EnableSendZeroData:
                    reply.Answer_Content = reply.IsError ? "禁止/允许发送传感器零点数据出现异常" : "禁止/允许发送传感器零点数据成功";
                    break;
                case CommandCode.TemperatureReached:
                    reply.Answer_Content = reply.IsError ? "炉温到达设定温度指令接收出现异常" : "炉温到达设定温度指令接收成功";
                    break;
                case CommandCode.CheckEquipment:
                    reply.Answer_Content = reply.IsError ? "仪器查询失败" : "仪器查询成功";
                    break;
                case CommandCode.MotorTest:
                    reply.Answer_Content = reply.IsError ? "电机运行测试指令异常" : "电机运行测试指令成功";
                    break;
                case CommandCode.MotorStep:
                    reply.Answer_Content = reply.IsError ? "加载步进电机升/降一步出现异常" : "加载步进电机升/降一步执行成功";
                    break;
                case CommandCode.MotorRunOrStop:
                    reply.Answer_Content = reply.IsError ? "加载步进电机停止/运行出现异常" : "加载步进电机停止/运行执行成功";
                    break;
                case CommandCode.MotorToZero:
                    reply.Answer_Content = reply.IsError ? "加载步进电机高速归零出现异常" : "加载步进电机高速归零执行成功";
                    break;
                case CommandCode.MotorDistanceSetting:
                    reply.Answer_Content = reply.IsError ? "加载步进电机空载行程设定出现异常" : "加载步进电机空载行程设定执行成功";
                    break;
                case CommandCode.MotorSpeedSetting:
                    reply.Answer_Content = reply.IsError ? "设置加载步进电机上升速度出现异常" : "设置加载步进电机上升速度执行成功";
                    break;
                ////////////case CommandCode.FurnaceDistanceSetting:
                ////////////    reply.Answer_Content = reply.IsError ? "加热炉按设定速度和距离下降出现异常" : "加热炉按设定速度和距离下降执行成功";
                ////////////    break;
                ////////////case CommandCode.FurnaceSpeedSetting:
                ////////////    reply.Answer_Content = reply.IsError ? "加热炉按设定速度和距离下降出现异常" : "加热炉按设定速度和距离下降执行成功";
                ////////////    break;
                case CommandCode.FurnaceToZero:
                    reply.Answer_Content = reply.IsError ? "加热炉归零指令出现异常" : "加热炉归零指令执行成功";
                    break;
            }
        }

        public static Slave_Reply Analyse(byte[] buffer)
        {
            Slave_Reply reply = new Slave_Reply();
            reply.Bytes = buffer;

            byte firstByte = buffer[0];
            byte secondByte = buffer[1];
            char firstChar = (char)((int)firstByte);
            char secondChar = (char)((int)secondByte);

            reply.RelatedCommandCode = GetRelatedCommandCode(buffer);

            switch (firstChar)
            {
                case '#':
                    reply.Code = Slave_ReplyCode.Answer;
                    break;
                case '?':
                    reply.Code = Slave_ReplyCode.Answer;
                    reply.IsError = true;
                    break;
                case 'P':
                    reply.Code = Slave_ReplyCode.P;
                    reply.Answer_Content = "加载电机到达预设行程";
                    break;
                case 'K':
                    reply.Code = Slave_ReplyCode.K;
                    reply.Answer_Content = (secondChar == '0') ? "加热炉到上限位（零位）" : "加热炉到下限位（出错）";
                    reply.IsError = (secondChar == '0') ? false : true;
                    break;
                case 'L':
                    reply.Code = Slave_ReplyCode.L;
                    reply.Answer_Content = (secondChar == '0') ? "加载电机到下限位（零位）" : "加载电机到上限位（出错）";
                    reply.IsError = (secondChar == '0') ? false : true;
                    break;
                case 'Z':
                    reply.Code = Slave_ReplyCode.Answer;
                    break;
                case 'R':
                    reply.Code = Slave_ReplyCode.R;
                    reply.Answer_Content = "测试开始";
                    break;
                case 'C':
                    reply.Code = Slave_ReplyCode.C;
                    reply.Answer_Content = "停止测试";
                    break;
                case 'F':
                    reply.Code = Slave_ReplyCode.F;
                    reply.Data = new byte[3];
                    Buffer.BlockCopy(buffer, 1, reply.Data, 0, 3);
                    reply.Data_Long = NumberSystem.BinaryToDecimal_Complement(reply.Data);
                    reply.Data_Double = Utilities.GetForceFromVoltage(reply.Data);
                    break;
                case 'E':
                    reply.Code = Slave_ReplyCode.E;
                    reply.Answer_Content = "力传感器通道错误";
                    break;
                case 'O':
                    reply.Code = Slave_ReplyCode.O;
                    reply.Answer_Content = "力传感器通道正常";
                    break;
                case 'G':
                    reply.Code = Slave_ReplyCode.G;
                    reply.Answer_Content = "加热炉到达预设行程";
                    break;
            }

            SetAnswer_Content(ref reply);

            return reply;
        }
    }
}
