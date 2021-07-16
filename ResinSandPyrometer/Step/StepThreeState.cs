using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class StepThreeState
    {

        private ThirdStep enumThree = ThirdStep.NONE;

        public ThirdStep EnumThree
        {
            get { return enumThree; }
            set { enumThree = value; }
        }

        //压力
        private float balancePress;

        public float BalancePress
        {
            get { return balancePress; }
            set { balancePress = value; }
        }

        private bool isBPGet = false;

        public bool IsBPGet
        {
            get { return isBPGet; }
            set { isBPGet = value; }
        }


        //最大时间
        private float endTime;

        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return isTimeReached; }
            set { isTimeReached = value; }
        }


        private float balanceTime = 0f;

        public float BalanceTime
        {
            get { return balanceTime; }
            set { balanceTime = value; }
        }
        public void getBalanceTime(float time)
        {
            this.balanceTime = time;
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


        //判断压力是否突变
        private bool isBalancePressSudChange = false;

        public bool IsBalancePressSudChange
        {
            get { return isBalancePressSudChange; }
            set { isBalancePressSudChange = value; }
        }

        int changeCount = 0;

        private Queue<float> balancePressSudChangeQueue = new Queue<float>();
        //检查压力是否突变
        public void CheckBlancePressChange(float pressure, float setPress)
        {
            if (this.balancePressSudChangeQueue.Count == 50)
            {
                this.balancePressSudChangeQueue.Dequeue();
            }
            this.balancePressSudChangeQueue.Enqueue(pressure);

            changeCount++;
            if (changeCount < 300) return;

            int sum = 0;
            if (this.balancePressSudChangeQueue.Count == 50)
            {
                float[] array = this.balancePressSudChangeQueue.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] > setPress - 0.005f)
                        sum++;
                }

                if (sum == 0 || changeCount > 1250)
                    this.isBalancePressSudChange = true;
            }
        }

        public void InitState()
        {
            this.balancePress = 0;
            this.endTime = 0;
            this.isBPGet = false;
            this.isBalancePressSudChange = false;
            this.changeCount = 0;
            this.balancePressSudChangeQueue.Clear();
            this.isTimeReached = false;
        }
    }
}
