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
using TwitterDotNet.Services.ImageLoader;
using Windows.UI.Xaml.Media;

namespace TwitterDotNet.ViewModels
{
    public class UserProfilPageViewModel : ViewModelBase
    {
        private readonly IImageLoader _imageLoader;

        public UserProfilPageViewModel()
        {
            _imageLoader = new ImageLoader();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            var loggedUser = User.GetAuthenticatedUser();

            if (!String.IsNullOrEmpty(loggedUser.ProfileImageUrl))
            {
                ProfilPicture = await _imageLoader.GetFromUrl(loggedUser.ProfileImageUrlFullSize);
                BannerPicture = await _imageLoader.GetFromUrl(loggedUser.ProfileBannerURL);
            }
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);
        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        private ImageSource _profilPicture;
        private ImageSource _bannerPicture;

        public ImageSource ProfilPicture
        {
            get { return _profilPicture; }
            set { _profilPicture = value; RaisePropertyChanged(); }
        }
        public ImageSource BannerPicture
        {
            get { return _bannerPicture; }
            set { _bannerPicture = value; RaisePropertyChanged(); }
        }

    }
}
