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
    public class GerenciadorDeArquivoDroid : IGerenciadorDeArquivo
    {
        public string CarregarTexto(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            return File.ReadAllText(filePath);
        }

        public void SalvarTexto(string filename, string text)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            File.WriteAllText(filePath, text);
        }
    }
}