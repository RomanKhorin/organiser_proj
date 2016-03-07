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
using System.Windows.Shapes;

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class PlansWindow : Window
    {
        public PlansWindow()
        {
            InitializeComponent();
            this.Closing += this.App_Closing;
        }

        /// <summary>
        /// Closing event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Closing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            PlanDescription descriptionWindow = new PlanDescription();
            descriptionWindow.ShowDialog();
        }

    }
}
