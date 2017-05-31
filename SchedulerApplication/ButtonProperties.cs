using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SchedulerApplication
{
    class ButtonProperties
    {
        public static readonly DependencyProperty TaskListProperty = DependencyProperty.Register("TaskList", typeof(BindingList<Task>), typeof(ButtonProperties));

        public static BindingList<Task> GetTaskList(UIElement e)
        {
            return (BindingList<Task>)e.GetValue(TaskListProperty);
        }

        public static void SetTaskList(UIElement e, BindingList<Task> data)
        {
            e.SetValue(TaskListProperty, data);
        }
    }
}
