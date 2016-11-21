using MaterialDesignThemes.Wpf;
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

namespace BetterDefaultBrowser.Views
{
    /// <summary>
    /// Interaktionslogik für OuterWindow.xaml
    /// </summary>
    public partial class OuterWindow : Window
    {
        private PackIcon maximizeIcon = new PackIcon { Kind = PackIconKind.WindowMaximize };
        private PackIcon restoreIcon = new PackIcon { Kind = PackIconKind.WindowRestore };
        public OuterWindow()
        {
            InitializeComponent();
        }
        private void Drag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CanClose(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CanMinimize(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CanMaximize(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
        private void MinimizeHandler(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void MaximizeHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                SystemCommands.MaximizeWindow(this);
            }
            else
            {
                SystemCommands.RestoreWindow(this);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                ButtonMaximizeRestore.Content = maximizeIcon;
            }
            else
            {
                ButtonMaximizeRestore.Content = restoreIcon;
            }
        }
    }
}
