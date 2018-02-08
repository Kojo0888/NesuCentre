using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NesuCentre.NodeConfiguration.Structure
{
    public class NodeSettingNewestOldestFileOrDirectory : NodeSettingBase
    {
        public bool Newest { get; set; }

        public bool Files { get; set; }

        public bool Directories { get; set; }

        public PathDate DateType { get; set; }

        public NodeSettingNewestOldestFileOrDirectory()
        {
            Newest = true;
            Files = false;
            Directories = false;
            DateType = PathDate.CreationTime;
        }
    }
}
