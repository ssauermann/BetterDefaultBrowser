using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Gateways
{
    using Models;

    /// <summary>
    /// Interface for all classes that load and return settings.
    /// </summary>
    public interface ISettingsGateway
    {
        /// <summary>
        /// Gets or sets the internal default browser.
        /// </summary>
        Browser DefaultBrowser { get; set; }

        /// <summary>
        /// Gets the saved filter list.
        /// </summary>
        List<Filter> Filters { get; }

        /// <summary>
        /// Updates a filter in the save file or adds it if it doesn't exist.
        /// </summary>
        /// <param name="filter">Filter with new information</param>
        void UpdateOrAddFilter(Filter filter);

        /// <summary>
        /// Removes a filter from the save file.
        /// </summary>
        /// <param name="filter">Filter to remove</param>
        void RemoveFilter(Filter filter);
    }
}
