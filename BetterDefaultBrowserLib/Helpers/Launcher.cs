using System;
using System.Diagnostics;
using System.Net;
using Serilog;

namespace BetterDefaultBrowser.Lib.Helpers
{
    using Models;

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
        public static void Launch(string path, string param)
        {
            Process proc = new Process
            {
                StartInfo =
                {
                    FileName = path,
                    Arguments = param
                }
            };

            Log.Debug("Opening {Path} with {Param} as parameters.", path, param);
            proc.Start();
        }

        /// <summary>
        ///  Open a browser on a webpage.
        /// </summary>
        /// <param name="browser">Browser to open</param>
        /// <param name="url">URL in an valid scheme to open</param>
        public static void LaunchBrowser(Browser browser, string url)
        {
            var escaped = url.Replace(" ", "%20");
            if (browser.Key == "MSEDGE")
            {
                ////https://github.com/mihula/RunEdge/blob/master/RunEdge/Program.cs
                Process.Start($"microsoft-edge:{escaped}");
            }
            else
            {
                Launch(browser.ApplicationPath, escaped);
            }
        }
    }
}
