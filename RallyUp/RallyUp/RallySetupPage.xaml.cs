using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Messaging;
using Plugin.ContactService;
using Plugin.ContactService.Shared;

using Newtonsoft.Json;

using RallyUp.Models;
using RallyUp;

namespace RallyUp
{
    public partial class RallySetupPage : ContentPage
    {
        TextSender sender;

        StackLayout parentLayout = new StackLayout { BackgroundColor = Color.FromHex("ffbd44") };

        List<RallyContact> selectedContacts = new List<RallyContact>();

        public RallySetupPage(TextSender sender)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            RenderSetupUI();
            this.sender = sender;
        }

        async void RenderSetupUI()
        {
            IList<Contact> contacts = await CrossContactService.Current.GetContactListAsync();
            IEnumerable<Contact> contactEnums = contacts.OrderBy(c => c.Name);
            contacts = contactEnums.ToList();

            ScrollView scrollView = new ScrollView { BackgroundColor = Color.Transparent };
            StackLayout fullStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(10)
            };

            Label title = new Label
            {
                Text = "START A RALLY",
                TextColor = Color.Black,
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            parentLayout.Children.Add(title);

            GrowingEditor invitationBox = new GrowingEditor
            {
                BackgroundColor = Color.AntiqueWhite,
                Placeholder = "Invitation goes here...",
                PlaceholderColor = Color.Gray,
                FontSize = 20,
                TextColor = Color.Black,
                Margin = new Thickness(10)
            };
            parentLayout.Children.Add(invitationBox);

            StackLayout addByNumberStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            GrowingEditor addByNumberEditor = new GrowingEditor
            {
                BackgroundColor = Color.AntiqueWhite,
                Placeholder = "Number goes here...",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black,
                Keyboard = Keyboard.Numeric
            };
            addByNumberStack.Children.Add(addByNumberEditor);

            Button addByNumberButton = new Button
            {
                BackgroundColor = Color.LightGray,
                Text = "Add by number"
            };
            addByNumberStack.Children.Add(addByNumberButton);
            addByNumberButton.Clicked += delegate
            {
                if (addByNumberEditor.Text != "")
                {
                    Contact contact = new Contact
                    {
                        Name = addByNumberEditor.Text,
                        Number = addByNumberEditor.Text
                    };

                    Button buttonTemplate = new Button
                    {
                        Text = addByNumberEditor.Text,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.LightGreen
                    };
                    selectedContacts.Add(new RallyContact(contact.Name, contact.Number));
                    buttonTemplate.Clicked += delegate
                    {
                        if (buttonTemplate.BackgroundColor == Color.LightGreen)
                        {
                            buttonTemplate.BackgroundColor = Color.LightGray;
                            selectedContacts.Remove(selectedContacts.Find(x => x.Name == contact.Name && x.Number == contact.Number));
                        }
                        else
                        {
                            buttonTemplate.BackgroundColor = Color.LightGreen;
                            selectedContacts.Add(new RallyContact(contact.Name, contact.Number));
                        }
                    };

                    fullStack.Children.Add(buttonTemplate);
                }
            };

            parentLayout.Children.Add(addByNumberStack);

            foreach (Contact contact in contacts.Where(x => x.Number != null))
            {
                if (contact.Number.Length >= 10)
                {
                    Button buttonTemplate = new Button
                    {
                        Text = contact.Name,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        BackgroundColor = Color.LightGray
                    };
                    buttonTemplate.Clicked += delegate
                    {
                        if (buttonTemplate.BackgroundColor == Color.LightGreen)
                        {
                            buttonTemplate.BackgroundColor = Color.LightGray;
                            selectedContacts.Remove(selectedContacts.Find(x => x.Name == contact.Name && x.Number == contact.Number));
                        }
                        else
                        {
                            buttonTemplate.BackgroundColor = Color.LightGreen;
                            selectedContacts.Add(new RallyContact(contact.Name, contact.Number));
                        }
                    };
                    fullStack.Children.Add(buttonTemplate);
                }
            }
            scrollView.Content = fullStack;
            parentLayout.Children.Add(scrollView);

            Label errorLabel = new Label
            {
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Center
            };
            parentLayout.Children.Add(errorLabel);

            Button startRallyButton = new Button
            {
                Text = "Rally Up!",
                TextColor = Color.Black,
                FontSize = 30,
                HorizontalOptions = LayoutOptions.Center
            };
            startRallyButton.Clicked += delegate
            {
                if (invitationBox.Text == "" || invitationBox.Text == null)
                {
                    errorLabel.IsVisible = true;
                    errorLabel.Text = "Error: Invitation message missing";
                }
                else if (selectedContacts.Count <= 0)
                {
                    errorLabel.IsVisible = true;
                    errorLabel.Text = "Error: No contacts selected";
                }
                else
                {
                    foreach (RallyContact selected in selectedContacts)
                    {
                        // For testing purposes only
                        //if (selected.Name == "Kerstan Thio")
                        //{
                        sender.sendText(selected.Number.ToString(), invitationBox.Text);
                        //}
                    }
                    /*var smsMessenger = CrossMessaging.Current.SmsMessenger;
                    if (smsMessenger.CanSendSmsInBackground)
                    {
                        foreach (RallyContact selected in selectedContacts)
                        {
                            // For testing purposes only
                            //if (selected.Name == "Kerstan Thio")
                            //{
                                smsMessenger.SendSmsInBackground(selected.Number.ToString(), invitationBox.Text);
                            //}
                        }
                    }*/

                    InitializeRallyUI(invitationBox.Text);
                }
            };
            parentLayout.Children.Add(startRallyButton);

            this.Content = parentLayout;
        }

        async void InitializeRallyUI(string invitation)
        {
            DateTime rallyStartTime = DateTime.Now;

            Rally newRally = new Rally
            {
                start = rallyStartTime,
                end = rallyStartTime.AddMinutes(10),
                invitation = invitation,
                invitees = selectedContacts
            };
            newRally.refreshInviteeSummary();

            List<Rally> rallyList = JsonConvert.DeserializeObject<List<Rally>>(Application.Current.Properties["RallyList"] as string);
            rallyList.Add(newRally);
            Application.Current.Properties["RallyList"] = JsonConvert.SerializeObject(rallyList);

            var newRallyPage = new CurrentRally(newRally, sender);
            await Navigation.PushAsync(newRallyPage);

            MessagingCenter.Send<RallySetupPage>(this, "RallyStarted");
        }
    }
}