using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    public class GlobaState
    {
        //第一个实验（高温抗压强度）变量状态
        private FirstLabState firstLabState = new FirstLabState();

        public FirstLabState FirstLabState
        {
            get { return this.firstLabState; }
            set { this.firstLabState = value; }
        }

        //第二个实验（膨胀力）变量状态
        private SecondLabState secondLabState = new SecondLabState();

        public SecondLabState SecondLabState
        {
            get { return this.secondLabState; }
            set { this.secondLabState = value; }
        }

        //第三个实验（耐高温时间）变量状态
        private ThirdLabState thirdLabState = new ThirdLabState();

        public ThirdLabState ThirdLabState
        {
            get { return this.thirdLabState; }
            set { this.thirdLabState = value; }
        }

        //第四个实验（高温急热膨胀率）变量状态
        private FourthLabState fouthLabState = new FourthLabState();

        public FourthLabState FouthLabState
        {
            get { return this.fouthLabState; }
            set { this.fouthLabState = value; }
        }

        //测试枚举
        private Labs labNum = Labs.None;

        public Labs LabNum
        {
            get { return this.labNum; }
            set { this.labNum = value; }
        }

        //是否开始测试
        private bool isStartTest = false;

        public bool IsStartTest
        {
            get { return this.isStartTest; }
            set { this.isStartTest = value; }
        }

        //结束测试
        private bool isEndTest = false;

        public bool IsEndTest
        {
            get { return this.isEndTest; }
            set { this.isEndTest = value; }
        }

        //高温抗压强度
        public void GoToFirstLab()
        {
            this.labNum = Labs.FirstLab;
            this.isStartTest = true;
            this.isEndTest = false;

            #region 2023-01-08
            this.pressureZeroQueue.Clear();
            this.isTimeReached = false;
            #endregion
        }

        //高温膨胀力
        public void GoToSecondLab()
        {
            this.labNum = Labs.SecondLab;
            this.isEndTest = false;
            this.isStartTest = true;

            #region 2023-01-08
            this.pressureZeroQueue.Clear();
            this.isTimeReached = false;
            #endregion
        }

        //耐高温时间
        public void GoToThreeStep()
        {
            this.labNum = Labs.ThirdLab;
            this.isEndTest = false;
            this.isStartTest = true;

            #region 2023-01-08
            this.pressureZeroQueue.Clear();
            this.isTimeReached = false;
            #endregion
        }

        //高温急热膨胀力
        public void GoToFourStep()
        {
            this.labNum = Labs.FourthLab;
            this.isEndTest = false;
            this.isStartTest = true;
        }

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return this.isTimeReached; }
            set { this.isTimeReached = value; }
        }

        private int countTime = 0;//收到指令的个数

        public void TimeCount(int timeCount)
        {
            this.countTime++;
            if (this.countTime == 5 * timeCount)
            {
                this.isTimeReached = true;
                this.countTime = 0;
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

            float[] zeroArray = pressureZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.pressureZero = sum / zeroArray.Length;
        }

    }
}
