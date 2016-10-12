using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Models
{
    /// <summary>
    /// Filter types
    /// </summary>
    public enum FilterTypes
    {
        /// <summary>
        /// Uses set values for regex creation
        /// </summary>
        MANAGED = 0,

        /// <summary>
        /// Uses first open browser
        /// </summary>
        OPEN = 1,

        /// <summary>
        /// Just plain regex
        /// </summary>
        PLAIN = 2
    }
}
