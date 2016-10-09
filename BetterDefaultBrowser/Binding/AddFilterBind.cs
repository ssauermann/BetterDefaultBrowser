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

        private FType filtertype;
        public FType FilterType
        {
            get { return filtertype; }
            set
            {
                filtertype = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FilterType"));
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
