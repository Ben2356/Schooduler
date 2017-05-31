using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace SchedulerApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO: handle globals and update function calls
        public static string GridRowCreate { get; set; }
        public static string GridColCreate { get; set; }
        public static string ViewingWeek { get; set; }
        public static List<Course> CourseList { get; set; }
        public static List<Time> timeRange;
        public static List<string> dayList = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        public static List<int> weekViewDayList { get; set; }

        //constructor
        public MainWindow()
        {
            GridRowCreate = Properties.Settings.Default.WeekViewRowCreate;
            GridColCreate = Properties.Settings.Default.WeekViewColCreate;
            int weekOfDay = DateTime.Now.Day;
            switch((DateTime.Now.DayOfWeek).ToString())
            {                
                case "Monday":
                    weekOfDay -= 1;
                    break;
                case "Tuesday":
                    weekOfDay -= 2;
                    break;
                case "Wednesday":
                    weekOfDay -= 3;
                    break;
                case "Thursday":
                    weekOfDay -= 4;
                    break;
                case "Friday":
                    weekOfDay -= 5;
                    break;
            }
            weekViewDayList = new List<int>();
            int day = weekOfDay + 1;
            for (int i = 0; i < dayList.Count; i++)
            {
                if (weekOfDay > DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                    day = 1;
                weekViewDayList.Add(day++);
            }
            ViewingWeek = DateTime.Now.Month + "/" + weekOfDay + "/" + DateTime.Now.Year;
            CourseList = new List<Course>();
            InitializeComponent();

            //server connection is established, if there is data to retrieve do it before drawing the grid
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM courses", Login.conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            //tuple represents course_name, time_start, time_end, and tile_color
            List<Tuple<string,string,string,string>> records = new List<Tuple<string,string,string,string>>();

            //construct a tuple for each record retrieved from database
            while(reader.Read())
            {
                records.Add(new Tuple<string, string, string, string>(reader["course_name"].ToString(), reader["time_start"].ToString(), reader["time_end"].ToString(), reader["tile_color"].ToString()));
            }
            reader.Close();

            foreach(Tuple<string,string,string,string> rec in records)
            {
                string courseName = rec.Item1;
                
                //get the day entries for the corresponding course
                List<string> courseDays = new List<string>();
                cmd = new MySqlCommand("SELECT day FROM days WHERE course_id = (SELECT course_id FROM courses WHERE course_name = \"" + courseName + "\")", Login.conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    courseDays.Add(reader["day"].ToString());
                }
                reader.Close();

                //get the task entries for the corresponding course
                BindingList<Task> taskList = new BindingList<Task>();
                Task t;
                cmd = new MySqlCommand("SELECT * FROM tasks WHERE course_id = (SELECT course_id FROM courses WHERE course_name = \"" + courseName + "\")", Login.conn);
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    t = new Task((int.Parse(reader["completed"].ToString()) == 0 ? false : true), reader["assignment_name"].ToString(), Utils.convertSQLDateTime(reader["due"].ToString()), reader["notes"].ToString());
                    taskList.Add(t);
                }
                reader.Close();

                //build and add the course object
                Time start = new Time(rec.Item2);
                Time end = new Time(rec.Item3);
                Color tileColor = (Color)(ColorConverter.ConvertFromString(rec.Item4));
                Course course = new Course(courseName, start, end, taskList, courseDays, tileColor);
                CourseList.Add(course);
            }

            timeRange = GridBuilders.drawGridRowTimes(gv_weekView);
            GridBuilders.drawColDayHeaders(gv_weekView,dayList);    
            Loaded += delegate
            {
                GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList);
            };
        }

        private void exitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void exitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Login.conn.Close();
            Application.Current.Shutdown();
        }

        private void settingsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void settingsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (prevSelectedButton != null)
                prevSelectedButton.IsChecked = false;
            Settings settingsWindow = new Settings();
            if (settingsWindow.ShowDialog() == true)
            {
                timeRange = GridBuilders.drawGridRowTimes(gv_weekView);
                GridBuilders.drawColDayHeaders(gv_weekView,dayList);

                //force UI layout updates
                Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
                
                GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList); 
            }               
        }

        private void taskList_AddingNew(object sender, AddingNewEventArgs e)
        {
            //MessageBox.Show("TEST!");
        }

        private void taskList_ListChanged(object sender, ListChangedEventArgs e)
        {
           //MessageBox.Show("MODIFY TEST!");
        }

        private bool isRefCall = false;
        private ToggleButton prevSelectedButton;
        private string tileName = null;

        private void courseTile_Clicked(object sender, RoutedEventArgs e)
        {
            if(prevSelectedButton != null)
                prevSelectedButton.IsChecked = false;
            if (isRefCall)
                return;
            ToggleButton btn = sender as ToggleButton;
            BindingList<Task> list = btn.GetValue(ButtonProperties.TaskListProperty) as BindingList<Task>;
            list.AddingNew += taskList_AddingNew;
            list.ListChanged += taskList_ListChanged;
            dg_tasks.ItemsSource = list;
            toggleButtonState(btn);
            prevSelectedButton = btn;
            add_task_button.IsEnabled = true;
            tileName = btn.Content.ToString();
        }

        private void courseTile_Unclicked(object sender, RoutedEventArgs e)
        {
            if (isRefCall)
                return;
            dg_tasks.ItemsSource = null;
            ToggleButton btn = sender as ToggleButton;
            toggleButtonState(btn);
            prevSelectedButton = null;
            add_task_button.IsEnabled = false;
        }

        //toggles all the buttons in the course's state except the selected button
        private void toggleButtonState(ToggleButton btn)
        {
            int selectedCourseIndex = Utils.indexOfCourse((string)btn.Content);
            if(CourseList[selectedCourseIndex].relatedButtons == null)
                return;          
            else
            {
                isRefCall = true;
                for (int i = 0; i < CourseList[selectedCourseIndex].relatedButtons.Count; i++)
                {
                    if (CourseList[selectedCourseIndex].relatedButtons[i] == btn)
                        continue;
                    CourseList[selectedCourseIndex].relatedButtons[i].IsChecked = !CourseList[selectedCourseIndex].relatedButtons[i].IsChecked;
                }
            }        
            isRefCall = false;
        }

        

        private void addCourseButton_Click(object sender, RoutedEventArgs e)
        {
            AddCourse addCourseWindow = new AddCourse(CourseList);
            if(addCourseWindow.ShowDialog() == true)
            {
                CourseList.Add(addCourseWindow.newCourse);
                GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList);
            }
        }

        private void menu_editCourseClick(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            var contextmenu = menuitem.Parent as ContextMenu;
            ToggleButton btn = ContextMenuService.GetPlacementTarget(contextmenu) as ToggleButton;
            Course editTarget = CourseList[Utils.indexOfCourse((string)btn.Content)];

            //need to force unclick of clicked buttons
            btn.IsChecked = false;

            AddCourse courseEditWindow = new AddCourse(editTarget.CourseName, editTarget.TimeStart, editTarget.TimeEnd, editTarget.CourseDay, editTarget.TileColor, editTarget.TaskList);
            if(courseEditWindow.ShowDialog() == true)
            {
                CourseList.Add(courseEditWindow.newCourse);
                timeRange = GridBuilders.drawGridRowTimes(gv_weekView);
                GridBuilders.drawColDayHeaders(gv_weekView, dayList);
                GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList);
            }
        }

        private void menu_deleteCourseClick(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            var contextmenu = menuitem.Parent as ContextMenu;
            ToggleButton btn = ContextMenuService.GetPlacementTarget(contextmenu) as ToggleButton;
            Course deleteCourse = CourseList[Utils.indexOfCourse((string)btn.Content)];
            CourseList.Remove(deleteCourse);
            timeRange = GridBuilders.drawGridRowTimes(gv_weekView);
            GridBuilders.drawColDayHeaders(gv_weekView, dayList);
            GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList);
            dg_tasks.ItemsSource = null;
        }

        private void dg_tasks_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //MessageBox.Show("edit ending");
        }

        private void newTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AddTask newTaskWindow = new AddTask(dg_tasks.ItemsSource as BindingList<Task>, tileName);
            newTaskWindow.ShowDialog();
        }
    }

    public class FullDateToMonthDay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format("{0:MM/dd hh:mm tt}", (DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(((string)value).Length != 0)
            {
                int lengthOfMonth = ((string)value).IndexOf('/');
                int startLoc = lengthOfMonth;
                string month = ((string)value).Substring(0, startLoc++);

                int lengthOfDay = ((string)value).IndexOf(' ') - startLoc;
                bool dayFormatX = lengthOfDay == 1 ? true : false;                     
                string day = ((string)value).Substring(startLoc, lengthOfDay);

                if (dayFormatX)
                    startLoc += 2;
                else
                    startLoc += 3;
                int lengthOfHour = ((string)value).IndexOf(':') - startLoc;
                bool hourFormatX = lengthOfHour == 1 ? true : false;     
                string hour = ((string)value).Substring(startLoc,lengthOfHour);

                if (hourFormatX)
                    startLoc += 2;
                else
                    startLoc += 3;
                string min = ((string)value).Substring(startLoc, 2);

                startLoc += 3;
                string tod = ((string)value).Substring(startLoc);
                tod = tod.ToUpper();

                //MessageBox.Show(month + "/" + day + " " + hour + ":" + min + " " + tod);
                int nMonth, nDay, nHour, nMin;

                if(isValidTOD(tod) && int.TryParse(month, out nMonth) && int.TryParse(day,out nDay) && int.TryParse(hour,out nHour) && int.TryParse(min,out nMin) && isValidDateTimeFields(nMonth,nDay,nHour,nMin))
                {
                    int nYear = (DateTime.Now).Year;
                    if ((DateTime.Now).Month > nMonth)
                        nYear++;

                    //if PM is appended to a situation where there is only 1 hour digit then add 12 hours to force to PM
                    if ((tod == "PM" || tod == "pm") && nHour != 12)
                    {
                        nHour += 12;
                    }
                    return new DateTime(nYear, nMonth, nDay, nHour, nMin, 0);
                }
            }
            MessageBox.Show("Invailid date or time format!\nFORMAT: MM/DD HH:MM TT","Data Formatting Error",MessageBoxButton.OK,MessageBoxImage.Error);
            return null;
        }

        private bool isValidTOD(string tod)
        {
            return (tod == "PM" || tod == "pm" || tod == "AM" || tod == "am") && tod.Length == 2;
        }

        private bool isValidDateTimeFields(int month, int day, int hour, int min)
        {
            if (DateTime.DaysInMonth(DateTime.Now.Year, month) < day)
                return false;
            return (month > 0 && month <= 12 && day > 0 && day <= 31 && hour >= 0 && hour < 24 && min >= 0 && min <= 59);
        }
    }
}
