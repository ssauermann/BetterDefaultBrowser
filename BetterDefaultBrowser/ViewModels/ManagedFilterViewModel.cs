using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Models.Enums;

namespace BetterDefaultBrowser.ViewModels
{
    public class ManagedFilterViewModel : PlainFilterViewModelT<ManagedFilter>
    {

        #region Constructor
        public ManagedFilterViewModel(ManagedFilter filter, ISettingsGateway settingsGateway, IBrowserGateway browserGateway) : base(filter, settingsGateway, browserGateway)
        {
        }
        #endregion

        #region MangedFilter Properties

        public Protocols Protocols
        {
            get { return Filter.Protocols; }
            set
            {
                if (Filter.Protocols != value)
                {
                    Filter.Protocols = value;
                    OnPropertyChanged(nameof(Protocols));
                }
            }
        }

        public Ignore Flags
        {
            get { return Filter.Flags; }
            set
            {
                if (Filter.Flags != value)
                {
                    Filter.Flags = value;
                    OnPropertyChanged(nameof(Flags));
                }
            }
        }

        public string Url
        {
            get { return Filter.Url; }
            set
            {
                if (Filter.Url != value)
                {
                    Filter.Url = Url;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }
        #endregion

    }
}
