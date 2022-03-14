using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;



namespace ComputerVisionExample
{
    class Program
    {

        // Add your Computer Vision subscription key and endpoint
        static string key = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY_HERE");
        static string endPoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");


        static void Main(string[] args)
        {

            //ImagesCollections in local Path
            List<string> imagePaths = new()
            {
                @"C:\Users\ppiov\Downloads\ExamplesCV\FaceAPI\PapaFrancisco.jpg",
                @"C:\Users\ppiov\Downloads\ExamplesCV\FaceAPI\Bill-02.jpg",
                @"C:\Users\ppiov\Downloads\ExamplesCV\MiguelMoto.jpg",
                //@"C:\Users\ppiov\Downloads\ExamplesCV\FaceAPI\ppiova-2020.jpg",
                //@"C:\Users\ppiov\Downloads\ExamplesCV\RiseoftheResistance.jpg"


            };

            //Client Object
            ComputerVisionClient client = new(new ApiKeyServiceClientCredentials(key)) { Endpoint = endPoint };

            foreach (string imagePath in imagePaths)
            {
                AnalyzeImage(client, imagePath).Wait();
            }

            Console.ReadLine();

        }

        private static async Task AnalyzeImage(ComputerVisionClient client, string imagePath)
        {
            var features = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Adult,
                VisualFeatureTypes.Brands,
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Color,
                VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces,
                VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Objects,
                VisualFeatureTypes.Tags

            };

            using Stream stream = File.OpenRead(imagePath);
            ImageAnalysis results = await client.AnalyzeImageInStreamAsync(stream, visualFeatures: features);


            //descriptions
            Console.WriteLine("\nSummary:");
            foreach (var caption in results.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} and confidence {caption.Confidence}");
            }
            //tags
            Console.WriteLine("\nTags:");
            foreach (var tag in results.Tags)
            {
                Console.WriteLine($"{tag.Name}");
            }
            //categories
            Console.WriteLine("\nCategories:");
            foreach (var categories in results.Categories)
            {
                Console.WriteLine($"{categories.Name} and confidence {categories.Score}");
            }

            // Detected faces
            if (null != results.Faces)
            {
                Console.WriteLine("Faces:");
                foreach (var face in results.Faces)
                {
                    Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, {face.FaceRectangle.Top}, " +
                      $"{face.FaceRectangle.Left + face.FaceRectangle.Width}, {face.FaceRectangle.Top + face.FaceRectangle.Height}");
                }
                Console.WriteLine();
            }

            // Adult or racy content, if any.
            if (null != results.Adult)
            {
                Console.WriteLine("\nAdult Content:");
                Console.WriteLine($"Has adult content:{results.Adult.IsAdultContent} with confidence {results.Adult.AdultScore}");
                Console.WriteLine($"Has racy content: {results.Adult.IsRacyContent} with confidence {results.Adult.RacyScore}");
                Console.WriteLine($"Has gory content: {results.Adult.IsGoryContent} with confidence {results.Adult.GoreScore}");
            }

            // COLORS
            if (null != results.Color)
            {
                Console.WriteLine("Color Scheme:");
                Console.WriteLine("Is black and white?: " + results.Color.IsBWImg);
                Console.WriteLine("Accent color: " + results.Color.AccentColor);
                Console.WriteLine("Dominant background color: " + results.Color.DominantColorBackground);
                Console.WriteLine("Dominant foreground color: " + results.Color.DominantColorForeground);
                Console.WriteLine("Dominant colors: " + string.Join(",", results.Color.DominantColors));
                Console.WriteLine();
            }

            // brands
            if (null != results.Brands)
            {
                Console.WriteLine("Brands:");
                foreach (var brand in results.Brands)
                {
                    Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                      $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
                }
                Console.WriteLine();
            }
            // Popular landmarks in image, if any.
            if (null != results.Categories)
            {
                Console.WriteLine("Landmarks:");
                foreach (var category in results.Categories)
                {
                    if (category.Detail?.Landmarks != null)
                    {
                        foreach (var landmark in category.Detail.Landmarks)
                        {
                            Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                        }
                    }
                }
                Console.WriteLine();
            }

            // Celebrities in image, if any.
            if (null != results.Categories)
            {
                Console.WriteLine("Celebrities:");
                foreach (var category in results.Categories)
                {
                    if (category.Detail?.Celebrities != null)
                    {
                        foreach (var celeb in category.Detail.Celebrities)
                        {
                            Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                              $"{celeb.FaceRectangle.Top},{celeb.FaceRectangle.Height},{celeb.FaceRectangle.Width}");
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
