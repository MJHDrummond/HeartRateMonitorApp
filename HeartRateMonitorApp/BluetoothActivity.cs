using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Bluetooth;
using Android.Runtime;
using System;
using HeartRateMonitorApp.Bluetooth;
using Android.Views;

namespace HeartRateMonitorApp
{
    [Activity(Label = "BluetoothActivity")]
    public class BluetoothActivity : Activity
    {
        //https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le
        
        private BluetoothAdapter _bluetoothAdapter;
        private MyScanCallback _scanCallback;
        private bool _isScanning;
        private Button _mainButton;
        private Button _scanButton;
        private Activity _activityContext = (Activity)Application.Context;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.Main);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_bluetooth);


            // Initialize BluetoothAdapter
            var bluetoothManager = (BluetoothManager)GetSystemService(Context.BluetoothService);
            _bluetoothAdapter = bluetoothManager.Adapter;

            // Create the BluetoothLeScanCallback
            //_scanCallback = new MyBluetoothLeScanCallback();

            // Check if BLE is supported on the device
            if (!_bluetoothAdapter.IsMultipleAdvertisementSupported)
            {
                Console.WriteLine("BLE is not supported on this device");
                //var message = $"BLE is not supported on this device";
                //ShowToastMessage(message);
                return;
            }

            _scanButton = FindViewById<Button>(Resource.Id.scanButton);

            _scanButton.Click += OnScanButtonClick;

            // Check if Bluetooth is enabled
            if (!_bluetoothAdapter.IsEnabled)
            {
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, 1);
            }

            //FindViews();
            // LinkEventHandlers();


        }
        private void OnScanButtonClick(object sender, EventArgs e)
        {
            // Start scanning for BLE devices
            StartScan();
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Start scanning for BLE devices
            StartScan();
        }

        protected override void OnPause()
        {
            base.OnPause();

            // Stop scanning for BLE devices
            StopScan();
        }

        private void StartScan()
        {
            // Check if already scanning
            if (_isScanning)
                return;

            // Check if BluetoothLeScanner is available
            if (_bluetoothAdapter?.BluetoothLeScanner == null)
            {
                Console.WriteLine("BluetoothLeScanner is not available");
                //var message = $"BluetoothLeScanner is not available";
                //ShowToastMessage(message);

                return;
            }

            // Start scanning for BLE devices
            _scanCallback = new MyScanCallback(_activityContext);
            _bluetoothAdapter.BluetoothLeScanner.StartScan(_scanCallback);

            _isScanning = true;
        }

        private void StopScan()
        {
            // Check if scanning
            if (!_isScanning)
                return;

            // Stop scanning for BLE devices
            _bluetoothAdapter.BluetoothLeScanner.StopScan(_scanCallback);

            _isScanning = false;
        }
        /*
        private void ShowToastMessage(string message)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, message, ToastLength.Long).Show();
            });
        }
        */
        private void LinkEventHandlers()
        {
            _mainButton.Click += _mainButton_Click;
        }

        private void _mainButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            //scan();
            //connectToDevice();
            //BluetoothAdapter.DefaultAdapter.StartDiscovery();

        }

        private void FindViews()
        {
            _mainButton = FindViewById<Button>(Resource.Id.main_button);
        }
    }
}