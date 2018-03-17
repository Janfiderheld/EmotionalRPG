using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Newtonsoft.Json;
namespace DialogTesting
{
    static class EmotionAPI
    {
        // **********************************************
        // *** Update or verify the following values. ***
        // **********************************************

        // Replace the subscriptionKey string value with your valid subscription key.
        const string subscriptionKey = "159b938d29714f52a98d6feb0fcc6d72";

        private static int counter = 0;

        // Replace or verify the region.
        //
        // You must use the same region in your REST API call as you used to obtain your subscription keys.
        // For example, if you obtained your subscription keys from the westus region, replace 
        // "westcentralus" in the URI below with "westus".
        //
        // NOTE: Free trial subscription keys are generated in the westcentralus region, so if you are using
        // a free trial subscription key, you should not need to change this region.
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/face/v1.0/detect";

        /*
static void Main()
{
Console.WriteLine (EmotionRequest().Result);
Console.WriteLine("\nPlease wait a moment for the results to appear. Then, press Enter to exit...\n");
Console.ReadLine();
}
*/
        /*
        static void Main()
        {
            /* Get the path and filename to process from the user.
            Console.WriteLine("Detect faces:");
            Console.Write("Enter the path to an image with faces that you wish to analyze: ");
            string imageFilePath = Console.ReadLine();
           

            string imageFilePath = "C://Users//Alissa//Pictures//Camera Roll//testfoto.jpg";
            VideoCapture capture = new VideoCapture();
            Bitmap image = capture.QueryFrame().Bitmap;
            image.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

            // Execute the REST API call.
            MakeAnalysisRequest(imageFilePath);

            Console.WriteLine("\nPlease wait a moment for the results to appear. Then, press Enter to exit...\n");
            Console.ReadLine();
        }*/

        public static async Task<EmotionEnum> EmotionRequest()
        {
            EmotionEnum e = EmotionEnum.Neutral;
            string imageFilePath = Environment.ExpandEnvironmentVariables("C:\\Users\\%USERPROFILE%\\Pictures\\EmotionalRPG\\reaktion_0" + counter + ".jpg");
            // string imageFilePath = "C:\\Users\\Alissa\\Pictures\\Camera Roll\\testfoto_ " + counter + ".jpg";
            VideoCapture capture = new VideoCapture();
            Bitmap image = capture.QueryFrame().Bitmap;
            capture.Dispose();
            try
            {
                Bitmap copy = new Bitmap(image);
                image.Dispose();
                copy.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                copy.Dispose();
                // image = (Bitmap) Bitmap.FromStream(new MemoryStream(File.ReadAllBytes(imageFilePath)));
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // Execute the REST API call.
            e = await MakeAnalysisRequest(imageFilePath);
            counter++;
            return e;
        }
        /// <summary>
        /// Gets the analysis of the specified image file by using the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file.</param>
        static async Task<EmotionEnum> MakeAnalysisRequest(string imageFilePath)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses,emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();
                /*
                // Display the JSON response.
                Console.WriteLine("\nResponse:\n");
                Console.WriteLine(JsonPrettyPrint(contentString));
                */
                content.Dispose();
                return ChooseEmotion(contentString);
                
                }
        }


        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }


        /// <summary>
        /// Formats the given JSON string by adding line breaks and indents.
        /// </summary>
        /// <param name="json">The raw JSON string to format.</param>
        /// <returns>The formatted JSON string.</returns>
        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            List<RootObject> faces = JsonConvert.DeserializeObject<List<RootObject>>(json);
            /*json = json.Replace(Environment.NewLine, "").Replace("\t", "");
            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();*/
            /*foreach(RootObject face in faces){
                if (face.faceAttributes.emotion.anger > 0.5)
                {
                    return "angry";
                } else if (face.faceAttributes.emotion.contempt > 0.5)
                {
                    return "contempt";
                } else if (face.faceAttributes.emotion.disgust > 0.5)
                {
                    return "disgusted";
                } else if (face.faceAttributes.emotion.fear > 0.5)
                {
                    return "fearful";
                } else if (face.faceAttributes.emotion.happiness > 0.5)
                {
                    return "happy";
                } else if (face.faceAttributes.emotion.sadness > 0.5)
                {
                    return "sad";
                } else if (face.faceAttributes.emotion.surprise > 0.5)
                {
                    return "surprised";
                } else
                {
                    return "neutral";
                }
            }
            */
            String s = "";
            foreach (RootObject face in faces)
            {
                s += "Anger: " + face.faceAttributes.emotion.anger + "\n";
                s += "Contempt: " + face.faceAttributes.emotion.contempt + "\n";
                s += "Disgust: " + face.faceAttributes.emotion.disgust + "\n";
                s += "Fear: " + face.faceAttributes.emotion.fear + "\n";
                s += "Happy: " + face.faceAttributes.emotion.happiness + "\n";
                s += "Neutral: " + face.faceAttributes.emotion.neutral + "\n";
                s += "Sadness: " + face.faceAttributes.emotion.sadness + "\n";
                s += "Surprise: " + face.faceAttributes.emotion.surprise + "\n";

            }
            return s;
        }




        private static EmotionEnum ChooseEmotion(string json)
        {
            EmotionEnum emo = EmotionEnum.Neutral;
            if (string.IsNullOrEmpty(json))
                return emo;

            List<RootObject> faces = JsonConvert.DeserializeObject<List<RootObject>>(json);
            if (faces.Count == 0)
            {
                return emo;
            }
            SortedList<EmotionEnum, double> emos = new SortedList<EmotionEnum, double>();
            
            emos.Add(EmotionEnum.Anger, faces[0].faceAttributes.emotion.anger);
            emos.Add(EmotionEnum.Contempt, faces[0].faceAttributes.emotion.contempt);
            emos.Add(EmotionEnum.Disgust, faces[0].faceAttributes.emotion.disgust);
            emos.Add(EmotionEnum.Fear, faces[0].faceAttributes.emotion.fear);
            emos.Add(EmotionEnum.Happy, faces[0].faceAttributes.emotion.happiness);
            emos.Add(EmotionEnum.Neutral, faces[0].faceAttributes.emotion.neutral);
            emos.Add(EmotionEnum.Sadness, faces[0].faceAttributes.emotion.sadness);
            emos.Add(EmotionEnum.Surprise, faces[0].faceAttributes.emotion.surprise);

            double max = 0.0;
            double current = 0.0;
            foreach (EmotionEnum e in emos.Keys)
            {
                emos.TryGetValue(e, out current);
                if (current > max)
                {
                    max = current;
                    emo = e;
                }
            }

            return emo;
        }
    }
}