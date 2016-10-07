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

            var win1 = new MainWindow();
            win1.Show();
        }

        /// <summary>
        /// If refresh buttonis clicked the dropdown menu for browserselection and the list of browser should be updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (var browser in AllBrowsers.InstalledBrowsers)
            {
                browser.update();
                browserList.ItemsSource = null;
                browserList.ItemsSource = AllBrowsers.InstalledBrowsers;
                comboBoxBrowserSelect.ItemsSource = null;
                comboBoxBrowserSelect.ItemsSource = AllBrowsers.InstalledBrowsers;
            }
        }

        private void Applybutton_Click(object sender, RoutedEventArgs e)
        {
            mainBind.saveCurrent();
        }
    }
}
