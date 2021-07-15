using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoatingAnalysis
{
    public enum EnumOfCmd
    {
        NONE,
        第一次关闭所有阀,
        打开阀一,
        第二次关闭所有阀,
        检查气压,
        激活开始按钮,
        开始测试,
        关闭所有阀
    }
}
