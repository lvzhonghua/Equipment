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
        private StepOneState oneState = new StepOneState();

        public StepOneState OneState
        {
            get { return oneState; }
            set { oneState = value; }
        }

        //第二步变量状态
        private StepTwoState twoState = new StepTwoState();

        public StepTwoState TwoState
        {
            get { return twoState; }
            set { twoState = value; }
        }

        //第三步变量状态
        private StepThreeState threeState = new StepThreeState();

        public StepThreeState ThreeState
        {
            get { return threeState; }
            set { threeState = value; }
        }

        private StepFourState fourState = new StepFourState();

        public StepFourState FourState
        {
            get { return fourState; }
            set { fourState = value; }
        }

        //测试枚举
        private Common.Steps stepNum = Common.Steps.None;

        public Common.Steps StepNum
        {
            get { return stepNum; }
            set { stepNum = value; }
        }



        //是否开始测试
        private bool isStarttest = false;

        public bool IsStarttest
        {
            get { return isStarttest; }
            set { isStarttest = value; }
        }

        //结束测试
        private bool isEndtest = false;

        public bool IsEndtest
        {
            get { return isEndtest; }
            set { isEndtest = value; }
        }

        //高温抗压强度
        public void GoToFirstStep()
        {
            this.stepNum = Common.Steps.FirstStep;
            this.isStarttest = true;
            this.isEndtest = false;
        }

        //高温膨胀力
        public void GoToSecondStep()
        {
            this.stepNum = Common.Steps.SecondStep;
            this.isEndtest = false;
            this.isStarttest = true;
        }


        //热稳定性
        public void GoToThreeStep()
        {
            this.stepNum = Common.Steps.ThirdStep;
            this.isEndtest = false;
            this.isStarttest = true;
        }

        //高温急热膨胀力
        public void GoToFourStep()
        {
            this.stepNum = Common.Steps.FourthStep;
            this.isEndtest = false;
            this.isStarttest = true;
        }

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return isTimeReached; }
            set { isTimeReached = value; }
        }

        private int countTime = 0;//收到指令的个数

        public void TimeCount(int timeCount)
        {
            countTime++;
            if (countTime == 5 * timeCount)
            {
                isTimeReached = true;
                countTime = 0;
            }

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

    }
}
