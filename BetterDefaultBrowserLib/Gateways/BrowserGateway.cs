using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using RegistryUtils;

namespace BetterDefaultBrowser.Lib.Gateways
{
    using Models;
    using static Helpers.OSVersions;

    /// <summary>
    /// Gateway to load all browser information from the registry.
    /// </summary>
    public class BrowserGateway : NotifyPropertyChangedBase, IBrowserGateway
    {
        #region Members
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static BrowserGateway _instance;

        /// <summary>
        /// Context for inter thread communication.
        /// </summary>
        private static readonly SynchronizationContext SynchronizationContext;

        /// <summary>
        /// List of installed browsers
        /// </summary>
        private readonly List<Browser> _installedBrowsers = new List<Browser>();

        /// <summary>
        /// Current system default browser
        /// </summary>
        private Browser _defaultBrowser;
        #endregion

        #region Static constructor
        /// <summary>
        /// Initializes static members of the <see cref="BrowserGateway" /> class.
        /// </summary>
        static BrowserGateway()
        {
            // Registry Monitors
            RegistryMonitor defaultBrowserMonitor = new RegistryMonitor(RegistryHive.CurrentUser, @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice");
            RegistryMonitor installedBrowsersMonitor = new RegistryMonitor(RegistryHive.LocalMachine, @"SOFTWARE\Clients\StartMenuInternet");

            defaultBrowserMonitor.RegChanged += OnDefaultBrowserChanged;
            installedBrowsersMonitor.RegChanged += OnInstalledBrowsersChanged;

            defaultBrowserMonitor.Start();
            installedBrowsersMonitor.Start();

            // Context for main thread
            SynchronizationContext = SynchronizationContext.Current;

            // Initial loading
            Instance.LoadBrowsers();
            Instance.LoadDefault();
        }
        #endregion

        #region Singleton Instance
        /// <summary>
        /// Prevents a default instance of the <see cref="BrowserGateway" /> class from being created. Ensures the singleton.
        /// </summary>
        private BrowserGateway()
        {
        }

        /// <summary>
        /// Gets the gateway instance.
        /// </summary>
        public static BrowserGateway Instance => _instance ?? (_instance = new BrowserGateway());

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the default browser.
        /// <para>When setting the default browser, any browser that is not part of the installed browsers will throw an exception.</para>
        /// </summary>
        public Browser DefaultBrowser
        {
            get
            {
                return _defaultBrowser;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Can not unset the default browser.");
                }

                if (!value.Equals(_defaultBrowser))
                {
                    SetDefaultBrowser(value);
                }
            }
        }

        /// <summary>
        /// Gets a list of the installed browsers in alphabetical order.
        /// </summary>
        public List<Browser> InstalledBrowsers => _installedBrowsers;

        #endregion Properties

        #region Methods
        /// <summary>
        /// Gets a specific installed browser by key.
        /// </summary>
        /// <param name="key">Browser key</param>
        /// <returns>Browser or null</returns>
        public Browser GetBrowser(string key)
        {
            return InstalledBrowsers.Find(b => b.Key == key);
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Loads the values for a specific browser referenced by its id from the registry and creates a browser object.
        /// </summary>
        /// <param name="key">Browser key (Must exist)</param>
        /// <returns>Browser object</returns>
        private static Browser CreateBrowser(string key)
        {
            Browser retBrowser;

            // Load values from registry
            var name = LoadBrowserValue(key, @"\Capabilities", "ApplicationName");
            var iconPath = LoadBrowserValue(key, @"\DefaultIcon", null);
            var applicationPath = LoadBrowserValue(key, @"\shell\open\command", null);
            var progId = LoadBrowserValue(key, @"\Capabilities\URLAssociations", "http");

            // Try to find a special browser entry.
            Browser specialBrowser;
            if (SpecialBrowsers.Map.TryGetValue(key, out specialBrowser))
            {
                // Use value if not null or dictionary value
                retBrowser = new Browser(key)
                {
                    Name = name ?? specialBrowser.Name,
                    IconPath = iconPath ?? specialBrowser.IconPath,
                    ApplicationPath = applicationPath ?? specialBrowser.ApplicationPath,
                    ProgId = progId ?? specialBrowser.ProgId
                };
            }
            else
            {
                // Use value directly
                retBrowser = new Browser(key)
                {
                    Name = name,
                    IconPath = iconPath,
                    ApplicationPath = applicationPath,
                    ProgId = progId
                };
            }

            // Check if all values are set. If not log this incident and throw exception.
            // (This browser should be reported to us and added to the special browser dictionary to support it correctly.)
            if (retBrowser.Name == null || retBrowser.IconPath == null || retBrowser.ProgId == null || retBrowser.ApplicationPath == null || retBrowser.Key == null)
            {
                Trace.WriteLine("Unsupported browser detected. Please report it to us so we can add support for this browser.");
                Trace.WriteLine("Please send us the following data:");
                Trace.WriteLine(retBrowser.ToString());
                throw new ApplicationException("Browser data missing!");
            }

            // All set, return browser
            return retBrowser;
        }

        /// <summary>
        /// Loads a browser value from the registry.
        /// </summary>
        /// <param name="browserKey">Browser key</param>
        /// <param name="registryPath">Path in registry where the information is located. Has to start with a slash.</param>
        /// <param name="registryValue">Value key to get</param>
        /// <returns>Value or null if not found.</returns>
        private static string LoadBrowserValue(string browserKey, string registryPath, string registryValue)
        {
            var path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\" + browserKey;
            var val = Registry.GetValue(path + registryPath, registryValue, null);

            // If value is set, return it else null
            return val?.ToString();
        }

        /// <summary>
        /// Sets the default browser or opens a user window to do so.
        /// </summary>
        /// <param name="newDefault">New default browser</param>
        private static void SetDefaultBrowser(Browser newDefault)
        {
            var version = GetVersion();
            if (version.HasFlag(OS.VISTA) || version.HasFlag(OS.WIN7))
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", newDefault.ProgId);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", "ProgId", newDefault.ProgId);
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\ftp\UserChoice", "ProgId", newDefault.ProgId);
            }
            else if (version.HasFlag(OS.WIN8))
            {
                OpenBrowserSelectWindow(newDefault.Name);
            }
            else if (version.HasFlag(OS.WIN10) || version.HasFlag(OS.NEWER))
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\AppData\Local\Packages\windows.immersivecontrolpanel_cw5n1h2txyewy\LocalState\Indexed\Settings\de-DE\AAA_SettingsPageAppsDefaults.settingcontent-ms");
            }
            else
            {
                throw new ApplicationException("Your OS is not supported. Sorry :(");
            }
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Event handler for default browser changes.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private static void OnDefaultBrowserChanged(object sender, EventArgs e)
        {
            SynchronizationContext.Post(
            delegate
            {
                Instance.LoadDefault();
            },
            null);
        }

        /// <summary>
        /// Event handler for installed browser changes.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event arguments</param>
        private static void OnInstalledBrowsersChanged(object sender, EventArgs e)
        {
            SynchronizationContext.Post(
            delegate
            {
                Instance.LoadBrowsers();
            },
            null);
        }
        #endregion

        #region Loaders
        /// <summary>
        /// Loads all installed browsers from the registry.
        /// </summary>
        private void LoadBrowsers()
        {
            // Clear browser list
            _installedBrowsers.Clear();

            // Open browser registry
            using (var keysReg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet"))
            {
                if (keysReg == null)
                {
                    throw new KeyNotFoundException(@"Registry key not found: HKLM\SOFTWARE\Clients\StartMenuInternet");
                }

                // Get key names of all installed browsers
                var keys = keysReg.GetSubKeyNames();

                // Add each browser into the list
                foreach (var key in keys)
                {
                    try
                    {
                        _installedBrowsers.Add(CreateBrowser(key));
                    }
                    catch (ApplicationException)
                    {
                        // Ignore invalid browser and do not add to list.
                    }
                }

                // MS Edge is not listed in this registry key
                // Only add if OS is Windows10 or newer
                var version = GetVersion();
                if (version.HasFlag(OS.WIN10) || version.HasFlag(OS.NEWER))
                {
                    _installedBrowsers.Add(SpecialBrowsers.Map["MSEDGE"]);
                }
            }

            // Trigger change event
            RaisePropertyChanged("InstalledBrowsers");
        }

        /// <summary>
        /// Loads the default browser from the registry. Should be called after <see cref="LoadBrowsers"/>.
        /// </summary>
        private void LoadDefault()
        {
            var regValue = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", null);

            // System currently has no default browser -> null will stay.
            // Else: Loop below will find a matching browser and set it.
            _defaultBrowser = null;

            if (regValue != null)
            {
                // Set default browser
                var id = regValue.ToString();

                foreach (Browser b in InstalledBrowsers)
                {
                    if (b.ProgId == id)
                    {
                        _defaultBrowser = b;
                        break;
                    }
                }
            }

            // Trigger change event
            RaisePropertyChanged("DefaultBrowser");
        }
        #endregion
    }
}
