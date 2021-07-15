using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GET3G_PC
{
	public class Command
	{
		private CommandCode code = CommandCode.NONE;

		public CommandCode Code
		{
			get { return this.code; }
			set { this.code = value; }
		}

		private string data = string.Empty;

		public string Data
		{
			get { return this.data; }
			set { this.data = value; }
		}

		private byte[] dataBuffer = null;

		public byte[] DataBuffer
		{
			get { return this.dataBuffer; }
			set { this.dataBuffer = value; }
		}
		
		public Command() { }

		public Command(CommandCode code, string data)
		{
			this.code = code;
			this.data = data;
		}

		public Command(CommandCode code, string data, byte[] dataBuffer) :
			this(code,data)
		{
			this.dataBuffer = dataBuffer;
		}

		public Command(byte[] buffer)
		{
			if (buffer.Length != 8)	//从从机返回的指令长度固定为8个字节
			{
				throw new Exception("指令字节个数不足8位");
			}

			string codeTemp = Encoding.Default.GetString(BytesOperator.CutBuffer(buffer,0,2));
			
			switch(codeTemp)
			{
				case "TF":	//炉温数据
					this.code = CommandCode.TF;
					this.data = BitConverter.ToString(BytesOperator.CutBuffer(buffer,2,5));
					this.dataBuffer = BytesOperator.CutBuffer(buffer, 2, 5);
					break;
                //case "TS":	//系统温度数据
                //    this.code = CommandCode.TS;
                //    this.data = BitConverter.ToString(BytesOperator.CutBuffer(buffer, 2, 3));
                //    this.dataBuffer = BytesOperator.CutBuffer(buffer, 2, 3);
                //    break;
				case "ST":	//START 开始发气量采集
					this.code = CommandCode.START;
					break;
				case "EN":	//END 结束发气量采集
					this.code = CommandCode.END;
					break;
				case "GD":	//发气量采集数据
					this.code = CommandCode.GD;
					this.data = BitConverter.ToString(BytesOperator.CutBuffer(buffer, 2, 5));
					this.dataBuffer = BytesOperator.CutBuffer(buffer, 2, 5);
					break;
				case "E0":	//E0：热电偶通道开路错误	E1：压力传感器通道开路错误	E2：系统温度传感器通道开路错误
					this.code = CommandCode.E0;
					break;
				case "E1":
					this.code = CommandCode.E1;
					break;
				case "E2":
					this.code = CommandCode.E2;
					break;
				case "O0":	//OK0：热电偶通道正常		OK1：压力传感器通道正常	OK2：系统温度传感器通道正常	
					this.code = CommandCode.O0;
					break;
				case "O1":
					this.code = CommandCode.O1;
					break;
				case "O2":
					this.code = CommandCode.O2;
					break;
				case "#0":	//仪器在线查询指令返回结果
					this.code = CommandCode._0;
					break;
				case "#1":	//发送加热炉控制指令返回结果
					this.code = CommandCode._1;
					this.data = BitConverter.ToString(BytesOperator.CutBuffer(buffer, 2, 2));
					this.dataBuffer = BytesOperator.CutBuffer(buffer, 2, 2);
					break;
                //case "#2":	//发送系统控制指令返回结果
                //    this.code = CommandCode._2;
                //    break;
				case "#3":	//请求重新发送上一个压力采样数据
					this.code = CommandCode._3;
					break;
				case "#4":	//开始实验指令返回结果
					this.code = CommandCode._4;
					break;
				case "#5":	//结束实验指令返回结果
					this.code = CommandCode._5;
					break;
				case "#6":	//开始发送数据指令返回结果
					this.code = CommandCode._6;
					break;
				case "#?":	//指令出错
					this.code = CommandCode._E;
					break;
                default:     //未开机前初始温度数据
                    this.code = CommandCode.InitData;
                    this.dataBuffer = buffer;
                    break;
			}
		}

		public override string ToString()
		{
			return Enum.ToObject(typeof(CommandCode),this.code).ToString() + this.data;
		}

		public byte[] ToBytes()
		{
			string codeName = this.code.ToString();
			codeName = codeName.Replace('_', '#');

			byte[] codeBytes = Encoding.Default.GetBytes(codeName);
			byte[] dataBytes = NumberSystem.HexStringToBytes(this.data);

			List<byte> bytes = new List<byte>();
			bytes.AddRange(codeBytes);
			bytes.AddRange(dataBytes);

			return bytes.ToArray();
		}
	}
}
