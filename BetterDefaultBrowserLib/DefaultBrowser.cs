using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BetterDefaultBrowserLib
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

        public Browser GetDefault()
        {
            var id = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", "NONE").ToString();

            foreach (Browser b in browsers)
            {
                if (b.ProgId.Equals(id))
                {
                    return b;
                }
            }

            return null;
        }

        public void SetDefault(Browser browser)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice", "ProgId", browser.ProgId);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\https\UserChoice", "ProgId", browser.ProgId);
            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\Shell\Associations\UrlAssociations\ftp\UserChoice", "ProgId", browser.ProgId);
        }

        public bool isInstalled()
        {
            foreach (var b in Browsers)
            {
                if(b.Name == "BDB Proxy")
                {
                    return true;
                }
            }
            return false;
        }

    }
}
