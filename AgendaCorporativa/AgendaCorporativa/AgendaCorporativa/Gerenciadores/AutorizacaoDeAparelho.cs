using AgendaCorporativa.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AgendaCorporativa.Gerenciadores
{
    class AutorizacaoDeAparelho
    {

        public bool VarificarPermissao()
        {
            bool result = false;

            string[] listaDeImeiDoAparelho = DependencyService.Get<IIMEIDoAparelho>().GetImei();
            string[] listaDeImeiPermitidos = new string[1]{"354785070938715"};

            foreach (string imeiUsuario in listaDeImeiDoAparelho)
            {
                if (listaDeImeiPermitidos.Contains(imeiUsuario))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
