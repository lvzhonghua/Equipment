using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    public class ThirdLabState
    {

        private ThirdLabStep step = ThirdLabStep.NONE;

        public ThirdLabStep Step
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

        public void GetBalanceTime(float time)
        {
            this.balanceTime = time;
        }

        private int countTime = 0;//收到指令的个数

        public void TimeCount(float timeCount)
        {
            this.countTime++;
            if (this.countTime == 5 * timeCount)
            {
                this.isTimeReached = true;
                this.countTime = 0;
            }
        }

        //判断压力是否突变
        private bool isBalancePressureSudChange = false;

        public bool IsBalancePressureSudChange
        {
            get { return this.isBalancePressureSudChange; }
            set { this.isBalancePressureSudChange = value; }
        }

        private int changeCount = 0;

        private Queue<float> balancePressureSudChangeQueue = new Queue<float>();

        //检查压力是否突变
        public void CheckBlancePressureChange(float pressure, float setPress)
        {
            #region 2023-01-08
            //if (this.balancePressureSudChangeQueue.Count == 50)
            if (this.balancePressureSudChangeQueue.Count == 10)
            #endregion
            {
                this.balancePressureSudChangeQueue.Dequeue();
            }
            this.balancePressureSudChangeQueue.Enqueue(pressure);

            this.changeCount++;

            #region 2023-01-08
            //if (this.changeCount < 300) return;
            if (this.changeCount < 100) return;
            #endregion

            int sum = 0;
            #region 2023-01-08
            //if (this.balancePressureSudChangeQueue.Count == 50)
            if (this.balancePressureSudChangeQueue.Count == 10)
            #endregion
            {
                float[] array = this.balancePressureSudChangeQueue.ToArray();
                for (int index = 0; index < array.Length; index++)
                {
                    #region 2023-01-08
                    //if (array[index] > setPress - 0.005f) sum++;
                    if (array[index] > setPress * 0.8f) sum++;
                    #endregion
                }

                if (sum == 0 || changeCount > 1250)  this.isBalancePressureSudChange = true;
            }
        }

        public void InitState()
        {
            this.balancePress = 0;
            this.endTime = 0;
            this.isBPGet = false;
            this.isBalancePressureSudChange = false;
            this.changeCount = 0;
            this.balancePressureSudChangeQueue.Clear();
            this.isTimeReached = false;
        }
    }
}
