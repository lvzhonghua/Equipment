using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class SecondStepState
    {

        private SecondStep step = SecondStep.NONE;

        public SecondStep Step
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
        private float pressZero = 0f;

        public float PressZero
        {
            get { return this.pressZero; }
            set { this.pressZero = value; }
        }

        private Queue<float> pressZeroQueue = new Queue<float>();

        //取零点值最后十个数（去掉最大最小值，剩余的十个数平均值作为零点值）
        public void GetPressZero(float pressure)
        {
            if (pressure < 0.1f) return;

            if (this.pressZeroQueue.Count == 5)
            {
                this.pressZeroQueue.Dequeue();
            }
            this.pressZeroQueue.Enqueue(pressure);

            float[] zeroArray = this.pressZeroQueue.ToArray();
            float sum = 0;
            for (int index = 0; index < zeroArray.Length; index++)
            {
                sum += zeroArray[index];
            }
            this.pressZero = sum / 5;
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
        private bool isPressSudChange = false;

        public bool IsPressSudChange
        {
            get { return this.isPressSudChange; }
            set { this.isPressSudChange = value; }
        }

        private int changeCount = 0;

        private Queue<float> pressSudChangeQueue = new Queue<float>();
        //检查膨胀力是否突变
        public void CheckPressSudChange(float pressure)
        {
            if (this.pressSudChangeQueue.Count == 10)
            {
                this.pressSudChangeQueue.Dequeue();
            }
            this.pressSudChangeQueue.Enqueue(pressure);

            this.changeCount++;
            if (this.changeCount < 75) return;

            int sum = 0;
            if (this.pressSudChangeQueue.Count == 10)
            {
                float[] array = this.pressSudChangeQueue.ToArray();
                for (int index = 0; index < array.Length; index++)
                {
                    if (1.5f * array[index] > this.maxPress)
                    {
                        sum++;
                    }
                }

                if (sum == 0 || this.changeCount > 450)
                {
                    this.isPressSudChange = true;
                }
            }
        }

        public void InitState()
        {
            this.maxPress = 0;
            this.maxPress_Time = 0;
            this.changeCount = 0;
            this.pressSudChangeQueue.Clear();
            this.isPressSudChange = false;
            this.pressZero = 0;
        }

    }
}
