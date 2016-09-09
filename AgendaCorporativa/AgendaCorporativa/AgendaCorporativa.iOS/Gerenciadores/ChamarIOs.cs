using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using AgendaCorporativa.Contratos;
using Foundation;
using UIKit;
using System.Runtime.CompilerServices;
using AgendaCorporativa.iOS.Gerenciadores;

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
                var av = new UIAlertView("Not supported",
                  "Scheme 'tel:' is not supported on this device",
                  null,
                  "OK",
                  null);
                av.Show();
            };
        }
    }
}