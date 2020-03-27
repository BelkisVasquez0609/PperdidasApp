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
    public partial class Registrar : Form
    {
        public Registrar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

           
            var x = new DataSet1TableAdapters.UsuariosTableAdapter();

            x.InsertQueryUsuarios(textBox3.Text,textBox2.Text,textBox1.Text,2);

            limpiar();

            MessageBox.Show("Usuario Registrado Con Exito!");

                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void limpiar()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {


                var x = new DataSet1TableAdapters.UsuariosTableAdapter();

                x.InsertQueryUsuarios(textBox3.Text, textBox2.Text, textBox1.Text, 2);

                limpiar();

                MessageBox.Show("Usuario Registrado Con Exito!");

                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           ;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
