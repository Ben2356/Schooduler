using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SchedulerApplication
{
    class ButtonProperties
    {
        public static readonly DependencyProperty TaskListProperty = DependencyProperty.Register("TaskList", typeof(List<Task>), typeof(ButtonProperties));

        public static List<Task> GetTaskList(UIElement e)
        {
            return (List<Task>)e.GetValue(TaskListProperty);
        }

        public static void SetTaskList(UIElement e, List<Task> data)
        {
            e.SetValue(TaskListProperty, data);
        }
    }
}
