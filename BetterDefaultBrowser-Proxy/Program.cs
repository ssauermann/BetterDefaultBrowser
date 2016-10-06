using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BetterDefaultBrowserLib;
using System.Diagnostics;
using BetterDefaultBrowserLib.Debug;

namespace BetterDefaultBrowser.Proxy
{

    /// <summary>
    /// Usage: Proxy.exe &lt;URL&gt;
    /// </summary>
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                DebugHelper.SetUpListener();

                //Process command line arguments
                string[] args = Environment.GetCommandLineArgs();

                //Use first argument as url or empty string if missing
                var url = "";
                if (args.Length > 1)
                    url = args[1];

                Debug.WriteLine("Information: " + "Url to open: '" + url + "'");

                //Read settings for filter and the default browser
                Settings settings = new Settings();

                var filters = settings.Filter;

                Debug.Assert(filters != null, "Filter list is null");

                var defBrowser = settings.DefaultBrowser;

                //Debug.Assert(defBrowser != null, "Default browser is null");
                if (defBrowser == null)
                {
                    Trace.TraceWarning("Default browser is not set!");
                    Environment.Exit(1);
                }

                //Select a browser based on the filters
                var selBrowser = defBrowser;

                foreach (var filter in filters)
                {
                    if (filter.match(url))
                    {
                        selBrowser = filter.AssignedBrowser;
                        break;
                    }
                }

                Debug.WriteLine("Information: " + "Matching browser: '" + selBrowser.Name + "'");

                //Loop catching
                if (selBrowser.Name == HardcodedValues.BROWSER_NAME)
                {
                    throw new ApplicationException("No wormholes allowed.");
                }

                //Start browser
                Process proc = new Process();
                proc.StartInfo.FileName = selBrowser.ApplicationPath;
                proc.StartInfo.Arguments = url;
                proc.Start();

                Debug.WriteLine("Information: " + "Browser opened.");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Environment.Exit(2);
            }
            finally
            {
                Trace.Close();
            }
        }
    }
}
