using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Helper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {

            string[] args = Environment.GetCommandLineArgs();
            //Usage: <EXE> -install <ProxyPath> <MainAppPath>
            if (args[1] == "-install")
            {
                if (args.Length <= 3)
                    return;
                String appPath = System.Reflection.Assembly.GetEntryAssembly().CodeBase;
                BetterDefaultBrowser.FakeBrowser.InstallFakeBrowser(appPath, args[2], args[3]);
            }
            else if (args[1] == "-uninstall")
            {
                BetterDefaultBrowser.FakeBrowser.UninstallFakeBrowser();
            }
            else if (args[1] == "-iconhide")
            {
                //No idea what to do
            }
            else if (args[1] == "-iconshow")
            {
                //No idea what to do
            }
        }
    }
}
