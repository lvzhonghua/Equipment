using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    /// <summary>
    /// 下位机发给上位机的回应代码
    /// 回应的数据结构：等字长，5个字节，其结构：指令(一个字节)数据(三个字节)结束符(一个字节，固定为0D)
    /// </summary>
    public enum Slave_ReplyCode
    {
        None,
        Answer, //收到上位机后的应答
        R,  //开始测试的回应，无数据，其内容为：R000D
        C,  //结束测试的回应，无数据，其内容为：C000D
        F,  //力传感器采集的数据，内容示例：F223D：三字节二进制补码，高位在前
        E,  //力传感器出错代码，内容示例：E001D：开路错误
        O,  //力传感器正常，内容示例：O001D：通道正常
        K,  //加热炉位置指示，内容示例：K000D：加热炉到上位限（零位）；K100D：加热炉到下限位（出错）
        L,  //加载步进电机位置指示，内容示例：L000D：加载步进电机到下位限；L100D：加载电机到上限位（出错）
        P,  //加载步进电机到达预设行程
        G,   //加热炉到达预设行程
    }
}
