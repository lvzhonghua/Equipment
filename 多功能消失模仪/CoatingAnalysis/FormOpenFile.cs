using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoatingAnalysis
{
    public partial class FormOpenFile : Form
    {
        ConfigFile cf = new ConfigFile("PaintTest");

        private AppSettingsReader settingsReader = new AppSettingsReader();

        private int testCount = 0;

        Font myBigFont = new Font(new FontFamily("宋体"), 16);

        Font myFont = new Font(new FontFamily("宋体"), 12);

        Pen myPen = new Pen(Brushes.Black);

        string[] myString = new string[13];
        public FormOpenFile()
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


        private void myPrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {

            string factory = (string)settingsReader.GetValue("Factory", typeof(string));
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Bitmap bm = new Bitmap(this.tranfChart_Open.Width, this.tranfChart_Open.Height);
            Rectangle r = new Rectangle(0, 0, bm.Width, bm.Height);
            e.Graphics.DrawString("消失模涂料测试检测报告", this.myBigFont, Brushes.Black, 290, 28);
            e.Graphics.DrawLine(this.myPen, 38, 60, 784, 60);
            e.Graphics.DrawLine(this.myPen, 38, 1130, 784, 1130);
            e.Graphics.DrawLine(this.myPen, 38, 60, 38, 1130);
            e.Graphics.DrawLine(this.myPen, 784, 60, 784, 1130);
            e.Graphics.DrawLine(this.myPen, 38, 100, 784, 100);
            e.Graphics.DrawLine(this.myPen, 38, 140, 784, 140);
            e.Graphics.DrawString("实验序号", this.myFont, Brushes.Black, 51, 74);
            e.Graphics.DrawString(this.myString[2], this.myFont, Brushes.Black, 140, 74);
            e.Graphics.DrawString("涂料名称", this.myFont, Brushes.Black, 211, 74);
            e.Graphics.DrawString(this.myString[3], this.myFont, Brushes.Black, 310, 74);
            e.Graphics.DrawString("来样单位", this.myFont, Brushes.Black, 451, 74);
            e.Graphics.DrawString(this.myString[6], this.myFont, Brushes.Black, 540, 74);

            e.Graphics.DrawString("试样厚度", this.myFont, Brushes.Black, 51, 114);
            e.Graphics.DrawString(this.myString[4], this.myFont, Brushes.Black, 140, 114);
            e.Graphics.DrawString("设置温度", this.myFont, Brushes.Black, 211, 114);
            e.Graphics.DrawString(this.myString[5], this.myFont, Brushes.Black, 310, 114);
            e.Graphics.DrawString("实验时间", this.myFont, Brushes.Black, 451, 114);
            e.Graphics.DrawString(this.myString[1], this.myFont, Brushes.Black, 540, 114);

            //e.Graphics.DrawLine(this.myPen, 38, 190, 784, 190);
            e.Graphics.DrawLine(this.myPen, 38, 230, 784, 230);
            e.Graphics.DrawLine(this.myPen, 38, 350, 784, 350);
            e.Graphics.DrawLine(this.myPen, 533, 290, 784, 290);

            e.Graphics.DrawLine(this.myPen, 38, 270, 287, 270);
            e.Graphics.DrawLine(this.myPen, 38, 310, 287, 310);
            e.Graphics.DrawLine(this.myPen, 287, 190, 287, 350);
            e.Graphics.DrawLine(this.myPen, 533, 190, 533, 350);
            e.Graphics.DrawLine(this.myPen, 414, 350, 414, 690);
            e.Graphics.DrawLine(this.myPen, 38, 690, 784, 690);

            e.Graphics.DrawLine(this.myPen, 124, 230, 124, 350);
            e.Graphics.DrawLine(this.myPen, 370, 230, 370, 350);
            e.Graphics.DrawLine(this.myPen, 617, 230, 617, 350);
            e.Graphics.DrawString("实验结果：", this.myBigFont, Brushes.Black, 51, 157);

            e.Graphics.DrawString("传输特性实验", this.myFont, Brushes.Black, 114, 204);
            e.Graphics.DrawString("最大压强", this.myFont, Brushes.Black, 51, 238);
            e.Graphics.DrawString(this.myString[7], this.myFont, Brushes.Black, 140, 238);
            e.Graphics.DrawString("对应时间", this.myFont, Brushes.Black, 51, 284);
            e.Graphics.DrawString(this.myString[8], this.myFont, Brushes.Black, 140, 284);
            e.Graphics.DrawString("透气性排出时间", this.myFont, Brushes.Black, 51, 324);
            e.Graphics.DrawString(this.myString[9], this.myFont, Brushes.Black, 140, 324);

            e.Graphics.DrawString("透气性实验", this.myFont, Brushes.Black, 377, 204);
            e.Graphics.DrawString("透气率", this.myFont, Brushes.Black, 306, 284);
            e.Graphics.DrawString(this.myString[10], this.myFont, Brushes.Black, 386, 284);

            e.Graphics.DrawString("高温强度实验", this.myFont, Brushes.Black, 607, 204);
            e.Graphics.DrawString("最大压强", this.myFont, Brushes.Black, 539, 254);
            e.Graphics.DrawString(this.myString[11], this.myFont, Brushes.Black, 633, 254);
            e.Graphics.DrawString("对应时间", this.myFont, Brushes.Black, 539, 314);
            e.Graphics.DrawString(this.myString[12], this.myFont, Brushes.Black, 633, 314);

            e.Graphics.DrawString("传输特性曲线", this.myBigFont, Brushes.Black, 157, 367);
            e.Graphics.DrawString("高温强度曲线", this.myBigFont, Brushes.Black, 527, 367);
            this.tranfChart_Open.DrawToBitmap(bm, r);
            e.Graphics.DrawImage(bm, new Rectangle(45, 400, bm.Width / 2, bm.Height / 2),
                new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

            this.strengthChart_Open.DrawToBitmap(bm, r);
            e.Graphics.DrawImage(bm, new Rectangle(415, 400, bm.Width / 2, bm.Height / 2),
                new Rectangle(0, 0, bm.Width, bm.Height), GraphicsUnit.Pixel);

            e.Graphics.DrawString("实验员:", this.myBigFont, Brushes.Black, 51, 777);
            e.Graphics.DrawString("  " + this.myString[0] , this.myBigFont, Brushes.Black, 121, 777);
            e.Graphics.DrawString("（签字）", this.myBigFont, Brushes.Black, 314, 777);
            e.Graphics.DrawString("审核:", this.myBigFont, Brushes.Black, 421, 777);
            e.Graphics.DrawString("（签字）", this.myBigFont, Brushes.Black, 684, 777);
            e.Graphics.DrawString("日期：        年     月    日", this.myBigFont, Brushes.Black, 51, 1000);
            e.Graphics.DrawString("日期：        年     月    日", this.myBigFont, Brushes.Black, 421, 1000);
            e.Graphics.DrawString("（盖章）", this.myBigFont, Brushes.Black, 680, 1050);
            e.Graphics.DrawString(factory, this.myBigFont, Brushes.Black, 310, 1100);
        }

        private void FormOpenFile_Load(object sender, EventArgs e)
        {
            //this.myPrintDoc.PrintPage += new PrintPageEventHandler(this.myPrintDoc_PrintPage);
        }

        private void ToolStripMenuPageSet_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();
            pageSetupDialog.Document = this.myPrintDoc;
            pageSetupDialog.ShowDialog();
        }

        private void ToolStripMenuPrintSet_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = this.myPrintDoc;
            printDialog.ShowDialog();
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            string path = String.Empty;
            OpenFileDialog opnDlg = new OpenFileDialog();

            if (opnDlg.ShowDialog() == DialogResult.OK)
            {
                path = opnDlg.FileName;
                this.tranfChart_Open.Series[0].Points.Clear();
                this.ventiChart_Open.Series[0].Points.Clear();
                this.strengthChart_Open.Series[0].Points.Clear();
                this.infoListBox.Items.Clear();
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                string strLine = null;
                string str = null;
                double xPos = 0;
                double yPos = 0;
                this.testCount = 0;
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (strLine.Contains("Info"))
                    {
                        strLine = strLine.Substring(4);
                        this.infoListBox.Items.Add(strLine);

                        switch (testCount)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                                this.myString[testCount] = strLine.Substring(5);
                                break;
                            case 7:
                            case 9:
                            case 10:
                            case 11:
                                this.myString[testCount] = strLine.Substring(9);
                                break;
                            case 8:
                            case 12:
                                this.myString[testCount] = strLine.Substring(11);
                                break;
                            case 13:
                            default:
                                break;
                        }
                        this.testCount++;
                    }
                    else if (strLine.Contains("Tp"))
                    {
                        str = strLine.Substring(strLine.IndexOf("X") + 1, strLine.Length - strLine.IndexOf("X") - 1);
                        str = str.Substring(0, str.IndexOf("Y"));
                        xPos = float.Parse(str);
                        str = strLine.Substring(strLine.IndexOf("Y") + 1, strLine.Length - strLine.IndexOf("Y") - 1);
                        yPos = float.Parse(str);

                        this.tranfChart_Open.Series[0].Points.AddXY(xPos, yPos);
                    }
                    else if (strLine.Contains("Vp"))
                    {
                        str = strLine.Substring(strLine.IndexOf("X") + 1, strLine.Length - strLine.IndexOf("X") - 1);
                        str = str.Substring(0, str.IndexOf("Y"));
                        xPos = float.Parse(str);
                        str = strLine.Substring(strLine.IndexOf("Y") + 1, strLine.Length - strLine.IndexOf("Y") - 1);
                        yPos = float.Parse(str);

                        this.ventiChart_Open.Series[0].Points.AddXY(xPos, yPos);
                    }
                    else if (strLine.Contains("Sp"))
                    {
                        str = strLine.Substring(strLine.IndexOf("X") + 1, strLine.Length - strLine.IndexOf("X") - 1);
                        str = str.Substring(0, str.IndexOf("Y"));
                        xPos = float.Parse(str);
                        str = strLine.Substring(strLine.IndexOf("Y") + 1, strLine.Length - strLine.IndexOf("Y") - 1);
                        yPos = float.Parse(str);

                        this.strengthChart_Open.Series[0].Points.AddXY(xPos, yPos);
                    }
                    else
                    {
                        infoListBox.Items.Add(strLine);
                    }
                }
                sr.Close();
            }
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            this.myPrintPreDia.Document = this.myPrintDoc;
            //PrintPreviewDialog ppd = new PrintPreviewDialog();
            //ppd.Document = this.myPrintDoc;


            try
            {
                //ppd.ShowDialog();
                DialogResult result = this.myPrintPreDia.ShowDialog();
                if (result == DialogResult.OK)
                    this.myPrintDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                return;
            }
        }
    }
}
