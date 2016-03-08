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
        PlanDescription plan;
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
        private void App_Closing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Shuts down the application after pressing
        /// the "Exit" button
        /// </summary>
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Adds the plan to the DataBase and plans listbox after filling
        /// the plan description in PlanDescription Window
        /// </summary>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                plan = new PlanDescription();
                plan.descriptionTextBox.Text = "";
                bool? result = plan.ShowDialog();

                if (result.Value == true)
                {
                    if (String.IsNullOrEmpty(plan.descriptionTextBox.Text) == false &&
                        InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        connection.Open();
                        cmd = new SqlCommand("Insert into [Plan] (User_login, Description, IsCompleted) values (@User_login, @Description, 'N')", connection);
                        cmd.Parameters.AddWithValue("@User_login", MainWindow.CurrentUser);
                        cmd.Parameters.AddWithValue("@Description", plan.descriptionTextBox.Text);
                        cmd.ExecuteNonQuery();
                        connection.Close();

                        plansListBox.Items.Add(plan.descriptionTextBox.Text);
                    }
                    else
                        MessageBox.Show("Description can't be empty or written in Russian (Write in English)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        /// <summary>
        /// Deletes selected plan from the DataBase
        /// and plans listbox
        /// </summary>
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plansListBox.SelectedItem == null)
                    MessageBox.Show("Select the plan you want to delete!", "Select the plan", MessageBoxButton.OK, MessageBoxImage.Information);
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

        /// <summary>
        /// Allows the user to edit the selected plan after pressing the "Edit" button,
        /// if the start plan was written with mistakes
        /// </summary>
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plansListBox.SelectedItem != null)
                {
                    plan = new PlanDescription();

                    plan.descriptionTextBox.Text = plansListBox.SelectedItem.ToString();
                    plan.ShowDialog();

                    if (string.IsNullOrEmpty(plan.descriptionTextBox.Text) == false &&
                        InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        connection.Open();
                        cmd = new SqlCommand(@"update [Plan] set Description='" + plan.descriptionTextBox.Text + "' where Description like '" + plansListBox.SelectedItem.ToString() + "'", connection);
                        cmd.ExecuteNonQuery();

                        plansListBox.Items.Clear();
                        var plans = new SqlCommand(("Select * from [Plan] where User_login like '" + MainWindow.CurrentUser + "' and IsCompleted='N'"), connection);
                        SqlDataReader reader = plans.ExecuteReader();
                        while (reader.Read())
                            plansListBox.Items.Add(reader.GetString(1));

                        connection.Close();
                    }
                    else
                        MessageBox.Show("The description field can't be empty or written in Russian!", "Description", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Select the plan you want to edit!", "Select the plan", MessageBoxButton.OK, MessageBoxImage.Information);
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
