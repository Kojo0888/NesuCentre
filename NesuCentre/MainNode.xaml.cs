﻿using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainNode : UserControl, INode
    {
        public bool Ejecting { get; set; }
        public bool AbortEjecting { get; set; }
        public bool Hiding { get; set; }
        public bool AbortHiding { get; set; }

        public MainNode()
        {
            SubNodeBase.MainParentNode = this;
            InitializeComponent();
            Canvas.SetLeft(this, System.Windows.SystemParameters.PrimaryScreenWidth - 20);
            A_EllipseAnimationEject_DA.From = System.Windows.SystemParameters.PrimaryScreenWidth - 20;
            A_EllipseAnimationEject_DA.To = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
            A_EllipseAnimationHide_DA.From = System.Windows.SystemParameters.PrimaryScreenWidth - 50;
            A_EllipseAnimationHide_DA.To = System.Windows.SystemParameters.PrimaryScreenWidth - 20;
            Canvas.SetTop(this, System.Windows.SystemParameters.PrimaryScreenHeight / 2 - 50);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Draggable
            //if (e.ChangedButton == MouseButton.Left)
            //    this.DragMove();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Hiding)
            {
                AbortEjecting = false;
                AbortHiding = true;
            }

            Ejecting = true;
            Task.Run(() =>
            {
                double distance = 10.0d;
                while (distance > 0.01)
                {
                    if (CheckEjectingAbort()) break;
                    Thread.Sleep(30);

                    if (CheckEjectingAbort()) break;
                    distance = this.Dispatcher.Invoke(() => { return ShiftCentralNode(100, true); });

                    if (CheckEjectingAbort()) break;
                }
                Ejecting = false;
                AbortEjecting = false;
            });
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Ejecting)
            {
                AbortEjecting = true;
                AbortHiding = false;
            }

            Hiding = true;
            Task.Run(() =>
            {
                double distance = 10.0d;
                while (distance > 0.01)
                {
                    if (CheckHideAbort()) break;
                    Thread.Sleep(30);

                    if (CheckHideAbort()) break;
                    distance = this.Dispatcher.Invoke(() => { return ShiftCentralNode(50, false); });

                    if (CheckHideAbort()) break;
                }
                Hiding = false;
                AbortHiding = false;
            });
        }
        
        private bool CheckEjectingAbort()
        {
            if (AbortEjecting)
            {
                AbortEjecting = false;
                return true;
            }
            return false;
        }

        private bool CheckHideAbort()
        {
            if (AbortHiding)
            {
                AbortHiding = false;
                return true;
            }
            return false;
        }

        private double ShiftCentralNode(double leftSpace, bool left)
        {
            double distance;
            if (left)
                distance = Canvas.GetLeft(this) - (System.Windows.SystemParameters.PrimaryScreenWidth - leftSpace);
            else
                distance = (System.Windows.SystemParameters.PrimaryScreenWidth - leftSpace) - Canvas.GetLeft(this);

            distance /= 5;

            if (left)
                Canvas.SetLeft(this, Canvas.GetLeft(this) - distance);
            else
                Canvas.SetLeft(this, Canvas.GetLeft(this) + distance);
            return distance;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DefineNodes(1);
        }

        private void DefineNodes(int nodes)
        {
            for (int i = 0; i < nodes; i++)
            {
                DefineNode(i, nodes);
            }
        }

        private void DefineNode(int index, int max)
        {
            var newSubNode = new SubNodeWindow(Canvas.GetLeft(this), Canvas.GetTop(this), Canvas.GetLeft(this) - 50,
                Canvas.GetTop(this) - index * 70, 1, ConfigurationCentre.RootNode);
            SubNodeBase.allNodeList.Add(newSubNode);
            SubNodeWindow.MainCanvas.Children.Add(newSubNode);
            //newSubNode.S_EjectX.Storyboard.Begin();
            //newSubNode.S_EjectY.Storyboard.Begin();
            //newSubNode.StartEjecting();
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
    }
}
