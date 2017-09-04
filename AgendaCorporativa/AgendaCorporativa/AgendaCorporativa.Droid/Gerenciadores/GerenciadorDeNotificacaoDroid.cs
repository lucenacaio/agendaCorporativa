using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AgendaCorporativa.Contratos;
using Xamarin.Forms;
using Android.Support.V4.App;
using Android.Graphics;
using AgendaCorporativa.Droid.Gerenciadores;
using AgendaCorporativa.Modelos;
using AgendaCorporativa.Extentions;

[assembly: Dependency(typeof(GerenciadorDeNotificacaoDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    public class GerenciadorDeNotificacaoDroid : IGerenciadorDeNotificacao
    {
        public void AgendaNotificacao(DateTime dateTime, string titulo, string mensagem)
        {
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("mensagem", mensagem);
            alarmIntent.PutExtra("titulo", titulo);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            var milisegundosDoSistema = SystemClock.ElapsedRealtime();
            
            //pega o espaço de tempo entre hoje e o dia da notificação
            TimeSpan ts = new TimeSpan(dateTime.Ticks - DateTime.Now.Ticks);
            
            alarmManager.Set(AlarmType.ElapsedRealtime, milisegundosDoSistema + Convert.ToInt64(ts.TotalMilliseconds), pendingIntent);

        }

        public void AgendaNotificacaoPeriodica(Periodo periodo, string titulo, string conteudo)
        {
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("titulo", titulo);
            alarmIntent.PutExtra("mensagem", conteudo);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            long intervaloEmMilisegundos = Convert.ToInt64(periodo.ToMiliseconds());
            var dataInicialEmMilisegundos = SystemClock.ElapsedRealtime() + intervaloEmMilisegundos;

            alarmManager.SetRepeating(AlarmType.ElapsedRealtime, dataInicialEmMilisegundos, intervaloEmMilisegundos, pendingIntent);

        }
    }

    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var message = intent.GetStringExtra("mensagem");
            var title = intent.GetStringExtra("titulo");

            var notIntent = new Intent(context, typeof(MainActivity));
            var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            var manager = NotificationManagerCompat.From(context);

            var style = new NotificationCompat.BigTextStyle();
            style.BigText(message);

            var wearableExtender = new NotificationCompat.WearableExtender()
                    .SetBackground(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.icon));

            //Generate a notification with just short text and small icon
            var builder = new NotificationCompat.Builder(context)
                            .SetContentIntent(contentIntent)
                            .SetSmallIcon(Resource.Drawable.icon)
                            .SetContentTitle(title)
                            .SetContentText(message)
                            .SetStyle(style)
                            .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                            .SetAutoCancel(true)
                            .Extend(wearableExtender);

            var notification = builder.Build();
            manager.Notify(0, notification);
        }
    }
}