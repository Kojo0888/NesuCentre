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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NesuCentre.Nodes.NodeControls
{
    /// <summary>
    /// Interaction logic for OsuNode.xaml
    /// </summary>
    public partial class OsuNode : NodeBase
    {
        private BeginStoryboard _hidingStoryboard { get; set; }
        private BeginStoryboard _startingStoryboard { get; set; }
        private BeginStoryboard _continuingStoryboard { get; set; }

        public OsuNode()
        {
            InitializeComponent();

            Canvas.SetTop(this, SystemParameters.PrimaryScreenHeight / 2 - this.Height / 2);

            NameScope.SetNameScope(this, new NameScope());


            InitializeHidingStoryboard();
            InitializeStartingStorybyard();
            InitializeContinuingStoryboard();

            this.Loaded += BeginStartingAnimationEvent;
            _startingStoryboard.Storyboard.Completed += BeginContinuingAnimationEvent;
            _hidingStoryboard.Storyboard.Completed += DisposeNode;

            C_NodeContainer_G.Width = this.Width * 0.65;
            C_NodeContainer_G.Height = this.Height * 0.65;
            //C_NodeContainer_G.Background = Brushes.BlanchedAlmond;
            C_NodeContainer_G.Opacity = 0.5;

            C_Version.Text = MainContainerWindow.CURRENT_VERISON;
        }

        private void BeginStartingAnimationEvent(object sender, EventArgs e)
        {
            _startingStoryboard.Storyboard.Begin(this, true);
            //_startingStoryboard.Storyboard.
        }

        private void BeginContinuingAnimationEvent(object sender, EventArgs e)
        {
            _continuingStoryboard.Storyboard.Begin(this, true);
        }

        private void DisposeNode(object sender, EventArgs e)
        {
            MainContainerWindow.MainContainer.RemoveOsuNode(this);
        }

        private void InitializeStartingStorybyard()
        {
            double scale = 15;
            TimeSpan appearDuration = new TimeSpan(0, 0, 0, 0, 300);

            _startingStoryboard = new BeginStoryboard();
            _startingStoryboard.Storyboard = new Storyboard();

            DoubleAnimation anim1 = new DoubleAnimation();
            anim1.From = SystemParameters.PrimaryScreenWidth;
            anim1.To = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
            anim1.Duration = appearDuration;
            _startingStoryboard.Storyboard.Children.Add(anim1);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTarget(anim1, C_OsuNode);

            //ellipse
            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.To = C_Ellipse.Width;
            anim2.From = scale;
            anim2.AutoReverse = false;
            anim2.Duration = appearDuration;
            _startingStoryboard.Storyboard.Children.Add(anim2);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(UserControl.WidthProperty));
            Storyboard.SetTarget(anim2, C_Ellipse);

            DoubleAnimation anim3 = new DoubleAnimation();
            anim3.To = C_Ellipse.Height;
            anim3.From = scale;
            anim3.AutoReverse = false;
            anim3.Duration = appearDuration;
            _startingStoryboard.Storyboard.Children.Add(anim3);
            Storyboard.SetTargetProperty(anim3, new PropertyPath(UserControl.HeightProperty));
            Storyboard.SetTarget(anim3, C_Ellipse);

            //ellipse pulse
            DoubleAnimation anim_ep = new DoubleAnimation();
            anim_ep.To = C_Ellipse_Pulse.Width;
            anim_ep.From = scale;
            anim_ep.AutoReverse = false;
            anim_ep.Duration = appearDuration;
            _startingStoryboard.Storyboard.Children.Add(anim_ep);
            Storyboard.SetTargetProperty(anim_ep, new PropertyPath(UserControl.WidthProperty));
            Storyboard.SetTarget(anim_ep, C_Ellipse_Pulse);

            DoubleAnimation anim_ep2 = new DoubleAnimation();
            anim_ep2.To = C_Ellipse_Pulse.Height;
            anim_ep2.From = scale;
            anim_ep2.AutoReverse = false;
            anim_ep2.Duration = appearDuration;
            _startingStoryboard.Storyboard.Children.Add(anim_ep2);
            Storyboard.SetTargetProperty(anim_ep2, new PropertyPath(UserControl.HeightProperty));
            Storyboard.SetTarget(anim_ep2, C_Ellipse_Pulse);
        }

        private void InitializeHidingStoryboard()
        {
            double scale = 15;
            TimeSpan appearDuration = new TimeSpan(0, 0, 0, 0, 300);

            _hidingStoryboard = new BeginStoryboard();
            _hidingStoryboard.Storyboard = new Storyboard();

            DoubleAnimation anim1 = new DoubleAnimation();
            anim1.To = SystemParameters.PrimaryScreenWidth + this.Width;
            //anim1.From = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
            anim1.Duration = appearDuration;
            anim1.AutoReverse = false;
            _hidingStoryboard.Storyboard.Children.Add(anim1);
            Storyboard.SetTargetProperty(anim1, new PropertyPath(Canvas.LeftProperty));
            Storyboard.SetTarget(anim1, C_OsuNode);

            //Ellipse
            DoubleAnimation anim2 = new DoubleAnimation();
            anim2.To = scale;
            anim2.Duration = appearDuration;
            anim2.AutoReverse = false;
            _hidingStoryboard.Storyboard.Children.Add(anim2);
            Storyboard.SetTargetProperty(anim2, new PropertyPath(UserControl.WidthProperty));
            Storyboard.SetTarget(anim2, C_Ellipse);

            DoubleAnimation anim3 = new DoubleAnimation();
            anim3.To = scale;
            anim3.Duration = appearDuration;
            anim3.AutoReverse = false;
            _hidingStoryboard.Storyboard.Children.Add(anim3);
            Storyboard.SetTargetProperty(anim3, new PropertyPath(UserControl.HeightProperty));
            Storyboard.SetTarget(anim3, C_Ellipse);

            //ellipse pulse
            DoubleAnimation anim_p = new DoubleAnimation();
            anim_p.To = scale;
            anim_p.Duration = appearDuration;
            anim_p.AutoReverse = false;
            _hidingStoryboard.Storyboard.Children.Add(anim_p);
            Storyboard.SetTargetProperty(anim_p, new PropertyPath(UserControl.WidthProperty));
            Storyboard.SetTarget(anim_p, C_Ellipse_Pulse);

            DoubleAnimation anim_p2 = new DoubleAnimation();
            anim_p2.To = scale;
            anim_p2.Duration = appearDuration;
            anim_p2.AutoReverse = false;
            _hidingStoryboard.Storyboard.Children.Add(anim_p2);
            Storyboard.SetTargetProperty(anim_p2, new PropertyPath(UserControl.HeightProperty));
            Storyboard.SetTarget(anim_p2, C_Ellipse_Pulse);

        }

        private void InitializeContinuingStoryboard()
        {
            TimeSpan appearDuration = new TimeSpan(0,0,0,0,300);
            double scale = 15;

            _continuingStoryboard = new BeginStoryboard();
            _continuingStoryboard.Storyboard = new Storyboard();

            var ease = new CubicEase() { EasingMode = EasingMode.EaseIn };
            bool autoreverse = true;
            Duration duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));
            TimeSpan beginTime = new TimeSpan(0, 0, 0, 0, 0);

            //DoubleAnimation anim1 = new DoubleAnimation();
            //anim1.From = SystemParameters.PrimaryScreenWidth / 2 - this.Width / 2;
            //anim1.To = anim1.From - scale;
            //anim1.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            //anim1.EasingFunction = ease;
            //anim1.AutoReverse = autoreverse;
            //anim1.Duration = duration;
            //anim1.BeginTime = beginTime;
            //_continuingStoryboard.Storyboard.Children.Add(anim1);
            //Storyboard.SetTargetProperty(anim1, new PropertyPath(Canvas.LeftProperty));
            //Storyboard.SetTarget(anim1, C_OsuNode);

            //DoubleAnimation anim2 = new DoubleAnimation();
            //anim2.From = SystemParameters.PrimaryScreenHeight / 2 - this.Height / 2;
            //anim2.To = anim2.From - scale;
            //anim2.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            //anim2.EasingFunction = ease;
            //anim2.AutoReverse = autoreverse;
            //anim2.Duration = duration;
            //anim2.BeginTime = beginTime;
            //_continuingStoryboard.Storyboard.Children.Add(anim2);
            //Storyboard.SetTargetProperty(anim2, new PropertyPath(Canvas.TopProperty));
            //Storyboard.SetTarget(anim2, C_OsuNode);

            //DoubleAnimation anim3 = new DoubleAnimation();
            //anim3.From = this.Width;
            //anim3.To = anim3.From + scale * 2;
            //anim3.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            //anim3.EasingFunction = ease;
            //anim3.AutoReverse = autoreverse;
            //anim3.Duration = duration;
            //anim3.BeginTime = beginTime;
            //_continuingStoryboard.Storyboard.Children.Add(anim3);
            //Storyboard.SetTargetProperty(anim3, new PropertyPath(UserControl.WidthProperty));
            //Storyboard.SetTarget(anim3, C_OsuNode);

            //DoubleAnimation anim4 = new DoubleAnimation();
            //anim4.From = this.Height;
            //anim4.To = anim4.From + scale * 2;
            //anim4.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            //anim4.EasingFunction = ease;
            //anim4.AutoReverse = autoreverse;
            //anim4.Duration = duration;
            //anim4.BeginTime = beginTime;
            //_continuingStoryboard.Storyboard.Children.Add(anim4);
            //Storyboard.SetTargetProperty(anim4, new PropertyPath(UserControl.HeightProperty));
            //Storyboard.SetTarget(anim4, C_OsuNode);

            //Ellipse
            DoubleAnimation anim_e1 = new DoubleAnimation();
            anim_e1.From = C_Ellipse.Width;
            anim_e1.To = anim_e1.From + scale * 2;
            anim_e1.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            anim_e1.EasingFunction = ease;
            anim_e1.AutoReverse = autoreverse;
            anim_e1.Duration = duration;
            anim_e1.BeginTime = beginTime;
            _continuingStoryboard.Storyboard.Children.Add(anim_e1);
            Storyboard.SetTargetProperty(anim_e1, new PropertyPath(UserControl.WidthProperty));
            Storyboard.SetTarget(anim_e1, C_Ellipse);

            DoubleAnimation anim_e2 = new DoubleAnimation();
            anim_e2.From = C_Ellipse.Height;
            anim_e2.To = anim_e2.From + scale * 2;
            anim_e2.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            anim_e2.EasingFunction = ease;
            anim_e2.AutoReverse = autoreverse;
            anim_e2.Duration = duration;
            anim_e2.BeginTime = beginTime;
            _continuingStoryboard.Storyboard.Children.Add(anim_e2);
            Storyboard.SetTargetProperty(anim_e2, new PropertyPath(UserControl.HeightProperty));
            Storyboard.SetTarget(anim_e2, C_Ellipse);

            //Elipse pulse
            var pulseEase = new CubicEase() { EasingMode = EasingMode.EaseOut };

            DoubleAnimation anim_ep1 = new DoubleAnimation();
            anim_ep1.From = C_Ellipse_Pulse.Width;
            anim_ep1.To = anim_ep1.From + scale * 2;
            anim_ep1.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            anim_ep1.EasingFunction = pulseEase;
            anim_ep1.AutoReverse = autoreverse;
            anim_ep1.Duration = duration;
            anim_ep1.BeginTime = new TimeSpan(0, 0, 0, 0, 500);
            _continuingStoryboard.Storyboard.Children.Add(anim_ep1);
            Storyboard.SetTargetProperty(anim_ep1, new PropertyPath(UserControl.WidthProperty));
            Storyboard.SetTarget(anim_ep1, C_Ellipse_Pulse);

            DoubleAnimation anim_ep2 = new DoubleAnimation();
            anim_ep2.From = C_Ellipse_Pulse.Height;
            anim_ep2.To = anim_ep2.From + scale * 2;
            anim_ep2.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            anim_ep2.EasingFunction = pulseEase;
            anim_ep2.AutoReverse = autoreverse;
            anim_ep2.Duration = duration;
            anim_ep2.BeginTime = new TimeSpan(0, 0, 0, 0, 500);
            _continuingStoryboard.Storyboard.Children.Add(anim_ep2);
            Storyboard.SetTargetProperty(anim_ep2, new PropertyPath(UserControl.HeightProperty));
            Storyboard.SetTarget(anim_ep2, C_Ellipse_Pulse);

            DoubleAnimation anim_ep3 = new DoubleAnimation();
            anim_ep3.From = 1;
            anim_ep3.To = 0;
            anim_ep3.RepeatBehavior = new System.Windows.Media.Animation.RepeatBehavior(int.MaxValue);
            anim_ep3.EasingFunction = pulseEase;
            anim_ep3.AutoReverse = false;
            anim_ep3.Duration = new Duration(new TimeSpan(0, 0, 0, 1));
            anim_ep3.BeginTime = new TimeSpan(0, 0, 0, 0, 500);
            _continuingStoryboard.Storyboard.Children.Add(anim_ep3);
            Storyboard.SetTargetProperty(anim_ep3, new PropertyPath(UserControl.OpacityProperty));
            Storyboard.SetTarget(anim_ep3, C_Ellipse_Pulse);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NodeConfigurationWindow window = new NodeConfigurationWindow();
            window.ShowDialog();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void C_OsuNode_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            BeginHidingNode();
        }

        private void BeginHidingNode()
        {
            //S_MainStoryboard.Pause();
            //S_MainStoryboard.Stop();
            //S_MainStoryboard.Remove();
            //S_MainStoryboard.Freeze();
            //BS_MainStoryboard.
            //S_MainStoryboard.

            //S_HideStoryboard.Begin();

            _continuingStoryboard.Storyboard.Stop(this);
            _hidingStoryboard.Storyboard.Begin(this, true);
        }
    }
}
