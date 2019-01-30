using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RallyUp.Droid
{
    [BroadcastReceiver(Label = "SMS Listener")]
    [IntentFilter(new string[] { "android.provider.Telephony.SMS_RECEIVED" })]
    public class SMSListener : BroadcastReceiver
    {
        public static readonly string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

        public override void OnReceive(Context context, Intent intent)
        {
            if(intent.Action == IntentAction)
            {
                Bundle bundle = intent.Extras;
                Android.Telephony.SmsMessage[] messages;
                Toast.MakeText(context, "Message received", ToastLength.Short);
            }
        }
    }
}