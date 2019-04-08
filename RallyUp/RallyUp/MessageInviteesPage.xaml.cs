using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.Messaging;
using Plugin.ContactService;
using Plugin.ContactService.Shared;

using RallyUp;
using RallyUp.Models;

namespace RallyUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageInviteesPage : ContentPage
    {
        List<RallyContact> recipientList = new List<RallyContact>();
        List<RallyContact> rallyList;
        bool acceptedSelected = false;
        bool unsureSelected = false;
        bool declinedSelected = false;

        public MessageInviteesPage(List<RallyContact> inviteeList)
        {
            InitializeComponent();
            rallyList = inviteeList;
        }

        void OnSelectAcceptedButtonClicked(object sender, EventArgs args)
        {
            if (acceptedSelected)
            {
                recipientList.RemoveAll(x => (x.Status == 3));
                selectAccepted.BackgroundColor = Color.LightGray;
            }
            else
            {
                recipientList.AddRange(rallyList.FindAll(x => (x.Status == 3)));
                selectAccepted.BackgroundColor = Color.LightGreen;
            }
            acceptedSelected = !acceptedSelected;
        }

        void OnSelectUnsureButtonClicked(object sender, EventArgs args)
        {
            if (unsureSelected)
            {
                recipientList.RemoveAll(x => (x.Status <= 1));
                selectUnsure.BackgroundColor = Color.LightGray;
            }
            else
            {
                recipientList.AddRange(rallyList.FindAll(x => (x.Status <= 1)));
                selectUnsure.BackgroundColor = Color.LightGreen;
            }
            unsureSelected = !unsureSelected;
        }

        void OnSelectDeclinedButtonClicked(object sender, EventArgs args)
        {
            if (declinedSelected)
            {
                recipientList.RemoveAll(x => (x.Status == 2));
                selectDeclined.BackgroundColor = Color.LightGray;
            }
            else
            {
                recipientList.AddRange(rallyList.FindAll(x => (x.Status == 2)));
                selectDeclined.BackgroundColor = Color.LightGreen;
            }
            declinedSelected = !declinedSelected;
        }

        async void OnSendButtonClicked(object sender, EventArgs args)
        {
            if (messageEditor.Text == "" || messageEditor.Text == null)
            {
                messageErrorBox.IsVisible = true;
                messageErrorBox.Text = "Error: Message missing";
            }
            else if (recipientList == new List<RallyContact>())
            {
                messageErrorBox.IsVisible = true;
                messageErrorBox.Text = "Error: No groups selected";
            }
            else
            {
                var smsMessenger = CrossMessaging.Current.SmsMessenger;
                if (smsMessenger.CanSendSmsInBackground)
                {
                    foreach (RallyContact selected in recipientList)
                    {
                        // For testing purposes only
                        if (selected.Name == "Kerstan Thio")
                        {
                            smsMessenger.SendSmsInBackground(selected.Number.ToString(), messageEditor.Text);
                        }
                    }
                }

                DependencyService.Get<IMessage>().LongAlert("Message sent!");

                Navigation.RemovePage(this);
            }
        }
    }
}