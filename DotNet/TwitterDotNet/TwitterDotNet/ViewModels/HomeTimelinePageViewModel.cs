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
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Interfaces;

namespace TwitterDotNet.ViewModels
{
    public class HomeTimelinePageViewModel : ViewModelBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (Tweets == null)
                FirstTimelineLoading();
            else
                TimelineReloading();

            await Task.CompletedTask;
        }

        private void FirstTimelineLoading()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 10;
            Tweets = Timeline.GetHomeTimeline(HomeTlParameters);
        }

        private void TimelineReloading()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 10;
            HomeTlParameters.SinceId = Tweets.ElementAt(0).Id;
            
            var newTweets = Timeline.GetHomeTimeline(HomeTlParameters);
            newTweets = newTweets.Concat(Tweets);
            Tweets = newTweets;
        }

        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);
        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
        private void GotoProfilPage() => NavigationService.Navigate(typeof(Views.UserProfilPage));
        private void GotoHomeTimeline() => TimelineReloading();

        private HomeTimelineParameters _homeTlParameters = new HomeTimelineParameters();
        public HomeTimelineParameters HomeTlParameters { get { return _homeTlParameters; } set { _homeTlParameters = value; } }
        
        private IEnumerable<ITweet> _tweets;
        public IEnumerable<ITweet> Tweets { get { return _tweets; } set { _tweets = value; RaisePropertyChanged(); } }

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
        private RelayCommand _gotoHomeTimelinePageCommand;
        public RelayCommand GotoHomeTimelinePageCommand
        {
            get
            {
                if (_gotoHomeTimelinePageCommand == null)
                    _gotoHomeTimelinePageCommand = new RelayCommand(GotoHomeTimeline);

                return _gotoHomeTimelinePageCommand;
            }
            set { _gotoHomeTimelinePageCommand = value; RaisePropertyChanged(); }
        }
    }
}
