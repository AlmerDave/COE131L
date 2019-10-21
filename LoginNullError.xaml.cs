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

namespace COE131L
{
    /// <summary>
    /// Interaction logic for LoginNullError.xaml
    /// </summary>
    public partial class LoginNullError : Window
    {
        public LoginNullError()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
