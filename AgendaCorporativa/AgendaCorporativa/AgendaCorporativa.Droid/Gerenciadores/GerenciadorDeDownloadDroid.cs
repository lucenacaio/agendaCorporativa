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
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace AgendaCorporativa.Droid.Gerenciadores
{
    public class GerenciadorDeDownloadDroid : IGerenciadorDeDownload
    {
        public async Task<string> IniciarDownload()
        {
            var webClient = new WebClient();

            webClient.Encoding = Encoding.UTF8;
            return await webClient.DownloadStringTaskAsync(new Uri("http://www.codeandlions.com/teste.csv"));
        }
    }
}