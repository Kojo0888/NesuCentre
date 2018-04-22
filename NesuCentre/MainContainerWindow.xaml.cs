using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for MainContainerWindow.xaml
    /// </summary>
    public partial class MainContainerWindow : Window
    {
        private bool _draggingCondition { get; set; } = false;
        private bool _dragging { get; set; } = false;
        private bool _rootNodeShowed { get; set; } = false;//Does not have practical use yet
        private bool _blockRootNodeHide { get; set; } = false;

        public MainContainerWindow()
        {
            InitializeComponent();
            this.Top = 0;
            this.Left = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;

            BezierDefineStartCoordinates();

            //Releasing Mouse.Capture
            Mouse.AddPreviewMouseUpOutsideCapturedElementHandler(this, BezierReleaseMouseCapture);
            Mouse.AddPreviewMouseUpHandler(this, BezierReleaseMouseCapture);
        }

        #region HANDLERS

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SubNodeBase.MainCanvas = C_Canvas;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                _rootNodeShowed = false;
                for (int i = 0; i < C_Canvas.Children.Count; i++)
                {
                    if (C_Canvas.Children[i] is SubNodeBase)
                    {
                        C_Canvas.Children.Remove(C_Canvas.Children[i]);
                        i--;
                    }
                }
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            BezierDrag();
            RootNodeApperance();
        }

        private void C_MainControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingCondition)
            {
                _draggingCondition = false;
                _dragging = true;

                //Getting Mouse.Move events out of bounds of Visible controls
                Mouse.Capture(this);
            }
        }

        private void C_MainControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _draggingCondition = true;
            if (Mouse.GetPosition(this).X + 1 >= System.Windows.SystemParameters.PrimaryScreenWidth)
            {
                _blockRootNodeHide = true;
            }
        }

        private void MouseMove_Event(object sender, MouseEventArgs e)
        {
            this.C_MainControl_MouseMove(this, null);
        }


        private void C_MainControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _rootNodeShowed = true;
        }

        private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        #endregion

        public void RootNodeApperance()
        {
            if (Mouse.GetPosition(this).X + 1 >= System.Windows.SystemParameters.PrimaryScreenWidth)
            {
                if (!_blockRootNodeHide)
                {
                    _rootNodeShowed = false;
                    foreach (var node in SubNodeBase.GetNodesWithStage(-1))
                    {
                        node.HideNode(System.Windows.SystemParameters.PrimaryScreenWidth - 20, System.Windows.SystemParameters.PrimaryScreenHeight / 2 - 50);
                        SubNodeBase.allNodeList.Remove(node);
                    }
                }
            }
            else
            {
                _blockRootNodeHide = false;
            }
        }

        private void DebugWriteLine(string message)
        {
            Debug.WriteLine(DateTime.Now.ToShortTimeString() + ": " + message);
        }

        private void BezierDefineStartCoordinates()
        {
            this.C_BezierSegment1.Point1 = new Point(SystemParameters.PrimaryScreenWidth,
                SystemParameters.PrimaryScreenHeight / 2 - C_MainControl.Height / 2 + 10);
            this.C_BezierSegment1.Point2 = new Point(SystemParameters.PrimaryScreenWidth,
                SystemParameters.PrimaryScreenHeight / 2 - C_MainControl.Height / 2 + 50);
            this.C_BezierSegment1.Point3 = new Point(SystemParameters.PrimaryScreenWidth,
                SystemParameters.PrimaryScreenHeight / 2);

            this.C_BezierSegment2.Point1 = new Point(SystemParameters.PrimaryScreenWidth,
                SystemParameters.PrimaryScreenHeight / 2);
            this.C_BezierSegment2.Point2 = new Point(SystemParameters.PrimaryScreenWidth,
                SystemParameters.PrimaryScreenHeight / 2 - 10);
            this.C_BezierSegment2.Point3 = new Point(SystemParameters.PrimaryScreenWidth,
                SystemParameters.PrimaryScreenHeight / 2 + C_MainControl.Height / 2 - 10);

            this.C_CurveStart.StartPoint = this.C_BezierSegment1.Point1;
        }

        private void BezierDrag()
        {
            if (_dragging)
            {
                double bezierMultiplayer = 0.9;
                double positionX = (SystemParameters.PrimaryScreenWidth - Mouse.GetPosition(this).X) * bezierMultiplayer;
                positionX = SystemParameters.PrimaryScreenWidth - positionX;
                C_BezierSegment1.Point3 = new Point(positionX, C_BezierSegment1.Point3.Y);
                C_BezierSegment2.Point1 = new Point(positionX, C_BezierSegment2.Point1.Y);
            }
        }

        private void BezierReleaseMouseCapture(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
            _draggingCondition = false;
            Mouse.Capture(this, CaptureMode.None);
            C_BezierSegment1.Point3 = new Point(SystemParameters.PrimaryScreenWidth, C_BezierSegment1.Point3.Y);
            C_BezierSegment2.Point1 = new Point(SystemParameters.PrimaryScreenWidth, C_BezierSegment2.Point1.Y);
        }
    }
}
