using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesuCentre.NodeConfiguration
{
    public class NodeStructure
    {
        public ObservableCollection<NodeStructure> Nodes { get; set; } = new ObservableCollection<NodeStructure>();

        public NodeDetails Details { get; set; }

        public NodeStructure()
        {
            Details = new NodeDetails();
            Details.Name = "New Node";
            Details.Path = "";
        }

        public override string ToString()
        {
            return Details.Name.ToString();
        }
    }
}
