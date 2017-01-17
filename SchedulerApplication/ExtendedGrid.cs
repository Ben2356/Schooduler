using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchedulerApplication
{
    //extension of the grid viewmodel that allows for grid lines on rows and columns
    public class ExtendedGrid : Grid
    {
        static ExtendedGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedGrid), new FrameworkPropertyMetadata(typeof(ExtendedGrid)));
        }

        public static readonly DependencyProperty ShowCustomGridLinesProperty = DependencyProperty.Register("ShowCustomGridLines", typeof(bool), typeof(ExtendedGrid), new UIPropertyMetadata(false));

        public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register("GridLineBrush", typeof(Brush), typeof(ExtendedGrid), new UIPropertyMetadata(Brushes.Black));

        public static readonly DependencyProperty GridLineThicknessProperty = DependencyProperty.Register("GridLineThickness", typeof(double), typeof(ExtendedGrid), new UIPropertyMetadata(1.0));

        public bool ShowCustomGridLines
        {
            get { return (bool)GetValue(ShowCustomGridLinesProperty); }
            set { SetValue(ShowCustomGridLinesProperty, value); }
        }

        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        public double GridLineThickness
        {
            get { return (double)GetValue(GridLineThicknessProperty); }
            set { SetValue(GridLineThicknessProperty, value); }
        }

        protected override void OnRender(DrawingContext dc)
        {
            if(ShowCustomGridLines)
            {
                foreach (var rowDefinition in RowDefinitions)
                    dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(0, rowDefinition.Offset), new Point(ActualWidth, rowDefinition.Offset));
                foreach (var columnDefintion in ColumnDefinitions)
                    dc.DrawLine(new Pen(GridLineBrush, GridLineThickness), new Point(columnDefintion.Offset, 0), new Point(columnDefintion.Offset, ActualHeight));
                dc.DrawRectangle(Brushes.Transparent, new Pen(GridLineBrush, GridLineThickness), new Rect(0, 0, ActualWidth, ActualHeight));
            }
            base.OnRender(dc);
        }

    }
}
