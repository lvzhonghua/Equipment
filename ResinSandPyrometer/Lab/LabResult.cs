using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    public class LabResult
    {
        /// <summary>
        /// 实验类型
        /// </summary>
        public string LabType { get; set; }

        /// <summary>
        /// 实验人员
        /// </summary>
        public string Tester { get; set; }

        /// <summary>
        /// 实验时间
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// 试样编号
        /// </summary>
        public string SampleID { get; set; }

        /// <summary>
        /// 试样名称
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// 试样尺寸
        /// </summary>
        public string SampleSize { get; set; }

        /// <summary>
        /// 目标温度
        /// </summary>
        public string TargetTemperature { get; set; }

        /// <summary>
        /// 来样单位
        /// </summary>
        public string Factory { get; set; }

        /// <summary>
        /// 重复次数
        /// </summary>
        public string RepeatNumber { get; set; }
        
        /// <summary>
        /// 实验值
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// 对应时间
        /// </summary>
        public float MaxValueTime { get; set; }

        /// <summary>
        /// 保温时间
        /// </summary>
        public float HoldingTime { get; set; }

        public string Path { get; set; }

        public List<PointF> LinePoints { get; set; } = new List<PointF>();

        public float ResultValue { get; set; } = 0f;

        public LabResult(string fileName)
        {
            this.Path = fileName;
        }
    }

    public class PathComparer : IComparer<LabResult>
    {
        public int Compare(LabResult x, LabResult y)
        {
            return x.Path.CompareTo(y.Path);
        }
    }
}
