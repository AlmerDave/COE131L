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
using Tulpep.NotificationWindow;
using System.Collections.ObjectModel;
using System.Media;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.IO;
using ExcelLibrary.SpreadSheet;
using ExcelLibrary.CompoundDocumentFormat;

namespace COE131L
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    /// 
    
    public partial class Main : Window
    {
        ObservableCollection<item> datamodel = new ObservableCollection<item>();
        User loggedUser = new User();
        
        private SoundPlayer notif;
        public Main(bool sw)
        {
            InitializeComponent();
            SettingsButton.IsEnabled = false;
            BreakageButton.IsEnabled = false;
            Edit_button.IsEnabled = false;
            notif = new SoundPlayer("notif.wav");



            string fName = loggedUser.firstName + " " + loggedUser.lastName;
            this.nameBox.Text = fName;

            loadDatagrid();

            loadnotif();
        }
        public Main(User loguser)
        {
 
            InitializeComponent();
            
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            loggedUser = loguser;
            notif = new SoundPlayer("notif.wav");



            string fName = loggedUser.firstName + " " + loggedUser.lastName;
            this.nameBox.Text = fName;

            loadDatagrid();


               
       
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
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

            if(Breakage_checkBox.IsChecked == true)
            {
                DataTable itemTable = new DataTable();
                itemTable = Database.searchBreakageRecord(search);
                this.itemGrid.ItemsSource = itemTable.DefaultView;
            }
            else
            {
                DataTable itemTable = new DataTable();
                itemTable = Database.searchRecord(search);
                this.itemGrid.ItemsSource = itemTable.DefaultView;
            }

           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            settingsWindow setwin = new settingsWindow(loggedUser.id,this);
            setwin.Show();
        }
        
        
        public void loadDatagrid()
        {

            if (Breakage_checkBox.IsChecked == true)
            {
                DataTable itemTable = new DataTable();
                itemTable = Database.getBreakageRecord();
                this.itemGrid.ItemsSource = itemTable.DefaultView;
            }
            else
            {
                DataTable itemTable = new DataTable();
                itemTable = Database.getRecord();
                this.itemGrid.ItemsSource = itemTable.DefaultView;
            }

            loadnotif();

        }
        public void loadBreakageGrid()
        {

            DataTable itemTable = new DataTable();
            itemTable = Database.getBreakageRecord();
            this.itemGrid.ItemsSource = itemTable.DefaultView;
            
        }
        public void loadnotif()
        {
            DateTime mydate = DateTime.Now;
            mydate = mydate.AddDays(2);
            string strdate = mydate.ToShortDateString();
            string[] arrdate = strdate.Split('/');
            
            if(int.Parse(arrdate[0]) < 10)
            {
                arrdate[0] = "0" + arrdate[0];
            }
            if(int.Parse(arrdate[1]) < 10)
            {
                arrdate[1] = "0" + arrdate[1];
            }

            string aheadDate = arrdate[0] + '/' + arrdate[1] + '/' + arrdate[2];
            List<item> notiftable = new List<item>();
            notiftable = Database.getSerial(aheadDate);

            foreach (item p in notiftable)
            {
                this.Serial_list.Items.Add(p.serialNumber);
            }

            if (Serial_list.Items.IsEmpty)
            {
                ;
            }
            else
            {
                Notification_button.Content = new PackIcon { Kind = PackIconKind.NotificationsActive, Width = 30, Height = 30 };
                Notification_button.Foreground = Brushes.Red;
            }

            if (Notification_button.Foreground == Brushes.Red)
            {
                notif.Play();
                PopupNotifier popup = new PopupNotifier();
                popup.Image = Properties.Resources.warn;
                popup.TitleText = "WARNING";
                popup.ContentText = "There are items to be decomissioned";

                popup.Popup();

            }
        }

        private void breakage_clicked(object sender, RoutedEventArgs e)
        {
            if (Breakage_checkBox.IsChecked == true)
            {
                loadBreakageGrid();
            }
            else
                loadDatagrid();
        }

        private void Notif_clicked(object sender, RoutedEventArgs e)
        {
            if (Serial_list.Items.IsEmpty)
            {
                ;
            }
            else
            {
                Notification_button.Content = new PackIcon { Kind = PackIconKind.Notifications, Width = 30, Height = 30 };
                Notification_button.Foreground = Brushes.White;
            }
            
            if (Notification_button.Foreground == Brushes.White)
            {
                notif.Play();
                PopupNotifier popup = new PopupNotifier();
                popup.Image = Properties.Resources.warn;
                popup.TitleText = "WARNING";
                popup.ContentText = "There are items to be decomissioned";

                popup.Popup();
            }

           


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BreakageWindow breakwin = new BreakageWindow(loggedUser.id,this);
            breakwin.Show();
        }

        private void EditButon_clicked(object sender, RoutedEventArgs e)
        {
            EditWindow edtiwin = new EditWindow(loggedUser.id, this);
            edtiwin.Show();
        }

        private void Print_Button_Clicked(object sender, RoutedEventArgs e)
        {
           

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Save",
                Filter = "CSV File (.csv) | *.csv",
                FileName = ""
            
            };

            if (sfd.ShowDialog() == true)
            {
                

                DataTable dt = new DataTable();
                dt = Database.getRecord();
                 Database.ExcelConvert();
                //dt.ToCSV(sfd.FileName);
                MessageBox.Show("Succesfully Export to CSV", "SUCCESSFULL", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        public static void sample()
        {
            //create new xls file 
            string file = "newdoc.xls";
            Workbook workbook = new Workbook();
            Worksheet worksheet = new Worksheet("First Sheet");
            worksheet.Cells[0, 1] = new Cell((short)1);
            worksheet.Cells[2, 0] = new Cell(9999999);
            worksheet.Cells[3, 3] = new Cell((decimal)3.45);
            worksheet.Cells[2, 2] = new Cell("Text string");
            worksheet.Cells[2, 4] = new Cell("Second string");
            worksheet.Cells[4, 0] = new Cell(32764.5, "#,##0.00");
            worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY-MM-DD");
            worksheet.Cells.ColumnWidth[0, 1] = 3000;
            workbook.Worksheets.Add(worksheet);
            workbook.Save(file);

            // open xls file 
            Workbook book = Workbook.Load(file);
            Worksheet sheet = book.Worksheets[0];

            /*
            // traverse cells 
            foreach (Pair, Cell > cell in sheet.Cells)
            {
                dgvCells[cell.Left.Right, cell.Left.Left].Value = cell.Right.Value;
            }
            */

            // traverse rows by Index 
            for (int rowIndex = sheet.Cells.FirstRowIndex; rowIndex <= sheet.Cells.LastRowIndex; rowIndex++)
            {
                Row row = sheet.Cells.GetRow(rowIndex);
                for (int colIndex = row.FirstColIndex; colIndex <= row.LastColIndex; colIndex++)
                {
                    Cell cell = row.GetCell(colIndex);

                }
            }
        }


    }
}
