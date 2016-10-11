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

        /// <summary>
        ///  Open a browser on a webpage.
        /// </summary>
        /// <param name="browser">Browser to open</param>
        /// <param name="url">URL to open</param>
        public static void LaunchBrowser(Browser browser, String url)
        {
            Launch(browser.ApplicationPath, url);
        }

        //https://github.com/mihula/RunEdge/blob/master/RunEdge/Program.cs
        /// <summary>
        /// Run MS Edge
        /// </summary>
        /// <param name="url">URL to open</param>
        public static void RunEdge(String url)
        {
            var uri = String.Join(" ", url);
            if (!String.IsNullOrEmpty(uri))
            {
                Uri uriResult;
                if (!(Uri.TryCreate(uri, UriKind.Absolute, out uriResult) && (uriResult != null) && ((uriResult.Scheme == Uri.UriSchemeHttp) || (uriResult.Scheme == Uri.UriSchemeHttps))))
                {
                    uri = @"http://" + uri;
                }
            }
            Process.Start($"microsoft-edge:{uri}");
        }

    }
}
