using AgendaCorporativa.Modelos;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgendaCorporativa.iOS.IOSExtentions
{
    public static class IOSExtentions
    {
        public static NSCalendarUnit ToNSCalendarUnit(this Periodo periodo)
        {
            NSCalendarUnit iosUnit = NSCalendarUnit.Month;

            switch (periodo)
            {
                case Periodo.Hora:
                    iosUnit = NSCalendarUnit.Hour;
                    break;
                case Periodo.Dia:
                    iosUnit = NSCalendarUnit.Day;
                    break;
                case Periodo.Semana:
                    iosUnit = NSCalendarUnit.Week;
                    break;
                case Periodo.Mes:
                    iosUnit = NSCalendarUnit.Month;
                    break;
                default:
                    break;
            }

            return iosUnit;
        }
    }
}
