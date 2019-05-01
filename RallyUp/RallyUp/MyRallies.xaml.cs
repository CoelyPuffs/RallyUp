using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RallyUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRallies : ContentPage
    {
        TextSender sender;
        bool isDeleting = false;

        public MyRallies(TextSender sender)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            RenderUI();
            this.sender = sender;
        }

        public void RenderUI()
        {
            StackLayout fullStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };
            List<Rally> rallyList = JsonConvert.DeserializeObject<List<Rally>>(Application.Current.Properties["RallyList"] as string);
            ScrollView scrollView = new ScrollView { BackgroundColor = Color.Transparent };
            StackLayout scrollStack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Margin = new Thickness(10)
            };

            Grid upperLayout = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                MinimumHeightRequest = 100
            };
            upperLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            upperLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            upperLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            upperLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Label titleLabel = new Label
            {
                Text = "My Rallies",
                TextColor = Color.Black,
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            upperLayout.Children.Add(titleLabel, 1, 0);

            Button deleteRallyButton = new Button
            {
                Text = "\U0001F5D1",
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.End,
                BackgroundColor = Color.LightGray
            };
            deleteRallyButton.Clicked += delegate
            {
                isDeleting = !isDeleting;
                if (isDeleting)
                {
                    deleteRallyButton.BackgroundColor = Color.Red;
                }
                else
                {
                    deleteRallyButton.BackgroundColor = Color.LightGray;
                }
            };
            upperLayout.Children.Add(deleteRallyButton, 2, 0);

            fullStack.Children.Add(upperLayout);

            foreach (Rally pastRally in rallyList)
            {
                Button rallyButtonTemplate = new Button
                {
                    Text = pastRally.start.ToShortDateString() + " " + pastRally.invitation,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.LightGray
                };
                rallyButtonTemplate.Clicked += delegate
                {
                    if (isDeleting)
                    {
                        rallyList.Remove(pastRally);
                        Application.Current.Properties["RallyList"] = JsonConvert.SerializeObject(rallyList);
                        scrollStack.Children.Remove(rallyButtonTemplate);
                    }
                    else
                    {
                        startRally(pastRally);
                    }
                };
                scrollStack.Children.Add(rallyButtonTemplate);
            }

            scrollView.Content = scrollStack;
            fullStack.Children.Add(scrollStack);
            this.Content = fullStack;
        }

        async public void startRally(Rally selectedRally)
        {
            await Navigation.PushAsync(new CurrentRally(selectedRally, sender));
        }
    }
}