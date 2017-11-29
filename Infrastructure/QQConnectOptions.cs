using Microsoft.AspNetCore.Authentication.OAuth;

namespace Microsoft.AspNetCore.Authentication.QQConnect
{
    public class QQConnectOptions : OAuthOptions
    {
        public QQConnectOptions()
        {
            //AuthenticationScheme = QQConsts.AuthenticationScheme;
            //DisplayName = AuthenticationScheme;
            CallbackPath = "/signin-qq"; // implicit
            AuthorizationEndpoint = QQConnectDefaults.AuthorizationEndpoint;
            TokenEndpoint = QQConnectDefaults.TokenEndpoint;
            UserInformationEndpoint = QQConnectDefaults.UserInformationEndpoint;
            OpenIdEndpoint = QQConnectDefaults.OpenIdEndpoint;
        }

        public string OpenIdEndpoint { get; }

        public string AppId
        {
            get => ClientId;
            set => ClientId = value;
        }

        public string AppKey
        {
            get => ClientSecret;
            set => ClientSecret = value;
        }

        public bool IsMobile { get; set; }
    }
}