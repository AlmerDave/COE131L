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
    /// Interaction logic for BreakageWindow.xaml
    /// </summary>
    public partial class BreakageWindow : Window
    {
        int loggedid;
        DateTime currentDate;
        Main win;
        public BreakageWindow()
        {
            InitializeComponent();
            
        }

        public BreakageWindow(int userid, Main mWin)
        {
            InitializeComponent();
            currentDate = System.DateTime.Today;
            this.loggedid = userid;
            this.textboxDate.Text = currentDate.ToString("dd/MM/yyyy");
            win = mWin;
        }

        private void buttonExecute_Click(object sender, RoutedEventArgs e)
        {
            string serialNum =  this.textboxSerial.Text;
            int studNum;// = Int32.Parse(this.textboxStudent.Text);
            int recby = this.loggedid;
            string daterec = this.textboxDate.Text;
            if (this.radioAdd.IsChecked == true)
            {

                studNum = Int32.Parse(this.textboxStudent.Text);
                if (Database.breakageAdd(serialNum, recby, studNum, daterec) == true)
                {
                    MessageBox.Show("Item is already on the list!", "Already Existing Item", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    
                    MessageBox.Show("Broken Item is saved to the list.", "Item Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                }

            }
            else if (this.radioRemove.IsChecked == true)
            {
                //item is removedd
                if(Database.breakageRemove(serialNum) == true)
                {
                    MessageBox.Show("Item is removed from the breakage list.", "Item Removed", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Item is not existing in the breakage list.", "Item not in the list", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textboxDate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
