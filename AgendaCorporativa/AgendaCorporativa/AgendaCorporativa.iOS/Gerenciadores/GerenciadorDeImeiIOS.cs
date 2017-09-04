using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgendaCorporativa.Contratos;
using System.Runtime.InteropServices;
using Foundation;
using Xamarin.Forms;
using AgendaCorporativa.iOS.Gerenciadores;

[assembly: Dependency(typeof(GerenciadorDeImeiIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    /// <summary>
    /// Classe responsavel em tratar o IMEI do aparelho com IOS
    /// </summary>
    public class GerenciadorDeImeiIOS : IGerenciadorDeImei
    {
        #region DLLs import

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern uint IOServiceGetMatchingService(uint masterPort, IntPtr matching);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern IntPtr IOServiceMatching(string s);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern IntPtr IORegistryEntryCreateCFProperty(uint entry, IntPtr key, IntPtr allocator, uint options);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        private static extern int IOObjectRelease(uint o);

        #endregion

        /// <summary>
        /// Obtem os IMEIs do aparelho.
        /// </summary>
        /// <remarks>Caso o aparelho tenha mais de um slot de chip, retorna mais de um EMEI.</remarks>
        /// <returns>Lista de IMEI</returns>
        public string[] ObtemImei()
        {
            string[] serial = new string[1];
            uint platformExpert = IOServiceGetMatchingService(0, IOServiceMatching("IOPlatformExpertDevice"));
            if (platformExpert != 0)
            {
                NSString key = (NSString)"IOPlatformSerialNumber";
                IntPtr serialNumber = IORegistryEntryCreateCFProperty(platformExpert, key.Handle, IntPtr.Zero, 0);
                if (serialNumber != IntPtr.Zero)
                {
                    serial[0] = NSString.FromHandle(serialNumber);
                }

                IOObjectRelease(platformExpert);
            }

            return serial;
        }
    }
}