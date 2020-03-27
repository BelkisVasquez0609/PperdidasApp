using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faceapp
{
    public partial class ReporteDesaparecidos : Form
    {
        public ReporteDesaparecidos()
        {
            InitializeComponent();
        }

   
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.reporteDesaparecidoTableAdapter.Fill(this.dataSet1.ReporteDesaparecido, fecha1ToolStripTextBox.Value.ToShortDateString(), fecha2ToolStripTextBox.Value.ToShortDateString());
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
