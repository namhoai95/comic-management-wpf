using System.Windows;
using System.Windows.Controls;
using QuanLyTruyenTranh.UserControl;

namespace QuanLyTruyenTranh.Helper
{
    public class CTabItem : TabItem
    {
        public void SetHeader(UIElement element)
        {
            var dockPanel = new DockPanel();
            dockPanel.Children.Add(element);

            var closeButton = new ucButtonClose();
            closeButton.Click += (sender, e) =>
            {
                var tabControl = Parent as ItemsControl;
                tabControl.Items.Remove(this);
            };

            dockPanel.Children.Add(closeButton);

            Header = dockPanel;
        }
    }
}
