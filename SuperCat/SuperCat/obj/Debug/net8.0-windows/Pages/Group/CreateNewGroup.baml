   M S B A M L   `   `   `  ����C  ?SuperCat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SuperCat.Pages.GroupSuperCat.Pages.Group  5      )    #SuperCat.Pages.Group.CreateNewGroup   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:SuperCat.Pages.Group     5        N� Title$  CreateNewGroup��5	         MainFone#���   5
       � 	Resources 5        
ListButton �� 
TargetType ��Name  ProfilStyle �� CornerRadius � Cursor  ButtonContent �� Triggers P� Property P� Value	 �� 
TargetName
 �� Property �� Value��%y  &	       �� 5   
   " ��6-   .R�5      �� $ ListButtonBorder��6   #���  5      $ 10��5      $
 Hand��5      ._�5      �� #��z��6!   $��Center=�5      $��Center=�5      #���  5      $��20(�5      5       5      P� 5       �@J�IsMouseOver$
 True��61   . �5      �� $	 ListButtonBorder��6   
 � �� �#1136Q   5      P� 5       � ��$
 True��6/   . �5      �� $	 ListButtonBorder��6   
 � ��
 � ���6Q   5      P� 5        � {�$ False��6/   . �5!      �� $	 ListButtonBorder��6   
 � ��
 �����6Q   5"      5$      5%      .��5'      (� .��5(   
   � $	��450��6   $	��800��6   .+�5)      �� $	��top=�6   $��Center=�5*        CreateGroupText#���  5+        	LogerText#���  5,      $��30(�5-      $��0, 30, 0, 0q�5.      �� 50      -   5        
���   �$��1q�52      $
 Hand��53      $��90,100,510,160q�54      ._�56      �� -   5         ��Name$ textImg��56      $!��../../Image/Icon/Groups.png��57      $��UniformToFill=�58      59      �� 5:        	ImageText#���  6   $��20(�5;      $	��Top=�5<      $
��Left=�5=      $��150,280,0,0q�6/   �� 5?        GroupNameText#���  6   $��20(�5@      $	��Top=�5A      $
��Left=�5B      $��330,130,0,0q�6/   �� 5C      -   5         ��Name$ NameText��5E      $��500,135,90,285q�5C      $��15(�5D      5F   1   �� 5H        NumberSeatsText#���  6   $��20(�5I      $	��Top=�5J      $
��Left=�5K      $��330,222,0,0q�6/   �� 5L      -   5         ��Name$ Place��5O       �� MaxDropDownHeight$	 150��5L      $��Center=�5M      $��Center=�5N      $��500,222,90,190q�5P      $��20(�5R      �� 5U      -   5         ��Name$ CreateGroup��5V      $��90,356,90,59q�5U      $��35��5W      #���  5X      $y�	Collapsed=�5Y      #���  5Z      ${�False��5\      �� 5]      -   5        $ UpdateGroup��5^      $��90,375,90,40q�5]      $��35��5_      $y�	Collapsed=�5`      #���  5a       	 UpdateGroupText#��� 	 5b      ${�False��5d      �� 5f      -   5        $ backBut��5g      $��80,50,690,370q�5h       
 backTemp#��� 
 5i      z�5k      �� 5l       �� Angle$ 90Q�6&   5n      �� 5p      -   5        $ InviteButton��5v      $��90,325,590,100q�5p      
�������  ClickButtom#���  5r        
InviteText#���  5s      ${�False��5t      $y�	Collapsed=�5u      5v   +   �� 5x      -	   5        $ DeleteButon��5~      $��238,325,442,100q�5x      
�������$y�	Collapsed=�5z      #���  5{        DeleteAccount#���  5|      ${�False��5}      5~   *   �� 5�      -
   5        $ UserList��5�      $��389,325,90,100q�5�      $��50, 0, 0, 0q�5�      $��18(�5�      $y�	Collapsed=�5�      .@�5�      ��  �� 
IsSelected$
 True��6     	UsersText#���  5�      5�      5�      5�      5�      