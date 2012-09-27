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
            frmCvnEditor editor = new frmCvnEditor(e.Node.Tag as TenshiEntry);
            editor.FormClosed += new FormClosedEventHandler(EditFinished);
            editor.Show();
        }

        void EditFinished(object sender, FormClosedEventArgs e)
        {
            frmCvnEditor editor = sender as frmCvnEditor;
            if (!editor.Changed || editor.Discard) return;
            editor.Entry.ChangedStream = editor.Data.ToStream();
        }

        private void FileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            ContextMenu m = new ContextMenu(ContextMenuItems);
            m.Tag = e.Node;
            m.Show(sender as Control, e.Location);
        }
    }
}
