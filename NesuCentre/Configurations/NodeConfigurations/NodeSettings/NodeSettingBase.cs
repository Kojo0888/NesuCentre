using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NesuCentre.NodeConfiguration.Structure
{
    [XmlInclude(typeof(NodeSettingNewestOldestFileOrDirectory))]
    public abstract class NodeSettingBase
    {
    }
}
