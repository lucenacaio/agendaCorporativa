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
using Java.Lang;
using Java.Lang.Reflect;

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

            List<string> imeis = new List<string>();

            BuildVersionCodes version = Build.VERSION.SdkInt;

            //Na versão 22 pra cima, possuem o metodo getDeviceId, que possibilita pegar mais de um IMEI.
            if (version >= BuildVersionCodes.LollipopMr1)
            {
                Class telephonyManagerClass = Class.ForName(telephonyManager.Class.Name);
                Class[] parameter = new Class[1];
                parameter[0] = Integer.Type;

                Method DeviceId = telephonyManagerClass.GetMethod("getDeviceId", parameter);

                Java.Lang.Object[] parametro = new Java.Lang.Object[1];

                for (int i = 0; i < 4; i++)
                {
                    parametro[0] = i;
                    var imei = DeviceId.Invoke(telephonyManager, parametro);
                    if (!imeis.Contains(imei.ToString()))
                        imeis.Add(imei.ToString());
                }
                imeis.Add("355954043656585");
            }
            else
            {
                //Obtem ID
                imeis.Add(telephonyManager.DeviceId);
            }
            return imeis.ToArray();
        }
    }
}