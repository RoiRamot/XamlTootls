   M S B A M L   `   `   `  ����K  GImageOverlayTool, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ImageOverlayToolImageOverlayTool  5   	   !    ImageOverlayTool.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;imageOverlayToolclr-namespace:ImageOverlayTool     5   	   -   5          �Name$
  This��5   	    � Title$ 
MainWindow��5   	    � AllowsTransparency . 
������  � 
ResizeMode$ 	CanResize=�5   	    � WindowState$ 	Maximized=�5   	    � WindowStyle$
 None=�5   	    � Topmost 5	   	   �� *	IsTopMost+ � DataContext 5         ImageOverlayTool.ViewModel  5   
    � 	Resources 5          BooleanToVisibilityConverter��%   &	        �� 5   
   6'   5      ��5      �� 5   
   	 �� 
TargetType"	 �6   .�5      �� '  !System.Windows.Shell.WindowChrome
 �� Property
 �@ WindowChrome �� Value 5        5         CaptionHeight$ 999999Q�6#      ResizeBorderThickness$ 1q�5   #   5      �� 5      
 � �� 5      �� 5        �� 
TargetType" �6&   .R�5!      � .+�5"      �� 
������ ._�5#   "   �� #��z��63   5$      5%      5&      5(      5)      .��5+      ]� .+�5,   
   o� $	��Top=�6   $��16(�5-      $��5q�5.         IsHitTestVisibleInChrome . .@�50      m� $��Change Picture��6    m� Command 6/   �� *LoadImageCommand+�� 51      $��Allways On Top��67   $��Center=�6P    d� 	IsChecked 6   �� *	IsTopMost+6t   �� 52      $��	Opacity: ��6   6+   �� 53      -   5         ��Name$ OpacitySlider��53       � Minimum$ 0Q�6,    � Maximum$ 1Q�68   $	��200��6D   $��Center=�6P    � Value$	 0.5Q�6k   6y   54      �� 55   
   $��Uniform=�57       J� Opacity 55      �� *Value+ �� ElementName$ OpacitySlider����56      �� *Image+57      58      59      