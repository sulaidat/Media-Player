   M S B A M L   `   `   `  ����G  CMedia Player, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Media_PlayerMedia_Player  5   	   '    !Media_Player.PlaylistRenameWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Media_Player     5   	   -   5          � Title$  PlaylistRenameWindow��5   	   $	��100��6&   $	��400��63   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��auto �6   �� 5      �� 5       � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   �� $��0��5      $��0��5      $��Name:��5      �� 5   
   -   5        $��txt_name��5      $��0��5      $��1��5      $��2��5      $��5q�5      ��5      �� *Name+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
    �� Orientation$ 
Horizontal=�5      $��Right=�5       $��5q�5!      $��1��5"      $��2��5#      .+�5$      �� -   5         �� 	IsDefault$
 True��5%      $��
0, 0, 5, 0q�5&      $��0��5'      $��Confirm��5)      �� 5*      -   5         �� IsCancel$
 True��5+      $��1��5,      $��Cancel��5.      5/      50      51      