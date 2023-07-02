using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Bluetooth;
using System;
using HeartRateMonitorApp.Bluetooth;
//using Android.Bluetooth.LE;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Exceptions;

namespace HeartRateMonitorApp
{
    [Activity(Label = "BluetoothActivity")]
    public class BluetoothActivity : Activity
    {
        //https://github.com/dotnet-bluetooth-le/dotnet-bluetooth-le
        private Button _mainButton;
        //private BluetoothLeScanner bluetoothLeScanner = bluetoothAdapter.getBluetoothLeScanner();
        //private bool scanning;
        //private Handler handler = new Handler();
        // Stops scanning after 10 seconds.
        private static int SCAN_PERIOD = 10000;

        IBluetoothLE _ble;
        Plugin.BLE.Abstractions.Contracts.IAdapter _adapter;
        ObservableCollection<IDevice> _deviceList;
        IDevice _device;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_bluetooth);

            FindViews();
            LinkEventHandlers();
            /*
                        //BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter; 
                        //BluetoothManager bluetoothManager = (BluetoothManager)Context.getSystemService(Context.BLUETOOTH_SERVICE);
                        BluetoothManager bluetoothManager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
                        BluetoothAdapter bluetoothAdapter = bluetoothManager.Adapter
                        if (adapter == null)
                            throw new Exception("No Bluetooth adapter found.");

                        if (!adapter.IsEnabled)
                            throw new Exception("Bluetooth adapter is not enabled.");
            */
            _ble = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;
            _adapter.ScanTimeout = SCAN_PERIOD;

            var state = _ble.State;

            _ble.StateChanged += (s, e) =>
            {
                //Debug.WriteLine($"The bluetooth state changed to {e.NewState}");
                //DisplayAlert("Alert", $"The bluetooth state changed to {e.NewState}", "OK");
                bluetoothStateAlert(s, e);
            };

            _deviceList = new ObservableCollection<IDevice>();
            //bltlist.ItemsSource = deviceList;

            //scan();
            //connectToDevice();


        }

        private void bluetoothStateAlert(object sender, EventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            AlertDialog alert = dialog.Create();
            alert.SetTitle("The bluetooth state changed");
            alert.SetMessage($"Info message with current state");
            alert.SetButton("OK", (c, ev) =>
            {
                // Ok button click task  
            });
            alert.Show();
        }

        public async void connectToDevice()
        {
            _device = _deviceList[0];
            try
            {
                await _adapter.ConnectToDeviceAsync(_device);
            }
            catch (DeviceConnectionException ex)
            {
                // ... could not connect to device
                throw new Exception(message: $"Could not connect to device: {ex.Message.ToString()}");
            }
        }
        public async void scan()
        {

            //TODO: Check if Bluetooth is on or not, see Enable code below.
            //TODO: Test with HR band close by
            try
            {
                _deviceList.Clear();
                _adapter.DeviceDiscovered += (s, a) =>
                {

                    _deviceList.Add(a.Device);
                    // DisplayAlert("Disc", a.Device.Id.ToString(), "OK");
                };

                //We have to test if the device is scanning 
                if (!_ble.Adapter.IsScanning)
                {
                    await _adapter.StartScanningForDevicesAsync();
                }
            }
            catch (Exception ex)
            {
                //DisplayAlert("Notice", ex.Message.ToString(), "Error !");
                throw new Exception(message: ex.Message.ToString());
            }
        }
        /*
                private void scanLeDevice()
                {
                    if (!scanning)
                    {
                        // Stops scanning after a predefined scan period.
                        handler.postDelayed(new Runnable() {
                        @Override
                        public void run()
                            {
                                scanning = false;
                                bluetoothLeScanner.stopScan(leScanCallback);
                            }
                        }, SCAN_PERIOD);

                    scanning = true;
                    bluetoothLeScanner.startScan(leScanCallback);
                    } else {
                    scanning = false;
                    bluetoothLeScanner.stopScan(leScanCallback);
                    }
                }   
        */
        /*
                protected override void OnResume()
                {
                    base.OnResume();
                    RegisterReceiver(BluetoothDeviceReceiver.Receiver, new IntentFilter(BluetoothDevice.ActionFound));
                }

                protected override void OnPause()
                {
                    base.OnPause();
                    UnregisterReceiver(BluetoothDeviceReceiver.Receiver);
                }
        */
        /*
                void Enable(object sender, EventArgs e)
                {
                    BluetoothManager _manager;
                    _manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
                    bool on = _manager.Adapter.IsEnabled;
                    if (on == true)
                    {
                        string error = "Bluetooth is Already on";
                        Toast.MakeText(this, error, ToastLength.Long).Show();
                        e.Equals(true);
                    }
                    else
                    {
                        string okay = "Bluetooth has been turned on";
                        _manager.Adapter.Enable();
                        Toast.MakeText(this, okay, ToastLength.Long).Show();

                    }
                }
                void Disable(object sender, EventArgs e)
                {
                    BluetoothManager _manager;
                    _manager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.BluetoothService);
                    bool on = _manager.Adapter.IsEnabled;
                    if (on == false)
                    {
                        string error = "Bluetooth is Already off";
                        Toast.MakeText(this, error, ToastLength.Long).Show();
                        e.Equals(false);

                    }
                    else
                    {
                        string okay = "Bluetooth has been turned off";
                        _manager.Adapter.Disable();
                        Toast.MakeText(this, okay, ToastLength.Long).Show();

                    }
                }
        */
        private void LinkEventHandlers()
        {
            _mainButton.Click += _mainButton_Click;
        }

        private void _mainButton_Click(object sender, EventArgs e)
        {
            //Intent intent = new Intent(this, typeof(MainActivity));
            //StartActivity(intent);
            scan();
            connectToDevice();
            //BluetoothAdapter.DefaultAdapter.StartDiscovery();

        }

        private void FindViews()
        {
            _mainButton = FindViewById<Button>(Resource.Id.main_button);
        }
    }
}