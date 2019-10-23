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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        int loggeduser;
        public EditWindow()
        {
            InitializeComponent();
        }

        public EditWindow(int userId)
        {
            InitializeComponent();
            loggeduser = userId;
            this.comboType.ItemsSource = Database.getItemtypes();
            
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            int sernum = Int32.Parse(this.textboxSerial.Text);
            item editItem = new item();

            if(checkEdit.IsChecked == true && string.IsNullOrWhiteSpace(this.textboxSerial.Text)== true) // CANNOT HAPPEN
            {
                MessageBox.Show("Search for a serial number first before editing information!","Cannot edit",MessageBoxButton.OK,MessageBoxImage.Exclamation);

            }
            else
            {
                if(Database.searchitem(sernum,ref editItem) == true)//ITEM IS EDITED
                {
                    this.textboxUser.Text = Database.getUsername(editItem.serialNumber);
                    this.textblockSupplier.Text = editItem.supplier;
                   // this.comboType.Text = 

                }
                else // ITEM DOES NOT EXIST
                {

                }
            }

        }

        private void comboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.comboModel.ItemsSource = Database.getModel(this.comboType.SelectedItem.ToString());
        }
    }
}
