using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Common
{
    public enum FirstStep
    {
        NONE,
        加热炉按行程下降,
        取预置零点值,
        电机上升,
        电机下降,
        抗压试样保温时间计时并更新零点值,
        采集数据压力是否突变,
        测试结束

    }

    public enum SecondStep
    {
        NONE,
        开始调试并发送指令,
        检测预载荷是否为10主机发送指令加载电机停止,

        检测载荷是否为9到11并调节预载荷,
        加热炉下降,
        采集数据并绘制膨胀力和时间曲线及膨胀力是否突变,

        结束测试

    }

    public enum ThirdStep
    {
        NONE,
        开始测试并发送指令,
        读取传感器采样值并判断预载荷是否达到预定值,
        继续采样并保证在一定范围内,
        加热炉下降,
        采样数据并绘制压强时间曲线及判断压强值是否突变,
        突变并返回,
        测试结束
    }

    public enum FourthStep
    {
        NONE,
        取预置零点值,
        电机上升,
        电机下降,
        加热炉下降,
        等待加热炉下降,
        更新零点值,
        采集位移数据并绘制曲线以及判断是否突变,
        测试结束
    }

    public enum Steps
    {
        None = 0,
        FirstStep = 1,//高温抗压强度
        SecondStep = 2,//高温膨胀力
        ThirdStep = 3,//热稳定性
        FourthStep = 4 //位移传感器
    }

    public enum StateOfSetParameter
    {
        NONE,
        设置加载电机加载速度,
        设置完毕
    }

}
