using ResinSandPyrometer.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class StepFourState
    {
        private FourthStep enumFour = FourthStep.NONE;

        public FourthStep EnumFour
        {
            get { return enumFour; }
            set { enumFour = value; }
        }

        //最大位移
        private float maxDisplacement = 0;

        public float MaxDisplacement
        {
            get { return maxDisplacement; }
            set { maxDisplacement = value; }
        }

        private float maxDisplacementTime = 0;

        public float MaxDisplacementTime
        {
            get { return maxDisplacementTime; }
            set { maxDisplacementTime = value; }
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

        private bool isOK = false;
        public bool IsOK
        {
            get { return isOK; }
            set { isOK = value; }
        }
        int count = 0;
        public void Sleep()
        {
            count++;
            if (count < 60) return;
            isOK = true;
        }

        private bool isWaitOver = false;
        public bool IsWaitOver
        {
            get { return isWaitOver; }
            set { isWaitOver = value; }
        }

        int waitCout = 0;
        public void Wait()
        {
            waitCout++;
            if (waitCout < 40) return;
            isWaitOver = true;
        }
        //定义零点值
        private float displacementZero = 0f;

        public float DisplacementZero
        {
            get { return displacementZero; }
            set { displacementZero = value; }
        }

        private Queue<float> DisplacementZeroQueue = new Queue<float>();

        //取零点值最后十个数（去掉最大最小值，剩余的十个数平均值作为零点值）
        public void GetDisplacementZero(float pressure)
        {
            if (pressure < 0.1f)
                return;

            if (this.DisplacementZeroQueue.Count == 5)
            {
                this.DisplacementZeroQueue.Dequeue();
            }
            this.DisplacementZeroQueue.Enqueue(pressure);

            float[] zeroArray = DisplacementZeroQueue.ToArray();
            float sum = 0;
            for (int i = 0; i < zeroArray.Length; i++)
            {
                sum += zeroArray[i];
            }
            this.DisplacementZero = sum / 5;
        }
        public void DisplacementZeroClear()
        {
            DisplacementZeroQueue.Clear();
            countTime = 0;
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
            get { return isDisplacementSudChange; }
            set { isDisplacementSudChange = value; }
        }

        //检查位移是否突变
        int changeCount = 0;
        public void CheckDisplacementSubChange(PointF point)
        {
            changeCount++;
            if (changeCount < 300) return;


            if ((maxDisplacement - point.Y) / maxDisplacement > 0.3 || changeCount > 1200)
            {
                isDisplacementSudChange = true;
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
