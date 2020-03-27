using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

namespace faceapp
{
    public partial class Login : Form
    {
        IFaceClient client;
        int idusuario;
        public Login(IFaceClient x)
        {
            client = x;

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var x = new DataSet1TableAdapters.UsuariosTableAdapter();
            idusuario = Convert.ToInt32(x.GetIdUsuario(textBox2.Text, textBox1.Text));
            if (Convert.ToInt32(x.GetRolUsuario(idusuario)) == 1)
            {
                var form  = new PantallaPrincipal(client, idusuario);

                form.menuStrip1.Visible = true;
                this.Hide();
                form.ShowDialog();

            
            }
            else if (Convert.ToInt32(x.GetRolUsuario(Convert.ToInt32(Convert.ToInt32(x.GetIdUsuario(textBox2.Text, textBox1.Text))))) == 2)
            {
              
                var form = new PantallaPrincipal(client, idusuario);
                form.menuStrip1.Visible = false;

                this.Hide();
                form.ShowDialog();

            }
            else
            {
                MessageBox.Show("Usuario Invalido");
            }
           
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Registrar().ShowDialog();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Registrar().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var x = new DataSet1TableAdapters.UsuariosTableAdapter();
            idusuario = Convert.ToInt32(x.GetIdUsuario(textBox2.Text, textBox1.Text));
            if (Convert.ToInt32(x.GetRolUsuario(idusuario)) == 1)
            {
                var form = new PantallaPrincipal(client, idusuario);

                form.menuStrip1.Visible = true;
                this.Hide();
                form.ShowDialog();


            }
            else if (Convert.ToInt32(x.GetRolUsuario(idusuario)) == 2)
            {

                var form = new PantallaPrincipal(client, idusuario);
                form.menuStrip1.Visible = false;

                this.Hide();
                form.ShowDialog();

            }
            else
            {
                MessageBox.Show("Usuario Invalido");
            }


        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
