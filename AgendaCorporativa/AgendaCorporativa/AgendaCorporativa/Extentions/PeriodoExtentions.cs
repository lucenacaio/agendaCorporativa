using AgendaCorporativa.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaCorporativa.Extentions
{
    public static class PeriodoExtentions
    {
        public static long ToMiliseconds(this Periodo periodo)
        {
            long miliseconds = 0;

            switch (periodo)
            {
                case Periodo.Hora:
                    miliseconds = Convert.ToInt64(new TimeSpan(1, 0, 0).TotalMilliseconds);
                    break;
                case Periodo.Dia:
                    miliseconds = Convert.ToInt64(new TimeSpan(1, 0, 0, 0).TotalMilliseconds);
                    break;
                case Periodo.Semana:
                    miliseconds = Convert.ToInt64(new TimeSpan(7, 0, 0, 0).TotalMilliseconds);
                    break;
                case Periodo.Mes:
                    miliseconds = Convert.ToInt64(new TimeSpan(30, 0, 0, 0).TotalMilliseconds);
                    break;
                default:
                    break;
            }

            return miliseconds;
        }
    }
}
