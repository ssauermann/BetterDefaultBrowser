using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.Lib.Gateways
{
    using Models;

    public interface ISettingsGateway
    {

        Browser DefaultBrowser { get; set; }

        List<Filter> Filters { get; }

        void UpdateOrAddFilter(Filter filter);

        void RemoveFilter(Filter filter);

    }
}
