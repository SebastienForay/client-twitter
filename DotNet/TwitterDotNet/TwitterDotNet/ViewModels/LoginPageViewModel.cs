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
    class LoginPageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            WebViewUriSource = new Uri(parameter.ToString());

            await Task.CompletedTask;
        }

        private Uri _webviewUriSource;
        public Uri WebViewUriSource
        {
            get { return _webviewUriSource; }
            set { _webviewUriSource = value; RaisePropertyChanged(); }
        }


        private RelayCommand _validatePinCodeCommand;
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

        private string _pinCode;
        public string PinCode { get { return _pinCode; } set { _pinCode = value; RaisePropertyChanged(); } }

        private async void ValidatePinCode()
        {
            if (!String.IsNullOrEmpty(PinCode))
            {
                var userCredentials = CredentialsCreator.GetCredentialsFromVerifierCode(PinCode, MainPageViewModel.AppCredentials);
                Auth.SetCredentials(userCredentials);

                var loggedUser = User.GetAuthenticatedUser();

                if (!String.IsNullOrEmpty(loggedUser.ScreenName))
                {

                    MainPageViewModel.TweetinviData.AccessToken = loggedUser.Credentials.AccessToken;
                    MainPageViewModel.TweetinviData.AccessTokenSecret = loggedUser.Credentials.AccessTokenSecret;

                    AccountManager ac = new AccountManager();
                    ac.CreateJsonData();
                    await ac.SaveDataToFile();

                    NavigationService.Navigate(typeof(Views.UserProfilPage));
                }
            }
        }
    }
}
