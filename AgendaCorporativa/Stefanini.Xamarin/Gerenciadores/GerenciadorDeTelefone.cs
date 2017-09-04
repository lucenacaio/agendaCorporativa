using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Stefanini.Xamarin.Gerenciadores
{
    public static class GerenciadorDeTelefone
    {

        /// <summary>
        /// Abre a tela de chamada do aparelho
        /// </summary>
        /// <param name="ddd">DDD</param>
        /// <param name="numero">Número do telefone sem DDD</param>
        public static void RealizarChamada(string ddd, string numero)
        {
            RealizarChamada(string.Concat(ddd, numero));
        }

        /// <summary>
        /// Abre atela de chamada do aparelho
        /// </summary>
        /// <param name="numero">Número do telefone com DDD</param>
        public static void RealizarChamada(string numero)
        {
            Device.OpenUri(new Uri(String.Format("tel:{0}", numero)));
        }

        /// <summary>
        /// Enviar email
        /// </summary>
        /// <param name="emailDestino">Email do destinatário</param>
        public static void EnviarEmail(string emailDestino)
        {
            Device.OpenUri(new Uri(String.Format("mailto:{0}", emailDestino)));
        }

        /// <summary>
        /// Enviar SMS
        /// </summary>
        /// <param name="numero">Número com DDD</param>
        public static void EnviarSms(string numero)
        {
            Device.OpenUri(new Uri(String.Format("sms:{0}", numero)));
        }

        /// <summary>
        /// Enviar SMS
        /// </summary>
        /// <param name="ddd">Número de DDD</param>
        /// <param name="numero">Número sem DDD</param>
        public static void EnviarSms(string ddd, string numero)
        {
            EnviarSms(string.Concat(ddd, numero));
        }
    }
}
