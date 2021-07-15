using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    //包含全部状态
    public class GlobaState
    {
        //第一步变量的状态
        private StepOneState oneState = new StepOneState();

        public StepOneState OneState
        {
            get { return oneState; }
            set { oneState = value; }
        }

        //第二步变量的状态
        private StepTwoState twoState = new StepTwoState();

        public StepTwoState TwoState
        {
            get { return twoState; }
            set { twoState = value; }
        }

        //第三步变量的状态
        private StepThreeState threeState = new StepThreeState();

        public StepThreeState ThreeState
        {
            get { return threeState; }
            set { threeState = value; }
        }

        //第二步中的Pb
        private float pb = 10;

        public float Pb
        {
            get { return pb; }
            set { pb = value; }
        }

        //是否开始了测试
        private bool isStartTest = false;

        public bool IsStartTest
        {
            get { return isStartTest; }
            set { isStartTest = value; }
        }

        //是否结束了测试
        private bool isEndTest = false;

        public bool IsEndTest
        {
            get { return isEndTest; }
            set { isEndTest = value; }
        }

        //步骤枚举
        private StepEnum stepNum = StepEnum.None;

        public StepEnum StepNum
        {
            get { return stepNum; }
            set { stepNum = value; }
        }

        //第一个传感器的零点数据
        private float firstPressZero = 0;

        public float FirstPressZero
        {
            get { return firstPressZero; }
            set { firstPressZero = value; }
        }

        private Queue<float> firstPressQueue = new Queue<float>();

        //第二个传感器的零点数据
        private float secondPressZero = 0;

        public float SecondPressZero
        {
            get { return secondPressZero; }
            set { secondPressZero = value; }
        }

        private Queue<float> secondPressQueue = new Queue<float>();

         //状态初始化构造函数
        public GlobaState()
        {
        }

         //取得第一个传感器的0点数据
        public void GetFirstZeroPress(float pressure)
        {
            if (this.firstPressQueue.Count == 5)
            {
                this.firstPressQueue.Dequeue();
            }
            this.firstPressQueue.Enqueue(pressure);

            float[] zeroArray = this.firstPressQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.FirstPressZero = sum / zeroArray.Length;
            
        }

         //取得第二个传感器的0点数据
        public void GetSecondZeroPress(float pressure)
        {
            if (this.secondPressQueue.Count == 5)
            {
                this.secondPressQueue.Dequeue();
            }
            this.secondPressQueue.Enqueue(pressure);

            float[] zeroArray = this.secondPressQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.SecondPressZero = sum / zeroArray.Length;
        }

        //使程序直接跳转至传输特性测试
        public void GotoFirstStep()
        {
            this.isStartTest = true;
            this.stepNum = StepEnum.FirstStep;
            this.oneState.EnumOfFS = EnumOfFirstStep.开始测试并打开阀1;
            this.isEndTest = false;
        }

        //使程序直接跳转至高温透气性
        public void GotoSecondStep()
        {
            this.isStartTest = true;
            this.stepNum = StepEnum.SecondStep;
            this.twoState.EnumOfSS = EnumOfSecondStep.打开阀3及压缩机;
            this.isEndTest = false;
        }

        //使程序直接跳转至高温强度
        public void GotoThridStep()
        {
            this.isStartTest = true;
            this.stepNum = StepEnum.ThirdStep;
            this.threeState.EnumOfTS = EnumOfThirdStep.打开阀2和3以及空压机;
            this.isEndTest = false;
        }

        //使程序直接结束
        public void GotoEndTest()
        {
            this.stepNum = StepEnum.None;
            this.threeState.EnumOfTS = EnumOfThirdStep.NONE;
            this.isEndTest = true;
            this.isStartTest = false;
        }

        public void InitState()
        {
            this.firstPressZero = 0;
            this.secondPressZero = 0;
            this.firstPressQueue.Clear();
            this.secondPressQueue.Clear();

            this.isStartTest = false;
            this.isEndTest = false;
            this.stepNum = StepEnum.None;

            this.oneState.InitState();
            this.twoState.InitState();
            this.threeState.InitState();
        }
    }
}
