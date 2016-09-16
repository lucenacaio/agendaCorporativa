using AgendaCorporativa.Contratos;
using AgendaCorporativa.iOS.Gerenciadores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(GerenciadorDeDownloadIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    /// <summary>
    /// Classe responsavel pelo tratamento de downloads no sistema iOS
    /// </summary>
    public class GerenciadorDeDownloadIOS : IGerenciadorDeDownload
    {
        /// <summary>
        /// Inicia o download no aparelho iOS
        /// </summary>
        /// <param name="url">Endereco do arquivo</param>
        public async Task<string> BaixaConteudoArquivo(string url)
        {
            var webClient = new WebClient();

            webClient.Encoding = Encoding.UTF8;
            return await webClient.DownloadStringTaskAsync(new Uri(url));
        }
    }
}
