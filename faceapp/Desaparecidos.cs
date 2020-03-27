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
    public partial class Desaparecidos : Form
    {
        int idusuario;
        public Desaparecidos(int idusuario)
        {
            this.idusuario = idusuario;
            InitializeComponent();
        }
        string rutaimagen = "";
        private void desaparecidosBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
            this.desaparecidosBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dataSet1);

                MessageBox.Show("Desaparecido Actualizado con Exito!");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

}

private void Desaparecidos_Load(object sender, EventArgs e)
        {

            try
            {

                // TODO: This line of code loads data into the 'dataSet1.Usuarios' table. You can move, or remove it, as needed.
                this.usuariosTableAdapter.Fill(this.dataSet1.Usuarios);
            // TODO: This line of code loads data into the 'dataSet1.Desaparecidos' table. You can move, or remove it, as needed.
            this.desaparecidosTableAdapter.Fill(this.dataSet1.Desaparecidos, idusuario);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (desaparecidosDataGridView.RowCount > 0)
                {


                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        PictureBox img = new PictureBox();
                        rutaimagen = openFileDialog1.FileName;
                        img.Image = Image.FromFile(rutaimagen);
                        byte[] byteArrayImagen = ImageToByteArray(img.Image);
                        desaparecidosDataGridView.CurrentRow.Cells[5].Value = byteArrayImagen;
                    }

                }
                else
                {
                    MessageBox.Show("No hay Registro nuevo o seleccionado");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido");
            }
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

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            desaparecidosDataGridView.CurrentRow.Cells[8].Value = true;
            desaparecidosDataGridView.CurrentRow.Cells[7].Value = idusuario;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
