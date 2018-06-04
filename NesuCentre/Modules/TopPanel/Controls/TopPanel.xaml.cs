using NesuCentre.Configurations.TopPanelConfiguration;
using NesuCentre.Modules.TopPanel.Controls;
using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {
        public static string test { get; set; }

        public TopPanelContainer ParentContainer { get; internal set; }

        public TopPanel()
        {
            InitializeComponent();
            InitializeAllTopPanelItems();
            C_Canvas.AllowDrop = true;
        }

        private void UserControl_Drop(object sender, DragEventArgs e)
        {
            var droppedData = e.Data;
            var paths = (e.Data.GetData(DataFormats.FileDrop) as String[]);
            if (paths == null || paths.Length == 0)
                return;

            string filepath = paths[0];

            var config = new TopPanelItemConfiguration();
            config.Path = filepath;
            config.X = Mouse.GetPosition(C_Canvas).X - 50;
            config.Y = Mouse.GetPosition(C_Canvas).Y - 50;
            ConfigurationCentre.Configuration.TopPanelItems.Add(config);
            C_Canvas.Children.Add(new TopPanelItem(config));
            ConfigurationCentre.ConfigurationChanged = true;
        }

        private void InitializeAllTopPanelItems()
        {
            foreach (var topItemconfig in ConfigurationCentre.Configuration.TopPanelItems)
            {
                C_Canvas.Children.Add(new TopPanelItem(topItemconfig));
            }
        }

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ParentContainer == null)
                return;

            if (C_Slider.Value <= 0.1)
                ParentContainer.LockedPanel = false;
            else
                ParentContainer.LockedPanel = true;
        }
    }
}
