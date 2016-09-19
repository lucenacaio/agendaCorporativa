using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    /// <summary>
    /// Gerenciador de armazenamento de arquivos
    /// </summary>
    public interface IGerenciadorDeArquivo
    {
        /// <summary>
        /// Salva o texto em um arquivo
        /// </summary>
        /// <param name="nomeDoArquivo"></param>
        /// <param name="texto"></param>
        void SalvarTexto(string nomeDoArquivo, string texto);

        /// <summary>
        /// Carrega o texto de um arquivo
        /// </summary>
        /// <param name="nomeDoArquivo"></param>
        /// <returns>Conteudo do arquivo</returns>
        string CarregarTexto(string nomeDoArquivo);
        
        /// <summary>
        /// Deleta o arquivo com o nome passado
        /// </summary>
        /// <param name="nomeDoArquivo">Nome do arquivo a ser deletado</param>
        void DeletarArquivo(string nomeDoArquivo);
    }
}
