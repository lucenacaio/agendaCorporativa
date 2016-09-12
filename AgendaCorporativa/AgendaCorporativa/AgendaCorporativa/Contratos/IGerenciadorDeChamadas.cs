using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    /// <summary>
    /// Gerenciador de chamadas telefônicas
    /// </summary>
    public interface IGerenciadorDeChamadas
    {
        /// <summary>
        /// Faz a chamada telefonica para o numero.
        /// </summary>
        /// <param name="numero">Numero para fazer a chamada</param>
        void ChamarNumero(string numero);
    }
}
