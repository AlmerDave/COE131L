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
    /// Interaction logic for settingsWindow.xaml
    /// </summary>
    public partial class settingsWindow : Window
    {
        public settingsWindow()
        {
            InitializeComponent();
        }

        private void ButtonExecutetype_Click(object sender, RoutedEventArgs e)
        {
            string typeName = this.textboxName.Text;
            string typeModel = this.textboxModel.Text;

            if (this.addRadio.IsChecked == true)
            {
                if (Database.addnewtype(typeName, typeModel) == true)
                {
                    //MEANS THAT THE INSERT FAILED MAKEA PROMPT OR MESSAGE
                }
                else
                {
                    //MEANS THAT THE INSERT FAILED MAKE A PROMPT OR MESSAGE SUCCESSFUL INSERT
                }
            }
            else if(this.removeRadio.IsChecked == true)
            {
                if(Database.removetype(typeName,typeModel) == false)
                {
                    //REMOVING OF THE TYPE IS NOT SUCCESSFUL MAKEA PROMOT TO THE USER
                }
                else
                {

                }
            }

        }
    }
}
