using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for TopPanelContainer.xaml
    /// </summary>
    public partial class TopPanelContainer : UserControl
    {
        public static double ADDITIONAL_MARGIN_ON_PANEL_SIDES => 300.0d;

        public static double TILT_ROTATE_STRENGTH => 0.0015d;

        public static double TILT_SLIDE_STRENGTH => 0.020d;

        private static bool _lockedPanel;

        private static bool _appearedPanel;

        public bool LockedPanel 
        {
            get
            {
                return _lockedPanel;
            }
            set
            {
                _lockedPanel = value;
            }
        }

        public TopPanelContainer()
        {
            InitializeComponent();
            InitializeGraphicalMess();
            C_MainPanelContainer.ParentContainer = this;
            DataContext = this;
        }

        private void InitializeGraphicalMess()
        {
            //MediaTimeline.DesiredFrameRateProperty.OverrideMetadata(typeof(System.Windows.Media.Animation.Timeline), new FrameworkPropertyMetadata(60));
            var divider = 1.90d;//variable just for calibration
            var leftMargin = SystemParameters.PrimaryScreenWidth / divider;
            var borderPanelMargin = ADDITIONAL_MARGIN_ON_PANEL_SIDES;
            //Blend design automatically sets MArgins, instead of Transform matrix... it works enough = don't touch it :)
            C_MainPanelContainer.Margin = new Thickness(leftMargin - borderPanelMargin, -C_MainPanelContainer.Height, -leftMargin -20, 0);//-20 tilt additional panel space, works enough. for this change it is necessary to rebuild whole animation. I shouldn't use designer at first place...
            C_MainPanelContainer.Width = SystemParameters.PrimaryScreenWidth + borderPanelMargin * 2;

            var defaultCanvasMargin = C_MainPanelContainer.C_Canvas.Margin.Bottom;//Assume that all are the same
            C_MainPanelContainer.C_Canvas.Margin = new Thickness(borderPanelMargin + defaultCanvasMargin, defaultCanvasMargin, borderPanelMargin + defaultCanvasMargin, defaultCanvasMargin);

            //butifull wpf
            Storyboard panelAppear = this.FindResource("S_PanelAppear") as Storyboard;
            var jebanywpf = panelAppear.Children[1] as DoubleAnimationUsingKeyFrames;
            var jebanykeyframy = jebanywpf.KeyFrames[0] as EasingDoubleKeyFrame;
            jebanykeyframy.Value = -leftMargin;//-SystemParameters.PrimaryScreenWidth + leftMargin; 
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!_lockedPanel)
            {
                var story = this.FindResource("S_PanelHide") as Storyboard;
                story.Begin(this);
                _appearedPanel = false;
            }
        }

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var mouse = Mouse.PrimaryDevice;
            var args = new MouseEventArgs(mouse, 0);
            args.RoutedEvent = Mouse.MouseMoveEvent;
            RaiseEvent(args);

            TiltTopPanel();
        }

        private void TiltTopPanel()
        {
            if (_appearedPanel)
            {
                double width = SystemParameters.PrimaryScreenWidth;
                double mouseX = Mouse.GetPosition(this).X;
                double angleValue = (mouseX - width / 2);

                RotateTopPanel(angleValue * TILT_ROTATE_STRENGTH);
                SlideTopPanel(angleValue * TILT_SLIDE_STRENGTH);
            }
        }

        private void RotateTopPanel(double value)
        {
            var trans = C_MainGrid.RenderTransform as TransformGroup;
            var rotateTransform = trans.Children.OfType<RotateTransform>()?.FirstOrDefault();
            if (rotateTransform != null)
            {
                trans.Children.Remove(rotateTransform);
                rotateTransform = new RotateTransform(value);
                trans.Children.Add(rotateTransform);
            }
        }

        private void SlideTopPanel(double value)
        {
            var trans = C_MainGrid.RenderTransform as TransformGroup;
            var translateTransform = trans.Children.OfType<TranslateTransform>()?.FirstOrDefault();
            if (translateTransform != null)
            {
                trans.Children.Remove(translateTransform);
                translateTransform = new TranslateTransform(value * TILT_SLIDE_STRENGTH, 0);
                trans.Children.Add(translateTransform);
            }
        }

        private void UserControl_PreviewDrop(object sender, DragEventArgs e)
        {
            //Storyboard story = this.FindResource("S_PanelAppear") as Storyboard;
            //story?.Begin(this);
        }

        private void UserControl_DragOver(object sender, DragEventArgs e)
        {
            if (!_appearedPanel)
            {
                
                Storyboard story = this.FindResource("S_PanelAppear") as Storyboard;
                story.Completed += (x,s) => { _appearedPanel = true; };
                story?.Begin(this);
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_appearedPanel)
            {
                Storyboard story = this.FindResource("S_PanelAppear") as Storyboard;
                story.Completed += (x, s) => { _appearedPanel = true; };
                story?.Begin(this);
            }
        }
    }
}
