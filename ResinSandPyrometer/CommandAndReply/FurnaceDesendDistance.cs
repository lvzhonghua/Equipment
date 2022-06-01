using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    public enum FurnaceDesendDistance
    {
        _0,             //代码：0，下降距离：150 mm，脉冲计数：BB80
        _1,             //代码：1，下降距离：152 mm，脉冲计数：BE00
        _2,             //代码：2，下降距离：154 mm，脉冲计数：C080
        _3,             //代码：3，下降距离：156 mm，脉冲计数：C300
        _4,             //代码：4，下降距离：158 mm，脉冲计数：C580
        _5,             //代码：5，下降距离：160 mm，脉冲计数：C800
        _6,             //代码：6，下降距离：162 mm，脉冲计数：CA80
        _7,             //代码：7，下降距离：164 mm，脉冲计数：CD00
        _8,             //代码：8，下降距离：166 mm，脉冲计数：CF80
        _9              //代码：9，下降距离：168 mm，脉冲计数：D200
    }
}
