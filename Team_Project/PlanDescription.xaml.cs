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

namespace Plan_Organiser
{
    /// <summary>
    /// Interaction logic for PlanDescription.xaml
    /// </summary>
    public partial class PlanDescription : Window
    {
        public PlanDescription()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the PlansDescription Window
        /// </summary>
        private void okDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
