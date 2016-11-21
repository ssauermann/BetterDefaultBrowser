using System;
using BetterDefaultBrowser.Lib;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Logic;
using BetterDefaultBrowser.Lib.Helpers;
using Serilog;

namespace BetterDefaultBrowser.Proxy
{

    /// <summary>
    /// Usage: Proxy.exe &lt;URL&gt;
    /// </summary>
    static class Program
    {
        private static Uri _uriResult;

        [STAThread]
        static void Main()
        {
            try
            {
                Logging.SetUp("Proxy");

                //Process command line arguments
                string[] args = Environment.GetCommandLineArgs();

                //Use first argument as url or empty string if missing
                var url = "";
                if (args.Length > 1)
                    url = args[1];

                Log.Debug("Proxy was started with parameter: {Url}", url);

                if (url != "")
                {
                    //Add protocol if missing. TODO: Fix this for file paths
                    if (
                        !(Uri.TryCreate(url, UriKind.Absolute, out _uriResult) && (_uriResult != null) &&
                          ((_uriResult.Scheme == Uri.UriSchemeHttp) || (_uriResult.Scheme == Uri.UriSchemeHttps))))
                    {
                        url = @"http://" + url;
                        Log.Debug("Url to open: {Url}", url);
                    }
                }

                var settings = new SettingsGateway(HardcodedValues.DATA_FOLDER + "settings.xml");
                var browserGw = BrowserGateway.Instance;

                var filters = settings.GetFilters();

                //Mustn't be null
                Debug.Assert(filters != null, "Filter list is null");

                if (settings.DefaultBrowser == null)
                {
                    Fail("Default browser is not set.", "set a default browser");
                    return;
                }

                var defBrowser = browserGw.GetBrowser(settings.DefaultBrowser.BrowserKey);

                if (defBrowser == null)
                {
                    Fail("Default browser is not installed.", "select another browser as default");
                    return;
                }

                //Select a browser based on the filters
                Browser selBrowser = null;

                foreach (var filter in filters)
                {
                    if (FilterMatcher.Match(filter, url, out selBrowser))
                    {
                        Log.Debug("{Browser} was matched via this filter: {Filter}", selBrowser, filter);
                        break;
                    }
                }

                if (selBrowser == null)
                {
                    Log.Debug("Using {DefBrowser} because no matching filter was found", defBrowser);
                    selBrowser = defBrowser;
                }

                //Loop catching
                if (selBrowser.Name == HardcodedValues.APP_NAME)
                {
                    Fail("No wormholes allowed. You must not create filters which reference BDB.", "remove all filters which reference BDB");
                    return;
                }

                //Start browser
                //Edge is special
                if (selBrowser.Key == "MSEDGE")
                    Launcher.RunEdge(url);
                else
                    Launcher.Launch(selBrowser.ApplicationPath, url);

                Log.Debug("Proxy finished.");

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception was unhandled.");
                Fail("Unhandled exception: " + ex.Message, "reset your settings. If this error appears again, contact the developers");
            }
            finally
            {
            }
        }

        /// <summary>
        /// Show infobox and exit.
        /// </summary>
        private static void Fail(string msg, string userMsg)
        {
            Log.Error(msg);
            var result = MessageBox.Show(msg + "\n" +
                "Please open '" + HardcodedValues.APP_NAME + "' and " + userMsg + ".\nClick yes to open BDB now.",
                HardcodedValues.APP_NAME + " - An error occured!",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);
            if (result == DialogResult.Yes)
            {
                RunMainExe();
            }
            Environment.Exit(1);
        }


        private static void RunMainExe()
        {
            try
            {
                var openSubKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet\"
                                                                  + HardcodedValues.APP_NAME + @"\Capabilities");
                if (openSubKey != null)
                {
                    var mainAppPath = openSubKey
                        .GetValue("ApplicationMainExe").ToString();

                    Launcher.Launch(mainAppPath, "-perror");
                }
            }
            catch
            {
                //Ignore and exit
                Environment.Exit(1);
            }
        }
    }
}
