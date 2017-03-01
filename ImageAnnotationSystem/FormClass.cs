using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneralAnnotationSystem
{
    public partial class FormClass : Form
    {
        public int result = -1;
        private Size picShowOriginalS;
        private Point picShowOriginalL;
        public FormClass(Image image, Point Location)
        {
            InitializeComponent();
            picShowOriginalS = picShow.Size;
            picShowOriginalL = picShow.Location;
            foreach (var name in ConfigFile.Classes)
            {
                lsvType.Items.Add(new ListViewItem(name) { ForeColor = new MyObject(name,0,0,0,0).Color });
            }
            picShow.Image = null;
            picShow.Size = picShowOriginalS;
            picShow.Location = picShowOriginalL;
            ImageProcess.FitSizeAndShow((Bitmap)image, picShow);

            Point pos = new Point(Location.X - 354 - 57 / 2, Location.Y - 20);
            if (pos.X + this.Width > Screen.GetWorkingArea(this).Width)
                pos.X = Screen.GetWorkingArea(this).Width - this.Width;
            if (pos.Y + Height > Screen.GetWorkingArea(this).Height)
                pos.Y = Screen.GetWorkingArea(this).Height - Height;
            this.Location = pos;
        }
        private void lsvType_Click(object sender, EventArgs e)
        {
            if (lsvType.SelectedIndices.Count == 1)
            {
                result = lsvType.SelectedIndices[0];
                this.Close();
            }
        }
        private void btnDel_Click(object sender, EventArgs e)
        {

            result = -1;
            this.Close();
        }
    }
}
