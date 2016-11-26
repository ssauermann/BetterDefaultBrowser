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
        #region Fields
        private string _urlDomain;
        private string _urlPage;
        private string _urlParameter;
        private string _urlPort;
        private string _urlTld;
        private string _urlSd;
        private string _urlProtocol;
        #endregion

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

        #region Saving

        public override void PrepareSave()
        {
            // Set ignore flags which have no matching content
            // e.g. Will ignore port if none is set
            if (UrlPage == null)
            {
                Flags |= Ignore.Page;
            }
            if (UrlParameter == null)
            {
                Flags |= Ignore.Parameter;
            }
            if (UrlPort == null)
            {
                Flags |= Ignore.Port;
            }
            if (UrlProtocol == null)
            {
                Flags |= Ignore.Protocol;
            }
            if (UrlSd == null)
            {
                Flags |= Ignore.SubDomain;
            }
            if (UrlTld == null)
            {
                Flags |= Ignore.TopLevelDomain;
            }

            // Prepare save of parent
            base.PrepareSave();
        }

        #endregion

        #region Button texts
        public string UrlDomain
        {
            get { return _urlDomain; }
            private set
            {
                if (_urlDomain != value)
                {
                    _urlDomain = value;
                    OnPropertyChanged(nameof(UrlDomain));
                }
            }
        }

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
                UrlDomain = matcher.Domain;
                UrlPage = matcher.Page;
                UrlParameter = matcher.Parameter;
                UrlPort = matcher.Port;
                UrlProtocol = matcher.Protocol;
                UrlSd = matcher.Sd;
                UrlTld = matcher.Tld;
            }
            else
            {
                UrlDomain = null;
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
