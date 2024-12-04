using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace ResinSandPyrometer.Lab
{
    class LabResultHelper
    {
        public static void ReadLabResult(ref LabResult labResult, string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader streamReader = new StreamReader(fileStream);

            string line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.Type = (LabType)Enum.Parse(typeof(LabType), line);   //实验类型

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.Tester = line;    //实验人员

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.DateTime = line;    //实验时间

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.SampleID = line;    //试样编号

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.SampleName = line;    //试样名称

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.SampleSize = line;    //试样尺寸

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.TargetTemperature = line;    //目标温度

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.Factory = line;    //来样单位

            line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.RepeatNumber = line;    //重复次数

            switch (labResult.Type)
            {
                case LabType.高温抗压强度试验:
                    line = streamReader.ReadLine();  //抗压强度：XXXX KPa
                    line = line.Split(':')[1];
                    labResult.Value = float.Parse(line.Replace("KPa",""));
                    
                    line = streamReader.ReadLine();  //对应时间：XXX S
                    line = line.Split(':')[1];
                    labResult.MaxValueTime = float.Parse(line.Replace("S", ""));
                    
                    line = streamReader.ReadLine();  //保温时间：XX S
                    line = line.Split(':')[1];
                    labResult.HoldingTime = float.Parse(line.Replace("S", ""));

                    break;
                case LabType.高温膨胀力试验:
                    line = streamReader.ReadLine();  //膨胀力值：XXXX N
                    line = line.Split(':')[1];
                    labResult.Value = float.Parse(line.Replace("N", ""));

                    line = streamReader.ReadLine();  //对应时间：XXX S
                    line = line.Split(':')[1];
                    labResult.MaxValueTime = float.Parse(line.Replace("S", ""));

                    line = streamReader.ReadLine();  //预载荷值：XX N
                    line = line.Split(':')[1];
                    labResult.PreloadValue = float.Parse(line.Replace("N", ""));

                    break;
                case LabType.耐高温时间试验:
                    line = streamReader.ReadLine();  //对应时间：XXX S
                    line = line.Split(':')[1];
                    labResult.Value = float.Parse(line.Replace("S", ""));
                    labResult.MaxValueTime = float.Parse(line.Replace("S", ""));

                    line = streamReader.ReadLine();  //设定强度：XXXX MPa
                    line = line.Split(':')[1];
                    labResult.PresetIntensity = float.Parse(line.Replace("MPa", ""));

                    break;
                case LabType.高温急热膨胀率试验:
                    line = streamReader.ReadLine();  //最大膨胀率：XXXX %
                    line = line.Split(':')[1];
                    labResult.Value = float.Parse(line.Replace("%", ""));

                    line = streamReader.ReadLine();  //最大膨胀率时间：XXXX S
                    line = line.Split(':')[1];
                    labResult.MaxValueTime = float.Parse(line.Replace("S", ""));

                    break;
            }

            while (streamReader.EndOfStream == false)
            {
                line = streamReader.ReadLine();
                string[] xy = line.Split(',');
                PointF point = new PointF();
                point.X = float.Parse(xy[0].Replace("X", ""));
                point.Y = float.Parse(xy[1].Replace("Y", ""));

                labResult.LinePoints.Add(point);
            }

            streamReader.Close();
            fileStream.Close();
            
        }
    }
}
