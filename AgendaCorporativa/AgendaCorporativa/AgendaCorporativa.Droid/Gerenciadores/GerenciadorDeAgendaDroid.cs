using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AgendaCorporativa.Contratos;
using Plugin.Contacts.Abstractions;
using AgendaCorporativa.Droid.Gerenciadores;
using Xamarin.Forms;
using AgendaCorporativa.Modelos;

[assembly: Dependency(typeof(GerenciadorDeAgendaDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    class GerenciadorDeAgendaDroid : IGerenciadorDeAgenda
    {

        public GerenciadorDeAgendaDroid() { }

        public void AtualizarAgendaDoAparelho(List<Contato> contatos)
        {
            throw new NotImplementedException();
        }
    }
}