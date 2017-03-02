using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ImageAnnotationSystem
{
    public partial class FormMain : Form
    {
        private Size picShowOriginalS;
        private Point picShowOriginalL;
        private ImageFiles imageFiles;
        private XMLInfo xmlInfo;
        public FormMain()
        {
            InitializeComponent();
            picShowOriginalS = picShow.Size;
            picShowOriginalL = picShow.Location;
            picShow.MouseWheel += new MouseEventHandler(picShow_MouseWheel);
            imageFiles = new ImageFiles();
            initFileList();
        }
        private void initFileList()
        {
            lvwFiles.Clear();
            lvwFiles.Columns.Add("Image List",252);
            ImageList imgList = new ImageList();        //Init thumbnail image list
            imgList.ColorDepth = ColorDepth.Depth24Bit;
            imgList.ImageSize = new Size(64, 64);
            this.lvwFiles.BeginUpdate();
            for (int i = 0; i < imageFiles.ImgFileList.Count(); i++)
            {
                try
                {
                    var currImage = Image.FromFile(imageFiles.ImgFileList.ElementAt(i).FullName);
                    imgList.Images.Add(ImageProcess.FixedSize(currImage,imgList.ImageSize,Color.White));
                    GC.Collect();
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.ImageIndex = i;
                    listViewItem.Text=imageFiles.ImgFileList.ElementAt(i).Name;
                    listViewItem.Tag = imageFiles.ImgFileList[i];
                    lvwFiles.Items.Add(listViewItem);
                }
                catch (OutOfMemoryException)
                {
                    MessageBox.Show("Read file failed:" + imageFiles.ImgFileList.ElementAt(i).Name);
                }
            }
            lvwFiles.SmallImageList = imgList;
            this.lvwFiles.EndUpdate();
        }
        private void picShow_MouseWheel(object sender, MouseEventArgs e)
        {
            if(lvwFiles.SelectedIndices.Count>0)
            {
                lvwFiles.Select();
                int selectnum = lvwFiles.SelectedIndices[0];
                if (e.Delta > 0)
                {
                    if (selectnum - 1 >= 0)
                    {
                        lvwFiles.SelectedItems[0].Selected = false;
                        lvwFiles.Items[selectnum - 1].Selected = true;
                    }
                }
                else
                {
                    if (selectnum + 1 < lvwFiles.Items.Count)
                    {           
                        lvwFiles.SelectedItems[0].Selected = false;
                        lvwFiles.Items[selectnum + 1].Selected = true;
                    }
                }
            }         
        }     
        private void lvwFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwFiles.SelectedIndices.Count == 1)
            {
                xmlInfo = new XMLInfo((FileInfo)lvwFiles.SelectedItems[0].Tag);
                showImage();
            }  
            GC.Collect();
        }
        private void showImage()
        {           
            picShow.Image = null;
            picShow.Size = picShowOriginalS;
            picShow.Location = picShowOriginalL;
            Bitmap source = (Bitmap)Image.FromFile(((FileInfo)lvwFiles.SelectedItems[0].Tag).FullName);
            ImageProcess.FitSizeAndShow(source, picShow);
            UpdateObjectList();
            tempBitmap = (Bitmap)picShow.Image;
            for (int i = 0; i < xmlInfo.ObjectList.Count(); i++)
            {
                tempBitmap = ImageProcess.DrawRectangle(ImageProcess.SourceToResized(xmlInfo.ObjectList[i].Min, source.Size, tempBitmap.Size), ImageProcess.SourceToResized(xmlInfo.ObjectList[i].Max, source.Size, tempBitmap.Size), tempBitmap, true, 2, xmlInfo.ObjectList[i].Color);
            }
            picShow.Image = tempBitmap;
        }
        private void UpdateObjectList()
        {
            lvwObject.Clear();
            lvwObject.Columns.Add("Object List", 252);
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(64, 64);
            imgList.ColorDepth = ColorDepth.Depth24Bit;
            lvwObject.BeginUpdate();  
            for (int i = 0; i < xmlInfo.ObjectList.Count(); i++)
            {
                var currImage = Image.FromFile(ConfigFile.WorkDirectory.FullName+"\\"+xmlInfo.ObjectList[i].Name+ "_240px\\"+xmlInfo.ImgFile.Name+"_"+xmlInfo.ObjectList[i].xmin + "_" + xmlInfo.ObjectList[i].ymin + "_" + xmlInfo.ObjectList[i].xmax + "_" + xmlInfo.ObjectList[i].ymax + ".bmp");
                imgList.Images.Add(ImageProcess.FixedSize(currImage, imgList.ImageSize, Color.White));
                GC.Collect();
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.ImageIndex = i;
                listViewItem.Text = xmlInfo.ObjectList[i].Name;
                listViewItem.ForeColor = xmlInfo.ObjectList[i].Color;
                listViewItem.Tag = xmlInfo.ObjectList[i];
                lvwObject.Items.Add(listViewItem);
            }        
            lvwObject.SmallImageList = imgList;
            lvwObject.EndUpdate();
        }
        Bitmap tempBitmap;
        Point startPoint;
        bool isDrag = false;
        private void picShow_MouseDown(object sender, MouseEventArgs e)
        {
            if (picShow.Image != null)
            {
                isDrag = true;
                tempBitmap = new Bitmap(picShow.Image);
                startPoint = e.Location;
            }
        }

        private void picShow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag == true)
            {
                picShow.Image = ImageProcess.DrawRectangle(startPoint, e.Location, tempBitmap, false, 2, Color.Black);
                GC.Collect();
            }
        }
        private void picShow_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrag == true)
            {
                isDrag = false;
                int x1, y1, x2, y2;
                Image source = Image.FromFile(xmlInfo.ImgFile.FullName);
                ImageProcess.InOrder(startPoint, e.Location, out x1, out y1, out x2, out y2, source);
                var p1 = new Point(x1, y1);
                var p2 = new Point(x2, y2);
                if (ImageProcess.isRectangle(p1, p2))
                { 
                    p1 = ImageProcess.ResizedToSource(startPoint, source.Size, picShow.Image.Size);
                    p2 = ImageProcess.ResizedToSource(e.Location, source.Size, picShow.Image.Size);
                    FormClass formClass = new FormClass(ImageProcess.CropRectangle(p1, p2, source), Control.MousePosition);
                    formClass.ShowDialog();
                    if (formClass.result != -1)
                    {
                        ImageProcess.InOrder(p1, p2, out x1, out y1, out x2, out y2,source);
                        MyObject myObject = new MyObject(formClass.result, x1, y1, x2, y2);
                        xmlInfo.AddObject(myObject);
                        tempBitmap = ImageProcess.DrawRectangle(startPoint, e.Location, tempBitmap, true, 2, myObject.Color);
                        UpdateObjectList();
                    }
                }
                picShow.Image = tempBitmap;
                GC.Collect();
            }
        }

        private void lvwObject_DoubleClick(object sender, EventArgs e)
        {
            Image source = Image.FromFile(xmlInfo.ImgFile.FullName);
            MyObject selected = (MyObject)lvwObject.SelectedItems[0].Tag;
            FormClass formClass = new FormClass(ImageProcess.CropRectangle(selected.Min, selected.Max, source), Control.MousePosition);
            formClass.ShowDialog();
            if (formClass.result == -1)
            {
                xmlInfo.DelObject(selected);
            }
            else
            {
                xmlInfo.DelObject(selected);
                selected.NameID = formClass.result;
                xmlInfo.AddObject(selected);
            }
            showImage();
        }

        private void lvwObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwObject.SelectedIndices.Count == 1)
            {
                Image source = Image.FromFile(xmlInfo.ImgFile.FullName);
                MyObject selected = (MyObject)lvwObject.SelectedItems[0].Tag;
                picShow.Image = tempBitmap;
                picShow.Image = ImageProcess.DrawRectangle(ImageProcess.SourceToResized(selected.Min, source.Size, tempBitmap.Size), ImageProcess.SourceToResized(selected.Max, source.Size, tempBitmap.Size), tempBitmap, true, 4, selected.Color);
                GC.Collect();
            }
        }
    }
}
