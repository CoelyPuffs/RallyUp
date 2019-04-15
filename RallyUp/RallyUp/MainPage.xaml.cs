using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace RallyUp
{
    public partial class MainPage : ContentPage
    {
        RallySetupPage setupPage = new RallySetupPage();

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            /*if (Application.Current.Properties.ContainsKey("CurrentRally"))
            {
                RunCurrentRally();
            }
            else
            {
                RunEventSetup();
                MessagingCenter.Subscribe<RallySetupPage>(this, "RallyStarted", (sender) =>
                {
                    Navigation.RemovePage(setupPage);
                });
            }*/
        }

        void OnNewRallyButtonClicked(object thisSender, EventArgs arg)
        {
            RunEventSetup();
            MessagingCenter.Subscribe<RallySetupPage>(this, "RallyStarted", (sender) =>
            {
                Navigation.RemovePage(setupPage);
            });
        }

        void OnMyRalliesButtonClicked(object sender, EventArgs arg)
        {
            //RunCurrentRally();
        }

        async void RunEventSetup()
        {
            await Navigation.PushAsync(setupPage);
        }

        async void RunCurrentRally()
        {
            Rally currentRally = (Rally)Application.Current.Properties["CurrentRally"];
            await Navigation.PushAsync(new CurrentRally(currentRally));
        }
    }
}