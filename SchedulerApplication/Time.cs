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
            militaryTimeHour = Utils.toMilitaryHour(hour, tod);
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
}
