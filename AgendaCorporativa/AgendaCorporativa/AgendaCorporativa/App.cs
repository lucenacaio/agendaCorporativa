using AgendaCorporativa.Contratos;
using AgendaCorporativa.Controladores;
using AgendaCorporativa.Excecoes;
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
            try
            {
                VerificarArquivoContatos();

                ControleDeAutorizacao.Autorizar();

                // The root page of your application
                MainPage = InicializaPagina(new ContatosList());

                //Agenda para que toda semana exibe uma notificacao de atualizar a agenda.
                DependencyService
                    .Get<IGerenciadorDeNotificacao>()
                    .AgendaNotificacaoPeriodica(Modelos.Periodo.Semana, "Agenda Coorporativa", "Mantenha sua agenda atualizada!");
            }
            catch (ExcecaoDeAutenticacao erro)
            {
                MainPage = InicializaPagina(new PaginaDeErro());

                ControleArquivo.DeletarArquivo();

                var alerta = DependencyService.Get<IAlerta>();
                alerta.AlertaDialog("Erro", erro.Message);
            }
            // The root page of your application
            //MainPage = mainPage;

        }

        private NavigationPage InicializaPagina(ContentPage pagina)
        {
            NavigationPage mainPage;

            mainPage = new NavigationPage(pagina);
            mainPage.BarBackgroundColor = Color.FromHex("004E9E");
            mainPage.BarTextColor = Color.White;

            return mainPage;
        }

        /// <summary>
        /// Verifica se o arquivo ja esta baixado, caso não, baixa e salva
        /// </summary>
        private async void VerificarArquivoContatos()
        {
            if (string.IsNullOrWhiteSpace(ControleArquivo.LerArquivo()))
            {
                await ControleArquivo.BaixareSalvarArquivo();
            }
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
