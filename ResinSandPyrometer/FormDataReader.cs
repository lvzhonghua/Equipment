using ResinSandPyrometer.Lab;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ResinSandPyrometer
{
    public partial class FormDataReader : Form
    {
        private List<LabResult> labResults = new List<LabResult>();
        private Font fontOfTitle = new Font("宋体", 18, FontStyle.Bold);
        private Font fontOfBold = new Font("宋体", 12, FontStyle.Bold);
        private Font fontOfNormal = new Font("宋体", 12);
        private float averageValue = 0;
        private string strAverageValue = string.Empty;
        private Image imageOfCurves = null;

        public FormDataReader()
        {
            InitializeComponent();
        }

        private void btnOpenFiles_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "文本文件|*.txt";

            if (dlgOpen.ShowDialog() != DialogResult.OK) return;

            string fileName = dlgOpen.FileName;

            string folder = Path.GetDirectoryName(fileName);
            string shortFileName = Path.GetFileName(fileName);
            string samePart = shortFileName.Substring(0, shortFileName.LastIndexOf("-"));

            int resultCount = 0;

            string[] fileNames = Directory.GetFiles(folder, "*.txt");
            this.labResults.Clear();

            foreach (string fileName_Temp in fileNames)
            {
                if(fileName_Temp.Contains(samePart) == false) continue;

                resultCount++;

                LabResult labResult = new LabResult(fileName_Temp);
                LabResultHelper.ReadLabResult(ref labResult, fileName_Temp);
                this.labResults.Add(labResult);
            }

            this.labResults.Sort(new PathComparer());

            this.ListLabResults(this.labResults);

            this.ShowLabResults(this.labResults);
        }

        private float GetAverageValue(List<LabResult> labResults)
        {
            float average = 0;
            float sum = 0;
            float maxValue = float.MinValue;
            float minValue = float.MaxValue;

            for (int index = 0; index < labResults.Count; index++)
            {
                sum += labResults[index].Value;
                if (maxValue < labResults[index].Value) maxValue = labResults[index].Value;
                if (minValue > labResults[index].Value) minValue = labResults[index].Value;
            }

            if (labResults.Count == 0)
            {
                average = 0;
            }
            else if (labResults.Count <= 2)
            {
                average = sum / labResults.Count;
            }
            else if(labResults.Count > 2)
            {
                average = (sum - maxValue - minValue) / (labResults.Count - 2);
            }

            return average;
        }

        private void ListLabResults(List<LabResult> labResults)
        {
            this.lstData.Items.Clear();

            LabResult resultOfFirst = labResults.First();

            this.lstData.Items.Add($"实验类型：{resultOfFirst.Type}");
            this.lstData.Items.Add($"实验人员：{resultOfFirst.Tester}");
            this.lstData.Items.Add($"实验时间：{resultOfFirst.DateTime}");
            this.lstData.Items.Add($"试样编号：{resultOfFirst.SampleID}");
            this.lstData.Items.Add($"试样名称：{resultOfFirst.SampleName}");
            this.lstData.Items.Add($"试样尺寸：{resultOfFirst.SampleSize}");
            this.lstData.Items.Add($"设定温度：{resultOfFirst.TargetTemperature}");
            this.lstData.Items.Add($"来样单位：{resultOfFirst.Factory}");
            this.lstData.Items.Add("  ");

            for (int index = 0; index < labResults.Count; index++)
            {
                this.lstData.Items.Add($"实验编号：{labResults[index].RepeatNumber}");

                switch (labResults[index].Type)
                {
                    case LabType.高温抗压强度试验:
                        this.lstData.Items.Add($"抗压强度：{labResults[index].Value} KPa"); //抗压强度：XXXX KPa
                        this.lstData.Items.Add($"对应时间：{labResults[index].MaxValueTime} S"); //对应时间：XXX S
                        this.lstData.Items.Add($"保温时间：{labResults[index].HoldingTime} S"); //保温时间：XX S

                        break;
                    case LabType.高温膨胀力试验:
                        this.lstData.Items.Add($"膨胀力值：{labResults[index].Value} N"); //膨胀力值：XXXX N
                        this.lstData.Items.Add($"对应时间：{labResults[index].MaxValueTime} S");//对应时间：XXX S
                        this.lstData.Items.Add($"预载荷值：{labResults[index].PreloadValue} N");//预载荷值：XX N

                        break;
                    case LabType.耐高温时间试验:
                        this.lstData.Items.Add($"对应时间：{labResults[index].Value} S");//对应时间：XXX S
                        this.lstData.Items.Add($"预设强度：{labResults[index].PresetIntensity} MPa");//设定强度：XXXX MPa

                        break;
                    case LabType.高温急热膨胀率试验:
                        this.lstData.Items.Add($"最大膨胀率：{labResults[index].Value} %");//最大膨胀率：XXXX %
                        this.lstData.Items.Add($"最大膨胀率时间：{labResults[index].MaxValueTime} S");//最大膨胀率时间：XXXX S

                        break;
                }

                this.lstData.Items.Add("  ");
            }

            this.averageValue = this.GetAverageValue(labResults);

            switch (resultOfFirst.Type)
            {
                case LabType.高温抗压强度试验:
                    this.strAverageValue = $"平均值：{this.averageValue} KPa";
                    break;
                case LabType.高温膨胀力试验:
                    this.strAverageValue = $"平均值：{this.averageValue} N";
                    break;
                case LabType.耐高温时间试验:
                    this.strAverageValue = $"平均值：{this.averageValue} S";
                    break;
                case LabType.高温急热膨胀率试验:
                    this.strAverageValue = $"平均值：{this.averageValue} %";
                    break;
            }

            this.lstData.Items.Add(this.strAverageValue);
        }

        private void ShowLabResults(List<LabResult> labResults)
        {
            this.chartData.Series.Clear();

            string serieName = string.Empty;
            string titleOfAxisY = string.Empty;

            LabResult resultOfFirst = labResults.First();

            switch (resultOfFirst.Type)
            {
                case LabType.高温抗压强度试验:
                    this.chartData.Titles[0].Text = $"抗压强度-时间曲线";
                    serieName = "抗压强度";
                    titleOfAxisY = "抗压强度（KPa）";
                    break;
                case LabType.高温膨胀力试验:
                    this.chartData.Titles[0].Text = $"膨胀力-时间曲线";
                    serieName = "膨胀力";
                    titleOfAxisY = "膨胀力（N）";
                    break;
                case LabType.耐高温时间试验:
                    this.chartData.Titles[0].Text = $"平衡强度-时间曲线";
                    serieName = "平衡强度";
                    titleOfAxisY = "平衡强度（MPa）";
                    break;
                case LabType.高温急热膨胀率试验:
                    this.chartData.Titles[0].Text = $"膨胀率-时间曲线";
                    serieName = "膨胀率";
                    titleOfAxisY = "膨胀率（%）";
                    break;
            }

            for (int index = 0; index < labResults.Count; index++)
            {
                this.chartData.ChartAreas[0].AxisY.Title = titleOfAxisY;
                var serie = this.chartData.Series.Add($"{serieName}-{labResults[index].RepeatNumber}");
                serie.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

                foreach (PointF point in labResults[index].LinePoints)
                {
                    serie.Points.AddXY(point.X, point.Y);
                }
            }

        }

        private void GetCurvesImage()
        {
            MemoryStream stream = new MemoryStream();
            this.chartData.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            stream.Position = 0;

            this.imageOfCurves = Image.FromStream(stream);

            stream.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.labResults.Count == 0)
            {  
                MessageBox.Show("无数据要打印，请先打开数据文件","提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.GetCurvesImage();

            this.dlgPrintPreview.Width = 1500;
            this.dlgPrintPreview.Height = 1000;
            this.dlgPrintPreview.ShowDialog();

        }

        private void DrawCurvesImage(Graphics graphics, Rectangle rectOfBounds)
        {
            RectangleF rectOfCurvesImage = new RectangleF();
            rectOfCurvesImage.Width = rectOfBounds.Width - 1 * 2;
            rectOfCurvesImage.Height = (float)rectOfCurvesImage.Width * this.imageOfCurves.Height / this.imageOfCurves.Width;
            rectOfCurvesImage.X = rectOfBounds.X + 1;
            rectOfCurvesImage.Y = rectOfBounds.Bottom - rectOfCurvesImage.Height;

            graphics.DrawImage(this.imageOfCurves, rectOfCurvesImage);
        }

        private void DrawLabResults(Graphics graphics, Rectangle rectOfBounds)
        {
            StringFormat stringFormat = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            string title = "树脂砂高温测试仪检测报告";
            SizeF sizeOfTitle = graphics.MeasureString(title, this.fontOfTitle);
            RectangleF rectOfTitle = new RectangleF();
            rectOfTitle.X = rectOfBounds.X;
            rectOfTitle.Y = rectOfBounds.Y;
            rectOfTitle.Width = rectOfBounds.Width;
            rectOfTitle.Height = sizeOfTitle.Height;

            graphics.DrawString(title, this.fontOfTitle, Brushes.Black, rectOfTitle, stringFormat);

            RectangleF rectOfFrame = new RectangleF();
            rectOfFrame.X = rectOfBounds.X;
            rectOfFrame.Y = rectOfTitle.Bottom + 5;
            rectOfFrame.Width = rectOfBounds.Width;
            rectOfFrame.Height = rectOfBounds.Bottom - rectOfFrame.Y;
            graphics.DrawRectangle(Pens.Black, Rectangle.Round(rectOfFrame));

            LabResult resultOfFirst = this.labResults.First();

            int spacing = 4;
            float xOffset = rectOfFrame.X;
            float yOffset = rectOfFrame.Y;
            
            float xPer = rectOfFrame.Width / 4f;

            //第一行：试样名称 试样编号 试验温度 来样单位
            string sampleName_Label = "试样名称:";
            SizeF sizeOfSampleName_Label = graphics.MeasureString(sampleName_Label, this.fontOfBold);
            RectangleF rectOfSampleName_Label = new RectangleF();
            rectOfSampleName_Label.X = xOffset;
            rectOfSampleName_Label.Y = yOffset;
            rectOfSampleName_Label.Width = sizeOfSampleName_Label.Width + spacing * 2;
            rectOfSampleName_Label.Height = sizeOfSampleName_Label.Height + spacing * 2;
            graphics.DrawString(sampleName_Label, this.fontOfBold, Brushes.Black, rectOfSampleName_Label, stringFormat);

            string sampleName = resultOfFirst.SampleName;
            SizeF sizeOfSampleName = graphics.MeasureString(sampleName, this.fontOfNormal);
            RectangleF rectOfSampleName = new RectangleF();
            rectOfSampleName.X = rectOfSampleName_Label.Right;
            rectOfSampleName.Y = yOffset;
            rectOfSampleName.Width = sizeOfSampleName.Width + spacing * 2;
            rectOfSampleName.Height = rectOfSampleName_Label.Height;
            graphics.DrawString(sampleName, this.fontOfNormal, Brushes.Black, rectOfSampleName, stringFormat);

            xOffset += xPer;
            string sampleID_Label = "试样编号:";
            SizeF sizeOfSampleID_Label = graphics.MeasureString(sampleID_Label, this.fontOfBold);
            RectangleF rectOfSampleID_Label = new RectangleF();
            rectOfSampleID_Label.X = xOffset;
            rectOfSampleID_Label.Y = yOffset;
            rectOfSampleID_Label.Width = sizeOfSampleID_Label.Width + spacing * 2;
            rectOfSampleID_Label.Height = sizeOfSampleID_Label.Height + spacing * 2;
            graphics.DrawString(sampleID_Label, this.fontOfBold, Brushes.Black, rectOfSampleID_Label, stringFormat);

            string sampleID = resultOfFirst.SampleID;
            SizeF sizeOfSampleID = graphics.MeasureString(sampleID, this.fontOfNormal);
            RectangleF rectOfSampleID = new RectangleF();
            rectOfSampleID.X = rectOfSampleID_Label.Right;
            rectOfSampleID.Y = yOffset;
            rectOfSampleID.Width = sizeOfSampleID.Width + spacing * 2;
            rectOfSampleID.Height = rectOfSampleID_Label.Height;
            graphics.DrawString(sampleID, this.fontOfNormal, Brushes.Black, rectOfSampleID, stringFormat);

            xOffset += xPer;
            string targetTemperature_Label = "试验温度:";
            SizeF sizeOfTargeTemperature_Label = graphics.MeasureString(targetTemperature_Label, this.fontOfBold);
            RectangleF rectOfTargetTemperature_Label = new RectangleF();
            rectOfTargetTemperature_Label.X = xOffset;
            rectOfTargetTemperature_Label.Y = yOffset;
            rectOfTargetTemperature_Label.Width = sizeOfTargeTemperature_Label.Width + spacing * 2;
            rectOfTargetTemperature_Label.Height = sizeOfTargeTemperature_Label.Height + spacing * 2;
            graphics.DrawString(targetTemperature_Label, this.fontOfBold, Brushes.Black, rectOfTargetTemperature_Label, stringFormat);

            string targetTemperature = resultOfFirst.TargetTemperature;
            SizeF sizeOfTargetTemperature = graphics.MeasureString(targetTemperature, this.fontOfNormal);
            RectangleF rectOfTargetTemperature = new RectangleF();
            rectOfTargetTemperature.X = rectOfTargetTemperature_Label.Right;
            rectOfTargetTemperature.Y = yOffset;
            rectOfTargetTemperature.Width = sizeOfTargetTemperature.Width + spacing * 2;
            rectOfTargetTemperature.Height = rectOfTargetTemperature_Label.Height;
            graphics.DrawString(targetTemperature, this.fontOfNormal, Brushes.Black, rectOfTargetTemperature, stringFormat);

            xOffset += xPer;
            string factory_Label = "来样单位:";
            SizeF sizeOfFactory_Label = graphics.MeasureString(factory_Label, this.fontOfBold);
            RectangleF rectOfFactory_Label = new RectangleF();
            rectOfFactory_Label.X = xOffset;
            rectOfFactory_Label.Y = yOffset;
            rectOfFactory_Label.Width = sizeOfFactory_Label.Width + spacing * 2;
            rectOfFactory_Label.Height = sizeOfFactory_Label.Height + spacing * 2;
            graphics.DrawString(factory_Label, this.fontOfBold, Brushes.Black, rectOfFactory_Label, stringFormat);

            string factory = resultOfFirst.Factory;
            SizeF sizeOfFactory = graphics.MeasureString(factory, this.fontOfNormal);
            RectangleF rectOfFactory = new RectangleF();
            rectOfFactory.X = rectOfFactory_Label.Right;
            rectOfFactory.Y = yOffset;
            rectOfFactory.Width = sizeOfFactory.Width + spacing * 2;
            rectOfFactory.Height = rectOfFactory_Label.Height;
            graphics.DrawString(factory, this.fontOfNormal, Brushes.Black, rectOfFactory, stringFormat);

            //第二行：试样尺寸 实验时间 测试人员
            xOffset = rectOfFrame.X;
            yOffset += rectOfFactory.Height;

            graphics.DrawLine(Pens.Black, rectOfFrame.Left, yOffset, rectOfFrame.Right, yOffset);

            string sampleSize_Label = "试样尺寸:";
            SizeF sizeOfSampleSize_Label = graphics.MeasureString(sampleSize_Label, this.fontOfBold);
            RectangleF rectOfSampleSize_Label = new RectangleF();
            rectOfSampleSize_Label.X = xOffset;
            rectOfSampleSize_Label.Y = yOffset;
            rectOfSampleSize_Label.Width = sizeOfSampleSize_Label.Width + spacing * 2;
            rectOfSampleSize_Label.Height = sizeOfSampleSize_Label.Height + spacing * 2;
            graphics.DrawString(sampleSize_Label, this.fontOfBold, Brushes.Black, rectOfSampleSize_Label, stringFormat);

            string sampleSize = resultOfFirst.SampleSize;
            SizeF sizeOfSampleSize = graphics.MeasureString(sampleSize, this.fontOfNormal);
            RectangleF rectOfSampleSize = new RectangleF();
            rectOfSampleSize.X = rectOfSampleName_Label.Right;
            rectOfSampleSize.Y = yOffset;
            rectOfSampleSize.Width = sizeOfSampleSize.Width + spacing * 2;
            rectOfSampleSize.Height = rectOfSampleSize_Label.Height;
            graphics.DrawString(sampleSize, this.fontOfNormal, Brushes.Black, rectOfSampleSize, stringFormat);

            xOffset += xPer;
            string dateTime_Label = "实验时间:";
            SizeF sizeOfDateTime_Label = graphics.MeasureString(dateTime_Label, this.fontOfBold);
            RectangleF rectOfDateTime_Label = new RectangleF();
            rectOfDateTime_Label.X = xOffset;
            rectOfDateTime_Label.Y = yOffset;
            rectOfDateTime_Label.Width = sizeOfDateTime_Label.Width + spacing * 2;
            rectOfDateTime_Label.Height = sizeOfDateTime_Label.Height + spacing * 2;
            graphics.DrawString(dateTime_Label, this.fontOfBold, Brushes.Black, rectOfDateTime_Label, stringFormat);

            string dateTime = resultOfFirst.DateTime;
            SizeF sizeOfDateTime = graphics.MeasureString(dateTime, this.fontOfNormal);
            RectangleF rectOfDateTime = new RectangleF();
            rectOfDateTime.X = rectOfDateTime_Label.Right;
            rectOfDateTime.Y = yOffset;
            rectOfDateTime.Width = sizeOfDateTime.Width + spacing * 2;
            rectOfDateTime.Height = rectOfDateTime_Label.Height;
            graphics.DrawString(dateTime, this.fontOfNormal, Brushes.Black, rectOfDateTime, stringFormat);

            xOffset += xPer + xPer;
            string tester_Label = "实验人员:";
            SizeF sizeOfTester_Label = graphics.MeasureString(tester_Label, this.fontOfBold);
            RectangleF rectOfTester_Label = new RectangleF();
            rectOfTester_Label.X = xOffset;
            rectOfTester_Label.Y = yOffset;
            rectOfTester_Label.Width = sizeOfTester_Label.Width + spacing * 2;
            rectOfTester_Label.Height = sizeOfTester_Label.Height + spacing * 2;
            graphics.DrawString(tester_Label, this.fontOfBold, Brushes.Black, rectOfTester_Label, stringFormat);

            string tester = resultOfFirst.Tester;
            SizeF sizeOfTester = graphics.MeasureString(tester, this.fontOfNormal);
            RectangleF rectOfTester = new RectangleF();
            rectOfTester.X = rectOfTester_Label.Right;
            rectOfTester.Y = yOffset;
            rectOfTester.Width = sizeOfTester.Width + spacing * 2;
            rectOfTester.Height = rectOfTester_Label.Height;
            graphics.DrawString(tester, this.fontOfNormal, Brushes.Black, rectOfTester, stringFormat);

            //第三行：实验：实验类型
            xOffset = rectOfFrame.X;
            yOffset += rectOfTester.Height;

            graphics.DrawLine(Pens.Black, rectOfFrame.Left, yOffset, rectOfFrame.Right, yOffset);

            string labType_Label = "实验类型:";
            SizeF sizeOfLabType_Label = graphics.MeasureString(labType_Label, this.fontOfBold);
            RectangleF rectOfLabType_Label = new RectangleF();
            rectOfLabType_Label.X = xOffset;
            rectOfLabType_Label.Y = yOffset;
            rectOfLabType_Label.Width = sizeOfLabType_Label.Width + spacing * 2;
            rectOfLabType_Label.Height = sizeOfLabType_Label.Height + spacing * 2;
            graphics.DrawString(labType_Label, this.fontOfBold, Brushes.Black, rectOfLabType_Label, stringFormat);

            string labType = resultOfFirst.Type.ToString();
            SizeF sizeOfLabType = graphics.MeasureString(labType, this.fontOfNormal);
            RectangleF rectOfLabType = new RectangleF();
            rectOfLabType.X = rectOfLabType_Label.Right;
            rectOfLabType.Y = yOffset;
            rectOfLabType.Width = sizeOfLabType.Width + spacing * 2;
            rectOfLabType.Height = rectOfLabType_Label.Height;
            graphics.DrawString(labType, this.fontOfNormal, Brushes.Black, rectOfLabType, stringFormat);

            //从第四行开始：实验序号 试验值 最大值时间
            xOffset = rectOfFrame.X;
            yOffset += rectOfLabType.Height;

            graphics.DrawLine(Pens.Black, rectOfFrame.Left, yOffset, rectOfFrame.Right, yOffset);

            string valueName = string.Empty;
            switch (resultOfFirst.Type)
            {
                case LabType.高温抗压强度试验:
                    valueName = "抗压强度（KPa）";
                    break;
                case LabType.高温膨胀力试验:
                    valueName = "膨胀力（N）";
                    break;
                case LabType.耐高温时间试验:
                    valueName = "维持时间（秒）";
                    break;
                case LabType.高温急热膨胀率试验:
                    valueName = "膨胀率（%）";
                    break;
            }

            float gridWidth = rectOfBounds.Width / 3f;
            float rowHeight = graphics.MeasureString (valueName, this.fontOfBold).Height + spacing * 2;

            for (int rowIndex = 1; rowIndex <= 1 + this.labResults.Count + 1; rowIndex++)
            {
                PointF a = new PointF(xOffset, yOffset + rowHeight * rowIndex);
                PointF b = new PointF(xOffset + rectOfFrame.Width, yOffset + rowHeight * rowIndex);
                graphics.DrawLine(Pens.Gray, a, b);
            }

            for (int columnIndex = 1; columnIndex < 4; columnIndex++)
            {
                PointF a = new PointF(xOffset + gridWidth * columnIndex, yOffset);

                PointF b = new PointF(xOffset + gridWidth * columnIndex, yOffset + rowHeight * (this.labResults.Count + 2));
                if (columnIndex == 2)
                {
                    b = new PointF(xOffset + gridWidth * columnIndex, yOffset + rowHeight * (this.labResults.Count + 1));
                }
               
                graphics.DrawLine(Pens.Gray, a, b);
            }

            RectangleF rectOfRepeatNumber = new RectangleF();
            rectOfRepeatNumber.X = xOffset;
            rectOfRepeatNumber.Y = yOffset;
            rectOfRepeatNumber.Width = gridWidth;
            rectOfRepeatNumber.Height = rowHeight;
            graphics.DrawString("试验序号", this.fontOfBold, Brushes.Black, rectOfRepeatNumber, stringFormat);

            RectangleF rectOfValue = new RectangleF();
            rectOfValue.X = xOffset + gridWidth;
            rectOfValue.Y = yOffset;
            rectOfValue.Width = gridWidth;
            rectOfValue.Height = rowHeight;
            graphics.DrawString(valueName, this.fontOfBold, Brushes.Black, rectOfValue, stringFormat);

            RectangleF rectOfMaxValueTime = new RectangleF();
            rectOfMaxValueTime.X = xOffset + gridWidth + gridWidth;
            rectOfMaxValueTime.Y = yOffset;
            rectOfMaxValueTime.Width = gridWidth;
            rectOfMaxValueTime.Height = rowHeight;
            if (valueName == "维持时间（秒）")
            {
                graphics.DrawString("设定强度（MPa）", this.fontOfBold, Brushes.Black, rectOfMaxValueTime, stringFormat);
            }
            else
            {
                graphics.DrawString("最大值时间（秒）", this.fontOfBold, Brushes.Black, rectOfMaxValueTime, stringFormat);
            }

            for (int index = 0; index < this.labResults.Count; index++)
            {
                rectOfRepeatNumber.X = xOffset;
                rectOfRepeatNumber.Y = yOffset + rowHeight * (index + 1);
                rectOfRepeatNumber.Width = gridWidth;
                rectOfRepeatNumber.Height = rowHeight;
                graphics.DrawString(this.labResults[index].RepeatNumber, this.fontOfNormal, Brushes.Black, rectOfRepeatNumber, stringFormat);

                rectOfValue.X = xOffset + gridWidth;
                rectOfValue.Y = yOffset + rowHeight * (index + 1);
                rectOfValue.Width = gridWidth;
                rectOfValue.Height = rowHeight;
                graphics.DrawString($"{this.labResults[index].Value}", this.fontOfNormal, Brushes.Black, rectOfValue, stringFormat);

                if (valueName == "维持时间（秒）")
                {
                    rectOfMaxValueTime.X = xOffset + gridWidth + gridWidth;
                    rectOfMaxValueTime.Y = yOffset + rowHeight * (index + 1);
                    rectOfMaxValueTime.Width = gridWidth;
                    rectOfMaxValueTime.Height = rowHeight;
                    graphics.DrawString($"{this.labResults[index].PresetIntensity}", this.fontOfNormal, Brushes.Black, rectOfMaxValueTime, stringFormat);
                }
                else
                {
                    rectOfMaxValueTime.X = xOffset + gridWidth + gridWidth;
                    rectOfMaxValueTime.Y = yOffset + rowHeight * (index + 1);
                    rectOfMaxValueTime.Width = gridWidth;
                    rectOfMaxValueTime.Height = rowHeight;
                    graphics.DrawString($"{this.labResults[index].MaxValueTime}", this.fontOfNormal, Brushes.Black, rectOfMaxValueTime, stringFormat);
                }
            }
            //平均值
            RectangleF rectOfAverageValue_Label = new RectangleF();
            rectOfAverageValue_Label.X = xOffset;
            rectOfAverageValue_Label.Y = yOffset + rowHeight * (this.labResults.Count + 1);
            rectOfAverageValue_Label.Width = gridWidth;
            rectOfAverageValue_Label.Height = rowHeight;
            graphics.DrawString("平均值：", this.fontOfBold, Brushes.Black, rectOfAverageValue_Label, stringFormat);
            
            RectangleF rectOfAverageValue = new RectangleF();
            rectOfAverageValue.X = rectOfAverageValue_Label.Right;
            rectOfAverageValue.Y = yOffset + rowHeight * (this.labResults.Count + 1);
            rectOfAverageValue.Width = gridWidth * 2;
            rectOfAverageValue.Height = rowHeight;

            if (this.labResults.Count < 3)
            {
                graphics.DrawString($"{this.averageValue}", this.fontOfBold, Brushes.Black, rectOfAverageValue, stringFormat);
            }
            else
            {
                graphics.DrawString($"{this.averageValue} （去掉最大最小值后求平均）", this.fontOfBold, Brushes.Black, rectOfAverageValue, stringFormat);
            }
        }

        private void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int padding = 40;
            Rectangle rectOfBounds = e.PageBounds;
            rectOfBounds.X = rectOfBounds.X + padding;
            rectOfBounds.Y = rectOfBounds.Y + padding;
            rectOfBounds.Width = rectOfBounds.Width - padding * 2;
            rectOfBounds.Height = rectOfBounds.Height - padding * 2;

            this.DrawLabResults(e.Graphics, rectOfBounds);

            this.DrawCurvesImage(e.Graphics, rectOfBounds);
        }
    }
}
