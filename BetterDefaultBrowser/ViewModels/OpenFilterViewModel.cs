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
        //public BindingList<Browser> Browsers;
        //public BindingList<Browser> UsableBrowsers;


        //reference to the main window
        MainWindow2 window;


        //Nasic constructor for xaml to call
        public OpenFilterViewModel() : this(new OpenFilter(), null)
        {
        }


        //call this constructor to create a new Openfilter
        public OpenFilterViewModel(MainWindow2 window) : this((new OpenFilter { Name = "Unnamed filter", OnlyOpen = false }), window)
        {
        }



        public OpenFilterViewModel(OpenFilter f, MainWindow2 window) : base(f)
        {
            oFilter = (OpenFilter)filter;
            Browsers = oFilter.Browsers;


            UsableBrowsers = cloneList(AllBrowsers.InstalledBrowsers, Browsers);


            //Set the inner filter in case an existent filter is loaded
            this.window = window;
            if (oFilter.InnerFilter != null)
                innerFilterType = oFilter.InnerFilter.Type;
        }


        #region Properties
        private BindingList<Browser> browsers;
        public BindingList<Browser> Browsers { get { return browsers; } set { browsers = value; } }
        private BindingList<Browser> usableBrowsers;
        public BindingList<Browser> UsableBrowsers { get { return usableBrowsers; } set { usableBrowsers = value; } }


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


        //Check which Browsers are selected in the listbox for usable Browsers
        private Browser usableBrowsersSelected;
        public Browser UsableBrowsersSelected
        {
            get { return usableBrowsersSelected; }
            set
            {
                usableBrowsersSelected = value;
                OnPropertyChanged("UsableBrowsersSelected");
            }
        }


        //Check which Browsers are selected in the listbox for added browsers
        private Browser browsersSelected;
        public Browser BrowsersSelected
        {
            get { return browsersSelected; }
            set
            {
                browsersSelected = value;
                OnPropertyChanged("BrowsersSelected");
            }
        }

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

        private FType innerFilterType;
        public FType InnerFilterType
        {
            get { return innerFilterType; }
            set
            {
                innerFilterType = value;
                OnPropertyChanged("InnerFilterType");
            }
        }

        private BindingList<T> cloneList<T>(BindingList<T> old, BindingList<T> excluded)
        {
            //exclude BDB from list of usable Browsers
            Browser BDB = new Browser(HardcodedValues.APP_NAME);

            BindingList<T> newer = new BindingList<T>();
            foreach (T obj in old)
            {
                if (!excluded.Contains(obj) && !obj.Equals(BDB))
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
            return Browsers.Count > 0;
        }

        public ICommand NextFilter { get { return new RelayCommand(NextFilterExecute, CanNextFilterExecute); } }



        protected virtual void AddButtonExecute()
        {
            Browsers.Add(usableBrowsersSelected);
            UsableBrowsers.Remove(usableBrowsersSelected);
        }

        protected virtual bool CanAddButtonExecute()
        {
            return usableBrowsersSelected != null && (UsableBrowsers.Count > 0);
        }
        public ICommand AddButton { get { return new RelayCommand(AddButtonExecute, CanAddButtonExecute); } }

        protected virtual void DeleteButtonExecute()
        {
            UsableBrowsers.Add(browsersSelected);
            Browsers.Remove(browsersSelected);
        }

        protected virtual bool CanDeleteButtonExecute()
        {
            return browsersSelected != null && (Browsers.Count > 0);
        }
        public ICommand DeleteButton { get { return new RelayCommand(DeleteButtonExecute, CanDeleteButtonExecute); } }
        #endregion

    }
}
