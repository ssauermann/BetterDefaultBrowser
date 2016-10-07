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
        private MainWindowBind mainBind = new MainWindowBind();

        public MainWindow2()
        {
            InitializeComponent();
            //Can probably be removed
            this.DataContext = mainBind;
            browserList.ItemsSource = AllBrowsers.InstalledBrowsers;
            comboBoxBrowserSelect.ItemsSource = AllBrowsers.InstalledBrowsers;

            WinVerLabel.Content = OSVersions.getVersion().ToString();

            var win1 = new MainWindow();
            win1.Show();
        }

        private void Applybutton_Click(object sender, RoutedEventArgs e)
        {
            mainBind.saveCurrent();
        }

        private void UninstallBDBMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("uninstall");
            EnableDisableInstallationButton();
        }

        private void InstallBDBMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("install " + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser-Proxy.exe" + " " + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser.exe");
            EnableDisableInstallationButton();
        }

        /// <summary>
        /// Checks wether BDB is installed and enables or disables the install buttons
        /// </summary>
        private void EnableDisableInstallationButton()
        {
            if (AllBrowsers.IsBDBInstalled)
            {
                UninstallBDBMenuItem.IsEnabled = true;
                InstallBDBMenuItem.IsEnabled = false;
            }
            else
            {
                UninstallBDBMenuItem.IsEnabled = false;
                InstallBDBMenuItem.IsEnabled = true;
            }
        }

        private void deleteSettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BetterDefaultBrowser", true);
            Application.Current.Shutdown();
        }
    }
}
