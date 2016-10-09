using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;
using static BetterDefaultBrowser.Lib.OSVersions;
using System.ComponentModel;
using RegistryUtils;
using System.Text.RegularExpressions;

namespace BetterDefaultBrowser.Lib
{
    /// <summary> 
    /// Browser with informationen saved in the registry.
    /// </summary>
    public class Browser : INotifyPropertyChanged, IEquatable<Browser>, IComparable<Browser>
    {
        #region Attributes & Constructor

        private String path;
        private bool isDefault;

        /// <summary>
        /// Read browser information for a specific key from the registry.
        /// </summary>
        /// <param name="keyName">Unique key as used in the registry path.</param>
        public Browser(String keyName)
        {
            if (keyName == null || keyName == "")
            {
                throw new ArgumentNullException("Key must not be null or empty.");
            }

            this.KeyName = keyName;
            if (KeyName != "MSEDGE")
            {
                this.path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\" + keyName;
                if ((Registry.GetValue(path, null, null)) == null)
                {
                    throw new ArgumentException("Browser key does not exist!");
                }
            }

            isDefault = this.Equals(AllBrowsers.Default);
        }
        #endregion

        #region Properties
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
                else if (KeyName == "VMWAREHOSTOPEN.EXE")
                {
                    return "VMwareHostOpen.AssocUrl";
                }

                //Edge is even more special, it has no entry in StartMenuInternet.
                //Note: This is arbitary
                if (KeyName == "MSEDGE")
                {
                    return "AppXq0fevzme2pys62n3e0fbqa7peapykr8v";
                    //For https
                    //AppX90nv6nhay5n6a98fnetv7tpk64pp35es
                }


                Trace.WriteLine(KeyName);
                Trace.WriteLine(Registry.GetValue(path + @"\Capabilities\URLAssociations", "http", null));

                var val = Registry.GetValue(path + @"\Capabilities\URLAssociations", "http", null);
                return (val == null) ? "" : val.ToString();
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
                else if (KeyName == "VMWAREHOSTOPEN.EXE")
                {
                    return "VMware Host Open";
                }

                //Edge is even more special, it has no entry in StartMenuInternet.
                //Note: This is arbitary
                if (KeyName == "MSEDGE")
                {
                    return "Edge";
                }

                var val = Registry.GetValue(path + @"\Capabilities", "ApplicationName", null);
                return (val == null) ? "" : val.ToString();
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

                var val = Registry.GetValue(path + @"\DefaultIcon", null, null);
                return (val == null) ? "" : val.ToString();
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
                var val = Registry.GetValue(path + @"\shell\open\command", null, null);

                var ret = (val == null) ? "" : val.ToString();

                if (ret.ElementAt(0) == '"')
                {
                    ret = ret.Substring(1, ret.Length - 2);
                }

                return ret;
            }
        }

        /// <summary>
        /// Is this browser currently the system default browser?
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }
            internal set
            {
                if (isDefault != value)
                {
                    isDefault = value;
                    OnPropertyChanged("IsDefault");
                }
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Sets this browser as the system default. Has to open a window for the user on windows 8 and later.
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
                var msg = "Please select all wanted protocolls and files.";
                WindowNotification.show(this, msg);
            }
            else if (version.HasFlag(OS.WIN10) || version.HasFlag(OS.NEWER))
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Packages\windows.immersivecontrolpanel_cw5n1h2txyewy\LocalState\Indexed\Settings\de-DE\AAA_SettingsPageAppsDefaults.settingcontent-ms");
                var msg = "Please select " + Name + " as your default webbrowser.";
                //var msg = "To set your default browser, go to Settings > System > Default apps.";
                WindowNotification.show(this, msg);
            }
            else
            {
                throw new ApplicationException("Your OS is not supported. Sorry :(");
            }
        }

        #endregion

        #region Event Handler
        /// <summary>
        /// Event handler to react to changes with IsDefault
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Object Methods
        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Browser))
            {
                return false;
            }
            return this.Equals(obj as Browser);
        }

        public override int GetHashCode()
        {
            return KeyName.GetHashCode();
        }

        public bool Equals(Browser other)
        {
            if (other == null)
                return false;
            return other.KeyName == this.KeyName;
        }

        public int CompareTo(Browser other)
        {
            if (other == null)
                return 1;
            return this.Name.CompareTo(other.Name);
        }
        #endregion
    }
}
