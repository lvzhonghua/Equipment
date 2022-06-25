using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class FirstStepState
    {

        private FirstStep step = FirstStep.NONE;

        public FirstStep Step
        {
            get { return this.step; }
            set { this.step = value; }
        }

        //最大压强值
        private float maxPressure = 0;

        public float MaxPressure
        {
            get { return this.maxPressure; }
            set { this.maxPressure = value; }
        }

        //最大压强值对应的时间
        private float maxPreesureTime = 0;

        public float MaxPreesureTime
        {
            get { return this.maxPreesureTime; }
            set { this.maxPreesureTime = value; }
        }

        public int CountTime { get; set; } = 0;

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return this.isTimeReached; }
            set { this.isTimeReached = value; }
        }

        private bool isCommandReached = false;

        public bool IsCommandReached
        {
            get { return this.isCommandReached; }
            set { this.isCommandReached = value; }
        }

        private int commandCount = 0;//收到指令的个数

        public void CommandCount(int timeCount)
        {
            this.commandCount++;
            if (this.commandCount == 5 * timeCount)
            {
                this.isCommandReached = true;
                this.commandCount = 0;
            }
        }

        //定义零点值
        private float pressureZero = 0f;

        public float PressureZero
        {
            get { return this.pressureZero; }
            set { this.pressureZero = value; }
        }

        private Queue<float> pressureZeroQueue = new Queue<float>();

        //取零点值最后十个数（去掉最大最小值，剩余的十个数平均值作为零点值）
        public void GetPressureZero(float pressure)
        {
            if (pressure < 0.1f) return;

            if (this.pressureZeroQueue.Count == 5)
            {
                this.pressureZeroQueue.Dequeue();
            }
            this.pressureZeroQueue.Enqueue(pressure);

            float[] zeroArray = this.pressureZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.pressureZero = sum / 5;
        }

        public void PressureZeroClear()
        {
            this.pressureZeroQueue.Clear();
            this.commandCount = 0;
        }

        public int TimeCount(int soakingTime)
        {
            this.CountTime++;
            if (this.CountTime == 5 * soakingTime)
            {
                this.isTimeReached = true;
                this.CountTime = 5 * soakingTime;
            }
            return soakingTime - this.CountTime / 5;
        }

        //得到最大压强值和对应的时间
        public void GetMaxPressure(PointF point)
        {
            if (point.Y > this.maxPressure)
            {
                this.maxPressure = point.Y;
                this.maxPreesureTime = point.X;
            }
        }

        //判断压强是否突变
        private bool isPressureSudChange = false;

        public bool IsPressureSudChange
        {
            get { return this.isPressureSudChange; }
            set { this.isPressureSudChange = value; }
        }

        //检查位移是否突变
        private int changeCount = 0;
        public void CheckPressureSubChange(float pressure)
        {
            this.changeCount++;
            if (this.changeCount < 75) return;

            if ((this.maxPressure - pressure) / this.maxPressure > 0.75 /*0.5*/ || this.changeCount >600 /*450*/)
            {
                this.isPressureSudChange = true;
            }
        }

        public void InitState()
        {
            this.commandCount = 0;
            this.isCommandReached = false;
            this.CountTime = 0;
            this.isTimeReached = false;
            this.changeCount = 0;
            this.isPressureSudChange = false;
            this.maxPressure = 0;
            this.maxPreesureTime = 0;
        }
    }
}
