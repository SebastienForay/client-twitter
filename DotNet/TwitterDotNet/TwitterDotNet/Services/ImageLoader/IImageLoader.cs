using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace TwitterDotNet.Services.ImageLoader
{
    public interface IImageLoader
    {
        Task<BitmapImage> GetFromUrl(string url);
    }
}