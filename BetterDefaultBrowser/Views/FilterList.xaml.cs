using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BetterDefaultBrowser.Views
{
    /// <summary>
    /// Interaktionslogik für FilterList.xaml
    /// </summary>
    public partial class FilterList : UserControl
    {
        public FilterList()
        {
            InitializeComponent();
            filters.ItemsSource = Settings.Filter;
        }

        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {
            MoveItem(-1);
        }

        public void MoveItem(int direction)
        {
            // Checking selected item
            if (filters.SelectedItem == null || filters.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = filters.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= filters.Items.Count)
                return; // Index out of range - nothing to do

            var selected = (Filter)filters.SelectedItem;
            var bindlist = (BindingList<Filter>)filters.ItemsSource;

            bindlist.Remove(selected);
            bindlist.Insert(newIndex, selected);
            filters.SelectedIndex = newIndex;
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            MoveItem(1);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (filters.SelectedItem == null || filters.SelectedIndex < 0)
                return; // No selected item - nothing to do

            var selected = (Filter)filters.SelectedItem;
            selected.Delete();
        }
    }
}
