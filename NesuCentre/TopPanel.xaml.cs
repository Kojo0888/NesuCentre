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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NesuCentre
{
    /// <summary>
    /// Interaction logic for TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {
        public static string test { get; set; }

        public TopPanel()
        {
            InitializeComponent();
            C_Canvas.AllowDrop = true;
        }

        private void C_Canvas_Drop(object sender, DragEventArgs e)
        {
            var droppedData = e.Data;
            var paths = (e.Data.GetData(DataFormats.FileDrop) as String[]);
            if (paths == null || paths.Length == 0)
                return;

            string filepath = paths[0];
            string filename = System.IO.Path.GetFileName(filepath);
        }
    }
}
