using System.Windows.Controls;
using System.Windows;
using DevExpress.AgDataGrid;
using System.Reflection;

//namespace Logistics_system.controls
//{
//    public partial class Preview : UserControl, IDemoItem {
//        IDemoItemOwner owner;

//        DataTemplate IDemoItem.OptionsPanelTemplate {
//            get { return (DataTemplate)Resources["OptionsPanelTemplate"]; }
//        }
//        IDemoItemOwner IDemoItem.Owner {
//            set { this.owner = value; }
//        }
//        DataPreviewVisibility GetDataPreviewVisibilityForRadioButton(RadioButton button) {
//            if (button == GetRadioButtonForDataPreviewVisibility(button, DataPreviewVisibility.Collapsed)) return DataPreviewVisibility.Collapsed;
//            if (button == GetRadioButtonForDataPreviewVisibility(button, DataPreviewVisibility.ForAllRows)) return DataPreviewVisibility.ForAllRows;
//            return DataPreviewVisibility.ForFocusedRow;
//        }
//        RadioButton GetRadioButtonForDataPreviewVisibility(FrameworkElement optionsPanelElement, DataPreviewVisibility visibility) {
//            return (RadioButton)optionsPanelElement.FindName("rb" + visibility.ToString());
//        }
//        Visibility GetVisibilityByIsVisible(bool isVisible) {
//            return isVisible ? Visibility.Visible : Visibility.Collapsed;
//        }
//        void InitializePreviewVisibilityOptions(FrameworkElement optionsPanelElement) {
//            foreach (FieldInfo fieldInfo in typeof(DataPreviewVisibility).GetFields()) {
//                if (fieldInfo.FieldType != typeof(DataPreviewVisibility)) continue;
//                DataPreviewVisibility previewVisibility = (DataPreviewVisibility)fieldInfo.GetRawConstantValue();
//                GetRadioButtonForDataPreviewVisibility(optionsPanelElement, previewVisibility).IsChecked = grid.PreviewVisibility == previewVisibility;
//            }
//        }
//        void OptionsPanel_Loaded(object sender, RoutedEventArgs e) {
//            InitializePreviewVisibilityOptions((FrameworkElement)sender);
//        }
//        string IDemoItem.XAMLBlock { get { return "demos/preview.xaml"; } }
//        string IDemoItem.CSBlock { get { return "Demos.Preview.xaml.cs"; } }
//    }
//}
