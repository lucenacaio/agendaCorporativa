using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaCorporativa.Modelos;

using Xamarin.Forms;
using AgendaCorporativa.Gerenciadores;

namespace AgendaCorporativa
{
    public partial class ContatosList : ContentPage
    {
        GerenciadorDeContatos gerenciador;
        public ContatosList()
        {
            InitializeComponent();
            gerenciador = new GerenciadorDeContatos();

            //todo verificar se é melhor criar o botão dinamico ou deixar no xaml @mpleite1
            //var syncButton = new Button
            //{
            //    Text = "Sincronizar Contatos",
            //    HeightRequest = 30
            //};
            //syncButton.Clicked += OnSyncItems;

            //botoesPainel.Children.Add(syncButton);

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await AtualizarContatos(true, syncItems: false);
        }

        async Task CompleteItem(Contato contato)
        {
            listaContatos.ItemsSource = await gerenciador.PesquisaContatos("");
        }

        public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Contato;
            if (Device.OS != TargetPlatform.iOS && contato != null)
            {
                await DisplayAlert(contato.NomeCompleto, "Selecionado: " + contato.NomeCompleto, "OK");

            }
        }

        public async void OnComplete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var contato = mi.CommandParameter as Contato;
            await CompleteItem(contato);
        }

        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;
            try
            {
                await AtualizarContatos(false, true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Erro ao recarregar", "Não foi possível recarregar os dados (" + error.Message + ")", "OK");
            }
        }

        public async void OnSyncItems(object sender, EventArgs e)
        {
            await AtualizarContatos(true, true);
        }

        private async Task AtualizarContatos(bool showActivityIndicator, bool syncItems)
        {
            listaContatos.ItemsSource = await gerenciador.PesquisaContatos("");
        }
    }
}
