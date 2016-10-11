using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static BetterDefaultBrowser.Lib.Filters.ManagedFilter;

namespace BetterDefaultBrowser.ViewModels
{
    class ManagedFilterViewModel : PlainFilterViewModel
    {
        private ManagedFilter mFilter;
        private OpenFilter oFilter = null;
        private bool IsSubfilter = false;
        public ManagedFilterViewModel()
            : this(new ManagedFilter
            {
                Name = "Unnamed filter",
                Protocols = Protocols.HTTP | Protocols.HTTPS,
                Flags = Ignore.Page | Ignore.Parameter | Ignore.SD
            })
        {
        }

        public ManagedFilterViewModel(ManagedFilter f) : base(f)
        {
            mFilter = (ManagedFilter)f;
        }

        public ManagedFilterViewModel(ManagedFilter f, OpenFilter o) : base(f)
        {
            mFilter = (ManagedFilter)f;
            oFilter = o;
            IsSubfilter = true;
        }

        #region Properties
        public Protocols Protocols
        {
            get { return mFilter.Protocols; }
            set
            {
                mFilter.Protocols = value;
                OnPropertyChanged("Protocols");
            }
        }

        public string URL
        {
            get { return mFilter.URL; }
            set
            {
                mFilter.URL = value;
                OnPropertyChanged("URL");
            }
        }

        public Ignore Flags
        {
            get
            {
                return mFilter.Flags;
            }
            set
            {
                if (!mFilter.Flags.Equals(value))
                {
                    mFilter.Flags = value;
                    OnPropertyChanged("Flags");
                    OnPropertyChanged("IgnoreSD");
                    OnPropertyChanged("IgnoreTLD");
                    OnPropertyChanged("IgnorePort");
                    OnPropertyChanged("IgnorePage");
                    OnPropertyChanged("IgnoreParameter");
                }
            }
        }


        #endregion


        #region Helper Properties
        public bool IgnoreSD
        {
            get
            {
                return Flags.HasFlag(Ignore.SD);
            }
            set
            {
                if (value)
                    Flags |= Ignore.SD;
                else
                    Flags &= ~Ignore.SD;
            }
        }

        public bool IgnoreTLD
        {
            get
            {
                return Flags.HasFlag(Ignore.TLD);
            }
            set
            {
                if (value)
                    Flags |= Ignore.TLD;
                else
                    Flags &= ~Ignore.TLD;
            }
        }

        public bool IgnorePort
        {
            get
            {
                return Flags.HasFlag(Ignore.Port);
            }
            set
            {
                if (value)
                    Flags |= Ignore.Port;
                else
                    Flags &= ~Ignore.Port;
            }
        }

        public bool IgnorePage
        {
            get
            {
                return Flags.HasFlag(Ignore.Page);
            }
            set
            {
                if (value)
                    Flags |= Ignore.Page;
                else
                    Flags &= ~Ignore.Page;
            }
        }

        public bool IgnoreParameter
        {
            get
            {
                return Flags.HasFlag(Ignore.Parameter);
            }
            set
            {
                if (value)
                    Flags |= Ignore.Parameter;
                else
                    Flags &= ~Ignore.Parameter;
            }
        }

        public bool ProtocolHTTP
        {
            get
            {
                return Protocols.HasFlag(Protocols.HTTP);
            }
            set
            {
                if (value)
                    Protocols |= Protocols.HTTP;
                else
                    Protocols &= ~Protocols.HTTP;
            }
        }

        public bool ProtocolHTTPS
        {
            get
            {
                return Protocols.HasFlag(Protocols.HTTPS);
            }
            set
            {
                if (value)
                    Protocols |= Protocols.HTTPS;
                else
                    Protocols &= ~Protocols.HTTPS;
            }
        }

        #endregion

        #region Commands
        protected override void StoreFilterExecute()
        {
            //Assuming browser is valid
            mFilter.AssignedBrowser = this.Browser;
            if (!IsSubfilter)
            {
                mFilter.Store();
            }
            else
            {
                oFilter.InnerFilter = mFilter;
                oFilter.Store();
            }
            MyVisibility = Visibility.Hidden;
        }

        protected override bool CanStoreFilterExecute()
        {
            return Name != "" && Browser != null && Protocols != 0 && RegexBuilder.URLIsValid(URL);
        }
        #endregion

    }
}
