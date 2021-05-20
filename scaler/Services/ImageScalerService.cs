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

            VOption kwargs = new VOption();
            kwargs.Add("n", -1); // Required for animated images

            using var image = Image.NewFromStream(
                stream,
                access: Enums.Access.Random,
                kwargs: kwargs
            );

            var format = getImageFormat(image.Get("vips-loader").ToString());

            using var smallImage = image.Resize(0.25);
            using var mediumImage = image.Resize(0.5);
            using var largeImage = image.Resize(0.75);

            // TODO upload to Minio / S3 instead and return in API
            var filename = Guid.NewGuid().ToString();;
            smallImage.WriteToFile($"output/{filename}-s.{format}");
            mediumImage.WriteToFile($"output/{filename}-m.{format}");
            largeImage.WriteToFile($"output/{filename}-l.{format}");
            image.WriteToFile($"output/{filename}.{format}");
        }

        private string getImageFormat(string loader)
        {
            return loader switch
            {
                "jpegload_source" => "jpg",
                "heifload_source" => "heif",
                "pngload_source" => "png",
                "gifload_source" => "gif",
                "webpload_source" => "webp",
                _ => throw new Exception($"Unknown image loader: {loader}"),
            };
        }
    }
}
