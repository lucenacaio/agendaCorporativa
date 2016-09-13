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

[assembly: Dependency(typeof(GerenciadorDeImeiDroid))]
namespace AgendaCorporativa.Droid.Gerenciadores
{
    /// <summary>
    /// Classe responsavel pelo tratamento do IMEI no sistema Android
    /// </summary>
    public class GerenciadorDeImeiDroid : IGerenciadorDeImei
    {
        /// <summary>
        /// Obtem o IMEI do aparelho Android
        /// </summary>
        /// <returns>Lista de IMEIs do Android</returns>
        public string[] ObtemImei()
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