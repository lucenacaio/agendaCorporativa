using AgendaCorporativa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AgendaCorporativa
{
    public partial class DetalharContato : ContentPage
    {
        Contato _contato;


        public DetalharContato()
        {
            InitializeComponent();

            Mock();
            LoadDados();
            listaTelefones.SeparatorVisibility = Xamarin.Forms.SeparatorVisibility.Default;
            listaTelefones.SeparatorColor = Color.FromHex("C8C7CC");
        }

        void Mock()
        {
            _contato = new Contato();
            _contato.Email = "email@mock.com.br";
            _contato.Telefones = new List<Telefone>()
            {
                new Telefone {Numero="981274020", DDD="83" },
                new Telefone {Numero="988881234", DDD="41" },
                new Telefone {Numero="912348765", DDD="21" }
            };
            _contato.NomeFuncionario = "Fulano";
            _contato.SobrenomeFuncionario = "Mock Mocado";
            _contato.NomeEmpresa = "Stefanini";
        }

        void LoadDados()
        {
            nomeSobrenome.Text = _contato.NomeCompleto;
            empresa.Text = _contato.NomeEmpresa;
            LoadListaTelefones();
            LoadListaEmails();
        }

        void LoadListaTelefones()
        {
            listaTelefones.ItemsSource = _contato.Telefones;
        }

        void LoadListaEmails()
        {
            List<string> emails = new List<string>();
            emails.Add(_contato.Email);
            listaEmails.ItemsSource = emails;
        }

    }
}
