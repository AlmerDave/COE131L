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

        bool usernameValid = false;
        bool fnameValid = false;
        bool lnameValid = false;
        bool passValid = false;

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

            User newUser = new User();
            if (string.IsNullOrWhiteSpace(this.TextBoxName.Text) || string.IsNullOrWhiteSpace(this.TextBoxFName.Text)
                || string.IsNullOrWhiteSpace(this.TextBoxLName.Text) || string.IsNullOrWhiteSpace(this.TextBoxPassword.Password.ToString()) || string.IsNullOrWhiteSpace(this.TextBoxCPassword.Password.ToString()))
            {
                MessageBox.Show("Please provide correct information!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (usernameValid && fnameValid && lnameValid && passValid)
            {
                newUser.firstName = this.TextBoxFName.Text;
                newUser.lastName = this.TextBoxLName.Text;
                newUser.userName = this.TextBoxName.Text;
                newUser.password = this.TextBoxPassword.Password;

                Database.insertAccount(newUser);

                this.Close();
                MainWindow login = new MainWindow();
                
                login.Show();
            }
            else
            {
                MessageBox.Show("Please provide correct information!", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            //MAKE A PROMPT FOR SUCCESDSFUL LOGIN
            
        }

        private void Snackbar_Click(object sender, RoutedEventArgs e)
        {
            SnackbarUnsavedChanges.IsActive = false;
            discardChanges = true;

           
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if(string.IsNullOrWhiteSpace(this.TextBoxName.Text))
            {
                this.userMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Alert;
                this.userMaterial.Foreground = Brushes.Red;

                this.usernameValid = false;
            }
            else
            {
                this.userMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Face;
                this.userMaterial.Foreground = this.TextBoxName.BorderBrush;
                this.usernameValid = true;
            }
        }

        private void TextBoxFName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TextBoxFName.Text))
            {
                this.fnameMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Alert;
                this.fnameMaterial.Foreground = Brushes.Red;

                this.fnameValid = false;
            }
            else
            {
                this.fnameMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.PermIdentity;
                this.fnameMaterial.Foreground = this.TextBoxName.BorderBrush;
                this.fnameValid = true;
            }
        }

        private void TextBoxLName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TextBoxLName.Text))
            {
                this.lnameMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Alert;
                this.lnameMaterial.Foreground = Brushes.Red;

                this.lnameValid = false;
            }
            else
            {
                this.lnameMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.PermIdentity;
                this.lnameMaterial.Foreground = this.TextBoxName.BorderBrush;
                this.lnameValid = true;
            }
        }

        private void TextBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TextBoxPassword.Password))
            { 
                this.passMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Alert;
                this.passMaterial.Foreground = Brushes.Red;

                this.passValid = false;
            }
            else
            {
                this.passMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Lock;
                this.passMaterial.Foreground = this.TextBoxName.BorderBrush;
                this.passValid = true;
            }
        }

        private void TextBoxCPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TextBoxCPassword.Password.ToString()) || !Equals(this.TextBoxPassword.Password.ToString(), this.TextBoxCPassword.Password.ToString()))
            {
                this.confirmMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Alert;
                this.confirmMaterial.Foreground = Brushes.Red;

                this.passValid = false;
            }
            else
            {
                this.confirmMaterial.Kind = MaterialDesignThemes.Wpf.PackIconKind.Lock;
                this.confirmMaterial.Foreground = this.TextBoxName.BorderBrush;
                this.passValid = true;
            }
        }
    }
}
