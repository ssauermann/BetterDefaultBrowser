using System;
using System.Collections.Generic;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Helpers;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Models.Enums;
using BetterDefaultBrowser.Properties;

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

        #region Display properties

        public IEnumerable<Tuple<Enum, string>> AvailableProtocols
        {
            get
            {

                var pro = EnumHelper.GetAllValuesAndDescriptions<Protocols>();

                Protocols anyProtocols = 0;
                foreach (var tuple in pro)
                {
                    anyProtocols |= (Protocols)tuple.Item1;
                }

                pro.Insert(0, new Tuple<Enum, string>(anyProtocols, Resources.AnyProtocol));

                if (Protocols == 0)
                {
                    Protocols = anyProtocols;
                }

                return pro;
            }
        }

        #endregion
    }
}
