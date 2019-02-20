using System;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;

using Xamarin.Forms;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Plugin.Messaging;

namespace RallyUp.Droid
{
    [Activity(Label = "RallyUp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            AppCenter.Start("c29d077a-bb20-4b14-8349-5d7ad1dcf7cd", typeof(Analytics), typeof(Crashes));

            RequestPermissions(new string[] { "android.permission.SEND_SMS",
                                              "android.permission.READ_PHONE_STATE",
                                              "android.permission.READ_SMS",
                                              "android.permission.READ_CONTACTS",
                                              "android.permission.RECEIVE_SMS",
                                              "android.permission.BROADCAST_SMS" }, 0);

            Toast.MakeText(ApplicationContext, "Permissions Granted", ToastLength.Short).Show();

            neoReceiver receiver = new neoReceiver();
            RegisterReceiver(receiver, new IntentFilter("android.provider.Telephony.SMS_RECEIVED"));
        }
    }

    public class neoReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var messages = Android.Provider.Telephony.Sms.Intents.GetMessagesFromIntent(intent);
            foreach (var message in messages)
            {
                // MessagingCenter.Send<neoReceiver>(this, message.OriginatingAddress.Length.ToString() + message.OriginatingAddress.ToString() + message.MessageBody);
                Toast.MakeText(context, message.MessageBody, ToastLength.Short).Show();
            }
        }
    }
}