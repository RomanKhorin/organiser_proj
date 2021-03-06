﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class PlansWindow : Window
    {
        /// <summary>
        /// Instance of the SqlCommand class
        /// </summary>
        SqlCommand cmd;

        /// <summary>
        /// Instance of the PlanDescription class
        /// </summary>
        PlanDescription plan;

        /// <summary>
        /// Constructor for the PlanWindow class
        /// </summary>
        public PlansWindow()
        {
            InitializeComponent();
            this.Closing += this.App_Closing;
        }

        /// <summary>
        /// Current Id for selected plan in plans listbox
        /// </summary>
        public int CurrentPlanId { get; set; }

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
                plan.DeadLine_datepicker.SelectedDate = DateTime.Today;
                bool? result = plan.ShowDialog();

                if (result.Value == true)
                {
                    if (String.IsNullOrEmpty(plan.descriptionTextBox.Text) == false &&
                            plan.DeadLine_datepicker.SelectedDate != null &&
                                InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                    {
                        MainWindow.Connection.Open();
                        cmd = new SqlCommand("Insert into [Plan] (User_login, Description, IsCompleted, DeadLine, Date) values (@User_login, @Description, 'N', @DeadLine, @Date)", MainWindow.Connection);
                        cmd.Parameters.AddWithValue("@User_login", MainWindow.CurrentUser);
                        cmd.Parameters.AddWithValue("@Description", plan.descriptionTextBox.Text);
                        cmd.Parameters.AddWithValue("@DeadLine", plan.DeadLine_datepicker.SelectedDate);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        MainWindow.Connection.Close();

                        plansListBox.Items.Add(plan.descriptionTextBox.Text + ": (Till " + plan.DeadLine_datepicker.SelectedDate.Value.ToShortDateString() + ")");
                    }
                    else
                        MessageBox.Show("Description can't be empty or written in Russian (Write in English)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
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
                    MainWindow.Connection.Open();
                    cmd = new SqlCommand(@"delete from [Plan] where Id like " + CurrentPlanId, MainWindow.Connection);
                    cmd.ExecuteNonQuery();
                    MainWindow.Connection.Close();

                    MainWindow.GetTasks(MainWindow.Connection, plansListBox, "N");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
        }

        /// <summary>
        /// Allows the user to edit the selected plan after pressing 
        /// the "Edit" button
        /// </summary>
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plansListBox.SelectedItem != null)
                {
                    plan = new PlanDescription();
                    plan.descriptionTextBox.Text = plansListBox.SelectedItem.ToString().Split(':')[0];
                    plan.DeadLine_datepicker.SelectedDate = GetDeadLineDate(MainWindow.Connection, CurrentPlanId);

                    bool? result = plan.ShowDialog();

                    if (result.Value == true)
                    {
                        if (string.IsNullOrEmpty(plan.descriptionTextBox.Text) == false &&
                                InputLanguageManager.Current.CurrentInputLanguage.Name == "en-US")
                        {
                            MainWindow.Connection.Open();
                            cmd = new SqlCommand(@"update [Plan] set Description='" + plan.descriptionTextBox.Text + "', "
                                + "DeadLine='" + plan.DeadLine_datepicker.SelectedDate + "' where Id like " + CurrentPlanId, MainWindow.Connection);
                            cmd.ExecuteNonQuery();
                            MainWindow.Connection.Close();

                            MainWindow.GetTasks(MainWindow.Connection, plansListBox, "N");
                        }
                        else
                            MessageBox.Show("The description field can't be empty or written in Russian!", "Description", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    MessageBox.Show("Select the plan you want to edit!", "Select the plan", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
        }

        /// <summary>
        /// Marks the selected task as completed after
        /// pressing the "Completed" button
        /// </summary>
        private void comlpetedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plansListBox.SelectedItem != null)
                {
                    MainWindow.Connection.Open();
                    cmd = new SqlCommand(@"update [Plan] set IsCompleted='Y' where Id like " + CurrentPlanId, MainWindow.Connection);
                    cmd.ExecuteNonQuery();
                    MainWindow.Connection.Close();

                    MainWindow.GetTasks(MainWindow.Connection, plansListBox, "N");
                }
                else
                    MessageBox.Show("Select the plan you want to be completed!", "Select the plan", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
        }

        /// <summary>
        /// Shows completed tasks after pressing
        /// the "Show completed tasks" button
        /// </summary>
        private void competedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                completedTasksListBox.Items.Clear();
                MainWindow.GetTasks(MainWindow.Connection, completedTasksListBox, "Y");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
        }

        /// <summary>
        /// Saves uncompleted plans to the text file after
        /// pressing the "Save" button
        /// </summary>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plansListBox.Items.Count > 0)
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.DefaultExt = ".txt";
                    saveFile.FileName = "Plan";
                    saveFile.Filter = "Document (.txt) | *txt";

                    bool? result = saveFile.ShowDialog();
                    if (result.Value)
                    {
                        StreamWriter sw = new StreamWriter(new FileStream(saveFile.FileName, FileMode.Create));
                        foreach (var item in plansListBox.Items)
                        {
                            sw.WriteLine("* " + item.ToString());
                        }
                        sw.Close();
                        MessageBox.Show("Plans successfully saved!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    MessageBox.Show("You can not save empty plan list!", "Empty plan list", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Gets the deadline date for selected plan in plans listbox
        /// </summary>
        private DateTime GetDeadLineDate(SqlConnection connection, int Id)
        {
            DateTime deadline = new DateTime();
            try
            {
                connection.Open();
                var time = new SqlCommand(("Select DeadLine from [Plan] where Id like " + CurrentPlanId + " and IsCompleted like 'N'"), connection);
                SqlDataReader reader = time.ExecuteReader();

                while (reader.Read())
                    deadline = reader.GetDateTime(0);
                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }

            return deadline;
        }

        /// <summary>
        /// Gets the Id for the selected plan in plans listbox
        /// </summary>
        private int GetId(SqlConnection connection, string description)
        {
            int Id = 0;
            try
            {
                connection.Open();
                var id_query = new SqlCommand(("Select Id from [Plan] where User_login like '" + MainWindow.CurrentUser + "'and Description like '" + description + "' and IsCompleted like 'N'"), connection);
                SqlDataReader reader = id_query.ExecuteReader();

                while (reader.Read())
                    Id = reader.GetInt32(0);
                connection.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainWindow.Connection.Close();
            }

            return Id;
        }

        /// <summary>
        /// Sets the Id for the selected plan in plans listbox
        /// </summary>
        private void plansListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (plansListBox.SelectedItem == null)
                CurrentPlanId = 0;
            else
                CurrentPlanId = GetId(MainWindow.Connection, plansListBox.SelectedItem.ToString().Split(':')[0]);
        }
    }
}
