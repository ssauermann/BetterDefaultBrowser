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
        private DefaultBrowser DB = new DefaultBrowser();

        public MainWindow2()
        {
            InitializeComponent();
            this.DataContext = mainBind;
            browserList.ItemsSource = DB.Browsers;
            comboBoxBrowserSelect.ItemsSource = DB.Browsers;
            
        }


        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            foreach (var browser in DB.Browsers)
            {
                browser.update();
            }
        }
    }
}
