using System.Collections.Generic;

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
        BrowserStorage DefaultBrowser { get; set; }

        /// <summary>
        /// Returns a shallow-copied list of all filters.
        /// </summary>
        List<Filter> GetFilters();

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
