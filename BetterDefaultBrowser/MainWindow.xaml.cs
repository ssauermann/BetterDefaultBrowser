﻿using System;
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
using BetterDefaultBrowserLib;
using System.Runtime.InteropServices;

namespace BetterDefaultBrowser
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DefaultBrowser dB = new DefaultBrowser();
        Settings settings = new Settings();
        public MainWindow()
        {
            InitializeComponent();
            refresh();
        }

        public void refresh()
        {
            dB = new DefaultBrowser();
            labelDefaultBrowser.Content = dB.GetDefault().Name;
            listBoxInstalledBrowsers.Items.Clear();
            foreach (var browser in dB.Browsers)
            {
                listBoxInstalledBrowsers.Items.Add(browser);
            }


            if (dB.isInstalled())
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
            if (listBoxInstalledBrowsers.SelectedItem != null) {
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
            (listBoxInstalledBrowsers.SelectedItem as Browser).StartWithWebsite(@"http://www.google.de");
        }

        private void btnSetDefault_Click(object sender, RoutedEventArgs e)
        {
            dB.SetDefault((listBoxInstalledBrowsers.SelectedItem as Browser));

            refresh();
        }

        private void btnTestOpen_Copy_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.google.de");
        }

        private void btnInstallBrowser_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("install " + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser-Proxy.exe" + " " + AppDomain.CurrentDomain.BaseDirectory + "BetterDefaultBrowser.exe");
            btnInstallBrowser.IsEnabled = false;
            btnUninstallBrowser.IsEnabled = true;
            refresh();
        }

        private void btnUninstallBrowser_Click(object sender, RoutedEventArgs e)
        {
            Helper.startHelper("uninstall");
            btnInstallBrowser.IsEnabled = true;
            btnUninstallBrowser.IsEnabled = false;
            refresh();
        }

        private void btnSetProxyFilter_Click(object sender, RoutedEventArgs e)
        {
            var filters = settings.Filter;
            filters.AddLast(new Filter(textBox.Text, listBoxInstalledBrowsers.SelectedItem as Browser));
            settings.Filter = filters;
        }

        private void btnSetProxyDefault_Click(object sender, RoutedEventArgs e)
        {
            settings.DefaultBrowser = listBoxInstalledBrowsers.SelectedItem as Browser;
        }

        private void btnDeleteSettings_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\BetterDefaultBrowser",true);
            Application.Current.Shutdown();
        }
    }
}
