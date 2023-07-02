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
            SetContentView(Resource.Layout.activity_bluetooth);
            Button bluetooth_button = FindViewById<Button>(Resource.Id.bluetooth_button);
            bluetooth_button.Click += delegate {
                StartActivity(typeof(MainActivity));
            };
            // Create your application here 
        }
    }
}