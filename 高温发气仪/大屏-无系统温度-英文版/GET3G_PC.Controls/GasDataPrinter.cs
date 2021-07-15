using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace GET3G_PC.Controls
{
    [Serializable]
    public class GasDataToPrint
    {
        private const float X_PADDING = 80f;
        private const float Y_PADDING = 15f;

        private List<long> gasTimeList = new List<long>();
        public List<long> GasTimeList
        {
            get { return this.gasTimeList; }
            set { this.gasTimeList = value; } 
        }

        private List<float> gasList = new List<float>();
        public List<float> GasList
        {
            get { return this.gasList; }

            set { this.gasList = value; }
        }

        private List<float> gasInCrementList = new List<float>();
        public List<float> GasInCrementList
        {
            get { return this.gasInCrementList; }

            set { this.gasInCrementList = value; }
        }

        private Rectangle rectToPrint;

        public Rectangle RectToPrint
        {
            get { return this.rectToPrint; }
            set { this.rectToPrint = value; }
        }

        
        private string sampleName;

        public string SampleName
        {
            get { return this.sampleName; }
            set { this.sampleName = value; }
        }

        private string sampleNumber;

        public string SampleNumber
        {
            get { return this.sampleNumber; }
            set { this.sampleNumber = value; }
        }

        private string sampleWeight;

        public string SampleWeight
        {
            get { return this.sampleWeight; }
            set { this.sampleWeight = value; }
        }

        private string sampleRepeat;

        public string SampleRepeat
        {
            get { return this.sampleRepeat; }
            set { this.sampleRepeat = value; }
        }

        private string gas;

        public string Gas
        {
            get { return this.gas; }
            set { this.gas = value; }
        }

        private string gasIncrement;

        public string GasIncrement
        {
            get { return this.gasIncrement; }
            set { this.gasIncrement = value; }
        }

        private string furnaceTargetTemperature;

        public string FurnaceTargetTemperature
        {
            get { return this.furnaceTargetTemperature; }
            set { this.furnaceTargetTemperature = value; }
        }

        private string factory;

        public string Factory
        {
            get { return this.factory; }
            set { this.factory = value; }
        }

        private string people;

        public string People
        {
            get { return this.people; }
            set { this.people = value; }
        }

        private string testTime;

        public string TestTime
        {
            get { return this.testTime; }
            set { this.testTime = value; }
        }

        private float FindMaxGas()
        {
            float maxGas = float.MinValue;
            for (int index = 0; index < this.gasList.Count; index++)
            {
                if (maxGas < this.gasList[index]) maxGas = this.gasList[index];
            }

            if (maxGas == 0) return 1f;
            return maxGas;
        }

        private float FindMinGas()
        {
            float minGas = float.MaxValue;
            for (int index = 0; index < this.gasList.Count; index++)
            {
                if (minGas > this.gasList[index]) minGas = this.gasList[index];
            }

            return minGas;
        }

        private float FindMaxGasIncrement()
        {
            float maxGasIncrement = float.MinValue;

            for (int index = 0; index < this.gasInCrementList.Count; index++)
            {
                if (maxGasIncrement < this.gasInCrementList[index]) maxGasIncrement = this.gasInCrementList[index];
            }

            if (maxGasIncrement == 0) return 1f;

            return maxGasIncrement;
        }

        private float FindMinGasIncrement()
        {
            float minGasIncrement = float.MaxValue;

            for (int index = 0; index < this.gasInCrementList.Count; index++)
            {
                if (minGasIncrement > this.gasInCrementList[index]) minGasIncrement = this.gasInCrementList[index];
            }

            return minGasIncrement;
        }

        private int GetGasHeightScaler()
        {
            int maxValue = (int)this.FindMaxGas();

            int fiveDiv = (int)(maxValue / 5);
            int fiveRest = maxValue % 5;

            if (fiveDiv == 0) return 5;

            //if (fiveRest == 0) return maxValue;

            return (fiveDiv + 1) * 5;
        }

        private int GetGasIncrementHeightScaler()
        {
            float maxValue = this.FindMaxGasIncrement();

            int oneDiv = (int)(maxValue / 1);
            float oneRest = maxValue % 1;

            if (oneDiv == 0) return 1;
            if (oneRest == 0) return (int)maxValue;

            return oneDiv + 1;
        }

        public void DrawGasDatas(Graphics graphicsToPrint)
        {
            float width = (float)this.rectToPrint.Width;
            float height = (float)this.rectToPrint.Height;

            float yGasUnit, yGasIncrementUnit;
            float maxGasIncrementUnit = this.FindMaxGasIncrement();
            float minGasIncrementUnit = this.FindMinGasIncrement();
            float maxGasUnit = this.FindMaxGas();
            float minGasUnit = this.FindMinGas();
            float yGasScaler = this.GetGasHeightScaler();                                                     

            float yGasIncrementScaler = this.GetGasIncrementHeightScaler();
            yGasUnit = (height / yGasScaler) * 0.9f;
            yGasIncrementUnit = (height / yGasIncrementScaler) * 0.9f;

            float fixedAxlePosition = height;

            int count = this.gasTimeList.Count - 1;

            if (count == 0) count = 1;

            float perWidth = width / count;
            int countFlag = count / 30 + 1;

            //绘制X轴（发气时间）
            //发气时间轴坐标变换
            PointF xStartPoint = new PointF(X_PADDING, fixedAxlePosition +135f);
            PointF xEndPoint = new PointF(X_PADDING + width-50f, fixedAxlePosition+135f);

            graphicsToPrint.DrawLine(Pens.Black, xStartPoint, xEndPoint);

            Font font = new Font("宋体", 9f);

            for (int index = 0; index < 11; index++)
            {
                PointF startPoint = new PointF(X_PADDING + (width- 90f) / 10 * index , fixedAxlePosition +129f);
                PointF endPoint = new PointF(X_PADDING + (width- 90f) / 10 * index , fixedAxlePosition +135f);

                graphicsToPrint.DrawLine(Pens.Black, startPoint, endPoint);
                if (index != 0)
                {
                    graphicsToPrint.DrawString((0.1 * countFlag * index).ToString("0.0"), font, Brushes.Black, startPoint.X - 10f, endPoint.Y+5f);
                }
            }

            //绘制时间X轴名称
            graphicsToPrint.DrawString("Time，min", font, Brushes.Black, new PointF(this.rectToPrint.Width / 2 - Y_PADDING, this.rectToPrint.Height+160f));

            ////绘制右侧Y轴（发气速度）			
            //PointF yGasSpeedStart = new PointF(X_PADDING + width - 90f, 152f);
            //PointF yGasSpeedEnd = new PointF(X_PADDING + width - 90f, this.rectToPrint.Height - 20 + 150f);

            //graphicsToPrint.DrawLine(Pens.Black, yGasSpeedStart, yGasSpeedEnd);
            //绘制时间轴箭头
            PointF pointA, pointB, pointC;
            pointA = new PointF(X_PADDING + width - 50f, fixedAxlePosition + 135f);
            pointB = new PointF(X_PADDING + width - 60f, fixedAxlePosition + 139f);
            pointC = new PointF(X_PADDING + width - 60f, fixedAxlePosition + 131f);
            graphicsToPrint.FillPolygon(Brushes.Black, new PointF[] { pointA, pointB, pointC });

            //pointA = new PointF(X_PADDING + width - 90f, 152f);
            //pointB = new PointF(X_PADDING + width - 94f, 162f);
            //pointC = new PointF(X_PADDING + width-86f, 162f);
            //graphicsToPrint.FillPolygon(Brushes.Black, new PointF[] { pointA, pointB, pointC });

            ////绘制“发气速度”坐标值
            //float fiveDiv_GasIncrement = yGasIncrementScaler / 5;

            //for (int index = 1; index <= 5; index++)		//Y轴正坐标的标值
            //{
            //    PointF startPoint = new PointF(X_PADDING + width -96f, fixedAxlePosition - fiveDiv_GasIncrement * index * yGasIncrementUnit+135f);
            //    PointF endPoint = new PointF(X_PADDING + width - 90f, fixedAxlePosition - fiveDiv_GasIncrement * index * yGasIncrementUnit +135f);
            //    graphicsToPrint.DrawLine(Pens.Black, startPoint, endPoint);

            //    PointF flagPosition = new PointF(X_PADDING + width + 5f - 90f, fixedAxlePosition - fiveDiv_GasIncrement * index * yGasIncrementUnit +130f);

            //    graphicsToPrint.DrawString(string.Format("{0:0.0}", fiveDiv_GasIncrement * index), font, Brushes.Blue, flagPosition.X, flagPosition.Y);
            //}

            //绘制左侧Y轴（发气量）
            PointF yGasStart = new PointF(X_PADDING, 152f);
            PointF yGasEnd = new PointF(X_PADDING, this.rectToPrint.Height - 20 + 150f);
            graphicsToPrint.DrawLine(Pens.Black, yGasStart, yGasEnd);

            //绘制箭头
            pointA = new PointF(X_PADDING, 152f);
            pointB = new PointF(X_PADDING - 4f, 162f);
            pointC = new PointF(X_PADDING + 4f, 162f);
            graphicsToPrint.FillPolygon(Brushes.Black, new PointF[] { pointA, pointB, pointC });

            //绘制“发气量”坐标值
            int fiveDiv_Gas = (int)(yGasScaler / 5);

            Pen penDash = new Pen(Brushes.Blue, 0.5f);
            penDash.DashStyle = DashStyle.Dash;

            for (int index = 0; index <= yGasScaler; index += fiveDiv_Gas)		//Y轴正坐标的标值
            {
                PointF startPoint = new PointF(X_PADDING, fixedAxlePosition - index * yGasUnit +135f);
                PointF endPoint = new PointF(X_PADDING + 6f , fixedAxlePosition - index * yGasUnit +135f);
                graphicsToPrint.DrawLine(Pens.Black, startPoint, endPoint);

                PointF flagPosition = new PointF(65f, fixedAxlePosition - index * yGasUnit +130f);
                graphicsToPrint.DrawString(string.Format("{0:0}", index), font, Brushes.Red, flagPosition.X, flagPosition.Y);

                if (index == 0) continue;

                PointF startPointLine = new PointF(X_PADDING, fixedAxlePosition - index * yGasUnit +135f);
                PointF endPointLine = new PointF(X_PADDING + width - 90f , fixedAxlePosition - index * yGasUnit +135f);

                graphicsToPrint.DrawLine(penDash, startPointLine, endPointLine);
            }

            ////绘制“发气速度”曲线             

            //for (int index = 0; index < this.gasInCrementList.Count - 1; index++)
            //{
            //    PointF startPoint = new PointF(X_PADDING + (width-90f) / (30f * countFlag) * index, fixedAxlePosition - yGasIncrementUnit * this.gasInCrementList[index] +135f);
            //    PointF endPoint = new PointF(X_PADDING + (width-90f) / (30f * countFlag) * (index + 1), fixedAxlePosition - yGasIncrementUnit * this.gasInCrementList[index + 1] +135f);
            //    graphicsToPrint.DrawLine(new Pen(Color.Blue, 1f), startPoint, endPoint);
            //}

            //绘制“发气量”曲线
            for (int index = 0; index < this.gasList.Count - 1; index++)
            {
                PointF startPoint = new PointF(X_PADDING + (width-90f) / (30f * countFlag) * index , fixedAxlePosition - yGasUnit * this.gasList[index]+135f);
                PointF endPoint = new PointF(X_PADDING + (width-90f) / (30f * countFlag) * (index + 1) , fixedAxlePosition - yGasUnit * this.gasList[index + 1] +135f);
                graphicsToPrint.DrawLine(new Pen(Color.Red, 1f), startPoint, endPoint);
            }

            ////////////////////////////////////////实验参数/////////////////////////////////////////
            Font fontSample = new Font("宋体", 11f);
            graphicsToPrint.DrawString("Sample:" + this.sampleName, fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 280f));
            graphicsToPrint.DrawString("No.:" + this.sampleNumber, fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING -100f, 310f));
            graphicsToPrint.DrawString("Weight:" + this.sampleWeight + "g", fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING -100f, 340f));
            graphicsToPrint.DrawString("Repeat:" + this.sampleRepeat + "次", fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING -100f, 370f));
            graphicsToPrint.DrawString("Max Gas:" + this.gas + " ml/g", fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 400f));
            graphicsToPrint.DrawString("Gas Speed:" + this.GasIncrement + " ml/g/s", fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 430f));
            graphicsToPrint.DrawString("Run Time:" + this.testTime + " mm:ss", fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 460f)); 
            graphicsToPrint.DrawString("Temperature:" + this.furnaceTargetTemperature + "℃", fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 490f));
            graphicsToPrint.DrawString("Factory:" + this.factory, fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 520f));
            graphicsToPrint.DrawString("Tester:" + this.people, fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 550f));
            graphicsToPrint.DrawString("Date:" + DateTime.Now.ToString("yyyy-MM-d"), fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 580f));
            graphicsToPrint.DrawString("Time:" + DateTime.Now.ToString("HH:mm:ss"), fontSample, Brushes.Black, new PointF(this.rectToPrint.Width + 2 * X_PADDING - 100f, 610f));
            ///////////////////////////////////////////////////////////////////////////////////////////

///////////旋转部分////////////////////////////
            //标记坐标轴名称
            PointF middlePoint = new PointF(4f+18f+25f, width / 2 - 60f );

            graphicsToPrint.TranslateTransform(middlePoint.X, middlePoint.Y + 150f);
            graphicsToPrint.RotateTransform(-90);

            graphicsToPrint.DrawString("Gas，ml/g", font, Brushes.Red, new PointF(0, 0));

            ////标记坐标轴名称
            //middlePoint = new PointF(90f, width + 95f -105f);

            //graphicsToPrint.TranslateTransform(middlePoint.X, middlePoint.Y);
            //graphicsToPrint.RotateTransform(-180);

            //graphicsToPrint.DrawString("发气速度，ml/g/s", font, Brushes.Blue, new PointF(0f, 0f));
        }
    }
}
