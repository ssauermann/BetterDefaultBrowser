using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using BetterDefaultBrowser.Lib;
using System.Diagnostics;
using BetterDefaultBrowser.Lib.Debug;

namespace BetterDefaultBrowser.Helper
{

    class FakeBrowser
    {

        private static readonly String keyId = HardcodedValues.APP_NAME;
        private static readonly String name = HardcodedValues.APP_NAME;
        private static readonly String progId = HardcodedValues.PROG_ID;
        private static readonly String appDesc = HardcodedValues.APP_DESC;

        public static void InstallFakeBrowser(String helperPath, String proxyPath, String appPath)
        {

            DebugHelper.WriteLine("Helper path: " + helperPath);
            DebugHelper.WriteLine("Proxy path: " + proxyPath);
            DebugHelper.WriteLine("App path: " + appPath);

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
            cap.SetValue("ApplicationDescription", appDesc);
            cap.SetValue("ApplicationIcon", proxyPath + ",0");
            cap.SetValue("ApplicationName", name);

            //THIS ONE IS FOR FINDING THE MAIN APPFLICATION FROM PROXY
            cap.SetValue("ApplicationMainExe", appPath);

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
            Exception caughtException = null;
            try
            {
                var k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet", true);
                if (k != null)
                    if (k.OpenSubKey(keyId) != null)
                        k.DeleteSubKeyTree(keyId);

            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            try
            {
                //Remove installed program
                var k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\RegisteredApplications", true);
                if (k != null)
                    if (k.OpenSubKey(keyId) != null)
                        k.DeleteSubKeyTree(keyId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            try
            {
                //Remove ProgId
                var k = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes", true);
                if (k != null)
                    if (k.OpenSubKey(progId) != null)
                        k.DeleteSubKeyTree(progId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            ChangeNotify.NotifySystemOfNewRegistration();

            if (caughtException != null)
            {
                throw caughtException;
            }
        }

    }
}
