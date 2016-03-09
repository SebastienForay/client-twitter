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
using TwitterDotNet.Services.AccountManager;

namespace TwitterDotNet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            AppCredentials = new TwitterCredentials(TweetinviData.ConsumerKey, TweetinviData.ConsumerSecret);
            var url = CredentialsCreator.GetAuthorizationURL(AppCredentials);
            Uri targeturi = new Uri(url);

            NavigationService.Navigate(typeof(Views.LoginPage), targeturi);
            
            await Task.CompletedTask;
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
        private void GotoProfilPage() => NavigationService.Navigate(typeof(Views.UserProfilPage));

        private static TweetinviData _tweetinviData = new TweetinviData();
        private static TwitterCredentials _appCredentials;
        public static TweetinviData TweetinviData { get { return _tweetinviData; } set { _tweetinviData = value; } }
        public static TwitterCredentials AppCredentials { get { return _appCredentials; } set { _appCredentials = value; } }
        
        private RelayCommand _gotoProfilPageCommand;
        public RelayCommand GotoProfilPageCommand
        {
            get
            {
                if (_gotoProfilPageCommand == null)
                    _gotoProfilPageCommand = new RelayCommand(GotoProfilPage);

                return _gotoProfilPageCommand;
            }
            set { _gotoProfilPageCommand = value; RaisePropertyChanged(); }
        }
    }
}

