using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    /// <summary>
    /// Interface para acessar o IMEI do aparelho
    /// </summary>
    public interface IGerenciadorDeImei
    {
        /// <summary>
        /// Obtem os IMEIs do aparelho
        /// </summary>
        /// <remarks>O aparelho pode possuir mais de um IMEI(um por entrada de chip)</remarks>
        /// <returns>Lista de IMEIs</returns>
        string[] ObtemImei();
    }
}
