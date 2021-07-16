using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResinSandPyrometer
{
    public partial class FormCalcForce : Form
    {
        public FormCalcForce()
        {
            InitializeComponent();
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            string[] bytesValue = this.txtBytes.Text.Split('-');

            byte[] bytes = new byte[3] { (byte)int.Parse(bytesValue[0], System.Globalization.NumberStyles.HexNumber),
                                                      (byte)int.Parse(bytesValue[1], System.Globalization.NumberStyles.HexNumber) ,
                                                      (byte)int.Parse(bytesValue[2], System.Globalization.NumberStyles.HexNumber) };
            
            long voltage = NumberSystem.BinaryToDecimal_Complement(bytes, 16);
            this.lblVoltage.Text = $"电压：{voltage}";

            double force = Utilities.GetForceFromVoltage((float)voltage,
                                                                            float.Parse(this.txtSensorMax.Text),
                                                                            float.Parse(this.txtSensorMV.Text),
                                                                            float.Parse(this.txtSensibility.Text),
                                                                            float.Parse(this.txtRevise.Text));

            this.txtForce.Text = force.ToString();
            this.txtWeight.Text = (force / 9.81).ToString();
        }
    }
}
