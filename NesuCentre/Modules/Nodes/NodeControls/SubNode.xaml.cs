using NesuCentre.NodeConfiguration;
using NesuCentre.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SubNode : SubNodeBase
    {

        public double CanvasTop
        {
            get { return Canvas.GetTop(this); }
            set { Canvas.SetTop(this, value); }
        }

        public double CanvasLeft
        {
            get { return Canvas.GetLeft(this); }
            set { Canvas.SetLeft(this, value); }
        }

        public SubNode()
        {
            InitializeComponent();
            //Canvas.SetLeft(this, System.Windows.SystemParameters.PrimaryScreenWidth - 50);
            //Canvas.SetTop(this, System.Windows.SystemParameters.PrimaryScreenHeight / 2 - 50);
        }

        public SubNode(double StartX, double StartY, double EndX, double EndY, int stage, NodeStructure configuration) 
            : base(StartX, StartY, EndX, EndY, stage, configuration)
        {
            InitializeComponent();

            A_EjectX_DA.From = StartX;
            A_EjectX_DA.To = EndX;
            A_EjectY_DA.From = StartY;
            A_EjectY_DA.To = EndY;

            A_HideX_DA.From = EndX;
            A_HideX_DA.To = StartX;
            A_HideY_DA.From = EndY;
            A_HideY_DA.To = StartY;

            S_Eject.Storyboard.Begin();

            base.S_Eject = this.S_Eject;
            base.S_Hide = this.S_Hide;

            C_NodeContainer_G.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            C_NodeContainer_G.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            if (configuration != null)
                C_Name.Text = configuration.Details.Name;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Draggable
            //if (e.ChangedButton == MouseButton.Left)
            //    this.DragMove();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            //StartEjecting();
            //HideAttachedNodes();
            //NodeWindow.HideNodesWithStage(base.nodeStage + 1, this);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            //HideNodes();
        }

        private void NodeWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExecuteNode();
        }

        private void C_MainUserControl_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(this.nodeConfig != null && this.nodeConfig.Details != null && this.nodeConfig.Details.Path != null)
            {
                ExecuteNodeShell();
            }
        }
    }
}
