using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer
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
            for (int index = 0; (index < length) && (startIndex + index < buffer.Length); index++)
            {
                bytes[index] = buffer[startIndex + index];
            }
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

    }
}
