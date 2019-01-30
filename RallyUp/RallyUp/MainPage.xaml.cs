using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Messaging;

namespace RallyUp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object Sender, EventArgs args)
        {
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSmsInBackground)
            {
                // smsMessenger.SendSmsInBackground("5107011865", "Testing");
                for (int i = 0; i < 10; i++)
                {
                    smsMessenger.SendSmsInBackground("4074919960", "This is for the kitkat");
                }
            }
        }
    }
}
