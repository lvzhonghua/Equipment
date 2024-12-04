using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Lab
{
    internal class LabResult
    {
        /// <summary>
        /// 实验类型
        /// </summary>
        public LabType Type { get; set; }

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

        /// <summary>
        /// 预载荷值
        /// </summary>
        public float PreloadValue { get; set; }

        /// <summary>
        /// 设定强度
        /// </summary>
        public float PresetIntensity { get; set; }

        /// <summary>
        /// 最终结果值
        /// </summary>
        public float ResultValue { get; set; } = 0f;
        
        /// <summary>
        /// 实验结果文件
        /// </summary>
        public string FileName { get; set; }

        public List<PointF> LinePoints { get; set; } = new List<PointF>();

        public LabResult(string fileName)
        {
            this.FileName = fileName;
        }
    }

    internal class PathComparer : IComparer<LabResult>
    {
        public int Compare(LabResult x, LabResult y)
        {
            return x.FileName.CompareTo(y.FileName);
        }
    }
}
