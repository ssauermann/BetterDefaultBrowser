using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;

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
        public String KeyName { get;}

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

        public override string ToString()
        {
            return Name;
        }

    }
}
