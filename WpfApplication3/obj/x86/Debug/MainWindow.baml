   M S B A M L   `   `   `  ����H  DMultiComboBox, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullF3clr-namespace:CustomControls;assembly=MultiComboBoxCustomControls  5   @    WpfApplication3    WpfApplication3.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Csrc3clr-namespace:CustomControls;assembly=MultiComboBox     6@     � Title$  
MainWindow��6�   $	��500��6�   $	��525��6�   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5
   
   �� $��1��6   $��0,10,0,0q�6    $��Multiple Selection��63   .��5      � $��5q�6    5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� .+�5      �� -   5         ��Name$ 	statusBox��5   (   $��10q�6   6(   �� 5      $(��"Selected Items Viewed in Text Box:��6   5      �� 5      $��0,5,0,0q�6   $��1��6+   $��30��68   ��5      ��  �� Path$ SelectedWords� �� Mode$ OneWay=��� 5,      $��3��6    �� Orientation$ 
Horizontal=�6*   .+�5-      �� -   5         ��Name$ txtBox1��5-      {�.  $	��200��6A   $��30��6M   $��50q�6Y   6h   �� 5.      -   5        $��Get New Word��5.      $	��100��6I   $��30��6U   6c   5/      �� 51      $��4��6   $ Vertical=�6*   .+�53      �� -   5        $#��Fetch Anagrams for above word��53      $	��200��6\   $��30��6h   $��10q�6t   6�   "   CustomControls.MultiComboBox  55      $��0,10,0,0q�6,    �� SelectionMode$ Multiple=�6>   $��30��6W   $��Center=�56   "      IsCreateNewEnabled . 	   DisplaySeparator$	 , ��58   "   ��59   "   �� *WordsGeneratedByAlgo+$   CustomControls.BindableListBox
   SelectedItems
 5:   "   �� *SelectedWords+5<      �� 5>      -   5        $��Commit��5>      $	��200��6.   $��30��6Q   $��30q�6]   6k   5?      5@      5A      5C      5D      