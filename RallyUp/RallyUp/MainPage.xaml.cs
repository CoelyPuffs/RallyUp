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
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            RunEventSetup();
        }

        async void RunEventSetup()
        {
            await Navigation.PushAsync(new RallySetupPage());
        }
    }
}