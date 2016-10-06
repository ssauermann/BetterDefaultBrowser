using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BetterDefaultBrowser.Lib
{
    public class DefaultBrowser
    {
        private LinkedList<Browser> browsers = new LinkedList<Browser>();

        public LinkedList<Browser> Browsers
        {
            get { return browsers; }
        }
        public DefaultBrowser()
        {
            var names = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet").GetSubKeyNames();
            foreach (var name in names)
            {
                browsers.AddLast(new Browser(name));
            }
        }

        public static Browser GetDefault()
        {
            var id = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", "NONE").ToString();

            foreach (Browser b in Browser.getInstalledBrowsers())
            {
                if (b.ProgId.Equals(id))
                {
                    return b;
                }
            }

            return null;
        }



        public bool isInstalled()
        {
            foreach (var b in Browsers)
            {
                if(b.Name == HardcodedValues.APP_NAME)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
