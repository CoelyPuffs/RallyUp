using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Newtonsoft.Json;

using RallyUp.Models;

namespace RallyUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentRally : ContentPage
    {
        StackLayout parentLayout = new StackLayout { BackgroundColor = Color.FromHex("ffbd44") };

        Rally currentRally;
        SemanticsDictionary rallyDictionary;
        RallyTimer timer;
        List<RallyContact> visibleInviteeList;
        TextSender sender;

        ScrollView inviteeScrollView = new ScrollView { BackgroundColor = Color.Transparent };
        StackLayout inviteeStack = new StackLayout { Orientation = StackOrientation.Vertical };
        List<Button> statusButtons = new List<Button>();

        public CurrentRally(Rally rallyData, TextSender textSender)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            currentRally = rallyData;
            timer = new RallyTimer(currentRally.end);
            this.sender = textSender;

            rallyDictionary = new SemanticsDictionary(currentRally.invitation);
            currentRally.refreshInviteeSummary();
            RenderCurrentRallyUI(currentRally);

            MessagingCenter.Subscribe<App, string>(this, "textReceived", (sender, arg) => {
                analyzeTextMessage(arg);
            });
        }

        void RenderCurrentRallyUI(Rally currentRally)
        {
            Grid upperLayout = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                MinimumHeightRequest = 100
            };
            upperLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            upperLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            upperLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            upperLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Label timerLabel = new Label
            {
                TextColor = Color.Black,
                FontSize = 40,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            timerLabel.BindingContext = timer;
            timerLabel.SetBinding(Label.TextProperty, "TimerString");
            upperLayout.Children.Add(timerLabel, 1, 0);

            Button messageInviteesButton = new Button
            {
                Text = "\U00002709",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.End
            };
            messageInviteesButton.Clicked += delegate
            {
                openNewMessagePage();
            };
            upperLayout.Children.Add(messageInviteesButton, 2, 0);

            parentLayout.Children.Add(upperLayout);

            Label invitation = new Label
            {
                Text = currentRally.invitation,
                TextColor = Color.Black,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            parentLayout.Children.Add(invitation);

            Label summary = new Label
            {
                TextColor = Color.Black,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            summary.BindingContext = currentRally;
            summary.SetBinding(Label.TextProperty, "inviteeSummary");
            parentLayout.Children.Add(summary);

            Picker sortPicker = new Picker
            {
                Title = "Sort by",
                TextColor = Color.Black,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Fill,
                BackgroundColor = Color.Transparent
            };

            sortPicker.Items.Add("Sort A-Z");
            sortPicker.Items.Add("Sort Z-A");
            sortPicker.Items.Add("Show Only Accepted");
            sortPicker.Items.Add("Show Only Declined");
            sortPicker.Items.Add("Show Only Undetermined");
            sortPicker.Items.Add("Show Only Unreplied");

            sortPicker.SelectedIndexChanged += delegate
            {
                switch (sortPicker.SelectedIndex)
                {
                    case 0:
                        visibleInviteeList = currentRally.invitees.OrderBy(i => i.Name).ToList();
                        refreshVisibleInviteeList();
                        break;
                    case 1:
                        visibleInviteeList = currentRally.invitees.OrderByDescending(i => i.Name).ToList();
                        refreshVisibleInviteeList();
                        break;
                    case 2:
                        visibleInviteeList = currentRally.invitees.Where(i => (i.Status == 3)).ToList();
                        refreshVisibleInviteeList();
                        break;
                    case 3:
                        visibleInviteeList = currentRally.invitees.Where(i => (i.Status == 2)).ToList();
                        refreshVisibleInviteeList();
                        break;
                    case 4:
                        visibleInviteeList = currentRally.invitees.Where(i => (i.Status == 1)).ToList();
                        refreshVisibleInviteeList();
                        break;
                    case 5:
                        visibleInviteeList = currentRally.invitees.Where(i => (i.Status == 0)).ToList();
                        refreshVisibleInviteeList();
                        break;
                }
            };
            sortPicker.SelectedIndex = 0;

            parentLayout.Children.Add(sortPicker);

            inviteeScrollView = new ScrollView {
                BackgroundColor = Color.Transparent
            };
            inviteeStack = new StackLayout { Orientation = StackOrientation.Vertical };

            foreach (RallyContact invitee in visibleInviteeList)
            {
                StackLayout inviteeStackTemplate = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(10)
                };

                Button inviteeTemplate = new Button
                {
                    Text = invitee.Name,
                    TextColor = Color.Black,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    BackgroundColor = Color.Transparent
                };

                Button semanticDisplayButton = new Button
                {
                    HorizontalOptions = LayoutOptions.End,
                    BackgroundColor = Color.Transparent,
                    TextColor = Color.Black,
                    FontSize = 20
                };
                semanticDisplayButton.Clicked += delegate
                {
                    invitee.incrementStatus();
                    currentRally.refreshInviteeSummary();
                };
                semanticDisplayButton.BindingContext = invitee;
                semanticDisplayButton.SetBinding(Button.TextProperty, "StatusString");

                /*ImageButton statusTemplate = new ImageButton
                {
                    Source = "checkMark.png"
                };*/

                //statusButtons.Add(semanticDisplayButton);

                inviteeStackTemplate.Children.Add(inviteeTemplate);
                inviteeStackTemplate.Children.Add(semanticDisplayButton);

                inviteeStack.Children.Add(inviteeStackTemplate);
            }
            inviteeScrollView.Content = inviteeStack;
            parentLayout.Children.Add(inviteeScrollView);

            Label errorLabel = new Label
            {
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Center
            };
            parentLayout.Children.Add(errorLabel);

            this.Content = parentLayout;
        }

        void analyzeTextMessage(string textMessage)
        {
            string[] splitmessage = textMessage.Split(':');
            int addressLength = Convert.ToInt32(splitmessage[0]);
            string backMessage = textMessage.Substring(splitmessage[0].Length + 1);
            string address = backMessage.Substring(0, addressLength);
            string message = backMessage.Substring(addressLength);

            RallyContact replier = currentRally.invitees.Find(x => x.Number == address.ToString());

            if (replier != null)
            {
                if (replier.Status == 0)
                {
                    replier.modifyStatus(1);
                }

                int semanticInt = rallyDictionary.analyzeSemantics(message);
                if (semanticInt < 0)
                {
                    replier.modifyStatus(2);
                }
                else if(semanticInt > 0)
                {
                    replier.modifyStatus(3);
                }
                currentRally.refreshInviteeSummary();
                List<Rally> rallyList = JsonConvert.DeserializeObject<List<Rally>>(Application.Current.Properties["RallyList"] as string);
                rallyList.Find(x => x.end == currentRally.end && x.invitation == currentRally.invitation).invitees = currentRally.invitees;
                Application.Current.Properties["RallyList"] = JsonConvert.SerializeObject(rallyList);
            }

            //Toast.MakeText(this, address + " sent: " + message + " which has a semantics value of: " + semanticInt.ToString(), ToastLength.Long).Show();
        }

        void refreshVisibleInviteeList()
        {
            inviteeStack = new StackLayout { Orientation = StackOrientation.Vertical };

            foreach (RallyContact invitee in visibleInviteeList)
            {
                StackLayout inviteeStackTemplate = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Margin = new Thickness(10)
                };

                Button inviteeTemplate = new Button
                {
                    Text = invitee.Name,
                    TextColor = Color.Black,
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    BackgroundColor = Color.Transparent
                };

                Button semanticDisplayButton = new Button
                {
                    HorizontalOptions = LayoutOptions.End,
                    BackgroundColor = Color.Transparent,
                    TextColor = Color.Black,
                    FontSize = 20
                };
                semanticDisplayButton.Clicked += delegate
                {
                    invitee.incrementStatus();
                    currentRally.refreshInviteeSummary();
                };
                semanticDisplayButton.BindingContext = invitee;
                semanticDisplayButton.SetBinding(Button.TextProperty, "StatusString");

                //statusButtons.Add(semanticDisplayButton);

                inviteeStackTemplate.Children.Add(inviteeTemplate);
                inviteeStackTemplate.Children.Add(semanticDisplayButton);

                inviteeStack.Children.Add(inviteeStackTemplate);
            }
            inviteeScrollView.Content = inviteeStack;
        }

        async void openNewMessagePage()
        {
            var newMessagePage = new MessageInviteesPage(currentRally.invitees, this.sender);
            await Navigation.PushAsync(newMessagePage);
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
    }
}