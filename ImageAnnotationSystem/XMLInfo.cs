using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;  
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace ImageAnnotationSystem
{
    class XMLInfo
    {
        public FileInfo ImgFile
        {
            get { return imgFile; }
        }
        private FileInfo imgFile;
        XElement root;
        public List<MyObject> ObjectList
        {
            get
            {
                List<MyObject> result = new List<MyObject>();
                IEnumerable<XElement> all_objects =
                   from el in root.Elements("object")
                   select el;
                foreach (XElement myobject in all_objects)
                {
                    if (ConfigFile.Classes.Where(s => string.Equals(myobject.Element("name").Value, s)).Count() == 1)
                        result.Add(new MyObject(
                            myobject.Element("name").Value,
                            int.Parse(myobject.Element("bndbox").Element("xmin").Value),
                            int.Parse(myobject.Element("bndbox").Element("ymin").Value),
                            int.Parse(myobject.Element("bndbox").Element("xmax").Value),
                            int.Parse(myobject.Element("bndbox").Element("ymax").Value)));
                    else
                        MessageBox.Show("Can not find class:" + myobject.Element("name").Value + " in config file.\nIt is from the XML info of " + imgFile.Name + " image file.\nSkip this object.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                return result;
            }
        }
        public XMLInfo(FileInfo imgfile)
        {
            imgFile = imgfile;
            if (File.Exists(imgFile.DirectoryName + "\\" + imgFile.Name + ".xml"))
            {
                root = XElement.Load(imgFile.DirectoryName + "\\" + imgFile.Name + ".xml");
            }
            else
            {
                int depth;
                switch (Image.FromFile(imgFile.FullName).PixelFormat)
                {
                    case PixelFormat.Format16bppArgb1555:
                        depth = 4;
                        break;
                    case PixelFormat.Format16bppGrayScale:
                        depth = 1;
                        break;
                    case PixelFormat.Format16bppRgb555:
                        depth = 3;
                        break;
                    case PixelFormat.Format16bppRgb565:
                        depth = 3;
                        break;
                    case PixelFormat.Format24bppRgb:
                        depth = 4;
                        break;
                    case PixelFormat.Format32bppArgb:
                        depth = 4;
                        break;
                    case PixelFormat.Format32bppPArgb:
                        depth = 4;
                        break;
                    case PixelFormat.Format32bppRgb:
                        depth = 3;
                        break;
                    case PixelFormat.Format48bppRgb:
                        depth = 3;
                        break;
                    case PixelFormat.Format64bppArgb:
                        depth = 4;
                        break;
                    case PixelFormat.Format64bppPArgb:
                        depth = 4;
                        break;
                    default:
                        depth = 0;      //unknown
                        break;
                }
                root =
                    new XElement
                    (
                        "annotation",
                        new XElement
                        (
                            "folder", imgFile.Directory.Name
                        ),
                        new XElement
                        (
                            "filename", imgFile.Name
                        ),
                        new XElement
                        (
                            "path", imgFile.FullName
                        ),
                        new XElement
                        (
                            "source",
                            new XElement
                            (
                                "database", "Unknown"
                            )
                        ),
                        new XElement
                        (
                            "size",
                            new XElement
                            (
                                "width", Image.FromFile(imgFile.FullName).Size.Width
                            ),
                            new XElement
                            (
                                "height", Image.FromFile(imgFile.FullName).Size.Height
                            ),
                            new XElement
                            (
                                "depth", depth.ToString()
                            )
                        ),
                        new XElement
                        (
                            "segmented", "0"
                        )
                    );
            }
        }
        public void SaveToFile()
        {
            root.Save(imgFile.DirectoryName + "\\" + imgFile.Name + ".xml");
        }
        public void AddObject(MyObject myobject)
        {
            var sourceimg = Image.FromFile(ImgFile.FullName);
            if (!Directory.Exists(imgFile.DirectoryName + "\\" + myobject.Name))
            {
                Directory.CreateDirectory(imgFile.DirectoryName + "\\" + myobject.Name);
            }
            if (!Directory.Exists(imgFile.DirectoryName + "\\" + myobject.Name + "_240px"))
            {
                Directory.CreateDirectory(imgFile.DirectoryName + "\\" + myobject.Name + "_240px");
            }
            ImageProcess.CropRectangle(new Point(myobject.xmin, myobject.ymin), new Point(myobject.xmax, myobject.ymax), sourceimg).Save(imgFile.DirectoryName + "\\" + myobject.Name + "\\" + imgFile.Name + "_" + myobject.xmin + "_" + myobject.ymin + "_" + myobject.xmax + "_" + myobject.ymax + ".bmp", ImageFormat.Bmp);
            Image image240 = ImageProcess.Crop240Rectangle(new Point(myobject.xmin, myobject.ymin), new Point(myobject.xmax, myobject.ymax), sourceimg);
            ImageProcess.FixedSize(image240, new Size(240, 240), Color.Black).Save(imgFile.DirectoryName + "\\" + myobject.Name + "_240px\\" + imgFile.Name + "_" + myobject.xmin + "_" + myobject.ymin + "_" + myobject.xmax + "_" + myobject.ymax + ".bmp", ImageFormat.Bmp);            
            root.Add
            (
                new XElement
                (
                    "object",
                    new XElement
                    (
                        "name", myobject.Name
                    ),
                    new XElement
                    (
                        "pose", "Unspecified"
                    ),
                    new XElement
                    (
                        "truncated", "0"
                    ),
                    new XElement
                    (
                        "difficult", "0"
                    ),
                    new XElement
                    (
                        "bndbox",
                        new XElement
                        (
                            "xmin", myobject.xmin.ToString()
                        ),
                         new XElement
                        (
                            "ymin", myobject.ymin.ToString()
                        ),
                        new XElement
                        (
                            "xmax", myobject.xmax.ToString()
                        ),
                        new XElement
                        (
                            "ymax", myobject.ymax.ToString()
                        )
                    )
                )
            );
            SaveToFile();
        }
        public void DelObject(MyObject myobject)
        {
            IEnumerable<XElement> aim_object =
                from el in root.Elements("object")
                where (string)el.Element("name") == myobject.Name
                && (string)el.Element("bndbox").Element("xmin") == myobject.xmin.ToString()
                && (string)el.Element("bndbox").Element("ymin") == myobject.ymin.ToString()
                && (string)el.Element("bndbox").Element("xmax") == myobject.xmax.ToString()
                && (string)el.Element("bndbox").Element("ymax") == myobject.ymax.ToString()
                select el;

            aim_object.First().Remove();
            File.Delete(imgFile.DirectoryName + "\\" + myobject.Name + "\\" + imgFile.Name + "_" + myobject.xmin + "_" + myobject.ymin + "_" + myobject.xmax + "_" + myobject.ymax + ".bmp");
            File.Delete(imgFile.DirectoryName + "\\" + myobject.Name + "_240px\\" + imgFile.Name + "_" + myobject.xmin + "_" + myobject.ymin + "_" + myobject.xmax + "_" + myobject.ymax + ".bmp");
            SaveToFile();
        }
    }
    public class MyObject
    {
        public int NameID;
        public string Name
        {
            get
            {
                return ConfigFile.Classes.ElementAt(NameID);
            }
            set
            {
                for (int i = 0; i < ConfigFile.Classes.Count(); i++)
                {
                    if (string.Equals(ConfigFile.Classes.ElementAt(i), value))
                        NameID = i;
                }
            }
        }
        public Point Min
        {
            get
            {
                return new Point(xmin, ymin);
            }
        }
        public Point Max
        {
            get
            {
                return new Point(xmax, ymax);
            }
        }
        public Color Color
        {
            get
            {
                switch (NameID % 9)
                {
                    case 0:
                        return Color.Red;
                    case 1:
                        return Color.Orange;
                    case 2:
                        return Color.YellowGreen;
                    case 3:
                        return Color.Green;
                    case 4:
                        return Color.DarkCyan;
                    case 5:
                        return Color.Blue;
                    case 6:
                        return Color.Purple;
                    case 7:
                        return Color.Gray;
                    default:
                        return Color.Black;
                }
            }
        }
        public int xmin;
        public int ymin;
        public int xmax;
        public int ymax;
        public MyObject(int nameID, int xmin, int ymin, int xmax, int ymax)
        {
            NameID = nameID;
            this.xmin = xmin;
            this.ymin = ymin;
            this.xmax = xmax;
            this.ymax = ymax;
        }
        public MyObject(string name, int xmin, int ymin, int xmax, int ymax)
        {
            Name = name;
            this.xmin = xmin;
            this.ymin = ymin;
            this.xmax = xmax;
            this.ymax = ymax;
        }
    }

}
