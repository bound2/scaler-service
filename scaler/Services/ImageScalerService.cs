using System;
using System.Net;
using NetVips;

namespace scaler.Services
{
    public class ImageScalerService
    {
        private readonly WebClient client;

        public ImageScalerService()
        {
            client = new WebClient();
        }

        public void Scale(string url)
        {
            using var stream = client.OpenRead(url);
            using var image = Image.NewFromStream(stream, access: Enums.Access.Random);

            var smallImage = image.Resize(0.25);
            var mediumImage = image.Resize(0.5);
            var largeImage = image.Resize(0.75);

            // TODO upload to Minio / S3 instead and return in API
            // TODO support HEIC and HEIF
            var filename = Guid.NewGuid().ToString();;
            smallImage.WriteToFile($"output/${filename}-s.jpg");
            mediumImage.WriteToFile($"output/${filename}-m.jpg");
            largeImage.WriteToFile($"output/${filename}-l.jpg");
            image.WriteToFile($"output/${filename}.jpg");
        }
    }
}
