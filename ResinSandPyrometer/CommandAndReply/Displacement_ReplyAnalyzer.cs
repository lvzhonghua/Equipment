using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 位移传感器回应解析器
    /// </summary>
    public class Displacement_ReplyAnalyzer
    {
        public static Displacement_Reply Analyse(byte[] buffer)
        {
            Displacement_Reply reply = new Displacement_Reply();
            reply.Bytes = buffer;
            reply.Bytes_String = BitConverter.ToString(buffer);

            byte[] integerBytes = new byte[2];
            //低字节在前，高字节在后
            integerBytes[0] = buffer[4];
            integerBytes[1] = buffer[3];

            var h = (integerBytes[1] & 0xf0) >> 4; // 高位

            bool isSign = true;
            if (h == 8)
            {
                integerBytes[1] = Convert.ToByte(integerBytes[1] ^ 0x80);
                isSign = false;
            }

            int integerInt = BitConverter.ToInt16(integerBytes, 0);

            byte[] decimalsBytes = new byte[2];
            decimalsBytes[0] = buffer[6];
            decimalsBytes[1] = buffer[5];

            int decimalsInt = BitConverter.ToInt16(decimalsBytes, 0);

            string distanceStr = string.Format("{0}.{1}", integerInt, decimalsInt);

            float distance = float.Parse(distanceStr);

            string show = distance.ToString("0.00");

            float displacement = 0;

            if (isSign)
            {
                displacement = float.Parse(show);
            }
            else
            {
                displacement = 0 - float.Parse(show);
            }

            reply.Displacement = displacement;
            return reply;
        }
    }
}
