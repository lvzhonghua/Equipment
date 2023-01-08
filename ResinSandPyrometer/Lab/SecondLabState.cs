using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    public class SecondLabState
    {
        private SecondLabStep step = SecondLabStep.NONE;

        public SecondLabStep Step
        {
            get { return this.step; }
            set { this.step = value; }
        }

        //判断膨胀力是否到达预载荷值
        private bool isLiReached = false;

        public bool IsLiReached
        {
            get { return this.isLiReached; }
            set { this.isLiReached = value; }
        }

        //定义零点值
        private float pressureZero = 0f;

        public float PressureZero
        {
            get { return this.pressureZero; }
            set { this.pressureZero = value; }
        }

        private Queue<float> pressZeroQueue = new Queue<float>();

        #region 2023-01-08
        private bool isPressureZero = false;

        public bool IsPressureZero
        {
            get { return this.isPressureZero; }
        }

        #endregion

        //取零点值最后十个数（去掉最大最小值，剩余的十个数平均值作为零点值）
        public void GetPressureZero(float pressure)
        {
            if (pressure < 0.1f) return;

            if (this.pressZeroQueue.Count == 3)
            {
                this.pressZeroQueue.Dequeue();

                this.isPressureZero = true;
            }
            this.pressZeroQueue.Enqueue(pressure);

            float[] zeroArray = this.pressZeroQueue.ToArray();
            float sum = 0;
            for (int index = 0; index < zeroArray.Length; index++)
            {
                sum += zeroArray[index];
            }
            this.pressureZero = sum / zeroArray.Length;
        }

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return this.isTimeReached; }
            set { this.isTimeReached = value; }
        }

        private int countTime = 0;//收到指令的个数

        public void TimeCount(float timeCount)
        {
            this.countTime++;
            if (this.countTime > 5 * timeCount)
            {
                this.isTimeReached = true;
                this.countTime = 0;
            }
        }

        //最大膨胀力
        private float maxPress;

        public float MaxPress
        {
            get { return this.maxPress; }
            set { this.maxPress = value; }
        }

        //最大膨胀力所对应的时间
        private float maxPress_Time;

        public float MaxPress_Time
        {
            get { return this.maxPress_Time; }
            set { this.maxPress_Time = value; }
        }

        //得到最大膨胀力和所对应的时间
        public void GetMaxPress(PointF point)
        {
            if (point.Y > this.maxPress)
            {
                this.maxPress = point.Y;
                this.maxPress_Time = point.X;
            }
        }

        //判断膨胀力是否突变
        private bool isPressureSudChange = false;

        public bool IsPressureSudChange
        {
            get { return this.isPressureSudChange; }
            set { this.isPressureSudChange = value; }
        }

        private int changeCount = 0;

        private Queue<float> pressureSudChangeQueue = new Queue<float>();
        //检查膨胀力是否突变
        public void CheckPressureSudChange(float pressure)
        {
            if (this.pressureSudChangeQueue.Count == 10)
            {
                this.pressureSudChangeQueue.Dequeue();
            }
            this.pressureSudChangeQueue.Enqueue(pressure);

            this.changeCount++;
            if (this.changeCount < 75) return;

            int sum = 0;
            if (this.pressureSudChangeQueue.Count == 10)
            {
                float[] array = this.pressureSudChangeQueue.ToArray();
                for (int index = 0; index < array.Length; index++)
                {
                    if (1.5f * array[index] > this.maxPress)
                    {
                        sum++;
                    }
                }

                if (sum == 0 || this.changeCount > 450)
                {
                    this.isPressureSudChange = true;
                }
            }
        }

        public void InitState()
        {
            this.maxPress = 0;
            this.maxPress_Time = 0;
            this.changeCount = 0;
            
            #region 2023-01-08
            this.pressZeroQueue.Clear();
            this.isPressureZero = false;
            #endregion

            this.pressureSudChangeQueue.Clear();
            this.isPressureSudChange = false;
            this.pressureZero = 0;
            this.isTimeReached = false;
        }

    }
}
