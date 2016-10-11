using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Filters;
using BetterDefaultBrowser.ViewModels;
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
using System.Windows.Shapes;

namespace BetterDefaultBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {

        private Binding.AddFilterBind addBind = new Binding.AddFilterBind();
        private MainWindowBind mainBind = new MainWindowBind();


        private Lib.Filters.ManagedFilter managed;
        private Lib.Filters.OpenFilter open;
        private Lib.Filters.PlainFilter plain;

        private BindingList<Browser> toAdd = new BindingList<Browser>();
        private BindingList<Browser> added = new BindingList<Browser>();

        private bool IsSubfilter = false;


        //New Important stuff
        private ManagedFilterViewModel mFP;
        public MainWindow2()
        {
            InitializeComponent();

            #region NewStuff
            mFP = new ManagedFilterViewModel();
            AddManagedFilterGrid.DataContext = mFP;
            FilterType.DataContext = addBind;


            #endregion

            #region Bindings
            //this.DataContext = mainBind;
            AddFilterGrid.DataContext = addBind;

            #endregion

            InstallMenu.DataContext = AllBrowsers.BDBInstalled.Instance;


            browserList.ItemsSource = AllBrowsers.InstalledBrowsers;
            //comboBoxBrowserSelect.ItemsSource = AllBrowsers.InstalledBrowsers;
            //comboBoxBrowserSelectManaged.ItemsSource = AllBrowsers.InstalledBrowsers;

            WinVerLabel.Content = OSVersions.getVersion().ToString();

            //Visibility Settings
            AddManagedFilterGrid.Visibility = Visibility.Hidden;
            //AddFilterGrid.Visibility = Visibility.Hidden;//Remove Later
            //AddOpenFilterGrid.Visibility = Visibility.Hidden;
            AddPlainFilterGrid.Visibility = Visibility.Hidden;

            //---Show UAC Admin icon in menu---
            var img = new Image();
            img.Source = UACIcon.ShieldSource;
            var img2 = new Image();
            img2.Source = UACIcon.ShieldSource;
            InstallBDBMenuItem.Icon = img;
            UninstallBDBMenuItem.Icon = img2;
            //---------------------------------            

            //var win1 = new MainWindow();
            //win1.Show();
        }


        private void Applybutton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubfilter)
            {
                managed.Store();
            }
            else
            {
                open.InnerFilter = managed;
                open.Store();
            }
            IsSubfilter = false;
            AddManagedFilterGrid.Visibility = Visibility.Hidden;
            AddFilterGrid.Visibility = Visibility.Visible;
        }

        private void UninstallBDBMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("uninstall");
        }

        private void InstallBDBMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("install " + "\"" + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser-Proxy.exe\"" + " " + "\"" + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser.exe\"");
        }

        private void deleteSettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.Delete(HardcodedValues.DATA_FOLDER, true);
            Application.Current.Shutdown();
        }


        private void SetDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            if (browserList.SelectedItem != null)
                (browserList.SelectedItem as Browser).SetDefault();
        }

        private void addFilterButton_Click(object sender, RoutedEventArgs e)
        {
            switch (addBind.FilterType)
            {
                case Lib.Filters.Filter.FType.MANAGED:
                    managed = new Lib.Filters.ManagedFilter();
                    //AddManagedFilterGrid.DataContext = managed;
                    AddFilterGrid.Visibility = Visibility.Hidden;
                    AddManagedFilterGrid.Visibility = Visibility.Visible;
                    return;
                case Lib.Filters.Filter.FType.OPEN:
                    open = new Lib.Filters.OpenFilter();
                    AddOpenFilterGrid.DataContext = open;
                    AddFilterGrid.Visibility = Visibility.Hidden;

                    //Clear and copy Browserlist
                    toAdd.Clear();
                    added.Clear();

                    toAdd = cloneList(AllBrowsers.InstalledBrowsers);

                    //Bind ItemSource
                    toAddBrowserlistBox.ItemsSource = toAdd;
                    addBrowserlistBox.ItemsSource = added;
                    AddOpenFilterGrid.Visibility = Visibility.Visible;
                    return;

                case Lib.Filters.Filter.FType.PLAIN:
                    plain = new Lib.Filters.PlainFilter();
                    AddPlainFilterGrid.DataContext = plain;
                    AddFilterGrid.Visibility = Visibility.Hidden;
                    AddPlainFilterGrid.Visibility = Visibility.Visible;
                    return;
            }
        }

        private void savePlainButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSubfilter)
            {
                plain.Store();
            }
            else
            {
                open.InnerFilter = plain;
                open.Store();
            }
            IsSubfilter = false;
            AddPlainFilterGrid.Visibility = Visibility.Hidden;
            AddFilterGrid.Visibility = Visibility.Visible;
        }

        private BindingList<T> cloneList<T>(BindingList<T> old)
        {
            BindingList<T> newer = new BindingList<T>();
            foreach (T obj in old)
            {
                newer.Add(obj);
            }
            return newer;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            added.Add(toAddBrowserlistBox.SelectedItem as Browser);
            toAdd.Remove(toAddBrowserlistBox.SelectedItem as Browser);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            toAdd.Add(addBrowserlistBox.SelectedItem as Browser);
            added.Remove(addBrowserlistBox.SelectedItem as Browser);
        }

        private void nextFilterButton_Click(object sender, RoutedEventArgs e)
        {
            switch (addBind.FilterType)
            {
                case Lib.Filters.Filter.FType.MANAGED:
                    //Marks the next form for the user as subfilter
                    IsSubfilter = true;

                    managed = new Lib.Filters.ManagedFilter();
                    //AddManagedFilterGrid.DataContext = managed;
                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    AddManagedFilterGrid.Visibility = Visibility.Visible;
                    return;
                case Lib.Filters.Filter.FType.OPEN:
                    throw new Exception("Illegal State when using openfilter");

                case Lib.Filters.Filter.FType.PLAIN:
                    //Marks the next form for the user as subfilter
                    IsSubfilter = true;

                    plain = new Lib.Filters.PlainFilter();
                    AddPlainFilterGrid.DataContext = plain;
                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    AddPlainFilterGrid.Visibility = Visibility.Visible;
                    return;
            }
        }


        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            switch (addBind.FilterType)
            {
                case Lib.Filters.Filter.FType.MANAGED:
                    AddManagedFilterGrid.DataContext = new ManagedFilterViewModel(new Lib.Filters.ManagedFilter { Name = "LOLOLO" });
                    AddManagedFilterGrid.Visibility = Visibility.Visible;
                    return;

                case Lib.Filters.Filter.FType.OPEN:
                    open = new Lib.Filters.OpenFilter();
                    AddOpenFilterGrid.DataContext = open;

                    //Clear and copy Browserlist
                    toAdd.Clear();
                    added.Clear();

                    toAdd = cloneList(AllBrowsers.InstalledBrowsers);

                    //Bind ItemSource
                    toAddBrowserlistBox.ItemsSource = toAdd;
                    addBrowserlistBox.ItemsSource = added;
                    AddOpenFilterGrid.Visibility = Visibility.Visible;
                    return;

                case Lib.Filters.Filter.FType.PLAIN:
                    AddPlainFilterGrid.DataContext = new PlainFilterViewModel(new Lib.Filters.PlainFilter { Name = "plain filter" });
                    AddFilterGrid.Visibility = Visibility.Hidden;
                    AddPlainFilterGrid.Visibility = Visibility.Visible;
                    return;
            }
        }

        #region FilterList Methods
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
        #endregion
    }
}
