using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    public class Command
    {
        public CommandCode Code { get; set; } = CommandCode.None;

        public string Code_String { get; set; }

        public byte[] Code_Bytes { get; set; }

        public string Data_String { get; set; }

        public byte[] Data { get; set; }

        public byte[] Bytes { get; set; }

    }
}
