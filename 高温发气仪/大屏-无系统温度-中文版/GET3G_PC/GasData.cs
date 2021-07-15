using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GET3G_PC
{
    public class GasData
    {
        private string gasDatas;

        public string GasDatas
        {
            get { return this.gasDatas; }
            set { this.gasDatas = value; }
        }

        private long time;

        public long Time
        {
            get { return this.time; }
            set { this.time = value; }
        }
        private string gasIncrementDatas;

        public string GasIncrementDatas
        {
            get { return this.gasIncrementDatas; }
            set { this.gasIncrementDatas = value; }
        }
    }
}
