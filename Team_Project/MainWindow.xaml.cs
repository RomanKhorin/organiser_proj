using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
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

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Instance of the PlanWindow class
        /// </summary>
        PlansWindow plan_window;
        private SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\" + WindowsIdentity.GetCurrent().Name.Split('\\')[1] + "\\Documents\\Visual Studio 2013\\Projects\\Team_Project\\ODB\\ODB\\ODB.mdf;Integrated Security=True;Connect Timeout=30");

        /// <summary>
        /// Current User in application
        /// </summary>
        public static string CurrentUser { get; set; }

        /// <summary>
        /// Current connection string to SQL DataBase
        /// </summary>
        public static SqlConnection Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Connection = connection;
        }

        /// <summary>
        /// Opens the registration window so user
        /// could sign up
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.ShowDialog();
        }

        /// <summary>
        /// After pressing the "Sign In" button checks if the user's 
        /// login and password exist in DataBase and after that
        /// may open the new plans window
        /// </summary>
        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(loginTextBox.Text) == false &&
                    String.IsNullOrEmpty(passwordTextBox.Password.ToString()) == false)
                {
                    plan_window = new PlansWindow();

                    connection.Open();
                    var validation = new SqlDataAdapter(("select count(*) from [User] where Login like '" + CurrentUser + "' " + "and Password like '" + passwordTextBox.Password.ToString() + "'"), connection);
                    DataTable dt = new DataTable();

                    validation.Fill(dt);
                    connection.Close();

                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        GetTasks(connection, plan_window.plansListBox, "N");
                        plan_window.Show();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Invalid login or password. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Check your login and password or sign up", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
        }

        /// <summary>
        /// Queries the DataBase, gets the uncompleted user's tasks
        /// and adds them to the plans listbox
        /// </summary>
        public static void GetTasks(SqlConnection connection, ListBox listbox, string status)
        {
            try
            {
                listbox.Items.Clear();
                connection.Open();
                var plans = new SqlCommand(("Select * from [Plan] where User_login like '" + CurrentUser + "' and IsCompleted like '" + status + "'"), connection);
                SqlDataReader reader = plans.ExecuteReader();

                while (reader.Read())
                    listbox.Items.Add(reader.GetString(1) + ": (Till " + reader.GetDateTime(3).ToShortDateString() + ")");

                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                connection.Close();
            }
        }

        /// <summary>
        /// Gets the current user name from the login textbox
        /// </summary>
        private void loginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurrentUser = loginTextBox.Text;
        }
    }
}
