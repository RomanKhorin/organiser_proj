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

namespace Team_Project
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

        private void okDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
