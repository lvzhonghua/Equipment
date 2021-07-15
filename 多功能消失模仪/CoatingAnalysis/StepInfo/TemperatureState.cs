using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public class TemperatureState
    {
        //判断温度是否重新设置
        private bool isTemReset = false;

        private bool startCount = false;
        public bool IsTemReset
        {
            get { return isTemReset; }
            set { isTemReset = value; }
        }

        //当温度达到时开始计数的参数
        private int timeCount = 0;

        public int TimeCount
        {
            get { return timeCount; }
            set { timeCount = value; }
        }

        //判断是否可以开始加热
        private bool isCanStartHeat = false;

        public bool IsCanStartHeat
        {
            get { return isCanStartHeat; }
            set { isCanStartHeat = value; }
        }

        //计数的函数
        public bool CountTime(float recTemperature, int setTemperature)
        {
            //if (recTemperature < setTemperature + 5 && recTemperature > setTemperature - 5)
            if (recTemperature > setTemperature-3)
            {
                this.startCount=true;
            }
            else if (recTemperature < setTemperature - 5)
            {
                this.startCount = false;
                this.timeCount = 0;
            }
            
            if(startCount)
            {
                this.timeCount ++;
            }
            

            if (this.timeCount >= 60)
               return true;
            return false;
        }

        //计数的函数
        public bool CountTime1(float recTemperature, int setTemperature)
        {
            //if (recTemperature < setTemperature + 5 && recTemperature > setTemperature - 5)
            if (recTemperature > setTemperature - 5)
            {
                this.timeCount++;
            }
            else
            {
                this.timeCount = 0;
            }

            if (this.timeCount >= 60)
                return true;
            return false;
        }
    }
}
