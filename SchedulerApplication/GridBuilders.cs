using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

//custom grid properties for dynamic grid generation
namespace SchedulerApplication
{
    class GridBuilders
    {
        private struct DimensionElement
        {
            public bool isStarVal { get; set; }
            public bool isAutoVal { get; set; }
            public double doubleVal { get; set; }
            public bool isDouble { get; set; }
        }

        //RowCount
        public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(GridBuilders), new PropertyMetadata(0, RowCountChanged));
        public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue <= 0)
                return;
            Grid grid = (Grid)obj;
            grid.RowDefinitions.Clear();
            for (int i = 0; i < (int)e.NewValue; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
        }
        public static int GetRowCount(UIElement e)
        {
            return (int)e.GetValue(RowCountProperty);
        }
        public static void SetRowCount(UIElement e, int value)
        {
            e.SetValue(RowCountProperty, value);
        }

        //RowHeight
        //suports row height as both double/int and "*" , no need for "Auto" support as that is default height set upon creation of rows
        public static readonly DependencyProperty RowHeightProperty = DependencyProperty.RegisterAttached("RowHeight", typeof(object), typeof(GridBuilders), new PropertyMetadata(GridLength.Auto.Value, RowHeightChanged));
        public static void RowHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            double result;
            bool rowIsDouble = double.TryParse((string)e.NewValue, out result);
            if (!(obj is Grid) || ((Grid)obj).RowDefinitions.Count == 0 || (rowIsDouble && result <= 0) || (!rowIsDouble && (string)e.NewValue != "*"))
                return;
            for (int i = 0; i < ((Grid)obj).RowDefinitions.Count; i++)
            {
                GridLength height = !rowIsDouble ? new GridLength(1, GridUnitType.Star) : new GridLength(result);
                ((Grid)obj).RowDefinitions[i].Height = height;
            }
        }
        public static object GetRowHeight(UIElement e)
        {
            return e.GetValue(RowHeightProperty);
        }
        public static void SetRowHeight(UIElement e, object value)
        {
            e.SetValue(RowHeightProperty, value);
        }

        //RowCreate
        //syntax for property is RowCreate="int rows, string listOfHeights"
        //ex) RowCreate="5,2,3,*,4,Auto" creates 5 rows, where the first row height is 2.0, second row height is 3.0, third row height is *, etc
        //ex) RowCreate="4,10" creates 4 rows, where all their heights are 10.0
        public static readonly DependencyProperty RowCreateProperty = DependencyProperty.RegisterAttached("RowCreate", typeof(string), typeof(GridBuilders), new PropertyMetadata("0,", CreateRows));
        public static void CreateRows(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            string input = ((string)e.NewValue);

            //index 0 refers to row 0
            List<DimensionElement> heightList = new List<DimensionElement>();

            int commaBreakLoc = input.IndexOf(',');
            int rows;
            bool isValid = int.TryParse(input.Substring(0, commaBreakLoc),out rows);
            int startLoc = ++commaBreakLoc;
            bool validHeight = true;
            while(commaBreakLoc != -1)
            {
                commaBreakLoc = input.IndexOf(',', startLoc);
                double h;
                string val;               
                if (commaBreakLoc == -1)     
                    val = input.Substring(startLoc);                 
                else
                {
                    val = input.Substring(startLoc, commaBreakLoc-startLoc);
                    startLoc = ++commaBreakLoc;
                }
                if (val == "*")
                    heightList.Add(new DimensionElement() { isStarVal = true, isDouble = false });
                else if(val == "Auto")
                    heightList.Add(new DimensionElement() { isAutoVal = true, isDouble = false });
                else if (double.TryParse(val, out h) && h > 0)
                    heightList.Add(new DimensionElement() { doubleVal = h, isDouble = true });
                else
                {
                    validHeight = false;
                    break;
                }                   
            }
            if (!(obj is Grid) || !isValid || rows <= 0 || !validHeight || heightList.Count > rows)
                return;
            Grid grid = (Grid)obj;
            grid.RowDefinitions.Clear();
            for (int i = 0; i < rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            setHeights(heightList,grid);
        }
        public static string GetRowCreate(UIElement e)
        {
            return (string)e.GetValue(RowCreateProperty);
        }
        public static void SetRowCreate(UIElement e, string value)
        {
            e.SetValue(RowCreateProperty, value);
        }
        private static void setHeights(List<DimensionElement> heightList, Grid grid)
        {
            GridLength height;
            bool lastValue = false;
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (i >= heightList.Count)
                    lastValue = true;
                if(heightList[lastValue ? heightList.Count - 1 : i].isDouble)
                    height = new GridLength(heightList[i].doubleVal);
                else if(heightList[lastValue ? heightList.Count - 1 : i].isStarVal)
                    height = new GridLength(1, GridUnitType.Star);
                else
                    height = GridLength.Auto;
                grid.RowDefinitions[i].Height = height;
            }
        }

        //ColumnCount
        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(GridBuilders), new PropertyMetadata(0, ColumnCountChanged));
        public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is Grid) || (int)e.NewValue <= 0)
                return;
            Grid grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();
            for (int i = 0; i < (int)e.NewValue; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }
        }
        public static int GetColumnCount(UIElement e)
        {
            return (int)e.GetValue(ColumnCountProperty);
        }
        public static void SetColumnCount(UIElement e, int value)
        {
            e.SetValue(ColumnCountProperty, value);
        }

        //ColumnWidth
        //suports column width as both double/int and "*" , no need for "Auto" support as that is default width set upon creation of columns
        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.RegisterAttached("ColumnWidth", typeof(object), typeof(GridBuilders), new PropertyMetadata(GridLength.Auto.Value, ColumnWidthChanged));
        public static void ColumnWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            double result;
            bool colIsDouble = double.TryParse((string)e.NewValue, out result);
            if (!(obj is Grid) || ((Grid)obj).ColumnDefinitions.Count == 0 || (colIsDouble && result <= 0) || (!colIsDouble && (string)e.NewValue != "*"))
                return;
            for (int i = 0; i < ((Grid)obj).ColumnDefinitions.Count; i++)
            {
                GridLength width = !colIsDouble ? new GridLength(1, GridUnitType.Star) : new GridLength(result);
                ((Grid)obj).ColumnDefinitions[i].Width = width;
            }
        }
        public static object GetColumnWidth(UIElement e)
        {
            return e.GetValue(ColumnWidthProperty);
        }
        public static void SetColumnWidth(UIElement e, object value)
        {
            e.SetValue(ColumnWidthProperty, value);
        }

        //ColumnCreate
        //syntax for property is ColumnCreate="int columns, string width"
        //ex) ColumnCreate="5,2,3,*,4,Auto" creates 5 columns, where the first column width is 2.0, second column width is 3.0, third column width is *, etc
        //ex) ColumnCreate="4,10" creates 4 columns, where all their widths are 10.0
        public static readonly DependencyProperty ColumnCreateProperty = DependencyProperty.RegisterAttached("ColumnCreate", typeof(string), typeof(GridBuilders), new PropertyMetadata("0,", CreateColumns));
        public static void CreateColumns(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            string input = ((string)e.NewValue);

            //index 0 refers to column 0
            List<DimensionElement> widthList = new List<DimensionElement>();

            int commaBreakLoc = input.IndexOf(',');
            int cols;
            bool isValid = int.TryParse(input.Substring(0, commaBreakLoc), out cols);
            int startLoc = ++commaBreakLoc;
            bool validWidth = true;
            while (commaBreakLoc != -1)
            {
                commaBreakLoc = input.IndexOf(',', startLoc);
                double w;
                string val;
                if (commaBreakLoc == -1)
                    val = input.Substring(startLoc);
                else
                {
                    val = input.Substring(startLoc, commaBreakLoc - startLoc);
                    startLoc = ++commaBreakLoc;
                }
                if (val == "*")
                    widthList.Add(new DimensionElement() { isStarVal = true, isDouble = false });
                else if (val == "Auto")
                    widthList.Add(new DimensionElement() { isAutoVal = true, isDouble = false });
                else if (double.TryParse(val, out w) && w > 0)
                    widthList.Add(new DimensionElement() { doubleVal = w, isDouble = true });
                else
                {
                    validWidth = false;
                    break;
                }
            }
            if (!(obj is Grid) || !isValid || cols <= 0 || !validWidth || widthList.Count > cols)
                return;
            Grid grid = (Grid)obj;
            grid.ColumnDefinitions.Clear();
            for (int i = 0; i < cols; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            setWidths(widthList, grid);
        }
        public static string GetColumnCreate(UIElement e)
        {
            return (string)e.GetValue(ColumnCreateProperty);
        }
        public static void SetColumnCreate(UIElement e, string value)
        {
            e.SetValue(ColumnCreateProperty, value);
        }
        private static void setWidths(List<DimensionElement> widthList, Grid grid)
        {
            GridLength width;
            bool lastValue = false;
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (i >= widthList.Count)
                    lastValue = true;
                if (widthList[lastValue ? widthList.Count - 1 : i].isDouble)
                    width = new GridLength(widthList[i].doubleVal);
                else if (widthList[lastValue ? widthList.Count - 1 : i].isStarVal)
                    width = new GridLength(1, GridUnitType.Star);
                else
                    width = GridLength.Auto;
                grid.ColumnDefinitions[i].Width = width;
            }
        }

        //uniformly assigns each row in the gridview a time that is between the user set start and end times, resizing the grid as needed
        public static List<Time> drawGridRowTimes(ExtendedGrid grid)
        {
            grid.Children.Clear();
            List<Time> timeRange = new List<Time>();
            string startTime = Properties.Settings.Default.timeStart;
            int start = Utils.getMilitaryHour(startTime);
            int end = Utils.getMilitaryHour(Properties.Settings.Default.timeEnd);
            float delta = (float)end - start;
            TextBlock time;
            Time currTime = new Time(startTime);                 
            MainWindow.GridRowCreate = (delta + 2) + ",50,*";
            grid.GetBindingExpression(RowCreateProperty).UpdateTarget();
            grid.InvalidateVisual();
            for (int i = 1; i < grid.RowDefinitions.Count; i++)
            {
                currTime = new Time(currTime.ToString());
                if (i != 1)                                   
                    currTime.Hour++;
                timeRange.Add(currTime);
                time = new TextBlock();
                time.Text = currTime.ToString();
                time.Style = grid.FindResource("weekViewRowText") as Style;
                Grid.SetRow(time, i);
                grid.Children.Add(time);
            }
            return timeRange;
        }

        //creates the headers at the top of each gridview column with the name of the day and the date it refers to
        public static void drawColDayHeaders(Grid grid, List<string> dayList)
        {
            for(int i = 1; i < grid.ColumnDefinitions.Count; i++)
            {
                TextBlock day = new TextBlock();
                day.Style = grid.FindResource("weekViewColText") as Style;
                int forwardSlashLoc = MainWindow.ViewingWeek.IndexOf('/');
                int viewingMonth = int.Parse(MainWindow.ViewingWeek.Substring(0, forwardSlashLoc));
                day.Text = dayList[i - 1] + " (" + viewingMonth + "/" + MainWindow.weekViewDayList[i - 1] + ")"; 
                Grid.SetColumn(day, i);
                grid.Children.Add(day);
            }
        }

        //12AM TO ANY TIME HAS BROKEN END TIME
        public static void drawCourses(Grid grid, List<Course> courseList, List<Time> timeRange, List<string> dayList)
        {
            for (int i = 0; i < courseList.Count; i++)
            {
                Time start = courseList[i].TimeStart;
                int min = start.Min;
                int mHour = start.MilitaryTimeHour;
                int endMin = courseList[i].TimeEnd.Min;
                int endMHour = courseList[i].TimeEnd.MilitaryTimeHour;
                int rowSpanCount = 0;
                float startMinRatio = (float)min / 60;
                float endMinRatio = (float)endMin / 60;
                bool endNotVisible = false;
                bool startNotVisible = false;
                if (endMHour > timeRange[timeRange.Count - 1].MilitaryTimeHour)
                    endNotVisible = true;
                if (mHour < timeRange[0].MilitaryTimeHour)
                    startNotVisible = true;
                int timeRangeStartLoc = -1;
                int dayColIndex = 0;
                for (int j = 0; j < timeRange.Count; j++)
                {
                    if (timeRange[j].MilitaryTimeHour >= mHour && timeRange[j].MilitaryTimeHour <= endMHour)
                    {
                        rowSpanCount++;
                        if(timeRangeStartLoc == -1)
                            timeRangeStartLoc = j + 1;
                    }
                }
                for(int k = 0; k < dayList.Count; k++)
                {
                    for(int l = 0; l < courseList[i].CourseDay.Count; l++)
                    {
                        if (dayList[k] == courseList[i].CourseDay[l])
                        {   
                            dayColIndex = k + 1;
                            ToggleButton courseTile = new ToggleButton();
                            courseTile.Style = grid.FindResource("CourseTile") as Style;
                            courseTile.Content = courseList[i].CourseName;
                            SolidColorBrush tileColor = new SolidColorBrush();
                            tileColor.Color = Color.FromArgb(230, courseList[i].TileColor.R, courseList[i].TileColor.G, courseList[i].TileColor.B);
                            courseTile.Background = tileColor;
                            courseTile.Margin = new Thickness(5, (startNotVisible ? 0 : (grid.RowDefinitions[1].ActualHeight * startMinRatio)), 5, (endNotVisible ? 0 : (grid.RowDefinitions[1].ActualHeight - (grid.RowDefinitions[1].ActualHeight * endMinRatio))));
                            if (courseList[i].CourseDay.Count > 1)
                            {
                                if (l == 0)
                                    courseList[i].relatedButtons = new List<ToggleButton>();
                                courseList[i].relatedButtons.Add(courseTile);
                            }
                            Grid.SetRowSpan(courseTile, rowSpanCount);
                            Grid.SetRow(courseTile, timeRangeStartLoc);
                            Grid.SetColumn(courseTile, dayColIndex);
                            grid.Children.Add(courseTile);
                            courseTile.SetValue(ButtonProperties.TaskListProperty, courseList[i].TaskList);
                        }
                    }
                }                
            }
        }
    }
}
