using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Common
{
    public class BytesOperator
    {
        //将制定的字节数组添加到数组buffer中去，startIndex为起点
        public static void AppendBuffer(byte[] buffer, byte[] bufferToAppend, int startIndex)
        {
            AppendBuffer(buffer, bufferToAppend, startIndex, 0, bufferToAppend.Length);
        }
        public static void AppendBuffer(byte[] buffer, byte[] bufferToAppend, int startIndex, int startIndexToAppend, int lengthToAppend)
        {
            for (int index = 0; index < lengthToAppend; index++)
            {
                buffer[startIndex + index] = bufferToAppend[startIndexToAppend + index];
            }
        }

        //取byte[]中一段的数组，startIndex为起点，length为长度
        public static byte[] CutBuffer(byte[] buffer, int startIndex, int length)
        {
            byte[] bytes = new byte[length];

            Buffer.BlockCopy(buffer, startIndex, bytes, 0,bytes.Length);

            return bytes;
        }

        //将浮点数保留四位
        //public static string GetFourValue(float value)
        //{
        //    string str = value.ToString();
        //    if (str.Length > 5)
        //    {
        //        str = str.Substring(0, 5);
        //    }
        //    return str;
        //}

        public static float GetOneVaule(float value)
        {
            int v = 0;

            v = Convert.ToInt32(value * 10);
            return v / 10.0f;
        }

        //将浮点数保留四位
        public static string GetThreeValue(float value)
        {
            string str1 = string.Format("{0:N3}", value);
            return str1;
        }

        /// <summary>
        /// 设置某一位的值
        /// </summary>
        /// <param name="data">原字节</param>
        /// <param name="index">要设置的位， 值从低到高为 1-8</param>
        /// <param name="flag">要设置的值 true / false</param>
        /// <returns>修改后的字节</returns>
        public static byte SetBit(byte data, int index, bool flag)
        {
            if (index > 8 || index < 1) throw new ArgumentOutOfRangeException();

            int v = index < 2 ? index : (2 << (index - 2));
            return flag ? (byte)(data | v) : (byte)(data & ~v);
        }

        /// <summary>
        /// 获取数据中某一位的值
        /// </summary>
        /// <param name="input">传入的数据类型,可换成其它数据类型,比如Int</param>
        /// <param name="index">要获取的第几位的序号,从0开始</param>
        /// <returns>返回值为-1表示获取值失败</returns>
        public static int GetBit(byte input, int index)
        {
            if (index > sizeof(byte))
            {
                return -1;
            }
            //左移到最高位
            int value = input << (sizeof(byte) - 1 - index);
            //右移到最低位
            value = value >> (sizeof(byte) - 1);
            return value;
        }
    }
}
