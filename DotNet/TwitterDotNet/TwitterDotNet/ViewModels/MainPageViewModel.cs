using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using TwitterDotNet.Services.TweetinviAPI;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
using System;

namespace TwitterDotNet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            GotoHomeTimelinePageCommand = new RelayCommand(GotoHomeTimeline);
            GotoNotificationsCommand = new RelayCommand(GotoNotifications);
            GotoMessagesCommand = new RelayCommand(GotoMessages);
            GotoFindPeopleCommand = new RelayCommand(GotoFindPeople);
            GotoSearchCommand = new RelayCommand(GotoSearch);

            GotoProfilPageCommand = new RelayCommand(GotoProfilPage);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            NavigationService.ClearHistory();

            AppCredentials = new TwitterCredentials(TweetinviData.ConsumerKey, TweetinviData.ConsumerSecret);
            var url = CredentialsCreator.GetAuthorizationURL(AppCredentials);

            if (String.IsNullOrEmpty(url))
                NavigationService.Navigate(typeof(Views.MainPage));

            Uri targeturi = new Uri(url);

            NavigationService.Navigate(typeof(Views.LoginPage), targeturi);

            await Task.CompletedTask;
        }

        private static TweetinviData _tweetinviData = new TweetinviData();
        private static TwitterCredentials _appCredentials;
        public static TweetinviData TweetinviData { get { return _tweetinviData; } set { _tweetinviData = value; } }
        public static TwitterCredentials AppCredentials { get { return _appCredentials; } set { _appCredentials = value; } }

        // TopBar Primary Commands
        private RelayCommand _gotoHomeTimelinePageCommand;
        private RelayCommand _gotoNotificationsCommand;
        private RelayCommand _gotoMessagesCommand;
        private RelayCommand _gotoFindPeopleCommand;
        private RelayCommand _gotoSearchCommand;

        public RelayCommand GotoHomeTimelinePageCommand { get { return _gotoHomeTimelinePageCommand; } set { _gotoHomeTimelinePageCommand = value; RaisePropertyChanged(); } }
        public RelayCommand GotoNotificationsCommand { get { return _gotoNotificationsCommand; } set { _gotoNotificationsCommand = value; RaisePropertyChanged(); } }
        public RelayCommand GotoMessagesCommand { get { return _gotoMessagesCommand; } set { _gotoMessagesCommand = value; RaisePropertyChanged(); } }
        public RelayCommand GotoFindPeopleCommand { get { return _gotoFindPeopleCommand; } set { _gotoFindPeopleCommand = value; RaisePropertyChanged(); } }
        public RelayCommand GotoSearchCommand { get { return _gotoSearchCommand; } set { _gotoSearchCommand = value; RaisePropertyChanged(); } }

        // TopBar Secondary Commands
        private RelayCommand _gotoProfilPageCommand;
        public RelayCommand GotoProfilPageCommand { get { return _gotoProfilPageCommand; } set { _gotoProfilPageCommand = value; RaisePropertyChanged(); } }


        // Commands Functions
            // Primary
        private void GotoHomeTimeline() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoNotifications() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoMessages() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoFindPeople() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoSearch() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));


            // Secondary
        private void GotoProfilPage() => NavigationService.Navigate(typeof(Views.UserProfilPage));
        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);
        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}

