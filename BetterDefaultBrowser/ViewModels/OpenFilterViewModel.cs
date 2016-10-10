using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterDefaultBrowser.ViewModels
{
    class OpenFilterViewModel : FilterViewModelBase
    {
        OpenFilter oFilter;
        BindingList<Browser> Browsers;
        BindingList<Browser> UsableBrowsers;
        public OpenFilterViewModel() : base(new OpenFilter())
        {
            oFilter = (OpenFilter)filter;
            Browsers = oFilter.Browsers;
            UsableBrowsers = cloneList(Browsers);
        }


        #region Properties
        public bool OnlyOpen
        {
            get { return oFilter.OnlyOpen; }
            set
            {
                oFilter.OnlyOpen = value;
                OnPropertyChanged("OnlyOpen");
            }
        }

        public Filter InnerFilter
        {
            get
            { return oFilter.InnerFilter; }
            set
            {
                oFilter.InnerFilter = value;
                OnPropertyChanged("InnerFilter");
            }
        }

        #endregion


        private BindingList<T> cloneList<T>(BindingList<T> old)
        {
            BindingList<T> newer = new BindingList<T>();
            foreach (T obj in old)
            {
                newer.Add(obj);
            }
            return newer;
        }
    }
}
