using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GET3G_PC
{
	public static class BytesOperator
	{		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="bufferToAppend"></param>
		/// <param name="startIndex"></param>
		public static void AppendBuffer(byte[] buffer, byte[] bufferToAppend, int startIndex)
		{
			AppendBuffer(buffer, bufferToAppend, startIndex, 0, bufferToAppend.Length);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="bufferToAppend"></param>
		/// <param name="startIndex"></param>
		/// <param name="startIndexToAppend"></param>
		/// <param name="lengthToAppend"></param>
		public static void AppendBuffer(byte[] buffer, byte[] bufferToAppend, int startIndex, int startIndexToAppend, int lengthToAppend)
		{
			for (int index = 0; index < lengthToAppend; index++)
			{
				buffer[startIndex + index] = bufferToAppend[startIndexToAppend + index];
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte[] CutBuffer(byte[] buffer, int startIndex, int length)
		{
			byte[] bytes = new byte[length];

			for (int index = 0; (index < length) && (startIndex + index < buffer.Length); index++)
			{
				bytes[index] = buffer[startIndex + index];
			}

			return bytes;
		}
	}
}
