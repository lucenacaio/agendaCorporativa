using AgendaCorporativa.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AgendaCorporativa
{
    public class App : Application
    {
        public App()
        {
            NavigationPage mainPage = new NavigationPage(new ContatosList());
            mainPage.BarBackgroundColor = Color.FromHex("004E9E");
            mainPage.BarTextColor = Color.White;

            // The root page of your application
            MainPage = mainPage;

            //DependencyService
            //    .Get<IGerenciadorDeNotificacao>()
            //    .AgendaNotificacao(DateTime.Now.AddDays(7), "Agenda Coorporativa", "Mantenha sua agenda atualizada!");
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
