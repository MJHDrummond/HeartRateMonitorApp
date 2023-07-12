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

namespace HeartRateMonitorApp.Bluetooth
{
    public class CustomTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        private readonly Action _clickAction;
        private bool _clicked;

        public CustomTouchListener(Action clickAction)
        {
            _clickAction = clickAction;
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _clicked = true;
                    break;
                case MotionEventActions.Up:
                    if (_clicked)
                    {
                        _clickAction?.Invoke();
                        _clicked = false;
                    }
                    break;
                case MotionEventActions.Cancel:
                    _clicked = false;
                    break;
            }

            return false;
        }
    }

}