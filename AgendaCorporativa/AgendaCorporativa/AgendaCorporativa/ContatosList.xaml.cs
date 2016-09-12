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

        private async void Pesquisa_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            listaContatos.ItemsSource = await gerenciadorDeContatos.PesquisaContatos(e.NewTextValue);
        }

        private async Task chamarContatos()
        {
            var contatos = await carregarAgenda();
            List<Contato> contatosLista = new List<Contato>();


            foreach (Contact contato in contatos)
            {
				List<Telefone> telefones = new List<Telefone>();
				List<EmailCorp> emails = new List<EmailCorp>();

				foreach (Phone phone in contato.Phones) 
				{
					Telefone telefone = new Telefone();
					telefone.Numero = phone.Number;
					telefones.Add(telefone);
				}

				foreach (Email email in contato.Emails)
				{
					EmailCorp emailCorp = new EmailCorp();
					emailCorp.Endereco = email.Address;
					emails.Add(emailCorp);
				}

                Contato cont = new Contato();
                cont.NomeFuncionario = contato.DisplayName;
				cont.Telefones = telefones;
				cont.Emails = emails;

                contatosLista.Add(cont);
            }
			listaContatos.ItemsSource = contatosLista;
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

                    var contacts = CrossContacts.Current.Contacts
                                            .Where(c => !string.IsNullOrWhiteSpace(c.FirstName) && c.Phones.Count > 0);

                    contatos = contacts.OrderBy(c => c.FirstName).ToList();
                });
            }

            return contatos;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await AtualizarContatos(true, syncItems: false);
        }

        public void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Contato;
            if (contato != null)
            {
                Navigation.PushAsync(new DetalharContato(contato));
            }
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
