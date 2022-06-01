using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    public enum MotorSpeed
    {
        _0,             //代码：0，加载速度：1 mm/min，定时时间：30 ms，定时常数：10BE，5000细分：A04C
        _1,             //代码：1，加载速度：2 mm/min，定时时间：15 ms，定时常数：885F，5000细分：D026
        _2,             //代码：2，加载速度：3 mm/min，定时时间：10 ms，定时常数：B03F，5000细分：E01A
        _3,             //代码：3，加载速度：4 mm/min，定时时间：7.5 ms，定时常数：C42F，5000细分：E813
        _4,             //代码：4，加载速度：5 mm/min，定时时间：6 ms，定时常数：E813，5000细分：ECDC
        _5,             //代码：5，加载速度：6 mm/min，定时时间：5 ms，定时常数：D81F，5000细分：F00D
        _6,             //代码：6，加载速度：7 mm/min，定时时间：4.29 ms，定时常数：DDD2，5000细分：F254
        _7,             //代码：7，加载速度：8 mm/min，定时时间：3.75 ms，定时常数：E217，5000细分：F40A
        _8,             //代码：8，加载速度：9 mm/min，定时时间：3.33 ms，定时常数：E56A，5000细分：F55E
        _9,             //代码：9，加载速度：10 mm/min，定时时间：3 ms，定时常数：E813，5000细分：F66E
    }
}
