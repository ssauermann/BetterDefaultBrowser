using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    public static class AllBrowsers
    {
        public static bool IsBrowserInstalled(String name)
        {
            foreach (var b in InstalledBrowsers)
            {
                if (b.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Default browser as set in registry.
        /// </summary>
        public static Browser Default
        {
            get
            {
                var id = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", "NONE").ToString();

                foreach (Browser b in InstalledBrowsers)
                {
                    if (b.ProgId.Equals(id))
                    {
                        return b;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// List of all installed browsers.
        /// </summary>
        public static LinkedList<Browser> InstalledBrowsers
        {
            get
            {
                LinkedList<Browser> browsers = new LinkedList<Browser>();

                var keys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet").GetSubKeyNames();
                foreach (var name in keys)
                {
                    browsers.AddLast(new Browser(name));
                }

                //MS Edge is special :(
                var version = OSVersions.getVersion();
                if (version.HasFlag(OSVersions.OS.WIN10) || version.HasFlag(OSVersions.OS.NEWER)){
                    browsers.AddLast(new Browser("MSEDGE"));
                }

                return browsers;
            }
        }

        public static bool IsBDBInstalled
        {
            get
            {
                return IsBrowserInstalled(HardcodedValues.APP_NAME);
            }
        }

        public static bool IsBDBDefault
        {
            get
            {
                return Default.KeyName == HardcodedValues.APP_NAME;
            }
        }
    }
}
