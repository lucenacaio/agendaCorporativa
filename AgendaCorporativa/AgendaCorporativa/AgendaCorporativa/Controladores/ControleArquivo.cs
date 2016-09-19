using AgendaCorporativa.Contratos;
using AgendaCorporativa.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgendaCorporativa.Controladores
{
    public static class ControleArquivo
    {
        /// <summary>
        /// Baixa arquivo mas não salva
        /// </summary>
        /// <returns></returns>
        public static async Task<string> BaixarArquivoTemp()
        {
            return await BaixarArquivo(false);
        }

        /// <summary>
        /// Baixa e salva o arquivo
        /// </summary>
        /// <returns></returns>
        public static async Task<string> BaixareSalvarArquivo()
        {
            return await BaixarArquivo(true);
        }

        private static async Task<string> BaixarArquivo(bool salvar)
        {
            var gerenciadorArquivo = DependencyService.Get<IGerenciadorDeArquivo>();
            var gerenciadorDownload = DependencyService.Get<IGerenciadorDeDownload>();

            var conteudo = await gerenciadorDownload.BaixaConteudoArquivo(Resources.UrlDoArquivo);


            if (salvar & !string.IsNullOrWhiteSpace(conteudo ?? ""))
                gerenciadorArquivo.SalvarTexto(Resources.NomeArquivoLocal, conteudo);

            return conteudo;
        }

        /// <summary>
        /// Retorna texto/conteudo do arquivo
        /// </summary>
        /// <returns></returns>
        public static string LerArquivo()
        {
            var gerenciadorArquivo = DependencyService.Get<IGerenciadorDeArquivo>();
            return gerenciadorArquivo.CarregarTexto(Resources.NomeArquivoLocal);
        }

        /// <summary>
        /// Deleta arquivo
        /// </summary>
        public static void DeletarArquivo()
        {
            var gerenciadorArquivo = DependencyService.Get<IGerenciadorDeArquivo>();
            gerenciadorArquivo.DeletarArquivo(Resources.NomeArquivoLocal);
        }
    }
}
