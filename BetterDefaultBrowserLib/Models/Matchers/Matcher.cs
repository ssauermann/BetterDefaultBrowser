using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Models.Matchers
{
    /// <summary>
    /// Abstract super type for all matchers.
    /// </summary>
    public abstract class Matcher
    {
        /// <summary>
        /// Checks if an url is matched by this matcher.
        /// </summary>
        /// <param name="url">Url to match</param>
        /// <returns>Does url match?</returns>
        public abstract bool IsMatch(string url);

        /// <summary>
        /// Returns the validation error for a given column name or null if no error exists.
        /// </summary>
        /// <param name="columnName">Field name</param>
        /// <returns>Null or the error string</returns>
        public abstract string GetValidationError(string columnName);

        #region IDataErrorInfo
        public string Error => string.Empty;

        public string this[string columnName] => GetValidationError(columnName);
        #endregion
    }
}
