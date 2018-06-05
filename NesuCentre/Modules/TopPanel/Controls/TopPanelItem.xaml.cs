using NesuCentre.Configurations.TopPanelConfiguration;
using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

namespace NesuCentre.Modules.TopPanel.Controls
{
    /// <summary>
    /// Interaction logic for TopPanelItem.xaml
    /// </summary>
    public partial class TopPanelItem : UserControl
    {
        public TopPanelItemConfiguration Configuration;

        private TopPanel _parentPanel;

        private System.Windows.Point _draggingPointOnItem;
        private bool _dragging { get; set; }

        //public TopPanelItem()
        //{
        //    InitializeComponent();
        //}

        public TopPanelItem(TopPanelItemConfiguration config, TopPanel parent)
        {
            InitializeComponent();
            Configuration = config;
            C_Name.Text = System.IO.Path.GetFileNameWithoutExtension(config.Path);
            System.Drawing.Icon extractedIcon = System.Drawing.Icon.ExtractAssociatedIcon(config.Path);
            C_Icon.Source = Convert(extractedIcon.ToBitmap());

            Canvas.SetLeft(this, config.X);
            Canvas.SetTop(this, config.Y);

            _parentPanel = parent;

            Mouse.AddPreviewMouseUpOutsideCapturedElementHandler(this, ReleaseMouseCapture);
            Mouse.AddPreviewMouseUpHandler(this, ReleaseMouseCapture);
        }

        private void ReleaseMouseCapture(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this, CaptureMode.None);
            _parentPanel.StartTrashCanHideAnimation();
            if (_parentPanel.IsMouseOverTrashCan())
            {
                _parentPanel.RemoveTopPanelItem(this);
            }
            _dragging = false;
        }

        public BitmapImage Convert(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseOver)
            {
                _draggingPointOnItem = Mouse.GetPosition(this);
                Mouse.Capture(this, CaptureMode.Element);
                
                _dragging = true;
            }
        }

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(this.Configuration.Path);
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                var point = Mouse.GetPosition(this.Parent as Canvas);
                SetCoordinates(point.X - _draggingPointOnItem.X, point.Y - _draggingPointOnItem.Y);
            }
        }

        private void SetCoordinates(double X, double Y)
        {
            Canvas parent = Parent as Canvas;

            if (X < 0)
                X = 0;
            else if (X > parent.ActualWidth - this.ActualWidth)
                X = parent.ActualWidth - this.ActualWidth;

            if (Y < 0)
                Y = 0;
            else if (Y > parent.ActualHeight - this.ActualHeight)
                Y = parent.ActualHeight - this.ActualHeight;

            this.Configuration.X = X;
            this.Configuration.Y = Y;
            Canvas.SetLeft(this, X);
            Canvas.SetTop(this, Y);

            _parentPanel.StartTrashCanAppearAnimation();

            ConfigurationCentre.ConfigurationChanged = true;
        }

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("PreveiwMouseMove" + new Random().Next());
        }
    }
}
