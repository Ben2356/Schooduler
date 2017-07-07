using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddTask.xaml
    /// </summary>
    public partial class AddTask : Window
    {
        private int hour;
        public int DueHour
        {
            get
            {
                return hour;
            }
            set
            {
                if (value > 12 || value <= 0)
                    Utils.hourValueError();
                else
                    hour = value;
            }
        }

        private int min;
        public int DueMin
        {
            get
            {
                return min;
            }
            set
            {
                if (value >= 60)
                    Utils.minuteValueError();
                else
                    min = value;
            }
        }

        private BindingList<Task> taskList;
        private string courseName;
        private List<string> todList = new List<string>() { "AM", "PM" };

        public AddTask(BindingList<Task> taskList, string courseName)
        {
            DueHour = 12;
            InitializeComponent();
            BtnCancel.IsCancel = true;
            cmb_TOD.ItemsSource = todList;
            this.taskList = taskList;
            this.courseName = courseName;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime? nDue = cal_dueDate.SelectedDate;

            //check to make sure that no required fields are left blank
            if(assignmentName.Text == "" || cmb_TOD.SelectedValue == null || nDue == null)
            {
                Utils.unfilledError();
                return;
            }
        
            DateTime due = new DateTime(((DateTime)nDue).Year, ((DateTime)nDue).Month, ((DateTime)nDue).Day, Utils.toMilitaryHour(DueHour, (string)cmb_TOD.SelectedValue), DueMin, 0);

            //make sure that the due date selected isn't in the past
            if (due.CompareTo(DateTime.Now) < 0)
            {
                MessageBox.Show("Due date is a passed date! Please select a current or later date.", "Past Date", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            taskList.Add(new Task(false, assignmentName.Text, due, notes.Text));

            //push the new task to the database
            MySqlCommand cmd = new MySqlCommand("INSERT INTO tasks (task_id,course_id,completed,assignment_name,due,notes) VALUES (NULL," + Utils.retrieveCourseId(courseName) + ",0,\"" + assignmentName.Text + "\",\"" + Utils.buildSQLDateTime(due) + "\",\"" + notes.Text + "\")",Login.conn);
            cmd.ExecuteNonQuery();
            DialogResult = true;
        }

        private void validateTimeChar(object sender, TextCompositionEventArgs e)
        {
            int t;
            e.Handled = !int.TryParse(e.Text, out t);
        }

        private void selectTextBox(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                tb.SelectAll();
            }
        }

        private void selectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }
    }
}
