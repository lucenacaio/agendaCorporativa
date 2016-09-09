using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgendaCorporativa.Modelos;

using Xamarin.Forms;
using AgendaCorporativa.Gerenciadores;
using AgendaCorporativa.Contratos;
using Plugin.Contacts.Abstractions;
using Plugin.Contacts;

namespace AgendaCorporativa
{
    public partial class ContatosList : ContentPage
    {
        GerenciadorDeContatos gerenciadorDeContatos;

        public ContatosList(IGerenciadorDeDownload gerenciadorDeDownload)
        {
            InitializeComponent();

            gerenciadorDeContatos = new GerenciadorDeContatos(gerenciadorDeDownload);
        }


        private async Task chamarContatos()
        {
            var contatos = await carregarAgenda();
            List<Contato> contatosLista = new List<Contato>();
            
            foreach (Contact contato in contatos)
            {
                Contato cont = new Contato();
                cont.NomeFuncionario = contato.DisplayName;
                contatosLista.Add(cont);
            }
        }

        private async Task<List<Contact>> carregarAgenda()
        {
            List<Contact> contatos = null;
            if (await CrossContacts.Current.RequestPermission())
            {
                CrossContacts.Current.PreferContactAggregation = false;//recommended
                                                                       //run in background
                await Task.Run(() =>
                {
                    if (CrossContacts.Current.Contacts == null)
                        return;

                    contatos = CrossContacts.Current.Contacts
                                            .Where(c => !string.IsNullOrWhiteSpace(c.FirstName) && c.Phones.Count > 0)?.ToList();

                    contatos = contatos.OrderBy(c => c.LastName).ToList();
                });
            }

            return contatos;
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await AtualizarContatos(true, syncItems: false);
        }

        async Task CompleteItem(Contato contato)
        {
            try
            {


                listaContatos.ItemsSource = await gerenciadorDeContatos.PesquisaContatos("");
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Contato;
            if (Device.OS != TargetPlatform.iOS && contato != null)
            {
                Navigation.PushAsync(new DetalharContato(contato));
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
            //await AtualizarContatos(true, true);
            await chamarContatos();
        }

        private async Task AtualizarContatos(bool showActivityIndicator, bool syncItems)
        {
            gerenciadorDeContatos.SincronizarContatos();

            listaContatos.ItemsSource = await gerenciadorDeContatos.PesquisaContatos("");
        }
    }
}
