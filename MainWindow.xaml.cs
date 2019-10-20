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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace COE131L
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Database databaseObject = new Database();
        
        public MainWindow()
        {
            InitializeComponent();
            Password_TextBox.IsEnabled = false;
            Error_textBox.Visibility = Visibility.Hidden;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            User loguser = new User();

            if(Database.accessUser(this.Email_TextBox.Text,this.Password_TextBox.Password.ToString(),ref loguser) == true)
            {
                //LOGIN SUCESS 
                Main main = new Main(loguser);
                this.Close();
                main.Show();
                

            }
            else
            {
                //LOGIN FAIL
                Error_textBox.Visibility = Visibility.Visible;

            }

            
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SignUp su = new SignUp();
            this.Close();
            su.Show();
        }

        private void email_textChanged(object sender, TextChangedEventArgs e)
        {
            Error_textBox.Visibility = Visibility.Hidden;
            Password_TextBox.IsEnabled = true;
            if(string.IsNullOrWhiteSpace(Email_TextBox.Text))
            {
                Password_TextBox.IsEnabled = false;
            }
        }
    }
}
