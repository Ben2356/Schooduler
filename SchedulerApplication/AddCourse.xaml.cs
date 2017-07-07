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
using System.ComponentModel;

namespace SchedulerApplication
{
    /// <summary>
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourse : Window
    {

        

        private int startHour;
        public int StartHour
        {
            get
            {
                return startHour;
            }
            set
            {
                if (value > 12 || value <= 0)
                    Utils.hourValueError();
                else
                    startHour = value;
            }
        }

        private int endHour;
        public int EndHour
        {
            get
            {
                return endHour;
            }
            set
            {
                if (value > 12 || value <= 0)
                    Utils.hourValueError();
                else
                    endHour = value;
            }
        }

        private int startMin;
        public int StartMin
        {
            get
            {
                return startMin;
            }
            set
            {
                if (value >= 60)
                    Utils.minuteValueError();
                else
                    startMin = value;
            }
        }

        private int endMin;
        public int EndMin
        {
            get
            {
                return endMin;
            }
            set
            {
                if (value >= 60)
                    Utils.minuteValueError();
                else
                    endMin = value;
            }
        }

        //couse to add to the courseList
        public Course newCourse { get; protected set; }

        public string ButtonName { get; set; }
        public string WindowTitle { get; set; }

        //used to identify old course when "updating" course information
        private string courseTitle = null;

        //task list from old course
        private BindingList<Task> taskList = null;

        private List<Course> CourseList { get; }
        private List<string> todList = new List<string>() { "AM", "PM" };
        private List<string> tileColorsList = new List<string>()
            {
                "Aqua",
                "Aquamarine",
                "Blue", //
                "BlueViolet", //
                "Chartreuse",
                "Coral",
                "CornflowerBlue",
                "Crimson",
                "Cyan",
                "DarkOrange",
                "DarkSalmon",
                "DarkSeaGreen",
                "DarkTurquoise",
                "DeepPink",
                "DeepSkyBlue",
                "DodgerBlue",
                "ForestGreen",
                "Fuchsia",
                "Gold",
                "GoldenRod",
                "Green",
                "GreenYellow",
                "HotPink",
                "IndianRed",
                "Khaki",
                "Lavender",
                "LavenderBlush",
                "LawnGreen",
                "LemonChiffon",
                "LightBlue",
                "LightCoral",
                "LightCyan",
                "LightGoldenrodYellow",
                "LightGreen",
                "LightPink",
                "LightSalmon",
                "LightSeaGreen",
                "LightSkyBlue",
                "LightSteelBlue",
                "Lime",
                "LimeGreen",
                "Magenta",
                "MediumAquamarine",
                "MediumOrchid",
                "MediumPurple",
                "MediumSeaGreen",
                "MediumSlateBlue",
                "MediumSpringGreen",
                "MediumVioletRed",
                "MistyRose",
                "Moccasin",
                "NavajoWhite",
                "OliveDrab",
                "Orange",
                "OrangeRed",
                "Orchid",
                "PaleGreen",
                "PaleTurquoise",
                "PaleVioletRed",
                "PeachPuff",
                "Peru",
                "Pink",
                "Plum",
                "PowderBlue",
                "Red",
                "RosyBrown",
                "RoyalBlue",
                "Salmon",
                "SandyBrown",
                "SeaGreen",
                "Sienna",
                "Silver",
                "SkyBlue",
                "SpringGreen",
                "SteelBlue",
                "Tan",
                "Thistle",
                "Tomato",
                "Turquoise",
                "Violet",
                "Wheat",
                "Yellow",
                "YellowGreen"
            };
        
        //constructor for creating a addCourse window for a new course
        //REMOVE ARGUMENT FOR COURSELIST AND JUST REFERENCE A STATIC VARIABLE IN THE MAINWINDOW?
        public AddCourse(List<Course> courseList)
        {
            DataContext = this;
            StartHour = EndHour = 12;
            StartMin = EndMin = 0;
            ButtonName = "Add Course";
            WindowTitle = "Add New Course";
            InitializeComponent();
            BtnCancel.IsCancel = true;
            CourseList = courseList;
            cmb_startTOD.ItemsSource = todList;
            cmb_endTOD.ItemsSource = todList;
            cmb_colorPicker.ItemsSource = tileColorsList;          
        }

        //constructor that populates the settings fields with values from pre-existing course
        public AddCourse(string courseTitle, Time startTime, Time endTime, List<string> courseDays, Color courseColor, BindingList<Task> courseTasks)
        {
            DataContext = this;
            WindowTitle = "Update Existing Course";
            ButtonName = "Update Course";
            InitializeComponent();
            //
            CourseList = MainWindow.CourseList;
            //
            this.courseTitle = courseTitle;
            courseName.Text = courseTitle;
            txt_startHour.Text = startTime.Hour.ToString();
            txt_startMin.Text = startTime.Min.ToString();
            cmb_startTOD.ItemsSource = todList;
            cmb_startTOD.SelectedIndex = todList.IndexOf(startTime.TOD);
            txt_endHour.Text = endTime.Hour.ToString();
            txt_endMin.Text = endTime.Min.ToString();
            cmb_endTOD.ItemsSource = todList;
            cmb_endTOD.SelectedIndex = todList.IndexOf(endTime.TOD);
            StartHour = startTime.Hour;
            StartMin = startTime.Min;
            EndHour = endTime.Hour;
            EndMin = endTime.Min;
            taskList = courseTasks;
            for(int i = 0; i < courseDays.Count; i++)
            {
                switch(courseDays[i])
                {
                    case "Monday":
                        M.IsChecked = true;
                        break;
                    case "Tuesday":
                        T.IsChecked = true;
                        break;
                    case "Wednesday":
                        W.IsChecked = true;
                        break;
                    case "Thursday":
                        TH.IsChecked = true;
                        break;
                    case "Friday":
                        F.IsChecked = true;
                        break;
                }
            }
            cmb_colorPicker.ItemsSource = tileColorsList;
            string colorName = (typeof(Colors).GetProperties().FirstOrDefault(p => Color.AreClose(courseColor, (Color)p.GetValue(null)))).Name;
            cmb_colorPicker.SelectedIndex = tileColorsList.IndexOf(colorName);          
        }

        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addButton_click(object sender, RoutedEventArgs e)
        {
            MySqlCommand cmd;

            //if modifying a current course then just delete the old course and remove the old corresponding table entries
            if (courseTitle != null)
            {
                int index = Utils.indexOfCourse(courseTitle);
                CourseList.RemoveAt(index);

                //delete from tasks table
                cmd = new MySqlCommand("DELETE FROM tasks WHERE course_id = (SELECT course_id FROM courses WHERE course_name = " + "\"" + courseTitle + "\"))", Login.conn);

                //delete from days table
                cmd = new MySqlCommand("DELETE FROM days WHERE course_id = (SELECT course_id FROM courses WHERE course_name = " + "\"" + courseTitle + "\"))", Login.conn);

                //delete from courses table
                cmd = new MySqlCommand("DELETE FROM courses WHERE course_name = " + "\"" + courseTitle + "\"", Login.conn);
            }

            List<string> courseDays = new List<string>();
            if (M.IsChecked == true)
                courseDays.Add("Monday");
            if (T.IsChecked == true)
                courseDays.Add("Tuesday");
            if (W.IsChecked == true)
                courseDays.Add("Wednesday");
            if (TH.IsChecked == true)
                courseDays.Add("Thursday");
            if (F.IsChecked == true)
                courseDays.Add("Friday");
            if (courseName.Text == "" || cmb_startTOD.SelectedValue == null || cmb_endTOD.SelectedValue == null || courseDays.Count == 0 || cmb_colorPicker.SelectedValue == null)
            {
                Utils.unfilledError();
                return;
            }

            if(StartHour == EndHour && StartMin == EndMin && cmb_startTOD.SelectedValue == cmb_endTOD.SelectedValue)
            {
                MessageBox.Show("Start and end times are the same!", "Same Times", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //check to make sure course name is unique disregarding case
            for(int i = 0; i < CourseList.Count; i++)
            {
                if(courseName.Text.ToUpper() == CourseList[i].CourseName.ToUpper())
                {
                    MessageBox.Show("Course name already exists! If you wish to modify the course right click on the tile and select Edit.", "Duplicate Course Name", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            //make sure class is not going back in time
            if(StartHour == EndHour && cmb_startTOD.SelectedValue == cmb_endTOD.SelectedValue && StartMin > EndMin)
            {
                MessageBox.Show("Invalid start and end times!", "Invalid Times", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //look through all the courses
            for (int i = 0; i < CourseList.Count; i++)
            {
                //look through each courses days
                for(int j = 0; j < CourseList[i].CourseDay.Count; j++)
                {
                    //look through each selected day
                    for(int k = 0; k < courseDays.Count(); k++)
                    {
                        //if there is a day that is selected that matches one of the courseDays check to make sure there are no collisions
                        if(courseDays[k] == CourseList[i].CourseDay[j])
                        {
                            int mStartHour;
                            if ((string)cmb_startTOD.SelectedValue == "PM" && StartHour == 12)
                                mStartHour = StartHour + 12;
                            else if ((string)cmb_startTOD.SelectedValue == "AM" && StartHour == 12)
                                mStartHour = StartHour - 12;
                            else
                                mStartHour = StartHour;

                            int mEndHour = (EndHour + ((EndHour != 12 && (string)cmb_endTOD.SelectedValue == "PM") ? 12 : 0));

                            //bottom half overlapping
                            if (mStartHour < CourseList[i].TimeEnd.MilitaryTimeHour && mEndHour > CourseList[i].TimeEnd.MilitaryTimeHour)
                            {
                                printConflictingCourseMessage(CourseList[i].CourseName, CourseList[i].CourseDay[j]);
                                return;
                            }

                            //bottom half is in the same hour the minutes need to be checked for overlapping
                            if(mStartHour == CourseList[i].TimeEnd.MilitaryTimeHour)
                            {
                                if(StartMin <= CourseList[i].TimeEnd.Min)
                                {
                                    printConflictingCourseMessage(CourseList[i].CourseName, CourseList[i].CourseDay[j]);
                                    return;
                                }
                            }

                            //top half overlapping
                            if(mEndHour > CourseList[i].TimeStart.MilitaryTimeHour && mStartHour < CourseList[i].TimeStart.MilitaryTimeHour)
                            {
                                printConflictingCourseMessage(CourseList[i].CourseName, CourseList[i].CourseDay[j]);
                                return;
                            }

                            //top half is in the same hour, the minutes need to be checked for overlapping
                            if(mEndHour == CourseList[i].TimeStart.MilitaryTimeHour)
                            {
                                if(EndMin >= CourseList[i].TimeEnd.Min)
                                {
                                    printConflictingCourseMessage(CourseList[i].CourseName, CourseList[i].CourseDay[j]);
                                    return;
                                }
                            }

                            //complete overlapping
                            if((mStartHour == CourseList[i].TimeStart.MilitaryTimeHour || mEndHour == CourseList[i].TimeEnd.MilitaryTimeHour) || (mStartHour >= CourseList[i].TimeStart.MilitaryTimeHour && mEndHour <= CourseList[i].TimeEnd.MilitaryTimeHour))
                            {
                                printConflictingCourseMessage(CourseList[i].CourseName, CourseList[i].CourseDay[j]);
                                return;
                            }
                        }
                    }
                }
            }

            Color color = (Color)ColorConverter.ConvertFromString((string)cmb_colorPicker.SelectedValue);
            Time start = new Time(StartHour, StartMin, (string)cmb_startTOD.SelectedValue);
            Time end = new Time(EndHour, EndMin, (string)cmb_endTOD.SelectedValue);
            newCourse = new Course(courseName.Text, start, end, taskList == null ? new BindingList<Task>() : taskList, courseDays, color);

            //push the course to the courses table
            cmd = new MySqlCommand("INSERT INTO courses(course_id,course_name,time_start,time_end,tile_color,user_id) VALUES (NULL," + "\"" + courseName.Text + "\",\"" + start.ToString() + "\",\"" + end.ToString() + "\",\"" + color.ToString() + "\"," + Login.userId + ")", Login.conn);
            cmd.ExecuteNonQuery();

            uint courseId = Utils.retrieveCourseId(courseName.Text);

            //push days to the days table
            for (int i = 0; i < courseDays.Count; i++)
            {
                cmd = new MySqlCommand("INSERT INTO days(day_id,course_id,day) VALUES (NULL," + courseId + ",\"" + courseDays[i] + "\")", Login.conn);
                cmd.ExecuteNonQuery();
            }

            //if this operation is an update then need to add the old taskList
            if (courseTitle != null)
            {
                for(int i = 0; i < taskList.Count; i++)
                {
                    string dateTimeStr = string.Format("{0yyyy-MM-dd HH:mm:ss}", taskList[i].Due);
                    cmd = new MySqlCommand("INSERT INTO tasks(task_id,course_id,completed,assignment_name,due,notes) VALUES (NULL, " + courseId + "," + (taskList[i].Completed ? 1 : 0) + ",\"" + taskList[i].Assignment + "\",\"" + dateTimeStr + "\",\"" + taskList[i].Notes + "\")", Login.conn);
                    cmd.ExecuteNonQuery();
                }
            }
                        
            DialogResult = true;
        }

        private void printConflictingCourseMessage(string name, string day)
        {
            MessageBox.Show(string.Format("Conflicting course {0} on {1} detected!", name, day), "Conflict Detected", MessageBoxButton.OK, MessageBoxImage.Error);
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
