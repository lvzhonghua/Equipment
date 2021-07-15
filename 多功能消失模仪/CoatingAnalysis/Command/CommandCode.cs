using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public enum  CommandCode
    {
        NONE,  //无效指令
        R,         //单片机开始按钮指令
        D,         //10kPa传感器返回指令（传感器1）
        H,         //100kPa传感器返回指令（传感器2）
        E,          //开路错误
        O,         //通道连接正常
        Sharp, //接收#000（0D）命令
        Error   //接收错误信息
    }
}
