using System;
using System.Collections.Generic;
using System.Windows.Input;
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Helpers;
using BetterDefaultBrowser.Lib.Logic;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.Lib.Models.Enums;
using BetterDefaultBrowser.Properties;

namespace BetterDefaultBrowser.ViewModels
{
    public class ManagedFilterViewModel : PlainFilterViewModelT<ManagedFilter>
    {
        private string _urlPage;
        private string _urlParameter;
        private string _urlPort;
        private string _urlTld;
        private string _urlSd;
        private string _urlProtocol;

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

        #endregion

        #region Button texts

        public string UrlPage
        {
            get { return _urlPage; }
            private set
            {
                if (_urlPage != value)
                {
                    _urlPage = value;
                    OnPropertyChanged(nameof(UrlPage));
                }
            }
        }

        public string UrlParameter
        {
            get { return _urlParameter; }
            private set
            {
                if (_urlParameter != value)
                {
                    _urlParameter = value;
                    OnPropertyChanged(nameof(UrlParameter));
                }
            }
        }

        public string UrlPort
        {
            get { return _urlPort; }
            private set
            {
                if (_urlPort != value)
                {
                    _urlPort = value;
                    OnPropertyChanged(nameof(UrlPort));
                }
            }
        }

        public string UrlProtocol
        {
            get { return _urlProtocol; }
            private set
            {
                if (_urlProtocol != value)
                {
                    _urlProtocol = value;
                    OnPropertyChanged(nameof(UrlProtocol));
                }
            }
        }

        public string UrlTld
        {
            get { return _urlTld; }
            private set
            {
                if (_urlTld != value)
                {
                    _urlTld = value;
                    OnPropertyChanged(nameof(UrlTld));
                }
            }
        }

        public string UrlSd
        {
            get { return _urlSd; }
            private set
            {
                if (_urlSd != value)
                {
                    _urlSd = value;
                    OnPropertyChanged(nameof(UrlSd));
                }
            }
        }

        private void UpdateUrlParts()
        {
            var matcher = new UrlParser(Url);
            if (matcher.Parse())
            {
                UrlProtocol = matcher.Protocol;
                UrlPort = "90";
                UrlTld = "de";
            }
            else
            {
                UrlPage = null;
                UrlParameter = null;
                UrlPort = null;
                UrlProtocol = null;
                UrlSd = null;
                UrlTld = null;
            }
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

        public ICommand IgnoreProtocol => new RelayCommand(
                     param => InvertFlag(Ignore.Protocol),
                     param => true
                 );

        public ICommand IgnoreSd => new RelayCommand(
                     param => InvertFlag(Ignore.SubDomain),
                     param => true
                 );

        public ICommand IgnoreTld => new RelayCommand(
                     param => InvertFlag(Ignore.TopLevelDomain),
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
