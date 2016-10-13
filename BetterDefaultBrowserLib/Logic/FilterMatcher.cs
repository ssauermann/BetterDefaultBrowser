using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace BetterDefaultBrowser.Lib.Logic
{
    using Gateways;
    using Models;

    /// <summary>
    /// Class for methods that checks the matching of a filter to an url.
    /// </summary>
    public static class FilterMatcher
    {
        /// <summary>
        /// Gateway instance
        /// </summary>
        private static IBrowserGateway gateway = BrowserGateway.Instance;

        /// <summary>
        /// Checks if a filter matches an url and provides the matched browser.
        /// </summary>
        /// <param name="f">Filter to match</param>
        /// <param name="url">Url to match</param>
        /// <param name="matchingResult">Browser as matching result if true is returned</param>
        /// <returns>Does filter match?</returns>
        public static bool Match(Filter f, string url, out Browser matchingResult)
        {
            if (f is PlainFilter)
            {
                // Managed filter has same matching as plain filter and (ManagedFilter is PlainFilter) holds.
                return Match((PlainFilter)f, url, out matchingResult);
            }
            else if (f is OpenFilter)
            {
                return Match((OpenFilter)f, url, out matchingResult);
            }
            else
            {
                throw new NotImplementedException("Missing implementation for this filter type.");
            }
        }

        /// <summary>
        /// Checks if a filter matches an url and provides the matched browser.
        /// </summary>
        /// <param name="f">Filter to match</param>
        /// <param name="url">Url to match</param>
        /// <param name="matchingResult">Browser as matching result if true is returned</param>
        /// <returns>Does filter match?</returns>
        private static bool Match(PlainFilter f, string url, out Browser matchingResult)
        {
            // Will work or model is invalid.
            var regex = new Regex(f.Regex);

            if (regex.IsMatch(url))
            {
                matchingResult = f.Browser;
                return true;
            }

            matchingResult = null;
            return false;
        }

        /// <summary>
        /// Checks if a filter matches an url and provides the matched browser.
        /// </summary>
        /// <param name="f">Filter to match</param>
        /// <param name="url">Url to match</param>
        /// <param name="matchingResult">Browser as matching result if true is returned</param>
        /// <returns>Does filter match?</returns>
        private static bool Match(OpenFilter f, string url, out Browser matchingResult)
        {
            matchingResult = null;

            // No match without inner filter
            if (f.InnerFilter == null)
            {
                return false;
            }

            Browser innerMatch;

            var isMatch = FilterMatcher.Match(f.InnerFilter, url, out innerMatch);

            // Inner filter does not match
            if (!isMatch)
            {
                return false;
            }

            // Inner filter does match
            foreach (Browser b in gateway.InstalledBrowsers)
            {
                if (IsBrowserOpen(b))
                {
                    matchingResult = b;
                    return true;
                }
            }

            // Browser list does not match -> check if OnlyOpen is set
            if (f.OnlyOpen)
            {
                // Did not match
                return false;
            }

            // Return inner browser
            matchingResult = innerMatch;
            return true;
        }

        /// <summary>
        /// Checks if a browser is currently opened.
        /// </summary>
        /// <param name="browser">Browser to check</param>
        /// <returns>Is browser running?</returns>
        private static bool IsBrowserOpen(Browser browser)
        {
            var path = browser.ApplicationPath;

            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    string processPath = p.MainModule.FileName;
                    int pathEql = string.Compare(
                                    Path.GetFullPath(processPath).TrimEnd('\\'),
                                    Path.GetFullPath(path).TrimEnd('\\'),
                                    StringComparison.InvariantCultureIgnoreCase);
                    if (pathEql == 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex) when (ex is NotSupportedException | ex is InvalidOperationException)
                {
                    // Ignore: Some processes (System / Idle) can not be inspected
                }
                catch (Win32Exception)
                {
                    // Win32 Applications can't inspect 64bit processes
                }
            }

            return false;
        }
    }
}
