using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    public class TemperatureState
    {
        //温度是否重设
        private bool isTemperatureReset = false;

        public bool IsTemperatureReset
        {
            get { return isTemperatureReset; }
            set { isTemperatureReset = value; }
        }

        private bool isCountSend = false;

        public bool IsCountSend
        {
            get { return this.isCountSend; }
            set { this.isCountSend = value; }
        }

        //当温度达到时开始计数
        private int timeCount = 0;

        public int TimeCount
        {
            get { return this.timeCount; }
            set { this.timeCount = value; }
        }

        //是否开始加热
        private bool isCanStartHeat = false;

        public bool IsCanStartHeat
        {
            get { return isCanStartHeat; }
            set { isCanStartHeat = value; }
        }

        //计数
        public bool CountTime(float recTemperature, int setTemperature)
        {
            if (recTemperature < setTemperature + 5 && recTemperature > setTemperature - 5)
            {
                this.timeCount++;
            }
            else
            {
                this.timeCount = 0;
            }

            if (this.timeCount >= 50)  return true;
            return false;
        }

        //判断温度是否突变
        private bool istemperatureSudChange = false;

        public bool IstemperatureSudChange
        {
            get { return this.istemperatureSudChange; }
            set { this.istemperatureSudChange = value; }
        }

        private Queue<float> temperatureSubChangeQueue = new Queue<float>();

        //检查压强是否突变
        public void ChecktemperatureSubChange(float pressure)
        {
            if (this.temperatureSubChangeQueue.Count == 2)
            {
                this.temperatureSubChangeQueue.Dequeue();
            }
            this.temperatureSubChangeQueue.Enqueue(pressure);

            float count = 0;
            float avg = 0;
            float sum = 0;

            if (this.temperatureSubChangeQueue.Count == 2)
            {
                float[] array = this.temperatureSubChangeQueue.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    sum += array[i];
                    avg = sum / this.temperatureSubChangeQueue.Count;
                    if (avg + 100 > array[i])  count++;
                }

                if (count == 0)  this.istemperatureSudChange = true;
            }
        }

    }
}
