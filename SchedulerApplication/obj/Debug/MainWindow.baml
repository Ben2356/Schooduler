   M S B A M L   `   `   `  ����O  KSchedulerApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SchedulerApplicationSchedulerApplication  5   	   %    SchedulerApplication.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:SchedulerApplication     5   	   -   5        $
��root��5   6     � Title$  
Schooduler��6	   $	��800��6   $
��auto��6)    � WindowStartupLocation$ CenterScreen=�6B    J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$ local:CustomCommands.Exit��5
      6a     5   
   -   5        $# local:CustomCommands.Settings��5      6i    � 	Resources 5         courseTileContextMenu��%�   &	        �� 5   
   .@�5      m� -   5        $��Edit Course��5      6,   m� 5      -   5        $��Delete Course��5      6.   5      5      .��5      ]� .+�5   
   �� $	��Top=�6   .+�5      o� .@�5      m� $��_File��6   .@�5      m�  m� Command$ local:CustomCommands.Exit��6   5      m� 5      $��_Edit��6   .@�5      m� $# local:CustomCommands.Settings��6   5      m� 5      $��_Help��6   .@�5      m� -   5        $��_About��5      6/   5       5!      �� 5$      $��20(�6   $��Center=�6&   $��10q�6C   	Week of: �� 5%      �6   ��  �� Path$ ViewingWeek� �� ElementName$
 root��5&      5'      � 5)   
   $��5q�6    � RowDefinitions 5*      �� 5+      $��* �6    $	��250��6+   �� 5,      $
��Auto �6    �� 5-      $��* �6    $	��150��6+   �� 5.      $
��Auto �6    	 � ColumnDefinitions	 51      .+�54      '   !SchedulerApplication.ExtendedGrid  -   5        
  Name$
 gv_weekView��55         ShowCustomGridLines .    GridLineBrush
 �   �   GridLineThickness$ 1Q�54   R   $��30,10,30,10q�6h   '  !SchedulerApplication.GridBuilders   	RowCreate 56      �� $ GridRowCreate� �� Mode$ TwoWay=�$
 root��   ColumnCreate 57      �� $ GridColCreate�$ TwoWay=�$
 root�� 58        weekViewColText �� 
TargetType �� Property �� Value  weekViewRowText  
CourseTile �� 
TargetType �� RadiusX �� RadiusY �� CornerRadius �� Triggers P� Property P� Value �� 
TargetName��%1  &	       &	 �     &	 2    7   �� 59      " ��64   .�5:      ��  � ��$
 Bold"�6A   �� 5;       � ��$ Center=�6>   �� 5<       � ��$ Center=�6@   �� 5=       � ��$ 18(�65   5>      �� 5?      " ��64   .�5@      ��  � ��$ Center=�6>   �� 5A       � ��$ Center=�6@   5B      �� 5G      " d�6   .�5H      ��  � ��$
 Bold"�67   �� 5I       � ��$ 25(�65   �� 5J       �@�ContextMenu8   68   -   5        �� 5M       � �� 5N      �� 5O   "   " d�62   .R�5P   &   � .+�5Q   *   �� $��	rectangle��64   #��z��6E   $ 10��6i   $ 10��6v   �� 5S   *   $��border��61   
������ $��0q�6X   
���   �$ 10��6�   �� 5T   *   #��z��6;   $��Center=�6_   $��Center=�6|   5U   '    5W   &   P� 5X   *    �@d�	IsChecked$
 TrueY�6G   . �5Y   .   �� $ border��65    � ��$ 10q�6d   5Z   +   P� 5[   *    �@d�	IsChecked$ FalseY�6G   . �5\   .   �� $ border��65    � ��$ 0q�6d   5]   +   5_   #   5a      5b      5c      5d      �� 5g      -	   5        $��add_course_button��5g      $��2q�6.   $��0,2,30,2q�6:   $��Right=�6L   $��1��6h   $��30��6u   .��6�   
Add Course6�   &   System.Windows.Controls.DataGrid  5j      -
   5        $��dg_tasks��5j   9   $��2��6   $��30,30,30,10q�6$      AutoGenerateColumns .     CanUserAddRows .   5k      -   'SchedulerApplication.FullDateToMonthDay  FullDateToMonthDay  test)  #System.Windows.Controls.DataGridRow��%�   &	       &	        5l      6/   �� 5o      "  6)   .�5p      ��  � �� 5q      �� 5r   "   "  62   5t   #   5v      5w      5z         Columns 5|      4  .System.Windows.Controls.DataGridCheckBoxColumn  5}      ,  &System.Windows.Controls.DataGridColumn   Header$ Finished��6-   1	  +System.Windows.Controls.DataGridBoundColumn  	  Binding  6?   �� *	Completed+0
  *System.Windows.Controls.DataGridTextColumn
  5~      $ 
Assignment��6)   !   Width5  /System.Windows.Controls.DataGridLengthConverter$! * 6\     6=   �� *
Assignment+6\   
  5      $	 Due��6)     66   �� *Due+" �� 	Converter#" [ 
  5�      $ Notes��6)   $! * 6R     68   �� *Notes+6R   5�      �� 5�      -   5        $��add_task_button��5�      $��2q�6,   $��0,2,30,2q�68   $��Right=�6J   $��3��6f   $��30��6s   {�.  .��6�   
New Task6�   5�      5�      5�      