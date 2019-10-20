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
using System.Data;

namespace COE131L
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        User loggedUser = new User();
        public Main(User loguser)
        {
            InitializeComponent();
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            loggedUser = loguser;

            string fName = loggedUser.firstName + " " + loggedUser.lastName;
            this.nameBox.Text = fName;
            
            DataTable itemTable = new DataTable();
            itemTable = Database.getRecord();
            this.itemGrid.ItemsSource = itemTable.DefaultView;
            

            
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible; 
        }

        private void search_textChanged(object sender, TextChangedEventArgs e)
        {
            var search = Search_textBox.Text;

            DataTable itemTable = new DataTable();
            itemTable = Database.searchRecord(search);
            this.itemGrid.ItemsSource = itemTable.DefaultView;
        }
    }
}
