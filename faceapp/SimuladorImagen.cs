using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Net;
using System.Net.Mail;

namespace faceapp
{
   public partial class SimuladorImagen : Form
   {
    Image img2;
    Image img1;
    static double porcent;
    string fileUri2;
    BitmapImage bit2;
    static string porcentaje;
    IFaceClient clientFace;
    const string RECOGNITION_MODEL1 = RecognitionModel.Recognition02;
    GeoCoordinateWatcher watcher;
    public SimuladorImagen(IFaceClient x)
        {
            clientFace = x;
            InitializeComponent();
             watcher = new GeoCoordinateWatcher();

        }
    public void SendCorreos(string correo2, string asunto, string cuerpo)
        {
            var fromAddress = new MailAddress("belvasquez06@gmail.com", "Vecindario Seguro");
            var toAddress = new MailAddress(correo2, correo2);
            string fromPassword = "abcd123@A";
            string subject = asunto;
            string body = cuerpo;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    public byte[] Imglist(int id)
        {

            var x = new DataSet1TableAdapters.DesaparecidosTableAdapter();
            var y = x.ListaImagenes(id);
            return (byte[]) y;
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
    private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
            var x = new DataSet1TableAdapters.DesaparecidosTableAdapter();
            var z = new DataSet1TableAdapters.RegistroSimilitudTableAdapter();
            var y = x.GetDataBy3();
            img1 = pictureBox2.Image;

            for (int i = 0; i < y.Count; i++)
            {
                if (Imglist(y[i].id) != null)
                {
                    img2 = ByteArrayToImage(Imglist(y[i].id));

                    await FindSimilar(clientFace, imagen(img1), imagen(img2), RECOGNITION_MODEL1);

                    if (porcent > 55)
                    {
                        MessageBox.Show("Esta Imagen es parecida con un " + porcent + "%" + " con el desaparecido: " + x.GetNombreDesaparecido(y[i].id) + " Con el Correo: " + x.GetCorreoDesaparecida(y[i].id));
                        z.InsertQueryRegistroSimilutud(ImageToByteArray(img1), y[i].id, DateTime.Now, GetLocationProperty(), Convert.ToDouble(porcent));
                        MessageBox.Show(GetLocationProperty());
                        SendCorreos(x.GetCorreoDesaparecida(y[i].id).ToString(), "Encontramos tu desaparecido", "hay Imagen es parecida con un " + porcent + "% de similitud" + " con el desaparecido: " + x.GetNombreDesaparecido(y[i].id) + ",\n en la siguiente ubicacion " + GetLocationProperty() + " Verifique su aplicacion y use el codigo de verificacion: " + z.GetUltimoID());
                        break;
                    }

                    }
                }
                if (porcent <= 55)
                    {
                        MessageBox.Show("No se Encontraron Similitudes con ningun desaparecido");
                    }

        

            button1.Visible = false;
            button2.Visible = true;
            }
            catch (Exception)
            {

                
            }
        }
    public Stream imagen(Image img)
        {
            var stream = new System.IO.MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
        }
    private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, Stream img, string RECOGNITION_MODEL1)
        {
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithStreamAsync(img, recognitionModel: RECOGNITION_MODEL1);

            return detectedFaces.ToList();
        }
    public static async Task FindSimilar(IFaceClient client, Stream img1, Stream img2, string RECOGNITION_MODEL1)
        {

            IList<Guid?> targetFaceIds = new List<Guid?>();

            var faces = await DetectFaceRecognize(client, img2, RECOGNITION_MODEL1);
            targetFaceIds.Add(faces[0].FaceId.Value);

            IList<DetectedFace> detectedFaces = await DetectFaceRecognize(client, img1, RECOGNITION_MODEL1);

            IList<SimilarFace> similarResults = await client.Face.FindSimilarAsync(detectedFaces[0].FaceId.Value, null, null, targetFaceIds);

            foreach (var similarResult in similarResults)
            {
                porcentaje = $"{similarResult.Confidence * 100}" + "%";
                porcent = similarResult.Confidence * 100;
            }


        }
    private void pictureBox2_Click(object sender, EventArgs e)
        {
         
        }
    private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var filePath2 = openFileDialog1.FileName;

                var fileUri2 = new Uri(filePath2);
                BitmapImage bitmapSource = new BitmapImage();

                bitmapSource.BeginInit();
                bitmapSource.CacheOption = BitmapCacheOption.None;
                bitmapSource.UriSource = fileUri2;
                bitmapSource.EndInit();
                bit2 = bitmapSource;
                pictureBox2.Image = new Bitmap(filePath2);

                button1.Visible = true;
                button2.Visible = false;
            }
        }
    public string GetLocationProperty()
        {
          
            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            
            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {

                return "Latitude: "+ coord.Latitude+"," +"Longitude: "  + coord.Longitude;
            }
            else
            {
                return "Unknown latitude and longitude.";
            }
        }
    private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetLocationProperty());
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
