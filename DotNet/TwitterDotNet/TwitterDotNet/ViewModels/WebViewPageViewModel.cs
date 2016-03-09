using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using TwitterDotNet.Services.TweetinviAPI;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace TwitterDotNet.ViewModels
{
    public class WebViewPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            string targetUri = parameter.ToString();

            WebViewUriSource = new Uri(targetUri);
            

            await Task.CompletedTask;
        }

        private Uri _webviewUriSource;
        public Uri WebViewUriSource
        {
            get { return _webviewUriSource; }
            set { _webviewUriSource = value; RaisePropertyChanged(); }
        }

    }
}
