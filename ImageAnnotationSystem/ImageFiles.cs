using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace ImageAnnotationSystem
{
    public class ImageFiles
    {
        public List<FileInfo> ImgFileList;
        public List<ImagePair> ImagePairList;
        public ImageFiles()
        {
            initImageFileList();
        }
        private void initImageFileList()
        {
            if (ConfigFile.ExistCropImage)
            {
                int cropImageCount = 0;
                ImgFileList = new List<FileInfo>();
                ImagePairList = new List<ImagePair>();

                var allImageFile = ConfigFile.WorkDirectory.EnumerateFiles("*", SearchOption.TopDirectoryOnly).Where(s => GlobalSettings.SupportExtension.Contains(s.Extension.ToLower()));
                foreach (var file in allImageFile)
                {
                    if (file.Name.StartsWith(ConfigFile.CropImagePrefix) && file.Name.EndsWith(ConfigFile.CropImageSuffix + file.Extension))
                    {
                        cropImageCount++;
                        var cropImage = file;
                        string imageName = cropImage.Name.Substring(ConfigFile.CropImagePrefix.Length, cropImage.Name.Length - (ConfigFile.CropImagePrefix + ConfigFile.CropImageSuffix + file.Extension).Length);
                        var imageFile = allImageFile.Where(s => s.Name.Substring(0, s.Name.Length - s.Extension.Length).Equals(imageName));
                        switch (imageFile.Count())
                        {
                            case 1:
                                ImagePairList.Add(new ImagePair(imageFile.First(), cropImage));
                                ImgFileList.Add(imageFile.First());
                                break;
                            case 0:
                                MessageBox.Show("Can not find the image corresponding to " + cropImage.Name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            default:
                                MessageBox.Show("Find more than one images corresponding to " + cropImage.Name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;                   
                        }
                    }
                    
                }
                if(ConfigFile.Classes.Count()==2)
                {
                    DialogResult dialogResult = MessageBox.Show("Find " + cropImageCount.ToString() + " crop image and " + ImagePairList.Count() + " pairs.\nWould you like to process auto generate?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        foreach(var imagepair in ImagePairList)
                        {
                            XMLInfo xmlInfo = new XMLInfo(imagepair.Image);
                            try
                            {
                                var targetList = ImageProcess.TargetObject((Bitmap)Image.FromFile(imagepair.CropImage.FullName));
                                var nonTargetList = ImageProcess.NonTargetObject((Bitmap)Image.FromFile(imagepair.CropImage.FullName), targetList, 2);
                                foreach (var tar in targetList)
                                {
                                    xmlInfo.AddObject(tar);
                                }
                                foreach (var nontar in nonTargetList)
                                {
                                    xmlInfo.AddObject(nontar);
                                }
                                xmlInfo.SaveToFile();
                            }
                            catch(Exception)
                            {
                                MessageBox.Show("Happened unknown error during the process of " + imagepair.Image.Name, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            else
            {
                ImgFileList = ConfigFile.WorkDirectory.EnumerateFiles("*", SearchOption.TopDirectoryOnly).Where(s => GlobalSettings.SupportExtension.Contains(s.Extension.ToLower())).ToList(); 
            }
        }
    }
    public class ImagePair
    {
        public FileInfo Image;
        public FileInfo CropImage;
        public ImagePair(FileInfo image, FileInfo cropimage)
        {
            Image = image;
            CropImage = cropimage;
        }
    }
}
