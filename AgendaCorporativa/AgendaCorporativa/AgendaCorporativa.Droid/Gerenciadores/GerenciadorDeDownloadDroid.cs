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
using AgendaCorporativa.Droid.Gerenciadores;
using Xamarin.Forms;

[assembly: Dependency(typeof(GerenciadorDeDownloadDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    public class GerenciadorDeDownloadDroid : IGerenciadorDeDownload
    {
        /// <summary>
        /// Classe responsavel pelo tratamento de downloads no sistema Android
        /// </summary>
        public async Task<string> BaixaConteudoArquivo(string url)
        {
            var webClient = new WebClient();

            webClient.Encoding = Encoding.UTF8;
            return await webClient.DownloadStringTaskAsync(new Uri(url));
        }
    }
}