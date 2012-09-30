using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace th105Edit
{
    public partial class frmArchiveManager : Form
    {
        HinanawiTenshi m_workingfile;
        string m_workingpath;
        frmSaveProgress m_save_progress;

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
            m_save_progress = new frmSaveProgress();

            ContextMenuItems[0].Click += new EventHandler(ContextMenu_EditClick);
            ContextMenuItems[1].Click += new EventHandler(ContextMenu_ExtractClick);
            ContextMenuItems[2].Click += new EventHandler(ContextMenu_ReplaceClick);
        }

        private delegate void OpenFileCallback(string Path);
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
                StartOpenSave();
                Thread th = new Thread(new ParameterizedThreadStart(OpenWork));
                th.Start(ofd.FileName);
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
            ContextMenu m = new ContextMenu(myContextMenu);
            m.Show(sender as Control, e.Location);
        }

        void ContextMenu_ExtractClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            TenshiEntry entry = ((sender as MenuItem).Tag as TreeNode).Tag as TenshiEntry;
            string entry_str = entry.Entry;
            string ext = entry_str.Substring(entry_str.IndexOf('.') + 1);
            sfd.Filter = ext + "(*." + ext + ")|*." + ext + "|모든 파일(*.*)|*.*";
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] buf = new byte[entry.Length];
                entry.Read(buf, 0, (int)entry.Length);
                File.WriteAllBytes(sfd.FileName, buf);
            }
        }

        void ContextMenu_ReplaceClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            TenshiEntry entry = ((sender as MenuItem).Tag as TreeNode).Tag as TenshiEntry;
            string entry_str = entry.Entry;
            string ext = entry_str.Substring(entry_str.IndexOf('.')+1);
            ofd.Filter = ext + "(*." + ext + ")|*." + ext;
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

        private void MenuSave_Click(object sender, EventArgs e)
        {
            StartOpenSave();
            Thread th = new Thread(new ParameterizedThreadStart(SaveWork));
            string temp_name = Path.GetTempFileName();
            th.Start(temp_name);
            m_save_progress.ShowDialog();
            th.Abort();
            if (MenuOpen.Enabled == false)
            {
                FinalizeOpenSave();
                File.Delete(temp_name);
            }
            else
            {
                File.Delete(m_workingpath);
                File.Move(temp_name, m_workingpath);
            }
        }

        private void MenuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "동방비상천 데이터 파일(*.dat)|*.dat|모든 파일(*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.OverwritePrompt = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StartOpenSave();
                Thread th = new Thread(new ParameterizedThreadStart(SaveWork));
                th.Start(sfd.FileName);
                m_save_progress.ShowDialog();
                th.Abort();
            }
        }

        private void OpenWork(object Path)
        {
            m_workingfile.Open(Path as string);
            Invoke(new OpenFileCallback(OpenFile), Path);
            Invoke(new OpenSaveCallback(FinalizeOpenSave));
        }
        private void SaveWork(object Path)
        {
            try
            {
                m_workingfile.Save(Path as string, new HinanawiTenshi.SaveFileCallback(m_save_progress.UpdateProgress));
            }
            catch
            {
            }
            finally
            {
                Invoke(new OpenSaveCallback(FinalizeOpenSave));
            }
        }

        private delegate void OpenSaveCallback();
        private void StartOpenSave()
        {
            MenuOpen.Enabled = false;
            MenuSave.Enabled = false;
            MenuSaveAs.Enabled = false;
            MenuReload.Enabled = false;
            FileTree.Enabled = false;
        }
        private void FinalizeOpenSave()
        {
            MenuOpen.Enabled = true;
            MenuSave.Enabled = true;
            MenuSaveAs.Enabled = true;
            MenuReload.Enabled = true;
            FileTree.Enabled = true;
            m_save_progress.Close();
        }

        private void MenuReload_Click(object sender, EventArgs e)
        {
            StartOpenSave();
            Thread th = new Thread(new ParameterizedThreadStart(OpenWork));
            th.Start(m_workingpath);
        }
    }
}
