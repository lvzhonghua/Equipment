using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Step
{
    public class ChartResult
    {
        public string Path = "";

        public List<PointF> linePoints = new List<PointF>();

        public float resultValue = 0f;



        public ChartResult(string path, int i)
        {
            this.Path = path + i + ".txt";
        }
    }
}
