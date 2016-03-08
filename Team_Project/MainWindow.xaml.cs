using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Team_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PlansWindow plan_window;
        SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Roma\\Documents\\Visual Studio 2013\\Projects\\Team_Project\\ODB\\ODB\\ODB.mdf;Integrated Security=True;Connect Timeout=30");

        public static string CurrentUser { get; set; }
        public static SqlConnection Connection { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Connection = connection;
            this.Closing += App_Closing;
        }
        private void App_Closing(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
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
                        GetUncompletedTasks(connection);
                        plan_window.Show();
                        this.Close();
                    }
                    else
                        MessageBox.Show("Invalid login or password", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Check your login and password or sign up", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// Queries the DataBase, gets the uncompleted user's tasks
        /// and adds them to the plans listbox
        /// </summary>
        public void GetUncompletedTasks(SqlConnection connection)
        {
            try
            {
                connection.Open();
                var plans = new SqlCommand(("Select * from [Plan] where User_login like '" + CurrentUser + "' and IsCompleted='N'"), connection);
                SqlDataReader reader = plans.ExecuteReader();

                while (reader.Read())
                    plan_window.plansListBox.Items.Add(reader.GetString(1));
                
                connection.Close();
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
        /// Gets the current user name from the login textbox
        /// </summary>
        private void loginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurrentUser = loginTextBox.Text;
        }
    }
}
