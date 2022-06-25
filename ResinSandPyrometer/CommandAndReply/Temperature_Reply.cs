using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 温控仪回应
    /// </summary>
    public class Temperature_Reply
    {  
        public float Temperature { get; set; }              //当前炉温
        public float SettingTemperature { get; set; }     //目标炉温设定值

        public int Output { get; set; }                        //输出值

        public Temperature_Reply_Warning Warning { get; set; } = Temperature_Reply_Warning.正常;  //状态

        public byte[] Bytes { get; set; }                     //原始字节数据
        public string Bytes_String { get; set; }            //原始字节的16进制

        public override string ToString()
        {
            return $"当前温度：{this.Temperature}，目标温度：{this.SettingTemperature}，状态：{this.Warning} 输出值：{this.Output} 原始数据：{this.Bytes_String}";
        }
    }
}
