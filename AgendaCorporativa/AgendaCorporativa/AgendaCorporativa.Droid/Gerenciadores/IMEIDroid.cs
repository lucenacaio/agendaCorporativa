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
using Android.Telephony;
using AgendaCorporativa.Droid.Gerenciadores;

[assembly: Dependency(typeof(IMEIDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    class IMEIDroid : IIMEIDoAparelho
    {
        public string[] GetImei()
        {
            var telephonyManager = (TelephonyManager)Forms.Context.GetSystemService(Context.TelephonyService);

            int? qtySims = telephonyManager?.PhoneCount;
            string[] imeis = new string[qtySims ?? 0];

            for (int i = 0; i < qtySims;)
            {
                imeis[i] = telephonyManager?.GetDeviceId(i++);
            }

            return imeis;
        }
    }
}