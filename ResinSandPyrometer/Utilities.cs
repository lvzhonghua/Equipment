﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer
{
    class Utilities
    {
        /// <summary>
        /// 根据电压值计算力（单位：牛顿）
        /// </summary>
        /// <param name="voltage">电压值</param>
        /// <param name="sensorMax">传感器最大量程</param>
        /// <param name="sensorMV">供桥电压</param>
        /// <param name="sensibility">传感器灵敏度</param>
        /// <param name="revise">校正系数</param>
        /// <returns>力（单位：牛顿）</returns>
        public static double GetForceFromVoltage(float voltage, float sensorMax, float sensorMV, float sensibility, float revise)
        {
            double result = voltage * revise * sensorMax *(2500.0f / sensorMV) / (8388607.0f * 128.0f * sensibility) * 9.81f;

            return result;
        }
    }
}