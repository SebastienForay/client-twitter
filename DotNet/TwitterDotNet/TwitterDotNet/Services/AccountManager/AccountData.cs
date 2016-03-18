using System;
using Tweetinvi;
using Tweetinvi.Core.Interfaces;

namespace TwitterDotNet.Services.AccountManager
{
    public class AccountData
    {
        public AccountData()
        {
            if (Auth.ApplicationCredentials != null)
            {
                _iLoggedUser = User.GetAuthenticatedUser();
            }
        }

        private IAuthenticatedUser _iLoggedUser;

        private string _accountUsername;
        private string _accountAccessToken;
        private string _accountAccessTokenSecret;

        public string AccountUsername
        {
            get
            {
                if (String.IsNullOrEmpty(_accountUsername))
                    _accountUsername = _iLoggedUser.ScreenName;

                return _accountUsername;
            }
            set { _accountUsername = value; }
        }

        public string AccountAccessToken
        {
            get
            {
                if (String.IsNullOrEmpty(_accountAccessToken))
                    _accountAccessToken = _iLoggedUser.Credentials.AccessToken;

                return _accountAccessToken;
            }
            set { _accountAccessToken = value; }
        }

        public string AccountAccessTokenSecret
        {
            get
            {
                if (String.IsNullOrEmpty(_accountAccessTokenSecret))
                    _accountAccessTokenSecret = _iLoggedUser.Credentials.AccessTokenSecret;

                return _accountAccessTokenSecret;
            }
            set { _accountAccessTokenSecret = value; }
        }

    }
}
