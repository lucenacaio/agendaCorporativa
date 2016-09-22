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
using Stefanini.Xamarin.Gerenciadores;

namespace AgendaCorporativa
{
    public partial class ContatosList : ContentPage
    {
        GerenciadorDeContatos gerenciadorDeContatos;

        public List<Contato> Contatos;

        private bool isBloqueado { get; set; }

        public ContatosList()
        {
            gerenciadorDeContatos = new GerenciadorDeContatos(DependencyService.Get<IGerenciadorDeDownload>());

            //Obtem os contatos do arquivo local
            Contatos = gerenciadorDeContatos.ObtemContatosDoArquivo();

            InitializeComponent();

            listaContatos.ItemsSource = Contatos;
        }

        private void Pesquisa_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            listaContatos.ItemsSource = (from contato in Contatos
                                         where contato.NomeCompleto.ToUpper().Contains(e.NewTextValue.ToUpper())
                                         orderby contato.NomeCompleto
                                         select contato)?.ToList();
        }

        public void ListaContatos_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contato = e.SelectedItem as Contato;
            if (contato != null)
            {
                if (!isBloqueado)
                    Navigation.PushAsync(new DetalharContato(contato));
            }
        }

        private void CarregandoDados(bool status)
        {
            syncIndicator.IsVisible = status;
            isBloqueado = status;
            listaContatos.IsEnabled = !status;
        }

        public async void ButtonSincronizar_OnClick(object sender, EventArgs e)
        {
           

            Exception error = null;
            try
            {
                //Não executa o método caso esteja sendo executado algum serviço
                if (isBloqueado) return;

                //Caso seja iniciado uma chamada de serviço, bloqueia as views
                CarregandoDados(true);
                //Limpa o textbox de pesquisa
                nomePesquisa.Text = "";

                //Baixa o arquivo
                await gerenciadorDeContatos.BaixarArquivoDeContatos();

                //Carrega os contatos do arquivo
                Contatos = gerenciadorDeContatos.ObtemContatosDoArquivo();
                listaContatos.ItemsSource = Contatos;

                //Carrega os contatos da Agenda
                //List<Contact> contatosDoAparelho = await GerenciadorDeAgenda.CarregaAgendaDoAparelho();

                //TODO - Atualizar a agenda do aparelho.
                DependencyService.Get<IGerenciadorDeAgenda>().AtualizarAgendaDoAparelho(Contatos);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                await DisplayAlert("Erro ao recarregar", "Não foi possível recarregar os dados (" + error.Message + ")", "OK");
            }
            //Quando o serviço é finalizado seja ou não com sucesso, habilita todas as views
            CarregandoDados(false);
        }
    }
}
