using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 位移传感器回应
    /// </summary>
    public class Displacement_Reply
    {
        public float Displacement { get; set; }                   //位移

        public byte[] Bytes { get; set; }                     //原始字节数据
        public string Bytes_String { get; set; }            //原始字节的16进制

        public override string ToString()
        {
            return $"位移值：{this.Displacement}，原始数据：{this.Bytes_String}";
        }
    }
}
