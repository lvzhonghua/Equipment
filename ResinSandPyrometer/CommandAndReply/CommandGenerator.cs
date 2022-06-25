using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 指令产生器
    /// </summary>
    public class CommandGenerator
    {
        /// <summary>
        /// 产生查询炉温指令
        /// </summary>
        /// <returns></returns>
        public static Command Generate_GetFurnaceTemperature()
        {
            return CommandHandler.CreateCommand(CommandCode.GetFurnaceTemperature, null);
        }

        public static Command Generate_SetFurnaceTargetTemperature(int target)
        {
            return CommandHandler.CreateCommand(CommandCode.SetFurnaceTargetTemperature,target.ToString());
        }

        public static Command Generate_StartTemperatureControl()
        {
            return CommandHandler.CreateCommand(CommandCode.StartTemperatureControl, null);
        }

        public static Command Generate_StopTemperatureControl()
        {
            return CommandHandler.CreateCommand(CommandCode.StopTemperatureControl, null);
        }

        public static Command Generate_GetDisplacement()
        {
            return CommandHandler.CreateCommand(CommandCode.GetDisplacement, null);
        }

        public static Command Generate_GetForce()
        {
            return CommandHandler.CreateCommand(CommandCode.GetForce, null);
        }

        public static Command Generate_SetTestType(TestType testType)
        {
            string testType_String = "1";
            switch (testType)
            {
                case TestType.高温抗压强度测试:
                default:
                    testType_String = "1";
                    break;
                case TestType.热稳定性测试:
                    testType_String = "2";
                    break;
                case TestType.高温膨胀力:
                    testType_String = "4";
                    break;
            }

            return CommandHandler.CreateCommand(CommandCode.SetTestType, testType_String);
        }

        public static Command Generate_Reset()
        {
            return CommandHandler.CreateCommand(CommandCode.Reset, null);
        }

        public static Command Generate_BeginTest()
        {
            return CommandHandler.CreateCommand(CommandCode.BeginTest, null);
        }

        public static Command Generate_EndTest()
        {
            return CommandHandler.CreateCommand(CommandCode.EndTest, null);
        }

        public static Command Generate_EnableSendZeroData()
        {
            return CommandHandler.CreateCommand(CommandCode.EnableSendZeroData, null);
        }

        public static Command Generate_TemperatureReached()
        {
            return CommandHandler.CreateCommand(CommandCode.TemperatureReached, null);
        }

        public static Command Generate_CheckEquipment()
        {
            return CommandHandler.CreateCommand(CommandCode.CheckEquipment, null);
        }

        public static Command Generate_MotorTest(MotorTestType testType)
        {
            string testType_String = "0";
            switch (testType)
            {
                case MotorTestType.加载电机下降:
                default:
                    testType_String = "0";
                    break;
                case MotorTestType.加载电机上升:
                    testType_String = "1";
                    break;
                case MotorTestType.加热炉下降:
                    testType_String = "2";
                    break;
                case MotorTestType.加热炉上升:
                    testType_String = "3";
                    break;
            }

            //return CommandHandler.CreateCommand(CommandCode.CheckEquipment, testType_String);
            return CommandHandler.CreateCommand(CommandCode.MotorTest, testType_String);
        }

        public static Command Generate_MotorStep(MotorUpOrDown upOrDown) 
        {
            string data_String = "0";

            switch (upOrDown)
            {
                case MotorUpOrDown.下降:
                default:
                    data_String = "0";
                    break;
                case MotorUpOrDown.上升:
                    data_String = "1";
                    break;
            }

            return CommandHandler.CreateCommand(CommandCode.MotorStep, data_String);
        }

        public static Command Generate_MotorRunOrStop(MotorRunOrStop runOrStop)
        {
            string data_String = "0";

            switch (runOrStop)
            {
                case MotorRunOrStop.停止:
                default:
                    data_String = "0";
                    break;
                case MotorRunOrStop.运行:
                    data_String = "1";
                    break;
            }

            return CommandHandler.CreateCommand(CommandCode.MotorRunOrStop, data_String);
        }

        public static Command Generate_MotorToZero()
        {
            return CommandHandler.CreateCommand(CommandCode.MotorToZero, null);
        }

        public static Command Generate_MotorDistanceSetting(float distance = 10)
        {
            short distanceValue = (short)(2500 * distance);

            Command command = new Command();
            command.Code = CommandCode.MotorDistanceSetting;
            command.Code_String = "#<";
            command.Code_Bytes = Encoding.ASCII.GetBytes(command.Code_String);
            command.Data_String = distanceValue.ToString();
            byte[] bytes = BitConverter.GetBytes(distanceValue);
            command.Data = new byte[] {bytes[1], bytes[0]};

           command.Bytes = new byte[command.Code_Bytes.Length + command.Data.Length];
            Buffer.BlockCopy(command.Code_Bytes, 0, command.Bytes, 0, 2);
            Buffer.BlockCopy(command.Data, 0, command.Bytes, 2, 2);

            return command;
        }

        public static Command Generate_MotorSpeedSetting(MotorSpeed motorSpeed = MotorSpeed._4)
        {
            string data_String = "0";

            switch (motorSpeed)
            {
                case MotorSpeed._0:
                default:
                    data_String = "0";
                    break;
                case MotorSpeed._1:
                    data_String = "1";
                    break;
                case MotorSpeed._2:
                    data_String = "2";
                    break;
                case MotorSpeed._3:
                    data_String = "3";
                    break;
                case MotorSpeed._4:
                    data_String = "4";
                    break;
                case MotorSpeed._5:
                    data_String = "5";
                    break;
                case MotorSpeed._6:
                    data_String = "6";
                    break;
                case MotorSpeed._7:
                    data_String = "7";
                    break;
                case MotorSpeed._8:
                    data_String = "8";
                    break;
                case MotorSpeed._9:
                    data_String = "9";
                    break;
            }

            return CommandHandler.CreateCommand(CommandCode.MotorSpeedSetting, data_String);
        }

        public static Command Generate_FurnaceDesend(FurnaceDesendSpeed speed = FurnaceDesendSpeed._4, FurnaceDesendDistance distance = FurnaceDesendDistance._5)
        {
            string speed_String = speed.ToString().Remove(0,1);
            string distance_String =distance.ToString().Remove(0,1);
            string data_String = $"{speed_String}{distance_String}";

            return CommandHandler.CreateCommand(CommandCode.FurnaceDesend, data_String);
        }

        public static Command Generate_FurnaceToZero()
        {
            return CommandHandler.CreateCommand(CommandCode.FurnaceToZero, null);
        }

    }
}
