using System;
using System.Collections.Generic;

using System.Text;

namespace CoatingAnalysis
{
    public class CommandInterpreter
    {
        private CommandCode cmdCode = CommandCode.NONE;

        //数据位
        private long data = 0;

        //数据位转为字符串
        private string dateString = "";

        //指令构造函数
        public CommandInterpreter(byte[] buffer)
        {
            byte[] checkBuffer = new byte[3];
            checkBuffer = BytesOperator.CutBuffer(buffer, 1, 3);
            if (buffer.Length == 5)	//从从机返回的指令长度固定为5个字节
            {
                string codeTemp = Encoding.Default.GetString(BytesOperator.CutBuffer(buffer, 0, 1));
                if (codeTemp == "#")
                    codeTemp = "Sharp";
                if (codeTemp == "?")
                    codeTemp = "Error";
                switch (codeTemp)
                {
                    case "R":
                        this.cmdCode = CommandCode.R;
                        this.dateString = "R" + Encoding.Default.GetString(checkBuffer);
                        break;
                    case "D":	
                        this.cmdCode = CommandCode.D;
                        this.data = TransCoding.BinaryToDecimal_Complement(checkBuffer, 16);
                        this.dateString = "D" + this.data;
                        break;
                    case "H":
                        this.cmdCode = CommandCode.H;
                        this.data = TransCoding.BinaryToDecimal_Complement(checkBuffer, 16);
                        this.dateString = "H" + this.data;
                         break;
                    case "E":
                         this.cmdCode = CommandCode.E;
                         this.dateString = "E"+Encoding.Default.GetString(checkBuffer);
                         break;  
                    case "O":
                         this.cmdCode = CommandCode.O;
                         this.dateString = "O"+Encoding.Default.GetString(checkBuffer);
                         break;
                    case "Sharp":
                         this.cmdCode = CommandCode.Sharp;
                         this.dateString = "#"+Encoding.Default.GetString(checkBuffer);
                         break;
                    case "Error":
                         this.cmdCode = CommandCode.Error;
                         this.dateString = "?"+Encoding.Default.GetString(checkBuffer);
                         break;
                    default:
                         break;
                }

                if (buffer[4] != 0x0D  && this.cmdCode != CommandCode.R)
                {
                    this.cmdCode = CommandCode.NONE;
                }
            }
        }

        //返回CommandCode指令
        public CommandCode GetFirstCode()
        {
            return this.cmdCode;
        }

        //返回指令所承载的数字
        public long GetData()
        {
            return this.data;
        }

        //返回指令所承载的字符串
        public string GetDateString()
        {
            return this.dateString;
        }
    }
}
