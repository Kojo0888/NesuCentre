using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesuCentre.NodeConfiguration.Structure
{
    public class NodeDetails
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public INodeSetting Setting { get; set; }

        public AdditionalOption Option { get; set; }
    }
}
