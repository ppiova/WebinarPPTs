using BlazorOCRImages.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BlazorOCRImages.Services
{
    public class ComputerVisionService
    {
        static string subscriptionKey;
        static string endpoint;
        static string uriBase;

        public ComputerVisionService()
        {
            subscriptionKey = "";
            endpoint = "https://xamarincomputevision.cognitiveservices.azure.com/";
            uriBase = endpoint + "vision/v3.2/ocr";
        }

        public async Task<OcrResultDTO> GetTextFromImage(byte[] imageFileBytes)
        {
            StringBuilder sb = new StringBuilder();
            OcrResultDTO ocrResultDTO = new OcrResultDTO();
            try
            {
                string JSONResult = await ReadTextFromStream(imageFileBytes);

                OcrResult ocrResult = JsonConvert.DeserializeObject<OcrResult>(JSONResult);

                    foreach (OcrLine ocrLine in ocrResult.Regions[0].Lines)
                    {
                        foreach (OcrWord ocrWord in ocrLine.Words)
                        {
                            sb.Append(ocrWord.Text);
                            sb.Append(' ');
                        }
                        sb.AppendLine();
                    }
               
                ocrResultDTO.DetectedText = sb.ToString();
                ocrResultDTO.Language = ocrResult.Language;
                return ocrResultDTO;
            }
            catch
            {
                ocrResultDTO.DetectedText = "Error occurred. Try again";
                ocrResultDTO.Language = "unk";
                return ocrResultDTO;
            }
        }

       

        static async Task<string> ReadTextFromStream(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                string requestParameters = "language=unk&detectOrientation=true";
                string uri = uriBase + "?" + requestParameters;
                HttpResponseMessage response;

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }

                string contentString = await response.Content.ReadAsStringAsync();
                string result = JToken.Parse(contentString).ToString();
                return result;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
