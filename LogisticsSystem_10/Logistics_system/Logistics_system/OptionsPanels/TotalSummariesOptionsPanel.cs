using System.Windows.Controls;
using System.Windows;
using DevExpress.AgDataGrid.Data;
using DevExpress.AgDataGrid;
using ZDIMSDemo.Controls;
namespace Logistics_system
{
    public partial class TotalSummaries : BaseUserControl, IDemoItem
    {
        IDemoItemOwner owner;

        DataTemplate IDemoItem.OptionsPanelTemplate
        {
            get { return (DataTemplate)Resources["OptionsPanelTemplate"]; }
        }
        IDemoItemOwner IDemoItem.Owner { set { this.owner = value; } }
        string IDemoItem.XAMLBlock { get { return "demos/totalsummaries.xaml"; } }
        string IDemoItem.CSBlock { get { return "Demos.TotalSummaries.xaml.cs"; } }

        RadioButton GetStandardSummariesRadioButton(FrameworkElement optionsPanelElement)
        {
            return (RadioButton)optionsPanelElement.FindName("rbStandard");
        }
        RadioButton GetCustomSummariesRadioButton(FrameworkElement optionsPanelElement)
        {
            return (RadioButton)optionsPanelElement.FindName("rbCustom");
        }
        void OptionsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!standardChecked) GetCustomSummariesRadioButton((FrameworkElement)sender).IsChecked = true;
            //if (grid.Totals.Count > 0) return;
            //if (GetStandardSummariesRadioButton((FrameworkElement)sender).IsChecked.Value)
            //{
            //    ShowStandardSummaries();
            //}
            //else
            //{
            //    ShowCustomSummaries();
            //}
        }
    }
}
