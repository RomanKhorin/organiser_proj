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
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            okButton.IsDefault = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!string.IsNullOrEmpty(loginTextBox.Text) && !string.IsNullOrEmpty(passwordTextBox.Text) &&
                    !string.IsNullOrEmpty(nameTextBox.Text) && !string.IsNullOrEmpty(surnameTextBox.Text) &&
                        birthDatePicker.SelectedDate.HasValue &&
                            InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                {
                    SqlConnection connection = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Roma\\Documents\\Visual Studio 2013\\Projects\\Team_Project\\ODB\\ODB\\ODB.mdf;Integrated Security=True;Connect Timeout=30");
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("Insert into [User] (Login, Password, Name, Surname, Date_of_birth) values (@Login, @Password, @Name, @Surname, @Date_of_birth)", connection);
                    cmd.Parameters.Add("@Login", loginTextBox.Text);
                    cmd.Parameters.Add("@Password", passwordTextBox.Text);
                    cmd.Parameters.Add("@Name", nameTextBox.Text);
                    cmd.Parameters.Add("@Surname", surnameTextBox.Text);
                    cmd.Parameters.Add("@Date_of_birth", birthDatePicker.SelectedDate);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    this.Close();
                }
                else
                    MessageBox.Show("All the fields must be filled in (in English)!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
