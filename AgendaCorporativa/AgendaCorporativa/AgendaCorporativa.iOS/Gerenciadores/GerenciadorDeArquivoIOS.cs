using AgendaCorporativa.Contratos;
using AgendaCorporativa.iOS.Gerenciadores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(GerenciadorDeArquivoIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    /// <summary>
    /// Classe responsavel pelo tratamento de arquivos no sistema iOS
    /// </summary>
    public class GerenciadorDeArquivoIOS : IGerenciadorDeArquivo
    {
        /// <summary>
        /// Carrega o conteudo do arquivo. 
        /// Se não existir arquivo retorna vazio.
        /// </summary>
        /// <param name="filename">Nome do arquivo</param>
        /// <returns></returns>
        public string CarregarTexto(string filename)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);

            return File.Exists(filePath) ? File.ReadAllText(filePath) : "";
        }

        public void SalvarTexto(string filename, string text)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }

        /// <summary>
        /// Deleta o arquivo com o nome passado
        /// </summary>
        /// <param name="nomeDoArquivo">Nome do arquivo a ser deletado</param>
        public void DeletarArquivo(string nomeDoArquivo)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, nomeDoArquivo);

            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
