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
using System.Diagnostics;

namespace TwitterDotNet.ViewModels
{
    public class HomeTimelinePageViewModel : ViewModelBase
    {
        public HomeTimelinePageViewModel()
        {
            Views.Busy.SetBusy(true, "Chargement de la timeline, veuillez patienter ...");

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
            if (Tweets.Count == 0)
                FirstTimelineLoading();
            else
                TimelineReloading();
            
            await Task.CompletedTask;

            Views.Busy.SetBusy(false);
        }

        private async void FirstTimelineLoading()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 50;
            var newTweets = Tweetinvi.Timeline.GetHomeTimeline(HomeTlParameters);

            if (newTweets != null)
            {
                foreach (var tweet in newTweets)
                {
                    var curTweet = tweet as Tweet;
                    Tweets.Add(curTweet);
                }

                await Task.CompletedTask;
            }
            else
                FirstTimelineLoading();
        }

        private async void TimelineReloading()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 50;
            HomeTlParameters.SinceId = Tweets.ElementAt(0).Id;

            var newTweets = Tweetinvi.Timeline.GetHomeTimeline(HomeTlParameters);
            if (newTweets != null)
            {
                foreach (var tweet in newTweets)
                {
                    var curTweet = tweet as Tweet;
                    Tweets.Insert(0, curTweet);
                }

                await Task.CompletedTask;
            }
            else
                TimelineReloading();
        }

        public async void LoadMoreTweets()
        {
            HomeTlParameters.MaximumNumberOfTweetsToRetrieve = 20;
            HomeTlParameters.MaxId = Tweets.ElementAt(Tweets.IndexOf(Tweets.Last())).Id;

            var newTweets = Tweetinvi.Timeline.GetHomeTimeline(HomeTlParameters);
            if (newTweets != null)
            {
                foreach (var tweet in newTweets)
                {
                    var curTweet = tweet as Tweet;

                    if (curTweet.Id != Tweets.Last().Id)
                        Tweets.Insert(Tweets.IndexOf(Tweets.Last()) + 1, curTweet);
                }

                await Task.CompletedTask;
            }
            else
                LoadMoreTweets();
        }

        private HomeTimelineParameters _homeTlParameters = new HomeTimelineParameters();
        public HomeTimelineParameters HomeTlParameters { get { return _homeTlParameters; } set { _homeTlParameters = value; } }
        
        private static ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        public static ObservableCollection<Tweet> Tweets { get { return _tweets; } set { _tweets = value; } }

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

            var tweetLocal = Tweets.Single(i => i.Id == tweetId);
            var tweetBeforRT = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;
            
            if (tweetBeforRT.Retweeted) Tweetinvi.Tweet.UnRetweet(tweetId);
            else Tweetinvi.Tweet.PublishRetweet((long)tweetId);

            var tweetAfterRT = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;

            Tweets.Insert(Tweets.IndexOf(tweetLocal), tweetAfterRT);
            Tweets.Remove(tweetLocal);
        }
        private void Like(object tweetIdStr)
        {
            var tweetId = Convert.ToInt64(tweetIdStr);

            var tweetLocal = Tweets.Single(i => i.Id == tweetId);

            var tweetBeforLike = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;

            if (tweetBeforLike.Favorited) Tweetinvi.Tweet.UnFavoriteTweet(tweetId);
            else Tweetinvi.Tweet.FavoriteTweet((long)tweetId);

            var tweetAfterLike = Tweetinvi.Tweet.GetTweet(tweetId) as Tweet;

            Tweets.Insert(Tweets.IndexOf(tweetLocal), tweetAfterLike);
            Tweets.Remove(tweetLocal);
        }
        private void Reply(object tweetIdStr)
        {
            var tweetId = Convert.ToInt64(tweetIdStr);
            NavigationService.Navigate(typeof(Views.TweetingPage), Tweets.IndexOf(Tweets.Single(i => i.Id == tweetId)));
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
        private void GotoSearch() => NavigationService.Navigate(typeof(Views.SearchPage));

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
