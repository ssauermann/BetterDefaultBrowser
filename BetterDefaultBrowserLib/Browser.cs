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
                if (KeyName == "IEXPLORE.EXE")
                {
                    return "IE.HTTP";
                    //For https
                    //IE.HTTPS
                }
                
                //Edge is even more special, it has no entry in StartMenuInternet.
                //Note: This is arbitary
                if (KeyName == "MSEDGE")
                {
                    return "AppXq0fevzme2pys62n3e0fbqa7peapykr8v";
                    //For https
                    //AppX90nv6nhay5n6a98fnetv7tpk64pp35es
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

                //Edge is even more special, it has no entry in StartMenuInternet.
                //Note: This is arbitary
                if (KeyName == "MSEDGE")
                {
                    return "Edge";
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
                //Edge is even more special, it has no entry in StartMenuInternet.
                //Note: This is arbitary
                if (KeyName == "MSEDGE")
                {
                    return @"%windir%\SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\MicrosoftEdge.exe,0";
                }

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
                //Edge is even more special, it has no entry in StartMenuInternet.
                //Note: This is arbitary
                if (KeyName == "MSEDGE")
                {
                    //Will open but without url, use Launcher.RunEdge(url) instead.
                    return "microsoft-edge:";
                }
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
                return AllBrowsers.Default.Equals(this);
            }
        }

        /// <summary>
        /// Sets this browser as the system default. Has to open a windows for the user on windows 8 and later.
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

        public override bool Equals(object obj)
        {
            if(!(obj is Browser)){
                return false;
            }
            var other = obj as Browser;
            return other.KeyName == this.KeyName;
        }

        public override int GetHashCode()
        {
            return KeyName.GetHashCode();
        }

    }
}
