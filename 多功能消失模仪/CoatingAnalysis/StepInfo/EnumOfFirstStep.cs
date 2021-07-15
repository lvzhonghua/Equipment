using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    //第一步测试包含的步骤的枚举
    public enum EnumOfFirstStep
    {
        NONE,
        开始测试并打开阀1,
        绘制压力时间曲线,
        关闭所有阀,
        转换至高温透气性,
        单步结束测试
    }
}
