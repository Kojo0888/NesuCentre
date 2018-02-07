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
using System.Windows.Shapes;

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for MainContainerWindow.xaml
    /// </summary>
    public partial class MainContainerWindow : Window
    {
        public MainContainerWindow()
        {
            InitializeComponent();
            this.Top = 0;
            this.Left = 0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SubNodeBase.MainCanvas = C_Canvas;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
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
            if(Mouse.GetPosition(this).X + 1 >= System.Windows.SystemParameters.PrimaryScreenWidth)
            {
                foreach (var node in SubNodeBase.GetNodesWithStage(-1))
                {
                    node.HideNode(System.Windows.SystemParameters.PrimaryScreenWidth - 20, System.Windows.SystemParameters.PrimaryScreenHeight / 2 - 50);
                    SubNodeBase.allNodeList.Remove(node);
                }
            }
        }
    }
}
