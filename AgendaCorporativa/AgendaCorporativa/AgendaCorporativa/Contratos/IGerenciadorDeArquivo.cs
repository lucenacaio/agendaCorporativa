using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    public interface IGerenciadorDeArquivo
    {
            void SalvarTexto(string filename, string text);

            string CarregarTexto(string filename);
    }
}
