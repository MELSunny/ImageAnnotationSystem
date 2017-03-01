namespace GeneralAnnotationSystem
{
    partial class FormClass
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picShow = new System.Windows.Forms.PictureBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.lsvType = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).BeginInit();
            this.SuspendLayout();
            // 
            // picShow
            // 
            this.picShow.Location = new System.Drawing.Point(12, 12);
            this.picShow.Name = "picShow";
            this.picShow.Size = new System.Drawing.Size(336, 336);
            this.picShow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picShow.TabIndex = 0;
            this.picShow.TabStop = false;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(354, 325);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(114, 23);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "Delate";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // lsvType
            // 
            this.lsvType.Location = new System.Drawing.Point(354, 12);
            this.lsvType.MultiSelect = false;
            this.lsvType.Name = "lsvType";
            this.lsvType.Size = new System.Drawing.Size(114, 307);
            this.lsvType.TabIndex = 3;
            this.lsvType.UseCompatibleStateImageBehavior = false;
            this.lsvType.View = System.Windows.Forms.View.List;
            this.lsvType.Click += new System.EventHandler(this.lsvType_Click);
            // 
            // FormClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 360);
            this.Controls.Add(this.lsvType);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.picShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormClass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Select";
            ((System.ComponentModel.ISupportInitialize)(this.picShow)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picShow;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ListView lsvType;
    }
    
}