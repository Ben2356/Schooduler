using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SchedulerApplication
{
    //course object class
    //CourseNames are unique identifiers
    public class Course
    {
        public string CourseName { get; set; }
        public Time TimeStart { get; set; }
        public Time TimeEnd { get; set; }
        public Color TileColor { get; set; }
        public List<Task> TaskList { get; set; }
        public List<string> CourseDay { get; set; }
        public List<ToggleButton> relatedButtons { get; set; }

        public Course()
        {
        }

        public Course(string name, Time timeStart, Time timeEnd, List<Task> tasks, List<string> days, Color color)
        {
            CourseName = name;
            TimeStart = timeStart;
            TimeEnd = timeEnd;
            TaskList = tasks;
            CourseDay = days;
            TileColor = color;
        }
    }
}
