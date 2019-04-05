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

using RallyUp.Droid;

namespace RallyUp.Droid
{
    [Activity(Label = "RallyUp", Icon = "@drawable/appIcon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            App app = new App();
            LoadApplication(app);

            AppCenter.Start("c29d077a-bb20-4b14-8349-5d7ad1dcf7cd", typeof(Analytics), typeof(Crashes));

            RequestPermissions(new string[] { "android.permission.SEND_SMS",
                                              "android.permission.READ_PHONE_STATE",
                                              "android.permission.READ_SMS",
                                              "android.permission.READ_CONTACTS",
                                              "android.permission.RECEIVE_SMS",
                                              "android.permission.BROADCAST_SMS" }, 0);

            Toast.MakeText(ApplicationContext, "Permissions Granted", ToastLength.Short).Show();

            SMSReceiver receiver = new SMSReceiver(app); ;

            MessagingCenter.Subscribe<RallyTimer>(app, "RallyStarted", (sender) =>
            {
                RegisterReceiver(receiver, new IntentFilter("android.provider.Telephony.SMS_RECEIVED"));
            });

            MessagingCenter.Subscribe<RallyTimer>(app, "RallyEnded", (sender) =>
            {
                UnregisterReceiver(receiver);
            });

            /*MessagingCenter.Subscribe<App, string>(app, "textReceived", (sender, arg) =>
            {
                string[] splitmessage = arg.Split(':');
                int addressLength = Convert.ToInt32(splitmessage[0]);
                string backMessage = arg.Substring(splitmessage[0].Length + 1);
                string address = backMessage.Substring(0, addressLength);
                string message = backMessage.Substring(addressLength);

                SemanticsDictionary dict = new SemanticsDictionary(message);
                int semanticInt = dict.analyzeSemantics();

                
                Toast.MakeText(this, address + " sent: " + message + " which has a semantics value of: " + semanticInt.ToString(), ToastLength.Long).Show();
            });*/
        }
    }
}