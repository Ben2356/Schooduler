   M S B A M L   `   `   `  ����O  KSchedulerApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SchedulerApplicationSchedulerApplication  5   	   "    SchedulerApplication.AddTask   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:SchedulerApplication     5   	   -   5        $��addtask_root��5   x     � Title$  Add New Task��6	    � SizeToContent$ WidthAndHeight=�6    � 
ResizeMode$ NoResize=�6=    � WindowStartupLocation$ CenterScreen=�6S   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��50 �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��50 �6   �� 5      $��* �6    � 	Resources 5   
    �� 
TargetType   	timeInput �� Property	 �� Value��%�   &	        �� 5      " ��6   .�5      ��  �@��AcceptsReturn	 .  62   �� 5       �@��
AcceptsTab	 .  6/   �� 5       � ��$	 30��6-   -   5        5!      5"      .+�5$   
   �� $��0��6   $��0��6   $��5,30,5,5q�6-   $��Bottom=�6?   .��6Z   Assignment Name:6l   �� 5%   
   -   5        $��assignmentName��5%   �   $��0��6   $��2��6   
 �� AcceptsReturn
 .   �� 
AcceptsTab .  $��	5,30,10,5q�6X   $��30��6k   $	��130��6z   $��Center=�6�   �� 5&   
   $��1��6   $��0��6   $��5q�6.   .��69   	Due Date:6D   (  "System.Windows.Controls.DatePicker  5'   
   -   5        $��cal_dueDate��5'   P   $��1��6   $��2��6"   $	��130��62   $��5,5,10,5q�6>   6P   �� 5(   
   $��2��6   $��0��6   $��5q�6.   .��69   	Time Due:6D   �� 5)   
   $��2��6   $��2��6"    �� Orientation$ 
Horizontal=�62   $��
5,10,10,10q�6K   .+�5*      �� -   5        $��txt_dueHour��5*   '   $��0,0,5,0q�6   #��[  6:   $��Center=�6�   ��6]   �� *	DueHour+ �� ElementName$ addtask_root��6�   �� 5+      :6   �� 5,      -   5        $��
txt_dueMin��5,   '   $��5,0,0,0q�6   #��[  69   $��Center=�6�   ��6\   �� *DueMin+$ addtask_root�� �� StringFormat$ {0:00}��6�   �� 5-      -   5        $��cmb_TOD��5-      $��10,0,0,0q�6&   $��45��68   5.      �� 5/   
   $��3��6   $��0��6   $��5q�6-   .��68   Notes (optional):6K   �� 50   
   -   5        $��notes��50   �   $��3��6   $��2��6   
 .   .  $��5,5,10,5q�6X   $��30��6j   $	��130��6y   $��Center=�6�   �� 52   
   $ 
Horizontal=�6   $��5��6.   $��3��6;   $��Center=�6O   .+�53      �� -	   5        $��	BtnCancel��53      $��10,10,50,10q�6&   $��80��6;   .��6a   	_Cancel6j   �� 54      -
   5        $��5��54      $��2��6"   $��50,10,10,10q�62   $��80��6G   .��6j   _Add6p   55      56      57      