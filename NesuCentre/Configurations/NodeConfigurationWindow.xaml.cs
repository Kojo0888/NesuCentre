﻿using NesuCentre.NodeConfiguration;
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

        public bool SettingUpControls { set; get; }

        public int DragDropState { get; set; }

        public NodeConfigurationWindow()
        {
            InitializeComponent();
            ConfigurationCentre.LoadConfiguration();
            CurrentNode = ConfigurationCentre.Configuration.RootNode;
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
            var paths = (e.Data.GetData(DataFormats.FileDrop) as String[]);
            if (paths == null || paths.Length == 0)
                return;

            string filepath = paths[0];
            string filename = System.IO.Path.GetFileName(filepath);

            CurrentNode.Nodes.Add(new NodeStructure() { Details = new NodeDetails() { Name = filename, Path = filepath} });
            C_NodeList.ItemsSource = CurrentNode.Nodes;
            RefreshListView();

        }

        private void C_NodeSetting_NewestOldestFileOrDirectory_SP_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var node = C_NodeSetting_NewestOldestFileOrDirectory_SP.DataContext as NodeStructure;
            if(node == null) return;

            if (node.Details.Setting is NodeSettingNewestOldestFileOrDirectory)
            {
                C_NodeSetting_Type_CBX.SelectedItem = AdditionalOption.NewestOldestFileOrDirectory;
                SetNewestOldestFileOrDirectorySettingControls(node.Details.Setting as NodeSettingNewestOldestFileOrDirectory);
            }
            else
            {
                C_NodeSetting_Type_CBX.SelectedItem = AdditionalOption.None;
                SetNoneSettingControls();
            }
        }

        private void SetNewestOldestFileOrDirectorySettingControls(NodeSettingNewestOldestFileOrDirectory setting)
        {
            C_NodeSetting_NewestOldestFileOrDirectory_SP.Visibility = Visibility.Visible;

            if (setting == null)
                return;

            SettingUpControls = true;

            C_NodeSettings_Directories_CHBX.IsChecked = setting.Directories;
            C_NodeSettings_Files_CHBX.IsChecked = setting.Files;
            C_NodeSettings_NewestDate_RBX.IsChecked = setting.Newest;
            C_NodeSettings_OldestDate_RBX.IsChecked = !setting.Newest;

            C_NodeSettings_LastAccessDate_RBTN.IsChecked = setting.DateType == PathDate.LastAccessTime;
            C_NodeSettings_LastWriteDate_RBTN.IsChecked = setting.DateType == PathDate.LastWriteTime;
            C_NodeSettings_CreationDate_RBTN.IsChecked = setting.DateType == PathDate.CreationTime;

            SettingUpControls = false;
        }

        private void SetNoneSettingControls()
        {
            C_NodeSetting_NewestOldestFileOrDirectory_SP.Visibility = Visibility.Collapsed;

            SettingUpControls = true;

            C_NodeSettings_Directories_CHBX.IsChecked = false;
            C_NodeSettings_Files_CHBX.IsChecked = false;
            C_NodeSettings_NewestDate_RBX.IsChecked = false;
            C_NodeSettings_OldestDate_RBX.IsChecked = false;

            C_NodeSettings_LastAccessDate_RBTN.IsChecked = false;
            C_NodeSettings_LastWriteDate_RBTN.IsChecked = false;
            C_NodeSettings_CreationDate_RBTN.IsChecked = false;

            SettingUpControls = false;
        }

        private void C_NodeSettings_LastWriteDate_RBTN_Checked(object sender, RoutedEventArgs e)
        {
            var node = C_NodeSetting_NewestOldestFileOrDirectory_SP.DataContext as NodeStructure;
            if (node == null) return;

            var settingNewerOlder = node.Details.Setting as NodeSettingNewestOldestFileOrDirectory;
            if (settingNewerOlder == null) return;

            if (!SettingUpControls)
            {
                if (C_NodeSettings_LastAccessDate_RBTN.IsChecked == true)
                    settingNewerOlder.DateType = PathDate.LastAccessTime;
                else if (C_NodeSettings_LastWriteDate_RBTN.IsChecked == true)
                    settingNewerOlder.DateType = PathDate.LastWriteTime;
                else if (C_NodeSettings_CreationDate_RBTN.IsChecked == true)
                    settingNewerOlder.DateType = PathDate.CreationTime;
            }
        }

        private void C_NodeSettings_NewestDate_RBX_Checked(object sender, RoutedEventArgs e)
        {
            var node = C_NodeSetting_NewestOldestFileOrDirectory_SP.DataContext as NodeStructure;
            if (node == null) return;

            var settingNewerOlder = node.Details.Setting as NodeSettingNewestOldestFileOrDirectory;
            if (settingNewerOlder == null) return;

            if (!SettingUpControls)
            {
                if (C_NodeSettings_OldestDate_RBX.IsChecked == true)
                    settingNewerOlder.Newest = false;
                else if (C_NodeSettings_NewestDate_RBX.IsChecked == true)
                    settingNewerOlder.Newest = true;
            }
        }

        private void C_NodeSetting_Type_CBX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var node = C_NodeSetting_NewestOldestFileOrDirectory_SP.DataContext as NodeStructure;
            if (node == null) return;

            var selected = C_NodeSetting_Type_CBX.SelectedItem.ToString();
            if (selected == AdditionalOption.NewestOldestFileOrDirectory.ToString())
            {
                if(node.Details.Setting == null || !(node.Details.Setting is NodeSettingNewestOldestFileOrDirectory))
                    node.Details.Setting = new NodeSettingNewestOldestFileOrDirectory();
                SetNewestOldestFileOrDirectorySettingControls(node.Details.Setting as NodeSettingNewestOldestFileOrDirectory);
            }
            else
            {
                node.Details.Setting = null;
                SetNoneSettingControls();
            }
        }

        private void C_NodeSettings_Files_CHBX_Checked(object sender, RoutedEventArgs e)
        {
            var node = C_NodeSetting_NewestOldestFileOrDirectory_SP.DataContext as NodeStructure;
            if (node == null) return;

            var settingNewerOlder = node.Details.Setting as NodeSettingNewestOldestFileOrDirectory;
            if (settingNewerOlder == null) return;

            if (!SettingUpControls)
            {
                settingNewerOlder.Files = C_NodeSettings_Files_CHBX.IsChecked == true ? true : false;
                settingNewerOlder.Directories = C_NodeSettings_Directories_CHBX.IsChecked == true ? true : false;
            }
        }

        private void C_NodeList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DragDropState > 0)
                DragDropState = 0;
             
        }

        private void C_NodeList_ItemDataTemplate_G_Drop(object sender, DragEventArgs e)
        {
            if(DragDropState > 5)
            {
                DragDropState = 0;
                var data = e.Data.GetData(typeof(NodeStructure)) as NodeStructure;
                if(data != null)
                {
                    var reciver = sender as Grid;
                    if (reciver == null)
                        return;
                    var reciverNode = reciver.DataContext as NodeStructure;
                    if (reciverNode == null)
                        return;
                    if (data == reciverNode)
                        return;
                    CurrentNode.Nodes.Remove(data);
                    reciverNode.Nodes.Add(data);
                    RefreshListView();
                }
            }
            DragDropState = 0;
        }

        private void C_NodeList_ItemDataTemplate_G_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragDropState = 1;
        }

        private void C_NodeList_ItemDataTemplate_G_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragDropState > 0 && DragDropState < 5)
                DragDropState++;
            if (DragDropState == 5)
            {
                DragDropState = 6;
                var selectedNode = C_NodeList.SelectedItem as NodeStructure;
                if (selectedNode == null)
                    return;
                DragDrop.DoDragDrop(this, selectedNode, DragDropEffects.Move);
            }
        }

        private void C_NodeList_MoveUp_BTN_Click(object sender, RoutedEventArgs e)
        {
            var nodeStructure = C_NodeList.SelectedItem as NodeStructure;
            if (nodeStructure == null)
                return;

            var index = C_NodeList.SelectedIndex;
            CurrentNode.Nodes.Remove(nodeStructure);
            if (index - 1 < 0)
                CurrentNode.Nodes.Insert(0, nodeStructure);
            else
                CurrentNode.Nodes.Insert(index - 1, nodeStructure);
            //C_NodeList.SelectedIndex = index - 1;
            RefreshListView();
        }

        private void C_NodeList_MoveDown_BTN_Click(object sender, RoutedEventArgs e)
        {
            var nodeStructure = C_NodeList.SelectedItem as NodeStructure;
            if (nodeStructure == null)
                return;

            var index = C_NodeList.SelectedIndex;
            CurrentNode.Nodes.Remove(nodeStructure);
            if (index + 1 < CurrentNode.Nodes.Count)
                CurrentNode.Nodes.Insert(index + 1, nodeStructure);
            else
                CurrentNode.Nodes.Add(nodeStructure);
            //C_NodeList.SelectedIndex = index + 1;
            RefreshListView();
        }

        private void C_NodeList_ItemDataTemplate_G_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var reciver = sender as Grid;
            if (reciver == null)
                return;
            var reciverNode = reciver.DataContext as NodeStructure;
            if (reciverNode == null)
                return;
            if (PathNodes.Count == 0)
                return;
            CurrentNode.Nodes.Remove(reciverNode);
            PathNodes.ElementAt(PathNodes.Count - 1).Nodes.Add(reciverNode);

        }
    }
}
