using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GiustoPrezzoApp
{
    public partial class Errore : ContentPage
    {
        public Errore()
        {
            InitializeComponent();
           
            imgErrore.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.Errore.png");
            btnReload.Source = ImageSource.FromResource("GiustoPrezzoApp.Immagini.GiocaAncora.png");
        }

        private void btnReload_OnClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new SplashPage();
        }
    }
}
