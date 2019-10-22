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

        public SignUp()
        {
           

            InitializeComponent();



        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxFName.Text) || string.IsNullOrWhiteSpace(TextBoxLName.Text) || string.IsNullOrWhiteSpace(TextBoxName.Text) 
                || string.IsNullOrWhiteSpace(TextBoxPassword.Password) || string.IsNullOrWhiteSpace(TextBoxCPassword.Password) || string.IsNullOrWhiteSpace(NickName_textBox.Text))
            {
                MessageBox.Show("Please Fill ALL Blanks", "INSUFFICIENT INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else if(TextBoxCPassword.Password != TextBoxPassword.Password)
            {
                MessageBox.Show("Password does not match", "MISMATCH", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
            else
            {
                SnackbarUnsavedChanges.IsActive = true;
            }
            
        }

        private void Snackbar_Click(object sender, RoutedEventArgs e)
        {

            SnackbarUnsavedChanges.IsActive = false;
            User newUser = new User();
            newUser.firstName = this.TextBoxFName.Text;
            newUser.lastName = this.TextBoxLName.Text;
            newUser.userName = this.TextBoxName.Text;
            newUser.password = this.TextBoxPassword.Password;
            newUser.nickname = this.NickName_textBox.Text;


            Database.insertAccount(newUser);
            //MAKE A PROMPT FOR SUCCESDSFUL LOGIN
            MainWindow login = new MainWindow();
            this.Close();
            login.Show();

        }

        private void BackButtonSign_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            this.Close();
            login.Show();
        }
    }
}
