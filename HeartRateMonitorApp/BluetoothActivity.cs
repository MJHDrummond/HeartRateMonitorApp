using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace HeartRateMonitorApp
{
    [Activity(Label = "BluetoothActivity")]
    public class BluetoothActivity : Activity
    {
        private Button _mainButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_bluetooth);

            FindViews();
            LinkEventHandlers();
        }

        private void LinkEventHandlers()
        {
            _mainButton.Click += _mainButton_Click;
        }

        private void _mainButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        private void FindViews()
        {
            _mainButton = FindViewById<Button>(Resource.Id.main_button);
        }
    }
}