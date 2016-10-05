using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BetterDefaultBrowser
{

    class FakeBrowser
    {

        private static readonly String keyId = "Better Default Browser";
        private static readonly String name = "BDB Proxy";
        private static readonly String progId = "BetterDefaultBrowserFake";

        public static void InstallFakeBrowser(String helperPath, String proxyPath)
        {
            //TODO: Test key existance
            //Set ProgID
            var classKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", true);
            var progKey = classKey.CreateSubKey(progId);
            progKey.SetValue(null, "HTML File");
            progKey.CreateSubKey("DefaultIcon").SetValue(null, proxyPath + ",0");
            progKey.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command").SetValue(null, proxyPath + " \"%1\"");


            //Set browser settings
           var myKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet", true).CreateSubKey(keyId);
            //Set name
            myKey.SetValue(null, keyId);

            var cap = myKey.CreateSubKey("Capabilities");
            cap.SetValue("ApplicationDescription", "Fake browser entry for 'Better Default Browser' proxy.");
            cap.SetValue("ApplicationIcon", proxyPath + ",0");  //Check this
            cap.SetValue("ApplicationName", name);

            var fa = cap.CreateSubKey("FileAssociations");
            fa.SetValue(".htm", progId);
            fa.SetValue(".html", progId);
            fa.SetValue(".shtml", progId);
            fa.SetValue(".xht", progId);
            fa.SetValue(".xhtml", progId);

            var sm = cap.CreateSubKey("StartMenu");
            sm.SetValue("StartMenuInternet", keyId);

            var ua = cap.CreateSubKey("URLAssociations");
            ua.SetValue("ftp", progId);
            ua.SetValue("http", progId);
            ua.SetValue("https", progId);

            var di = myKey.CreateSubKey("DefaultIcon");
            di.SetValue(null, proxyPath + ",0");

            var ii = myKey.CreateSubKey("InstallInfo");
            ii.SetValue("HideIconsCommand", helperPath + " -iconhide");
            ii.SetValue("IconsVisible", 1, RegistryValueKind.DWord);
            ii.SetValue("ReinstallCommand", helperPath + " -install");
            ii.SetValue("ShowIconsCommand", helperPath + " -iconshow");

            var shellOpenCommand = myKey.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command");
            shellOpenCommand.SetValue(null, proxyPath);
            myKey.Close();

            //Set installed program
            Registry.LocalMachine.OpenSubKey(@"SOFTWARE\RegisteredApplications", true).SetValue(keyId, @"SOFTWARE\Clients\StartMenuInternet\" + keyId + @"\Capabilities");

            ChangeNotify.NotifySystemOfNewRegistration();
        }

        public static void UninstallFakeBrowser()
        {
            Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet", true).DeleteSubKeyTree(keyId);
            
            //Remove installed program
            Registry.LocalMachine.OpenSubKey(@"SOFTWARE\RegisteredApplications", true).DeleteValue(keyId);

            //Remove ProgId
            Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", true).DeleteSubKeyTree(progId);

            ChangeNotify.NotifySystemOfNewRegistration();
        }

    }
}
