using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Containing static helper methods to validate the OS Version.
    /// </summary>
    public static class OSVersions
    {
        [Flags]
        public enum OS
        {
            INVALID = 1 << 0,
            OUTDATED = 1 << 1,
            VISTA = 1 << 2,
            WIN7 = 1 << 3,
            WIN8 = 1 << 4,
            WIN10 = 1 << 5,
            NEWER = 1 << 6
        }

        public static OS getVersion()
        {
            int major = System.Environment.OSVersion.Version.Major;
            int minor = System.Environment.OSVersion.Version.Minor;

            if (major <= 5)
                return OS.OUTDATED;
            else if (major == 6 && minor == 0)
                return OS.VISTA;
            else if (major == 6 && minor == 1)
                return OS.WIN7;
            else if (major == 6 && minor == 2)
                return OS.WIN8;
            else if (major == 10 && minor == 0)
                return OS.WIN10;
            else if (major > 10)
                return OS.NEWER;

            return OS.INVALID;
        }

        /// <summary>
        /// Opens the set default window for Vista to Win8.1
        /// </summary>
        public static void openBrowserSelectWindow(String appName)
        {
            IApplicationAssociationRegistrationUI app = (IApplicationAssociationRegistrationUI)new ApplicationAssociationRegistrationUI();
            int hr = app.LaunchAdvancedAssociationUI(appName);
            Exception error = Marshal.GetExceptionForHR(hr);
            if(error!=null)
            {
                throw error;
            }
        }

        //http://stackoverflow.com/questions/29847034/how-to-show-set-program-associations-window-in-windows-8-8-1

        [Guid("1f76a169-f994-40ac-8fc8-0959e8874710")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IApplicationAssociationRegistrationUI
        {
            [PreserveSig]
            int LaunchAdvancedAssociationUI([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegName);
        }

        [ComImport]
        [Guid("1968106d-f3b5-44cf-890e-116fcb9ecef1")]
        public class ApplicationAssociationRegistrationUI
        {
        }

    }
}
