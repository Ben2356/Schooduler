   M S B A M L   `   `   `  ����O  KSchedulerApplication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SchedulerApplicationSchedulerApplication  5   	        SchedulerApplication.Login   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:SchedulerApplication     5   	     � Title$  
User Login��5   	   $	��400��6   $	��300��6)    � WindowStartupLocation$ CenterScreen=�65   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��50��6   $��* �6+   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��Center=�6!   $��20,15,20,10q�6>   !Sign in or create a new account6u   �� 5   
   $��2��6   $
��Left=�6!   $��20,0,0,0q�6<   $��Center=�6N   	Username:6t   �� 5   
   -   5        $��usernameTextBox��5      $��3��6)    �� AcceptsReturn .   �� 
AcceptsTab .  $��30��6_   $��20,10,20,20q�6n   6�   �� 5   
   $��4��6   $
��Left=�6!   $��20,0,0,0q�6<   $��Center=�6N   	Password:6t   �� 5   
   -   5        $��readablePasswordBox��5      $��5��6   $y�Hidden=�6:   $	��Top=�6N   $��30��6f   $��
20,10,20,0q�6u   �� 5   
   $��5��6   .+�5      G� -   5        $��passwordTextBox��5      $��30��61   $��
20,10,20,0q�6@   6W   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� -   5        $��forgotPasswordTextBlock��5      $
��Left=�6;   $��
20,5,50,10q�6V   $��11(�6j   
����  �Forgot password?6�   �� 5       -   5        $��showPassCheckBox��5       $��Right=�63   $��
30,5,20,10q�6O   $��10(�6c   .��6�   Show Password6�   5!      5"      �� 5#   
   -   5        $��statusMessage��5#   !   $��6��6   $��Center=�66   
���  ��6S   �� 5$   
   $��8��6   .+�5%      �� -   5        $��20��5%      $��50��6$   $	��100��62   $��20,20,20,10q�6A    �� 	IsDefault . .��6{   _Login6�   �� 5&      -   5        $��20��5&      $��50��6$   $	��100��62   $��20,10,20,20q�6A   .��6m   	_Register6x   5'      5(      5)      