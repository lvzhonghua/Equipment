using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ResinSandPyrometer
{
    public partial class DialogDataLook : Form
    {
        private DataPointCollection pointCollection = null;

        public DataPointCollection PointCollection
        {
            get { return this.pointCollection; }
            set 
            { 
                this.pointCollection = value;

                if (this.pointCollection == null)
                {
                    this.txtData.Text = "";
                    return;
                }

                foreach (DataPoint point in this.pointCollection)
                {
                    this.txtData.AppendText($"{point.XValue:0}\t{point.YValues[0]:0.000}\r\n");
                }
            }
        }

        public DialogDataLook()
        {
            InitializeComponent();
        }
    }
}
