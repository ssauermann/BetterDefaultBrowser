﻿using BetterDefaultBrowser.Lib;
using BetterDefaultBrowser.Lib.Debug;
using BetterDefaultBrowser.Lib.Filters;
using BetterDefaultBrowser.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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



        private BindingList<Browser> toAdd = new BindingList<Browser>();
        private BindingList<Browser> added = new BindingList<Browser>();



        private OpenFilterViewModel openFilterVM = new OpenFilterViewModel();
        private ManagedFilterViewModel managedFilterVM = new ManagedFilterViewModel();
        private PlainFilterViewModel plainFilterVM = new PlainFilterViewModel();


        public MainWindow2()
        {
            InitializeComponent();

            #region ContextSetting
            FilterType.DataContext = addBind;
            filterTypeComboBox2.DataContext = addBind;
            filters.ItemsSource = Settings.Filter;
            InstallMenu.DataContext = AllBrowsers.BDBInstalled.Instance;

            #endregion






            browserList.ItemsSource = AllBrowsers.InstalledBrowsers;


            WinVerLabel.Content = OSVersions.getVersion().ToString();



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


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            openFilterVM.Browsers.Add((Browser)toAddBrowserlistBox.SelectedItem);
            openFilterVM.UsableBrowsers.Remove((Browser)toAddBrowserlistBox.SelectedItem);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            openFilterVM.UsableBrowsers.Add((Browser)addBrowserlistBox.SelectedItem);
            openFilterVM.Browsers.Remove((Browser)addBrowserlistBox.SelectedItem);
        }

        private void nextFilterButton_Click(object sender, RoutedEventArgs e)
        {
            switch (addBind.FilterTypeInner)
            {
                case Lib.Filters.Filter.FType.MANAGED:

                    if (openFilterVM.oFilter.InnerFilter == null)
                        managedFilterVM = new ManagedFilterViewModel(openFilterVM.oFilter);
                    else
                        managedFilterVM = new ManagedFilterViewModel(openFilterVM.oFilter.InnerFilter as ManagedFilter, openFilterVM.oFilter);
                    AddManagedFilterGrid.DataContext = managedFilterVM;

                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    managedFilterVM.MyVisibility = Visibility.Visible;
                    return;
                case Lib.Filters.Filter.FType.OPEN:
                    throw new Exception("Illegal State when using openfilter");

                case Lib.Filters.Filter.FType.PLAIN:

                    if (openFilterVM.oFilter.InnerFilter == null)
                        plainFilterVM = new PlainFilterViewModel(openFilterVM.oFilter);
                    else
                        plainFilterVM = new PlainFilterViewModel(openFilterVM.oFilter.InnerFilter as PlainFilter, openFilterVM.oFilter);
                    AddPlainFilterGrid.DataContext = plainFilterVM;
                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Visible;
                    return;
            }
        }


        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            filterTypeComboBox2.Visibility = Visibility.Visible;
            labelUnderFilter.Visibility = Visibility.Visible;
            switch (addBind.FilterType)
            {
                case Lib.Filters.Filter.FType.MANAGED:
                    managedFilterVM = new ManagedFilterViewModel();
                    AddManagedFilterGrid.DataContext = managedFilterVM;

                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Hidden;
                    managedFilterVM.MyVisibility = Visibility.Visible;
                    return;

                case Lib.Filters.Filter.FType.OPEN:
                    openFilterVM = new OpenFilterViewModel();
                    AddOpenFilterGrid.DataContext = openFilterVM;


                    //Bind ItemSource
                    toAddBrowserlistBox.ItemsSource = openFilterVM.UsableBrowsers;
                    addBrowserlistBox.ItemsSource = openFilterVM.Browsers;
                    managedFilterVM.MyVisibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Hidden;
                    AddOpenFilterGrid.Visibility = Visibility.Visible;
                    return;

                case Lib.Filters.Filter.FType.PLAIN:
                    plainFilterVM = new PlainFilterViewModel();
                    AddPlainFilterGrid.DataContext = plainFilterVM;

                    managedFilterVM.MyVisibility = Visibility.Hidden;
                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Visible;
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

            Settings.FilterOrderChanged();
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

        private void buttonEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedFilter = filters.SelectedItem as Filter;
            if (selectedFilter == null)
                return;

            switch (selectedFilter.Type)
            {
                case Lib.Filters.Filter.FType.MANAGED:
                    managedFilterVM = new ManagedFilterViewModel((ManagedFilter)selectedFilter);
                    AddManagedFilterGrid.DataContext = managedFilterVM;


                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Hidden;
                    managedFilterVM.MyVisibility = Visibility.Visible;
                    return;

                case Lib.Filters.Filter.FType.OPEN:
                    openFilterVM = new OpenFilterViewModel((OpenFilter)selectedFilter);
                    AddOpenFilterGrid.DataContext = openFilterVM;


                    //Bind ItemSource
                    toAddBrowserlistBox.ItemsSource = openFilterVM.UsableBrowsers;
                    addBrowserlistBox.ItemsSource = openFilterVM.Browsers;


                    managedFilterVM.MyVisibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Hidden;
                    AddOpenFilterGrid.Visibility = Visibility.Visible;



                    filterTypeComboBox2.Visibility = Visibility.Hidden;
                    filterTypeComboBox2.SelectedItem = (selectedFilter as OpenFilter).InnerFilter.Type;
                    labelUnderFilter.Visibility = Visibility.Hidden;

                    return;

                case Lib.Filters.Filter.FType.PLAIN:
                    plainFilterVM = new PlainFilterViewModel((PlainFilter)selectedFilter);
                    AddPlainFilterGrid.DataContext = plainFilterVM;


                    managedFilterVM.MyVisibility = Visibility.Hidden;
                    AddOpenFilterGrid.Visibility = Visibility.Hidden;
                    plainFilterVM.MyVisibility = Visibility.Visible;
                    return;
            }
        }

        #endregion

        private void bDBDefaultButton_Click(object sender, RoutedEventArgs e)
        {
            if (browserList.SelectedItem != null)
                Settings.DefaultBrowser = (Browser)browserList.SelectedItem;
        }

        private void MenuItemIssue_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://gitreports.com/issue/ssauermann/BetterDefaultBrowser");
        }

        private void MenuItemReadme_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"https://github.com/ssauermann/BetterDefaultBrowser/tree/master#better-default-browser---bdb");
        }

        private void MenuItemLog_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(HardcodedValues.DATA_FOLDER);
        }

        private void MenuItemRegDump_Click(object sender, RoutedEventArgs e)
        {
            DebugHelper.PrintRegistryDump();
        }
    }
}
