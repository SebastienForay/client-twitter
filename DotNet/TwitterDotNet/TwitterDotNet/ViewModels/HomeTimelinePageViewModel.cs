using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using TwitterDotNet.Services.TweetinviAPI;
using Tweetinvi.Core.Credentials;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using TwitterDotNet.Services.AccountManager;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Logic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace TwitterDotNet.ViewModels
{
    public class HomeTimelinePageViewModel : ViewModelBase
    {
        public HomeTimelinePageViewModel()
        {
            GotoHomeTimelinePageCommand = new RelayCommand(GotoHomeTimeline);
            GotoNotificationsCommand = new RelayCommand(GotoNotifications);
            GotoMessagesCommand = new RelayCommand(GotoMessages);
            GotoFindPeopleCommand = new RelayCommand(GotoFindPeople);
            GotoSearchCommand = new RelayCommand(GotoSearch);

            GotoProfilPageCommand = new RelayCommand(GotoProfilPage);

            GotoTweetingPageCommand = new RelayCommand(GotoTweetingPage);

            RetweetCommand = new RelayCommand<object>(param => Retweet((string)param));
            LikeCommand = new RelayCommand<object>(param => Like((string)param));
            ReplyCommand = new RelayCommand<object>(param => Reply((string)param));
            GotoUserProfilViaIdCommand = new RelayCommand<object>(param => NavigationService.Navigate(typeof(Views.UserProfilPage), param));
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (TweetsCollection.Count == 0)
                FirstTimelineLoading();
            else
                TimelineReloading();

            await Task.CompletedTask;
        }

        private async void FirstTimelineLoading()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 50;
            var newTweets = Tweetinvi.Timeline.GetHomeTimeline(HomeTlParameters);
            foreach (var tweet in newTweets)
            {
                var curTweet = tweet as Tweet;
                TweetsCollection.Add(curTweet);
            }

            await Task.CompletedTask;
        }

        private async void TimelineReloading()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 50;
            HomeTlParameters.SinceId = TweetsCollection.ElementAt(0).Id;

            var newTweets = Tweetinvi.Timeline.GetHomeTimeline(HomeTlParameters);
            foreach (var tweet in newTweets)
            {
                var curTweet = tweet as Tweet;
                TweetsCollection.Insert(0, curTweet);
            }

            await Task.CompletedTask;
        }

        private HomeTimelineParameters _homeTlParameters = new HomeTimelineParameters();
        public HomeTimelineParameters HomeTlParameters { get { return _homeTlParameters; } set { _homeTlParameters = value; } }
        
        private static ObservableCollection<Tweet> _tweetsCollection = new ObservableCollection<Tweet>();
        public static ObservableCollection<Tweet> TweetsCollection { get { return _tweetsCollection; } set { _tweetsCollection = value; } }

        // Tweets Commands
        private RelayCommand<object> _retweetCommand;
        public RelayCommand<object> RetweetCommand { get { return _retweetCommand; } set { _retweetCommand = value; } }
        private RelayCommand<object> _likeCommand;
        public RelayCommand<object> LikeCommand { get { return _likeCommand; } set { _likeCommand = value; } }
        private RelayCommand<object> _replyCommand;
        public RelayCommand<object> ReplyCommand { get { return _replyCommand; } set { _replyCommand = value; } }
        private RelayCommand<object> _gotoUserProfilViaIdCommand;
        public RelayCommand<object> GotoUserProfilViaIdCommand { get { return _gotoUserProfilViaIdCommand; } set { _gotoUserProfilViaIdCommand = value; } }


        private void Retweet(object tweetIdStr)
        {
            var tweetId = Convert.ToInt64(tweetIdStr);

            var tweetLocal = TweetsCollection.Single(i => i.Id == tweetId);
            var tweetBeforRT = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;
            
            if (tweetBeforRT.Retweeted) Tweetinvi.Tweet.UnRetweet(tweetId);
            else Tweetinvi.Tweet.PublishRetweet((long)tweetId);

            var tweetAfterRT = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;

            TweetsCollection.Insert(TweetsCollection.IndexOf(tweetLocal), tweetAfterRT);
            TweetsCollection.Remove(tweetLocal);
        }
        private void Like(object tweetIdStr)
        {
            var tweetId = Convert.ToInt64(tweetIdStr);

            var tweetLocal = TweetsCollection.Single(i => i.Id == tweetId);

            var tweetBeforLike = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;

            if (tweetBeforLike.Favorited) Tweetinvi.Tweet.UnFavoriteTweet(tweetId);
            else Tweetinvi.Tweet.FavoriteTweet((long)tweetId);

            var tweetAfterLike = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;

            TweetsCollection.Insert(TweetsCollection.IndexOf(tweetLocal), tweetAfterLike);
            TweetsCollection.Remove(tweetLocal);
        }
        private void Reply(object tweetIdStr)
        {
            var tweetId = Convert.ToInt64(tweetIdStr);
            NavigationService.Navigate(typeof(Views.TweetingPage), TweetsCollection.IndexOf(TweetsCollection.Single(i => i.Id == tweetId)));
        }

        // TopBar Primary Commands
        private RelayCommand _gotoHomeTimelinePageCommand;
        private RelayCommand _gotoNotificationsCommand;
        private RelayCommand _gotoMessagesCommand;
        private RelayCommand _gotoFindPeopleCommand;
        private RelayCommand _gotoSearchCommand;

        public RelayCommand GotoHomeTimelinePageCommand { get { return _gotoHomeTimelinePageCommand; } set { _gotoHomeTimelinePageCommand = value; } }
        public RelayCommand GotoNotificationsCommand { get { return _gotoNotificationsCommand; } set { _gotoNotificationsCommand = value; } }
        public RelayCommand GotoMessagesCommand { get { return _gotoMessagesCommand; } set { _gotoMessagesCommand = value; } }
        public RelayCommand GotoFindPeopleCommand { get { return _gotoFindPeopleCommand; } set { _gotoFindPeopleCommand = value; } }
        public RelayCommand GotoSearchCommand { get { return _gotoSearchCommand; } set { _gotoSearchCommand = value; } }

        // TopBar Secondary Commands
        private RelayCommand _gotoProfilPageCommand;
        public RelayCommand GotoProfilPageCommand { get { return _gotoProfilPageCommand; } set { _gotoProfilPageCommand = value; } }


        // Commands Functions
            // Primary
        private void GotoHomeTimeline()
        {
            if (NavigationService.CurrentPageType == typeof(Views.HomeTimelinePage))
                TimelineReloading();
            else
                NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        }
        private void GotoNotifications() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoMessages() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoFindPeople() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoSearch() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));

            // Secondary
        private void GotoProfilPage() => NavigationService.Navigate(typeof(Views.UserProfilPage));
        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);
        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);

        // Bottom Bar Commands
        private RelayCommand _gotoTweetingPageCommand;
        public RelayCommand GotoTweetingPageCommand { get { return _gotoTweetingPageCommand; } set { _gotoTweetingPageCommand = value; } }

        // Bottom Bar Functions
        private void GotoTweetingPage() => NavigationService.Navigate(typeof(Views.TweetingPage));
    }
}
