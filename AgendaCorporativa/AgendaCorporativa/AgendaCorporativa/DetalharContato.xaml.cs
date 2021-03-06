﻿using AgendaCorporativa.Contratos;
using AgendaCorporativa.Modelos;
using Stefanini.Xamarin.Gerenciadores;
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


        public DetalharContato(Contato contato)
        {
            InitializeComponent();

            _contato = contato;

            LoadDados();
            listaTelefones.SeparatorVisibility = Xamarin.Forms.SeparatorVisibility.Default;
            listaTelefones.SeparatorColor = Color.FromHex("C8C7CC");
        }

        public void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var telefone = e.SelectedItem as Telefone;
            GerenciadorDeTelefone.RealizarChamada(telefone.DDD, telefone.Numero);
            
        }

        /// <summary>
        /// Carrega Nome e Empresa de contato e chama o carregamento de Telefones e emails
        /// </summary>
        void LoadDados()
        {
            nomeSobrenome.Text = _contato.NomeCompleto;
            empresa.Text = _contato.NomeEmpresa;
            LoadListaTelefones();
            LoadListaEmails();
        }

        /// <summary>
        /// Carrega lista de telefones
        /// </summary>
        void LoadListaTelefones()
        {
            listaTelefones.ItemsSource = _contato.Telefones;
        }

        /// <summary>
        /// Carregar lista de emails
        /// </summary>
        void LoadListaEmails()
        {
            listaEmails.ItemsSource = _contato.Emails;
        }

    }
}
