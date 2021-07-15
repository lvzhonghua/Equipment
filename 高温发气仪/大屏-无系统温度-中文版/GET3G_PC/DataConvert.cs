using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace GET3G_PC
{
	public class DataConvert
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="rawValue"></param>
		/// <returns></returns>
		public static float ConvertToRoomTemperature(long rawValue)
		{
			return (float)(rawValue * 0.0078125);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rawValue"></param>
		/// <returns></returns>
        //public static float ConvertToSystemTemperature(long rawValue)
        //{
        //    AppSettingsReader reader = new AppSettingsReader();
        //    float referenceVoltage = (float)reader.GetValue("ReferenceVoltage", typeof(float));
        //    float sampleResistance = (float)reader.GetValue("SampleResistance", typeof(float));
        //    return (float)(rawValue * referenceVoltage / 16777215f / sampleResistance/2f-273.15);
        //}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rawValue"></param>
		/// <returns></returns>
		public static float ConvertToThermocouple(long rawValue)
		{
			AppSettingsReader reader = new AppSettingsReader();
			float referenceVoltage = (float)reader.GetValue("ReferenceVoltage", typeof(float));
			return (float)(rawValue * referenceVoltage / 16777215f / 32f);
		}

		public static float ConvertToThermocouple_Compress(long rawValue)
		{
			AppSettingsReader reader = new AppSettingsReader();
			float referenceVoltage = (float)reader.GetValue("ReferenceVoltage", typeof(float));
			return (float)(rawValue * referenceVoltage / 8388607f / 1000 / 32f);

		}
		
		public static float ConvertTemperatureToV(float temperature)
		{
			double b0 = -1.7600413686 * Math.Pow(10, 1);
			double b1 = 3.8921204975 * Math.Pow(10, 1);
			double b2 = 1.8558770032 * Math.Pow(10, -2);
			double b3 = -9.9457592874 * Math.Pow(10, -5);
			double b4 = 3.1840945719 * Math.Pow(10, -7);
			double b5 = -5.6072844889 * Math.Pow(10, -10);
			double b6 = 5.6075059059 * Math.Pow(10, -13);
			double b7 = -3.2020720003 * Math.Pow(10, -16);
			double b8 = 9.7151147152 * Math.Pow(10, -20);
			double b9 = -1.2104721275 * Math.Pow(10, -23);

			double c0 = 1.185976 * Math.Pow(10, 2);
			double c1 = -1.183432 * Math.Pow(10, -4);

			double[] cS = new double[10]{b0,b1,b2,b3,b4,b5,b6,b7,b8,b9};

			double V = 0;

			for (int i = 0; i < 10; i++)
			{
				V += cS[i] * Math.Pow(temperature, i);
			}

			V+= c0 * Math.Pow(Math.E, c1 * (temperature - 126.9686) * (temperature - 126.9686));
			return (float)V ;
		}

		public static float ConvertVToTemperature(float v)
		{
			double[] dS = null;

			//-200C~0C    -5891uV~0uV
			if (v >= -5891 && v < 0)
			{
				double d0 = 0;
				double d1 = 2.5173462 * Math.Pow(10, -2);
				double d2 = -1.1662878 * Math.Pow(10, -6);
				double d3 = -1.0833638 * Math.Pow(10, -9);
				double d4 = -8.9773540 * Math.Pow(10, -13);
				double d5 = -3.7342377 * Math.Pow(10, -16);
				double d6 = -8.6632643 * Math.Pow(10, -20);
				double d7 = -1.0450598 * Math.Pow(10, -23);
				double d8 = -5.1920577 * Math.Pow(10, -28);

				dS = new double[9] {d0,d1,d2,d3,d4,d5,d6,d7,d8 };
			}

			//0C~500C    0uV~20644uV
			if (v > 0 && v < 20644)
			{
				double d0 = 0;
				double d1 = 2.508355 * Math.Pow(10, -2);
				double d2 = 7.860106 * Math.Pow(10, -8);
				double d3 = -2.503131 * Math.Pow(10, -10);
				double d4 = 8.315270 * Math.Pow(10, -14);
				double d5 = -1.228034 * Math.Pow(10, -17);
				double d6 = 9.804036 * Math.Pow(10, -22);
				double d7 = -4.413030 * Math.Pow(10, -26);
				double d8 = 1.057734 * Math.Pow(10, -30);
				double d9 = -1.052755  * Math.Pow(10, -35);

				dS = new double[10] { d0,d1, d2, d3, d4, d5, d6, d7, d8,d9 };
			}

			//500C~1372C    20644uV~54886uV
			if (v >= 20644 && v < 54886)
			{
				double d0 = -1.318058 * Math.Pow(10, 2);
				double d1 = 4.830222 * Math.Pow(10, -2);
				double d2 = -1.646031 * Math.Pow(10, -6);
				double d3 = 5.464731 * Math.Pow(10, -11);
				double d4 = -9.650715  * Math.Pow(10, -16);
				double d5 = 8.802193 * Math.Pow(10, -21);
				double d6 = -3.110810 * Math.Pow(10, -26);

				dS = new double[7] {d0, d1, d2, d3, d4, d5, d6 };

			}

			if (dS == null)
			{
				return 0;
			}

			double t = 0;

			for (int i = 0; i < dS.Length; i++)
			{
				t += dS[i] * Math.Pow(v, i);
			}

			return (float)t;
		}

		public static float ConvertVToGas_850(float VG_i,float VG_0,float sampleWeight,long time)
		{
			AppSettingsReader reader = new AppSettingsReader();
			float k1 = (float)reader.GetValue("K1_850", typeof(float));
            float k2 = (float)reader.GetValue("K2_850", typeof(float));
			return (float)Math.Round((k1 * (VG_i - VG_0) / sampleWeight)-k2*time, 1);
		}
        
        public static float ConvertVToGas_1000(float VG_i, float VG_0, float sampleWeight, long time)
        {
            AppSettingsReader reader = new AppSettingsReader();
            float k1 = (float)reader.GetValue("K1_1000", typeof(float));
            float k2 = (float)reader.GetValue("K2_1000", typeof(float));
            return (float)Math.Round((k1 * (VG_i - VG_0) / sampleWeight) - k2 * time, 1);
        }

        public static float ConvertVToGas_700(float VG_i, float VG_0, float sampleWeight, long time)
        {
            AppSettingsReader reader = new AppSettingsReader();
            float k1 = (float)reader.GetValue("K1_700", typeof(float));
            float k2 = (float)reader.GetValue("K2_700", typeof(float));
            return (float)Math.Round((k1 * (VG_i - VG_0) / sampleWeight) - k2 * time, 1);
        }

        public static float ConvertVToGas_900(float VG_i, float VG_0, float sampleWeight, long time)
        {
            AppSettingsReader reader = new AppSettingsReader();
            float k1 = (float)reader.GetValue("K1_900", typeof(float));
            float k2 = (float)reader.GetValue("K2_900", typeof(float));
            return (float)Math.Round((k1 * (VG_i - VG_0) / sampleWeight) - k2 * time, 1);
        }
    }
}
