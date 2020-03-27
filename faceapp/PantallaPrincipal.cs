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
    public partial class PantallaPrincipal : Form
    {
        IFaceClient clientFace;
        int idusuario;
        public PantallaPrincipal(IFaceClient x,int idusuario)
        {
            this.idusuario = idusuario;
            clientFace = x;
            InitializeComponent();
        }

        private void comparadorDeImagenesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ComparativoImagenes(clientFace).ShowDialog();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new usuarios().ShowDialog();
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Roles().ShowDialog();
        }

        private void simulaodrDeCamaraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SimuladorImagen(clientFace).ShowDialog();
        }

        private void personasDesaparecidasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ReporteDesaparecidos().ShowDialog();
        }

        private void personasEncontradasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ReporteEncontrados().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Validador().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Desaparecidos(idusuario).ShowDialog();
        }

        private void PantallaPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ValidarReconocimientoBtn_Click(object sender, EventArgs e)
        {
           
           new Validador().ShowDialog();
        }

        private void RegitrarPersonaDesaparecidaBtn_Click(object sender, EventArgs e)
        {
            new Desaparecidos(idusuario).ShowDialog();
        }
        private void AbrirFormEnPanel(object formhija)
        {
            if (this.panelfondo.Controls.Count > 0)
                this.panelfondo.Controls.RemoveAt(0);
            Form fh = formhija as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panelfondo.Controls.Add(fh);
            this.panelfondo.Tag = fh;
            fh.Show();

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Login form = new Login(clientFace);
            form.ShowDialog();
         
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void PantallaPrincipal_Load(object sender, EventArgs e)
        {

        }
    }
}
