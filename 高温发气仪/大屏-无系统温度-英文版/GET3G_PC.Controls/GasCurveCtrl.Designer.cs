namespace GET3G_PC.Controls
{
    partial class GasCurveCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.grpContainer = new System.Windows.Forms.GroupBox();
            this.panCurve = new System.Windows.Forms.Panel();
            this.grpContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpContainer
            // 
            this.grpContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpContainer.Controls.Add(this.panCurve);
            this.grpContainer.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grpContainer.Location = new System.Drawing.Point(3, 3);
            this.grpContainer.Name = "grpContainer";
            this.grpContainer.Size = new System.Drawing.Size(528, 301);
            this.grpContainer.TabIndex = 0;
            this.grpContainer.TabStop = false;
            this.grpContainer.Text = "[Gas-Speed]Curve";
            this.grpContainer.Resize += new System.EventHandler(this.grpContainer_Resize);
            // 
            // panCurve
            // 
            this.panCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panCurve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panCurve.Location = new System.Drawing.Point(3, 23);
            this.panCurve.Name = "panCurve";
            this.panCurve.Size = new System.Drawing.Size(522, 272);
            this.panCurve.TabIndex = 0;
            this.panCurve.Paint += new System.Windows.Forms.PaintEventHandler(this.panCurve_Paint);
            // 
            // GasCurveCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpContainer);
            this.Name = "GasCurveCtrl";
            this.Size = new System.Drawing.Size(534, 307);
            this.grpContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpContainer;
        private System.Windows.Forms.Panel panCurve;
    }
}
