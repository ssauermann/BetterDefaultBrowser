using Microsoft.Win32;
using RegistryUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace BetterDefaultBrowser.Lib
{
    public static class AllBrowsers
    {
        private static BindingList<Browser> browsers = new BindingList<Browser>();
        private static List<Browser> browserList = new List<Browser>();
        private static Browser @default;

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
                return @default;
            }
        }

        /// <summary>
        /// List of all installed browsers.
        /// </summary>
        public static BindingList<Browser> InstalledBrowsers
        {
            get
            {
                return browsers;
            }
        }

        #region Value Loaders
        private static void LoadDefault()
        {
            var idK = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", null);

            if (idK != null)
            {
                var id = idK.ToString();

                foreach (Browser b in InstalledBrowsers)
                    if (b.ProgId.Equals(id))
                    {
                        AllBrowsers.@default = b;
                        break;
                    }
            }
            else
                AllBrowsers.@default = null;

            //Set default for all browsers:
            foreach (var b in InstalledBrowsers)
            {
                if (b.Equals(AllBrowsers.Default))
                    b.IsDefault = true;
                else
                    b.IsDefault = false;
            }
        }



        private static void LoadBrowsers()
        {
            browsers.Clear();
            AllBrowsers.browserList.Clear();
            var keys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet").GetSubKeyNames();
            foreach (var name in keys)
            {
                AllBrowsers.browserList.Add(new Browser(name));
            }

            //MS Edge is special :(
            var version = OSVersions.getVersion();
            if (version.HasFlag(OSVersions.OS.WIN10) || version.HasFlag(OSVersions.OS.NEWER))
            {
                AllBrowsers.browserList.Add(new Browser("MSEDGE"));
            }

            //Sort
            AllBrowsers.browserList.Sort();
            
            //Add the browsers to the actual bindingList
            foreach(var browser in AllBrowsers.browserList)
            {
                browsers.Add(browser);
            }

            BDBInstalled.Instance.IsBDBInstalled = IsBrowserInstalled(HardcodedValues.APP_NAME);
        }


        #endregion

        #region Convenience Methods

        public static BDBInstalled IsBDBInstalled
        {
            get
            {
                return BDBInstalled.Instance;
            }
        }

        public static bool IsBDBDefault
        {
            get
            {
                return Default.KeyName == HardcodedValues.APP_NAME;
            }
        }
        #endregion

        #region Event Handler
        private static SynchronizationContext synchronizationContext;
        static AllBrowsers()
        {
            //Registry Monitors
            RegistryMonitor defaultBrowserMonitor = new RegistryMonitor(RegistryHive.CurrentUser, @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            RegistryMonitor installedBrowsersMonitor = new RegistryMonitor(RegistryHive.LocalMachine, @"SOFTWARE\Clients\StartMenuInternet");

            defaultBrowserMonitor.RegChanged += new EventHandler(OnDefaultBrowserChanged);
            installedBrowsersMonitor.RegChanged += new EventHandler(OnInstalledBrowsersChanged);

            defaultBrowserMonitor.Start();
            installedBrowsersMonitor.Start();

            //TODO: When to stop? Do they have to?

            //Context for main thread
            synchronizationContext = SynchronizationContext.Current;

            //Initial loading
            LoadBrowsers();
            LoadDefault();        
        }

        private static void OnDefaultBrowserChanged(object sender, EventArgs e)
        {
            synchronizationContext.Post(delegate
            {
                LoadDefault();
            }, null);
        }
        private static void OnInstalledBrowsersChanged(object sender, EventArgs e)
        {
            synchronizationContext.Post(delegate
            {
                LoadBrowsers();
            }, null);
        }

        #endregion


        #region Subclasses
        public class BDBInstalled : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private bool isBDBInstalled=false;
            public bool IsBDBInstalled
            {
                get
                {
                    return isBDBInstalled;
                }
                set
                {
                    isBDBInstalled = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsBDBInstalled"));
                }
            }

            public static BDBInstalled Instance { get; private set; }
            protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, e);
            }

            static BDBInstalled()
            {
                Instance = new BDBInstalled();
            }
        }
        #endregion
    }
}
