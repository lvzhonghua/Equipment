
namespace ResinSandPyrometer
{
    partial class FormDebug
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtLog_Temperature = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtCOM_Temperature = new System.Windows.Forms.ToolStripTextBox();
            this.btnConnect_Temperature = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDisconnect_Temperature = new System.Windows.Forms.ToolStripButton();
            this.btnSend_Temperature = new System.Windows.Forms.ToolStripButton();
            this.cboCommand_Temperature = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtLog_Displacement = new System.Windows.Forms.TextBox();
            this.tttt = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.txtCOM_Displacement = new System.Windows.Forms.ToolStripTextBox();
            this.btnConnect_Displacement = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDisconnect_Displacement = new System.Windows.Forms.ToolStripButton();
            this.btnSend_Displacement = new System.Windows.Forms.ToolStripButton();
            this.cboCommand_Displacement = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtLog_Slave = new System.Windows.Forms.TextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.txtCOM_Slave = new System.Windows.Forms.ToolStripTextBox();
            this.btnConnect_Slave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDisconnect_Slave = new System.Windows.Forms.ToolStripButton();
            this.btnSend_Slave = new System.Windows.Forms.ToolStripButton();
            this.cboCommand_Slave = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCalibrate = new System.Windows.Forms.ToolStripButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnCalcForce = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtAutoDetect = new System.Windows.Forms.TextBox();
            this.btnAutoDetect = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tttt.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(724, 419);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.txtLog_Temperature);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(716, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "温控仪";
            // 
            // txtLog_Temperature
            // 
            this.txtLog_Temperature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog_Temperature.Location = new System.Drawing.Point(3, 28);
            this.txtLog_Temperature.Multiline = true;
            this.txtLog_Temperature.Name = "txtLog_Temperature";
            this.txtLog_Temperature.ReadOnly = true;
            this.txtLog_Temperature.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog_Temperature.Size = new System.Drawing.Size(710, 362);
            this.txtLog_Temperature.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtCOM_Temperature,
            this.btnConnect_Temperature,
            this.toolStripSeparator1,
            this.btnDisconnect_Temperature,
            this.btnSend_Temperature,
            this.cboCommand_Temperature,
            this.toolStripLabel2});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(710, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel1.Text = "串口：";
            // 
            // txtCOM_Temperature
            // 
            this.txtCOM_Temperature.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtCOM_Temperature.Name = "txtCOM_Temperature";
            this.txtCOM_Temperature.Size = new System.Drawing.Size(60, 25);
            this.txtCOM_Temperature.Text = "COM3";
            // 
            // btnConnect_Temperature
            // 
            this.btnConnect_Temperature.Image = global::ResinSandPyrometer.Properties.Resources.连接_红色_16;
            this.btnConnect_Temperature.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect_Temperature.Name = "btnConnect_Temperature";
            this.btnConnect_Temperature.Size = new System.Drawing.Size(52, 22);
            this.btnConnect_Temperature.Text = "连接";
            this.btnConnect_Temperature.Click += new System.EventHandler(this.btnConnect_Temperature_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDisconnect_Temperature
            // 
            this.btnDisconnect_Temperature.Image = global::ResinSandPyrometer.Properties.Resources.断开链接2_16;
            this.btnDisconnect_Temperature.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisconnect_Temperature.Name = "btnDisconnect_Temperature";
            this.btnDisconnect_Temperature.Size = new System.Drawing.Size(52, 22);
            this.btnDisconnect_Temperature.Text = "断开";
            this.btnDisconnect_Temperature.Click += new System.EventHandler(this.btnDisconnect_Temperature_Click);
            // 
            // btnSend_Temperature
            // 
            this.btnSend_Temperature.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSend_Temperature.Image = global::ResinSandPyrometer.Properties.Resources.发送_16;
            this.btnSend_Temperature.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSend_Temperature.Name = "btnSend_Temperature";
            this.btnSend_Temperature.Size = new System.Drawing.Size(52, 22);
            this.btnSend_Temperature.Text = "发送";
            this.btnSend_Temperature.Click += new System.EventHandler(this.btnSend_Temperature_Click);
            // 
            // cboCommand_Temperature
            // 
            this.cboCommand_Temperature.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cboCommand_Temperature.Items.AddRange(new object[] {
            "查询",
            "设定目标炉温：20度",
            "设定目标炉温：30度",
            "设定目标炉温：50度",
            "设定目标炉温：100度",
            "设定目标炉温：250度",
            "设定目标炉温：500度",
            "设定目标炉温：1000度",
            "停止温控仪",
            "启动温控仪"});
            this.cboCommand_Temperature.Name = "cboCommand_Temperature";
            this.cboCommand_Temperature.Size = new System.Drawing.Size(121, 25);
            this.cboCommand_Temperature.Text = "查询";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel2.Text = "指令";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.txtLog_Displacement);
            this.tabPage2.Controls.Add(this.tttt);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(716, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "位移传感器";
            // 
            // txtLog_Displacement
            // 
            this.txtLog_Displacement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog_Displacement.Location = new System.Drawing.Point(3, 28);
            this.txtLog_Displacement.Multiline = true;
            this.txtLog_Displacement.Name = "txtLog_Displacement";
            this.txtLog_Displacement.ReadOnly = true;
            this.txtLog_Displacement.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog_Displacement.Size = new System.Drawing.Size(710, 362);
            this.txtLog_Displacement.TabIndex = 3;
            // 
            // tttt
            // 
            this.tttt.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.txtCOM_Displacement,
            this.btnConnect_Displacement,
            this.toolStripSeparator2,
            this.btnDisconnect_Displacement,
            this.btnSend_Displacement,
            this.cboCommand_Displacement,
            this.toolStripLabel4});
            this.tttt.Location = new System.Drawing.Point(3, 3);
            this.tttt.Name = "tttt";
            this.tttt.Size = new System.Drawing.Size(710, 25);
            this.tttt.TabIndex = 2;
            this.tttt.Text = "toolStrip2";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel3.Text = "串口：";
            // 
            // txtCOM_Displacement
            // 
            this.txtCOM_Displacement.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtCOM_Displacement.Name = "txtCOM_Displacement";
            this.txtCOM_Displacement.Size = new System.Drawing.Size(60, 25);
            this.txtCOM_Displacement.Text = "COM3";
            // 
            // btnConnect_Displacement
            // 
            this.btnConnect_Displacement.Image = global::ResinSandPyrometer.Properties.Resources.连接_红色_16;
            this.btnConnect_Displacement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect_Displacement.Name = "btnConnect_Displacement";
            this.btnConnect_Displacement.Size = new System.Drawing.Size(52, 22);
            this.btnConnect_Displacement.Text = "连接";
            this.btnConnect_Displacement.Click += new System.EventHandler(this.btnConnect_Displacement_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDisconnect_Displacement
            // 
            this.btnDisconnect_Displacement.Image = global::ResinSandPyrometer.Properties.Resources.断开链接2_16;
            this.btnDisconnect_Displacement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisconnect_Displacement.Name = "btnDisconnect_Displacement";
            this.btnDisconnect_Displacement.Size = new System.Drawing.Size(52, 22);
            this.btnDisconnect_Displacement.Text = "断开";
            this.btnDisconnect_Displacement.Click += new System.EventHandler(this.btnDisconnect_Displacement_Click);
            // 
            // btnSend_Displacement
            // 
            this.btnSend_Displacement.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSend_Displacement.Image = global::ResinSandPyrometer.Properties.Resources.发送_16;
            this.btnSend_Displacement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSend_Displacement.Name = "btnSend_Displacement";
            this.btnSend_Displacement.Size = new System.Drawing.Size(52, 22);
            this.btnSend_Displacement.Text = "发送";
            this.btnSend_Displacement.Click += new System.EventHandler(this.btnSend_Displacement_Click);
            // 
            // cboCommand_Displacement
            // 
            this.cboCommand_Displacement.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cboCommand_Displacement.Items.AddRange(new object[] {
            "查询"});
            this.cboCommand_Displacement.Name = "cboCommand_Displacement";
            this.cboCommand_Displacement.Size = new System.Drawing.Size(121, 25);
            this.cboCommand_Displacement.Text = "查询";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel4.Text = "指令";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.txtLog_Slave);
            this.tabPage3.Controls.Add(this.toolStrip2);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(716, 393);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "下位机";
            // 
            // txtLog_Slave
            // 
            this.txtLog_Slave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog_Slave.Location = new System.Drawing.Point(3, 28);
            this.txtLog_Slave.Multiline = true;
            this.txtLog_Slave.Name = "txtLog_Slave";
            this.txtLog_Slave.ReadOnly = true;
            this.txtLog_Slave.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog_Slave.Size = new System.Drawing.Size(710, 362);
            this.txtLog_Slave.TabIndex = 4;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel5,
            this.txtCOM_Slave,
            this.btnConnect_Slave,
            this.toolStripSeparator3,
            this.btnDisconnect_Slave,
            this.btnSend_Slave,
            this.cboCommand_Slave,
            this.toolStripLabel6,
            this.toolStripSeparator4,
            this.btnCalibrate});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(710, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(44, 22);
            this.toolStripLabel5.Text = "串口：";
            // 
            // txtCOM_Slave
            // 
            this.txtCOM_Slave.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.txtCOM_Slave.Name = "txtCOM_Slave";
            this.txtCOM_Slave.Size = new System.Drawing.Size(60, 25);
            this.txtCOM_Slave.Text = "COM4";
            // 
            // btnConnect_Slave
            // 
            this.btnConnect_Slave.Image = global::ResinSandPyrometer.Properties.Resources.连接_红色_16;
            this.btnConnect_Slave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect_Slave.Name = "btnConnect_Slave";
            this.btnConnect_Slave.Size = new System.Drawing.Size(52, 22);
            this.btnConnect_Slave.Text = "连接";
            this.btnConnect_Slave.Click += new System.EventHandler(this.btnConnect_Slave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDisconnect_Slave
            // 
            this.btnDisconnect_Slave.Image = global::ResinSandPyrometer.Properties.Resources.断开链接2_16;
            this.btnDisconnect_Slave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDisconnect_Slave.Name = "btnDisconnect_Slave";
            this.btnDisconnect_Slave.Size = new System.Drawing.Size(52, 22);
            this.btnDisconnect_Slave.Text = "断开";
            this.btnDisconnect_Slave.Click += new System.EventHandler(this.btnDisconnect_Slave_Click);
            // 
            // btnSend_Slave
            // 
            this.btnSend_Slave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSend_Slave.Image = global::ResinSandPyrometer.Properties.Resources.发送_16;
            this.btnSend_Slave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSend_Slave.Name = "btnSend_Slave";
            this.btnSend_Slave.Size = new System.Drawing.Size(52, 22);
            this.btnSend_Slave.Text = "发送";
            this.btnSend_Slave.Click += new System.EventHandler(this.btnSend_Slave_Click);
            // 
            // cboCommand_Slave
            // 
            this.cboCommand_Slave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cboCommand_Slave.Items.AddRange(new object[] {
            "查询",
            "炉子归零",
            "炉子到默认工作位",
            "炉子到工作位0",
            "炉子到工作位1",
            "炉子到工作位2",
            "炉子到工作位3",
            "炉子到工作位4",
            "炉子到工作位5",
            "炉子到工作位6",
            "炉子到工作位7",
            "炉子到工作位8",
            "炉子到工作位9",
            "托盘上升一步",
            "托盘下降一步",
            "托盘归零",
            "设置托盘上升速度(默认值)",
            "设置托盘上升行程(2mm)",
            "设置托盘上升行程(4mm)",
            "设置托盘上升行程(6mm)",
            "设置托盘上升行程(8mm)",
            "设置托盘上升行程(10mm)",
            "设置托盘上升行程(12mm)",
            "设置托盘上升行程(14mm)",
            "设置托盘上升行程(16mm)",
            "设置托盘上升行程(18mm)",
            "设置托盘上升行程(20mm)",
            "力传感器当前值查询",
            "设置测试方式(高温抗压强度)",
            "设置测试方式(热稳定性)",
            "设置测试方式(高温膨胀力)",
            "单片机复位",
            "开始测试",
            "结束测试",
            "禁止/允许发送传感器零点数据",
            "炉温到达设定温度",
            "托盘上升2mm",
            "托盘下降2mm"});
            this.cboCommand_Slave.Name = "cboCommand_Slave";
            this.cboCommand_Slave.Size = new System.Drawing.Size(221, 25);
            this.cboCommand_Slave.Text = "查询";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(32, 22);
            this.toolStripLabel6.Text = "指令";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.Image = global::ResinSandPyrometer.Properties.Resources.校准_red_16;
            this.btnCalibrate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Size = new System.Drawing.Size(52, 22);
            this.btnCalibrate.Text = "校准";
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.btnCalcForce);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(716, 393);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "力的计算";
            // 
            // btnCalcForce
            // 
            this.btnCalcForce.Location = new System.Drawing.Point(8, 6);
            this.btnCalcForce.Name = "btnCalcForce";
            this.btnCalcForce.Size = new System.Drawing.Size(75, 23);
            this.btnCalcForce.TabIndex = 0;
            this.btnCalcForce.Text = "力的计算";
            this.btnCalcForce.UseVisualStyleBackColor = true;
            this.btnCalcForce.Click += new System.EventHandler(this.btnCalcForce_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage5.Controls.Add(this.txtAutoDetect);
            this.tabPage5.Controls.Add(this.btnAutoDetect);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(716, 393);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "串口自动匹配";
            // 
            // txtAutoDetect
            // 
            this.txtAutoDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAutoDetect.Location = new System.Drawing.Point(6, 35);
            this.txtAutoDetect.Multiline = true;
            this.txtAutoDetect.Name = "txtAutoDetect";
            this.txtAutoDetect.ReadOnly = true;
            this.txtAutoDetect.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAutoDetect.Size = new System.Drawing.Size(702, 350);
            this.txtAutoDetect.TabIndex = 2;
            // 
            // btnAutoDetect
            // 
            this.btnAutoDetect.Location = new System.Drawing.Point(6, 6);
            this.btnAutoDetect.Name = "btnAutoDetect";
            this.btnAutoDetect.Size = new System.Drawing.Size(75, 23);
            this.btnAutoDetect.TabIndex = 0;
            this.btnAutoDetect.Text = "自动匹配";
            this.btnAutoDetect.UseVisualStyleBackColor = true;
            this.btnAutoDetect.Click += new System.EventHandler(this.btnAutoDetect_Click);
            // 
            // FormDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 419);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormDebug";
            this.Text = "调试工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDebug_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tttt.ResumeLayout(false);
            this.tttt.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtCOM_Temperature;
        private System.Windows.Forms.ToolStripButton btnConnect_Temperature;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDisconnect_Temperature;
        private System.Windows.Forms.TextBox txtLog_Temperature;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cboCommand_Temperature;
        private System.Windows.Forms.ToolStripButton btnSend_Temperature;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtLog_Displacement;
        private System.Windows.Forms.ToolStrip tttt;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox txtCOM_Displacement;
        private System.Windows.Forms.ToolStripButton btnConnect_Displacement;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnDisconnect_Displacement;
        private System.Windows.Forms.ToolStripButton btnSend_Displacement;
        private System.Windows.Forms.ToolStripComboBox cboCommand_Displacement;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtLog_Slave;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripTextBox txtCOM_Slave;
        private System.Windows.Forms.ToolStripButton btnConnect_Slave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnDisconnect_Slave;
        private System.Windows.Forms.ToolStripButton btnSend_Slave;
        private System.Windows.Forms.ToolStripComboBox cboCommand_Slave;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnCalcForce;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnCalibrate;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtAutoDetect;
        private System.Windows.Forms.Button btnAutoDetect;
    }
}