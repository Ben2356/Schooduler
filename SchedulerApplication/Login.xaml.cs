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

using MySql.Data.MySqlClient;

namespace SchedulerApplication
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    //responsible for establishing a connection to the mysql server so that data can be pulled from it to populate MainWindow
    public partial class Login : Window
    {
        public static MySqlConnection conn;

        public Login()
        {
            Closing += Window_Closing;
            InitializeComponent();
        }

        //only want to shutdown application if alt-f4 or window close button is clicked
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == null)
                Application.Current.Shutdown();
        }

        //attempt to create a connection to the mysql server if it succeeds then move on to main window
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //temporarily bypass entire user login system for dev purposes
            string sqlPassword = "";
            string connStr = "server=localhost;user=root;database=schoodulerTest;port=3306;password="+sqlPassword;
            bool validLogin = true;
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
            } catch(MySqlException ex)
            {
                validLogin = false;
                switch(ex.Number)
                {
                    case 0:
                        MessageBox.Show("Unable to connect to server!", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    //temporary until a system is place to handle invalid user credentials
                    case 1045:
                        MessageBox.Show("INVALID USERNAME/PASSWORD", "CREDITENTIALS", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            if(validLogin)
                DialogResult = true;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.ShowDialog();
        }
    }
}
