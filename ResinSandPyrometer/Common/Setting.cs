using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResinSandPyrometer.Common
{
    public class Setting
    {
        public static float PresureRate = 101.325f;

        /// <summary>
        /// 地球重力系数
        /// </summary>
        public static float G {get;set;} = 9.81f;
      
        /// <summary>
        /// 试样编号
        /// </summary>
        public static string SpecimenNum { get; set; }

        /// <summary>
        /// 试样名称
        /// </summary>
        public static string SpecimenName { get; set; }

        /// <summary>
        /// 试样内径
        /// </summary>
        public static float InnerDiameter { get; set; }

        /// <summary>
        /// 试样外径
        /// </summary>
        public static float SpecimenDiameter { get; set; }

        /// <summary>
        /// 试样高度
        /// </summary>
        public static float SpecimenHeight { get; set; }

        /// <summary>
        /// 实验人员
        /// </summary>
        public static string ExperimentPerson { get; set; }
        
        /// <summary>
        /// 实验单位
        /// </summary>
        public static string ExperimentUnit { get; set; }

        /// <summary>
        /// 电机空载行程
        /// </summary>
        public static float MotorIdlePath { get; set; } = 2.5f;

        /// <summary>
        /// 电机加载速度
        /// </summary>
        public static int MotorLoadingSpeed { get; set; }

        /// <summary>
        /// 膨胀率电机空载行程
        /// </summary>
        public static float DisplacementMotorIdlePath { get; set; }

        /// <summary>
        /// 加热炉升降速度
        /// </summary>
        public static int FurnaceLiftingSpeed { get; set; }

        /// <summary>
        /// 加热炉下降距离
        /// </summary>
        public static int FurnaceFallingDistance { get; set; }
        
        /// <summary>
        /// 保温时间
        /// </summary>
        public static int SoakingTime { get; set; }

        /// <summary>
        /// 预载荷力
        /// </summary>
        public static int PreloadedForce { get; set; }

        /// <summary>
        /// 预载荷压强
        /// </summary>
        public static float PreloadedPressure { get; set; }

        /// <summary>
        /// 设定温度
        /// </summary>
        public static int SettingTemperature { get; set; }

        /// <summary>
        /// 传感器最大量程
        /// </summary>
        public static int SensorMax { get; set; } = 20;

        /// <summary>
        /// 传感器灵敏度系数
        /// </summary>
        public static float SensorSys { get; set; } = 2;

        /// <summary>
        /// 供桥电压
        /// </summary>
        public static int SensorMV { get; set; } = 10;

        /// <summary>
        /// 校正
        /// </summary>
        public static float TxtRevise { get; set; } = 1.0f;

        /// <summary>
        /// 重复次数
        /// </summary>
        public static int RepeatTimes { get; set; }

        private static int testType = 0;
        public static int TestType
        {
            get { return testType; }
            set { testType = value; }
        }

        private static List<PointF> points = new List<PointF>();
        public static List<PointF> Points
        {
            get { return points; }
            set { points = value; }
        }

        public static string TestTime
        {
            get { return DateTime.Now.ToLongDateString().ToString(); }
        }

        public static void InitResult(int type)
        {
            testType = type;
            points.Clear();
        }

        public static string GetLabType()
        {
            string result= "高温抗压强度试验";
            switch (testType)
            {
                case 0:
                    result = "高温抗压强度试验";
                    break;
                case 1:
                    result = "高温膨胀力试验";
                    break;
                case 2:
                    result = "耐高温时间试验";
                    break;
                case 3:
                    result = "高温急热膨胀率试验";
                    break;
            }

            return result;
        }

        public static float GetArea()
        {
            return (float)(Math.PI * (Math.Pow(SpecimenDiameter / 2, 2) - Math.Pow(InnerDiameter / 2, 2)));
        }
        public static void Load()
        {
            Dictionary<string, string> allSettings = SettingReaderAndWriter.GetAllSettings();
            SpecimenNum = allSettings["SpecimenNum"];
            SpecimenName = allSettings["SpecimenName"];
            InnerDiameter = float.Parse(allSettings["InnerDiameter"]);
            SpecimenDiameter = float.Parse(allSettings["SpecimenDiameter"]);
            SpecimenHeight = float.Parse(allSettings["SpecimenHeight"]);
            ExperimentPerson = allSettings["ExperimentPerson"];
            ExperimentUnit = allSettings["ExperimentUnit"];
            MotorIdlePath = float.Parse(allSettings["MotorIdlePath"]);
            MotorLoadingSpeed = int.Parse(allSettings["MotorLoadingSpeed"]);
            DisplacementMotorIdlePath = float.Parse(allSettings["DisplacementMotorIdlePath"]);
            FurnaceLiftingSpeed = int.Parse(allSettings["FurnaceLiftingSpeed"]);
            FurnaceFallingDistance = int.Parse(allSettings["FurnaceFallingDistance"]);
            SoakingTime = int.Parse(allSettings["SoakingTime"]);
            PreloadedForce = int.Parse(allSettings["PreloadedForce"]);
            PreloadedPressure = float.Parse(allSettings["PreloadedPressure"]);
            SettingTemperature = int.Parse(allSettings["SettingTemperature"]);
            SensorMax = int.Parse(allSettings["SensorMax"]);
            SensorSys = float.Parse(allSettings["SensorSys"]);
            SensorMV = int.Parse(allSettings["SensorMV"]);
            TxtRevise = float.Parse(allSettings["TxtRevise"]);
        }

        public static void Save(string key,string value)
        {
            SettingReaderAndWriter.WriteAppSetting(key, value);
        }

        public static void SaveSpecimenSetting(string experimentPerson,
                                                               string experimentUnit,
                                                               string specimenNum,
                                                               string specimenName,
                                                               string innerDiameter,
                                                               string specimenDiameter,
                                                               string specimenHeight,
                                                               string num)
        {
            SettingReaderAndWriter.WriteAppSetting("ExperimentPerson", experimentPerson);
            SettingReaderAndWriter.WriteAppSetting("ExperimentUnit", experimentUnit);
            SettingReaderAndWriter.WriteAppSetting("SpecimenNum", specimenNum);
            SettingReaderAndWriter.WriteAppSetting("SpecimenName", specimenName);
            SettingReaderAndWriter.WriteAppSetting("InnerDiameter", innerDiameter);
            SettingReaderAndWriter.WriteAppSetting("SpecimenDiameter", specimenDiameter);
            SettingReaderAndWriter.WriteAppSetting("SpecimenHeight", specimenHeight);
            RepeatTimes = int.Parse(num);
            ExperimentPerson = experimentPerson;
            ExperimentUnit = experimentUnit;
            SpecimenNum = specimenNum;
            SpecimenName = specimenName;
            InnerDiameter = float.Parse(innerDiameter);
            SpecimenDiameter = float.Parse(specimenDiameter);
            SpecimenHeight = float.Parse(specimenHeight);
        }

        public static void SaveParameterSetting(string motorIdlePath, 
                                         string motorLoadingSpeed,
                                         string displacementMotorIdlePath,
                                         string furnaceLiftingSpeed, 
                                         string furnaceFallingDistance, 
                                         string soakingTime, 
                                         string preloadedForce, 
                                         string preloadedPressure)
        {
            SettingReaderAndWriter.WriteAppSetting("MotorIdlePath", motorIdlePath);
            SettingReaderAndWriter.WriteAppSetting("MotorLoadingSpeed", motorLoadingSpeed);
            SettingReaderAndWriter.WriteAppSetting("DisplacementMotorIdlePath", displacementMotorIdlePath);
            SettingReaderAndWriter.WriteAppSetting("FurnaceLiftingSpeed", furnaceLiftingSpeed);
            SettingReaderAndWriter.WriteAppSetting("FurnaceFallingDistance", furnaceFallingDistance);
            SettingReaderAndWriter.WriteAppSetting("SoakingTime", soakingTime);
            SettingReaderAndWriter.WriteAppSetting("PreloadedForce", preloadedForce);
            SettingReaderAndWriter.WriteAppSetting("PreloadedPressure", preloadedPressure);
            MotorIdlePath = float.Parse(motorIdlePath);
            MotorLoadingSpeed = int.Parse(motorLoadingSpeed);
            DisplacementMotorIdlePath = float.Parse(displacementMotorIdlePath);
            FurnaceLiftingSpeed = int.Parse(furnaceLiftingSpeed);
            FurnaceFallingDistance = int.Parse(furnaceFallingDistance);
            SoakingTime = int.Parse(soakingTime);
            PreloadedForce = int.Parse(preloadedForce);
            PreloadedPressure = float.Parse(preloadedPressure);
        }

    }
}
