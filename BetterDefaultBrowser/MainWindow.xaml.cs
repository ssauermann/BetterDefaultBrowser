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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BetterDefaultBrowser.Lib;
using System.Runtime.InteropServices;
using BetterDefaultBrowser.Lib.Filters;

namespace BetterDefaultBrowser
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lblWinVersion.Content = OSVersions.getVersion().ToString();
            refresh();
        }

        public void refresh()
        {
            labelDefaultBrowser.Content = AllBrowsers.Default.Name;
            listBoxInstalledBrowsers.Items.Clear();
            foreach (var browser in AllBrowsers.InstalledBrowsers)
            {
                listBoxInstalledBrowsers.Items.Add(browser);
            }


            if (AllBrowsers.BDBInstalled.Instance.IsBDBInstalled)
            {
                btnInstallBrowser.IsEnabled = false;
                btnUninstallBrowser.IsEnabled = true;
            }
            else
            {
                btnInstallBrowser.IsEnabled = true;
                btnUninstallBrowser.IsEnabled = false;
            }
        }

        private void listBoxInstalledBrowsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxInstalledBrowsers.SelectedItem != null)
            {
                btnSetDefault.IsEnabled = true;
                btnTestOpen.IsEnabled = true;
                btnSetProxyDefault.IsEnabled = true;
                btnSetProxyFilter.IsEnabled = true;
            }
            else
            {
                btnSetDefault.IsEnabled = false;
                btnTestOpen.IsEnabled = false;
                btnSetProxyDefault.IsEnabled = false;
                btnSetProxyFilter.IsEnabled = false;
            }

        }

        private void btnTestOpen_Click(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchBrowser(listBoxInstalledBrowsers.SelectedItem as Browser, @"http://www.google.de");
        }

        private void btnSetDefault_Click(object sender, RoutedEventArgs e)
        {
            (listBoxInstalledBrowsers.SelectedItem as Browser).SetDefault();

            refresh();
        }

        private void btnTestOpen_Copy_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.de");
        }

        private void btnInstallBrowser_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("install " + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser-Proxy.exe" + " " + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser.exe");
            refresh();
        }

        private void btnUninstallBrowser_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("uninstall");
            refresh();
        }

        private void btnSetProxyFilter_Click(object sender, RoutedEventArgs e)
        {
            var filters = Settings.Filter;
            var f = new PlainFilter() { RegEx = textBox.Text, AssignedBrowser = listBoxInstalledBrowsers.SelectedItem as Browser };
            filters.AddLast(f);
            f.Store();
        }

        private void btnSetProxyDefault_Click(object sender, RoutedEventArgs e)
        {
            Settings.DefaultBrowser = listBoxInstalledBrowsers.SelectedItem as Browser;
        }

        private void btnDeleteSettings_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.Delete(HardcodedValues.DATA_FOLDER, true);
            Application.Current.Shutdown();
        }
    }
}
