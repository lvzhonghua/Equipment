using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.CommandAndReply
{
    public enum CommandCode
    {
        None,
        GetForce,                                           //指令0：查询力传感器当前值，#0
        SetTestType,                                       //指令1：设置测试方式，#1
        Reset,                                                //指令2：单片机复位，#2
        BeginTest,                                          //指令3：开始测试，#3
        EndTest,                                            //指令4：结束测试，#4
        EnableSendZeroData,                           //指令5：禁止/允许发送传感器零点数据，#5
        TemperatureReached,                           //指令6：炉温到达设定的温度，#6
        CheckEquipment,                                 //指令7：仪器指令查询，#7
        MotorTest,                                          //指令8：电机运行测试指令，#80, 0-加载电机下降，1-加载电机上升；2-加热炉下降，3-加热炉上升
        MotorStep,                                          //指令9：加载电机升/降一步（0.001mm）指令，#9X，X=0-下降；X=1-上升
        MotorRunOrStop,                                 //指令10：加载电机停止/运行指令，#:X，X=0停止；X=1运行
        MotorToZero,                                       //指令11：加载步进电机高速归零指令，#;
        MotorDistanceSetting,                           //指令12：加载步进电机空载行程设定值，共4个字节，#<HH
        MotorSpeedSetting,                              //指令13：设置加载步进电机上升速度指令，共3个字节，#=4
        FurnaceDesend,                                   //指令14：以指定的速度和距离控制炉子下降
        //FurnaceDistanceSetting,                     //指令14：加热炉速度、下降距离指令，共4个字节，#>45，4-设定速度，5-设定距离
        //FurnaceSpeedSetting,                         //指令14：加热炉速度、下降距离指令，共4个字节，#>45，4-设定速度，5-设定距离
        FurnaceToZero,                                     //指令15：加热炉归零指令，共2个字节，#?
        GetFurnaceTemperature,                        //查询炉温指令（接收方：温控仪）
        SetFurnaceTargetTemperature,                //设定工作炉温（接收放：温控仪）
        StartTemperatureControl,                        //启动温控仪（接收方：温控仪）
        StopTemperatureControl,                        //停止温控仪（接收方：温控仪）
        GetDisplacement,                                 //获取位移值（接收方：位移传感器）
    }
}