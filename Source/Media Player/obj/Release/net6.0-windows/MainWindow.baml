   M S B A M L   `   `   `  ���� G  CMedia Player, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Media_PlayerMedia_Player      Media_Player.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:Media_Player     -     � Title$  
MainWindow��$	��450��$	��800��$	��500�� � 	Resources  �� 
TargetType �� Property �� Value��%E   'o�       �� " o�.���  � ��$ 14(���  � ��
 ��İ�.���  � ShowGridLines$
 True�� � ColumnDefinitions ��  � RowDefinitions �� $
��auto ��� �� $
��auto ��� $
��auto ��� $
��auto �.+�`�  `� 
Background
 �����$��0��.�c� 	 c� 	BandIndex$	 1��.@�o� .@�m� $��Media��.@�m� -   $��Open File...��m� $
��hehe��m� $
��hehe��m� $
��hehe���� $	��oke���� $	��oke��c� $	 2��.@��� $	��oke���� $	��oke���� $	��oke��c� $	 3��.@��� $	��oke���� $	��oke���� $	��oke���� �� $	��oke���� $	��oke���� $	��oke��r� -   $��	mediaView��$
|�True��$��1��
 r� StretchDirection$
 DownOnly=� r� LoadedBehavior$ Manual=� r� UnloadedBehavior$
 Stop=�� $��2�� �� $
��auto ��� �� $
��auto � �� .+��� -   $��txt_progress��$��0��$��00:00:00��$��
5, 0, 5, 0q��� -   $��slider_timeline��$��1�� �� IsMoveToPointEnabled$
 True������  �� 
TargetType" ��.R�� ���	#05000000.+�   Media_Player.CustomPopup    Name$ 	InfoPopup��$
��Auto��$
��Auto��$	��Top=� � 	StaysOpen$ False��$��False�� � AllowsTransparency$
 True�� � PlacementTarget ��  �� ElementName$ Thumb��.����  �� Padding$ 2q� �� CornerRadius$ 3��
���   �._��� 
�������$��tuyendeptrai��]�  ]�Name$ 
PART_Track�� ]� Thumb n�  n�Name$ Thumb��$��10��$��20�� �� Triggers P�  P� 
SourceName$ Thumb�� P� Property �@n�
IsDragging P� Value$
 True��. ���  �� 
TargetName$ 	InfoPopup�� � ��$
 True��P� $ Thumb�� �@n�
IsDragging$ False��. ��� $ 	InfoPopup�� � ��$ False���� -   $��txt_duration��$��2��$��99:59:59��$��
5, 0, 5, 0q�� $��3�� �� $	��10* ��� $
��457* ��� $
��auto � �� .+��� $��0�� �� Orientation$ 
Horizontal=�$��3��$��	0,0,333,0q�.+�d� -   $��toggle_play�� d� 	IsChecked$ FalseY�$
��20px��$��5q�.���� $��/imgs/play.png���� #��["��� -   $
��20px��$��5q�.���� $��/imgs/previous.png���� -	   $
��20px��$��5q�.���� $��/imgs/stop.png���� -
   $
��20px��$��5q�.���� $��/imgs/next.png���� #��["��� -   $��btn_openplaylist��$
��20px��$��5q�.���� $��/imgs/playlist.png���� -   $��btn_loop��$
��20px��$��5q�.���� $��/imgs/repeat.png���� -   $��btn_shuffle��$
��20px��$��5q�.���� $��/imgs/shuffle.png���� $��2��$ 
Horizontal=�.+��� $��/imgs/speaker.png��$
��20px���� -   $��slider_volume��$��Center=�  � Minimum$  0Q�! � Maximum$! 1Q�" � Value$	" 0.5Q�$��70���� 
���   �$��5q�$��Center=�$��Speed���� -   $��slider_speed��$��Center=�$" 1Q�$��70���� 
���   �$��5q�$��Center=�$��Seek To���� -   $��slider_seekto��$��5q�$��70��