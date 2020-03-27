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
    public partial class ReporteEncontrados : Form
    {
        public ReporteEncontrados()
        {
            InitializeComponent();
        }

        private void reporteEncontradoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.reporteEncontradoBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {
          
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {

            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.reporteEncontradoTableAdapter.Fill(this.dataSet1.ReporteEncontrado, Vfecha1ToolStripTextBox.Value.ToShortDateString(), fecha2ToolStripTextBox.Value.ToShortDateString());
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
