using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ResinSandPyrometer.Lab;

namespace ResinSandPyrometer
{
    public partial class FormDataReader : Form
    {
        private int lineCount = 0;
        private int pathCount = 0;

        private Font myBigFont = new Font(new FontFamily("宋体"), 16);

        private Font myFont = new Font(new FontFamily("宋体"), 12);

        private Pen myPen = new Pen(Brushes.Black);
        private string[] myString = new string[35];
        private int testType = 0;


        public FormDataReader()
        {
            InitializeComponent();

            PaperSize ps = null;

            foreach (PaperSize p in this.myPrintDoc.PrinterSettings.PaperSizes)
            {
                if (p.PaperName.Equals("A4"))
                    ps = p;
            }
            this.myPrintDoc.DefaultPageSettings.PaperSize = ps;
        }

        private void FormOpenFile_Load(object sender, EventArgs e)
        {
            this.myPrintDoc.PrintPage += new PrintPageEventHandler(this.myPrintDoc_PrintPage);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();

            openDlg.Filter = "文本格式|*.txt";

            DialogResult dlgResult = openDlg.ShowDialog();
            
            if (dlgResult != DialogResult.OK) return;

            string path = openDlg.FileName;

            string folder = Path.GetDirectoryName(path);
            string shortFileName = Path.GetFileName(path);
            string samePart = shortFileName.Substring(0, shortFileName.LastIndexOf("-"));

            this.pathCount = 0;

            string[] fileNames = Directory.GetFiles(folder, "*.txt");
            
            List<LabResult> chartResults = new List<LabResult>();

            foreach (string fileName in fileNames)
            {
                if(fileName.Contains(samePart) == false) continue;

                this.pathCount++;

                LabResult chartResult = new LabResult(fileName);
                chartResults.Add(chartResult);
            }

            chartResults.Sort(new PathComparer());

            //this.chart.Series[0].Points.Clear();
            //this.chart.Series[1].Points.Clear();
            //this.chart.Series[2].Points.Clear();

            this.infoListBox.Items.Clear();

            foreach (LabResult chartResult in chartResults)
            {
                StreamReader streamReader = new StreamReader(chartResult.Path, Encoding.UTF8);

                shortFileName = Path.GetFileName(chartResult.Path);
                string fileSort = shortFileName.Substring(shortFileName.LastIndexOf("-") + 1, (shortFileName.LastIndexOf(".") - shortFileName.LastIndexOf("-") - 1));

                string strLine = null;
                string str = null;
                float xPos = 0;
                float yPos = 0;
                this.lineCount = 0;

                if (fileSort == "1")    //第一个文件
                {
                    while ((strLine = streamReader.ReadLine()) != null)
                    {
                        if (strLine.Contains("Info"))
                        {
                            strLine = strLine.Substring(4);
                            this.infoListBox.Items.Add(strLine);

                            switch (this.lineCount)
                            {
                                case 0:
                                    strLine = strLine.Substring(5);
                                    if (strLine == "高温抗压强度试验")
                                        testType = 0;
                                    else if (strLine == "高温膨胀力试验")
                                        testType = 1;
                                    else if (strLine == "耐高温时间试验")
                                        testType = 2;
                                    else
                                        testType = 3;
                                    this.myString[lineCount] = strLine;
                                    break;
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                case 8:
                                    strLine = strLine.Substring(5);
                                    this.myString[lineCount] = strLine;
                                    break;
                                case 9:
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 3));
                                            this.myString[lineCount] = chartResult.ResultValue.ToString();
                                            break;
                                        case 1:
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 1));
                                            this.myString[lineCount] = chartResult.ResultValue.ToString();
                                            break;
                                        case 2:
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 1));
                                            this.myString[lineCount] = chartResult.ResultValue.ToString();
                                            break;
                                        case 3:
                                            strLine = strLine.Substring(1);
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 1));
                                            this.myString[lineCount] = chartResult.ResultValue.ToString();
                                            break;
                                    }
                                    break;
                                case 10:
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                        case 1:
                                            this.myString[lineCount] = strLine.Substring(0, strLine.Length - 1);
                                            break;
                                        case 2:
                                            this.myString[lineCount] = strLine.Substring(0, strLine.Length - 3);
                                            break;
                                        case 3:
                                            strLine = strLine.Substring(3);
                                            this.myString[lineCount] = strLine.Substring(0, strLine.Length - 1);
                                            break;
                                    }
                                    break;
                                case 11:
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                        case 1:
                                            this.myString[lineCount] = strLine.Substring(0, strLine.Length - 1);
                                            break;
                                        case 2:
                                        case 3:
                                            break;
                                    }
                                    break;
                            }
                            this.lineCount++;
                        }
                        else if (strLine.Contains("X"))
                        {
                            str = strLine.Substring(1, strLine.IndexOf(",") - 1);
                            xPos = Convert.ToSingle(str);
                            str = strLine.Substring(strLine.IndexOf("Y") + 1);
                            yPos = Convert.ToSingle(str);
                            PointF point = new PointF(xPos, yPos);
                            chartResult.LinePoints.Add(point);
                        }
                    }
                }
                else
                {
                    this.infoListBox.Items.Add("\r\n");

                    while ((strLine = streamReader.ReadLine()) != null)
                    {
                        if (strLine.Contains("Info"))
                        {
                            strLine = strLine.Substring(4);
                            switch (lineCount)
                            {
                                case 0:
                                    strLine = strLine.Substring(5);
                                    if (strLine == "高温抗压强度试验")
                                        testType = 0;
                                    else if (strLine == "高温膨胀力试验")
                                        testType = 1;
                                    else if (strLine == "耐高温时间试验")
                                        testType = 2;
                                    else
                                        testType = 3;

                                    break;
                                case 8:
                                    this.infoListBox.Items.Add(strLine);
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                        case 1:
                                            this.myString[12] = strLine;
                                            break;
                                        case 2:
                                        case 3:
                                            this.myString[11] = strLine;
                                            break;
                                    }
                                    break;
                                case 9:
                                    this.infoListBox.Items.Add(strLine);
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 3));
                                            this.myString[13] = chartResult.ResultValue.ToString();
                                            break;
                                        case 1:
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 1));
                                            this.myString[13] = chartResult.ResultValue.ToString();
                                            break;
                                        case 2:
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 1));
                                            this.myString[12] = chartResult.ResultValue.ToString();
                                            break;
                                        case 3:
                                            strLine = strLine.Substring(1);
                                            chartResult.ResultValue = Convert.ToSingle(strLine.Substring(0, strLine.Length - 1));
                                            this.myString[12] = chartResult.ResultValue.ToString();
                                            break;
                                    }
                                    break;
                                case 10:
                                    this.infoListBox.Items.Add(strLine);
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                        case 1:
                                            this.myString[14] = strLine.Substring(0, strLine.Length - 1);
                                            break;
                                        case 2:
                                            this.myString[13] = strLine.Substring(0, strLine.Length - 3);
                                            break;
                                        case 3:
                                            strLine = strLine.Substring(3);
                                            this.myString[13] = strLine.Substring(0, strLine.Length - 1);
                                            break;
                                    }
                                    break;
                                case 11:
                                    this.infoListBox.Items.Add(strLine);
                                    strLine = strLine.Substring(5);
                                    switch (this.testType)
                                    {
                                        case 0:
                                        case 1:
                                            this.myString[15] = strLine.Substring(0, strLine.Length - 1);
                                            break;
                                        case 2:
                                        case 3:
                                            break;
                                    }
                                    break;
                            }
                            this.lineCount++;
                        }
                        else if (strLine.Contains("X"))
                        {
                            str = strLine.Substring(1, strLine.IndexOf(",") - 1);
                            xPos = Convert.ToSingle(str);
                            str = strLine.Substring(strLine.IndexOf("Y") + 1);
                            yPos = Convert.ToSingle(str);
                            PointF point = new PointF(xPos, yPos);
                            chartResult.LinePoints.Add(point);

                        }
                    }
                }
                streamReader.Close();
            }

            float sum = 0;
            float avg = 0;

            List<float> results = new List<float>();

            foreach (LabResult chartResult in chartResults)
            {
                sum += chartResult.ResultValue;
                //this.infoListBox.Items.Add(cr.resultValue);
                results.Add(chartResult.ResultValue);
            }
            results.Sort();

            for (int index = 1; index <= this.pathCount; index++)
            {
                if (index == 1)
                {
                    foreach (LabResult cr in chartResults)
                    {
                        if (cr.ResultValue == results[0])
                        {
                            foreach (PointF point in cr.LinePoints)
                            {
                                this.chart.Series[0].Points.AddXY(point.X, point.Y);
                            }
                        }
                    }
                    continue;
                }



            }

            if (pathCount == 1)
            {

                
            }
            else if (pathCount == 2)
            {
                avg = sum / chartResults.Count;

                switch (this.testType)
                {
                    case 0:
                        this.myString[16] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "KPa");
                        break;
                    case 1:
                        this.myString[16] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "N");
                        break;
                    case 2:
                        this.myString[14] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "S");
                        break;
                    case 3:
                        this.myString[14] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "%");
                        break;
                }

                foreach (LabResult cr in chartResults)
                {
                    if (cr.ResultValue == results[0])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[0].Points.AddXY(point.X, point.Y);
                        }
                    }
                    if (cr.ResultValue == results[1])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[1].Points.AddXY(point.X, point.Y);
                        }
                    }

                }


            }
            else if (pathCount == 3)
            {
                avg = sum / chartResults.Count;

                switch (this.testType)
                {
                    case 0:
                        this.myString[20] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "KPa");
                        break;
                    case 1:
                        this.myString[20] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "N");
                        break;
                    case 2:
                        this.myString[17] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "S");
                        break;
                    case 3:
                        this.myString[17] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "%");
                        break;
                }

                foreach (LabResult cr in chartResults)
                {
                    if (cr.ResultValue == results[0])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[0].Points.AddXY(point.X, point.Y);
                        }
                    }

                    if (cr.ResultValue == results[1])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[1].Points.AddXY(point.X, point.Y);
                        }
                    }

                    if (cr.ResultValue == results[2])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[2].Points.AddXY(point.X, point.Y);
                        }
                    }
                }

            }
            else if (pathCount == 4)
            {

                avg = (sum - results[0] - results[3]) / (chartResults.Count - 2);
                switch (this.testType)
                {
                    case 0:
                        this.myString[24] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "KPa");
                        break;
                    case 1:
                        this.myString[24] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "N");
                        break;
                    case 2:
                        this.myString[20] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "S");
                        break;
                    case 3:
                        this.myString[20] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "%");
                        break;
                }

                foreach (LabResult cr in chartResults)
                {
                    if (cr.ResultValue == results[1])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[0].Points.AddXY(point.X, point.Y);
                        }
                    }

                    if (cr.ResultValue == results[2])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[1].Points.AddXY(point.X, point.Y);
                        }
                    }

                }


            }

            else if (pathCount == 5)
            {

                avg = (sum - results[0] - results[4]) / (chartResults.Count - 2);

                switch (this.testType)
                {
                    case 0:
                        this.myString[28] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "KPa");
                        break;
                    case 1:
                        this.myString[28] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "N");
                        break;
                    case 2:
                        this.myString[23] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "S");
                        break;
                    case 3:
                        this.myString[23] = avg.ToString();
                        this.infoListBox.Items.Add("平均值：" + avg + "%");
                        break;
                }
                foreach (LabResult cr in chartResults)
                {
                    if (cr.ResultValue == results[1])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[0].Points.AddXY(point.X, point.Y);
                        }
                    }

                    if (cr.ResultValue == results[2])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[1].Points.AddXY(point.X, point.Y);
                        }
                    }
                    if (cr.ResultValue == results[3])
                    {
                        foreach (PointF point in cr.LinePoints)
                        {
                            this.chart.Series[2].Points.AddXY(point.X, point.Y);
                        }

                    }
                }
            }

            this.chart.Titles.Clear();
            this.chart.Series[0].Name = "  ";
            this.chart.Series[1].Name = " ";
            this.chart.Series[2].Name = "    ";
            switch (this.testType)
            {
                case 0:
                    this.chart.Series[0].Name = "抗压强度-1";
                    this.chart.ChartAreas[0].AxisY.Title = "抗压强度（Kpa）";
                    this.chart.Titles.Add("抗压强度--时间曲线");
                    this.chart.Series[1].Name = "抗压强度-2";

                    this.chart.Series[2].Name = "抗压强度-3";
                    break;
                case 1:
                    this.chart.Series[0].Name = "膨胀力-1";
                    this.chart.ChartAreas[0].AxisY.Title = "膨胀力（N）";
                    this.chart.Titles.Add("膨胀力--时间曲线");
                    this.chart.Series[1].Name = "膨胀力-2";
                    this.chart.Series[2].Name = "膨胀力-3";
                    break;
                case 2:
                    this.chart.Series[0].Name = "平衡强度-1";
                    this.chart.ChartAreas[0].AxisY.Title = "平衡强度（MPa）";
                    this.chart.Titles.Add("平衡强度--时间曲线");
                    this.chart.Series[1].Name = "平衡强度-2";
                    this.chart.Series[2].Name = "平衡强度-3";
                    break;
                case 3:
                    this.chart.Series[0].Name = "膨胀率-1";
                    this.chart.ChartAreas[0].AxisY.Title = "膨胀率（%）";
                    this.chart.Titles.Add("膨胀率--时间曲线");
                    this.chart.Series[1].Name = "膨胀率-2";
                    this.chart.Series[2].Name = "膨胀率-3";
                    break;

            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.myPrintPreDoc.Document = this.myPrintDoc;
            DialogResult result = this.myPrintPreDoc.ShowDialog();
            if (result == DialogResult.OK) this.myPrintDoc.Print();
        }

        private void myPrintDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Bitmap bm = new Bitmap(this.chart.Width, this.chart.Height);
            Rectangle r = new Rectangle(0, 0, bm.Width, bm.Height);
            e.Graphics.DrawString("树脂砂高温测试仪检测报告", this.myBigFont, Brushes.Black, 300, 30);
            e.Graphics.DrawLine(this.myPen, 44, 60, 784, 60);
            e.Graphics.DrawLine(this.myPen, 44, 1150, 784, 1150);
            e.Graphics.DrawLine(this.myPen, 44, 1057, 784, 1057);
            e.Graphics.DrawLine(this.myPen, 44, 60, 44, 1150);
            e.Graphics.DrawLine(this.myPen, 784, 60, 784, 1150);
            e.Graphics.DrawLine(this.myPen, 44, 100, 784, 100);
            e.Graphics.DrawLine(this.myPen, 44, 140, 784, 140);

            e.Graphics.DrawLine(this.myPen, 44, 190, 784, 190);
            e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);

            e.Graphics.DrawString("试样名称:", this.myFont, Brushes.Black, 51, 74);
            e.Graphics.DrawString(this.myString[4], this.myFont, Brushes.Black, 125, 74);

            e.Graphics.DrawString("试样编号:", this.myFont, Brushes.Black, 251, 74);
            e.Graphics.DrawString(this.myString[3], this.myFont, Brushes.Black, 330, 74);

            e.Graphics.DrawString("测试温度:", this.myFont, Brushes.Black, 451, 74);
            e.Graphics.DrawString(this.myString[6], this.myFont, Brushes.Black, 525, 74);

            e.Graphics.DrawString("来样单位:", this.myFont, Brushes.Black, 591, 74);
            e.Graphics.DrawString(this.myString[7], this.myFont, Brushes.Black, 680, 74);
            e.Graphics.DrawString("实验人员:", this.myFont, Brushes.Black, 451, 114);
            e.Graphics.DrawString(this.myString[1], this.myFont, Brushes.Black, 525, 114);

            e.Graphics.DrawString("试样尺寸:", this.myFont, Brushes.Black, 51, 114);
            e.Graphics.DrawString(this.myString[5], this.myFont, Brushes.Black, 125, 114);
            e.Graphics.DrawString("实验时间:", this.myFont, Brushes.Black, 251, 114);
            e.Graphics.DrawString(this.myString[2], this.myFont, Brushes.Black, 330, 114);


            //e.Graphics.DrawLine(this.myPen, 44, 190, 229, 230);
            e.Graphics.DrawString("重复次数(次)", this.myFont, Brushes.Black, 46, 210);

            e.Graphics.DrawString("实验结果：", this.myBigFont, Brushes.Black, 51, 157);


            switch (this.testType)
            {
                case 0:
                    e.Graphics.DrawString("高温抗压强度实验", this.myFont, Brushes.Black, 200, 160);
                    e.Graphics.DrawString("抗压强度(KPa)", this.myFont, Brushes.Black, 235, 210);
                    e.Graphics.DrawString(this.myString[9], this.myFont, Brushes.Black, 235, 250);

                    e.Graphics.DrawString("对应时间(S)", this.myFont, Brushes.Black, 420, 210);
                    e.Graphics.DrawString(this.myString[10], this.myFont, Brushes.Black, 420, 250);
                    e.Graphics.DrawString("试样保温时间(S)", this.myFont, Brushes.Black, 605, 210);
                    e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 605, 250);
                    if (this.pathCount == 1)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 270);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 270);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 270);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 380, bm.Width - 50, bm.Height),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 2)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 350);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 350);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 350);


                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 235, 330);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 330);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 405, bm.Width - 50, bm.Height - 30),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                        //e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        //e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        //e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        //e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        //e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);
                        //e.Graphics.DrawLine(this.myPen, 44, 1050, 784, 1050);

                        //e.Graphics.DrawLine(this.myPen, 229, 190, 229, 470);
                        //e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);
                    }
                    else if (this.pathCount == 3)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 390);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 390);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 390);


                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 235, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 420, 330);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 605, 330);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 235, 370);

                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 370);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 445, bm.Width - 50, bm.Height - 70),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 4)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 51, 370);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 430);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 235, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 420, 330);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 605, 330);

                        e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 235, 370);

                        e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 420, 370);

                        e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);
                        e.Graphics.DrawString(this.myString[24], this.myFont, Brushes.Black, 235, 410);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 410);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 485, bm.Width - 50, bm.Height - 110),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    else if (this.pathCount == 5)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 51, 370);
                        e.Graphics.DrawString(this.myString[24], this.myFont, Brushes.Black, 51, 410);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 470);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 470);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 470);


                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 235, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 420, 330);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 605, 330);

                        e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 235, 370);

                        e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 420, 370);

                        e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);

                        e.Graphics.DrawString(this.myString[25], this.myFont, Brushes.Black, 235, 410);

                        e.Graphics.DrawString(this.myString[26], this.myFont, Brushes.Black, 420, 410);

                        e.Graphics.DrawString(this.myString[27], this.myFont, Brushes.Black, 605, 410);
                        e.Graphics.DrawString(this.myString[28], this.myFont, Brushes.Black, 235, 450);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 450);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 525, bm.Width - 50, bm.Height - 150),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    break;
                case 1:
                    e.Graphics.DrawString("高温膨胀力实验", this.myFont, Brushes.Black, 200, 160);
                    e.Graphics.DrawString("最大膨胀力值(N)", this.myFont, Brushes.Black, 235, 210);
                    e.Graphics.DrawString(this.myString[9], this.myFont, Brushes.Black, 235, 250);

                    e.Graphics.DrawString("对应时间(S)", this.myFont, Brushes.Black, 420, 210);
                    e.Graphics.DrawString(this.myString[10], this.myFont, Brushes.Black, 420, 250);
                    e.Graphics.DrawString("预载荷值(N)", this.myFont, Brushes.Black, 605, 210);
                    e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 605, 250);
                    if (this.pathCount == 1)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 270);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 270);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 270);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 405, bm.Width - 50, bm.Height),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    else if (this.pathCount == 2)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 350);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 350);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 350);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 235, 330);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 330);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 405, bm.Width - 50, bm.Height - 30),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                        //e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        //e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        //e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        //e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        //e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);
                        //e.Graphics.DrawLine(this.myPen, 44, 1050, 784, 1050);

                        //e.Graphics.DrawLine(this.myPen, 229, 190, 229, 470);
                        //e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);
                    }
                    else if (this.pathCount == 3)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 390);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 390);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 390);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 235, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 420, 330);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 605, 330);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 235, 370);

                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 370);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 445, bm.Width - 50, bm.Height - 70),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    else if (this.pathCount == 4)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 51, 370);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 430);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);


                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 235, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 420, 330);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 605, 330);

                        e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 235, 370);

                        e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 420, 370);

                        e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);
                        e.Graphics.DrawString(this.myString[24], this.myFont, Brushes.Black, 235, 410);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 410);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 485, bm.Width - 50, bm.Height - 110),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 5)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 51, 370);
                        e.Graphics.DrawString(this.myString[24], this.myFont, Brushes.Black, 51, 410);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 470);
                        e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);


                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 235, 290);

                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 420, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 605, 290);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 235, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 420, 330);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 605, 330);

                        e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 235, 370);

                        e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 420, 370);

                        e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);

                        e.Graphics.DrawString(this.myString[25], this.myFont, Brushes.Black, 235, 410);

                        e.Graphics.DrawString(this.myString[26], this.myFont, Brushes.Black, 420, 410);

                        e.Graphics.DrawString(this.myString[27], this.myFont, Brushes.Black, 605, 410);
                        e.Graphics.DrawString(this.myString[28], this.myFont, Brushes.Black, 235, 450);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 51, 450);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 525, bm.Width - 50, bm.Height - 150),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    break;
                case 2:
                    e.Graphics.DrawString("高温热平衡性强度实验", this.myFont, Brushes.Black, 200, 160);
                    //e.Graphics.DrawString("预载荷值(MPa)", this.myFont, Brushes.Black, 235, 210);
                    //e.Graphics.DrawString(this.myString[9], this.myFont, Brushes.Black, 235, 250);

                    e.Graphics.DrawString("最大时间(S)", this.myFont, Brushes.Black, 260, 210);
                    e.Graphics.DrawString(this.myString[9], this.myFont, Brushes.Black, 260, 250);
                    e.Graphics.DrawString("预载荷值(MPa)", this.myFont, Brushes.Black, 550, 210);
                    e.Graphics.DrawString(this.myString[10], this.myFont, Brushes.Black, 550, 250);
                    if (this.pathCount == 1)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 270);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 270);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 270);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 380, bm.Width - 50, bm.Height),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 2)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 74, 290);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 350);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 350);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 350);

                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 330);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 400, bm.Width - 50, bm.Height - 30),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                        //e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        //e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        //e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        //e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        //e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);
                        //e.Graphics.DrawLine(this.myPen, 44, 1050, 784, 1050);

                        //e.Graphics.DrawLine(this.myPen, 229, 190, 229, 470);
                        //e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);
                    }
                    else if (this.pathCount == 3)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 74, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 74, 330);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 390);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 390);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 390);

                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 550, 330);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 260, 370);

                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 370);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 405, bm.Width - 50, bm.Height - 30),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 4)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 51, 370);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 430);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 430);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);

                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 550, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 260, 370);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 550, 370);

                        //e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 235, 370);

                        //e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 420, 370);

                        //e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 260, 410);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 410);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 445, bm.Width - 50, bm.Height - 110),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    else if (this.pathCount == 5)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 74, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 74, 330);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 74, 370);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 74, 410);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 470);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 470);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 470);


                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 550, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 260, 370);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 550, 370);

                        e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 260, 410);

                        e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 550, 410);

                        //e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);

                        //e.Graphics.DrawString(this.myString[25], this.myFont, Brushes.Black, 235, 410);

                        //e.Graphics.DrawString(this.myString[26], this.myFont, Brushes.Black, 420, 410);

                        //e.Graphics.DrawString(this.myString[27], this.myFont, Brushes.Black, 605, 410);
                        e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 260, 450);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 450);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 525, bm.Width - 50, bm.Height - 150),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    break;
                case 3:
                    e.Graphics.DrawString("高温急热膨胀率实验", this.myFont, Brushes.Black, 200, 160);
                    e.Graphics.DrawString("最大膨胀率(%)", this.myFont, Brushes.Black, 260, 210);
                    e.Graphics.DrawString(this.myString[9], this.myFont, Brushes.Black, 260, 250);
                    e.Graphics.DrawString("最大时间(S)", this.myFont, Brushes.Black, 550, 210);
                    e.Graphics.DrawString(this.myString[10], this.myFont, Brushes.Black, 550, 250);
                    if (this.pathCount == 1)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);

                        e.Graphics.DrawLine(this.myPen, 229, 190, 229, 270);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 270);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 270);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 380, bm.Width - 50, bm.Height),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 2)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 74, 290);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 350);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 350);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 350);

                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 330);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 400, bm.Width - 50, bm.Height - 30),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                        //e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        //e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        //e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        //e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        //e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);
                        //e.Graphics.DrawLine(this.myPen, 44, 1050, 784, 1050);

                        //e.Graphics.DrawLine(this.myPen, 229, 190, 229, 470);
                        //e.Graphics.DrawLine(this.myPen, 414, 190, 414, 430);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);
                    }
                    else if (this.pathCount == 3)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 74, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 74, 330);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 390);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 390);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 390);


                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 550, 330);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 260, 370);

                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 370);
                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 405, bm.Width - 50, bm.Height - 30),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

                    }
                    else if (this.pathCount == 4)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 51, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 51, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 51, 330);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 51, 370);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 430);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 430);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 430);


                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 550, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 260, 370);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 550, 370);

                        //e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 235, 370);

                        //e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 420, 370);

                        //e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 260, 410);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 410);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 445, bm.Width - 50, bm.Height - 110),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    else if (this.pathCount == 5)
                    {
                        e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 74, 250);
                        e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 74, 290);
                        e.Graphics.DrawString(this.myString[14], this.myFont, Brushes.Black, 74, 330);
                        e.Graphics.DrawString(this.myString[17], this.myFont, Brushes.Black, 74, 370);
                        e.Graphics.DrawString(this.myString[20], this.myFont, Brushes.Black, 74, 410);
                        e.Graphics.DrawLine(this.myPen, 44, 230, 784, 230);
                        e.Graphics.DrawLine(this.myPen, 44, 270, 784, 270);
                        e.Graphics.DrawLine(this.myPen, 44, 310, 784, 310);
                        e.Graphics.DrawLine(this.myPen, 44, 350, 784, 350);
                        e.Graphics.DrawLine(this.myPen, 44, 390, 784, 390);
                        e.Graphics.DrawLine(this.myPen, 44, 430, 784, 430);
                        e.Graphics.DrawLine(this.myPen, 44, 470, 784, 470);

                        e.Graphics.DrawLine(this.myPen, 230, 190, 230, 470);
                        e.Graphics.DrawLine(this.myPen, 520, 190, 520, 470);
                        //e.Graphics.DrawLine(this.myPen, 599, 190, 599, 470);


                        e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 260, 290);

                        e.Graphics.DrawString(this.myString[13], this.myFont, Brushes.Black, 550, 290);

                        e.Graphics.DrawString(this.myString[15], this.myFont, Brushes.Black, 260, 330);
                        e.Graphics.DrawString(this.myString[16], this.myFont, Brushes.Black, 550, 330);

                        e.Graphics.DrawString(this.myString[18], this.myFont, Brushes.Black, 260, 370);

                        e.Graphics.DrawString(this.myString[19], this.myFont, Brushes.Black, 550, 370);

                        e.Graphics.DrawString(this.myString[21], this.myFont, Brushes.Black, 260, 410);

                        e.Graphics.DrawString(this.myString[22], this.myFont, Brushes.Black, 550, 410);

                        //e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 605, 370);

                        //e.Graphics.DrawString(this.myString[25], this.myFont, Brushes.Black, 235, 410);

                        //e.Graphics.DrawString(this.myString[26], this.myFont, Brushes.Black, 420, 410);

                        //e.Graphics.DrawString(this.myString[27], this.myFont, Brushes.Black, 605, 410);
                        e.Graphics.DrawString(this.myString[23], this.myFont, Brushes.Black, 260, 450);
                        e.Graphics.DrawString("平均值", this.myFont, Brushes.Black, 74, 450);

                        this.chart.DrawToBitmap(bm, r);
                        e.Graphics.DrawImage(bm, new Rectangle(45, 525, bm.Width - 50, bm.Height - 150),
                        new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);
                    }
                    break;
            }
            
            e.Graphics.DrawString("实验员:", this.myBigFont, Brushes.Black, 60, 1060);
            e.Graphics.DrawString("(" + this.myString[1] + ")", this.myBigFont, Brushes.Black, 140, 1060);
            e.Graphics.DrawString("审核:", this.myBigFont, Brushes.Black, 521, 1060);
            e.Graphics.DrawString("日期：   年     月    日", this.myBigFont, Brushes.Black, 451, 1126);

        }

    }
}