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
using Xamarin.Forms;
using AgendaCorporativa.Droid.Gerenciadores;

[assembly: Dependency(typeof(GerenciadorDeChamadasDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    /// <summary>
    /// Classe responsavel pelo tratamento de chamadas no sistema Android
    /// </summary>
    public class GerenciadorDeChamadasDroid : IGerenciadorDeChamadas
    {
        public void ChamarNumero(string numero)
        {
            var uri = Android.Net.Uri.Parse("tel:" + numero);
            var intent = new Intent(Intent.ActionDial, uri);
            Forms.Context.StartActivity(intent);
        }
    }
}