   M S B A M L   `   `   `  ����G  CMedia Player, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Media_PlayerMedia_Player  5   	   !    Media_Player.PlaylistWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Media_Player     5   	   -   5          � Title$  PlaylistWindow��5   	   $	��450��6    $	��800��6-   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��5 �6   �� 5      $	��5*  �6    � RowDefinitions 5   
   �� 5      .+�5   
   � $��0��5       5      �� 5      $
��auto �6    �� 5      $
��auto �6    .+�5      �� $��0��5       �� Orientation$ Vertical=�5      .+�5      �� $��My Playlists��5       �� 5!      -   5        $��listview_playlist��5#      $��Stretch=�5$      $��Disabled=�5%       � 	Resources 5&         listview_contextmenu��%�  &	        �� 5'      .@�5(      m� -   5        $��Rename��5(   '   67   m� 5)      -   5        $��Remove��5)   '   67   m� 5*      -   5        $��Export Single Playlist...��5*   '   6J   m� 5+      -   5        $��Export All Playlists...��5+   '   6H   m� 5,      -   5        $��Import Single Playlist...��5,   '   6J   m� 5-      -   5        $"��Import Multiple Playlists...��5-   '   6M   53      54      ��55      �� 56       �� 
TargetType" ��6    .�57      ��  �� Property �@�ContextMenu �� Value# [  6<    �� Triggers 59      �� 5:   "   	 �� Binding	 6.   �� *	IsPlaying+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� Value True6q   .a�5;   &   ��  � �� �#a0a0a46C   5<   #   5>      ��5@      �� 5A      .R�5B      �  5C   "   �� 5D   &   $��20 �67   �� 5E   &   $��* �67   .+�5G   "   �� $��0��5H   %   $��/imgs/playlist.png��5I   %   �� 5J   "   $��1��5K   %   ��5L   %   �� *
Background+$
 PropertyChanged=���5M   %   �� *Name+$
 PropertyChanged=�5N      5O      5Q      5R      �� 5S      -	   5        $
��Left=�5U      $��1��5V      $��+ New Playlist��5W      5X      �� 5Z   
   $��1��5[      $��Stretch=�5\      �� 5]   
   $��2��5^      $ Vertical=�5_      .+�5a      &   System.Windows.Controls.DataGrid  -
   5        $��datagrid_medias��5d       J� 	AllowDrop$
 True��5b         AutoGenerateColumns$ False��5e       5f        rowMenu��%[   &	       �� 5g      .@�5h      m� -   5        $��Remove��5h   #   63   5i      5j      ��5k      �� 5l      )  #System.Windows.Controls.DataGridRow"  6   .�5m      ��  �@�ContextMenu# [ 68   -   5         5o      �� 5p      	 6*   �� *	IsPlaying+$
 PropertyChanged=� True6m   .a�5q   "   ��  � �� �#a0a0a46?   5r      5t         Columns 5v      0  *System.Windows.Controls.DataGridTextColumn  5w      ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly$
 True��5x         Header$
 Name��5y      1  +System.Windows.Controls.DataGridBoundColumn   Binding 5z      �� *Name+  5{      $
 True��5|      $
 Path��5}       5~      �� *Path+5�      �� 5�      -   5        $��btn_addMedia��5�      $y�Hidden=�5�      $
��Left=�5�      $��+ Add Media��5�      5�      5�      5�      