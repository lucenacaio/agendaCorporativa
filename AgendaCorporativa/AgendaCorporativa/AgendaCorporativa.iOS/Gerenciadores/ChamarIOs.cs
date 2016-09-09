using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgendaCorporativa.Contratos;
using Foundation;
using UIKit;
using AgendaCorporativa.iOS.Gerenciadores;
using Xamarin.Forms;

[assembly: Dependency(typeof(ChamarIOs))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    class ChamarIOs : IChamar
    {
        public void ChamarNumero(string numero)
        {
            var url = new NSUrl("tel:" + numero);
            UIApplication.SharedApplication.OpenUrl(url);
            if (!UIApplication.SharedApplication.OpenUrl(url))
            {
                var av = new UIAlertView("N�o � suportado",
                  "Esse numero n�o � suportado nesse aparelho",
                  null,
                  "OK",
                  null);
                av.Show();
            };
        }
    }
}