using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using System;
using Windows.UI.Xaml;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Parameters;
using System.Collections.ObjectModel;
using Tweetinvi.Logic;

namespace TwitterDotNet.ViewModels
{
    public class UserProfilPageViewModel : ViewModelBase
    {
        public UserProfilPageViewModel()
        {
            Views.Busy.SetBusy(true, "Chargement du profil, veuillez patienter ...");

            GotoHomeTimelinePageCommand = new RelayCommand(GotoHomeTimeline);
            GotoNotificationsCommand = new RelayCommand(GotoNotifications);
            GotoMessagesCommand = new RelayCommand(GotoMessages);
            GotoFindPeopleCommand = new RelayCommand(GotoFindPeople);
            GotoSearchCommand = new RelayCommand(GotoSearch);

            GotoProfilPageCommand = new RelayCommand(GotoProfilPage);

            RetweetCommand = new RelayCommand<object>(param => Retweet((string)param));
            LikeCommand = new RelayCommand<object>(param => Like((string)param));
            ReplyCommand = new RelayCommand<object>(param => Reply((string)param));
            DeleteTweetCommand = new RelayCommand<object>(param => Delete((string)param));

            GotoUserProfilViaIdCommand = new RelayCommand<object>(param => NavigationService.Navigate(typeof(Views.UserProfilPage), param));
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (parameter != null)
                UserToLoad = Tweetinvi.User.GetUserFromId(Convert.ToInt64(parameter));
            else
                UserToLoad = Tweetinvi.User.GetAuthenticatedUser();

            if (UserToLoad != null)
            {
                if (UserToLoad.Verified)
                    VerifiedIconVisibility = Visibility.Visible;
            }

            if (Tweets.Count == 0)
                FirstTimelineLoading();
            else
                TimelineReloading();

            Views.Busy.SetBusy(false);
            await Task.CompletedTask;
        }

        private async void FirstTimelineLoading()
        {
            var newTweets = Tweetinvi.Timeline.GetUserTimeline(UserToLoad.Id);

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
            var newTweets = Tweetinvi.Timeline.GetUserTimeline(UserToLoad.Id);
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
            var newTweets = Tweetinvi.Timeline.GetUserTimeline(UserToLoad.Id, new UserTimelineParameters {
                MaxId = Tweets.Last().Id,
                MaximumNumberOfTweetsToRetrieve = 20
            });
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

        private ObservableCollection<Tweet> _Tweets = new ObservableCollection<Tweet>();
        public ObservableCollection<Tweet> Tweets { get { return _Tweets; } set { _Tweets = value; } }

        private IUser _userToLoad;
        public IUser UserToLoad { get { return _userToLoad; } set { _userToLoad = value; RaisePropertyChanged(); } }

        private Visibility _verifiedIconVisibility = Visibility.Collapsed;
        public Visibility VerifiedIconVisibility { get { return _verifiedIconVisibility; } set { _verifiedIconVisibility = value; RaisePropertyChanged(); } }

        // Tweets Commands
        private RelayCommand<object> _retweetCommand;
        public RelayCommand<object> RetweetCommand { get { return _retweetCommand; } set { _retweetCommand = value; } }
        private RelayCommand<object> _likeCommand;
        public RelayCommand<object> LikeCommand { get { return _likeCommand; } set { _likeCommand = value; } }
        private RelayCommand<object> _replyCommand;
        public RelayCommand<object> ReplyCommand { get { return _replyCommand; } set { _replyCommand = value; } }
        private RelayCommand<object> _gotoUserProfilViaIdCommand;
        public RelayCommand<object> GotoUserProfilViaIdCommand { get { return _gotoUserProfilViaIdCommand; } set { _gotoUserProfilViaIdCommand = value; } }
        private RelayCommand<object> _deleteTweetCommand;
        public RelayCommand<object> DeleteTweetCommand { get { return _deleteTweetCommand; } set { _deleteTweetCommand = value; } }

        private void Delete(object tweetIdStr)
        {
            var tweetId = Convert.ToInt64(tweetIdStr);
            var theTweet = Tweets.Single(i => i.Id == tweetId);

            if (theTweet.CreatedBy.Id == Tweetinvi.User.GetAuthenticatedUser().Id)
            {
                if (theTweet.Retweeted) Tweetinvi.Tweet.UnRetweet(tweetId);
                else Tweetinvi.Tweet.DestroyTweet(tweetId);

                Tweets.Remove(theTweet);
            }
        }
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
