using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public class StepTwoState:StepState
    {
        //打开空压机及传感器2通道的指令
        private byte[] openTwoBuffer = new byte[3];

        public byte[] OpenTwoBuffer
        {
            get { return openTwoBuffer; }
            set { openTwoBuffer = value; }
        }

        //转换至第三步测试
        private byte[] changeToThree = new byte[3];

        public byte[] ChangeToThree
        {
            get { return changeToThree; }
            set { changeToThree = value; }
        }

        //判断压力是否达到
        private bool isPressReached = false;

        public bool IsPressReached
        {
            get { return isPressReached; }
            set { isPressReached = value; }
        }

        //步骤二的枚举
        private EnumOfSecondStep enumOfSS = EnumOfSecondStep.NONE;

        public EnumOfSecondStep EnumOfSS
        {
            get { return enumOfSS; }
            set { enumOfSS = value; }
        }

        //判断压力是否到达零点
        private bool isPressZero = false;

        public bool IsPressZero
        {
            get { return isPressZero; }
            set { isPressZero = value; }
        }

        private Queue<float> checkPress = new Queue<float>();

        //检查压力是否到达零点
        //press = 传感器值 - 取的零点
        public void CheckPressIsZero(float press)
        {
            if (this.checkPress.Count == 20)
                this.checkPress.Dequeue();
            this.checkPress.Enqueue(press);

            if (this.checkPress.Count == 20)
            {
                int sum = 0;
                float[] checkArrayOne = this.checkPress.ToArray();
                for (int i = 0; i < checkArrayOne.Length; i++)
                {
                    if (checkArrayOne[i] > 0.5)
                        sum += 1;
                }
                if (sum == 0)
                    this.IsPressZero = true;
            }
        }

        //检查压力是否超过10kPa
        public void CheckPressReach(float press)
        {
            if (press > 10)
                this.isPressReached = true;
        }

        //构造函数
        public StepTwoState()
        {
            BytesOperator.AppendBuffer(this.OpenBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.OpenBuffer[2] = 0x03;

            BytesOperator.AppendBuffer(this.CloseBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.CloseBuffer[2] = 0x0F;

            BytesOperator.AppendBuffer(this.openTwoBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.openTwoBuffer[2] = 0x0D;

            /*转换至高温强度测试*/
            BytesOperator.AppendBuffer(this.changeToThree, Encoding.Default.GetBytes("#2"), 0);
            this.changeToThree[2] = 0x03;
        }

        public void InitState()
        {
            this.isPressReached = false;
            this.enumOfSS = EnumOfSecondStep.NONE;
            this.isPressZero = false;
            this.checkPress.Clear();
        }

    }
}
