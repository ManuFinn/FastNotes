﻿using MarcTron.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Plugin.LatestVersion;

namespace MyNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoView : ContentPage
    {
        public InfoView()
        {
            

            InitializeComponent();
            //var isLatest = await CrossLatestVersion.Current
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CrossMTAdmob.Current.ShowRewardedVideo();
            //CrossMTAdmob.Current.
        }
    }
}