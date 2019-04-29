using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace RallyUp
{
    public partial class MainPage : ContentPage
    {
        TextSender sender;
        RallySetupPage setupPage;

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            if (!Application.Current.Properties.ContainsKey("RallyList"))
            {
                List<Rally> rallyList = new List<Rally>();
                Application.Current.Properties["RallyList"] = JsonConvert.SerializeObject(rallyList);
            }
            sender = new TextSender();
            setupPage = new RallySetupPage(sender);
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
            RunMyRallies();
        }

        async void RunEventSetup()
        {
            await Navigation.PushAsync(setupPage);
        }

        async void RunMyRallies()
        {
            await Navigation.PushAsync(new MyRallies(sender));
        }
    }
}