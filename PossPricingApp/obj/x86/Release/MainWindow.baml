   M S B A M L   `   `   `  ���� [  WJDB.Library.Controls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d3369d1bcc76ef37Y@clr-namespace:JDB.Library.Controls;assembly=JDB.Library.ControlsJDB.Library.Controls  ]Bclr-namespace:JDB.Library.Converters;assembly=JDB.Library.ControlsJDB.Library.Converters  eFclr-namespace:JDB.Library.Controls.Icons;assembly=JDB.Library.ControlsJDB.Library.Controls.Icons   PossPricingApp   PossPricingApp.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Ucontrols@clr-namespace:JDB.Library.Controls;assembly=JDB.Library.Controls     Y
convertersBclr-namespace:JDB.Library.Converters;assembly=JDB.Library.Controls     WiconFclr-namespace:JDB.Library.Controls.Icons;assembly=JDB.Library.Controls     -   $��myWindow��  � Title$/  )Riverfront Heifer Development Application��$	��574��$
��1000�����#ADD6FF � 	Resources  �� 
TargetType �� Property �� Value(   "JDB.Library.Controls.NumberTextBox7   1JDB.Library.Converters.ValueToVisibilityConverter   ValueToVisibilityConverter4   .JDB.Library.Converters.ValueToBooleanConverter  ValueToBooleanConverter+   %JDB.Library.Controls.TabSizeConverter  TabSizeConverter `� 	Converter �� RelativeSource �� Mode �� AncestorType	 �� Path��%  '��       '  %     &	  7     &	 <     &	 A     '�� F     7  �� " ��.���  � ��$
 Bold"��� "  .�-         �� " ��.���  � �� `� 8   .1���  �� $ FindAncestor=�" ���� $	 ActualWidth� �� $ FindAncestor=�" ��.��]� ��. .+�� 
 � ColumnDefinitions
 �� $	��300 ��� $	��300 ��� $��* �.+�� $��0�� � RowDefinitions �� $��* ��� $	��165 �.+�� $��0��.+��� $��	10,10,0,0q�$	��Top=�$
��Left=� �� Orientation$ 
Horizontal=�.+��� $��24(�$��Center=�	Customers�� -   $��10,0,0,0q�$��18��$��18��$��Center=�$��Center=�$��Center=�.��$   JDB.Library.Controls.Icons.Add  �� -   $��10,40,10,10q�{���  �� ElementName$ myWindow��$	 EditMode� �� 	Converter# [  �� ConverterParameter$ reverse������ $ myWindow��$	 	Customers� �� SelectedItem �� $ myWindow��$	 SelectedCustomer� � ContextMenu �� .@�m� -   $��Change Name��m� -   $��Delete������ .R�� $��35��.+��� $��10,0,0,0q�$
��Left=�$��Center=����� *Name+� $��1��.+��� $��14(�$��15,5,0,0q�$	��Top=�$
��Left=�Customer Notes�� $��15,25,15,43q����� $ myWindow��$	 SelectedCustomer.Notes�� $��1�� �� $	��305 ��� $��* �.+�� $��0��$��0,48,0,0q�.+��� $ Vertical=�.+��� $ 
Horizontal=�$
��Left=�$	��Top=�$��	20,10,0,0q�.+��� $��18(�Date:(  "System.Windows.Controls.DatePicker  $��12(�$��95��$��15 0 0 0q�$��Center=�   SelectedDate �� $	 SelectedDate��� -   $��25,0,0,0q�$��20��$��20���� $��0,5,0,0q�$��Stretch=�$��Stretch=�$	��170��.@��� -   $��
WorkDayTab��$��Work Day��{��� $ myWindow��$	 EditMode�# [ $ reverse��.��� .+��� $��16(�$��	15,10,0,0q�$
��Left=�$	��Top=�Distillers:  $��75��$��	0,10,15,0q�$��Right=�$��Right=�$��22��$	��Top=����� $ myWindow��$&	  SelectedWorkDay.PoundsDistillers� �� StringFormat$ {0:F2}���� $��16(�$��	15,35,0,0q�$
��Left=�$	��Top=�Hay:  $��75��$��	0,35,15,0q�$��Right=�$	��Top=�$��Right=����� $ myWindow��$	 SelectedWorkDay.PoundsHay�$ {0:F2}���� $��16(�$��	15,60,0,0q�$
��Left=�$	��Top=�Corn:  $��75��$��	0,60,15,0q�$��Right=�$	��Top=�$��Right=����� $ myWindow��$ 	 SelectedWorkDay.PoundsCorn�$ {0:F2}���� $��16(�$��	15,85,0,0q�$
��Left=�$	��Top=�	Minerals:  $��75��$��	0,85,15,0q�$��Right=�$	��Top=�$��Right=����� $ myWindow��$$	 SelectedWorkDay.PoundsMinerals�$ {0:F2}���� $��16(�$��
15,110,0,0q�$
��Left=�$	��Top=�
Yardage:  $��75��$��
0,110,15,0q�$��Right=�$	��Top=�$��Right=����� $ myWindow��$)	 #SelectedWorkDay.NumberOfHeadYardage�$ {0:F2}���� -	   $��MiscTab��$��Miscellaneous��{��� $ myWindow��$	 EditMode�# [ $ reverse��.��� .+��� $��16(�$��	15,15,0,0q�$
��Left=�$	��Top=�	Pounds:�� $��	0,15,15,0q�$��Right=�$	��Top=�$ 
Horizontal=�.+��� $��16(�$  $��75��$��3,0,0,0q�$��Right=����� $ myWindow��$	 SelectedMiscCharge.Amount�$ {0:F2}���� $��16(�$��	15,45,0,0q�$
��Left=�$	��Top=�Description:�� $��20,70,20,10q� �� TextWrapping$ WrapWithOverflow=����� $ myWindow��$$	 SelectedMiscCharge.Description�� $��0,10,0,0q�.+��� -
   $	��100��$��30��$
��Bold"�$��Center=����#4DDB94y��� $ myWindow��$	 EditMode�# [  $ reverse��.��
Add Charge� 
 �� $��* ��� $��* �.+��� -   $��0��$��75��$��30��$
��Bold"�$��Center=����#4DDB94y��� $ myWindow��$	 EditMode�# [  .��Save�� -   $��1��$��75��$��30��$
��Bold"�$��Center=����#FF8080y��� $ myWindow��$	 EditMode�# [  .��Cancel�� $��1��$	��225��$	��166��$��Center=�$	��Top=�$
��38,0q����� $��16(�$
��Bold"�Prices Per Ton.��� .+��� $��14(�$��10,5,0,0q�$
��Left=�$	��Top=�
Distillers  $��22��$��75��$��0,3,10,0q�$	��Top=�$��Right=����� $	 PricePerTonDistillers�$ {0:F2}���� $��14(�$��	10,33,0,0q�$
��Left=�$	��Top=�Hay  $��	0,30,10,0q�$��22��$��75��$	��Top=�$��Right=����� $	 PricePerTonHay�$ {0:F2}���� $��14(�$��	10,60,0,0q�$
��Left=�$	��Top=�Corn  $��22��$��75��$��	0,57,10,0q�$	��Top=�$��Right=����� $	 PricePerTonCorn�$ {0:F2}���� $��14(�$��	10,87,0,0q�$
��Left=�$	��Top=�
Minerals  $��22��$��75��$��	0,84,10,0q�$	��Top=�$��Right=����� $	 PricePerTonMinerals�$ {0:F2}���� $��14(�$��
10,114,0,0q�$
��Left=�$	��Top=�	Yardage  $��22��$��75��$��
0,111,10,0q�$	��Top=�$��Right=����� $	 PricePerHeadYardage�$ {0:F2}��� $��2��.+��� $��24(�$��5,10,0,0q�$	��Top=�$
��Left=�	Charges�� $ 
Horizontal=�$��20��$��	10,45,0,0q�$
��Left=�$	��Top=�.+��� $��10,0,0,0q�$
��Left=�$��Center=�	Select:�� -   $��10(�$
��Bold"�$��25��$��18��$��15,0,0,0q�$
��Left=�$��Center=�.��All�� -   $��10(�$
��Bold"�$��35��$��18��$��15,0,0,0q�$
��Left=�$��Center=�.��None�� -   $
��Bold"�$��	106,0,0,0q� d� 	IsChecked �� $ myWindow��$	 ShowAllCharges�.��Show All Charges�� $ 
Horizontal=�$��20��$��	72,67,0,0q�$
��Left=�$	��Top=�.+��� -   $��10(�$
��Bold"�$��50��$��18��$��0,0,0,0q�$
��Left=�$��Center=�.��	Between  -   $��BetweenDatePicker1��$��10(�$��85��$��20��$��5,0,0,0q�$��Center=��� $��3,0,0,0q�$��Center=�and  -   $��BetweenDatePicker2��$��10(�$��85��$��3,0,0,0q�$��Center=�&   System.Windows.Controls.DataGrid  -     Name$ ChargesGrid��$��10,89,10,43q�   AutoGenerateColumns .     SelectionMode$ Single=����� $ myWindow��$	 Charges�{��� $ myWindow��$	 EditMode�# [ $ reverse�� �� $ myWindow��$	 SelectedCharge� �� .@�m� -   $
��Edit��m� -   $��Delete��   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  ,	  &System.Windows.Controls.DataGridColumn 	  Header$ Include�� 	  Width5
  /System.Windows.Controls.DataGridLengthConverter$ 50
    CellTemplate �� .R��� $��Center=�$��Center=� �� $	 Include� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�0  *System.Windows.Controls.DataGridTextColumn  $
 Date��$ 60
 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� $
	 Date�$ M/d/yy��  $ Description��$ *
  �� $	 Description�  $ Amount��$ 75
  �� $	 Total�$ C���� -   $��75��$��30��$
��Bold"�$��0,0,0,6q�$��Center=�$��Bottom=�.��Print�� $��3��$��22(�$��10,0,0,5q�$
��Left=�$��Bottom=�Riverfront Heifer Development�� $��3��$��Italic$�$��12(�$��
348,0,0,10q�$
��Left=�$��Bottom=�75Big or small, we'll mount 'em all. - President/C.E.O.