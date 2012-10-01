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

using System;
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
            new MenuItem("삭제"),
        };

        public frmArchiveManager()
        {
            InitializeComponent();
            m_workingfile = new HinanawiTenshi();

            ContextMenuItems[0].Click += new EventHandler(ContextMenu_EditClick);
            ContextMenuItems[1].Click += new EventHandler(ContextMenu_ExtractClick);
            ContextMenuItems[2].Click += new EventHandler(ContextMenu_ReplaceClick);
            ContextMenuItems[3].Click += new EventHandler(ContextMenu_DeleteClick);
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

        void ContextMenu_DeleteClick(object sender, EventArgs e)
        {
            TenshiEntry entry = ((sender as MenuItem).Tag as TreeNode).Tag as TenshiEntry;
            m_workingfile.Entries.Remove(entry);
            ((sender as MenuItem).Tag as TreeNode).Remove();
        }

        void ContextMenu_ExtractClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            TenshiEntry entry = ((sender as MenuItem).Tag as TreeNode).Tag as TenshiEntry;
            string entry_str = entry.Entry;
            string ext = Path.GetExtension(entry_str);
            sfd.Filter = ext + "(*" + ext + ")|*" + ext + "|모든 파일(*.*)|*.*";
            sfd.FileName = Path.GetFileName(entry_str);
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
            m_save_progress = new frmSaveProgress();
            Thread th = new Thread(new ParameterizedThreadStart(SaveWork));
            string temp_name = Path.GetTempFileName();
            th.Start(temp_name);
            m_save_progress.ShowDialog();
            th.Abort();
            if (MenuOpen.Enabled == false)
            {
                FinalizeOpenSave(true);
                File.Delete(temp_name);
            }
            else
            {
                File.Delete(m_workingpath);
                File.Move(temp_name, m_workingpath);
                Reload();
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
                m_save_progress = new frmSaveProgress();
                Thread th = new Thread(new ParameterizedThreadStart(SaveWork));
                th.Start(sfd.FileName);
                m_save_progress.ShowDialog();
                th.Abort();
                if (MenuOpen.Enabled == false)
                {
                    FinalizeOpenSave(false);
                    File.Delete(sfd.FileName);
                }
                else
                {
                    m_workingpath = sfd.FileName;
                    Reload();
                }
            }
        }

        private void OpenWork(object Path)
        {
            m_workingfile.Open(Path as string);
            Invoke(new OpenFileCallback(OpenFile), Path);
            Invoke(new OpenSaveCallback(FinalizeOpenSave), false);
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
                Invoke(new OpenSaveCallback(FinalizeOpenSave), true);
            }
        }

        private delegate void OpenSaveCallback(bool isSave);
        private void StartOpenSave()
        {
            MenuOpen.Enabled = false;
            MenuSave.Enabled = false;
            MenuSaveAs.Enabled = false;
            MenuAdd.Enabled = false;
            MenuReload.Enabled = false;
            FileTree.Enabled = false;
        }
        private void FinalizeOpenSave(bool isSave)
        {
            MenuOpen.Enabled = true;
            MenuSave.Enabled = true;
            MenuSaveAs.Enabled = true;
            MenuAdd.Enabled = true;
            MenuReload.Enabled = true;
            FileTree.Enabled = true;
            if (isSave) m_save_progress.Close();
        }

        private void MenuReload_Click(object sender, EventArgs e)
        {
            Reload();
        }
        private void Reload()
        {
            StartOpenSave();
            Thread th = new Thread(new ParameterizedThreadStart(OpenWork));
            th.Start(m_workingpath);
        }

        private void MenuAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "cvn 파일(*.cv*)|*.cv*|모든 파일(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = ofd.CheckPathExists = true;
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file_name in ofd.FileNames)
                {
                    frmEntryName dlgName = new frmEntryName();
                    dlgName.Top = this.Top + 50;
                    dlgName.Left = this.Left + 50;
                    dlgName.Entry = file_name;
                    dlgName.ShowDialog();
                    if (dlgName.Entry == string.Empty) return;
                    TenshiEntry i = new TenshiEntry(null, dlgName.Entry, 0, 0);
                    i.ChangedStream = File.OpenRead(file_name);
                    m_workingfile.Entries.Add(i);
                    AddNode(i);
                }
            }
        }
    }
}
