   M S B A M L   `   `   `  ����O  KSchedulerApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SchedulerApplicationSchedulerApplication  5   	   #    SchedulerApplication.Settings   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:SchedulerApplication     5   	     � Title$  Settings��5   	    � SizeToContent$ WidthAndHeight=�6    � WindowStartupLocation$ CenterScreen=�69    � 
ResizeMode$ NoResize=�6^   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��50 �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $	��100 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6    $��3q�6-   $
��Left=�68   .��6S   Start Time:6`   �� 5   
   -   5        $��
cmbMinTime��5   ;   $��2��6   $��0��6#   $��5q�60   6;   �� 5   
   $��0��6   $��1��6    $��3q�6-   $
��Left=�68   .��6S   	End Time:6^   �� 5   
   -   5        $��
cmbMaxTime��5   ;   $��2��6   $��1��6#   $��5q�60   6;   �� 5   
   -   5        $��	BtnCancel��5   F   $��0��6   $��3��6!   $��10q�6.   $��2q�6:   .��6r   	_Cancel6{   �� 5   
   -   5        $��confirm��5   :   $��2��6   $��3��6!   $��10q�6.   $��2q�6I   .��6q   
C_onfirm6{   5      5      