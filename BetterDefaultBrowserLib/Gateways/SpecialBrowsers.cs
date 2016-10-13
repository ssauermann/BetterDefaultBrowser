using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Gateways
{
    using Models;

    /// <summary>
    /// Class with browser data which can't be found at the normal location in the registry.
    /// </summary>
    internal static class SpecialBrowsers
    {
        /// <summary>
        /// Dictionary for storing data which can't be found at the normal location in the registry.
        /// </summary>
        public static readonly Dictionary<string, Browser> Map = new Dictionary<string, Browser>();

        /// <summary>
        /// Initializes static members of the <see cref="SpecialBrowsers" /> class.
        /// </summary>
        static SpecialBrowsers()
        {
            // Initialize special browsers
            Map["MSEDGE"] = new Browser("MSEDGE")
            {
                Name = "Microsoft Edge",
                ProgId = "AppXq0fevzme2pys62n3e0fbqa7peapykr8v",
                IconPath = @"%windir%\SystemApps\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\MicrosoftEdge.exe,0",
                ApplicationPath = "microsoft-edge:"
            };

            Map["IEXPLORE.EXE"] = new Browser("IEXPLORE.EXE")
            {
                Name = "Internet Explorer",
                ProgId = "IE.HTTP"
            };

            Map["VMWAREHOSTOPEN.EXE"] = new Browser("VMWAREHOSTOPEN.EXE")
            {
                Name = "VMware Host Open",
                ProgId = "VMwareHostOpen.AssocUrl"
            };
        }
    }
}
