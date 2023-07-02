using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeartRateMonitorApp.Models
{
    public class AuthorizationResults
    {
        public string AccessToken { get; set; }

        public string IdentityToken { get; set; }

        public bool IsAuthorized { get; set; }

        public string RefreshToken { get; set; }

        public string Scope { get; set; }
    }
}