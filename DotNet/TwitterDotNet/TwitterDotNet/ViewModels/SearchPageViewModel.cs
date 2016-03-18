using Template10.Mvvm;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Tweetinvi.Logic;
using GalaSoft.MvvmLight.Command;
using System;
using System.Linq;

namespace TwitterDotNet.ViewModels
{
    public class SearchPageViewModel : ViewModelBase
    {
        public SearchPageViewModel()
        {
            GotoHomeTimelinePageCommand = new RelayCommand(GotoHomeTimeline);
            GotoNotificationsCommand = new RelayCommand(GotoNotifications);
            GotoMessagesCommand = new RelayCommand(GotoMessages);
            GotoFindPeopleCommand = new RelayCommand(GotoFindPeople);
            GotoSearchCommand = new RelayCommand(GotoSearch);

            GotoProfilPageCommand = new RelayCommand(GotoProfilPage);

            RetweetCommand = new RelayCommand<object>(param => Retweet((string)param));
            LikeCommand = new RelayCommand<object>(param => Like((string)param));
            ReplyCommand = new RelayCommand<object>(param => Reply((string)param));
            GotoUserProfilViaIdCommand = new RelayCommand<object>(param => NavigationService.Navigate(typeof(Views.UserProfilPage), param));

            SearchCommand = new RelayCommand(SearchResults);
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { _searchText = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<Tweet> _tweets = new ObservableCollection<Tweet>();
        public ObservableCollection<Tweet> Tweets { get { return _tweets; } set { _tweets = value; RaisePropertyChanged(); } }

        private ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> Users { get { return _users; } set { _users = value; RaisePropertyChanged(); } }

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get { return _searchCommand; }
            set { _searchCommand = value; }
        }

        private void SearchResults()
        {
            if (Tweets.Count != 0)
                Tweets.Clear();

            if (Users.Count != 0)
                Users.Clear();

            SearchTweetsAsync();
            SearchUsersAsync();
        }

        private async void SearchTweetsAsync()
        {
            var tweetsFound = Tweetinvi.Search.SearchTweets(SearchText);
            foreach (var tweet in tweetsFound)
            {
                var curTweet = tweet as Tweet;
                Tweets.Add(curTweet);
            }

            await Task.CompletedTask;
        }

        private async void SearchUsersAsync()
        {
            var usersFound = Tweetinvi.Search.SearchUsers(SearchText);
            foreach (var user in usersFound)
            {
                var curUser = user as User;
                Users.Add(curUser);
            }
            
            await Task.CompletedTask;
        }

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
        private void GotoHomeTimeline() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoNotifications() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoMessages() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoFindPeople() => NavigationService.Navigate(typeof(Views.HomeTimelinePage));
        private void GotoSearch() => NavigationService.Navigate(typeof(Views.SearchPage));

            // Secondary
        private void GotoProfilPage() => NavigationService.Navigate(typeof(Views.UserProfilPage));
        public void GotoSettings() => NavigationService.Navigate(typeof(Views.SettingsPage), 0);
        public void GotoPrivacy() => NavigationService.Navigate(typeof(Views.SettingsPage), 1);
        public void GotoAbout() => NavigationService.Navigate(typeof(Views.SettingsPage), 2);
    }
}
