using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer
{
    public enum CommandCode
    {
        NONE,
        R,
        C,
        F,
        E,
        O,
        K,
        L,
        P,
        G,
        _7,//#7仪器查询指令
        Sharp,
        Error,
    }
}