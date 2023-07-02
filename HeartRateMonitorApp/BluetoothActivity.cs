using Android.App;
using Android.OS;
using Android.Widget;

namespace HeartRateMonitorApp
{
    [Activity(Label = "BluetoothActivity")]
    public class BluetoothActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_bluetooth);
            Button main_button = FindViewById<Button>(Resource.Id.main_button);
            main_button.Click += delegate {
                StartActivity(typeof(MainActivity));
            };
            // Create your application here 
        }
    }
}