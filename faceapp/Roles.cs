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
    public partial class Roles : Form
    {
        public Roles()
        {
            InitializeComponent();
        }

        private void rolesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {

            
            this.Validate();
            this.rolesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

                MessageBox.Show("Rol Actualizado Con Exito");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Roles_Load(object sender, EventArgs e)
        {
            try
            {
           
            // TODO: This line of code loads data into the 'dataSet1.Roles' table. You can move, or remove it, as needed.
            this.rolesTableAdapter.Fill(this.dataSet1.Roles);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
