   M S B A M L   `   `   `  ���� G  CMedia Player, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Media_PlayerMedia_Player  !    Media_Player.PlaylistWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:Media_Player     -     � Title$  PlaylistWindow��$	��450��$	��800��.���  � ColumnDefinitions �� $��1* ��� $��5 ��� $	��5*  � � RowDefinitions �� .+�� $��0�� �� $
��auto ��� $
��auto �.+��� $��0�� �� Orientation$ Vertical=�.+��� $��My Playlists���� -   $��listview_playlist��$��Stretch=�$��Disabled=� � 	Resources    listview_contextmenu��%�   &	        �� .@�m� -   $��Rename��m� -   $��Remove��m� -   $��Export Single Playlist...��m� -   $��Export All Playlists...��m� -   $��Import Single Playlist...��m� -   $"��Import Multiple Playlists...������  �� 
TargetType" ��.���  �� Property �@�ContextMenu �� Value# [   �� Triggers �� 	 �� Binding	 �� *	IsPlaying+
 �� UpdateSourceTrigger$
 PropertyChanged=� �� Value True.a���  � �� �#a0a0a4���� .R��  �� $��20 ��� $��* �.+��� $��0��$��/imgs/playlist.png���� $��1������ *
Background+$
 PropertyChanged=����� *Name+$
 PropertyChanged=��� -	   $
��Left=�$��1��$��+ New Playlist���� $��1��$��Stretch=��� $��2��$ Vertical=�.+�&   System.Windows.Controls.DataGrid  -
   $��datagrid_medias�� J� 	AllowDrop$
 True��   AutoGenerateColumns$ False��   rowMenu��%)   &	       �� .@�m� -   $��Remove������ )  #System.Windows.Controls.DataGridRow"  .���  �@�ContextMenu# [ -    �� 	 �� *	IsPlaying+$
 PropertyChanged=� True.a���  � �� �#a0a0a4   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   
IsReadOnly$
 True��   Header$
 Name��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+  $
 True��$
 Path�� �� *Path+�� -   $��btn_addMedia��$y�Hidden=�$
��Left=�$��+ Add Media��