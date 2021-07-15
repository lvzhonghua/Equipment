using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using System.Xml;
using System.Xml.Serialization;

using GET3G_PC.Controls;

namespace GasDataReader
{
    public partial class FormMain : Form
    { 
        
        private GasDataToPrint gasDataToPrint = null;
        
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            //将文件反序列化为对象
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Gas Data(*.gas)|*.gas";

            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = openDlg.FileName;

                    //反序列化
                    FileStream stream = new FileStream(fileName, FileMode.Open);
                    XmlSerializer serializer = new XmlSerializer(typeof(GasDataToPrint));
                    this.gasDataToPrint = (GasDataToPrint)serializer.Deserialize(stream);
                    stream.Close();

                    //this.gasCurveCtrl.Interval = 2;
                    this.gasCurveCtrl.Interval = 1;
                    this.gasCurveCtrl.GasTimeList = this.gasDataToPrint.GasTimeList;
                    this.gasCurveCtrl.GasInCrementList = this.gasDataToPrint.GasInCrementList;
                    this.gasCurveCtrl.GasList = this.gasDataToPrint.GasList;

                    this.gasCurveCtrl.DisplayGasCurve();

                    this.tabData.SelectedIndex = 0;

                    //显示数据
                    this.txtGasData.Clear();

                    if (this.gasDataToPrint == null) return;

                    //string strTemp = "试样参数：\r\n";
                    //this.txtGasData.AppendText(strTemp);

                    string strTemp = string.Format("\tName: {0}\r\n", this.gasDataToPrint.SampleName);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tSample No.: {0}\r\n", this.gasDataToPrint.SampleNumber);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tFactory: {0}\r\n", this.gasDataToPrint.Factory);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tTester: {0}\r\n", this.gasDataToPrint.People);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tRepeat: {0}\r\n", this.gasDataToPrint.SampleRepeat);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tWeight: {0}g\r\n", this.gasDataToPrint.SampleWeight);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tMax Gas: {0}ml/g\r\n", this.gasDataToPrint.Gas);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tGas Speed: {0}ml/g/s\r\n", this.gasDataToPrint.GasIncrement);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tTesting TEMP: {0}℃\r\n", this.gasDataToPrint.FurnaceTargetTemperature);
                    this.txtGasData.AppendText(strTemp);

                    string testTime = this.gasDataToPrint.TestTime;
                    string testDate ="";
                    if (testTime.IndexOf(" ") != -1)
                    {
                        testDate = testTime.Substring(0, testTime.IndexOf(" "));
                    }
                    
                    testTime = testTime.Substring(testTime.LastIndexOf(" ")+1);

                    strTemp = string.Format("\tDate: {0}\r\n",testDate);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\tTime: {0}\r\n", testTime);
                    this.txtGasData.AppendText(strTemp);

                    strTemp = "\r\n\r\nGas Data: \r\n";
                    this.txtGasData.AppendText(strTemp);

                    strTemp = string.Format("\t{0}(s)\t   {1}(ml/g)\t    {2}(ml/g/s)\r\n", "Time", "Gas", "Speed");
                    this.txtGasData.AppendText(strTemp);

                    for (int index = 0; index < this.gasDataToPrint.GasTimeList.Count; index++)
                    {
                        strTemp = string.Format("\t {0}\t\t{1}\t\t{2:0.00}\r\n",
                                                            this.gasCurveCtrl.Interval * this.gasDataToPrint.GasTimeList[index],
                                                            this.gasDataToPrint.GasList[index],
                                                            this.gasDataToPrint.GasInCrementList[index]);

                        this.txtGasData.AppendText(strTemp);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Program error：" + ex.Message,"Error Tips",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                }
            }


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //打印
            if (this.gasDataToPrint == null)
            {
                MessageBox.Show("Gas Data is null, can not be print.","Tips",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            this.printDocument.DefaultPageSettings.Landscape = true;

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = this.printDocument;
            printPreviewDialog.Show(this);
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //打印
            //绘制曲线部分
            this.gasDataToPrint.DrawGasDatas(e.Graphics);
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (this.gasDataToPrint == null) return;

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "CSV(*.csv)|*.csv";

            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    //string strTemp = "试样参数：";
                    //stringBuilder.AppendLine(strTemp);

                    string strTemp = string.Format(",Name: {0}", this.gasDataToPrint.SampleName);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Sample No.: {0}", this.gasDataToPrint.SampleNumber);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Factory: {0}", this.gasDataToPrint.Factory);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Tester: {0}", this.gasDataToPrint.People);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Repeat: {0}", this.gasDataToPrint.SampleRepeat);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Weight: {0}g", this.gasDataToPrint.SampleWeight);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Max Gass: {0}ml/g", this.gasDataToPrint.Gas);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Gas Speed: {0}ml/g/s", this.gasDataToPrint.GasIncrement);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Testing TEMP: {0}℃", this.gasDataToPrint.FurnaceTargetTemperature);
                    stringBuilder.AppendLine(strTemp);

                    string testTime = this.gasDataToPrint.TestTime;
                    string testDate = "";
                    if (testTime.IndexOf(" ") != -1)
                    {
                        testDate = testTime.Substring(0, testTime.IndexOf(" "));
                    }

                    testTime = testTime.Substring(testTime.LastIndexOf(" ") + 1);

                    strTemp = string.Format(",Date: {0}", testDate);
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",Time: {0}", testTime);
                    stringBuilder.AppendLine(strTemp);
           
                    strTemp = "\r\nGas Data: ";
                    stringBuilder.AppendLine(strTemp);

                    strTemp = string.Format(",{0}(s),{1}(ml/g),{2}(ml/g/s)", "Time", "Gas", "Gas Speed");
                    stringBuilder.AppendLine(strTemp);

                    for (int index = 0; index < this.gasDataToPrint.GasTimeList.Count; index++)
                    {
                        //strTemp = string.Format(",{0},{1},{2:0.00}",
                        //                                    2 * this.gasDataToPrint.GasTimeList[index],
                        //                                    this.gasDataToPrint.GasList[index],
                        //                                    this.gasDataToPrint.GasInCrementList[index]);
                        strTemp = string.Format(",{0},{1},{2:0.00}",
                                                                this.gasDataToPrint.GasTimeList[index],
                                                                this.gasDataToPrint.GasList[index],
                                                                this.gasDataToPrint.GasInCrementList[index]);

                        stringBuilder.AppendLine(strTemp);

                    }

                    string fileName = saveDlg.FileName;
                    StreamWriter streamWriter = new StreamWriter(fileName, false, Encoding.Default);
                    streamWriter.Write(stringBuilder.ToString());
                    streamWriter.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data Saving Error: " + ex.Message, "错误提示",
                                                MessageBoxButtons.OK,
                                                MessageBoxIcon.Error);
                }
            }
        }

        private void btnSaveToImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "PNG(*.png)|*.png";

            if (saveDlg.ShowDialog() != DialogResult.OK) return;
            string fileName = saveDlg.FileName;

            //将图形保存到图像
            int width = this.gasDataToPrint.RectToPrint.Width + 400;
            int height = this.gasDataToPrint.RectToPrint.Height + 400;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);

            this.gasDataToPrint.DrawGasDatas(graphics);

            graphics.Dispose();

            bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
            bitmap.Dispose();
        }
    }
}
