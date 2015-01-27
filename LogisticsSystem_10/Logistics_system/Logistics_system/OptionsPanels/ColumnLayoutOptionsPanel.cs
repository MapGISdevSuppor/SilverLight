using System.Windows.Controls;
using System.Windows;
using DevExpress.AgDataGrid;

//namespace DataGridDemo {
//    public partial class ColumnsLayout : UserControl, IDemoItem {
//        IDemoItemOwner owner;

//        DataTemplate IDemoItem.OptionsPanelTemplate {
//            get { return (DataTemplate)Resources["OptionsPanelTemplate"]; }
//        }
//        IDemoItemOwner IDemoItem.Owner { set { this.owner = value; } }
//        string IDemoItem.XAMLBlock { get { return "demos/columnlayout.xaml"; } }
//        string IDemoItem.CSBlock { get { return "Demos.ColumnLayout.xaml.cs"; } }

//        CheckBox GetColumnAutoWidthCheckBox(FrameworkElement optionsPanelElement) {
//            return (CheckBox)optionsPanelElement.FindName("cbColumnAutoWidth");
//        }
//        void OptionsPanel_Loaded(object sender, RoutedEventArgs e) {
//            CheckBox checkBox = GetColumnAutoWidthCheckBox((FrameworkElement)sender);
//            checkBox.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Left);
//            checkBox.IsChecked = grid.ColumnsAutoWidth;
//        }
//    }
//}
