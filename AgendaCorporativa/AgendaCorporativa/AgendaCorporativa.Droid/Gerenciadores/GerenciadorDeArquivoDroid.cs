using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgendaCorporativa.Contratos;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Xamarin.Forms;
using AgendaCorporativa.Droid.Gerenciadores;

[assembly: Dependency(typeof(GerenciadorDeArquivoDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    /// <summary>
    /// Classe responsavel pelo tratamento de arquivos no sistema Android
    /// </summary>
    public class GerenciadorDeArquivoDroid : IGerenciadorDeArquivo
    {
        /// <summary>
        /// Carrega o texto do arquivo
        /// </summary>
        /// <param name="nomeDoArquivo">Nome do arquivo</param>
        /// <returns>Conteudo do arquivo</returns>
        public string CarregarTexto(string nomeDoArquivo)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, nomeDoArquivo);
            return File.Exists(filePath) ? File.ReadAllText(filePath) : "";
        }

        /// <summary>
        /// Salva o texto em um arquivo
        /// </summary>
        /// <param name="nomeDoArquivo">Nome do arquivo</param>
        /// <param name="texto">Conteudo do arquivo</param>
        public void SalvarTexto(string nomeDoArquivo, string texto)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, nomeDoArquivo);
            File.WriteAllText(filePath, texto);
        }

        /// <summary>
        /// Deleta o arquivo com o nome passado
        /// </summary>
        /// <param name="nomeDoArquivo">Nome do arquivo a ser deletado</param>
        public void DeletarArquivo(string nomeDoArquivo)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, nomeDoArquivo);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}