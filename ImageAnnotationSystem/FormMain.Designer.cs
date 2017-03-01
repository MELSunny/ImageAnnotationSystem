namespace GeneralAnnotationSystem
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.picShow = new System.Windows.Forms.PictureBox();
            this.lvwFiles = new System.Windows.Forms.ListView();
            this.lvwObject = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).BeginInit();
            this.SuspendLayout();
            // 
            // picShow
            // 
            this.picShow.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picShow.Location = new System.Drawing.Point(12, 12);
            this.picShow.Name = "picShow";
            this.picShow.Size = new System.Drawing.Size(705, 705);
            this.picShow.TabIndex = 0;
            this.picShow.TabStop = false;
            this.picShow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picShow_MouseDown);
            this.picShow.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picShow_MouseMove);
            this.picShow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picShow_MouseUp);
            // 
            // lvwFiles
            // 
            this.lvwFiles.FullRowSelect = true;
            this.lvwFiles.Location = new System.Drawing.Point(723, 12);
            this.lvwFiles.MultiSelect = false;
            this.lvwFiles.Name = "lvwFiles";
            this.lvwFiles.Size = new System.Drawing.Size(273, 373);
            this.lvwFiles.TabIndex = 6;
            this.lvwFiles.UseCompatibleStateImageBehavior = false;
            this.lvwFiles.View = System.Windows.Forms.View.Details;
            this.lvwFiles.SelectedIndexChanged += new System.EventHandler(this.lvwFiles_SelectedIndexChanged);
            // 
            // lvwObject
            // 
            this.lvwObject.Location = new System.Drawing.Point(723, 391);
            this.lvwObject.MultiSelect = false;
            this.lvwObject.Name = "lvwObject";
            this.lvwObject.Size = new System.Drawing.Size(273, 326);
            this.lvwObject.TabIndex = 7;
            this.lvwObject.UseCompatibleStateImageBehavior = false;
            this.lvwObject.View = System.Windows.Forms.View.SmallIcon;
            this.lvwObject.SelectedIndexChanged += new System.EventHandler(this.lvwObject_SelectedIndexChanged);
            this.lvwObject.DoubleClick += new System.EventHandler(this.lvwObject_DoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.lvwObject);
            this.Controls.Add(this.lvwFiles);
            this.Controls.Add(this.picShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Annotation System";
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picShow;
        private System.Windows.Forms.ListView lvwFiles;
        private System.Windows.Forms.ListView lvwObject;
    }
}

