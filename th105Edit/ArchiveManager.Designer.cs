/*
Copyright VBChunguk  2012

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
*/

namespace th105Edit
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
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuReload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate3 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.FileTree = new System.Windows.Forms.TreeView();
            this.MenuAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSeparate0 = new System.Windows.Forms.ToolStripSeparator();
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
            this.MenuSaveAs,
            this.MenuSeparate0,
            this.MenuAdd,
            this.MenuSeparate1,
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
            this.MenuOpen.Size = new System.Drawing.Size(187, 22);
            this.MenuOpen.Text = "열기...";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Enabled = false;
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.MenuSave.Size = new System.Drawing.Size(187, 22);
            this.MenuSave.Text = "저장";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuSaveAs
            // 
            this.MenuSaveAs.Enabled = false;
            this.MenuSaveAs.Name = "MenuSaveAs";
            this.MenuSaveAs.Size = new System.Drawing.Size(187, 22);
            this.MenuSaveAs.Text = "다른 이름으로 저장...";
            this.MenuSaveAs.Click += new System.EventHandler(this.MenuSaveAs_Click);
            // 
            // MenuSeparate1
            // 
            this.MenuSeparate1.Name = "MenuSeparate1";
            this.MenuSeparate1.Size = new System.Drawing.Size(184, 6);
            // 
            // MenuReload
            // 
            this.MenuReload.Enabled = false;
            this.MenuReload.Name = "MenuReload";
            this.MenuReload.Size = new System.Drawing.Size(187, 22);
            this.MenuReload.Text = "마지막 저장 상태로";
            this.MenuReload.Click += new System.EventHandler(this.MenuReload_Click);
            // 
            // MenuSeparate3
            // 
            this.MenuSeparate3.Name = "MenuSeparate3";
            this.MenuSeparate3.Size = new System.Drawing.Size(184, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(187, 22);
            this.MenuExit.Text = "종료";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // FileTree
            // 
            this.FileTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FileTree.Location = new System.Drawing.Point(0, 24);
            this.FileTree.Name = "FileTree";
            this.FileTree.PathSeparator = "/";
            this.FileTree.Size = new System.Drawing.Size(404, 310);
            this.FileTree.TabIndex = 1;
            this.FileTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FileTree_NodeMouseClick);
            this.FileTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.FileTree_NodeMouseDoubleClick);
            // 
            // MenuAdd
            // 
            this.MenuAdd.Enabled = false;
            this.MenuAdd.Name = "MenuAdd";
            this.MenuAdd.Size = new System.Drawing.Size(187, 22);
            this.MenuAdd.Text = "추가...";
            this.MenuAdd.Click += new System.EventHandler(this.MenuAdd_Click);
            // 
            // MenuSeparate0
            // 
            this.MenuSeparate0.Name = "MenuSeparate0";
            this.MenuSeparate0.Size = new System.Drawing.Size(184, 6);
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
        private System.Windows.Forms.ToolStripMenuItem MenuReload;
        private System.Windows.Forms.ToolStripSeparator MenuSeparate3;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
        private System.Windows.Forms.TreeView FileTree;
        private System.Windows.Forms.ToolStripMenuItem MenuSaveAs;
        private System.Windows.Forms.ToolStripSeparator MenuSeparate0;
        private System.Windows.Forms.ToolStripMenuItem MenuAdd;
    }
}