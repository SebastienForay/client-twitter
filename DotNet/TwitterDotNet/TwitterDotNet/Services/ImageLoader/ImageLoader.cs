using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace TwitterDotNet.Services.ImageLoader
{
    public class ImageLoader : IImageLoader
    {
        public async Task<BitmapImage> GetFromUrl(string url)
        {
            using (var client = new HttpClient())
            {
                var imageData = await client.GetByteArrayAsync(url);
                using (var ms = new MemoryStream(imageData))
                {
                    var image = new BitmapImage();
                    await image.SetSourceAsync(ms.AsRandomAccessStream());
                    return image;
                }
            }
        }
    }
}
