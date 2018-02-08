using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NesuCentre.NodeConfiguration.Structure
{
    public static class ConfigurationCentre
    {
        public static string NODE_CONFIGURATION_FILE_NAME = "NodeConfiguration.config";
        public static string NODE_CONFIGURATION_BACKUP_FILE_NAME = "NodeConfiguration_backup.config";

        public static NodeStructure RootNode = new NodeStructure() { Details = new NodeDetails() { Name = "Root"} };

        static ConfigurationCentre()
        {
            LoadConfiguration();
        }

        public static void SaveConfiguration()
        {
            if (File.Exists(NODE_CONFIGURATION_BACKUP_FILE_NAME))
                File.Delete(NODE_CONFIGURATION_BACKUP_FILE_NAME);

            if (File.Exists(NODE_CONFIGURATION_FILE_NAME))
                File.Move(NODE_CONFIGURATION_FILE_NAME, NODE_CONFIGURATION_BACKUP_FILE_NAME);

            XmlSerializer xmlSerializer = new XmlSerializer(RootNode.GetType());
            using (TextWriter textWriter = new StreamWriter(NODE_CONFIGURATION_FILE_NAME))
            {
                xmlSerializer.Serialize(textWriter, RootNode);
            }
        }

        public static void LoadConfiguration()
        {
            if (!File.Exists(NODE_CONFIGURATION_FILE_NAME))
                return;

            XmlSerializer xmlSerializer = new XmlSerializer(RootNode.GetType());
            using (TextReader textReader = new StreamReader(NODE_CONFIGURATION_FILE_NAME))
            {
                RootNode = xmlSerializer.Deserialize(textReader) as NodeStructure;
            }
        }
    }
}
