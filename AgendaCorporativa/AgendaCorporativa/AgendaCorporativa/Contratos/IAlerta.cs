using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Contratos
{
    public interface IAlerta
    {
        void AlertaDialogAndCloseApp(string titulo, string msg);

        void AlertaDialog(string titulo, string msg);
    }
}
