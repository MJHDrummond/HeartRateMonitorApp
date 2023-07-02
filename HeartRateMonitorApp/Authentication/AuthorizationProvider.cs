using Android.Net;
using HeartRateMonitorApp.Authentication;

namespace HeartRateMonitorApp.Authentication
{
    public class AuthorizationProvider
    {

        //private static readonly Uri ConfigurationEndpoint = Uri.Parse($"{Configuration.OrgUrl}/oauth2/default/.well-known/openid-configuration");

        private static readonly string[] Scopes = new[] { "openid", "profile", "email", "offline_access" };
    }
}