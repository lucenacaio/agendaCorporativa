using AgendaCorporativa.Contratos;
using AgendaCorporativa.Excecoes;
using AgendaCorporativa.Gerenciadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgendaCorporativa.Controladores
{
    public static class ControleDeAutorizacao
    {
        /// <summary>
        /// Verifica autorização de aparelho para usar o aplicativo
        /// </summary>
        /// <exception cref="ExcecaoDeAutenticacao">Erro de autenticação</exception>
        public static void Autorizar()
        {
            var gerenciadorDeContatos = new GerenciadorDeContatos(DependencyService.Get<IGerenciadorDeDownload>());

            var imeisDoArquivo = gerenciadorDeContatos.ObtemImeis();

            var listaDeImeiDoAparelho = DependencyService.Get<IGerenciadorDeImei>().ObtemImei();

            bool result = (from imei in listaDeImeiDoAparelho
                           where imeisDoArquivo.Contains(imei)
                           select imei).Count() > 0;

            if(!result)
                throw new ExcecaoDeAutenticacao("Aparelho não autorizado.\n Imei: " + listaDeImeiDoAparelho[0]);
        }
    }
}
