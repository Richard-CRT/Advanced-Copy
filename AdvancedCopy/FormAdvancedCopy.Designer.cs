namespace AdvancedCopy
{
    partial class FormAdvancedCopy
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAdvancedCopy));
            this.treeView_fileSystem = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.button_exclude = new System.Windows.Forms.Button();
            this.button_include = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView_fileSystem
            // 
            this.treeView_fileSystem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_fileSystem.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeView_fileSystem.HideSelection = false;
            this.treeView_fileSystem.Location = new System.Drawing.Point(12, 12);
            this.treeView_fileSystem.Name = "treeView_fileSystem";
            this.treeView_fileSystem.Size = new System.Drawing.Size(522, 426);
            this.treeView_fileSystem.StateImageList = this.imageList;
            this.treeView_fileSystem.TabIndex = 0;
            this.treeView_fileSystem.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_fileSystem_BeforeExpand);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "folder.png");
            // 
            // button_exclude
            // 
            this.button_exclude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exclude.Location = new System.Drawing.Point(540, 12);
            this.button_exclude.Name = "button_exclude";
            this.button_exclude.Size = new System.Drawing.Size(75, 23);
            this.button_exclude.TabIndex = 1;
            this.button_exclude.Text = "Exclude";
            this.button_exclude.UseVisualStyleBackColor = true;
            this.button_exclude.Click += new System.EventHandler(this.button_exclude_Click);
            // 
            // button_include
            // 
            this.button_include.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_include.Location = new System.Drawing.Point(540, 41);
            this.button_include.Name = "button_include";
            this.button_include.Size = new System.Drawing.Size(75, 23);
            this.button_include.TabIndex = 2;
            this.button_include.Text = "Include";
            this.button_include.UseVisualStyleBackColor = true;
            this.button_include.Click += new System.EventHandler(this.button_include_Click);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(540, 99);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 3;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_load
            // 
            this.button_load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_load.Location = new System.Drawing.Point(540, 70);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(75, 23);
            this.button_load.TabIndex = 4;
            this.button_load.Text = "Load";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // FormAdvancedCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 450);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_include);
            this.Controls.Add(this.button_exclude);
            this.Controls.Add(this.treeView_fileSystem);
            this.Name = "FormAdvancedCopy";
            this.Text = "Advanced Copy";
            this.Load += new System.EventHandler(this.FormAdvancedCopy_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView_fileSystem;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Button button_exclude;
        private System.Windows.Forms.Button button_include;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_load;
    }
}

