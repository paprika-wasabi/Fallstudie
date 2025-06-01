﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AOT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();  
            MainContent.Content = new HomePage();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomePage();
        }

        private void PendingButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PendingPage();
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Dashboard();
        }
    }
}