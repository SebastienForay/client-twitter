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
    public class MainPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            //Auth.SetUserCredentials(TweetinviData.ConsumerKey, TweetinviData.ConsumerSecret, TweetinviData.AccessToken, TweetinviData.AccessTokenSecret);
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



        private void GotoProfilPage() => NavigationService.Navigate(typeof(Views.UserProfilPage), 0);

        private TweetinviData _tweetinviData = new TweetinviData();
        public TweetinviData TweetinviData { get { return _tweetinviData; } set { _tweetinviData = value; } }


        private string _tweetText;
        private string _pinCode;
        public string TweetText { get { return _tweetText; } set { _tweetText = value; RaisePropertyChanged(); } }
        public string PinCode { get { return _pinCode; } set { _pinCode = value; RaisePropertyChanged(); } }

        private TwitterCredentials _appCredentials;
        public TwitterCredentials AppCredentials { get { return _appCredentials; } set { _appCredentials = value; } }

        private Visibility _connectButtonVisibility = Visibility.Visible;
        private Visibility _pincodeTextboxVisibility = Visibility.Collapsed;
        private Visibility _validateButtonVisibility = Visibility.Collapsed;
        private Visibility _tweetTextTextboxVisibility = Visibility.Collapsed;
        private Visibility _tweetButtonVisibility = Visibility.Collapsed;
        private Visibility _gotoProfilButtonVisibility = Visibility.Collapsed;

        public Visibility ConnectButtonVisibility { get { return _connectButtonVisibility; } set { _connectButtonVisibility = value; RaisePropertyChanged(); } }
        public Visibility PinCodeTextBoxVisibility { get { return _pincodeTextboxVisibility; } set { _pincodeTextboxVisibility = value; RaisePropertyChanged(); } }
        public Visibility ValidateButtonVisibility { get { return _validateButtonVisibility; } set { _validateButtonVisibility = value; RaisePropertyChanged(); } }
        public Visibility TweetTextTextboxVisibility { get { return _tweetTextTextboxVisibility; } set { _tweetTextTextboxVisibility = value; RaisePropertyChanged(); } }
        public Visibility TweetButtonVisibility { get { return _tweetButtonVisibility; } set { _tweetButtonVisibility = value; RaisePropertyChanged(); } }
        public Visibility GotoProfilButtonVisibility { get { return _gotoProfilButtonVisibility; } set { _gotoProfilButtonVisibility = value; RaisePropertyChanged(); } }
        

        private RelayCommand _publishTweetCommand;
        private RelayCommand _validatePinCodeCommand;
        private RelayCommand _connectCommand;
        private RelayCommand _gotoProfilPageCommand;

        public RelayCommand PublishTweetCommand
        {
            get
            {
                if (_publishTweetCommand == null)
                    _publishTweetCommand = new RelayCommand(PublishTweet);

                return _publishTweetCommand;
            }
            set { _publishTweetCommand = value; RaisePropertyChanged(); }
        }
        public RelayCommand ValidatePinCodeCommand
        {
            get
            {
                if (_validatePinCodeCommand == null)
                    _validatePinCodeCommand = new RelayCommand(ValidatePinCode);

                return _validatePinCodeCommand;
            }
            set { _validatePinCodeCommand = value; RaisePropertyChanged(); }
        }
        public RelayCommand ConnectCommand
        {
            get
            {
                if (_connectCommand == null)
                    _connectCommand = new RelayCommand(Connect);

                return _connectCommand;
            }
            set { _connectCommand = value; RaisePropertyChanged(); }
        }
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

        private void Connect()
        {
            AppCredentials = new TwitterCredentials(TweetinviData.ConsumerKey, TweetinviData.ConsumerSecret);
            var url = CredentialsCreator.GetAuthorizationURL(AppCredentials);
            Uri targeturi = new Uri(url);

            ConnectButtonVisibility = Visibility.Collapsed;
            PinCodeTextBoxVisibility = Visibility.Visible;
            ValidateButtonVisibility = Visibility.Visible;

            NavigationService.Navigate(typeof(Views.WebViewPage), targeturi);
        }

        private void ValidatePinCode()
        {
            var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(PinCode, AppCredentials);
            Auth.SetCredentials(userCredentials);

            var loggedUser = User.GetAuthenticatedUser();

            if (!String.IsNullOrEmpty(loggedUser.Name))
            {
                PinCodeTextBoxVisibility = Visibility.Collapsed;
                ValidateButtonVisibility = Visibility.Collapsed;

                TweetTextTextboxVisibility = Visibility.Visible;
                TweetButtonVisibility = Visibility.Visible;

                GotoProfilButtonVisibility = Visibility.Visible;

                TweetinviData.AccessToken = loggedUser.Credentials.AccessToken;
                TweetinviData.AccessTokenSecret = loggedUser.Credentials.AccessTokenSecret;
            }
        }

        private void PublishTweet()
        {
            Tweet.PublishTweet(TweetText);
        }
    }
}

