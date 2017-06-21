using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        //returns whether the username is already present in the database
        private bool usernameTaken(string user)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE username = \"" + user + "\"", Login.conn);
            MySqlDataReader r = cmd.ExecuteReader();
            if (r.Read())
            {
                r.Close();
                return true;
            }
            else
            {
                r.Close();
                return false;
            }
        }

        //GOING TO NEED A MUCH MORE EXTENSIVE WAY TO VALIDATE EMAIL ADDRESSES
        private bool isValidEmail(string email)
        {
            if (email == "")
                return false;
            try
            {
                MailAddress m = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            //force last minute sync to keep both the visible password and hidden password the same
            if (showPassCheckBox.IsChecked == true)
            {
                passwordTextBox.Password = readablePasswordBox.Text;
            }
            else
            {
                readablePasswordBox.Text = passwordTextBox.Password;
            }

            bool isInvalid = false;
            statusUsernameTextBlock.Text = "";
            statusPasswordTextBlock.Text = "";
            statusConfirmPasswordTextBlock.Text = "";
            statusEmailTextBlock.Text = "";

            //check to see if the username is valid
            if (usernameTextBox.Text.Length < 4)
            {
                statusUsernameTextBlock.Text = "Username too short!";
                isInvalid = true;
            }
            else if (!Regex.IsMatch(usernameTextBox.Text,"^[a-zA-Z0-9_]+$"))
            {
                statusUsernameTextBlock.Text = "Invalid characters!";
                isInvalid = true;
            }

            //check to see if the password is valid
            if(passwordTextBox.Password.Length < 6)
            {
                statusPasswordTextBlock.Text = "Password too short!";
                isInvalid = true;
            }
            
            //validate that both the passwords are the same
            if(confirmPasswordTextBox.Password == "")
            {
                statusConfirmPasswordTextBlock.Text = "Empty password field!";
                isInvalid = true;
            }
            else if(passwordTextBox.Password != confirmPasswordTextBox.Password)
            {
                statusConfirmPasswordTextBlock.Text = "Passwords don't match!";
                isInvalid = true;
            }

            //check to see if the email address is valid
            if(!isValidEmail(emailTextBox.Text))
            {
                statusEmailTextBlock.Text = "Please enter a valid email address!";
                isInvalid = true;
            }

            if(!isInvalid)
            {

                //check to see that the username isn't already in use
                if (usernameTaken(usernameTextBox.Text))
                {
                    statusUsernameTextBlock.Text = "Username taken!";
                    return;
                }

                //encrypt user passwords before storing them into the users database table
                byte[] key = Utils.generateEncryptedBytes();
                byte[] iv = Utils.generateEncryptedBytes();
                string encryptedPass = Utils.encryptString(passwordTextBox.Password,key,iv);

                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (user_id,username,password,email) VALUES (NULL,\"" + usernameTextBox.Text + "\",\"" + encryptedPass + "\",\"" + emailTextBox.Text + "\")", Login.conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("User account created successfully!", "User Creation Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
