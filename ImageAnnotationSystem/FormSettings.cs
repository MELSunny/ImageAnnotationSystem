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
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            btnConfirm.Enabled = false;
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog imgFolderBrowserDialog = new FolderBrowserDialog();
            if (imgFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = imgFolderBrowserDialog.SelectedPath;
                DirectoryInfo directoryInfo = new DirectoryInfo(imgFolderBrowserDialog.SelectedPath);
                ConfigFile.WorkDirectory = directoryInfo;
                btnConfirm.Enabled = true;
            }
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(ConfigFile.WorkDirectory!=null)
            {
                ConfigFile.ReadConfig();
                FormMain formMain = new FormMain();
                Hide();
                formMain.ShowDialog();
                Show();
            }
        }
    }
    public static class GlobalSettings
    {
        public static List<string> SupportExtension = new List<string> { ".jpg", ".bmp", ".png", ".tiff", ".tif" };
    }
}
