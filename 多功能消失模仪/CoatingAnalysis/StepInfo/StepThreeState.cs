using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public class StepThreeState:StepState
    {
        //最大的压力
        private float maxPressure = 0;

        public float MaxPressure
        {
            get { return maxPressure; }
            set { maxPressure = value; }
        }

        //最大压力对应的时间
        private float maxPressureTime = 0;

        public float MaxPressureTime
        {
          get { return maxPressureTime; }
          set { maxPressureTime = value; }
        }

        //第三步的枚举
        private EnumOfThirdStep enumOfTS = EnumOfThirdStep.NONE;

        public EnumOfThirdStep EnumOfTS
        {
            get { return enumOfTS; }
            set { enumOfTS = value; }
        }

        //判断压力是否突变
        private bool isPressSudChange = false;

        public bool IsPressSudChange
        {
            get { return isPressSudChange; }
            set { isPressSudChange = value; }
        }

        //判断压力是否过大
        private bool isPressureTooLarge = false;

        public bool IsPressureTooLarge
        {
            get { return isPressureTooLarge; }
            set { isPressureTooLarge = value; }
        }

        private bool isVentEnough = false;

        public bool IsVentEnough
        {
            get { return isVentEnough; }
            set { isVentEnough = value; }
        }

        private bool isTestBroken = false;

        public bool IsTestBroken
        {
            get { return isTestBroken; }
            set { isTestBroken = value; }
        }

        private int countVent = 0;

        private int countTimes = 0;

        private byte[] closeKongBuffer = new byte[3];

        public byte[] CloseKongBuffer
        {
            get { return closeKongBuffer; }
            set { closeKongBuffer = value; }
        }

        private Queue<float> pressSudChangeQueue = new Queue<float>();

        //检查压力的突变
        public void CheckPressSubChange(float pressure)
        {
            if (this.pressSudChangeQueue.Count == 20)
            {
                this.pressSudChangeQueue.Dequeue();
            }
            this.pressSudChangeQueue.Enqueue(pressure);
            int sum = 0;
            if (this.pressSudChangeQueue.Count == 20)
            {
                float[] array = this.pressSudChangeQueue.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (1.1f*array[i] >= this.maxPressure)
                        sum ++;
                }
                if (sum == 0)
                    this.isPressSudChange = true;
            }
        }
        public void CheckPressSubChange(PointF point)
        {
            float pressure = point.Y;
            if (this.pressSudChangeQueue.Count == 20)
            {
                this.pressSudChangeQueue.Dequeue();
            }
            this.pressSudChangeQueue.Enqueue(pressure);
            int sum = 0;
            if (pressSudChangeQueue.Last<float>() != this.maxPressure)
            {
                
                float[] array = this.pressSudChangeQueue.ToArray();
                for (int i = array.Length-1; i > 0; i--)
                {
                    if (array[i] < this.maxPressure)
                        sum++;
                    if (array[i] == this.maxPressure)
                        break;
                }
                if (sum >6 )
                    this.isPressSudChange = true;
            }
        }

        //构造函数 确定将要发的一些指令
        public StepThreeState()
        {
            BytesOperator.AppendBuffer(this.OpenBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.OpenBuffer[2] = 0x01;

            BytesOperator.AppendBuffer(this.CloseBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.CloseBuffer[2] = 0x0F;

            BytesOperator.AppendBuffer(this.closeKongBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.closeKongBuffer[2] = 0x09;
        }

        //取得最大的压力值和时间
        public void GetMaxPress(PointF point)
        {
            if (point.Y>this.maxPressure)
            {
                this.maxPressure = point.Y;
                this.maxPressureTime = point.X;
            }
        }

        //检查压力是否过大
        public void CheckLarge(float pressure)
        {
            if (pressure > 85)
                this.isPressureTooLarge = true;
        }

        //停止空压机后计数50次 方便自动排气
        public void CheckVentEnough()
        {
            this.countVent++;
            if (countVent >=50)
                this.isVentEnough = true;
        }

        //判断试样是否在第三步前已经破坏
        public void CheckTestBroken(float pressure)
        {
            this.countTimes++;
            if (this.countTimes == 200)
            {
                 this.isTestBroken = true;
            }
        }

        public void InitState()
        {
            this.maxPressure = 0;
            this.maxPressureTime = 0;
            this.enumOfTS = EnumOfThirdStep.NONE;
            this.isPressSudChange = false;
            this.isPressureTooLarge = false;
            this.isVentEnough = false;
            this.isTestBroken = false;
            this.countVent = 0;
            this.countTimes = 0;
            this.pressSudChangeQueue.Clear();
        }
    }
}
