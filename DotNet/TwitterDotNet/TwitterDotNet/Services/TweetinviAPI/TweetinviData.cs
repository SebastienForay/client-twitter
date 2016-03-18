namespace TwitterDotNet.Services.TweetinviAPI
{
    public class TweetinviData
    {
        private string _consumerKey = "CONSUMER_KEY_GOES_HERE";
        private string _consumerSecret = "CONSUMER_SECRET_KEY_GOES_HERE";
        private string _accesToken;
        private string _accessTokenSecret;

        public string ConsumerKey { get { return _consumerKey; } set { _consumerKey = value; } }
        public string ConsumerSecret { get { return _consumerSecret; } set { _consumerSecret = value; } }
        public string AccessToken { get { return _accesToken; } set { _accesToken = value; } }
        public string AccessTokenSecret { get { return _accessTokenSecret; } set { _accessTokenSecret = value; } }  
    }
}