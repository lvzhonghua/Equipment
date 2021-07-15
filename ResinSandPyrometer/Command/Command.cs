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

        private CommandCode cmdCode = CommandCode.NONE;

        public CommandCode CmdCode
        {
            get { return cmdCode; }
            set { cmdCode = value; }
        }

        private long data = 0;
        private string dateString = "";

        public Command(byte[] buffer)
        {
            byte[] checkBuffer = new byte[3];
            byte[] checkBuffer1 = new byte[4];

            checkBuffer = BytesOperator.CutBuffer(buffer, 1, 3);
            checkBuffer1 = BytesOperator.CutBuffer(buffer, 0, 4);

            if (buffer.Length == 5)//主机发送指令长度
            {
                string comdTemp = Encoding.Default.GetString(BytesOperator.CutBuffer(buffer, 0, 1));

                if (comdTemp == "#")//返回值正确指令#
                {
                    comdTemp = "Sharp";
                }

                if (comdTemp == "Z")//#7指令
                {
                    comdTemp = "_7";
                }
                if (comdTemp == "?")//返回值错误指令
                {
                    comdTemp = "Error";
                }

                switch (comdTemp)//从机指令
                {
                    case "R":
                        this.cmdCode = CommandCode.R;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "C":
                        this.cmdCode = CommandCode.C;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "F":
                        this.cmdCode = CommandCode.F;
                        this.data = NumberSystem.BinaryToDecimal_Complement(checkBuffer, 16);
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "E":
                        this.cmdCode = CommandCode.E;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "O":
                        this.cmdCode = CommandCode.O;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "K":
                        this.cmdCode = CommandCode.K;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "L":
                        this.cmdCode = CommandCode.L;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "P":
                        this.cmdCode = CommandCode.P;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "G":
                        this.cmdCode = CommandCode.G;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;

                    case "Sharp":
                        this.cmdCode = CommandCode.Sharp;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "Error":
                        this.cmdCode = CommandCode.Error;
                        this.dateString = Encoding.Default.GetString(checkBuffer);
                        break;
                    case "_7":
                        this.cmdCode = CommandCode._7;
                        this.dateString = Encoding.Default.GetString(checkBuffer1);
                        break;


                    default:
                        break;
                }

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
            return this.dateString;
        }

    }
}
