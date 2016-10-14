using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Debug;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetterDefaultBrowser.Helper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                DebugHelper.SetUpListener();

                string[] args = Environment.GetCommandLineArgs();
                Debug.WriteLine("Helper was started!");

                //Usage: <EXE> -install <ProxyPath> <MainAppPath>
                if (args[1] == "-install")
                {
                    if (args.Length <= 3)
                        return;
                    FakeBrowser.InstallFakeBrowser(args[0], args[2], args[3]);
                    DebugHelper.WriteLine("BDB was installed.");
                }
                else if (args[1] == "-uninstall")
                {
                    FakeBrowser.UninstallFakeBrowser();
                    DebugHelper.WriteLine("BDB was uninstalled.");
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
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                var result = MessageBox.Show("An error has occured!\n" +
                    "Please open '" + HardcodedValues.APP_NAME + "' and reset your configuration.\n" +
                    "If this error appears again, report it to the developers and use windows to reset your default browser.",
                        HardcodedValues.APP_NAME + " - An error has occured!",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);
                if (result == DialogResult.Yes)
                {
                    runMainExe(HardcodedValues.HELPER_ERROR_CODE.UNKOWN);
                }
                Environment.Exit(1);
            }
            finally
            {
                Trace.Close();
            }
        }

        private static void runMainExe(HardcodedValues.HELPER_ERROR_CODE error)
        {
            try
            {
                var mainAppPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet\"
                    + HardcodedValues.APP_NAME + @"\Capabilities")
                    .GetValue("ApplicationMainExe").ToString();

                Launcher.Launch(mainAppPath, "-herror " + error.ToString());
            }
            catch
            {
                //Ignore and exit
                Environment.Exit(1);
            }
        }
    }
}
