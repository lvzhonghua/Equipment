using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GET3G_PC
{
	public class NumberSystem
	{
		public static float BinaryToFloat(byte[] buffer)
		{
			//四个字节的最高为为阶码，后面三个字节为尾数,数符在尾数的最高位

			//第一位为“移码”的阶码，减去80H得到阶码
			long firstByteValue = BinaryToDecimal(BytesOperator.CutBuffer(buffer, 0, 1), 16);

			if (firstByteValue == 0) return 0;	//如果阶码为零，则整个数为零

			firstByteValue = firstByteValue - 128;

			double stepValue = Math.Pow(2, firstByteValue);

			long mantissa = BinaryToDecimal(BytesOperator.CutBuffer(buffer, 1, 3), 16);
			string mantissaString = DecimalToBinary(mantissa, 2);

			int length = mantissaString.Length;

			for (int index = 0; index < 24 - length; index++)		//不足24位，在前面补零
			{
				mantissaString = "0" + mantissaString;
			}

			float mantissaValue = 0;

			for (int index = 0; index < mantissaString.Length; index++)
			{
				//数符，0表示正数，1表示复数
				char c = mantissaString[index];
				if (index == 0)
				{
					if (c == '1')
					{
						stepValue = stepValue * -1;
					}
					mantissaValue += (float)Math.Pow(2, -(index + 1));

					continue;
				}

				if (c == '1')
				{
					mantissaValue += (float)Math.Pow(2, -(index + 1));
				}
			}

			return (float)(stepValue * mantissaValue);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="fromBase"></param>
		/// <returns></returns>
		public static long BinaryToDecimal_Complement(byte[] buffer, int fromBase)
		{
			long firstValue = Convert.ToInt64(BitConverter.ToString(buffer, 0, 1), 16);

			if (firstValue <= 127)			//正数
			{
				return BinaryToDecimal(buffer, 16);
			}

			//负数
			long maxValue = (long)Math.Pow(2, buffer.Length * 8);

			return 0 - (maxValue - BinaryToDecimal(buffer, 16));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="fromBase"></param>
		/// <returns></returns>
		public static long BinaryToDecimal(byte[] buffer, int fromBase)
		{
			string value = BitConverter.ToString(buffer);
			value = value.ToUpper();							//转换为大写字符串
			value = value.Replace("-", string.Empty);	//去掉"-"间隔符

			long result = 0;
			result = Convert.ToInt64(value, fromBase);

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="number"></param>
		/// <param name="bin"></param>
		/// <returns></returns>
		public static string DecimalToBinary(long number, int bin)
		{
			if (bin < 2 || bin > 16)
			{
				throw new ArgumentOutOfRangeException("bin", "只支持将十进制数据转换为二进制至16进制！");
			}
			Stack<char> stack = new Stack<char>();
			do
			{
				long residue = number % bin;//取余；   
				char c = (residue < 10) ? (char)(residue + 48) : (char)(residue + 55);
				stack.Push(c); //进栈；   
			}
			while ((number = number / bin) != 0);

			string result = string.Empty;
			while (stack.Count > 0)
			{
				result += stack.Pop().ToString();
			}
			return result;
		}

		public static byte[] HexStringToBytes(string hexString)
		{
			List<byte> bytes = new List<byte>();

			for (int index = 0; index < hexString.Length - 1; index += 2)
			{
				string hex = hexString.Substring(index, 2);

				bytes.Add(Convert.ToByte(hex, 16));
			}

			return bytes.ToArray();
		}
	}
}
