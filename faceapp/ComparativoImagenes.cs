using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using SharpDX.Direct2D1.Effects;
using System.Windows;
using System.Text;

namespace faceapp
{
    public partial class ComparativoImagenes : Form
    {
        IFaceClient clientFace;
        static string porcentaje;
        //const string RECOGNITION_MODEL2 = RecognitionModel.Recognition02;
        const string RECOGNITION_MODEL1 = RecognitionModel.Recognition02;

        private IList<DetectedFace> faceList;

        private double resizeFactor;
        private string[] faceDescriptions;
        string filePath1;
        BitmapImage bit1;
        string filePath2;
        BitmapImage bit2;
        string defaultStatusBarText = "";

        public ComparativoImagenes(IFaceClient x)
        {
            clientFace = x;
            InitializeComponent();
        }

        public Stream imagen(Image img)
        {
            try
            {
                var stream = new System.IO.MemoryStream();
                img.Save(stream, ImageFormat.Jpeg);
                stream.Position = 0;
               
                return stream;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                await FindSimilar(clientFace, imagen(pictureBox1.Image), imagen(pictureBox2.Image), RECOGNITION_MODEL1);

                Similitudlbl.Text = porcentaje;

            }
            catch (Exception)
            {

            }
        
        }     

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, Stream img, string RECOGNITION_MODEL1)
        {
            try
            {
                IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithStreamAsync(img, recognitionModel: RECOGNITION_MODEL1);

                return detectedFaces.ToList();
            }
            catch (Exception)
            {
                return null;

            }
            
        }
        public static async Task FindSimilar(IFaceClient client, Stream img1, Stream img2, string RECOGNITION_MODEL1)
        {
            try
            {

           
            IList<Guid?> targetFaceIds = new List<Guid?>();
           
                var faces = await DetectFaceRecognize(client, img2, RECOGNITION_MODEL1);
                targetFaceIds.Add(faces[0].FaceId.Value);
           
            IList<DetectedFace> detectedFaces = await DetectFaceRecognize(client, img1, RECOGNITION_MODEL1);
         
            IList<SimilarFace> similarResults = await client.Face.FindSimilarAsync(detectedFaces[0].FaceId.Value, null, null, targetFaceIds);
            if (similarResults.Count > 0)
            {
                foreach (var similarResult in similarResults)
                {
                    porcentaje = $"{similarResult.Confidence * 100}" + "%";
                }
            }
            else
            {
                MessageBox.Show("No se Encontro Similitud");
            }

            }
            catch (Exception)
            {

            }

        }
              
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath1 = openFileDialog1.FileName;

                    var fileUri1 = new Uri(filePath1);

                    BitmapImage bitmapSource = new BitmapImage();

                    bitmapSource.BeginInit();
                    bitmapSource.CacheOption = BitmapCacheOption.None;
                    bitmapSource.UriSource = fileUri1;
                    bitmapSource.EndInit();

                    bit1 = bitmapSource;
                    pictureBox1.Image = new Bitmap(filePath1);

                }
            }
            catch (Exception)
            {

              
            }
           
        }
    
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath2 = openFileDialog1.FileName;

                    var fileUri2 = new Uri(filePath2);
                    BitmapImage bitmapSource = new BitmapImage();

                    bitmapSource.BeginInit();
                    bitmapSource.CacheOption = BitmapCacheOption.None;
                    bitmapSource.UriSource = fileUri2;
                    bitmapSource.EndInit();
                    bit2 = bitmapSource;
                    pictureBox2.Image = new Bitmap(filePath2);


                }
            }
            catch (Exception)
            {

             
            }
            
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                await DetectarRostro(bit1, pictureBox1.Image, filePath1);
            }
            catch (Exception)
            {

               
            }
       
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                await DetectarRostro(bit2, pictureBox2.Image, filePath2);
            }
            catch (Exception)
            {

               
            }
           
        }
        private async Task DetectarRostro(BitmapImage bitmapSource, Image img, string path)
        {
            try
            {

         
            faceList = await UploadAndDetectFaces(path);

            if (faceList.Count > 0)
            {
                // Prepare to draw rectangles around the faces.
                DrawingVisual visual = new DrawingVisual();
                DrawingContext drawingContext = visual.RenderOpen();
                drawingContext.DrawImage(bitmapSource,
                    new Rect(0, 0, bitmapSource.Width, bitmapSource.Height));
                double dpi = bitmapSource.DpiX;
                // Some images don't contain dpi info.
                resizeFactor = (dpi == 0) ? 1 : 96 / dpi;
                faceDescriptions = new String[faceList.Count];

                for (int i = 0; i < faceList.Count; ++i)
                {
                    DetectedFace face = faceList[i];

                    // Draw a rectangle on the face.
                    drawingContext.DrawRectangle(
                       System.Windows.Media.Brushes.Transparent,
                        new System.Windows.Media.Pen(System.Windows.Media.Brushes.Red, 2),
                        new Rect(
                            face.FaceRectangle.Left * resizeFactor,
                            face.FaceRectangle.Top * resizeFactor,
                            face.FaceRectangle.Width * resizeFactor,
                            face.FaceRectangle.Height * resizeFactor
                            )
                    );

                    // Store the face description.
                    faceDescriptions[i] = FaceDescription(face);
                    defaultStatusBarText = FaceDescription(face);
                }

                drawingContext.Close();

                // Display the image with the rectangle around the face.
                RenderTargetBitmap faceWithRectBitmap = new RenderTargetBitmap(
                    (int)(bitmapSource.PixelWidth * resizeFactor),
                    (int)(bitmapSource.PixelHeight * resizeFactor),
                    96,
                    96,
                    PixelFormats.Pbgra32);

                faceWithRectBitmap.Render(visual);
                //  img = faceWithRectBitmap;

                // Set the status bar text.
                MessageBox.Show(defaultStatusBarText);
            }
            }
            catch (Exception)
            {

               
            }
        }

        private async Task<IList<DetectedFace>> UploadAndDetectFaces(string imageFilePath)
        {
            try
            {
                // The list of Face attributes to return.
                IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    FaceAttributeType.Gender, FaceAttributeType.Age,
                    FaceAttributeType.Smile, FaceAttributeType.Emotion,
                    FaceAttributeType.Glasses, FaceAttributeType.Hair
                };

            // Call the Face API.
            
                using (Stream imageFileStream = File.OpenRead(imageFilePath))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.
                    IList<DetectedFace> faceList =
                        await clientFace.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);
                    return faceList;
                }
            }
            // Catch and display Face API errors.
            catch (APIErrorException f)
            {
                MessageBox.Show(f.Message);
                return new List<DetectedFace>();
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
                return new List<DetectedFace>();
            }
        }

        private string FaceDescription(DetectedFace face)
        {
            try
            {

           
            StringBuilder sb = new StringBuilder();

            sb.Append("Face: ");

            // Add the gender, age, and smile.
            sb.Append(face.FaceAttributes.Gender);
            sb.Append(", ");
            sb.Append(face.FaceAttributes.Age);
            sb.Append(", ");
            sb.Append(String.Format("smile {0:F1}%, ", face.FaceAttributes.Smile * 100));

            // Add the emotions. Display all emotions over 10%.
            sb.Append("Emotion: ");
            Emotion emotionScores = face.FaceAttributes.Emotion;
            if (emotionScores.Anger >= 0.1f) sb.Append(
                String.Format("anger {0:F1}%, ", emotionScores.Anger * 100));
            if (emotionScores.Contempt >= 0.1f) sb.Append(
                String.Format("contempt {0:F1}%, ", emotionScores.Contempt * 100));
            if (emotionScores.Disgust >= 0.1f) sb.Append(
                String.Format("disgust {0:F1}%, ", emotionScores.Disgust * 100));
            if (emotionScores.Fear >= 0.1f) sb.Append(
                String.Format("fear {0:F1}%, ", emotionScores.Fear * 100));
            if (emotionScores.Happiness >= 0.1f) sb.Append(
                String.Format("happiness {0:F1}%, ", emotionScores.Happiness * 100));
            if (emotionScores.Neutral >= 0.1f) sb.Append(
                String.Format("neutral {0:F1}%, ", emotionScores.Neutral * 100));
            if (emotionScores.Sadness >= 0.1f) sb.Append(
                String.Format("sadness {0:F1}%, ", emotionScores.Sadness * 100));
            if (emotionScores.Surprise >= 0.1f) sb.Append(
                String.Format("surprise {0:F1}%, ", emotionScores.Surprise * 100));

            // Add glasses.
            sb.Append(face.FaceAttributes.Glasses);
            sb.Append(", ");

            // Add hair.
            sb.Append("Hair: ");

            // Display baldness confidence if over 1%.
            if (face.FaceAttributes.Hair.Bald >= 0.01f)
                sb.Append(String.Format("bald {0:F1}% ", face.FaceAttributes.Hair.Bald * 100));

            // Display all hair color attributes over 10%.
            IList<HairColor> hairColors = face.FaceAttributes.Hair.HairColor;
            foreach (HairColor hairColor in hairColors)
            {
                if (hairColor.Confidence >= 0.1f)
                {
                    sb.Append(hairColor.Color.ToString());
                    sb.Append(String.Format(" {0:F1}% ", hairColor.Confidence * 100));
                }
            }

            // Return the built string.
            return sb.ToString();
            }
            catch (Exception)
            {

                return null;
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception)
            {

              
            }
          
        }
    }
}
