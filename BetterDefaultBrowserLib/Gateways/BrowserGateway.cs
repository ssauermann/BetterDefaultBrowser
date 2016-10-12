using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Win32;

namespace BetterDefaultBrowser.Lib.Gateways
{
    using Models;

    /// <summary>
    /// Gateway to load all browser information from the registry.
    /// </summary>
    public class BrowserGateway : NotifyPropertyChangedBase, IBrowserGateway
    {
        #region Members
        /// <summary>
        /// Singleton instance
        /// </summary>
        private static BrowserGateway instance;

        /// <summary>
        /// Dictionary for storing data which can't be found at the normal location in the registry.
        /// </summary>
        private static Dictionary<string, Browser> specialBrowsers = new Dictionary<string, Browser>();

        /// <summary>
        /// List of installed browsers
        /// </summary>
        private readonly List<Browser> installedBrowsers = new List<Browser>();

        /// <summary>
        /// Current system default browser
        /// </summary>
        private Browser defaultBrowser;
        #endregion

        #region Special Browsers
        /// <summary>
        /// Initializes static members of the <see cref="BrowserGateway" /> class.
        /// </summary>
        static BrowserGateway()
        {
            specialBrowsers["MSEDGE"] = new Browser("MSEDGE")
            {
                Name = "Microsoft Edge",
                ProgId = "AppXq0fevzme2pys62n3e0fbqa7peapykr8v",
                IconPath = @"%windir%\SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\MicrosoftEdge.exe,0",
                ApplicationPath = "microsoft-edge:"
            };

            specialBrowsers["IEXPLORE.EXE"] = new Browser("IEXPLORE.EXE")
            {
                Name = "Internet Explorer",
                ProgId = "IE.HTTP"
            };

            specialBrowsers["VMWAREHOSTOPEN.EXE"] = new Browser("VMWAREHOSTOPEN.EXE")
            {
                Name = "VMware Host Open",
                ProgId = "VMwareHostOpen.AssocUrl"
            };
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
        public static BrowserGateway Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BrowserGateway();
                }

                return instance;
            }
        }
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
                return this.defaultBrowser;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a list of the installed browsers in alphabetical order.
        /// </summary>
        public List<Browser> InstalledBrowsers
        {
            get
            {
                return this.installedBrowsers;
            }
        }
        #endregion Properties

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
            if (BrowserGateway.specialBrowsers.TryGetValue(key, out specialBrowser))
            {
                // Use value if not null or dictionary value
                retBrowser = new Browser(key)
                {
                    Name = (name != null) ? name : specialBrowser.Name,
                    IconPath = (iconPath != null) ? iconPath : specialBrowser.IconPath,
                    ApplicationPath = (applicationPath != null) ? applicationPath : specialBrowser.ApplicationPath,
                    ProgId = (progId != null) ? progId : specialBrowser.ProgId
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

            // If value is set, return it
            if (val != null)
            {
                return val.ToString();
            }

            // Value not found
            return null;
        }
        #endregion

        #region Loaders
        /// <summary>
        /// Loads all installed browsers from the registry.
        /// </summary>
        private void LoadBrowsers()
        {
            // Clear browser list
            this.installedBrowsers.Clear();

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
                        this.installedBrowsers.Add(BrowserGateway.CreateBrowser(key));
                    }
                    catch (ApplicationException)
                    {
                        // Ignore invalid browser and do not add to list.
                    }
                }

                // MS Edge is not listed in this registry key
                // Only add if OS is Windows10 or newer
                var version = OSVersions.getVersion();
                if (version.HasFlag(OSVersions.OS.WIN10) || version.HasFlag(OSVersions.OS.NEWER))
                {
                    this.installedBrowsers.Add(BrowserGateway.specialBrowsers["MSEDGE"]);
                }
            }
        }
        #endregion
    }
}
