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

            //establishes a root connection to the mySQL database
            //**obvious security flaw when creating a new user or validating an existing user by using the root account within the application - 
            //better way might be to send a signal to a script on the same machine as the server and the script will run the root user to build the new
            //account or validate an existing one, a level of indirection

            //this is the temporary solution
            string sqlPassword = "";
            string connStr = "server=localhost;user=root;database=schoodulerTest;port=3306;password=" + sqlPassword;
            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Root user is unable to connect to server!", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case 1045:
                        MessageBox.Show("Invalid root username/password", "Creditentials", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        //only want to shutdown application if alt-f4 or window close button is clicked
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == null)
            {
                conn.Close();
                Application.Current.Shutdown();
            } 
        }

        //validate the users exists and the creditentials are correct before proceeding on to main screen
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            statusMessage.Text = "";

            //force last minute sync to keep both the visible password and hidden password the same
            if(showPassCheckBox.IsChecked == true)
            {
                passwordTextBox.Password = readablePasswordBox.Text;
            }
            else
            {
                readablePasswordBox.Text = passwordTextBox.Password;
            }

            //check to see that the username and password fields are populated
            if(usernameTextBox.Text == "" || passwordTextBox.Password == "")
            {
                statusMessage.Text = "Please enter your username and password!";
                return;
            }

            //will check the encrypted password is the same as one stored in DB given the DB's password key and IV
            MySqlCommand cmd = new MySqlCommand("SELECT password FROM users WHERE username = \"" + usernameTextBox.Text + "\"", conn);
            MySqlDataReader r = cmd.ExecuteReader();
            if(r.Read())
            {
                string encryptedPassComparison = r[0].ToString();
                byte[] encryptedBase, iv, key;
                Utils.parseEncryptedString(encryptedPassComparison, out key, out iv, out encryptedBase);
                string encryptedPass = Utils.encryptString(passwordTextBox.Password, key, iv);
                if(encryptedPass == encryptedPassComparison)
                {
                    r.Close();
                    DialogResult = true;
                    return;
                }
                else
                {
                    statusMessage.Text = "Invalid username or password!";
                }
            }
            //user doesn't exist
            else
            {
                statusMessage.Text = "Invalid username or password!";
            }
            r.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.ShowDialog();
        }

        private void showPassCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            readablePasswordBox.Text = passwordTextBox.Password;
            readablePasswordBox.Visibility = Visibility.Visible;
            passwordTextBox.Visibility = Visibility.Hidden;
        }

        private void showPassCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordTextBox.Password = readablePasswordBox.Text;
            readablePasswordBox.Visibility = Visibility.Hidden;
            passwordTextBox.Visibility = Visibility.Visible;
        }
    }
}
