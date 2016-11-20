using System.Collections.Generic;

namespace BetterDefaultBrowser.Lib.Gateways
{
    using Models;

    /// <summary>
    /// Interface for all classes that load and return browser information.
    /// </summary>
    public interface IBrowserGateway
    {
        /// <summary>
        /// Gets or sets the default browser.
        /// <para>When setting the default browser, any browser that is not part of the installed browsers will throw an exception.</para>
        /// </summary>
        Browser DefaultBrowser { get; set; }

        /// <summary>
        /// Gets a list of the installed browsers in alphabetical order.
        /// </summary>
        List<Browser> InstalledBrowsers { get; }

        /// <summary>
        /// Gets a specific installed browser by key. Will return null if the browser is not found.
        /// </summary>
        /// <param name="key">Browser key</param>
        /// <returns>Browser or null</returns>
        Browser GetBrowser(string key);
    }
}
