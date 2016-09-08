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
    public class AutorizacaoDeAparelho
    {

        public async Task<bool> VarificarPermissao()
        {
            bool result = false;

            GerenciadorDeContatos gerenciadorDeContatos = new GerenciadorDeContatos(DependencyService.Get<IGerenciadorDeDownload>());
            string[] listaDeImeiDoAparelho = DependencyService.Get<IIMEIDoAparelho>().GetImei();
            List<Contato> contatos = await gerenciadorDeContatos.PesquisaContatos("");

            foreach (string imeiUsuario in listaDeImeiDoAparelho)
            {
                result = contatos.Exists(x => string.Equals(x.IMEI, imeiUsuario));
                if (result)
                    break;
            }

            return result;
        }
    }
}
