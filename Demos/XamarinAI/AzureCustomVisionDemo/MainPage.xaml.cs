using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AzureCustomVisionDemo
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async Task<Stream> PickImage()
        {
            await CrossMedia.Current.Initialize();

            var pickOrCamera = await DisplayActionSheet("¿Quieres elegir una foto de la galería o tomar una foto con la cámara?", "Cancelar", null, "Elegir de la galería", "Tomar con la cámara");

            MediaFile pickedImage = null;

            switch(pickOrCamera)
            {
                case "Cancel":
                    return null;
                case "Elegir de la galería":
                    pickedImage = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small
                    });
                    break;
                case "Tomar con la cámara":
                    if (!CrossMedia.Current.IsCameraAvailable)
                    {
                        await DisplayAlert("Cámara no disponible", "Lo sentimos, la cámara no parece disponible. ¿Quizás está ejecutando esta aplicación en un emulador o simulador donde la vista de la cámara no es compatible?", "OK");

                        break;
                    }

                    pickedImage = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        SaveToAlbum = false
                    });
                    break;
            }

            if (pickedImage == null)
                return null;

            CapturedImage.Source = ImageSource.FromStream(() => pickedImage.GetStreamWithImageRotatedForExternalStorage());

            return pickedImage.GetStreamWithImageRotatedForExternalStorage();
        }

        void SetLabel(string text)
        {
            DescriptionLabel.IsVisible = true;
            TagLabel.Text = $"A {text}";
        }

        // This part will run your image against the models locally with CoreML and TensorFlow
        async void Button_Local_Clicked(object sender, EventArgs e)
        {
            var imageStream = await PickImage();

            if (imageStream == null)
                return;

            var result = await DependencyService.Resolve<IPlatformPredictionService>().Classify(imageStream);

            // If you go with the OnDeviceCustomVision plugin you can simply do this as well
            // var tags = await CrossImageClassifier.Current.ClassifyImage(imageStream);
            // var result = tags.OrderByDescending(t => t.Probability).First().Tag;
            // More info here: https://github.com/jimbobbennett/Xam.Plugins.OnDeviceCustomVision

            DescriptionLabel.IsVisible = true;
            SetLabel($"{result.Tag} ({result.Confidence}%)");
        }

        // This part communicates with the customvision.ai REST services
        async void Button_Azure_Clicked(object sender, EventArgs e)
        {
            var imageStream = await PickImage();

            if (imageStream == null)
                return;

            var client = new CustomVisionPredictionClient
            {
                ApiKey = KeysAndUrls.CustomVisionPredictionApiKey,
                Endpoint = KeysAndUrls.PredictionUrl
            };

            var result = await client.ClassifyImageAsync(KeysAndUrls.ProjectId, KeysAndUrls.IterationName, imageStream);
            var bestResult = result.Predictions.OrderByDescending(p => p.Probability).FirstOrDefault();

            if (bestResult == null)
                return;

            SetLabel($"{bestResult.TagName} ({Math.Round(bestResult.Probability * 100, 2)}%)");
        }
    }
}