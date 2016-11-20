using System;
using System.Text.RegularExpressions;

namespace BetterDefaultBrowser.Lib.Helpers
{
    /// <summary>
    /// Helper methods for regex handling
    /// </summary>
    internal static class RegexHelper
    {
        /// <summary>
        /// Test an string if it is an valid regular expression.
        /// </summary>
        /// <param name="pattern">String to test</param>
        /// <returns>Is valid?</returns>
        public static bool IsValid(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Regex.IsMatch(string.Empty, pattern);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}