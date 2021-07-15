using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    //第二步测试包含的步骤的枚举
    public enum EnumOfSecondStep
    {
        NONE,
        打开阀3及压缩机,
        第一次关闭所有阀,
        延时2s并记录Pb,
        打开阀2,
        绘制压力时间曲线,
        第二次关闭所有阀,
        高温强度测试,
        单步结束测试
    }
}
