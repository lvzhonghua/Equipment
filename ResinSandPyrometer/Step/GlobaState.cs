using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class GlobaState
    {
        //第一步变量状态
        private FirstStepState firstStepState = new FirstStepState();

        public FirstStepState FirstStepState
        {
            get { return this.firstStepState; }
            set { this.firstStepState = value; }
        }

        //第二步变量状态
        private SecondStepState secondStepState = new SecondStepState();

        public SecondStepState SecondStepState
        {
            get { return this.secondStepState; }
            set { this.secondStepState = value; }
        }

        //第三步变量状态
        private ThirdStepState thirdStepState = new ThirdStepState();

        public ThirdStepState ThirdStepState
        {
            get { return this.thirdStepState; }
            set { this.thirdStepState = value; }
        }

        private FourthStepState fouthStepState = new FourthStepState();

        public FourthStepState FouthStepState
        {
            get { return this.fouthStepState; }
            set { this.fouthStepState = value; }
        }

        //测试枚举
        private Steps stepNum = Steps.None;

        public Steps StepNum
        {
            get { return this.stepNum; }
            set { this.stepNum = value; }
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
        public void GoToFirstStep()
        {
            this.stepNum = Common.Steps.FirstStep;
            this.isStartTest = true;
            this.isEndTest = false;
        }

        //高温膨胀力
        public void GoToSecondStep()
        {
            this.stepNum = Common.Steps.SecondStep;
            this.isEndTest = false;
            this.isStartTest = true;
        }

        //热稳定性
        public void GoToThreeStep()
        {
            this.stepNum = Common.Steps.ThirdStep;
            this.isEndTest = false;
            this.isStartTest = true;
        }

        //高温急热膨胀力
        public void GoToFourStep()
        {
            this.stepNum = Common.Steps.FourthStep;
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

            float[] zeroArray = pressZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.PressZero = sum / 5;
        }

    }
}
