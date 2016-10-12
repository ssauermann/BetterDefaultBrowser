using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BetterDefaultBrowser.Lib.Filters.Filter;

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


            UsableBrowsers = cloneList(AllBrowsers.InstalledBrowsers, Browsers);
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

        public BindingList<FType> InnerFilterTypes
        {
            get
            {
                var l = new BindingList<FType>();
                l.Add(FType.MANAGED);
                l.Add(FType.PLAIN);
                return l;
            }
        }

        private FType innerFilterType = FType.MANAGED;
        public FType InnerFilterType
        {
            get { return innerFilterType; }
            set
            {
                innerFilterType = value;
                OnPropertyChanged("FilterTypeInner");
            }
        }

        private BindingList<T> cloneList<T>(BindingList<T> old, BindingList<T> excluded)
        {
            BindingList<T> newer = new BindingList<T>();
            foreach (T obj in old)
            {
                if (!excluded.Contains(obj))
                    newer.Add(obj);
            }
            return newer;
        }
    }
}
