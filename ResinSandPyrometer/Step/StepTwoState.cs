using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class StepTwoState
    {

        private SecondStep enumTwo = SecondStep.NONE;

        public SecondStep EnumTwo
        {
            get { return enumTwo; }
            set { enumTwo = value; }
        }
        //判断膨胀力是否到达预载荷值
        private bool isLiReached = false;

        public bool IsLiReached
        {
            get { return isLiReached; }
            set { isLiReached = value; }
        }
        //定义零点值
        private float pressZero = 0f;

        public float PressZero
        {
            get { return pressZero; }
            set { pressZero = value; }
        }

        private Queue<float> PPressZeroQueue = new Queue<float>();

        //取零点值最后十个数（去掉最大最小值，剩余的十个数平均值作为零点值）
        public void GetPressZero(float pressure)
        {
            if (pressure < 0.1f)
                return;

            if (this.PPressZeroQueue.Count == 5)
            {
                this.PPressZeroQueue.Dequeue();
            }
            this.PPressZeroQueue.Enqueue(pressure);

            float[] zeroArray = PPressZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.PressZero = sum / 5;
        }


        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return isTimeReached; }
            set { isTimeReached = value; }
        }

        private int countTime = 0;//收到指令的个数

        public void TimeCount(float timeCount)
        {
            countTime++;
            if (countTime > 5 * timeCount)
            {
                isTimeReached = true;
                countTime = 0;
            }

        }

        //最大膨胀力
        private float maxPress;

        public float MaxPress
        {
            get { return maxPress; }
            set { maxPress = value; }
        }

        //最大膨胀力所对应的时间
        private float maxPress_Time;

        public float MaxPress_Time
        {
            get { return maxPress_Time; }
            set { maxPress_Time = value; }
        }

        //得到最大膨胀力和所对应的时间
        public void GetMaxPress(PointF point)
        {
            if (point.Y > maxPress)
            {
                this.maxPress = point.Y;
                this.maxPress_Time = point.X;
            }
        }

        //判断膨胀力是否突变
        private bool isPressSudChange = false;

        public bool IsPressSudChange
        {
            get { return isPressSudChange; }
            set { isPressSudChange = value; }
        }

        int changeCount = 0;

        private Queue<float> PressSudChangeQueue = new Queue<float>();
        //检查膨胀力是否突变
        public void CheckPressSudChange(float pressure)
        {
            if (this.PressSudChangeQueue.Count == 10)
            {
                this.PressSudChangeQueue.Dequeue();
            }
            this.PressSudChangeQueue.Enqueue(pressure);

            changeCount++;
            if (changeCount < 75) return;

            int sum = 0;
            if (this.PressSudChangeQueue.Count == 10)
            {
                float[] array = this.PressSudChangeQueue.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (1.5f * array[i] > maxPress)
                    {
                        sum++;
                    }
                }

                if (sum == 0 || changeCount > 450)
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
            this.PressSudChangeQueue.Clear();
            this.isPressSudChange = false;
            this.pressZero = 0;
        }

    }
}
