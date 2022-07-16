using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ResinSandPyrometer.Common
{
    /// <summary>
    /// 记录压力传感器的数据
    /// </summary>
    class ForceLogger
    { 
        public static string CreateLogFileForFirsLab()
        {
            string fileName_FirstLab = string.Format("{0:0000}-{1:00}-{2:00}-{3:00}-{4:00}-{5:00}.csv",
                                                                     DateTime.Now.Year,
                                                                     DateTime.Now.Month,
                                                                     DateTime.Now.Day,
                                                                     DateTime.Now.Hour,
                                                                     DateTime.Now.Minute,
                                                                     DateTime.Now.Second);

            return fileName_FirstLab;

        }

        public static void WriteForceForFirstLab(string fileName, float force, float presureZero, float trueForce)
        {
            try
            {
                string dirName = "Data";
                if (Directory.Exists(dirName) == false) Directory.CreateDirectory(dirName);

                StreamWriter writer = new StreamWriter(string.Format(@"{0}\{1}", dirName, fileName), true, Encoding.Default);

                writer.WriteLine("{0},{1},{2}", force, presureZero, trueForce);

                writer.Close();
            }
            catch (Exception)
            {

            }
        }

    }
}
