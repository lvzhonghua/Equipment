using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 下位机回应
    /// </summary>
    public class Slave_Reply
    {
        public Slave_ReplyCode Code { get; set; } = Slave_ReplyCode.None;
        public string Code_String { get; set; }
        public string Answer_Content { get; set; }
        public bool IsError { get; set; } = false;
        public byte Code_Byte { get; set; }
        public byte[] Data { get; set; }
        public string Data_String { get; set; }
        public double Data_Double { get; set; } = 0;
        public double Data_Long { get; set; } = 0;
        public byte[] Bytes { get; set; }
        public CommandCode RelatedCommandCode { get; set; } = CommandCode.None;
    }
}
