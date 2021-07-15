using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    //第三步测试包含的步骤的枚举
    public enum EnumOfThirdStep
    {
        NONE,
        打开阀2和3以及空压机,
        绘制压力时间曲线,
        关闭空压机,
        关闭所有阀,
        测试终点
    }
}
