using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoatingAnalysis
{
    public class TestResult
    {
        //传输特性的最大压力
        public float TransMaxPress = 0;

        //传输特性最大压力对应的时间
        public float TranMaxPrees_Time = 0;

        //传输特性对应的结束时间
        public float TranEndTime = 0; 

        //高温透气率
        public double Ventiratio = 0;

        //高温强度的最大强度
        public float StrengthMaxPress = 0;

        //高温强度对应的时间
        public float StrengthMaxPress_Time = 0;

        //高温透气率对应的时间te
        public float VentiEndTime = 1;
    }
}
