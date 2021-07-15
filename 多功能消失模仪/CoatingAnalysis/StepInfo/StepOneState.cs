using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public class StepOneState : StepState
    {
        //测试的时间是否超过了100s（即是否超时）（当计数达到500个时变为true）
        private bool isTimeEnough = false;

        //测试一的时间计数
        private int count = 0;

        //测试开始后是否记录10s的数据（即50个数据）
        private bool countReached = false;

        //接收传感器1数据转换为接收传感器2数据
        private byte[] changeTestBuffer = new byte[3];

        public byte[] ChangeTestBuffer
        {
            get { return changeTestBuffer; }
            set { changeTestBuffer = value; }
        }

        //测试一的枚举
        private EnumOfFirstStep enumOfFS = EnumOfFirstStep.NONE;

        public EnumOfFirstStep EnumOfFS
        {
            get { return enumOfFS; }
            set { enumOfFS = value; }
        }

        //压力是否为零的变量
        private bool isPressZero = false;

        public bool IsPressZero
        {
            get { return isPressZero; }
            set { isPressZero = value; }
        }

        private float tranEndTime = 0f;

        public float  TranEndTime 
        {
            get{return tranEndTime;}
        }

        private Queue<float> checkPress = new Queue<float>();
        private Queue<float> timeQueue = new Queue<float>();
        //检查压力是否变为
        //press = 传感器值 - zeroPress
        public void CheckPressIsZero(float currentTime,float press,float zeroPress)
        {
            this.count++;

            //计数达到50次（必要条件）
            if (this.count > 50)
                this.countReached = true;

            //计数500次（非正常情况处理）100秒
            if (this.count > 500){
                this.isPressZero = true;
                this.tranEndTime = currentTime;
            }
            
            if (this.timeQueue.Count == 10)
            {this.timeQueue.Dequeue();}

            this.timeQueue.Enqueue(currentTime);

            //if (this.checkPress.Count == 15)
            //{this.checkPress.Dequeue();}
            //this.checkPress.Enqueue(press);

            /*计数100次且传感器压力值连续十次小于0.145KP时判定为到达零点压力*/
            this.checkPress.Enqueue(press);
            if (this.checkPress.Count == 11)
            {
                this.checkPress.Dequeue();
                float[] checkArray = this.checkPress.ToArray();
                int sum = 0;
                for (int i = 0; i < checkArray.Length; i++)
                {
                    if (checkArray[i]+zeroPress>0.145f)
                        sum += 1;
                }
                if (sum == 0 && this.countReached){
                    this.isPressZero = true;
                    this.tranEndTime=this.timeQueue.Dequeue();
                 }
                   
            }
        }



        public void CheckPressIsZero(float press, float zeroPress)
        {
            this.count++;

            //计数达到50次（必要条件）
            if (this.count > 50)
                this.countReached = true;

            //计数500次（非正常情况处理）100秒
            if (this.count > 500)
            {
                this.isPressZero = true;
            }

            //if (this.checkPress.Count == 15)
            //{this.checkPress.Dequeue();}
            //this.checkPress.Enqueue(press);

            /*计数100次且传感器压力值连续十二次小于0.145KP时判定为到达零点压力*/
            this.checkPress.Enqueue(press);
            if (this.checkPress.Count == 13)
            {
                this.checkPress.Dequeue();
                float[] checkArray = this.checkPress.ToArray();
                int sum = 0;
                for (int i = 0; i < checkArray.Length; i++)
                {
                    if (checkArray[i] + zeroPress > 0.145f)
                        sum += 1;
                }
                if (sum == 0 && this.countReached)
                {
                    this.isPressZero = true;
                }

            }
        }
        //构造函数 将要发的一些指令提前确定下来
        public StepOneState()
        {
            /*打开阀1*/
            BytesOperator.AppendBuffer(this.OpenBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.OpenBuffer[2] = 0x0E;

            /*关闭阀1*/
            BytesOperator.AppendBuffer(this.CloseBuffer, Encoding.Default.GetBytes("#3"), 0);
            this.CloseBuffer[2] = 0x0F;

            /*进行高温透气性测试（参考指令为#2x01 实际测试为 #2x02）*/
            BytesOperator.AppendBuffer(this.changeTestBuffer, Encoding.Default.GetBytes("#2"), 0);
            this.changeTestBuffer[2] = 0x02;
        }

        public void InitState()
        {
            this.isTimeEnough = true;
            this.count = 0;
            this.countReached = false;
            this.enumOfFS = EnumOfFirstStep.NONE;
            this.isPressZero = false;

            this.checkPress.Clear();
        }

    }
}
