using AgendaCorporativa.Contratos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.iOS.Gerenciadores
{
    public class GerenciadorDeDownloadIOS : IGerenciadorDeDownload
    {
        public async Task<string> IniciarDownload(string url)
        {
            var webClient = new WebClient();

            webClient.Encoding = Encoding.UTF8;
            return await webClient.DownloadStringTaskAsync(new Uri(url));
        }
    }
}
