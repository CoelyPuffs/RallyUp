using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Messaging;
using Plugin.ContactService;

using RallyUp.Models;
using RallyUp;

namespace RallyUp
{
    public partial class MainPage : ContentPage
    {
        StackLayout parentLayout = new StackLayout();

        List<Contact> selectedContacts = new List<Contact>();

        public MainPage()
        {
            InitializeComponent();
            RenderUI();
            // MessagingCenter.Subscribe<> 
        }

        async void RenderUI()
        {
            var contacts = await CrossContactService.Current.GetContactListAsync();
            List<Contact> contactList = new List<Contact>();

            ScrollView scrollView = new ScrollView();
            StackLayout fullStack = new StackLayout { Orientation = StackOrientation.Vertical };
            

            foreach (Plugin.ContactService.Shared.Contact contact in contacts)
            {
                Button buttonTemplate = new Button
                {
                    Text = contact.Name,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                buttonTemplate.Clicked += delegate
                {
                    if (buttonTemplate.BackgroundColor == Color.LightSeaGreen)
                    {
                        buttonTemplate.BackgroundColor = Color.LightGray;
                        selectedContacts.Remove(selectedContacts.Find(x => x.Name == contact.Name && x.Number == contact.Number));
                    }
                    else
                    {
                        buttonTemplate.BackgroundColor = Color.LightSeaGreen;
                        selectedContacts.Add(new Contact(contact.Name, contact.Number));
                    }
                };
                fullStack.Children.Add(new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        buttonTemplate
                    }
                });
                //contactList.Add(new Contact(contact.Name, contact.Number));
            }
            scrollView.Content = fullStack;
            parentLayout.Children.Add(scrollView);

            Button selectContactsButton = new Button();
            selectContactsButton.Text = "Select Contacts";
            selectContactsButton.Clicked += delegate
            {
                string selectedContactsString = "";
                foreach (Contact selected in selectedContacts)
                {
                    selectedContactsString += selected.Name + '\n';
                }
                DisplayAlert("Selected Contacts", selectedContactsString, "OK");
            };
            parentLayout.Children.Add(selectContactsButton);
            
            this.Content = parentLayout;
            DisplayAlert("Done", "Done loading contacts", "OK");
        }

        private void OnSelectContactsButtonClicked(object sender, EventArgs e)
        {
            selectedContacts = new List<Contact>();
            string selectedContactsString = "";
            /* foreach (Xamarin.Forms.MultiSelectListView.SelectableItem contact in contactList.Contacts)
            {
                if (contact.IsSelected)
                {
                    selectedContacts.Add((Contact)contact.Data);
                    selectedContactsString += ((Contact)contact.Data).Name + '\n';
                }
            }*/
            DisplayAlert("Selected Contacts", selectedContactsString, "OK");
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