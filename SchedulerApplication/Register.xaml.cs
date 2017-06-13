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
            return false;
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
            statusUsernameTextBlock.Text = "";
            statusPasswordTextBlock.Text = "";
            statusConfirmPasswordTextBlock.Text = "";
            statusEmailTextBlock.Text = "";

            //check to see if the username is valid
            if (usernameTextBox.Text.Length < 4)
            {
                statusUsernameTextBlock.Text = "Username too short!";
            }
            else if (!Regex.IsMatch(usernameTextBox.Text,"^[a-zA-Z0-9_]+$"))
            {
                statusUsernameTextBlock.Text = "Invalid characters!";
            }
            else if(usernameTaken(usernameTextBox.Text))
            {
                statusUsernameTextBlock.Text = "Username taken!";
            }

            //check to see if the password is valid
            if(passwordTextBox.Password.Length < 6)
            {
                statusPasswordTextBlock.Text = "Password too short!";
            }
            
            //validate that both the passwords are the same
            if(confirmPasswordTextBox.Password == "")
            {
                statusConfirmPasswordTextBlock.Text = "Empty password field!";
            }
            else if(passwordTextBox.Password != confirmPasswordTextBox.Password)
            {
                statusConfirmPasswordTextBlock.Text = "Passwords don't match!";
            }

            //check to see if the email address is valid
            if(!isValidEmail(emailTextBox.Text))
            {
                statusEmailTextBlock.Text = "Please enter a valid email address!";
            }

            //**obvious security flaw when creating a new user by programming in the root the account to create the new user - 
            //better way might be to send a signal to a script on the same machine as the server and the script will run the root user to build the new
            //account, a level of indirection


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
