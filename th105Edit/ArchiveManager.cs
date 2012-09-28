using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cvn_helper
{
    public partial class frmArchiveManager : Form
    {
        HinanawiTenshi m_workingfile;
        string m_workingpath;

        private static MenuItem[] ContextMenuItems = new MenuItem[]
        {
            new MenuItem("수정..."),
            new MenuItem("추출..."),
            new MenuItem("교체..."),
        };

        public frmArchiveManager()
        {
            InitializeComponent();
            m_workingfile = new HinanawiTenshi();
        }

        private void OpenFile(string Path)
        {
            m_workingpath = Path;
            m_workingfile.Open(m_workingpath);
            FileTree.BeginUpdate();
            FileTree.Nodes.Clear();
            foreach (TenshiEntry i in m_workingfile.Entries)
            {
                AddNode(i);
            }
            FileTree.EndUpdate();
        }
        private void AddNode(TenshiEntry i)
        {
            string[] Path = i.EntryPath;
            int index;
            TreeNodeCollection root = FileTree.Nodes;
            TreeNodeCollection working = root;
            for (index = 0; index < Path.Length; index++)
            {
                string node = Path[index];
                if (!working.ContainsKey(node)) break;
                working = working[node].Nodes;
            }
            TreeNode last = null;
            for (; index < Path.Length; index++)
            {
                working.Add(new TreeNode(Path[index]) { Name = Path[index], Tag = null });
                last = working[Path[index]];
                working = working[Path[index]].Nodes;
            }
            if (last != null)
            {
                last.Tag = i;
            }
        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "동방비상천 데이터 파일(*.dat)|*.dat|모든 파일(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = ofd.CheckPathExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                OpenFile(ofd.FileName);
            }
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FileTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (e.Node.Tag == null) return;
            EditStart(e.Node);
        }

        private void EditStart(TreeNode entry)
        {
            TenshiEntry palette = null;
            TreeNode parent = entry.Parent;
            if (parent != null)
            {
                TreeNodeCollection same_dir = parent.Nodes;
                foreach (TreeNode i in same_dir)
                {
                    if (i.Tag == null) continue;
                    string[] entry_path = (i.Tag as TenshiEntry).EntryPath;
                    if (entry_path[entry_path.Length - 1] == "palette000.pal")
                    {
                        palette = i.Tag as TenshiEntry;
                        break;
                    }
                }
            }
            frmCvnEditor editor = new frmCvnEditor(entry.Tag as TenshiEntry, palette);
            if (editor.Failed) return;
            editor.FormClosed += new FormClosedEventHandler(EditFinished);
            editor.Show();
        }

        private void EditFinished(object sender, FormClosedEventArgs e)
        {
            frmCvnEditor editor = sender as frmCvnEditor;
            if (!editor.Changed || editor.Discard) return;
            editor.Entry.ChangedStream = editor.Data.ToStream();
        }

        private void FileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.Node.Tag == null) return;
            MenuItem[] myContextMenu = new MenuItem[ContextMenuItems.Length];
            ContextMenuItems.CopyTo(myContextMenu, 0);
            foreach (MenuItem i in myContextMenu) i.Tag = e.Node;
            myContextMenu[0].Click += new EventHandler(ContextMenu_EditClick);
            myContextMenu[2].Click += new EventHandler(ContextMenu_ReplaceClick);
            ContextMenu m = new ContextMenu(myContextMenu);
            m.Show(sender as Control, e.Location);
        }

        void ContextMenu_ReplaceClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            TenshiEntry entry = ((sender as MenuItem).Tag as TreeNode).Tag as TenshiEntry;
            string entry_str = entry.Entry;
            string ext = entry_str.Substring(entry_str.IndexOf('.')+1);
            ofd.Filter = ext + " (*." + ext + "|*." + ext;
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                entry.ChangedStream = ofd.OpenFile();
            }
        }

        void ContextMenu_EditClick(object sender, EventArgs e)
        {
            EditStart((sender as MenuItem).Tag as TreeNode);
        }
    }
}
