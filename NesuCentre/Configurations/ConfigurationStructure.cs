using NesuCentre.Configurations.TopPanelConfiguration;
using NesuCentre.NodeConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesuCentre.Configurations
{
    public class ConfigurationStructure
    {
        public List<NodeStructure> nodes = new List<NodeStructure>();

        public List<TopPanelItem> topItems = new List<TopPanelItem>();

        public string ConfigurationVestion = "v0.0.0.2";
    }
}
