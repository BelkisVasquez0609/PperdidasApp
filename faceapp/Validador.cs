using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace faceapp
{
    public partial class Validador : Form
    {
        int iddesaparecido;
        int idsimilar;
        DataSet1TableAdapters.RegistroSimilitudTableAdapter z;
        DataSet1TableAdapters.DesaparecidosTableAdapter x;

        public Validador()
        {
            InitializeComponent();
            z = new DataSet1TableAdapters.RegistroSimilitudTableAdapter();
            x = new DataSet1TableAdapters.DesaparecidosTableAdapter();
        }
        public byte[] ImageToByteArray(System.Drawing.Image imagen)
        {
            MemoryStream ms = new MemoryStream();
            imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            return Image.FromStream(ms);
        }        
        private void GetImagen2()
        {
            var s = z.Getimagenes(idsimilar);
            pictureBox2.Image = ByteArrayToImage((byte[])s);
            panel1.Visible = true;
        }
        private void GetImagen1()
        {
             var y = x.ListaImagenes(iddesaparecido);
            pictureBox1.Image = ByteArrayToImage((byte[])y);
        }
        private void button1_Click(object sender, EventArgs e)
        {

            idsimilar = Convert.ToInt32(textBox1.Text);
            iddesaparecido = Convert.ToInt32(z.GetIdDesaparecida(idsimilar));
            label6.Text = Math.Round(Convert.ToDouble(z.getsimilitud(idsimilar)),2).ToString() + "%";
            label8.Text = z.GetUbication(idsimilar);
            label10.Text = Convert.ToDateTime(z.GetHorabyid(idsimilar)).ToLongTimeString();
            label11.Text = Convert.ToDateTime(z.GetHorabyid(idsimilar)).ToShortDateString();
            if (iddesaparecido > 0)
            {
                GetImagen1();
                GetImagen2();
            }
            else
            {
                MessageBox.Show("Codigo Invalido!");
            }
          

        }
        private void button2_Click(object sender, EventArgs e)
        {
            x.Encontrado(iddesaparecido);

            MessageBox.Show("Desaparecido Encontrado");
            panel1.Visible = false;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
