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
using AgendaCorporativa.Droid.Gerenciadores;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertaDialogDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    public class AlertaDialogDroid : IAlerta
    {
        public void AlertaDialog(string titulo, string msg)
        {
            AlertDialog.Builder builder = PrepareAlerta(titulo, msg);
            builder.SetPositiveButton("OK", delegate { });
            builder.Show();
        }

        public void AlertaDialogAndCloseApp(string titulo, string msg)
        {
            AlertDialog.Builder builder = PrepareAlerta(titulo, msg);
            builder.SetPositiveButton("OK", delegate { ((Activity)Forms.Context).Finish(); });
            builder.Show();
        }

        private AlertDialog.Builder PrepareAlerta(string titulo, string msg)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Forms.Context);
            builder.SetTitle(titulo);
            builder.SetMessage(msg);
            builder.SetCancelable(false);

            return builder;
        }
    }
}