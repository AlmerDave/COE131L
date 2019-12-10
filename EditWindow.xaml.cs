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

        Main win;
        public EditWindow()
        {
            InitializeComponent();
        }

        public EditWindow(int userId, Main mwin)
        {
        

            InitializeComponent();
            loggeduser = userId;
            this.textboxUser.IsEnabled = false;

            win = mwin;
        }

        private void buttonSearch_Click(object sender, RoutedEventArgs e)
        {
            string sernum =  this.textboxSerial.Text ;
            item editItem = new item();
            List<string> arrList = new List<string>();

            if(checkEdit.IsChecked == true && string.IsNullOrWhiteSpace(this.textboxSerial.Text)== true) // CANNOT HAPPEN
            {
                MessageBox.Show("Search for a serial number first before editing information!","Cannot edit",MessageBoxButton.OK,MessageBoxImage.Exclamation);

            }
            else
            {
                if(Database.searchitem(sernum,ref editItem, ref arrList) == true)//ITEM IS EDITED
                {
                    List<item> itmList = new List<item>();

                    this.comboType.ItemsSource=Database.getItemtypes();
               
                    this.textboxUser.Text = Database.getUsername(editItem.serialNumber);
                    this.textboxSupplier.Text = editItem.supplier;
                    this.comboType.Text = arrList[0];
                    this.comboStatus.Text = arrList[1];
                    this.comboCondition.Text = arrList[2];
                    this.textboxUser.Text = arrList[3];
                    string[] datedel = editItem.datedelivered.Split('/');
         
                    this.ComboMonth.Text = datedel[0];
                    this.ComboDay.Text = datedel[1];
                    this.textboxYear.Text = datedel[2];

                    string[] datedecom = editItem.datedecomm.Split('/');

                    this.ComboMonth_decom.Text = datedecom[0];
                    this.ComboDay_decom.Text = datedecom[1];
                    this.textboxYear_C.Text = datedecom[2];


                    itmList = Database.getModel(this.comboType.Text);
                    if (comboModel.HasItems)
                    {
                        comboModel.Items.Clear();
                        foreach (item p in itmList)
                        {
                            this.comboModel.Items.Add(p.model);
                        }
                    }
                    else
                    {
                        foreach (item p in itmList)
                        {
                            this.comboModel.Items.Add(p.model);
                        }
                    }
                    comboModel.Text = editItem.model;
                }
                else // ITEM DOES NOT EXIST
                {
                    MessageBox.Show("The entered item is not found in the record!", "Item not found", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        private void comboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<item> itmList = new List<item>();
            itmList = Database.getModel(this.comboType.SelectedItem.ToString());
            if (comboModel.HasItems)
            {
                comboModel.Items.Clear();
                foreach (item p in itmList)
                {
                    this.comboModel.Items.Add(p.model);
                }
            }
            else
            {
                foreach (item p in itmList)
                {
                    this.comboModel.Items.Add(p.model);
                }
            }

        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            item editItem = new item();
            

            editItem.serialNumber = this.textboxSerial.Text;

            List<item> modelist = new List<item>();
            modelist = Database.storeRecord();

            foreach (item p in modelist)
            {
                if (p.model == comboModel.Text)
                {
                    editItem.itemType = p.itemType;
                }
            }
            editItem.addedby = loggeduser;
            editItem.supplier = this.textboxSupplier.Text;
           
            editItem.statusId = this.comboStatus.SelectedIndex + 1;
           
            editItem.conditionId = this.comboCondition.SelectedIndex + 1;

            //MONTH DAY YEAR (DEFAULT)
            string datedel = ComboMonth.Text + "/" + ComboDay.Text + "/" + textboxYear.Text;
            editItem.datedelivered = datedel;

            string datedecom = ComboMonth_decom.Text + "/" + ComboDay_decom.Text + "/" + textboxYear_C.Text;
            editItem.datedecomm = datedecom;

            Database.UpdateEdit(editItem);
            MessageBox.Show("Item is saved to the record!", "Item Saved", MessageBoxButton.OK, MessageBoxImage.Information);
           
            win.loadDatagrid();

        }
    }
}
