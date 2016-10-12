using System.ComponentModel;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Model of a open filter.
    /// </summary>
    public class OpenFilter : Filter
    {
        /// <summary>
        /// Priority list of assigned browsers.
        /// </summary>
        private BindingList<Browser> browsers = new BindingList<Browser>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFilter" /> class.
        /// </summary>
        public OpenFilter() : base(FilterTypes.OPEN)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether only currently running browsers should be used
        /// or a new browser should be opened if no running one matches.
        /// </summary>
        public bool OnlyOpen { get; set; }

        /// <summary>
        /// Gets the list of the browser priority list.
        /// </summary>
        public BindingList<Browser> Browsers
        {
            get
            {
                return this.browsers;
            }
        }

        /// <summary>
        /// Gets or sets the inner filter.
        /// </summary>
        public Filter InnerFilter { get; set; }
    }
}
