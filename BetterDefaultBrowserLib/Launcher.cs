using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Methods for launching extra processes.
    /// </summary>
    public static class Launcher
    {
        /// <summary>
        /// Start a process.
        /// </summary>
        /// <param name="path">Path to executable.</param>
        /// <param name="param">Parameter string for the new process.</param>
        public static void Launch(String path, String param)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = path;
            proc.StartInfo.Arguments = param;
            proc.Start();
        }

        public static void LaunchBrowser(Browser browser, String url)
        {
            Launch(browser.ApplicationPath, url);
        }
    }
}
