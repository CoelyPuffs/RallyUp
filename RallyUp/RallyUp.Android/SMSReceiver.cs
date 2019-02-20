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
using Android.Telephony;

using Xamarin.Forms;

namespace RallyUp.Droid
{
    [BroadcastReceiver (Enabled = true, Exported = true)]
    public class SMSReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var messages = Android.Provider.Telephony.Sms.Intents.GetMessagesFromIntent(intent);
            foreach (var message in messages)
            {
                //MessagingCenter.Send<SMSReceiver>(this, message.MessageBody);
                Toast.MakeText(context, message.MessageBody, ToastLength.Short).Show();
            }
        }
    }
}