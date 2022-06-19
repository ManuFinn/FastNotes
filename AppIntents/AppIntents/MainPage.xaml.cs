using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppIntents
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ToastButton.Clicked += ToastButton_Clicked;
        }

        private void ToastButton_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<Toast>().show("Pipo");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<SnackInterface>().SnackbarShow("a");
        }
    }
}
