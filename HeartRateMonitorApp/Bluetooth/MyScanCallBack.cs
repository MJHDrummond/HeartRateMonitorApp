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
        private Activity _activityContext;

        public MyScanCallback(Activity activityContext)
        {
            this._activityContext = activityContext;
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);

            // Handle the scanned BLE device
            var device = result.Device;
            var rssi = result.Rssi;

            // Access device information
            var deviceName = device.Name;
            var deviceAddress = device.Address;

            // Do something with the scanned device
            Console.WriteLine($"Found BLE device: {deviceName} ({deviceAddress}), RSSI: {rssi}");

            // Display the scanned device information as a toast
            //var message = $"Found BLE device: {deviceName} ({deviceAddress}), RSSI: {rssi}";
            //ShowToastMessage(message);

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