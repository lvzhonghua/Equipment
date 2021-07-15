using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public abstract class StepState
    {
        private byte[] openBuffer = new byte[3];

        public byte[] OpenBuffer
        {
            get { return openBuffer; }
            set { openBuffer = value; }
        }

        private byte[] closeBuffer = new byte[3];

        public byte[] CloseBuffer
        {
            get { return closeBuffer; }
            set { closeBuffer = value; }
        }
    }
}
