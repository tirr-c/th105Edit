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
                AddNode(i.EntryPath);
            }
            FileTree.EndUpdate();
        }
        private void AddNode(string[] Path)
        {
            int index;
            TreeNodeCollection root = FileTree.Nodes;
            TreeNodeCollection working = root;
            for (index = 0; index < Path.Length; index++)
            {
                string i = Path[index];
                if (!working.ContainsKey(i)) break;
                working = working[i].Nodes;
            }
            for (; index < Path.Length; index++)
            {
                working.Add(new TreeNode(Path[index]) { Name = Path[index] });
                working = working[Path[index]].Nodes;
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
    }
}
