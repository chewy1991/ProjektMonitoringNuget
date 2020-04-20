﻿using ProjektMonitoringNuget.ViewModel;
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

namespace ProjektMonitoringNuget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MonitoringViewModel monitor = new MonitoringViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = monitor;
        }

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {
            monitor.Select();            
            //txtBox.ItemsSource = monitor.Result;
            //txtBox.Text = monitor.Result;
        }        
    }
}