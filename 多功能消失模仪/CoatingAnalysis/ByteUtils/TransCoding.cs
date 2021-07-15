using System;
using System.Collections.Generic;

using System.Text;

namespace CoatingAnalysis
{
    public class TransCoding
    {
        //2进制转10进制补码
        public static long BinaryToDecimal_Complement(byte[] buffer, int fromBase)
        {
            long firstValue = Convert.ToInt64(BitConverter.ToString(buffer, 0, 1), 16);

            if (firstValue <= 127)			//正数 
            {
                return BinaryToDecimal(buffer, 16);
            }

            //负数
            long maxValue = (long)Math.Pow(2, buffer.Length * 8);

            return 0 - (maxValue - BinaryToDecimal(buffer, 16));
        }

        //2进制转10进制原码
        public static long BinaryToDecimal(byte[] buffer, int fromBase)
        {
            string value = BitConverter.ToString(buffer);
            value = value.ToUpper();							//转换为大写字符串
            value = value.Replace("-", string.Empty);	//去掉"-"间隔符

            long result = 0;
            result = Convert.ToInt64(value, fromBase);

            return result;
        }

        //温控仪设置温度转换为发送的指令
        public static byte[] TemperatureToByte8(int number)
        {
            byte[] buffer = new byte[8];
            buffer[0] = 0x81;
            buffer[1] = 0x81;
            buffer[2] = 0x43;
            buffer[3] = 0x00;
            buffer[4] = (byte)(number * 10 % 256);
            buffer[5] = (byte)(number * 10 / 256);
            buffer[6] = (byte)((number * 10 + 68) % 256);
            buffer[7] = (byte)((number * 10 + 68) / 256);
            return buffer;
        }
    }
}
