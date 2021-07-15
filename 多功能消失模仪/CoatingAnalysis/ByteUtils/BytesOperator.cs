using System;
using System.Collections.Generic;

using System.Text;

namespace CoatingAnalysis
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
            byte[] bytes = new byte[length];//数组长度依变量而定

            for (int index = 0; (index < length) && (startIndex + index < buffer.Length); index++)//判断条件是：小于byte数组长度以及buffer的总长
            {
                bytes[index] = buffer[startIndex + index];
            }

            return bytes;
        }

        //将所得浮点数转换为四个字符串
        public static string GetFourValue(float value)
        {
            string str = value.ToString();
            if (str.Length > 4)
                str = str.Substring(0, 4);
            return str;
        }

        //将所得浮点数转换为四个字符串
        public static string GetFourValue(double value)
        {
            string str = value.ToString();
            if (str.Length > 4)
                str = str.Substring(0, 4);
            return str;
        }
    }
}
