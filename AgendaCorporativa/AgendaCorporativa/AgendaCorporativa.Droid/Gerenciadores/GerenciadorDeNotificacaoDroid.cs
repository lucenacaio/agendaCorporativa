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
            var ticksSystem = SystemClock.ElapsedRealtime();

            var ticks = dateTime.Ticks - DateTime.Now.Ticks;
            TimeSpan ts = new TimeSpan(ticks);


            //TODO: For demo set after 5 seconds.
            alarmManager.Set(AlarmType.ElapsedRealtime, ticksSystem+Convert.ToInt64(ts.TotalMilliseconds), pendingIntent);

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