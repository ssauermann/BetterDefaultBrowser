using System;
using System.Diagnostics;

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
            proc.Start();
        }

        /// <summary>
        ///  Open a browser on a webpage.
        /// </summary>
        /// <param name="browser">Browser to open</param>
        /// <param name="url">URL to open</param>
        public static void LaunchBrowser(Browser browser, string url)
        {
            Launch(browser.ApplicationPath, url);
        }

        ////https://github.com/mihula/RunEdge/blob/master/RunEdge/Program.cs

        /// <summary>
        /// Run MS Edge
        /// </summary>
        /// <param name="url">URL to open</param>
        public static void RunEdge(string url)
        {
            var uri = string.Join(" ", url);
            if (!string.IsNullOrEmpty(uri))
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
