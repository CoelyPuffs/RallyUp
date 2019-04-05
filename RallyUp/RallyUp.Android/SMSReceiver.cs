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
    [BroadcastReceiver]
    public class SMSReceiver : BroadcastReceiver
    {
        App app { get; set; }

        public SMSReceiver(){

        }

        public SMSReceiver(App app)
        {
            this.app = app;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var messages = Android.Provider.Telephony.Sms.Intents.GetMessagesFromIntent(intent);
            foreach (var message in messages)
            {
                Xamarin.Forms.MessagingCenter.Send<App, string>(app, "textReceived", message.OriginatingAddress.Length.ToString() + ':' +  message.OriginatingAddress.ToString() + message.MessageBody);
                //Toast.MakeText(context, message.MessageBody, ToastLength.Short).Show();
            }
        }
    }
}