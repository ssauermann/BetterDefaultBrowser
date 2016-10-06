using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using static BetterDefaultBrowser.Lib.OSVersions;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Browser with informationen saved in the registry.
    /// </summary>
    public class Browser
    {
        private String path;

        /// <summary>
        /// Read browser information for a specific key from the registry.
        /// </summary>
        /// <param name="keyName">Unique key as used in the registry path.</param>
        public Browser(String keyName)
        {
            this.KeyName = keyName;
            this.path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\" + keyName;
            if ((Registry.GetValue(path, null, null)) == null)
            {
                throw new ArgumentException("Browser key not existing");
            }
        }
        /// <summary>
        /// Unique key, identifying the browser in registry.
        /// </summary>
        public String KeyName { get; }

        /// <summary>
        /// Programm ID, used to reference installation registry details.
        /// </summary>
        public String ProgId
        {
            get
            {
                //Internet explorer is special :)
                //TODO: MS Edge
                if (KeyName.Equals("IEXPLORE.EXE"))
                {
                    //This is not always true. IE.HTTPS is used for https type.
                    return "IE.HTTP";
                }

                return Registry.GetValue(path + @"\Capabilities\URLAssociations", "http", "NONE").ToString();
            }
        }

        /// <summary>
        /// Full browser name for display.
        /// </summary>
        public String Name
        {
            get
            {
                //Internet explorer is special :)
                if (KeyName.Equals("IEXPLORE.EXE"))
                {
                    return "Internet Explorer";
                }

                return Registry.GetValue(path + @"\Capabilities", "ApplicationName", "NONE").ToString();
            }
        }

        /// <summary>
        /// Path to browser icon. ICO format.
        /// </summary>
        public String IconPath
        {
            get
            {
                return Registry.GetValue(path + @"\DefaultIcon", null, "NONE").ToString();
            }
        }

        //TODO: What if a browser has not the <EXE> <URL> call scheme?
        /// <summary>
        /// Path to the browser executable.
        /// </summary>
        public String ApplicationPath
        {
            get
            {
                return Registry.GetValue(path + @"\shell\open\command", null, "NONE").ToString();
            }
        }

        /// <summary>
        /// Is this browser currently the system default browser?
        /// </summary>
        public bool IsDefault
        {
            get
            {
                //STUB: TODO
                if (KeyName == "FIREFOX.EXE")
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Sets this browser as the system default. DO NOT CALL THIS METHOD ON WINDOWS 8 AND LATER.
        /// </summary>
        public void SetDefault()
        {
            var version = OSVersions.getVersion();
            if (version.HasFlag(OS.VISTA) || version.HasFlag(OS.WIN7))
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", ProgId);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", "ProgId", ProgId);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\ftp\UserChoice", "ProgId", ProgId);
            }
            else if (version.HasFlag(OS.WIN8))
            {
                OSVersions.openBrowserSelectWindow(Name);
            }
            else if (version.HasFlag(OS.WIN10) || version.HasFlag(OS.NEWER))
            {
                var msg = "To set your default browser, go to Settings > System > Default apps.";
                WindowNotification.show(this, msg);
            }else
            {
                throw new ApplicationException("Your OS is not supported. Sorry :(");
            }
        }

        public override string ToString()
        {
            return Name;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////     static below     ////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////

        public static LinkedList<Browser> getInstalledBrowsers()
        {
            return null;
        }

    }
}
