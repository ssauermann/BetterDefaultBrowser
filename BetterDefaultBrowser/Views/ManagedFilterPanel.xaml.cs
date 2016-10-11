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

namespace BetterDefaultBrowser.Views
{
    /// <summary>
    /// Interaktionslogik für ManagedFilterPanel.xaml
    /// </summary>
    public partial class ManagedFilterPanel : UserControl
    {
        public ManagedFilterPanel()
        {
            InitializeComponent();
            if (BrowserComboBox.Items.Count > 0)
            {
                BrowserComboBox.SelectedItem = BrowserComboBox.Items[0];
            }
        }
    }
}
