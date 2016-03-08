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

        /// <summary>
        /// After pressing the "OK" buton adds user to the DataBase
        /// so he could login
        /// </summary>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginTextBox.Text) && !string.IsNullOrEmpty(passwordTextBox.Text) &&
                    !string.IsNullOrEmpty(nameTextBox.Text) && !string.IsNullOrEmpty(surnameTextBox.Text) &&
                        birthDatePicker.SelectedDate.HasValue &&
                            InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                {
                    MainWindow.Connection.Open();
                    SqlCommand cmd = new SqlCommand("Insert into [User] (Login, Password, Name, Surname, Date_of_birth) values (@Login, @Password, @Name, @Surname, @Date_of_birth)", MainWindow.Connection);
                    cmd.Parameters.AddWithValue("@Login", loginTextBox.Text);
                    cmd.Parameters.AddWithValue("@Password", passwordTextBox.Text);
                    cmd.Parameters.AddWithValue("@Name", nameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Surname", surnameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Date_of_birth", birthDatePicker.SelectedDate);
                    cmd.ExecuteNonQuery();
                    MainWindow.Connection.Close();

                    this.Close();
                }
                else
                    MessageBox.Show("All the fields must be filled in (in English)!", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
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
        /// After pressing the "Cancel" button
        /// exits from the registration window
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
