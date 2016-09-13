using AgendaCorporativa.Contratos;
using AgendaCorporativa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgendaCorporativa.Gerenciadores
{
    public class GerenciadorDeAutorizacao
    {
        private readonly string UrlDoArquivo = "http://www.codeandlions.com/csv_agenda.csv";

        public bool VerificarPermissao()
        {
            var gerenciadorDeContatos = new GerenciadorDeContatos(DependencyService.Get<IGerenciadorDeDownload>());

            List<string> imeisDoArquivo = gerenciadorDeContatos.ObtemImeis();

            string[] listaDeImeiDoAparelho = DependencyService.Get<IGerenciadorDeImei>().ObtemImei();

            bool result = (from imei in listaDeImeiDoAparelho
                           where imeisDoArquivo.Contains(imei)
                           select imei).Count() > 0;

            return result;
        }
    }
}
