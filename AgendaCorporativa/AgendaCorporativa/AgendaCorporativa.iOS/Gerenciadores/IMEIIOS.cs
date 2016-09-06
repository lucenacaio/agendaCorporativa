using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AgendaCorporativa.Contratos;
using System.Runtime.InteropServices;
using Foundation;
using Xamarin.Forms;
using AgendaCorporativa.iOS.Gerenciadores;

[assembly: Dependency(typeof(IMEIIOS))]
namespace AgendaCorporativa.iOS.Gerenciadores
{
    class IMEIIOS : IIMEIDoAparelho
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

        public string GetImei()
        {
            string serial = string.Empty;
            uint platformExpert = IOServiceGetMatchingService(0, IOServiceMatching("IOPlatformExpertDevice"));
            if (platformExpert != 0)
            {
                NSString key = (NSString)"IOPlatformSerialNumber";
                IntPtr serialNumber = IORegistryEntryCreateCFProperty(platformExpert, key.Handle, IntPtr.Zero, 0);
                if (serialNumber != IntPtr.Zero)
                {
                    serial = NSString.FromHandle(serialNumber);
                }

                IOObjectRelease(platformExpert);
            }

            return serial;
        }
    }
}