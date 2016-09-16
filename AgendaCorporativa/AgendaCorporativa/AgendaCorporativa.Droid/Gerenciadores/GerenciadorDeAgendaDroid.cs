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

[assembly: Dependency(typeof(GerenciadorDeAgendaDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    class GerenciadorDeAgendaDroid : IGerenciadorDeAgenda
    {

        public GerenciadorDeAgendaDroid() { }
        public void AtualizarAgendaDoAparelho(List<Contact> contatos)
        {
            throw new NotImplementedException();
        }
    }
}