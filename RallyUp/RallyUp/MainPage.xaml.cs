using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Messaging;
using Plugin.ContactService;

using RallyUp.ViewModel;

namespace RallyUp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //BindingContext = new ContactsViewModel();

            DisplayContacts();
        }

        async void DisplayContacts()
        {
            var contacts = await CrossContactService.Current.GetContactListAsync();

            BindingContext = new ContactsViewModel(contacts);

            /*contactNames = new string[contacts.Count];

            for (int i = 0; i < contacts.Count; i++)
            {
                contactNames[i] = contacts[i].Name;
            }

            ContactsListView.ItemsSource = contactNames;
            ContactsListView.BackgroundColor = Color.White;*/
        }

        private void OnSelectContactsButtonClicked(object sender, EventArgs e)
        {

        }

        void OnSendButtonClicked(object Sender, EventArgs args)
        {
            var smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSmsInBackground)
            {
                smsMessenger.SendSmsInBackground("5107011865", "Testing");
            }
        }
    }
}
