using AgendaCorporativa.Contratos;
using AgendaCorporativa.iOS.Gerenciadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertaDialogIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    public class AlertaDialogIOS : IAlerta
    {
        public void AlertaDialog(string titulo, string msg)
        {
            PrepareAlerta(titulo, msg);
        }

        public void AlertaDialogAndCloseApp(string titulo, string msg)
        {
            UIAlertView alert = PrepareAlerta(titulo, msg);

            alert.Clicked += Alert_Clicked;
        }

        private UIAlertView PrepareAlerta(string titulo, string msg)
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = titulo,
                Message = msg
            };
            alert.AddButton("OK");
            alert.Show();

            return alert;
        }

        private void Alert_Clicked(object sender, UIButtonEventArgs e)
        {
            Thread.CurrentThread.Abort();
        }
    }
}