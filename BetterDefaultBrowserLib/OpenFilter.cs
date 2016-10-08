using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib
{
    /// <summary>
    /// Filter which will use currently running browsers first before opening a new one.
    /// </summary>
    class OpenFilter : Filter
    {
        private bool onlyOpen;
        private BindingList<Browser> browsers = new BindingList<Browser>();

        /// <summary>
        /// Create a new OpenFilter.
        /// </summary>
        /// <param name="inner">Inner Filter used for Matching.</param>
        public OpenFilter(Filter inner) : base(inner.AssignedBrowser)
        {
            this.Type = FType.OPEN;
        }

        /// <summary>
        /// Do not start a new browsers, only use open browsers from Browsers list, else do not match.
        /// </summary>
        public Boolean OnlyOpen
        {
            get
            {
                return onlyOpen;
            }
            set
            {
                if(onlyOpen != value)
                {
                    onlyOpen = value;
                    OnPropertyChanged("OnlyOpen");
                }
            }
        }

        /// <summary>
        /// Priority list which browser to use if opened.
        /// </summary>
        public BindingList<Browser> Browsers
        {
            get
            {
                return browsers;
            }
        }

    }
}
