using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;

namespace BetterDefaultBrowserLib
{
    public class Browser
    {
        public String KeyName;
        private String path;

        public Browser(String keyName)
        {
            this.KeyName = keyName;
            this.path = @"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet\" + keyName;
            if ((Registry.GetValue(path, null, null)) == null)
            {
                throw new ArgumentException("Browser key not existing");
            }
        }

        public String ProgId
        {
            get
            {
                //Internet explorer is special :)
                if (KeyName.Equals("IEXPLORE.EXE"))
                {
                    return "IE.HTTP";
                }

                return Registry.GetValue(path + @"\Capabilities\URLAssociations", "http", "NONE").ToString();
            }
        }

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

        public String IconPath
        {
            get
            {
                return Registry.GetValue(path + @"\DefaultIcon", null, "NONE").ToString();
            }
        }

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

        public void StartWithWebsite(String website)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = this.ApplicationPath;
            proc.StartInfo.Arguments = website;
            proc.Start();
        }
    }
}
