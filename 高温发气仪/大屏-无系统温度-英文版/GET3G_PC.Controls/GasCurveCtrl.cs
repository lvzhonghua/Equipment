using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GET3G_PC.Controls
{
    public partial class GasCurveCtrl : UserControl
    {
        private const float X_PADDING = 55f;
        private const float Y_PADDING = 15f;
        
        private const float DOWN_LEVEL = 50f;

        private int interval = 1;

        public int Interval
        {
            get { return interval; }
            set { interval = value; }
        }


        public string CurveName
        {
            get { return this.grpContainer.Text; }
            set { this.grpContainer.Text = value; }
        }

        private long gasTime = 0;
        public long GasTime
        {
            get { return this.gasTime; }
            set
            {
                this.gasTime = value;
                this.gasTimeList.Add(this.gasTime);
            }
        }

        private List<long> gasTimeList = new List<long>();
        public List<long> GasTimeList
        {
            get { return this.gasTimeList; }
            set { this.gasTimeList = value; }
        }

        private float gas = 0;
        public float Gas
        {
            get { return this.gas; }
            set
            {
                this.gas = value;
                this.gasList.Add(this.gas);
            }
        }

        private List<float> gasList = new List<float>();
        public List<float> GasList
        {
            get { return this.gasList; }
            set { this.gasList = value; }
        }

        private float gasInCrement = 0;
        public float GasInCrement
        {
            get { return this.gasInCrement; }
            set
            {
                this.gasInCrement = value;
                this.gasInCrementList.Add(this.gasInCrement);
            }
        }

        private List<float> gasInCrementList = new List<float>();
        public List<float> GasInCrementList
        {
            get { return this.gasInCrementList; }

            set { this.gasInCrementList = value; }
        }

        public GasCurveCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 清空发气曲线数据
        /// </summary>
        public void ResetGasDatas()
        {
            this.gasTimeList.Clear();
            this.gasList.Clear();
            this.gasInCrementList.Clear();
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

        /// <summary>
        /// 绘制发气曲线
        /// </summary>
        public void DisplayGasCurve()
        {
            float width = (float)this.panCurve.Width - X_PADDING * 2;
            float height = (float)this.panCurve.Height - Y_PADDING * 2;

            float yGasUnit, yGasIncrementUnit;
            float maxGasIncrementUnit= this.FindMaxGasIncrement();
            float minGasIncrementUnit= this.FindMinGasIncrement();
            float maxGasUnit= this.FindMaxGas();
            float minGasUnit= this.FindMinGas();
            float yGasScaler = this.GetGasHeightScaler();

            float yGasIncrementScaler = this.GetGasIncrementHeightScaler();
            yGasUnit = (height / yGasScaler) * 0.9f;
            yGasIncrementUnit = (height / yGasIncrementScaler) * 0.9f;
           
            float fixedAxlePosition = height;

            int count = this.gasTimeList.Count - 1;

            if (count == 0)
            {
                count = 1;
            }

            float perWidth = width / count;

            Graphics grp = this.panCurve.CreateGraphics();
            BufferedGraphics bufferGrp = BufferedGraphicsManager.Current.Allocate(grp, this.panCurve.ClientRectangle);

            bufferGrp.Graphics.Clear(Color.Black);

            int countFlag = count / (30 / this.interval) + 1;

            //int countFlag = count /(60/this.interval) + 1;

            //绘制X轴（发气时间）
            //发气时间轴坐标变换
            PointF xStartPoint = new PointF(X_PADDING,fixedAxlePosition-15f);
            PointF xEndPoint = new PointF(X_PADDING + width, fixedAxlePosition-15f);

            bufferGrp.Graphics.DrawLine(Pens.White, xStartPoint, xEndPoint);

            Font font = new Font("宋体", 9f);

            for (int index = 0; index < 11; index++)
            {
                PointF startPoint = new PointF(X_PADDING + width / 10 * index, fixedAxlePosition - 6f - 15f);
                PointF endPoint = new PointF(X_PADDING + width / 10 * index, fixedAxlePosition - 15f);

                bufferGrp.Graphics.DrawLine(Pens.White, startPoint, endPoint);
                if (index != 0)
                {
                    //bufferGrp.Graphics.DrawString((0.1 * countFlag * index).ToString("0.0"), font, Brushes.White, startPoint.X - 10f, endPoint.Y+5f);
                    float time_X = 0.1f * countFlag * index;
                    int minute_X = (int)Math.Floor(time_X);
                    int second_X = (int)((time_X - minute_X) * 60);
                    bufferGrp.Graphics.DrawString(string.Format("{0}:{1:00}",minute_X,second_X), font, Brushes.White, startPoint.X - 10f, endPoint.Y + 5f);
                }
            }

            //绘制时间X轴名称
            bufferGrp.Graphics.DrawString("Time，min", font, Brushes.White, new PointF(this.panCurve.Width/2-Y_PADDING, this.panCurve.Height-Y_PADDING-10f));
            
            //绘制右侧Y轴（发气速度）			
            PointF yGasSpeedStart = new PointF(X_PADDING + width, 2f);
            PointF yGasSpeedEnd = new PointF(X_PADDING + width, this.panCurve.Height-20);

            bufferGrp.Graphics.DrawLine(Pens.White, yGasSpeedStart, yGasSpeedEnd);
            //绘制箭头
            PointF pointA, pointB, pointC;
            pointA = new PointF(X_PADDING + width, 2f);
            pointB = new PointF(X_PADDING + width - 4f, 12f);
            pointC = new PointF(X_PADDING + width + 4f, 12f);
            bufferGrp.Graphics.FillPolygon(Brushes.White, new PointF[] { pointA, pointB, pointC });
                           
            //绘制“发气速度”坐标值
            float fiveDiv_GasIncrement = yGasIncrementScaler / 5;

            for (int index = 1; index <= 5; index++)		//Y轴正坐标的标值
            {
                PointF startPoint = new PointF(X_PADDING + width - 6f, fixedAxlePosition - fiveDiv_GasIncrement * index * yGasIncrementUnit - 15f);
                PointF endPoint = new PointF(X_PADDING + width, fixedAxlePosition - fiveDiv_GasIncrement * index * yGasIncrementUnit - 15f);
                bufferGrp.Graphics.DrawLine(Pens.White, startPoint, endPoint);

                PointF flagPosition = new PointF(X_PADDING + width + 5f, fixedAxlePosition - fiveDiv_GasIncrement * index * yGasIncrementUnit - 15f - 5f);

                bufferGrp.Graphics.DrawString(string.Format("{0:0.0}", index*fiveDiv_GasIncrement), font, Brushes.Yellow, flagPosition.X, flagPosition.Y);
            }

            //绘制左侧Y轴（发气量）
            PointF yGasStart = new PointF(X_PADDING, 2f);
            PointF yGasEnd = new PointF(X_PADDING, this.panCurve.Height - 20);
            bufferGrp.Graphics.DrawLine(Pens.White, yGasStart, yGasEnd);

            //绘制箭头
            pointA = new PointF(X_PADDING, 2f);
            pointB = new PointF(X_PADDING - 4f, 12f);
            pointC = new PointF(X_PADDING + 4f, 12f);
            bufferGrp.Graphics.FillPolygon(Brushes.White, new PointF[] { pointA, pointB, pointC });
            
            //绘制“发气量”坐标值
            int fiveDiv_Gas = (int)(yGasScaler / 5);

            Pen penDash = new Pen(Brushes.Blue, 0.5f);
            penDash.DashStyle = DashStyle.Dash;

            for (int index = 0; index <= yGasScaler; index+=fiveDiv_Gas)		//Y轴正坐标的标值
            {
                PointF startPoint = new PointF(X_PADDING,fixedAxlePosition -  index * yGasUnit -15f);
                PointF endPoint = new PointF(X_PADDING+6f,fixedAxlePosition -  index * yGasUnit-15f);
                bufferGrp.Graphics.DrawLine(Pens.White, startPoint, endPoint);

                PointF flagPosition = new PointF(40f, fixedAxlePosition  - index * yGasUnit-15f-5f);
                bufferGrp.Graphics.DrawString(string.Format("{0:0}",index), font, Brushes.Red, flagPosition.X, flagPosition.Y);

                if (index == 0) continue;

                PointF startPointLine = new PointF(X_PADDING, fixedAxlePosition - index * yGasUnit - 15f);
                PointF endPointLine = new PointF(X_PADDING + width, fixedAxlePosition - index * yGasUnit - 15f);

                bufferGrp.Graphics.DrawLine(penDash, startPointLine, endPointLine);
            }

            //绘制“发气速度”曲线             

            for (int index = 0; index < this.gasInCrementList.Count - 1; index++)
            {
                PointF startPoint = new PointF(X_PADDING + width / ((60 / interval) * countFlag) * (index + 1), fixedAxlePosition - yGasIncrementUnit * this.gasInCrementList[index] - 15f);
                PointF endPoint = new PointF(X_PADDING + width / ((60 / interval) * countFlag) * (index + 2), fixedAxlePosition - yGasIncrementUnit * this.gasInCrementList[index + 1] - 15f);
                bufferGrp.Graphics.DrawLine(new Pen(Color.Yellow, 1f), startPoint, endPoint);
            }

            //绘制“发气量”曲线
            for (int index = 0; index < this.gasList.Count - 1; index++)
            {
                PointF startPoint = new PointF(X_PADDING + width / ((60 / interval) * countFlag) * (index +1), fixedAxlePosition - yGasUnit * this.gasList[index] - 15f);
                PointF endPoint = new PointF(X_PADDING + width / ((60 / interval) * countFlag) * (index + 2), fixedAxlePosition - yGasUnit * this.gasList[index + 1] - 15f);
                bufferGrp.Graphics.DrawLine(new Pen(Color.Red, 1f), startPoint, endPoint);
            }

            //标记坐标轴名称
            PointF middlePoint = new PointF(4f,width / 2 - 60f);

            bufferGrp.Graphics.TranslateTransform(middlePoint.X, middlePoint.Y);
            bufferGrp.Graphics.RotateTransform(-90);

            bufferGrp.Graphics.DrawString("Gas，ml/g", font, Brushes.Red, new PointF(0, 0));
            
            //标记坐标轴名称
            middlePoint = new PointF(90f, width + 95f);

            bufferGrp.Graphics.TranslateTransform(middlePoint.X, middlePoint.Y);
            bufferGrp.Graphics.RotateTransform(-180);

            bufferGrp.Graphics.DrawString("Speed，ml/g/s", font, Brushes.Yellow, new PointF(0f, 0f));

            bufferGrp.Render();
        }

        private void panCurve_Paint(object sender, PaintEventArgs e)
        {
             this.DisplayGasCurve();		//实验进行时，绘制“发气量/发气速度”曲线
        }

        private void grpContainer_Resize(object sender, EventArgs e)
        {
             this.DisplayGasCurve();		//实验进行时，绘制“发气量/发气速度”曲线
        }
    }
}
