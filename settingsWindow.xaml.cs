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
        Main wind;

        List<string> dayList = new List<string>();
        List<string> monthList = new List<string>();
        public settingsWindow()
        {
            InitializeComponent(); 
            this.comboType.ItemsSource =  Database.getItemtypes();
            
        }
        public settingsWindow(int userId, Main mWin)
        {
            wind = mWin;
            InitializeComponent();
            this.comboType.ItemsSource = Database.getItemtypes();
            loggedUser = userId;
            //ADD COMBO BOX ELEMENTS 
           
            dayList.Add("01");dayList.Add("02");dayList.Add("03");
            dayList.Add("04");dayList.Add("05");dayList.Add("06");
            dayList.Add("07");dayList.Add("08");dayList.Add("09");
            dayList.Add("10");dayList.Add("11");dayList.Add("12");
            dayList.Add("13");dayList.Add("14");dayList.Add("15");
            dayList.Add("16");dayList.Add("17");dayList.Add("18");
            dayList.Add("19");dayList.Add("20");dayList.Add("21");
            dayList.Add("22");dayList.Add("23");dayList.Add("24");
            dayList.Add("25");dayList.Add("26");dayList.Add("27");
            dayList.Add("28");dayList.Add("29");dayList.Add("30");
            dayList.Add("31");

            comboDay.ItemsSource = dayList;

            monthList.Add("01");monthList.Add("02");
            monthList.Add("03"); monthList.Add("04");
            monthList.Add("05");monthList.Add("06");
            monthList.Add("07");monthList.Add("08");
            monthList.Add("09");monthList.Add("10");
            monthList.Add("11"); monthList.Add("12");
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


        private void comboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<item> modellist = new List<item>();
            modellist = Database.getModel(this.comboType.SelectedItem.ToString());

            if(comboModel.HasItems)
            {
                comboModel.Items.Clear();
                foreach (item p in modellist)
                {
                    this.comboModel.Items.Add(p.model);
                }
            }
            else
            {
                foreach (item p in modellist)
                {
                    this.comboModel.Items.Add(p.model);
                }
            }
            
            //this.comboModel.ItemsSource = Database.getModel(this.comboType.SelectedItem.ToString());

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
                    string date = this.comboMonth.SelectedItem.ToString() + "/" + this.comboDay.SelectedItem.ToString() + "/" + this.textboxYear.Text;
                    DateTime deldate = DateTime.ParseExact(date, "MM/dd/yyyy", null);
                    DateTime datedecom = deldate.AddMonths(Int32.Parse(this.textblockMonth.Text));
                    datedecom = datedecom.AddYears(Int32.Parse(this.textblockYear.Text));

                    newItem.serialNumber = this.textboxSerial.Text;

                    List<item> modelist = new List<item>();
                    modelist = Database.storeRecord();

                foreach (item p in modelist)
                {
                    if (p.model == comboModel.Text)
                    {
                        newItem.itemType = p.itemType;
                    }
                }

                    newItem.addedby = loggedUser;
                    newItem.statusId = this.itemStat;
                    newItem.conditionId = this.condId;
                    newItem.supplier = this.textboxSupplier.Text;
                    newItem.datedelivered = deldate.ToString("MM/dd/yyyy");
                    newItem.datedecomm = datedecom.ToString("MM/dd/yyyy");
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
