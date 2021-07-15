using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResinSandPyrometer.Common
{
    public class SampleLoggerOnTextFile
    {
        private static string FILE_NAME = string.Format("{0:0000}-{1:00}-{2:00}.txt",
                                                                            DateTime.Now.Year,
                                                                            DateTime.Now.Month,
                                                                            DateTime.Now.Day);

        /// <summary>
        /// 添加日志项
        /// </summary>
        /// <param name="log"></param>
        public static void Log(string log)
        {
            try
            {
                string dirName = "Logs";
                if (Directory.Exists(dirName) == false) Directory.CreateDirectory(dirName);

                DateTime now = DateTime.Now;
                FILE_NAME = string.Format("{0:0000}-{1:00}-{2:00}.txt",
                                                        now.Year,
                                                        now.Month,
                                                        now.Day);

                StreamWriter writer = new StreamWriter(string.Format(@"{0}\{1}", dirName, FILE_NAME), true, Encoding.Default);

                writer.WriteLine("{0}:{1}", now, log);

                writer.Close();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 添加日志项
        /// </summary>
        /// <param name="log"></param>
        public static void Log(string file, string log)
        {
            try
            {


                DateTime now = DateTime.Now;
                FILE_NAME = string.Format("{0:0000}-{1:00}-{2:00}.txt",
                                                        now.Year,
                                                        now.Month,
                                                        now.Day);

                StreamWriter writer = new StreamWriter(string.Format(@"{0}\{1}", file, FILE_NAME), true, Encoding.Default);

                writer.WriteLine("{0}:{1}", now, log);

                writer.Close();
            }
            catch (Exception)
            {

            }
        }

        public static void Clear()
        {
            try
            {
                if (File.Exists(FILE_NAME)) File.Delete(FILE_NAME);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 获取所有日志项
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>日志集合</returns>
        public static List<string> GetAllLogs(string fileName)
        {
            List<string> logs = new List<string>();

            try
            {
                StreamReader reader = new StreamReader(fileName, Encoding.Default);

                while (reader.EndOfStream == false)
                {
                    string log = reader.ReadLine();

                    logs.Add(log);
                }

                reader.Close();
            }
            catch (Exception)
            {
            }

            return logs;
        }

        /// <summary>
        /// 获取所有日志项
        /// </summary>
        /// <returns>日志集合</returns>
        public static List<string> GetAllLogs()
        {
            return GetAllLogs(FILE_NAME);
        }

        /// <summary>
        /// 显示某一段时间的日志
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>日志集合</returns>
        public static List<string> GetLogsByPeriod(DateTime startTime, DateTime endTime)
        {
            DateTime dtStart = startTime.Subtract(startTime.TimeOfDay);
            DateTime dtEnd = endTime.Date.AddDays(1);

            List<string> logs = new List<string>();

            try
            {
                StreamReader reader = new StreamReader(FILE_NAME, Encoding.Default);

                while (reader.EndOfStream == false)
                {
                    string log = reader.ReadLine();

                    string[] values = log.Split('：');
                    if (values.Length > 0)
                    {
                        DateTime dateTime = DateTime.Parse(values[0]);

                        if (dateTime >= dtStart && dateTime <= dtEnd) logs.Add(log);
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
            }

            return logs;
        }
    }
}
