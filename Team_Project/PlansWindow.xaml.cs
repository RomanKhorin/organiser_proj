using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        SqlCommand cmd;
        
        MainWindow main_window = new MainWindow();
        SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Roma\\Documents\\Visual Studio 2013\\Projects\\Team_Project\\ODB\\ODB\\ODB.mdf;Integrated Security=True;Connect Timeout=30");

        public static string CurrentPlan { get; set; }
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

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plansListBox.SelectedItem == null)
                    MessageBox.Show("Select the plan you want to be delete!", "Select the plan", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                {
                    connection.Open();
                    cmd = new SqlCommand(@"delete from [Plan] where Description like '" + plansListBox.SelectedItem + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    plansListBox.Items.Remove(plansListBox.SelectedItem);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
