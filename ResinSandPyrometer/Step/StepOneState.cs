using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class StepOneState
    {

        private FirstStep enumFirst = FirstStep.NONE;

        public FirstStep EnumFirst
        {
            get { return enumFirst; }
            set { enumFirst = value; }
        }
        //最大压强值
        private float maxPressure = 0;

        public float MaxPressure
        {
            get { return maxPressure; }
            set { maxPressure = value; }
        }

        //最大压强值对应的时间
        private float maxPreesureTime = 0;

        public float MaxPreesureTime
        {
            get { return maxPreesureTime; }
            set { maxPreesureTime = value; }
        }

        public int countTime = 0;

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return isTimeReached; }
            set { isTimeReached = value; }
        }

        private bool isCommandReached = false;

        public bool IsCommandReached
        {
            get { return isCommandReached; }
            set { isCommandReached = value; }
        }

        private int commandCount = 0;//收到指令的个数

        public void CommandCount(int timeCount)
        {
            commandCount++;
            if (commandCount == 5 * timeCount)
            {
                isCommandReached = true;
                commandCount = 0;
            }

        }

        //定义零点值
        private float pressureZero = 0f;

        public float PressureZero
        {
            get { return pressureZero; }
            set { pressureZero = value; }
        }

        private Queue<float> PressureZeroQueue = new Queue<float>();

        //取零点值最后十个数（去掉最大最小值，剩余的十个数平均值作为零点值）
        public void GetPressureZero(float pressure)
        {
            if (pressure < 0.1f)
                return;

            if (this.PressureZeroQueue.Count == 5)
            {
                this.PressureZeroQueue.Dequeue();
            }
            this.PressureZeroQueue.Enqueue(pressure);

            float[] zeroArray = PressureZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.PressureZero = sum / 5;
        }

        public void PressureZeroClear()
        {
            PressureZeroQueue.Clear();
            commandCount = 0;
        }
        public int TimeCount(int soakingTime)
        {
            countTime++;
            if (countTime == 5 * soakingTime)
            {
                isTimeReached = true;
                countTime = 5 * soakingTime;
            }
            return soakingTime - countTime / 5;
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
        private bool ispressureSudChange = false;

        public bool IspressureSudChange
        {
            get { return ispressureSudChange; }
            set { ispressureSudChange = value; }
        }

        //检查位移是否突变
        int changeCount = 0;
        public void CheckPressureSubChange(float pressure)
        {
            changeCount++;
            if (changeCount < 75) return;

            if ((maxPressure - pressure) / maxPressure > 0.75 /*0.5*/ || changeCount >600 /*450*/)
            {
                ispressureSudChange = true;
            }

        }

        public void InitState()
        {
            this.commandCount = 0;
            this.isCommandReached = false;
            this.countTime = 0;
            this.isTimeReached = false;
            this.changeCount = 0;
            this.ispressureSudChange = false;
            this.maxPressure = 0;
            this.maxPreesureTime = 0;
        }
    }
}
