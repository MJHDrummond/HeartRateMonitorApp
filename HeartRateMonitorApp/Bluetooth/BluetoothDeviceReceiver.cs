using Android.Bluetooth;
using Android.Content;

namespace HeartRateMonitorApp.Bluetooth
{
    public class BluetoothDeviceReceiver : BroadcastReceiver
    {
        private static BluetoothDeviceReceiver _instance;
        public static BluetoothDeviceReceiver Receiver => _instance ?? (_instance = new BluetoothDeviceReceiver());

        public override void OnReceive(Context context, Intent intent)
        {
            var action = intent.Action;

            if (action != BluetoothDevice.ActionFound)
            {
                return;
            }

            // Get the device
            var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);
        }
    }
}