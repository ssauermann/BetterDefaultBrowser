using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static BetterDefaultBrowser.Lib.Filters.Filter;

namespace BetterDefaultBrowser.ViewModels
{
    public abstract class FilterViewModelBase : INotifyPropertyChanged
    {
        protected Filter filter;
        public FilterViewModelBase(Filter filter)
        {
            this.filter = filter;
        }

        #region Properties
        public string Name
        {
            get { return filter.Name; }
            set
            {
                filter.Name = value;
                OnPropertyChanged("Name");
            }
        }

        private Visibility myVisibility = Visibility.Hidden;
        public Visibility MyVisibility
        {
            get { return myVisibility; }
            set
            {
                myVisibility = value;
                OnPropertyChanged("MyVisibility");
            }
        }

        #endregion

        #region Handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler prop = PropertyChanged;
            if (prop != null)
                prop(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
