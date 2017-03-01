using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ImageAnnotationSystem
{
    public static class ConfigFile
    {
        public static DirectoryInfo WorkDirectory;
        private static XElement config;
        public static void ReadConfig()
        {
            if (File.Exists(WorkDirectory.FullName + ".config"))
            {
                config = XElement.Load(WorkDirectory.FullName + ".config");
            }
            else
            {
                createConfig();
            }       
        }
        private static void createConfig()
        {
            config = new XElement
                    (
                        "Config",
                        new XElement
                        (
                            "FolderName", WorkDirectory.Name
                        ),
                        new XElement
                        (
                            "CropImage",
                            new XElement
                            (
                                "Exist", true
                            ),
                            new XElement
                            (
                                "Prefix", "cp"
                            ),
                            new XElement
                            (
                                "Suffix", "pic"
                            )
                        ),
                        new XElement
                        (
                            "Classes",
                            new XElement
                            (
                                "Class", "Type1"
                            ), 
                            new XElement
                            (
                                "Class", "Type2"
                            ),
                            new XElement
                            (
                                "Class", "Others"
                            )
                        )
                    );
            config.Save(WorkDirectory.FullName + ".config");
            MessageBox.Show("Can not find the config file of selected directory.\nThe example of the config file is created. \nPlease modify the data of the config file.","Can not find the config file!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            System.Diagnostics.Process.Start(WorkDirectory.FullName + ".config");
        }
        public static bool ExistCropImage 
        {
            get { return Convert.ToBoolean(config.Element("CropImage").Element("Exist").Value); }
        }
        public static string CropImagePrefix
        {
            get { return config.Element("CropImage").Element("Prefix").Value; }
        }
        public static string CropImageSuffix
        {
            get { return config.Element("CropImage").Element("Suffix").Value; }
        }
        public static IEnumerable<string> Classes
        {
            get {
                return config.Element("Classes").Elements("Class").Select(item => item.Value);
            }
        }    
    }
}
