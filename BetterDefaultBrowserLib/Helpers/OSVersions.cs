using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace BetterDefaultBrowser.Lib.Helpers
{
    /// <summary>
    /// Containing static helper methods to validate the OS Version.
    /// </summary>
    public static class OSVersions
    {
        /// <summary>
        /// Selection of windows OS versions.
        /// </summary>
        [Flags]
        public enum OS
        {
            /// <summary>
            /// Invalid value was returned
            /// </summary>
            INVALID = 1 << 0,

            /// <summary>
            /// Windows XP or older
            /// </summary>
            OUTDATED = 1 << 1,

            /// <summary>
            /// Windows Vista
            /// </summary>
            VISTA = 1 << 2,

            /// <summary>
            /// Windows 7
            /// </summary>
            WIN7 = 1 << 3,

            /// <summary>
            /// Windows 8 or 8.1
            /// </summary>
            WIN8 = 1 << 4,

            /// <summary>
            /// Windows 10
            /// </summary>
            WIN10 = 1 << 5,

            /// <summary>
            /// Windows 11 3/4 or whatever microsoft did to increase the internal version number.
            /// </summary>
            NEWER = 1 << 6
        }

        /// <summary>
        /// Get the currently running windows OS version.
        /// </summary>
        /// <returns>Windows version</returns>
        public static OS GetVersion()
        {
            int major = System.Environment.OSVersion.Version.Major;
            int minor = System.Environment.OSVersion.Version.Minor;

            // Win10 has sometimes wrong values
            var tenVers = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMajorVersionNumber", null);
            if (tenVers != null && tenVers.ToString() == "10")
            {
                return OS.WIN10;
            }

            if (major <= 5)
            {
                return OS.OUTDATED;
            }
            else if (major == 6 && minor == 0)
            {
                return OS.VISTA;
            }
            else if (major == 6 && minor == 1)
            {
                return OS.WIN7;
            }
            else if (major == 6 && minor == 2)
            {
                return OS.WIN8;
            }
            else if (major == 10 && minor == 0)
            {
                return OS.WIN10;
            }
            else if (major > 10)
            {
                return OS.NEWER;
            }

            return OS.INVALID;
        }

        /// <summary>
        /// Opens the 'set default' window for Vista to Win8.1 for a specific application.
        /// </summary>
        /// <param name="appName">Application name</param>
        public static void OpenBrowserSelectWindow(string appName)
        {
            IApplicationAssociationRegistrationUI app = (IApplicationAssociationRegistrationUI)new ApplicationAssociationRegistrationUI();
            int hr = app.LaunchAdvancedAssociationUI(appName);
            Exception error = Marshal.GetExceptionForHR(hr);
            if (error != null)
            {
                throw error;
            }
        }

        //// http://stackoverflow.com/questions/29847034/how-to-show-set-program-associations-window-in-windows-8-8-1

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "*", Justification = "Copy from stackoverflow")]
        [Guid("1f76a169-f994-40ac-8fc8-0959e8874710")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IApplicationAssociationRegistrationUI
        {
            [PreserveSig]
            int LaunchAdvancedAssociationUI([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegName);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "*", Justification = "Copy from stackoverflow")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.Ordering", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Copy from stackoverflow")]
        [ComImport]
        [Guid("1968106d-f3b5-44cf-890e-116fcb9ecef1")]
        private class ApplicationAssociationRegistrationUI
        {
        }
    }
}
