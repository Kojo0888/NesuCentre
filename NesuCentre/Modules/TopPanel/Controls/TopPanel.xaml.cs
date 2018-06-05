using NesuCentre.Configurations.TopPanelConfiguration;
using NesuCentre.Modules.TopPanel.Controls;
using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NesuCentre.Modules.TopPanel.Controls
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
            C_Canvas.Children.Add(new TopPanelItem(config, this));
            ConfigurationCentre.ConfigurationChanged = true;
        }

        private void InitializeAllTopPanelItems()
        {
            foreach (var topItemconfig in ConfigurationCentre.Configuration.TopPanelItems)
            {
                C_Canvas.Children.Add(new TopPanelItem(topItemconfig, this));
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

        private void C_TrashImage_MouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("MouseMove");

        }

        private void C_TrashImage_MouseEnter(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("MouseEnter");

            //Storyboard story = this.FindResource("S_TrashCanAppear") as Storyboard;
            //story?.Begin(this);
        }

        private void C_TrashImage_MouseLeave(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("MouseLeves");

            //Storyboard story = this.FindResource("S_TrashCanHide") as Storyboard;
            //story?.Begin(this);
        }

        public void StartTrashCanAppearAnimation()
        {
            if(C_TrashCan.Opacity < 0.1)
            {
                FindAndStartStoryboard("S_LockControlHide");
                FindAndStartStoryboard("S_TrashCanAppear");
            }
        }

        private void FindAndStartStoryboard(string storyboardResourceKey)
        {
            Storyboard story = this.FindResource(storyboardResourceKey) as Storyboard;
            story?.Begin(this);
        }

        public void StartTrashCanHideAnimation()
        {
            FindAndStartStoryboard("S_LockControlAppear");
            FindAndStartStoryboard("S_TrashCanHide");
        }

        public bool IsMouseOverTrashCan()
        {
            System.Windows.Point point = Mouse.GetPosition(C_TrashCan);
            Debug.WriteLine(point);

            if (point.X > 0 && point.Y > 0)
                return true;
            else
                return false;
            //return C_TrashCan.IsMouseOver;
        }

        public void RemoveTopPanelItem(TopPanelItem item)
        {
            C_Canvas.Children.Remove(item);
            ConfigurationCentre.Configuration.TopPanelItems.Remove(item.Configuration);
        }
    }
}
