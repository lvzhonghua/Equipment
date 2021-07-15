using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace GET3G_PC
{
	public class PIDController
	{
		private class PIDParameters
		{
			private float p;

			public float P
			{
				get { return this.p; }
				set { p = value; }
			}

			private float i;

			public float I
			{
				get { return this.i; }
				set { this.i = value; }
			}

			private float d;

			public float D
			{
				get { return this.d; }
				set { this.d = value; }
			}

			private float t = 1;

			public float T
			{
				get { return this.t; }
				set { this.t = value; }
			}

			private float beta;

			public float Beta
			{
				get { return this.beta; }
				set { this.beta = value; }
			}
			


			public PIDParameters() { }

			public PIDParameters(float u0, float p, float i, float d, float t,float beta)
			{
				this.p = p;
				this.i = i;
				this.d = d;
				this.t = t;
				this.beta = beta;
			}

		}

		public float udelta;

		private PIDParameters pidParameters;
        private int pidMaxOutPut = 100;

		public PIDController() { }
		public PIDController(Controller controller)
		{
			this.pidParameters = new PIDParameters();
			AppSettingsReader reader = new AppSettingsReader();

            this.pidMaxOutPut = (int)reader.GetValue("PIDMaxOutPut",typeof(int));

			switch (controller)
			{
				case Controller.Furnace_1000:
					this.pidParameters.P = (float)reader.GetValue("Furnace_1000_P", typeof(float));
					this.pidParameters.I = (float)reader.GetValue("Furnace_1000_I", typeof(float));
					this.pidParameters.D = (float)reader.GetValue("Furnace_1000_D", typeof(float));
					this.pidParameters.T = (float)reader.GetValue("DrawData_Interval", typeof(float));
					this.pidParameters.Beta = (float)reader.GetValue("Furnace_1000_Beta", typeof(float));
					break;

				case Controller.Furnace_850:
					this.pidParameters.P = (float)reader.GetValue("Furnace_850_P", typeof(float));
					this.pidParameters.I = (float)reader.GetValue("Furnace_850_I", typeof(float));
					this.pidParameters.D = (float)reader.GetValue("Furnace_850_D", typeof(float));
					this.pidParameters.T = (float)reader.GetValue("DrawData_Interval", typeof(float));
					this.pidParameters.Beta = (float)reader.GetValue("Furnace_850_Beta", typeof(float));
					break;

				case Controller.Furnace_700:
					this.pidParameters.P = (float)reader.GetValue("Furnace_700_P", typeof(float));
					this.pidParameters.I = (float)reader.GetValue("Furnace_700_I", typeof(float));
					this.pidParameters.D = (float)reader.GetValue("Furnace_700_D", typeof(float));
					this.pidParameters.T = (float)reader.GetValue("DrawData_Interval", typeof(float));
					this.pidParameters.Beta = (float)reader.GetValue("Furnace_700_Beta", typeof(float));
					break;

                case Controller.Furnace_900:
                    this.pidParameters.P = (float)reader.GetValue("Furnace_900_P", typeof(float));
                    this.pidParameters.I = (float)reader.GetValue("Furnace_900_I", typeof(float));
                    this.pidParameters.D = (float)reader.GetValue("Furnace_900_D", typeof(float));
                    this.pidParameters.T = (float)reader.GetValue("DrawData_Interval", typeof(float));
                    this.pidParameters.Beta = (float)reader.GetValue("Furnace_900_Beta", typeof(float));
                    break;
				
                //case Controller.System:
                //    this.pidParameters.P = (float)reader.GetValue("System_P", typeof(float));
                //    this.pidParameters.I = (float)reader.GetValue("System_I", typeof(float));
                //    this.pidParameters.D = (float)reader.GetValue("System_D", typeof(float));
                //    this.pidParameters.T = (float)reader.GetValue("DrawData_Interval", typeof(float));
                //    this.pidParameters.Beta = (float)reader.GetValue("System_Beta",typeof(float));
                //    break;
			}
		}

		public byte OutPut(float tTarget, float tCurrent, float tLast, float tLastLast, ref float uLast, ref float udelta)
		{
			float outPutValue = 0;
			float P = this.pidParameters.P;
			float I = this.pidParameters.I;
			float D = this.pidParameters.D;
			float T = this.pidParameters.T;
			float ek = tTarget - tCurrent;
			float beta = (Math.Abs(ek) <= this.pidParameters.Beta) ? 1 : 0;
			float ek_1 = tTarget - tLast;
			float ek_2 = tTarget - tLastLast;
			float uDelta = P * (tLast - tCurrent) +
										beta * P * T / I * (tTarget - tCurrent) +
										P * D / T * (2 * tLast - tCurrent - tLastLast);
			this.udelta = uDelta;
			outPutValue = uLast + uDelta;
			uLast = outPutValue;
			if (outPutValue < 0) outPutValue = 0;
			//if (outPutValue > 200) outPutValue = 200;

            if (outPutValue > this.pidMaxOutPut) outPutValue = this.pidMaxOutPut;

			//当ek <= -15时，即炉温测量值超过炉温设定值15ºC，
			//主机发送一个除0-100以外的任意值，即断开加热装置
			if (ek <= -50) outPutValue = 210;
			this.udelta = uDelta;
			return (byte)((int)outPutValue);
		}


	}
}
