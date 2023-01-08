using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    public class FourthLabState
    {
        private FourthLabStep step = FourthLabStep.NONE;

        public FourthLabStep Step
        {
            get { return this.step; }
            set { this.step = value; }
        }

        //最大位移
        private float maxDisplacement = 0;

        public float MaxDisplacement
        {
            get { return this.maxDisplacement; }
            set { this.maxDisplacement = value; }
        }

        private float maxDisplacementTime = 0;

        public float MaxDisplacementTime
        {
            get { return this.maxDisplacementTime; }
            set { this.maxDisplacementTime = value; }
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

        private bool isOK = false;
        public bool IsOK
        {
            get { return this.isOK; }
            set { this.isOK = value; }
        }

        private int count = 0;
        public void Sleep()
        {
            this.count++;
            #region 2023-01-07
            //if (this.count < 60) return;
            if (this.count < 1) return;
            #endregion

            this.isOK = true;
        }

        private bool isWaitOver = false;
        public bool IsWaitOver
        {
            get { return this.isWaitOver; }
            set { this.isWaitOver = value; }
        }

        private int waitCout = 0;
        public void Wait()
        {
            this.waitCout++;
            #region 2023-01-07 修改实验流程
            //if (this.waitCout < 40) return;
            if (this.waitCout < 18) return;
            #endregion

            this.isWaitOver = true;
        }

        //定义零点值
        private float displacementZero = 0f;
        public float DisplacementZero
        {
            get { return this.displacementZero; }
            set { this.displacementZero = value; }
        }

        private Queue<float> displacementZeroQueue = new Queue<float>();

        //取零点值最后5个数（去掉最大最小值，剩余的5个数平均值作为零点值）
        public void GetDisplacementZero(float displacement)
        {
            #region 2023-01-07
            //if (this.displacementZeroQueue.Count == 5)
            //{
            //    this.displacementZeroQueue.Dequeue();
            //}
            //this.displacementZeroQueue.Enqueue(displacement);

            //float[] zeroArray = displacementZeroQueue.ToArray();
            //float sum = 0;
            //for (int index = 0; index < zeroArray.Length; index++)
            //{
            //    sum += zeroArray[index];
            //}
            //this.displacementZero = sum / 5;
            #endregion

            if (this.displacementZeroQueue.Count == 1)
            {
                this.displacementZeroQueue.Dequeue();
            }
            this.displacementZeroQueue.Enqueue(displacement);

            float[] zeroArray = displacementZeroQueue.ToArray();
            float sum = 0;
            for (int index = 0; index < zeroArray.Length; index++)
            {
                sum += zeroArray[index];
            }
            this.displacementZero = sum / zeroArray.Length;

        }

        public void DisplacementZeroClear()
        {
            this.displacementZeroQueue.Clear();
            this.countTime = 0;
        }

        //得到最大位移值和对应的时间
        public void GetMaxDisplacement(PointF point)
        {
            if (point.Y > this.maxDisplacement)
            {
                this.maxDisplacement = point.Y;
                this.maxDisplacementTime = point.X;
            }
        }

        //判断压强是否突变
        private bool isDisplacementSudChange = false;

        public bool IsDisplacementSudChange
        {
            get { return this.isDisplacementSudChange; }
            set { this.isDisplacementSudChange = value; }
        }

        //检查位移是否突变
        private int changeCount = 0;
        public void CheckDisplacementSubChange(PointF point)
        {
            this.changeCount++;
            if (this.changeCount < 300) return;

            #region 2023-01-07 
            //if ((this.maxDisplacement - point.Y) / this.maxDisplacement > 0.3 || this.changeCount > 1200)
            if ((this.maxDisplacement - point.Y) / this.maxDisplacement > 0.5 || this.changeCount > 1200)
            #endregion
            {
                this.isDisplacementSudChange = true;
            }
        }

        public void InitState()
        {
            this.count = 0;
            this.isOK = false;
            this.waitCout = 0;
            this.isWaitOver = false;
            this.maxDisplacement = 0;
            this.maxDisplacementTime = 0;
            this.changeCount = 0;
            this.isDisplacementSudChange = false;
            this.isTimeReached = false;
        }

    }
}
