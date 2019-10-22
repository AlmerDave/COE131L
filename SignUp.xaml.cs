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
using System.IO;
using System.Data.SQLite;


namespace COE131L
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
        Database databaseObject = new Database();
        bool discardChanges;
        public SignUp()
        {
           

            InitializeComponent();
            discardChanges = false;


        }

        private void SignUp_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          
            if (discardChanges == true)
            {
                e.Cancel = false;
                MainWindow login = new MainWindow();
                login.Show();
            }
            else
            {
                SnackbarUnsavedChanges.IsActive = true;
                e.Cancel = true;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {

            //string query = "INSERT INTO UserInformation('UserName', 'FirstName', 'LastName', 'Password')" +
            //                "VALUES(@username, @firstname, @lastname, @Password)";
            //using (SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection))
            //{
            //    databaseObject.myConnection.Open();
            //    myCommand.Parameters.AddWithValue("@username", TextBoxName.Text);
            //    myCommand.Parameters.AddWithValue("@firstname", TextBoxFName.Text);
            //    myCommand.Parameters.AddWithValue("@lastname", TextBoxLName.Text);
            //    myCommand.Parameters.AddWithValue("@Password", TextBoxPassword.Password);

            //    myCommand.ExecuteNonQuery();
            //    databaseObject.myConnection.Close();
            //}
            User newUser = new User();
            newUser.firstName = this.TextBoxFName.Text;
            newUser.lastName = this.TextBoxLName.Text;
            newUser.userName = this.TextBoxName.Text;
            newUser.password = this.TextBoxPassword.Password;
           
            Database.insertAccount(newUser);
            //MAKE A PROMPT FOR SUCCESDSFUL LOGIN
            MainWindow login = new MainWindow();
            this.Hide();
            login.Show();
        }

        private void Snackbar_Click(object sender, RoutedEventArgs e)
        {
            SnackbarUnsavedChanges.IsActive = false;
            discardChanges = true;
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            this.Hide();
            login.Show();
        }
    }
}
