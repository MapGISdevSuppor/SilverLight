using System.Windows.Controls;
using System.Windows;
using DevExpress.AgDataGrid;
using System.Reflection;
using Logistics_system.controls;
using ZDIMSDemo.Controls;
namespace Logistics_system
{
    public partial class DataForm : BaseUserControl, IDemoItem
    {
        IDemoItemOwner owner;

        DataTemplate IDemoItem.OptionsPanelTemplate {
            get { return (DataTemplate)Resources["OptionsPanelTemplate"]; }
        }
        IDemoItemOwner IDemoItem.Owner { set { this.owner = value; } }
		string IDemoItem.XAMLBlock { get { return "controls/DataForm.xaml"; } }
        string IDemoItem.CSBlock { get { return "controls.DataForm.xaml.cs"; } }

        FrameworkElement cachedPanel;
        RadioButton RBNormal {
			get {
				return (RadioButton)cachedPanel.FindName("rbNormal");
			}
        }
		RadioButton RBToolTip {
			get {
                return cachedPanel != null ? cachedPanel.FindName("rbToolTip") as RadioButton : null;
            }
        }
		RadioButton RBOutside {
			get {
				return (RadioButton)cachedPanel.FindName("rbOutside");
			}
        }
        void InitializePreviewVisibilityOptions(FrameworkElement optionsPanelElement) {
			if(currentChecked != null) ((RadioButton)cachedPanel.FindName(currentChecked)).IsChecked = true;
			else RBNormal.IsChecked = true;
        }
        void OptionsPanel_Loaded(object sender, RoutedEventArgs e) {
			cachedPanel = (FrameworkElement)sender;
            InitializePreviewVisibilityOptions(cachedPanel);
        }
    }
}
