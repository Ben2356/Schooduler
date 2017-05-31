using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SchedulerApplication
{
    public static class Utils
    {
        public static void parseTimeString(string input, out int hour, out int min, out string tod)
        {
            int colonLoc = input.IndexOf(':');
            int spaceLoc = input.IndexOf(' ');
            hour = int.Parse(input.Substring(0, colonLoc));
            min = int.Parse(input.Substring(colonLoc + 1, spaceLoc - colonLoc));
            tod = input.Substring(spaceLoc + 1);
        }

        public static void parseTimeString(string input, out int hour, out int min, out string tod, out int militaryHour)
        {
            int colonLoc = input.IndexOf(':');
            int spaceLoc = input.IndexOf(' ');
            hour = int.Parse(input.Substring(0, colonLoc));
            min = int.Parse(input.Substring(colonLoc + 1, spaceLoc - colonLoc));
            tod = input.Substring(spaceLoc + 1);

            //cases that time is not 12 and is PM then need to add 12 to military hour
            if (tod == "PM" && hour != 12)
                militaryHour = hour + 12;

            //casses that time is 12 AM then need to set military hour to 0
            else if (tod == "AM" && hour == 12)
                militaryHour = hour - 12;
            
            //all other times that are AM need nothing added to the military hour
            else
                militaryHour = hour;
        }

        public static int getMilitaryHour(string input)
        {
            int colonLoc = input.IndexOf(':');
            int hour = int.Parse(input.Substring(0, colonLoc));
            int spaceLoc = input.IndexOf(' ');
            string tod = input.Substring(spaceLoc + 1);
            if (tod == "PM" && hour != 12)
                return (hour += 12);
            else if (tod == "AM" && hour == 12)
                return 0;
            else
                return hour;
        }

        //returns the index of a specific course name within a List<Course> object or if the specified course doesn't exist -1 is returned
        public static int indexOfCourse(string courseName)
        {
            for (int i = 0; i < MainWindow.CourseList.Count; i++)
            {
                if (MainWindow.CourseList[i].CourseName == courseName)
                    return i;
            }
            return -1;
        }
        
        //error message for invalid hour values
        public static void hourValueError()
        {
            MessageBox.Show("Invalid Hour Value!\nPlease enter an hour that is between 1 and 12 inclusive.", "Invalid Hour Value", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //error message for invalid min values
        public static void minuteValueError()
        {
            MessageBox.Show("Invalid Minute Value!\nPlease enter a minute that is between 1 and 59 inclusive.", "Invalid Minute Value", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //error message for unfilled fields remaining in a form
        public static void unfilledError()
        {
            MessageBox.Show("Please fill in all fields before preceding!", "Unfilled Fields", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //converts an hour to military time hour given the tod
        public static int toMilitaryHour(int hour, string tod)
        {
            //cases that time is not 12 and is PM then need to add 12 to military hour
            if (tod == "PM" && hour != 12)
                return hour + 12;

            //cases that time is 12 AM then need to set military hour to 0
            else if (tod == "AM" && hour == 12)
                return hour - 12;

            //all other times that are AM need nothing added to the military hour
            else
                return hour;
        }

        //parses datetime strings from the mysql database that are formatted MM/DD/YYYY HH:MM:SS TT and builds the DateTime object
        public static DateTime convertSQLDateTime(string str)
        {
            int start = 0;
            int index = str.IndexOf('/', start);
            int length = index;
            int month = int.Parse(str.Substring(0, length));
            start += length+1;
            index = str.IndexOf('/', start);
            length = index - start;
            int day = int.Parse(str.Substring(start, length));
            start += length+1;
            int year = int.Parse(str.Substring(start, 4));
            start += 5;
            index = str.IndexOf(':', start);
            length = index - start;
            int hour = int.Parse(str.Substring(start, length));
            start += length+1;
            int min = int.Parse(str.Substring(start, 2));
            string tod = str.Substring(str.Length - 2, 2);
            int mHour = toMilitaryHour(hour, tod);
            return new DateTime(year, month, day, mHour, min, 0);
        }

        //builds the string required to build a mysql datetime type formatted as YYYY-MM-DD HH:MM:SS from a DateTime.toString()
        public static string buildSQLDateTime(DateTime dt)
        {
            return dt.Year + "-" + padDateTimeElements(dt.Month) + "-" + padDateTimeElements(dt.Day) + " " + padDateTimeElements(dt.Hour) + ":" + padDateTimeElements(dt.Minute) + ":" + padDateTimeElements(dt.Second);
        }

        //helper function for buildSQLDateTime that pads single digit numbers with an extra 0 in front of the digit
        private static string padDateTimeElements(int elem)
        {
            if (elem < 10)
                return "0" + elem;
            else
                return elem.ToString();
        }
    }
}
