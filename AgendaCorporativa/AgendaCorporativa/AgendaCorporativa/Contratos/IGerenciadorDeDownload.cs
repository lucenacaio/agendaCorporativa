using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    public interface IGerenciadorDeDownload
    {
        /// <summary>
        /// Baixa o arquivo e retorna o conteudo do mesmo.
        /// </summary>
        /// <param name="url">Endereço do arquivo</param>
        /// <returns>Conteudo do arquivo</returns>
        Task<string> IniciarDownload(string url);
    }
}
