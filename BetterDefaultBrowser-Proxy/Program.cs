using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterDefaultBrowser.Lib;
using System.Diagnostics;
using BetterDefaultBrowser.Lib.Debug;
using System.Windows.Forms;
using Microsoft.Win32;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Logic;
using BetterDefaultBrowser.Lib.Helpers;

namespace BetterDefaultBrowser.Proxy
{

    /// <summary>
    /// Usage: Proxy.exe &lt;URL&gt;
    /// </summary>
    static class Program
    {
        private static Uri uriResult;

        [STAThread]
        static void Main()
        {
            try
            {
                DebugHelper.SetUpListener();
                Debug.WriteLine("Proxy was started!");

                //Process command line arguments
                string[] args = Environment.GetCommandLineArgs();

                //Use first argument as url or empty string if missing
                var url = "";
                if (args.Length > 1)
                    url = args[1];

                //Add protocol if missing. TODO: Fix this for file paths
                if (!(Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult != null) && ((uriResult.Scheme == Uri.UriSchemeHttp) || (uriResult.Scheme == Uri.UriSchemeHttps))))
                {
                    url = @"http://" + url;
                }

                Debug.WriteLine("Information: " + "Url to open: '" + url + "'");


                var filters = SettingsGateway.Instance.Filters;

                //Mustn't be null
                Debug.Assert(filters != null, "Filter list is null");

                var defBrowser = SettingsGateway.Instance.DefaultBrowser;

                if (defBrowser == null)
                {
                    failBrowser();
                }

                //Select a browser based on the filters
                Browser selBrowser = null;

                foreach (var filter in filters)
                {
                    if (FilterMatcher.Match(filter, url, out selBrowser))
                    {
                        break;
                    }
                }

                if (selBrowser == null)
                {
                    selBrowser = defBrowser;
                }

                Debug.WriteLine("Information: " + "Matching browser: '" + selBrowser.Name + "'");

                //Loop catching
                if (selBrowser.Name == HardcodedValues.APP_NAME)
                {
                    failLoop();
                }

                //Start browser
                //Edge is special
                if (selBrowser.Key == "MSEDGE")
                    Launcher.RunEdge(url);
                else
                    Launcher.Launch(selBrowser.ApplicationPath, url);


                Debug.WriteLine("Information: " + "Browser opened.");

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
                    runMainExe(HardcodedValues.PROXY_ERROR_CODE.UNKOWN);
                }
                Environment.Exit(1);
            }
            finally
            {
                Trace.Close();
            }
        }

        /// <summary>
        /// Show infobox and exit.
        /// </summary>
        private static void failLoop()
        {
            Trace.TraceWarning("No wormholes allowed!");
            var result = MessageBox.Show("This request would loop!\n" +
                "Please open '" + HardcodedValues.APP_NAME + "' and remove filters that redirect to this proxy.",
                HardcodedValues.APP_NAME + " - No wormholes allowed!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                runMainExe(HardcodedValues.PROXY_ERROR_CODE.LOOP);
            }
            Environment.Exit(1);
        }

        private static void failBrowser()
        {
            Trace.TraceWarning("Default browser is not set!");
            var result = MessageBox.Show("The default browser is not set.\n" +
                "Please open '" + HardcodedValues.APP_NAME + "' and set it there.",
                HardcodedValues.APP_NAME + " - Exception",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                runMainExe(HardcodedValues.PROXY_ERROR_CODE.DEFAULT_BROWSER);
            }
            Environment.Exit(1);
        }

        private static void runMainExe(HardcodedValues.PROXY_ERROR_CODE error)
        {
            try
            {
                var mainAppPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet\"
                    + HardcodedValues.APP_NAME + @"\Capabilities")
                    .GetValue("ApplicationMainExe").ToString();

                Launcher.Launch(mainAppPath, "-perror " + error.ToString());
            }
            catch
            {
                //Ignore and exit
                Environment.Exit(1);
            }
        }
    }
}
