using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;

namespace BetterDefaultBrowser.ViewModels
{
    public class PlainFilterViewModel : PlainFilterViewModelT<PlainFilter>
    {
        public PlainFilterViewModel(PlainFilter filter, ISettingsGateway settingsGateway, IBrowserGateway browserGateway) : base(filter, settingsGateway, browserGateway)
        {
        }
    }
}
