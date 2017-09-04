using AgendaCorporativa.Contratos;
using AgendaCorporativa.iOS.Gerenciadores;
using AgendaCorporativa.iOS.IOSExtentions;
using AgendaCorporativa.Modelos;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(GerenciadorDeNotificacaoIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    public class GerenciadorDeNotificacaoIOS : IGerenciadorDeNotificacao
    {
        public void AgendaNotificacao(DateTime dateTime, string title, string message)
        {
            // Cria a notificação
            var notification = new UILocalNotification();

            var ticks = dateTime.Ticks - DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(ticks);

            // Essa notificação é na data.
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(ts.TotalSeconds);

            // Configura a notificacao(Alerta)
            notification.AlertAction = title;
            notification.AlertBody = message;
            notification.AlertTitle = title;

            // Altera o numero da badge
            notification.ApplicationIconBadgeNumber = 1;

            // Seta o som 
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }

        public void AgendaNotificacaoPeriodica(Periodo periodo, string titulo, string conteudo)
        {
            // Cria a notificação
            var notification = new UILocalNotification();
            
            // Periodo em que vai avisar..
            notification.RepeatInterval = periodo.ToNSCalendarUnit();

            // Configura a notificacao(Alerta)
            notification.AlertAction = titulo;
            notification.AlertBody = conteudo;
            notification.AlertTitle = titulo;

            // Altera o numero da badge
            notification.ApplicationIconBadgeNumber = 1;

            // Seta o som 
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }
    }
}
