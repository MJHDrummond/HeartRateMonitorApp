using Android.App;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeartRateMonitorApp.Bluetooth
{
    public class MyScanCallback : ScanCallback
    {
        private readonly BluetoothActivity _bluetoothActivity;

        public MyScanCallback(BluetoothActivity bluetoothActivity)
        {
            _bluetoothActivity = bluetoothActivity;
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);

            // Get the device name from the scan result
            string deviceName = result.Device?.Name;

            // Add the device name to the list
            if (!string.IsNullOrEmpty(deviceName) && !_bluetoothActivity._devices.Contains(deviceName))
            {
                _bluetoothActivity._devices.Add(deviceName);
                _bluetoothActivity.RunOnUiThread(() => _bluetoothActivity._deviceListAdapter.NotifyDataSetChanged());
            }

            /*
            // Handle the scanned BLE device
            var device = result.Device;
            var rssi = result.Rssi;

            // Access device information
            var deviceName = device.Name;
            var deviceAddress = device.Address;
            */
            // Do something with the scanned device
            Console.WriteLine($"Found BLE device: {deviceName} ({result.Device.Address}), RSSI: {result.Rssi}");


        }
        /*
        private void ShowToastMessage(string message)
        {
            _activityContext?.RunOnUiThread(() =>
            {
                Toast.MakeText(_activityContext, message, ToastLength.Long).Show();
            });
        }
        */

    }
}