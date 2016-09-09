using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgendaCorporativa
{
    public class App : Application
    {

        public App(Contratos.IGerenciadorDeDownload gerenciadorDeDownload)
        {
            NavigationPage mainPage = new NavigationPage(new ContatosList(gerenciadorDeDownload));
            mainPage.BarBackgroundColor = Color.FromHex("004E9E");
            mainPage.BarTextColor = Color.White;

            // The root page of your application
            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
