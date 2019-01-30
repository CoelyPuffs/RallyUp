using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Telephony;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

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

            string[] permissions = { "SEND_SMS, READ_SMS", "READ_PHONE_STATE" };
            RequestPermissions(permissions, 1);

            SMSListener testListener = new SMSListener();
        }
    }
}