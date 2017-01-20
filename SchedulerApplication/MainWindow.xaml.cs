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
        public List<Course> CourseList { get; set; }
        public List<Time> timeRange;
        public List<string> dayList = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
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

        public bool isRefCall = false;
        public ToggleButton prevSelectedButton;

        private void courseTile_Clicked(object sender, RoutedEventArgs e)
        {
            if(prevSelectedButton != null)
                prevSelectedButton.IsChecked = false;
            if (isRefCall)
                return;
            ToggleButton btn = sender as ToggleButton;
            List<Task> list = btn.GetValue(ButtonProperties.TaskListProperty) as List<Task>;
            dg_tasks.ItemsSource = list;
            toggleButtonState(btn);
            prevSelectedButton = btn;
        }

        private void courseTile_Unclicked(object sender, RoutedEventArgs e)
        {
            if (isRefCall)
                return;
            dg_tasks.ItemsSource = null;
            ToggleButton btn = sender as ToggleButton;
            toggleButtonState(btn);
            prevSelectedButton = null;
        }

        //toggles all the buttons in the course's state except the selected button
        private void toggleButtonState(ToggleButton btn)
        {
            int selectedCourseIndex = indexOfCourse((string)btn.Content);
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

        //returns the index of a specific course name within a List<Course> object or if the specified course doesn't exist -1 is returned
        private int indexOfCourse(string courseName)
        {
            for(int i = 0; i < CourseList.Count; i++)
            {
                if (CourseList[i].CourseName == courseName)
                    return i;
            }
            return -1;
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
            Course editTarget = CourseList[indexOfCourse((string)btn.Content)];
            AddCourse courseEditWindow = new AddCourse(editTarget.CourseName, editTarget.TimeStart, editTarget.TimeEnd, editTarget.CourseDay, editTarget.TileColor);
            if(courseEditWindow.ShowDialog() == true)
            {
                GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList);
            }
        }

        private void menu_deleteCourseClick(object sender, RoutedEventArgs e)
        {
            var menuitem = sender as MenuItem;
            var contextmenu = menuitem.Parent as ContextMenu;
            ToggleButton btn = ContextMenuService.GetPlacementTarget(contextmenu) as ToggleButton;
            Course deleteCourse = CourseList[indexOfCourse((string)btn.Content)];
            CourseList.Remove(deleteCourse);
            timeRange = GridBuilders.drawGridRowTimes(gv_weekView);
            GridBuilders.drawColDayHeaders(gv_weekView, dayList);
            GridBuilders.drawCourses(gv_weekView, CourseList, timeRange, dayList);
            dg_tasks.ItemsSource = null;
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
