using Android.Content;
using OpenId.AppAuth;
using Org.Json;

namespace HeartRateMonitorApp.Extensions
{
    public static class IntentExtensions
    {
        public const string AuthStateKey = "authState";

        public static string GetAuthStateExtra(this Intent intent)
        {
            return intent.GetStringExtra(AuthStateKey);
        }

        public static bool HasAuthStateExtra(this Intent intent)
        {
            return intent.HasExtra(AuthStateKey);
        }

        public static void PutAuthStateExtra(this Intent intent, AuthState authState)
        {
            intent.PutExtra(AuthStateKey, authState.JsonSerializeString());
        }

        public static bool TryGetAuthStateFromExtra(this Intent intent, out AuthState authState)
        {
            authState = null;

            if (!intent.HasAuthStateExtra())
            {
                return false;
            }

            try
            {
                authState = AuthState.JsonDeserialize(intent.GetAuthStateExtra());
            }
            catch (JSONException)
            {
                return false;
            }

            return authState != null;
        }
    }
}