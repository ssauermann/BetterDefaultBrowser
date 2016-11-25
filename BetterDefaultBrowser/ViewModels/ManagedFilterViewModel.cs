using System;
using System.Collections.Generic;
using System.Windows.Input;
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
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Url))
                {
                    UpdateUrlParts();
                }
            };
        }
        #endregion

        #region ManagedFilter Properties

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
                    Filter.Url = value;
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

        #region Button texts

        public string UrlPage { get; private set; }
        public string UrlParameter { get; private set; }
        public string UrlPort { get; private set; }
        public string UrlTld { get; private set; }
        public string UrlSd { get; private set; }

        private void UpdateUrlParts()
        {

        }

        #endregion

        #region Button invert commands

        public ICommand IgnorePage => new RelayCommand(
                         param => InvertFlag(Ignore.Page),
                         param => true
                     );

        public ICommand IgnoreParameter => new RelayCommand(
                     param => InvertFlag(Ignore.Parameter),
                     param => true
                 );

        public ICommand IgnorePort => new RelayCommand(
                     param => InvertFlag(Ignore.Port),
                     param => true
                 );

        public ICommand IgnoreSd => new RelayCommand(
                     param => InvertFlag(Ignore.SD),
                     param => true
                 );

        public ICommand IgnoreTld => new RelayCommand(
                     param => InvertFlag(Ignore.TLD),
                     param => true
                 );

        private void InvertFlag(Ignore flag)
        {
            if (Flags.HasFlag(flag))
            {
                Flags &= ~flag;
            }
            else
            {
                Flags |= flag;
            }
        }

        #endregion
    }
}
