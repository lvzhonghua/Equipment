using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer
{
    public class Command
    {
        private CommandCode code = CommandCode.NONE;

        public CommandCode Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        private long data = 0;
        private string dataString = "";

        public Command(byte[] buffer)
        {
            byte[] bytesOfData = BytesOperator.CutBuffer(buffer, 1, 3);

            if (buffer.Length == 5)//主机发送指令长度
            {
                string commandType = Encoding.ASCII.GetString(BytesOperator.CutBuffer(buffer, 0, 1));

                if (commandType == "#")//返回值正确指令#
                {
                    commandType = "Sharp";
                }

                if (commandType == "Z")//#7指令
                {
                    commandType = "_7";
                }
                if (commandType == "?")//返回值错误指令
                {
                    commandType = "Error";
                }

                switch (commandType)//从机指令
                {
                    case "R":
                        this.code = CommandCode.R;
                        break;
                    case "C":
                        this.code = CommandCode.C;
                        break;
                    case "F":
                        this.code = CommandCode.F;
                        this.data = NumberSystem.BinaryToDecimal_Complement(bytesOfData, 16);
                        break;
                    case "E":
                        this.code = CommandCode.E;
                        break;
                    case "O":
                        this.code = CommandCode.O;
                        break;
                    case "K":
                        this.code = CommandCode.K;
                        break;
                    case "L":
                        this.code = CommandCode.L;
                        break;
                    case "P":
                        this.code = CommandCode.P;
                        break;
                    case "G":
                        this.code = CommandCode.G;
                        break;
                    case "Sharp":
                        this.code = CommandCode.Sharp;
                        break;
                    case "Error":
                        this.code = CommandCode.Error;
                        break;
                    case "_7":
                        this.code = CommandCode._7;
                        break;
                }
                this.dataString = BitConverter.ToString(bytesOfData);
                if(this.code == CommandCode._7) this.dataString = BitConverter.ToString(BytesOperator.CutBuffer(buffer, 0, 4));
            }
        }

        //返回F的数据
        public long GetData()
        {
            return this.data;
        }

        //返回字符串
        public string GetDataString()
        {
            return this.dataString;
        }

    }
}
