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
        public Task(bool isCompleted, string assignmentName, DateTime dueDate, string assignmentNotes)
        {
            completed = isCompleted;
            Assignment = assignmentName;
            Due = dueDate;
            Notes = assignmentNotes;
        }

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
