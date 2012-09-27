namespace cvn_helper
{
    partial class frmArchiveManager
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
            this.MyMenu = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuReload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.MyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MyMenu
            // 
            this.MyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile});
            this.MyMenu.Location = new System.Drawing.Point(0, 0);
            this.MyMenu.Name = "MyMenu";
            this.MyMenu.Size = new System.Drawing.Size(404, 24);
            this.MyMenu.TabIndex = 0;
            this.MyMenu.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuOpen,
            this.MenuSave,
            this.MenuSeparate1,
            this.MenuEdit,
            this.MenuExtract,
            this.MenuReplace,
            this.MenuDelete,
            this.MenuSeparate2,
            this.MenuReload,
            this.MenuSeparate3,
            this.MenuExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(43, 20);
            this.MenuFile.Text = "파일";
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuOpen.Size = new System.Drawing.Size(178, 22);
            this.MenuOpen.Text = "열기...";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuSeparate1
            // 
            this.MenuSeparate1.Name = "MenuSeparate1";
            this.MenuSeparate1.Size = new System.Drawing.Size(175, 6);
            // 
            // MenuSave
            // 
            this.MenuSave.Enabled = false;
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuSave.Size = new System.Drawing.Size(178, 22);
            this.MenuSave.Text = "저장";
            // 
            // MenuExtract
            // 
            this.MenuExtract.Enabled = false;
            this.MenuExtract.Name = "MenuExtract";
            this.MenuExtract.Size = new System.Drawing.Size(178, 22);
            this.MenuExtract.Text = "추출...";
            // 
            // MenuReplace
            // 
            this.MenuReplace.Enabled = false;
            this.MenuReplace.Name = "MenuReplace";
            this.MenuReplace.Size = new System.Drawing.Size(178, 22);
            this.MenuReplace.Text = "교환...";
            // 
            // MenuDelete
            // 
            this.MenuDelete.Enabled = false;
            this.MenuDelete.Name = "MenuDelete";
            this.MenuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.MenuDelete.Size = new System.Drawing.Size(178, 22);
            this.MenuDelete.Text = "삭제";
            // 
            // MenuSeparate2
            // 
            this.MenuSeparate2.Name = "MenuSeparate2";
            this.MenuSeparate2.Size = new System.Drawing.Size(175, 6);
            // 
            // MenuReload
            // 
            this.MenuReload.Enabled = false;
            this.MenuReload.Name = "MenuReload";
            this.MenuReload.Size = new System.Drawing.Size(178, 22);
            this.MenuReload.Text = "마지막 저장 상태로";
            // 
            // MenuSeparate3
            // 
            this.MenuSeparate3.Name = "MenuSeparate3";
            this.MenuSeparate3.Size = new System.Drawing.Size(175, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(178, 22);
            this.MenuExit.Text = "종료";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // MenuEdit
            // 
            this.MenuEdit.Enabled = false;
            this.MenuEdit.Name = "MenuEdit";
            this.MenuEdit.Size = new System.Drawing.Size(178, 22);
            this.MenuEdit.Text = "수정...";
            // 
            // FileTree
            // 
            this.FileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTree.Location = new System.Drawing.Point(0, 24);
            this.FileTree.Name = "FileTree";
            this.FileTree.PathSeparator = "/";
            this.FileTree.Size = new System.Drawing.Size(404, 310);
            this.FileTree.TabIndex = 1;
            // 
            // frmArchiveManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 334);
            this.Controls.Add(this.FileTree);
            this.Controls.Add(this.MyMenu);
            this.MainMenuStrip = this.MyMenu;
            this.Name = "frmArchiveManager";
            this.Text = "아카이브 관리자";
            this.MyMenu.ResumeLayout(false);
            this.MyMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MyMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripSeparator MenuSeparate1;
        private System.Windows.Forms.ToolStripMenuItem MenuEdit;
        private System.Windows.Forms.ToolStripMenuItem MenuExtract;
        private System.Windows.Forms.ToolStripMenuItem MenuReplace;
        private System.Windows.Forms.ToolStripMenuItem MenuDelete;
        private System.Windows.Forms.ToolStripSeparator MenuSeparate2;
        private System.Windows.Forms.ToolStripMenuItem MenuReload;
        private System.Windows.Forms.ToolStripSeparator MenuSeparate3;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.TreeView FileTree;
    }
}