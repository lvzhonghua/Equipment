using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class ThirdStepState
    {

        private ThirdStep step = ThirdStep.NONE;

        public ThirdStep Step
        {
            get { return this.step; }
            set { this.step = value; }
        }

        //压力
        private float balancePress;

        public float BalancePress
        {
            get { return this.balancePress; }
            set { this.balancePress = value; }
        }

        private bool isBPGet = false;

        public bool IsBPGet
        {
            get { return this.isBPGet; }
            set { this.isBPGet = value; }
        }

        //结束次数
        private float endTime;

        public float EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }

        private bool isTimeReached = false;

        public bool IsTimeReached
        {
            get { return this.isTimeReached; }
            set { this.isTimeReached = value; }
        }


        private float balanceTime = 0f;

        public float BalanceTime
        {
            get { return this.balanceTime; }
            set { this.balanceTime = value; }
        }

        public void getBalanceTime(float time)
        {
            this.balanceTime = time;
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

        //判断压力是否突变
        private bool isBalancePressSudChange = false;

        public bool IsBalancePressSudChange
        {
            get { return this.isBalancePressSudChange; }
            set { this.isBalancePressSudChange = value; }
        }

        private int changeCount = 0;

        private Queue<float> balancePressSudChangeQueue = new Queue<float>();
        //检查压力是否突变
        public void CheckBlancePressChange(float pressure, float setPress)
        {
            if (this.balancePressSudChangeQueue.Count == 50)
            {
                this.balancePressSudChangeQueue.Dequeue();
            }
            this.balancePressSudChangeQueue.Enqueue(pressure);

            this.changeCount++;
            if (this.changeCount < 300) return;

            int sum = 0;
            if (this.balancePressSudChangeQueue.Count == 50)
            {
                float[] array = this.balancePressSudChangeQueue.ToArray();
                for (int index = 0; index < array.Length; index++)
                {
                    if (array[index] > setPress - 0.005f) sum++;
                }

                if (sum == 0 || changeCount > 1250)  this.isBalancePressSudChange = true;
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
