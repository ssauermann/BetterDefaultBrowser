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
using BetterDefaultBrowser.Lib.Gateways;
using BetterDefaultBrowser.Lib.Models;
using BetterDefaultBrowser.ViewModels;
using MahApps.Metro.Controls;

namespace BetterDefaultBrowser
{
    /// <summary>
    /// Logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();


            // Create the ViewModel to which 
            // the main window binds.
            string path = @".\mysettings.xml";
            var viewModel = new MainWindowViewModel(path);

            // When the ViewModel asks to be closed, 
            // close the window.
            EventHandler handler = null;
            handler = delegate
            {
                viewModel.RequestClose -= handler;
                Close();
            };
            viewModel.RequestClose += handler;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            DataContext = viewModel;
        }
    }
}
