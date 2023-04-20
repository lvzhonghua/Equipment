using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ResinSandPyrometer.Lab
{
    class LabResultHelper
    {
        public static void ReaderSampleInfo(ref LabResult labResult, string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader streamReader = new StreamReader(fileStream);

            string line = streamReader.ReadLine();
            line = line.Split(':')[1];
            labResult.LabType = line;   //实验类型

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

            streamReader.Close();
            fileStream.Close();
            
        }
    }
}
