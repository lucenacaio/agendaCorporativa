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
            //isBloqueado = status;
            listaContatos.IsEnabled = !status;
        }

        public async void ButtonSincronizar_OnClick(object sender, EventArgs e)
        {
            //Caso seja iniciado uma chamada de serviço, bloqueia as views
            CarregandoDados(true);

            //Limpa o textbox de pesquisa
            nomePesquisa.Text = "";

            string titulo = "", msg = "";

            List<Contato> result = await Task.Run(() => Sincroniza(out titulo, out msg));

            listaContatos.ItemsSource = result;

            //Quando o serviço é finalizado seja ou não com sucesso, habilita todas as views
            CarregandoDados(false);

            IAlerta alerta = DependencyService.Get<IAlerta>();
            alerta.AlertaDialog(titulo, msg);
        }

        private List<Contato> Sincroniza(out string titulo, out string message)
        {
            titulo = "";
            message = "";

            //Não executa o método caso esteja sendo executado algum serviço
            if (!isBloqueado)
            {
                titulo = "Sucesso";
                message = "Atualização finalizada";

                try
                {
                    AtualizaMsgLoad("Baixando arquivo...");
                    gerenciadorDeContatos.BaixarArquivoDeContatos();

                    AtualizaMsgLoad("Carregando contatos do arquivo...");
                    Contatos = gerenciadorDeContatos.ObtemContatosDoArquivo();

                    AtualizaMsgLoad("Atualizando lista de contatos...");
                    DependencyService.Get<IGerenciadorDeAgenda>().AtualizarAgendaDoAparelho(Contatos);
                }
                catch (Exception ex)
                {
                    message = "Não foi possível recarregar os dados (" + ex.Message + ")";
                    titulo = "Erro";
                }
            }

            return Contatos;
        }

        private void AtualizaMsgLoad(string msg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                textLoad.Text = msg;
            });
        }
    }
}
