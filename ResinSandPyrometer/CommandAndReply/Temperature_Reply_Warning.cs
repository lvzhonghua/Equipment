using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 温控仪报警状态值
    /// </summary>
    [Flags]
    public enum Temperature_Reply_Warning
    {
        正常 = 0,
        上限报警 = 1,
        下限报警 = 2,
        正偏差报警 = 4,
        负偏差报警 = 8,
        输入超量程报警 = 16
    }
}
