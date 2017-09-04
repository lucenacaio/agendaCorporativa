using AgendaCorporativa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    public interface IGerenciadorDeNotificacao
    {
        void AgendaNotificacao(DateTime dateTime, string title, string message);

        void AgendaNotificacaoPeriodica(Periodo periodo, string titulo, string conteudo);
    }

}
