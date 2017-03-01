using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.Structure;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Emgu.CV.Util;
using System.Threading;

namespace ImageAnnotationSystem
{
    public static class ImageProcess
    {
        public static Image FixedSize(Image imgPhoto, Size targetSize, Color color)
        {
            var Width = targetSize.Width;
            var Height = targetSize.Height;
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(color);
            grPhoto.InterpolationMode =InterpolationMode.NearestNeighbor;
            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            grPhoto.Dispose();
            return bmPhoto;
        }
        public static void FitSizeAndShow(Bitmap SourceBitmap, PictureBox container)
        {
            Size targetsize = new Size();
            if ((float)container.Width / container.Height > (float)SourceBitmap.Width / SourceBitmap.Height)
            {
                targetsize.Width = (int)((float)container.Height * SourceBitmap.Width / SourceBitmap.Height);
                targetsize.Height = container.Height;
                container.Location = new Point((container.Width - targetsize.Width) / 2 + container.Location.X, container.Location.Y);
                container.Width = targetsize.Width;
            }
            else
            {
                targetsize.Width = container.Width;
                targetsize.Height = (int)((float)container.Width * SourceBitmap.Height / SourceBitmap.Width);
                container.Location = new Point(container.Location.X, (container.Height - targetsize.Height) / 2 + container.Location.Y);
                container.Height = targetsize.Height;
            }
            container.Image = new Bitmap(SourceBitmap, targetsize);
        }
        public static void InOrder(Point p1, Point p2, out int x1, out int y1, out int x2, out int y2,Image source)
        {
            if (p1.X > p2.X)
            {
                x1 = p2.X;
                x2 = p1.X;
            }
            else
            {
                x1 = p1.X;
                x2 = p2.X;
            }
            if (p1.Y > p2.Y)
            {
                y1 = p2.Y;
                y2 = p1.Y;
            }
            else
            {
                y1 = p1.Y;
                y2 = p2.Y;
            }
            if (source != null)
            {
                if (x2 >= source.Width)
                    x2 = source.Width - 1;
                if (y2 >= source.Height)
                    y2 = source.Height - 1;
            }
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
        }
        public static Point SourceToResized(Point sourcepoint, Size sourcesize, Size resizedsize)
        {

            int ResizedX = (int)((double)resizedsize.Width / sourcesize.Width * sourcepoint.X);
            int ResizedY = (int)((double)resizedsize.Height / sourcesize.Height * sourcepoint.Y);
            return new Point(ResizedX, ResizedY);
        }
        public static Point ResizedToSource(Point resizedpoint, Size sourcesize, Size resizedsize)
        {
            int SourceX = (int)((double)sourcesize.Width / resizedsize.Width * resizedpoint.X);
            int SourceY = (int)((double)sourcesize.Height / resizedsize.Height * resizedpoint.Y);
            return new Point(SourceX, SourceY);
        }
        public static Bitmap DrawRectangle(Point p1, Point p2, Bitmap image, bool highquanlity, int width, Color color)
        {
            int x1, y1, x2, y2;
            InOrder(p1, p2, out x1, out y1, out x2, out y2,image);
            Bitmap imageNew = new Bitmap(image);
            Graphics use = Graphics.FromImage(imageNew);
            if (highquanlity)
            {
                use.SmoothingMode = SmoothingMode.AntiAlias;
                use.InterpolationMode = InterpolationMode.HighQualityBicubic;
                use.CompositingQuality = CompositingQuality.HighQuality;
            }
            use.DrawRectangle(new Pen(color, width), x1, y1, x2 - x1, y2 - y1);
            use.Dispose();
            return imageNew;
        }
        public static Bitmap CropRectangle(Point p1, Point p2,Image sourceimg)
        {
            int x1, y1, x2, y2;
            InOrder(p1, p2, out x1, out y1, out x2, out y2,sourceimg);
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            Bitmap result = new Bitmap(x2 - x1, y2 - y1);
            Graphics use = Graphics.FromImage(result);
            use.DrawImage(sourceimg, new Rectangle(0, 0, x2 - x1, y2 - y1), new Rectangle(x1, y1, x2 - x1, y2 - y1), GraphicsUnit.Pixel);
            use.Dispose();
            return result;
        }
        public static Bitmap Crop240Rectangle(Point p1, Point p2, Image sourceimg)
        {
            int x1, y1, x2, y2;
            InOrder(p1, p2, out x1, out y1, out x2, out y2,sourceimg);
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            Bitmap result = new Bitmap(240, 240);
            Graphics use = Graphics.FromImage(result);
            int x3 = (x1 + x2) / 2 - 120;
            int y3 = (y1 + y2) / 2 - 120;
            use.DrawImage(sourceimg, new Rectangle(0, 0, 240, 240), new Rectangle(x3, y3, 240, 240), GraphicsUnit.Pixel);
            use.Dispose();
            return result;
        }
        public static bool isRectangle(Point p1, Point p2)
        {
            if (p1.X == p2.X || p1.Y == p2.Y)
                return false;
            else
                return true;
        }
        public static List<MyObject> TargetObject(Bitmap binimg)
        {
            Image<Gray, Byte> imgBinary = new Image<Gray, Byte>(binimg);
            VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(imgBinary, contoursDetected, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
            int count = contoursDetected.Size;
            List<MyObject> res = new List<MyObject>();
            for (int i = 0; i < count; i++)
            {
                using (VectorOfPoint currContour = contoursDetected[i])
                {
                    int xmin = currContour.ToArray()[0].X;
                    int xmax = currContour.ToArray()[0].X;
                    int ymin= currContour.ToArray()[0].Y;
                    int ymax = currContour.ToArray()[0].Y;
                    foreach (var val in currContour.ToArray())
                    {
                        int x = val.X;
                        int y = val.Y;
                        if(x< xmin)
                            xmin = x;
                        if(x>xmax)
                            xmax = x;
                        if (y < ymin)
                            ymin = y;
                        if (y > ymax)
                            ymax = y;                       
                    }
                    res.Add(new MyObject(1, xmin, ymin, xmax, ymax));
                }
            }
            return res; 
        }
        public static List<MyObject> NonTargetObject(Bitmap binimg, List<MyObject> targetobject,int count)
        {
            List<MyObject> res = new List<MyObject>();
            int doTime = 0;
            double rite1=0.3;
            double rite2 = 0.4;
            while (res.Count< count)
            {             
                Random rd = new Random((GetRandomSeed()* new Guid().GetHashCode()*2)%149);
                int w = rd.Next((int)(binimg.Size.Width * rite1), (int)(binimg.Size.Width * rite2));
                rd = new Random((GetRandomSeed()* new Guid().GetHashCode()) % 150);
                int h = rd.Next((int)(binimg.Size.Height * rite1), (int)(binimg.Size.Height * rite2));
                rd = new Random((GetRandomSeed() + new Guid().GetHashCode()) % 151);
                int x = rd.Next(0, binimg.Size.Width - w);
                rd = new Random((GetRandomSeed() + new Guid().GetHashCode() * 2) % 152);
                int y = rd.Next(0, binimg.Size.Height - h); 
                int xmin = x;
                int ymin = y;
                int xmax = x + w - 1;
                int ymax = y + h - 1;
                bool tmp = true;
                foreach(var myobject in targetobject)
                {
                    if(judgeCross(myobject.xmin, myobject.ymin, myobject.xmax, myobject.ymax, xmin, ymin, xmax, ymax))
                    {
                        tmp = false;
                    }
                }
                foreach (var myobject in res)
                {
                    if (judgeCross(myobject.xmin, myobject.ymin, myobject.xmax, myobject.ymax, xmin, ymin, xmax, ymax))
                    {
                        tmp = false;
                    }
                }
                if (tmp)
                    res.Add(new MyObject(0, xmin, ymin, xmax, ymax));
                doTime++;
                if(doTime>100)
                {
                    rite1 = rite1 * .8;
                    rite2 = rite2 * .8;
                    doTime = 0;
                }
                if (rite1 < 0.1)
                    return res;
            }
            return res;

        }
        private static bool judgeCross(int xmin1,int ymin1,int xmax1,int ymax1,int xmin2,int ymin2,int xmax2,int ymax2)
        {
            int minx = Math.Max(xmin1, xmin2);
            int miny = Math.Max(ymin1, ymin2);
            int maxx = Math.Min(xmax1, xmax2);
            int maxy = Math.Min(ymax1, ymax2);
            //if ((minx > maxx) && (miny > maxy))
            if((xmin1 > xmax2 || ymin1 > ymax2 || xmin2 > xmax1 || ymin2 > ymax1))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        static int GetRandomSeed()
        {
            //Thread.Sleep(1);
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
