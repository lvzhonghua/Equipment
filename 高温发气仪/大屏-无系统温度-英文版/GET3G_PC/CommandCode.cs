using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GET3G_PC
{
	/// <summary>
	/// 
	/// </summary>
	public enum CommandCode
	{
		NONE,		//空指令

		#region 主机指令/从机相应指令代码
		_0,			//#0，仪器在线查询
		_1,			//#1，发送加热炉控制参数
		_2,			//#2，发送系统控制参数
		_3,			//#3，请求重新发送上一个压力采集数据
		_4,			//#4，开始实验
		_5,			//#5，结束实验
		_6,			//#6，开始传输数据
		_E,			//
		#endregion

		#region 从机指令
		TF,			//炉温采集数据
		TS,			//系统温度采集数据
        START,		//开始发气量实验
		END,			//结束发气量实验
		GD,			//发气量采集数据
		E0,			//热电偶通道开路错误
		E1,			//压力传感器通道开路错误
		E2,			//系统温度传感器通道开路错误
		O0,			//热电偶通道正常
		O1,			//压力传感器通道正常
		O2,			//系统温度传感器通道正常
		#endregion

        #region 初始温度数据
        InitData
        #endregion

    }
}
