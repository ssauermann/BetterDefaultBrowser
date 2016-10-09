using BetterDefaultBrowser.Lib;
using System;
using System.Collections.Generic;
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


        public MainWindow2()
        {
            InitializeComponent();

            #region Bindings
            this.DataContext = mainBind;
            AddFilterGrid.DataContext = addBind;

            #endregion

            InstallMenu.DataContext = AllBrowsers.BDBInstalled.Instance;


            browserList.ItemsSource = AllBrowsers.InstalledBrowsers;
            comboBoxBrowserSelect.ItemsSource = AllBrowsers.InstalledBrowsers;

            WinVerLabel.Content = OSVersions.getVersion().ToString();

            //Visibility Settings
            AddManagedFilterGrid.Visibility = Visibility.Hidden;
            AddFilterGrid.Visibility = Visibility.Visible;
            AddOpenFilterGrid.Visibility = Visibility.Hidden;
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
            managed.Store();
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
            System.IO.Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BetterDefaultBrowser", true);
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
                    AddManagedFilterGrid.DataContext = managed;
                    AddFilterGrid.Visibility = Visibility.Hidden;
                    AddManagedFilterGrid.Visibility = Visibility.Visible;
                    return;
                case Lib.Filters.Filter.FType.OPEN:
                    open = new Lib.Filters.OpenFilter();
                    AddOpenFilterGrid.DataContext = open;
                    AddFilterGrid.Visibility = Visibility.Hidden;
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
    }
}
