using BetterDefaultBrowser.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BetterDefaultBrowser.Lib.Filters.Filter;

namespace BetterDefaultBrowser.Binding
{
    class AddFilterBind : INotifyPropertyChanged
    {
        private Browser browser;
        public Browser Browser
        {
            get { return browser; }
            set
            {
                browser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Browser"));
            }
        }

        private FType filtertype = FType.MANAGED;
        public FType FilterType
        {
            get { return filtertype; }
            set
            {
                filtertype = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterType"));
            }
        }

        private FType filtertype2 = FType.MANAGED;
        public FType FilterTypeInner
        {
            get { return filtertype2; }
            set
            {
                filtertype2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterTypeInner"));
            }
        }

        public BindingList<FType> FilterTypes
        {
            get
            {
                var types = Enum.GetValues(typeof(FType));
                return new BindingList<FType>(types.OfType<FType>().ToList());
            }
        }

        public BindingList<FType> FilterTypesInner
        {
            get
            {
                var l = new BindingList<FType>();
                l.Add(FType.MANAGED);
                l.Add(FType.PLAIN);
                return l;
            }
        }

        #region Handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion
    }
}
