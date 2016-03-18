using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Tweetinvi.Logic;
using Windows.UI.Xaml.Navigation;

namespace TwitterDotNet.ViewModels
{
    public class TweetingPageViewModel : ViewModelBase
    {
        public TweetingPageViewModel()
        {
            TweetCommand = new RelayCommand(PublishTweet);
            GotoPreviousPageCommand = new RelayCommand(GotoPreviousPage);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                TweetToReply = HomeTimelinePageViewModel.Tweets[(int)parameter];

                TweetText = "@" + TweetToReply.CreatedBy.ScreenName + " ";
            }

            await Task.CompletedTask;
        }

        private Tweet _tweetToReply;
        public Tweet TweetToReply
        {
            get { return _tweetToReply; }
            set { _tweetToReply = value; }
        }


        private string _tweetText = "";
        private string _tweetTextLengthRemaining = "140";
        public string TweetText
        {
            get { return _tweetText; }
            set
            {
                _tweetText = value;
                TweetTextLengthRemaining = (140 - _tweetText.Length).ToString();
            }
        }
        public string TweetTextLengthRemaining { get { return _tweetTextLengthRemaining; } set { _tweetTextLengthRemaining = value; RaisePropertyChanged(); } }


        private RelayCommand _tweetCommand;
        public RelayCommand TweetCommand { get { return _tweetCommand; } set { _tweetCommand = value; } }
        
        private RelayCommand _gotoPreviousPageCommand;
        public RelayCommand GotoPreviousPageCommand { get { return _gotoPreviousPageCommand; } set { _gotoPreviousPageCommand = value; RaisePropertyChanged(); } }
        private void GotoPreviousPage() => NavigationService.GoBack();

        private void PublishTweet()
        {
            if (TweetToReply != null)
            {
                Tweetinvi.Tweet.PublishTweetInReplyTo(TweetText, TweetToReply.Id);
                NavigationService.Navigate((typeof(Views.HomeTimelinePage)));
            }
            else
            {
                Tweetinvi.Tweet.PublishTweet(TweetText);
                NavigationService.Navigate((typeof(Views.HomeTimelinePage)));
            }
        }
    }
}
