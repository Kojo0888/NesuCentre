using NesuCentre.NodeConfiguration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NesuCentre.Nodes
{
    public class SubNodeBase : NodeBase, INode
    {
        public bool Ejecting { get; set; }
        public bool AbortEjecting { get; set; }
        public bool Hiding { get; set; }
        public bool AbortHiding { get; set; }

        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }

        public double Multiplayer { get; set; } = 5;
        public double MovementDistance { get; set; } = 50;

        private Task _ejectingTask;
        private Task _hidingTask;

        protected NodeStructure nodeConfig;
        private List<SubNodeBase> nodeList = new List<SubNodeBase>();

        public static List<SubNodeBase> allNodeList = new List<SubNodeBase>();

        public static Canvas MainCanvas { get; set; }
        public static MainNode MainParentNode { get; set; }

        public int nodeStage { get; set; }
        private bool _ignoreFirstMouseLeave { get; set; } = false;
        public bool CheckIgnoreFirstMouseLeave { get { return _ignoreFirstMouseLeave; } }

        public BeginStoryboard S_Eject { get; set; }
        public BeginStoryboard S_Hide { get; set; }

        #region enptyRoutedevent

        public static readonly RoutedEvent EmptyRoutedEvent = EventManager.RegisterRoutedEvent("EmptyEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SubNodeBase));

        public event RoutedEventHandler EmptyEvent
        {
            add
            {
                this.AddHandler(EmptyRoutedEvent, value);
            }

            remove
            {
                this.RemoveHandler(EmptyRoutedEvent, value);
            }
        }

        #endregion


        public SubNodeBase()
        {

        }

        public SubNodeBase(double StartX, double StartY, double EndX, double EndY, int stage, NodeStructure configuration) : base()
        {
            this.StartX = StartX;
            this.StartY = StartY;
            this.EndX = EndX;
            this.EndY = EndY;
            this.nodeStage = stage;
            Canvas.SetLeft(this, StartX);
            Canvas.SetTop(this, StartY);
            _ignoreFirstMouseLeave = true;
            nodeConfig = configuration;
        }

        #region Obsolete methods
        [Obsolete]
        public void StartEjecting()
        {
            if (Hiding)
            {
                AbortHiding = true;
                AbortEjecting = false;
            }

            if (!Ejecting)
            {
                Ejecting = true;
                Task.Run(() =>
                {
                    Tuple<double, double> distance = new Tuple<double, double>(100, 100);
                    while (distance.Item1 > 0.1 || distance.Item2 > 0.1)
                    {
                        if (CheckEjectingAbort()) return;
                        Thread.Sleep(23);
                        //Task.Delay(50);
                        if (CheckEjectingAbort()) return;
                        distance = this.Dispatcher.Invoke(() =>
                        {
                            return PunchNode(EndX, EndY);
                        });
                        if (CheckEjectingAbort()) return;
                        Debug.WriteLine($"Ejecting {distance} \tStartX:{StartX} EndX:{EndX}");
                    }
                    AbortEjecting = false;
                    Ejecting = false;
                    _ignoreFirstMouseLeave = false;
                    Debug.WriteLine($"Ejecting ended");
                });
            }
        }

        [Obsolete]
        public void StartHiding(bool removeAfter)
        {
            if (_ignoreFirstMouseLeave)
                return;

            if (Ejecting)
            {
                AbortEjecting = true;
                AbortHiding = false;
            }

            if (!Hiding)
            {
                Hiding = true;
                Task.Run(() =>
                {
                    var speed = 20;
                    Tuple<double, double> distance = new Tuple<double, double>(-100, -100);
                    while (!((distance.Item1 < 3 && distance.Item1 > -3) && (distance.Item2 < 3 && distance.Item2 > -3)))
                    //while (!((distance.Item1 < speed && distance.Item1 > -speed) 
                    //    && (distance.Item2 < speed && distance.Item2 > -speed)))
                    {
                        if (CheckHideAbort()) return;
                        Thread.Sleep(23);
                        //Task.Delay(50);
                        if (CheckHideAbort()) return;
                        distance = this.Dispatcher.Invoke(() =>
                        {
                            return PunchNode(StartX, StartY);
                        });
                        if (CheckHideAbort()) return;
                        Debug.WriteLine($"Hiding {distance}");
                    }
                    AbortHiding = false;
                    Hiding = false;
                    Debug.WriteLine($"Hiding ended");
                    if (removeAfter)
                        this.Dispatcher.Invoke(() =>
                        {
                            CloseNode(this);
                        });
                });
            }
        }

        [Obsolete]
        public void HideAttachedNodes()
        {
            foreach (var node in nodeList)
            {
                node.StartHiding(true);
            }
        }

        [Obsolete]
        private Tuple<double, double> PunchNode(double destenationX, double destenationY)
        {
            double distanceLeft = Canvas.GetLeft(this) - destenationX;
            if (distanceLeft != 0)
            {
                distanceLeft /= Multiplayer;
                Canvas.SetLeft(this, Canvas.GetLeft(this) - distanceLeft);
            }

            double distanceTop = Canvas.GetTop(this) - destenationY;
            if (distanceTop != 0)
            {
                distanceTop /= Multiplayer;
                Canvas.SetTop(this, Canvas.GetTop(this) - distanceTop);
            }

            return new Tuple<double, double>(distanceLeft, distanceTop);
        }

        [Obsolete]
        private Tuple<double, double> MoveNode(double destenationX, double destenationY, double speed = 0)
        {
            if (speed == 0)
                speed = Multiplayer;

            double distanceLeft = Canvas.GetLeft(this) - destenationX;
            //distanceLeft = (distanceLeft / Math.Abs(distanceLeft));
            //if (distanceLeft == 0)
            //    distanceLeft = speed;
            if (distanceLeft > 0 && distanceLeft > speed)
                distanceLeft = speed;
            if (distanceLeft < 0 && distanceLeft < -speed)
                distanceLeft = -speed;

            if (distanceLeft > 0 || distanceLeft < 0)
                Canvas.SetLeft(this, Canvas.GetLeft(this) - distanceLeft);

            double distanceTop = Canvas.GetTop(this) - destenationY;
            //distanceTop = (distanceTop / Math.Abs(distanceTop));
            //if (distanceTop == 0)
            //    distanceTop = speed;
            //distanceTop = distanceTop;
            if (distanceTop > 0 && distanceTop > speed)
                distanceTop = speed;
            if (distanceTop < 0 && distanceTop < -speed)
                distanceTop = -speed;

            if (distanceTop > 0 || distanceTop < 0)
                Canvas.SetTop(this, Canvas.GetTop(this) - distanceTop);

            return new Tuple<double, double>(Canvas.GetLeft(this) - destenationX, Canvas.GetTop(this) - destenationY);
        }

        [Obsolete]
        private bool CheckEjectingAbort()
        {
            if (AbortEjecting)
            {
                AbortEjecting = false;
                Ejecting = false;
                Debug.WriteLine("Ejecting cancellation detected");
                return true;
            }
            return false;
        }

        [Obsolete]
        private bool CheckHideAbort()
        {
            if (AbortHiding)
            {
                AbortHiding = false;
                Hiding = false;
                Debug.WriteLine("Hiding cancellation detected");
                return true;
            }
            return false;
        }
        #endregion

        public List<SubNodeBase> DefineNodes()
        {
            if (nodeConfig == null)
            {
                Debug.WriteLine("Node coniguration is null");
                return null;
            }

            nodeList.Clear();
            for (int i = 0; i < nodeConfig.Nodes.Count; i++)
            {
                var node = DefineNode(i, nodeConfig.Nodes.Count, nodeConfig.Nodes[i]);
                nodeList.Add(node);
                allNodeList.Add(node);
            }
            return nodeList;
        }

        private SubNodeBase DefineNode(int index, int max, NodeStructure configuration)
        {
            var minusHalfMax = -(max / 2);
            var indexFromMinusHalfMax = minusHalfMax + index;
            var forthPartOfPI = Math.PI / 4;
            //var 
            var multiplierForLeftMove = Math.Sin(Math.PI / 2 + (Math.PI / 3.95 * indexFromMinusHalfMax));
            var multiplierForTopMove = Math.Cos(Math.PI / 2 + (Math.PI / 3.95 * indexFromMinusHalfMax));
            var radius = 120 + 5 * max / 2;
            var newSubNode = new SubNode(Canvas.GetLeft(this),
                Canvas.GetTop(this),
                Canvas.GetLeft(this) - multiplierForLeftMove * radius,
                Canvas.GetTop(this) - multiplierForTopMove * radius,
                //Canvas.GetTop(this) + ((max - 1) * 100 / 2) - index * 100,
                nodeStage + 1, configuration);

            //newSubNode.StartEjecting();
            return newSubNode;
        }

        public void CloseNodes()
        {
            nodeList.ForEach(fe => MainCanvas.Children.Remove(fe));
            nodeList.Clear();

            //for (int i = 0; i < nodeWindowList.Count; i++)
            //{
            //    CloseNode(nodeWindowList[i]);
            //}
        }

        private void CloseNode(SubNodeBase node)
        {
            allNodeList.Remove(node);
            MainCanvas.Children.Remove(node);
        }

        public static void HideNodesWithStage(int stage, INode senderNode)
        {
            foreach (var node in GetNodesWithStage(stage))
            {
                node.SetEndingPoint(Canvas.GetLeft(senderNode as UIElement), Canvas.GetTop(senderNode as UIElement));

                if (senderNode == node)
                    continue;

                SetNodeRemovadAfterHiding(node);
                node.S_Hide.Storyboard.Begin();
            }
        }

        private void SetEndingPoint(double endX, double endY)
        {
            var endAnimationX = S_Hide.Storyboard.Children.FirstOrDefault(f => f.Name == "A_HideX_DA") as DoubleAnimation;
            if (endAnimationX != null)
                endAnimationX.To = endX;

            var endAnimationY = S_Hide.Storyboard.Children.FirstOrDefault(f => f.Name == "A_HideY_DA") as DoubleAnimation;
            if (endAnimationY != null)
                endAnimationY.To = endY;
        }

        public static List<SubNodeBase> GetNodesWithStage(int stage)
        {
            return allNodeList.Where(w => w.nodeStage >= stage).ToList();
        }

        public void HideNode(double X, double Y)
        {
            SetEndingPoint(X, Y);
            SetNodeRemovadAfterHiding(this);
            S_Hide.Storyboard.Begin();
        }

        public static void SetNodeRemovadAfterHiding(SubNodeBase node)
        {
            node.S_Hide.Storyboard.Completed += ((s, o) =>
            {
                node.Dispatcher.Invoke(() =>
                {
                    node.CloseNode(node);
                });
            });
        }

        public void ExecuteNode()
        {
            if (this.nodeConfig != null && (this.nodeConfig.Nodes == null || this.nodeConfig.Nodes.Count() == 0))
            {
                ExecuteNodeShell();
            }
            else
            {
                HideNodesWithStage(nodeStage, this);
                var nodes = DefineNodes();
                foreach (var node in nodes)
                {
                    MainCanvas.Children.Add(node);
                }
            }
        }

        public void ExecuteNodeShell()
        {
            try
            {
                ExecuteNodeBase();
                HideNodesWithStage(0, SubNodeBase.MainParentNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message);
            }
        }

        public void ExecuteNodeBase()
        {
            if(this.nodeConfig.Details.Setting is NodeConfiguration.Structure.NodeSettingNewestOldestFileOrDirectory)
            {
                var config = this.nodeConfig.Details.Setting as NodeConfiguration.Structure.NodeSettingNewestOldestFileOrDirectory;
                RunNewestOldestPath(config.Files, config.Directories, config.DateType);
            }
            else
            {
                Process.Start(this.nodeConfig.Details.Path);
            }
        }

        public void RunNewestOldestPath(bool files, bool dictionaries, PathDate dateType)
        {
            if (Directory.Exists(this.nodeConfig.Details.Path))
            {
                DateTime newestDateTime = new DateTime();
                string newestPath = string.Empty;
                List<string> paths = new List<string>();

                if (files)
                    paths.AddRange(Directory.GetFiles(this.nodeConfig.Details.Path));
                if (dictionaries)
                    paths.AddRange(Directory.GetDirectories(this.nodeConfig.Details.Path));

                foreach (var path in paths)
                {
                    DateTime? dt = null;
                    if (dateType == PathDate.LastWriteTime) dt = File.GetLastWriteTime(path);
                    if (dateType == PathDate.LastAccessTime) dt = File.GetLastAccessTime(path);
                    if (dateType == PathDate.CreationTime) dt = File.GetCreationTime(path);

                    if (dt != null && dt.Value.Ticks > newestDateTime.Ticks)
                    {
                        newestDateTime = dt.Value;
                        newestPath = path;
                    }
                }
                if (!string.IsNullOrEmpty(newestPath))
                    Process.Start(newestPath);
            }
            else
            {
                MessageBox.Show("Path is not a folder");
            }
        }
    }
}
