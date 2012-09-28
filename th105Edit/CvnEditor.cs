using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace cvn_helper
{
    public partial class frmCvnEditor : Form
    {
        private TenshiEntry m_entry, m_palette;
        private cvnBase m_workingfile;
        private string m_workingpath;
        private bool m_discard_changes;
        private bool m_is_from_stream;
        private bool m_changed;
        private bool m_failed;

        private static Encoding EUCKR = Encoding.GetEncoding(949);
        private static Encoding ShiftJIS = Encoding.GetEncoding("Shift-JIS");

        public bool Discard
        {
            get { return m_discard_changes; }
        }
        public bool Changed
        {
            get { return m_changed; }
        }
        public cvnBase Data
        {
            get { return m_workingfile; }
        }
        public TenshiEntry Entry
        {
            get { return m_entry; }
        }
        public bool Failed
        {
            get { return m_failed; }
        }

        public frmCvnEditor()
        {
            m_entry = m_palette = null;
            m_workingfile = null;
            m_workingpath = string.Empty;
            m_discard_changes = true;
            m_is_from_stream = false;
            m_changed = false;
            m_failed = false;
            InitializeComponent();
        }
        public frmCvnEditor(TenshiEntry BaseFile, TenshiEntry Palette = null)
            : this()
        {
            m_entry = BaseFile;
            m_palette = Palette;
            m_workingpath = m_entry.Entry;
            try
            {
                m_workingfile = cvn.Open(BaseFile, BaseFile.Type, Palette);
            }
            catch (FormatException)
            {
                m_failed = true;
                return;
            }
            if (m_workingfile == null)
            {
                m_failed = true;
                return;
            }
            m_is_from_stream = true;
            RefreshView();
            m_changed = false;
        }

        private void RefreshView()
        {
            ChangeEnabled(m_workingfile.Type);
            switch (m_workingfile.Type)
            {
                case cvnType.Text:
                    cv0Data.Text = m_workingfile.Data as string;
                    break;
                case cvnType.CSV:
                    cv1UpdateList();
                    break;
                case cvnType.Graphic:
                    cv2Image.Image = m_workingfile.Data as Bitmap;
                    int w = Width - cv2Image.Width + cv2Image.Image.Width;
                    int h = Height - cv2Image.Height + cv2Image.Image.Height;
                    Width = w;
                    Height = h;
                    break;
            }
        }

        private void ChangeEnabled(cvnType type)
        {
            MenuExtract.Enabled = true;
            Control[] controls = new Control[] { cv0Data, cv1List, cv2Image };
            int pass_index = 3;
            switch (type)
            {
                case cvnType.Text:
                    pass_index = 0;
                    MenuEncodings.Visible = MenuEncodings.Enabled = MenuChangeEncoding.Enabled = true;
                    break;
                case cvnType.CSV:
                    pass_index = 1;
                    MenuEncodings.Visible = MenuEncodings.Enabled = MenuChangeEncoding.Enabled = true;
                    break;
                case cvnType.Graphic:
                    pass_index = 2;
                    break;
            }
            for (int i = 0; i < controls.Length; i++)
            {
                if (pass_index == i) break;
                controls[i].Visible = false;
                controls[i].Enabled = false;
                controls[i].Dock = DockStyle.None;
            }
            if (pass_index >= controls.Length) return;
            controls[pass_index].Visible = true;
            controls[pass_index].Enabled = true;
            controls[pass_index].Dock = DockStyle.Fill;
        }
        
        private void MenuJapanese_CheckedChanged(object sender, EventArgs e)
        {
            bool val = (sender as ToolStripMenuItem).Checked;
            MenuKorean.Checked = !val;
            if (val)
            {
                (m_workingfile as cv0).StringEncoding = ShiftJIS;
                RefreshView();
            }
            m_changed = true;
        }

        private void MenuKorean_CheckedChanged(object sender, EventArgs e)
        {
            bool val = (sender as ToolStripMenuItem).Checked;
            MenuJapanese.Checked = !val;
            if (val)
            {
                (m_workingfile as cv0).StringEncoding = EUCKR;
                RefreshView();
            }
            m_changed = true;
        }

        private void cv1List_DoubleClick(object sender, EventArgs e)
        {
            cv1RecordEditor editor =
                new cv1RecordEditor((m_workingfile.Data as cv1DataCollection)[(sender as ListView).SelectedIndices[0]]);
            editor.ShowDialog();
            cv1UpdateList();
        }

        private void cv1UpdateList()
        {
            cv1DataCollection coll = m_workingfile.Data as cv1DataCollection;
            int len = coll[0].Fields.Length;
            cv1List.Items.Clear();
            cv1List.Columns.Clear();
            for (int i = 0; i < len; i++)
                cv1List.Columns.Add((i + 1) + "번 필드");
            foreach (cv1DataLine i in coll)
                cv1List.Items.Add(new ListViewItem(i.Fields));
            m_changed = true;
        }

        private void Save()
        {
            switch (m_workingfile.Type)
            {
                case cvnType.Text:
                    m_workingfile.SetData(cv0Data.Text);
                    break;
                case cvnType.CSV:
                case cvnType.Graphic:
                    break;
            }
            if (!m_is_from_stream) m_workingfile.SaveToFile(m_workingpath);
        }
        private void Save(Encoding StringEncoding)
        {
            switch (m_workingfile.Type)
            {
                case cvnType.Text:
                    (m_workingfile as cv0).SetData(cv0Data.Text, StringEncoding);
                    break;
                case cvnType.CSV:
                    (m_workingfile as cv1).ConvertEncoding(StringEncoding);
                    break;
                case cvnType.Graphic:
                    return;
            }
            if (!m_is_from_stream) m_workingfile.SaveToFile(m_workingpath);
            RefreshView();
            m_changed = true;
        }
        
        private void MenuExit_Click(object sender, EventArgs e)
        {
            m_discard_changes = false;
            Close();
        }

        private void MenuExtract_Click(object sender, EventArgs e)
        {
            dlgSave.Reset();
            switch (m_workingfile.Type)
            {
                case cvnType.Text:
                    dlgSave.Filter = "텍스트 파일(*.txt)|*.txt";
                    break;
                case cvnType.CSV:
                    dlgSave.Filter = "CSV 시트(*.csv)|*.csv";
                    break;
                case cvnType.Graphic:
                    dlgSave.Filter = "PNG(*.png)|*.png";
                    break;
            }
            dlgSave.Filter += "|모든 파일(*.*)|*.*";
            dlgSave.FilterIndex = 1;
            dlgSave.OverwritePrompt = true;
            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                m_workingfile.Extract(dlgSave.FileName);
            }
        }

        private void frmCvnEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_changed && m_discard_changes)
            {
                switch(MessageBox.Show(m_workingpath + "을(를) 저장하시겠습니까?", "수정 마치기", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case DialogResult.Yes:
                        m_discard_changes = false;
                        break;
                }
            }
            if (!m_discard_changes)
                Save();
        }

        private void MenuSaveAsKorean_Click(object sender, EventArgs e)
        {
            Save(EUCKR);
        }

        private void MenuSaveAsJapanese_Click(object sender, EventArgs e)
        {
            Save(ShiftJIS);
        }

        private void cv0Data_TextChanged(object sender, EventArgs e)
        {
            m_changed = true;
        }
    }
}
