using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerApplication
{
    //task class for storing tasks for a course
    public class Task
    {
        private bool completed;
        public Task()
        {
            completed = false;
        }
        public string Assignment { get; set; }
        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }
        public DateTime Due { get; set; }
        public string Notes { get; set; }
    }
}
