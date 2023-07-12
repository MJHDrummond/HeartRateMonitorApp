using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Bluetooth;
using Android.Runtime;
using System;
using HeartRateMonitorApp.Bluetooth;
using Android.Views;
using System.Collections.Generic;
using Android;
using Android.Content.PM;
using System.Threading;

namespace HeartRateMonitorApp
{
    [Activity(Label = "BluetoothActivity")]
    public class BluetoothActivity : Activity
    {
        //https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le
        
        private BluetoothAdapter _bluetoothAdapter;
        private MyScanCallback _scanCallback;
        private bool _isScanning;
        //private Button _mainButton;
        private Button _scanButton;
        //private Activity _activityContext = (Activity)Application.Context;
        private ListView _deviceListView;
        public ArrayAdapter<string> _deviceListAdapter;
        public List<string> _devices;
        private const int RequestLocationPermissionCode = 1;
        //private BluetoothStateReceiver _bluetoothStateReceiver;
        private Timer _scanTimer;



        //TODO: Make time customizable
        //TODO: Add countdown timer to how long scanner is left.
        //TODO: Get list of devices found working and shown on activity

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Console.WriteLine("OnCreate method called"); // Debug log
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

            _scanButton = FindViewById<Button>(Resource.Id.scanButton);
            _scanButton.Click += (sender, e) => OnScanButtonClick(sender, e);

            _deviceListView = FindViewById<ListView>(Resource.Id.deviceListView);
            _devices = new List<string>();
            _deviceListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _devices);
            _deviceListView.Adapter = _deviceListAdapter;
            //_scanButton.Touch += OnScanButtonTouch;

            /*
            // Attach touch event handler to the scan button
            _scanButton.Click += (sender, e) => OnScanButtonClick(sender, e);
            _scanButton.Touch += (sender, e) =>
            {
                if (e.Event.Action == MotionEventActions.Up)
                {
                    OnScanButtonClick(sender, e);
                }
            };
            */

            // Check if Bluetooth is enabled
            if (!_bluetoothAdapter.IsEnabled)
            {
                var message = $"Bluetooth is not enabled";
                ShowToastMessage(message);
                Intent enableBtIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(enableBtIntent, 1);
            }

            if (!_bluetoothAdapter.IsMultipleAdvertisementSupported)
            {
                //Console.WriteLine("BLE is not supported on this device");
                var message = $"BLE is not supported on this device";
                ShowToastMessage(message);
                return;
            }



            //FindViews();
            //LinkEventHandlers();
            //FindViews();
            // LinkEventHandlers();


        }
        private void OnScanButtonClick(object sender, EventArgs e)
        {
            var message = $"Button clicked";
            ShowToastMessage(message);

            RequestLocationPermission();

            // Clear the existing devices list
            _devices.Clear();
            _deviceListAdapter.NotifyDataSetChanged();

            // Start scanning for BLE devices
            StartScan();
        }

        private void OnScanButtonTouch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up)
            {
                Toast.MakeText(this, "Scan button touched", ToastLength.Short).Show(); // Display a toast message

                var message = $"Button clicked";
                ShowToastMessage(message);
                // Start scanning for BLE devices   
                StartScan();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Start scanning for BLE devices
            StartScan();

            // Register the Bluetooth state receiver
            //_bluetoothStateReceiver = new BluetoothStateReceiver();
            //RegisterReceiver(_bluetoothStateReceiver, new IntentFilter(BluetoothAdapter.ActionStateChanged));

        }

        protected override void OnPause()
        {
            base.OnPause();

            // Stop scanning for BLE devices
            StopScanNow();

            // Unregister the Bluetooth state receiver
            //UnregisterReceiver(_bluetoothStateReceiver);
        }

        private void StartScan()
        {
            // Check if already scanning
            if (_isScanning)
                return;

            // Check if BluetoothLeScanner is available
            if (_bluetoothAdapter?.BluetoothLeScanner == null)
            {
                //Console.WriteLine("BluetoothLeScanner is not available");
                var message = $"BluetoothLeScanner is not available";
                ShowToastMessage(message);
                return;
            }

            // Start scanning for BLE devices
            //_scanCallback = new MyScanCallback(_activityContext);
            _scanCallback = new MyScanCallback(this);
            _bluetoothAdapter.BluetoothLeScanner.StartScan(_scanCallback);

            _isScanning = true;
            // Start the timer to stop scanning after 10 seconds
            _scanTimer = new Timer(StopScan, null, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
        }

        private void StopScanNow()
        {
            // Check if scanning
            if (!_isScanning)
                return;

            // Stop scanning for BLE devices
            _bluetoothAdapter.BluetoothLeScanner.StopScan(_scanCallback);

            _isScanning = false;

            // Dispose of the timer
            _scanTimer?.Dispose();
        }
        private void StopScan(object state)
        {
            // Check if scanning
            if (!_isScanning)
                return;

            // Stop scanning for BLE devices
            _bluetoothAdapter.BluetoothLeScanner.StopScan(_scanCallback);

            _isScanning = false;
        }

        private void RequestLocationPermission()
        {
            if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
            {
                // Location permission has not been granted, request it
                RequestPermissions(new string[] { Manifest.Permission.AccessFineLocation }, RequestLocationPermissionCode);
            }
            else
            {
                // Location permission has already been granted, start scanning
                StartScan();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == RequestLocationPermissionCode)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Location permission granted, start scanning
                    StartScan();
                }
                else
                {
                    // Location permission denied, handle accordingly (e.g., show an error message)
                    var message = "Location permission denied";
                    ShowToastMessage(message);
                }
            }
        }


        private void ShowToastMessage(string message)
        {
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, message, ToastLength.Long).Show();
            });
        }
        
        private void LinkEventHandlers()
        {
            _scanButton.Click += _scanButton_Click;
        }

        private void _scanButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Button clicked"); // Debug log

            // Start scanning for BLE devices
            StartScan();
            //Intent intent = new Intent(this, typeof(MainActivity));
            //StartActivity(intent);
            //scan();
            //connectToDevice();
            //BluetoothAdapter.DefaultAdapter.StartDiscovery();

        }

        private void FindViews()
        {
            _scanButton = FindViewById<Button>(Resource.Id.scanButton);
        }


        /*
    public class BluetoothStateReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string action = intent.Action;

            if (action == BluetoothAdapter.ActionStateChanged)
            {
                int state = intent.GetIntExtra(BluetoothAdapter.ExtraState, BluetoothAdapter.Error);

                switch (state)
                {
                    case BluetoothAdapter.StateOn:
                        // Bluetooth is enabled, start scanning or perform any other necessary actions
                        StartScan();
                        break;
                    case BluetoothAdapter.StateOff:
                        // Bluetooth is disabled, handle accordingly (e.g., show a message)
                        var message = "Bluetooth is disabled";
                        ShowToastMessage(message);
                        break;
                }
            }
        }
        */
    }
}
