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

namespace SchedulerApplication
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            BtnCancel.IsCancel = true;
            List<string> minTimeList = new List<string>();
            minTimeList.Add("12:00 AM");
            for (int i = 1; i < 12; i++)
                minTimeList.Add(i + ":00 AM");
            cmbMinTime.ItemsSource = minTimeList;
            List<string> maxTimeList = new List<string>();
            maxTimeList.Add("12:00 PM");
            for (int i = 1; i < 12; i++)
                maxTimeList.Add(i + ":00 PM");
            cmbMaxTime.ItemsSource = maxTimeList;
            string configFileStartTime = Properties.Settings.Default.timeStart;
            string configFileEndTime = Properties.Settings.Default.timeEnd;
            cmbMinTime.SelectedIndex = minTimeList.IndexOf(configFileStartTime);
            cmbMaxTime.SelectedIndex = maxTimeList.IndexOf(configFileEndTime);
        }

        private void cancelButton_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void confirmButton_click(object sender, RoutedEventArgs e)
        {
            //at some point we want to store the timeStart and timeEnd as the Time class objects to reduce need for parsing later on
            Properties.Settings.Default.timeStart = (string)cmbMinTime.SelectedValue;
            Properties.Settings.Default.timeEnd = (string)cmbMaxTime.SelectedValue;
            Properties.Settings.Default.Save();
            this.DialogResult = true;
        }
    }
}
