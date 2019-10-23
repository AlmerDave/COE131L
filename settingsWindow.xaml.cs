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
using System.Collections.ObjectModel;
namespace COE131L
{
    /// <summary>
    /// Interaction logic for settingsWindow.xaml
    /// </summary>
    /// 
    
    public partial class settingsWindow : Window
    {
        
        int itemStat;
        int condId;
        int loggedUser;
  

        List<string> dayList = new List<string>();
        List<string> monthList = new List<string>();
        public settingsWindow()
        {
            InitializeComponent();
            this.comboType.ItemsSource = Database.getItemtypes();
            
        }
        public settingsWindow(int userId,Main mWin)
        {
            int dayVal = 1;
            int monthVal = 1;
            InitializeComponent();
            this.comboType.ItemsSource = Database.getItemtypes();
            loggedUser = userId;
            //ADD COMBO BOX ELEMENTS 
            for (; dayVal <= 31;dayVal++ )
            {
                dayList.Add(dayVal.ToString());

            }
            comboDay.ItemsSource = dayList;

            while(monthVal <= 12)
            {
                monthList.Add(monthVal.ToString());
                monthVal++;
            }
            this.comboMonth.ItemsSource = monthList;

        }

        private void ButtonExecutetype_Click(object sender, RoutedEventArgs e)
        {
            string typeName = this.textboxName.Text;
            string typeModel = this.textboxModel.Text;

            //ITEM TYPE ADD MODE
            if (this.addRadio.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(typeName) == false || string.IsNullOrWhiteSpace(typeModel) == false)
                {
                    if (Database.addnewtype(typeName, typeModel) == true)
                    {
                        //MEANS THAT THE INSERT FAILED MAKEA PROMPT OR MESSAGE
                        MessageBox.Show("Insert of new type failed! Please try again.", "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                       //MEANS THAT THE INSERT SUCCESSFUL MAKE A PROMPT OR MESSAGE SUCCESSFUL INSERT
                       MessageBox.Show("Insert of new type success!", "Successful Insert", MessageBoxButton.OK, MessageBoxImage.Information);
                       this.comboType.ItemsSource = Database.getItemtypes();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter all required information!","Insufficient input",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }//ITEM TYPE REMOVE MODE
            else if (this.removeRadio.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(typeName) == false && string.IsNullOrWhiteSpace(typeModel) == false)
                {
                    if (Database.removetype(typeName, typeModel) == false)
                    {
                        //REMOVING OF THE TYPE IS NOT SUCCESSFUL MAKEA PROMOT TO THE USER
                        MessageBox.Show("Removal of a new type failed! Please try again.", "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("Removal of a new type success!", "Successful removal", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.comboType.ItemsSource = Database.getItemtypes();
                    }
                }
                else
                {
                    MessageBox.Show("Input all required information!","Insufficient Information",MessageBoxButton.OK,MessageBoxImage.Information);
                }
            }
           else
           {
                MessageBox.Show("Please select a mode!", "Select Mode", MessageBoxButton.OK, MessageBoxImage.Exclamation);    
           }
            
        
        }

        private void typeRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void comboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.comboModel.ItemsSource = Database.getModel(this.comboType.SelectedItem.ToString());
        }

        private void buttonEquipExec_Click(object sender, RoutedEventArgs e)
        {
            if(this.radioGood.IsChecked == true)
            {
                itemStat = 1;
            }
            else if(this.radioDecom.IsChecked == true)
            {
                itemStat = 2;
            }

            if(this.radioGoodCon.IsChecked == true)
            {
                condId = 1;
            }
            else if(this.radioBadCon.IsChecked == true)
            {
                condId = 3;
            }
            else if(this.radioRepCon.IsChecked == true)
            {
                condId = 2;
            }

            //ADD SELECTED
            if (this.radioEqauipadd.IsChecked == true)
            {
                
                    item newItem = new item();
                    string date = this.comboDay.SelectedItem.ToString() + "/" + this.comboMonth.SelectedItem.ToString() + "/" + this.textboxYear.Text;
                    DateTime deldate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                    DateTime datedecom = deldate.AddMonths(Int32.Parse(this.textblockMonth.Text));
                    datedecom = datedecom.AddYears(Int32.Parse(this.textblockYear.Text));

                    newItem.serialNumber = Int32.Parse(this.textboxSerial.Text);
                    newItem.itemType = this.comboType.SelectedIndex + 1;
                    newItem.addedby = loggedUser;
                    newItem.statusId = this.itemStat;
                    newItem.conditionId = this.condId;
                    newItem.supplier = this.textboxSupplier.Text;
                    newItem.datedelivered = deldate.ToString("dd/MM/yyyy");
                    newItem.datedecomm = datedecom.ToString("dd/MM/yyyy");
                    newItem.model = this.comboModel.SelectedItem.ToString();

                    if (Database.addItem(newItem) == true) //FAILED TO ADD ITEM TO THE RECORD 
                    {
                        MessageBox.Show("Item already exists in the record! Please enter other information.", "Existing item", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.textboxModel.Clear();
                        this.textboxName.Clear();
                        this.addRadio.IsChecked = false;
                        this.removeRadio.IsChecked = false;
                    }
                    else //SUCCESSFULLY ADDED
                    {
                        MessageBox.Show("Item is added to the record!", "Item added", MessageBoxButton.OK, MessageBoxImage.Information);
                        wind.loadDatagrid();
                        //CLEAR ALL THE CONTENT OF THE EQUIPMENT PART
                        this.textboxModel.Clear();
                        this.textboxName.Clear();
                        this.addRadio.IsChecked = false;
                        this.removeRadio.IsChecked = false;

                    }
                    
               
            }
            //remove selected
            else if(this.raduiEquiprem.IsChecked == true)
            {

                    if (Database.removeItem(Int32.Parse(this.textboxSerial.Text)) == true)//ITEM IS REMOVED 
                    {
                        MessageBox.Show("Item is successfully removed from the record.", "Item Removed", MessageBoxButton.OK, MessageBoxImage.Information);
                        wind.loadDatagrid();
                    }
                    else
                    {
                        MessageBox.Show("Item failed to be removed from the record.", "Removal Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                
                
            }
            // none selected
            else
            {
                MessageBox.Show("Please select a mode to execute.", "Incomplete input", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
