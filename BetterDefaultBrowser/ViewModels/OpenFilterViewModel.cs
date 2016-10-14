using BetterDefaultBrowser.Commands;
using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static BetterDefaultBrowser.Lib.Filters.Filter;

namespace BetterDefaultBrowser.ViewModels
{
    class OpenFilterViewModel : FilterViewModelBase
    {
        public OpenFilter oFilter;
        public BindingList<Browser> Browsers;
        public BindingList<Browser> UsableBrowsers;


        //reference to the main window
        MainWindow2 window;


        public OpenFilterViewModel() : this(new OpenFilter { Name = "Unnamed filter", OnlyOpen = false })
        {
        }

        /// <summary>
        /// To give a reference to the window in order to set the data context
        /// </summary>
        /// <param name="window"></param>
        public OpenFilterViewModel(MainWindow2 window) : this()
        {
            this.window = window;

            Browsers = new BindingList<Browser>();
            UsableBrowsers = cloneList(AllBrowsers.InstalledBrowsers, new BindingList<Browser>());
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

        #region Commands
        /// <summary>
        /// Opens the subfilter
        /// </summary>
        protected virtual void NextFilterExecute()
        {
            switch (innerFilterType)
            {
                case Lib.Filters.Filter.FType.MANAGED:
                    ManagedFilterViewModel managedFilterVM = null;
                    if (InnerFilter == null)
                        managedFilterVM = new ManagedFilterViewModel(oFilter);
                    else
                        managedFilterVM = new ManagedFilterViewModel(oFilter.InnerFilter as ManagedFilter, oFilter);
                    window.AddManagedFilterGrid.DataContext = managedFilterVM;

                    window.AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    managedFilterVM.MyVisibility = Visibility.Visible;
                    return;
                case Lib.Filters.Filter.FType.OPEN:
                    throw new Exception("Illegal State when using openfilter");

                case Lib.Filters.Filter.FType.PLAIN:
                    PlainFilterViewModel plainFilterVM = null;
                    if (oFilter.InnerFilter == null)
                        plainFilterVM = new PlainFilterViewModel(oFilter);
                    else
                        plainFilterVM = new PlainFilterViewModel(oFilter.InnerFilter as PlainFilter, oFilter);
                    window.AddPlainFilterGrid.DataContext = plainFilterVM;
                    window.AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Visible;
                    return;
            }
        }


        protected virtual bool CanNextFilterExecute()
        {
            return false;
        }

        public ICommand StoreFilter { get { return new RelayCommand(NextFilterExecute, CanNextFilterExecute); } }
        #endregion
    }
}
