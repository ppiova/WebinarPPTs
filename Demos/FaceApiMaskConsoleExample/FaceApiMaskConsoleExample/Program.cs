using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FaceApiMaskConsoleExample
{
    class Program
    {
        // From your Face subscription in the Azure portal, get your subscription key and endpoint.
        static string key = Environment.GetEnvironmentVariable("FACE_SUBSCRIPTION_KEY_HERE");
        static string endPoint = Environment.GetEnvironmentVariable("FACE_ENDPOINT_HERE");


        // URL for the images with mask.
        static string imageBaseUrl = "https://demosai.blob.core.windows.net/faceapi/";

        static void Main(string[] args)
        {
            //faces wearing masks compared with model 3
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;

            // Authenticate.
            IFaceClient client = Authenticate(endPoint, key);

            // Detect - Mask from faces.
            DetectMask(client, imageBaseUrl, RECOGNITION_MODEL3).Wait();

        }
        private static IFaceClient Authenticate(string endPoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endPoint };
        }
        private static async Task DetectMask(IFaceClient client, string url, string recognitionModel)
        {
            Console.WriteLine("========DETECT MASK========");
            Console.WriteLine();

            // Create a list of images
            List<string> imageFileNames = new List<string>
                            {
                                "MaskJuanchi-01.jpeg",     // a man using mask NoseAndMouthCovered
							    "MaskJuanchi-02.jpg",     // a man using mask NoseAndMouthCovered false
								"Piova-Lentes.jpeg" ,        // piova using glasses and mask
                                "personas-barbijos.jpg"     //
                            };
            foreach (var imageFileName in imageFileNames)
            {
                IList<DetectedFace> detectedFaces;

                // Detect faces with attributes Mask and HeadPose from image url.
                detectedFaces = await client.Face.DetectWithUrlAsync($"{url}{imageFileName}",
                        returnFaceAttributes: new List<FaceAttributeType>
                        { FaceAttributeType.Mask,
                        FaceAttributeType.HeadPose},
                        // We specify detection model 3 because we are retrieving attributes of mask.
                        detectionModel: DetectionModel.Detection03,
                        recognitionModel: recognitionModel);
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{imageFileName}`.");
                
                foreach (var face in detectedFaces)
                {
                   
                    Console.WriteLine($"Face attributes for {imageFileName}:");

                    // Get bounding box of the faces
                    Console.WriteLine($"Rectangle(Left/Top/Width/Height) : {face.FaceRectangle.Left} {face.FaceRectangle.Top} {face.FaceRectangle.Width} {face.FaceRectangle.Height}");
                    //Mask type returns 'noMask', 'faceMask', 'otherMaskOrOcclusion', or 'uncertain'.  
                    Console.WriteLine($"Type Mask: {face.FaceAttributes.Mask.Type}");
                    //Value returns a boolean 'noseAndMouthCovered' indicating whether nose and mouth are covered.
                    Console.WriteLine($"Nose And Mouth Covered: {face.FaceAttributes.Mask.NoseAndMouthCovered}");
                    Console.WriteLine("================");
                }
            }

        }
       
    }
}
