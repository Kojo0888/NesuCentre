using NesuCentre.Configurations;
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
        public static string NODE_CONFIGURATION_BACKUP_FOLDER_NAME = "NodeConfigurationBackups";

        public static string CONFIGURATION_EXTENSION => ".config";

        public static bool ConfigurationChanged { get; set; } = false;

        public static ConfigurationStructure Configuration = new ConfigurationStructure();//TODO attach new Configuration

        //public static NodeStructure RootNode = new NodeStructure() { Details = new NodeDetails() { Name = "Root"} };

        static ConfigurationCentre()
        {
            LoadConfiguration();
        }

        public static void SaveConfiguration()
        {
            MoveConfigurationToBackupFolder();

            XmlSerializer xmlSerializer = new XmlSerializer(Configuration.GetType());
            using (TextWriter textWriter = new StreamWriter(NODE_CONFIGURATION_FILE_NAME))
            {
                xmlSerializer.Serialize(textWriter, Configuration);
            }
        }

        private static void MoveConfigurationToBackupFolder()
        {
            if (!Directory.Exists(NODE_CONFIGURATION_BACKUP_FOLDER_NAME))
                Directory.CreateDirectory(NODE_CONFIGURATION_BACKUP_FOLDER_NAME);

            if (File.Exists(NODE_CONFIGURATION_FILE_NAME))
            {
                DateTime dt = File.GetLastWriteTime(NODE_CONFIGURATION_FILE_NAME);
                string newPath = Path.Combine(NODE_CONFIGURATION_BACKUP_FOLDER_NAME,
                    $"ConfigurationBackup_{dt.ToShortDateString().Replace('/','_')}__{dt.ToLongTimeString().Replace(':','_').Replace(' ','_')}{CONFIGURATION_EXTENSION}");

                if (File.Exists(newPath))
                {
                    int index = 1;
                    string newPathWidthNumber = newPath + "_" +  index + CONFIGURATION_EXTENSION;
                    while (File.Exists(newPathWidthNumber))
                    {
                        newPathWidthNumber = newPath + "_" + (index++);
                    }

                    File.Move(NODE_CONFIGURATION_FILE_NAME, newPathWidthNumber);
                }

                File.Move(NODE_CONFIGURATION_FILE_NAME, newPath);
            }

            ScanAndRemoveBackupsOlderThanWeek();
        }

        private static void ScanAndRemoveBackupsOlderThanWeek()
        {
            foreach (var path in Directory.GetFiles(NODE_CONFIGURATION_BACKUP_FOLDER_NAME))
            {
                if (File.GetLastAccessTime(NODE_CONFIGURATION_BACKUP_FOLDER_NAME).AddDays(7) < DateTime.Now)
                    File.Delete(path);
            }
        }

        public static void LoadConfiguration()
        {
            if (!File.Exists(NODE_CONFIGURATION_FILE_NAME))
                return;

            XmlSerializer xmlSerializer = new XmlSerializer(Configuration.GetType());
            using (TextReader textReader = new StreamReader(NODE_CONFIGURATION_FILE_NAME))
            {
                Configuration = xmlSerializer.Deserialize(textReader) as ConfigurationStructure;
            }
        }
    }
}
