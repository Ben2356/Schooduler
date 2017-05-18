using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerApplication
{
    //class for storing time type elements
    public class Time
    {

        private int hour;
        private int min;
        private string tod;
        private int militaryTimeHour;
        
        public int Hour
        {
            get { return hour; }
            set
            {
                bool isUpdating = false;
                hour = value;
                if (hour > 12)
                    hour -= 12;
                if (value >= 12 && tod == "AM" && militaryTimeHour != 12)
                {
                    tod = "PM";
                    isUpdating = true;
                }             
                else if (value >= 12 && tod == "PM" && militaryTimeHour != 12)
                {
                    tod = "AM";
                    isUpdating = true;
                }
                militaryTimeHour = hour + ((tod == "PM" && !isUpdating) ? 12 : 0);
            }
        }
        public int Min
        {
            get { return min; }
            set
            {
                min = value;
                if (min >= 60)
                {
                    Hour++;
                    min -= 60;
                }
            }
        }
        public string TOD
        {
            get { return tod; }
            set { tod = value; }
        }
        public int MilitaryTimeHour
        {
            get { return militaryTimeHour; }

            set
            {
                militaryTimeHour = value;
                if (militaryTimeHour >= 12 && militaryTimeHour != 24)
                    tod = "PM";
                else if (militaryTimeHour < 12 || militaryTimeHour == 24)
                    tod = "AM";
            }

        }

        public Time(string strTime)
        {
            Utils.parseTimeString(strTime, out hour, out min, out tod, out militaryTimeHour);
        }

        public Time(int hour, int min, string tod)
        {
            this.hour = hour;
            this.min = min;
            this.tod = tod;

            //NEED TO MAKE A UTILS FUNCTION TO REDUCE DUPLICATE CODE
            //cases that time is not 12 and is PM then need to add 12 to military hour
            if (tod == "PM" && hour != 12)
                militaryTimeHour = hour + 12;
            //cases that time is 12 AM then need to set military hour to 0
            else if (tod == "AM" && hour == 12)
                militaryTimeHour = hour - 12;
            //all other times that are AM need nothing added to the military hour
            else
                militaryTimeHour = hour;
        }

        public override string ToString()
        {
            string strTime = hour + ":";
            if (min == 0)
                strTime += "00";
            else if (min < 10)
                strTime += "0" + min;
            else
                strTime += min;
            return strTime + " " + tod;
        }

        public string militaryToString()
        {
            string strTime = militaryTimeHour + ":";
            if (min == 0)
                strTime += "00";
            else if (min < 10)
                strTime += "0" + min;
            else
                strTime += min;
            return strTime + " " + tod;
        }
    }

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
            //militaryHour = (tod == "PM" && hour != 12 ? hour + 12 : hour);

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
    }
}
