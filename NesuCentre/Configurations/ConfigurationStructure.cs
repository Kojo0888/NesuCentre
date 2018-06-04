using NesuCentre.Configurations.TopPanelConfiguration;
using NesuCentre.NodeConfiguration;
using NesuCentre.NodeConfiguration.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesuCentre.Configurations
{
    public class ConfigurationStructure
    {
        public NodeStructure RootNode = new NodeStructure() { Details = new NodeDetails() { Name = "Root" } };

        public List<TopPanelItemConfiguration> TopPanelItems = new List<TopPanelItemConfiguration>();

        public string ConfigurationVestion = "v0.0.0.2";
    }
}
