using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoatingAnalysis
{
    public class TestSample
    {
        private float testDepth = 1.0f;

        public float TestDepth
        {
            get { return testDepth; }
            set { testDepth = value; }
        }

        private bool isPbGet = false;

        public bool IsPbGet
        {
            get { return isPbGet; }
            set { isPbGet = value; }
        }

        private String personName = "--";

        public String PersonName
        {
            get { return personName; }
            set { personName = value; }
        }

        private string testTime;

        public string TestTime
        {
            get { return testTime; }
            set { testTime = value; }
        }

        private int testNum = 1;

        public int TestNum
        {
            get { return testNum; }
            set { testNum = value; }
        }

        private String testName = "消失模涂料";

        public String TestName
        {
            get { return testName; }
            set { testName = value; }
        }

        private int setTemperature = 600;

        public int SetTemperature
        {
            get { return setTemperature; }
            set { setTemperature = value; }
        }

        private string testFrom = "hust";

        public string TestFrom
        {
            get { return testFrom; }
            set { testFrom = value; }
        }


        private List<PointF> transPoints = new List<PointF>();

        public List<PointF> TransPoints
        {
            get { return transPoints; }
            set { transPoints = value; }
        }

        private List<PointF> ventiPoints = new List<PointF>();

        public List<PointF> VentiPoints
        {
            get { return ventiPoints; }
            set { ventiPoints = value; }
        }

        private List<PointF> strengthPoints = new List<PointF>();

        public List<PointF> StrengthPoints
        {
            get { return strengthPoints; }
            set { strengthPoints = value; }
        }

        private TestResult testResult = new TestResult();

        public TestResult TestResult
        {
            get { return testResult; }
            set { testResult = value; }
        }

        public TestSample(int setTemOfTest)
        {
            this.setTemperature = setTemOfTest;
        }

        public void CleanPoint()
        {
            if (this.transPoints != null)
               this.transPoints.Clear();

            if (this.ventiPoints!= null)
                this.ventiPoints.Clear();

            if (this.strengthPoints != null)
                this.strengthPoints.Clear();

            this.TestResult = new TestResult();
        }

        public void GetTranMaxPressure(PointF point)
        {
            if (point.Y>this.testResult.TransMaxPress)
            {
                this.testResult.TransMaxPress = point.Y;
                this.testResult.TranMaxPrees_Time = point.X;
            }
        }

        public void GetVentina(float pb)
        {
            this.testResult.Ventiratio = 447 *testDepth / (this.testResult.VentiEndTime *20*pb);
        }

        public void GetTe(float pb,PointF point)
        {
            if (point.Y>=0.05f*pb)
                this.testResult.VentiEndTime = point.X;
        }

        public void GetVentiEndTime(float time)
        {
            this.testResult.VentiEndTime = time;
        }
    }
}
