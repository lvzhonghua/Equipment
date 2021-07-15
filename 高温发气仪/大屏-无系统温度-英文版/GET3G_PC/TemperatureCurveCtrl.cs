using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Configuration;

namespace GET3G_PC
{
    /// <summary>
    /// 该控件将不再显示系统温度曲线（2014-11-20）
    /// </summary>
    public partial class TemperatureCurveCtrl : UserControl
    {
        private long nTimeCount = 0;                   //温度坐标变换计时用
        private const float X_PADDING = 55f;
        private const float Y_PADDING = 15f;

        private float timeInterval = 2;

        private const float DOWN_LEVEL = 50f;

        public string CurveName
        {
            get { return this.grpContainer.Text; }
            set { this.grpContainer.Text = value; }
        }

        public TemperatureCurveCtrl()
        {
            InitializeComponent();

            this.timeInterval = 2;
        }

        private float furnaceTemperature = 0;

        public float FurnaceTemperature
        {
            get { return this.furnaceTemperature; }
            set
            {
                this.furnaceTemperature = value;
                this.furnaceTemperatureList.Add(this.furnaceTemperature);             
            }
        }

        //private float systemTemperature = 0;

        //public float SystemTemperature
        //{
        //    get { return this.systemTemperature; }
        //    set
        //    {
        //        this.systemTemperature = value;
        //        this.systemTemperatureList.Add(this.systemTemperature);
        //    }
        //}

        private List<float> furnaceTemperatureList = new List<float>();

        public List<float> FurnaceTemperatureList
        {
            get { return this.furnaceTemperatureList; }
        }

        //private List<float> systemTemperatureList = new List<float>();

        //public List<float> SystemTemperatureList
        //{
        //    get { return this.systemTemperatureList; }
        //}

        /// <summary>
        /// 清空升温过程曲线
        /// </summary>
        public void ResetFurnaceTemperatures()
        {
            this.furnaceTemperatureList.Clear();
            //this.systemTemperatureList.Clear();
        }

        private float targetFurnaceTemperature = 850.0f;

        /// <summary>
        /// 预定炉温
        /// </summary>
        public float TargetFurnaceTemperature
        {
            get { return this.targetFurnaceTemperature; }
            set { this.targetFurnaceTemperature = value; }
        }

        //private float targetSystemTemperature = 105.0f;
        ////预定系统温度
        //public float TargetSystemTemperature
        //{
        //    get { return this.targetSystemTemperature; }
        //    set { this.targetSystemTemperature = value; }
        //}
        /// <summary>
        /// 升温过程结束标值温度
        /// </summary>
        private float upProgressEndTemperature
        {
            get { return this.targetFurnaceTemperature - DOWN_LEVEL; }
        }

        private float FindMaxFurnaceTemperature()
        {
            return this.targetFurnaceTemperature + 50.0f;
        }

        private float FindMinFurnaceTemperature()
        {
            return 0f;
        }

        /// <summary>
        /// 绘制炉子升温曲线
        /// </summary>
        public void DisplayTemperatureCurve()
        {
            this.nTimeCount++;

            float width = (float)this.panCurve.Width - X_PADDING * 2;
            float height = (float)this.panCurve.Height - Y_PADDING * 2;

            float maxFurnaceTemperature = this.FindMaxFurnaceTemperature();
            float minFurnaceTemperature = this.FindMinFurnaceTemperature();
            float yFurnaceTemperatureScale = maxFurnaceTemperature - minFurnaceTemperature;

            if (yFurnaceTemperatureScale == 0) yFurnaceTemperatureScale = maxFurnaceTemperature;

            float yUnit_Furnace = (height / yFurnaceTemperatureScale) * 0.9f;
            float yUnit_System = (height / 120) * 0.9f;
            float xAxlePosition = height;

            int count_Furnace = this.furnaceTemperatureList.Count - 1;
            if (count_Furnace == 0) count_Furnace = 1;

            //int count_System = this.systemTemperatureList.Count - 1;
            //if (count_System == 0) count_System = 1;

            float perWidth_Furnace = width / count_Furnace;

            //float perWidth_System = width / count_System;

            Graphics grp = this.panCurve.CreateGraphics();

            BufferedGraphics bufferGrp = BufferedGraphicsManager.Current.Allocate(grp, this.panCurve.ClientRectangle);

            bufferGrp.Graphics.Clear(Color.Black);

            Font font = new Font("宋体", 9f);
            //绘制X轴（时间）
            //时间轴坐标变换
            PointF xStartPoint = new PointF(X_PADDING, xAxlePosition);
            PointF xEndPoint = new PointF(X_PADDING + width, xAxlePosition);

            bufferGrp.Graphics.DrawLine(Pens.White, xStartPoint, xEndPoint);

            //绘制X轴时间坐标
            float xPerWidth = width / 10;
            int time = (int)this.timeInterval * count_Furnace;
            int fiveMinutes_Div = time / (5 * 60);
            int fiveMinutes_Rest = time % (5 * 60);
            if (fiveMinutes_Rest != 0)
            {
                fiveMinutes_Div += 1;
            }
            
            for (int index = 1; index <10; index++)
            {
                PointF startPoint = new PointF(X_PADDING + xPerWidth * index, xAxlePosition-4f);
                PointF endPoint = new PointF(X_PADDING + xPerWidth * index, xAxlePosition);
                bufferGrp.Graphics.DrawLine(Pens.White, startPoint, endPoint);

                float xTimeValue = (float)(0.5f * fiveMinutes_Div * index);                
                bufferGrp.Graphics.DrawString(xTimeValue.ToString("0.0"), font, Brushes.White, startPoint.X - 10f, endPoint.Y + 4f);
               
            }

            //绘制“炉温”坐标值
            int valueCount = (int)((yFurnaceTemperatureScale + 50) / 100);
            for (int index = 0; index <= valueCount; index++)
            {
                PointF startPoint = new PointF(X_PADDING+6f, xAxlePosition - (100 * index) * yUnit_Furnace);
                PointF endPoint = new PointF(X_PADDING, xAxlePosition - (100 * index) * yUnit_Furnace);

                bufferGrp.Graphics.DrawLine(Pens.White, startPoint, endPoint);

                PointF flagPosition = new PointF(28f, xAxlePosition - (100 * index) * yUnit_Furnace-5f);
                bufferGrp.Graphics.DrawString((100 * index + minFurnaceTemperature).ToString(), font, Brushes.Yellow, flagPosition);
            }

            //炉子目标温度用虚线表示
            Pen penTemp = new Pen(Color.Blue);
            penTemp.Width = 0.8f;
            penTemp.DashStyle = DashStyle.Dash;
            bufferGrp.Graphics.DrawLine(penTemp, new PointF(X_PADDING, xAxlePosition - (this.targetFurnaceTemperature - minFurnaceTemperature) * yUnit_Furnace),
                                                                  new PointF(X_PADDING + width, xAxlePosition - (this.targetFurnaceTemperature - minFurnaceTemperature) * yUnit_Furnace));
            //系统目标温度用虚线表示
            //Pen penTempSystem= new Pen(Color.LawnGreen);
            //penTempSystem.Width = 0.8f;
            //penTempSystem.DashStyle = DashStyle.Dash;
            //bufferGrp.Graphics.DrawLine(penTempSystem, new PointF(X_PADDING, xAxlePosition -(105 * yUnit_System)),
            //                                                      new PointF(X_PADDING + width, xAxlePosition -(105* yUnit_System)));

            //绘制左侧Y轴（炉温）
            PointF yFurnaceTemperatureStart = new PointF(X_PADDING, 2f);
            PointF yFurnaceTemperatureEnd = new PointF(X_PADDING, this.panCurve.Height - 10);
            bufferGrp.Graphics.DrawLine(Pens.White, yFurnaceTemperatureStart, yFurnaceTemperatureEnd);

            //绘制箭头
            PointF pointA = new PointF(X_PADDING, 2f);
            PointF pointB = new PointF(X_PADDING - 4f, 12f);
            PointF pointC = new PointF(X_PADDING + 4f, 12f);
            bufferGrp.Graphics.FillPolygon(Brushes.White, new PointF[] { pointA, pointB, pointC });

            //标记坐标轴名称
            //bufferGrp.Graphics.DrawString("炉温", font, Brushes.Red, new PointF(X_PADDING + 6f, Y_PADDING));
            
            //绘制右侧Y轴（系统温度）
            //PointF ySystemTemperatureStart = new PointF(X_PADDING + width, 2f);
            //PointF ySystemTemperatureEnd = new PointF(X_PADDING + width,this.panCurve.Height - 10);
            //bufferGrp.Graphics.DrawLine(Pens.White,ySystemTemperatureStart,ySystemTemperatureEnd);

            //绘制箭头
            //pointA = new PointF(X_PADDING + width, 2f);
            //pointB = new PointF(X_PADDING + width - 4f, 12f);
            //pointC = new PointF(X_PADDING + width + 4f, 12f);
            //bufferGrp.Graphics.FillPolygon(Brushes.White, new PointF[] { pointA, pointB, pointC });
            //标记坐标轴名称
            //bufferGrp.Graphics.DrawString("系统", font, Brushes.Red, new PointF(width + 20, Y_PADDING));

            //绘制“系统温度”坐标值
            //valueCount = (int)((105 + 15) / 20);
            //for (int index = 1; index <= valueCount; index++)
            //{
            //    PointF startPoint = new PointF(X_PADDING + width - 6f, xAxlePosition - (20 * index) * yUnit_System);
            //    PointF endPoint = new PointF(X_PADDING + width, xAxlePosition - (20 * index) * yUnit_System);

            //    bufferGrp.Graphics.DrawLine(Pens.White, startPoint, endPoint);

            //    PointF flagPosition = new PointF(X_PADDING + width + 7f, xAxlePosition - (20 * index) * yUnit_System-5f);
            //    bufferGrp.Graphics.DrawString((20 * index).ToString(), font, Brushes.Red, flagPosition);
            //}

            //绘制“炉温曲线”曲线
            float xUnit = width / (fiveMinutes_Div * 300 / this.timeInterval);
            for (int index = 0; index < this.furnaceTemperatureList.Count - 1; index++)
            {
                PointF startPoint = new PointF(X_PADDING + xUnit * index, xAxlePosition - yUnit_Furnace * this.furnaceTemperatureList[index]);
                PointF endPoint = new PointF(X_PADDING + xUnit * (index + 1), xAxlePosition - yUnit_Furnace * this.furnaceTemperatureList[index + 1]);

                bufferGrp.Graphics.DrawLine(new Pen(Color.Yellow, 1f), startPoint, endPoint);
            }

            //绘制“系统温度曲线”曲线
            //for (int index = 0; index < this.systemTemperatureList.Count - 1; index++)
            //{
            //    PointF startPoint = new PointF(X_PADDING + xUnit * index, xAxlePosition - yUnit_System * this.systemTemperatureList[index]);
            //    PointF endPoint = new PointF(X_PADDING + xUnit * (index + 1), xAxlePosition - yUnit_System * this.systemTemperatureList[index + 1]);

            //    bufferGrp.Graphics.DrawLine(new Pen(Color.Red, 1f), startPoint, endPoint);
            //}
            
            bufferGrp.Render();
        }

        private void panCurve_Paint(object sender, PaintEventArgs e)
        {
            this.DisplayTemperatureCurve();		//实验停止时，绘制温度曲线
        }

        private void grpContainer_Resize(object sender, EventArgs e)
        {
            this.DisplayTemperatureCurve();		//实验停止时，绘制温度曲线
        }    
    }
}