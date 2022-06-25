using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ResinSandPyrometer.Common;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 温控仪回应解析器
    /// </summary>
    public class Temperature_ReplyAnalyzer
    {
        public static Temperature_Reply Analyse(byte[] buffer)
        {
            Temperature_Reply reply = new Temperature_Reply();

            reply.Bytes = buffer;
            reply.Bytes_String = BitConverter.ToString(buffer);

            byte[] tempBuffer_1 = new byte[2];
            //低字节在前，高字节在后
            tempBuffer_1[0] = buffer[1];
            tempBuffer_1[1] = buffer[0];

            byte[] tempBuffer_2 = new byte[2];
            //低字节在前，高字节在后
            tempBuffer_2[0] = buffer[3];
            tempBuffer_2[1] = buffer[2];

            reply.Temperature = 0.1f * NumberSystem.BinaryToDecimal_Complement(tempBuffer_1, 16);
            reply.SettingTemperature = 0.1f * NumberSystem.BinaryToDecimal_Complement(tempBuffer_2, 16);
            reply.Output = (int)buffer[4];

            byte warningByte = buffer[5];
            int bit_0 = BytesOperator.GetBit(warningByte, 0);
            int bit_1 = BytesOperator.GetBit(warningByte, 1);
            int bit_2 = BytesOperator.GetBit(warningByte, 2);
            int bit_3 = BytesOperator.GetBit(warningByte, 3);
            int bit_4 = BytesOperator.GetBit(warningByte, 4);

            Temperature_Reply_Warning warning = Temperature_Reply_Warning.正常;
            if (bit_0 == 1) warning |= Temperature_Reply_Warning.上限报警;
            if (bit_1 == 1) warning |= Temperature_Reply_Warning.下限报警;
            if (bit_2 == 1) warning |= Temperature_Reply_Warning.正偏差报警;
            if (bit_3 == 1) warning |= Temperature_Reply_Warning.负偏差报警;
            if (bit_4 == 1) warning |= Temperature_Reply_Warning.输入超量程报警;

            reply.Warning = warning;
            
            return reply;
        }
    }
}
