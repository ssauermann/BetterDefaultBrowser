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
        public OpenFilter oFilter;
        public BindingList<Browser> Browsers;
        public BindingList<Browser> UsableBrowsers;
        public OpenFilterViewModel() : this(new OpenFilter { Name = "Unnamed filter", OnlyOpen = false })
        {
        }

        public OpenFilterViewModel(OpenFilter f) : base(f)
        {
            oFilter = (OpenFilter)filter;
            Browsers = oFilter.Browsers;
            UsableBrowsers = cloneList(AllBrowsers.InstalledBrowsers);
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
