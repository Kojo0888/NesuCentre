using NesuCentre.NodeConfiguration;
using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for NodeConfigurationWindow.xaml`
    /// </summary>
    public partial class NodeConfigurationWindow : Window
    {
        public NodeStructure CurrentNode { get; set; }

        public List<NodeStructure> PathNodes = new List<NodeStructure>();

        public NodeConfigurationWindow()
        {
            InitializeComponent();
            ConfigurationCentre.LoadConfiguration();
            CurrentNode = ConfigurationCentre.RootNode;
            C_NodeList.ItemsSource = CurrentNode.Nodes;
            RefreshListView();
            RefreshPath();
        }

        private void NewNode_Click(object sender, RoutedEventArgs e)
        {
            CurrentNode.Nodes.Add(new NodeStructure());
            C_NodeList.ItemsSource = CurrentNode.Nodes;
            RefreshListView();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            //back
            if (PathNodes.Count == 0)
                return;
            CurrentNode = PathNodes[PathNodes.Count - 1];
            C_NodeList.ItemsSource = CurrentNode.Nodes;
            RefreshListView();

            if(PathNodes.Count > 0)
                PathNodes.RemoveAt(PathNodes.Count - 1);
            RefreshPath();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //save
            ConfigurationCentre.SaveConfiguration();
        }

        private void C_NodeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //select node
            var selection = C_NodeList.SelectedItem as NodeStructure;
            if (selection == null)
            {
                Debug.WriteLine("Selection is null");
                return;
            }
            PathNodes.Add(CurrentNode);
            CurrentNode = selection;
            C_NodeList.ItemsSource = CurrentNode.Nodes;
            RefreshListView();

            RefreshPath();
        }

        private void RefreshListView()
        {
            CollectionViewSource.GetDefaultView(C_NodeList.ItemsSource).Refresh();
        }

        private void RefreshPath()
        {
            List<string> pathElements = new List<string>();
            pathElements.AddRange(PathNodes.Select(s => s.Details.Name));
            pathElements.Add(CurrentNode.Details.Name);
            C_Path.Text = string.Join(">>", pathElements);
        }

        private void C_NodeList_KeyDown(object sender, KeyEventArgs e)
        {
            //remove
            if(e.Key == Key.Delete)
            {
                var toRemove = C_NodeList.SelectedItem as NodeStructure;
                if (toRemove == null)
                {
                    Debug.WriteLine("ToRemove variable is null");
                    return;
                }

                CurrentNode.Nodes.Remove(toRemove);
                C_NodeList.ItemsSource = CurrentNode.Nodes;
                RefreshListView();
            }
        }

        private void C_NodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshListView();
        }

        private void C_NodeName_TBX_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView();
        }

        private void C_NodeList_Drop(object sender, DragEventArgs e)
        {
            string filepath = (e.Data.GetData(DataFormats.FileDrop) as String[])[0];
            string filename = System.IO.Path.GetFileName(filepath);

            CurrentNode.Nodes.Add(new NodeStructure() { Details = new NodeDetails() { Name = filename, Path = filepath} });
            C_NodeList.ItemsSource = CurrentNode.Nodes;
            RefreshListView();
        }
    }
}
