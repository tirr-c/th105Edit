using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace cvn_helper
{
    public partial class frmMain : Form
    {
        cvnBase m_workingfile;
        string m_workingpath;

        public frmMain()
        {
            m_workingfile = null;
            m_workingpath = string.Empty;
            InitializeComponent();
        }

        private void MenuOpen_Click(object sender, EventArgs e)
        {
            dlgOpen.Reset();
            dlgOpen.Filter = "지원되는 모든 파일(*.cv0, *.cv1, *.cv2, *.cv3)|*.cv0;*.cv1;*.cv2;*.cv3|" +
                "암호화된 평문 텍스트(*.cv0)|*.cv0|암호화된 CVS(*.cv1)|*.cv1|그래픽(*.cv2)|*.cv2|사운드(*.cv3)|*.cv3";
            dlgOpen.FilterIndex = 0;
            dlgOpen.FileOk += new CancelEventHandler(dlgOpenCVN_FileOk);
            dlgOpen.ShowDialog();
        }

        private void dlgOpenCVN_FileOk(object sender, CancelEventArgs e)
        {
            m_workingpath = dlgOpen.FileName;
            m_workingfile = cvn.Open(m_workingpath);
            if (m_workingfile.Type == cvnType.Text || m_workingfile.Type == cvnType.CSV)
            {
                MenuEncodings.Visible = true;
                MenuEncodings.Enabled = true;
                (m_workingfile as cv0).StringEncoding = Encoding.GetEncoding(949);
                MenuKorean.Checked = true;
            }
            else
            {
                MenuEncodings.Visible = false;
                MenuEncodings.Enabled = false;
            }
            MenuSave.Enabled = true;
            MenuSaveAs.Enabled = true;
            MenuExtract.Enabled = true;

            RefreshView();
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
                    break;
            }
        }

        private void ChangeEnabled(cvnType type)
        {
            Control[] controls = new Control[] { cv0Data, cv1List, cv2Image };
            int pass_index = 3;
            switch (type)
            {
                case cvnType.Text:
                    pass_index = 0;
                    break;
                case cvnType.CSV:
                    pass_index = 1;
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
            MenuKorean.Checked = !(sender as ToolStripMenuItem).Checked;
        }

        private void MenuKorean_CheckedChanged(object sender, EventArgs e)
        {
            MenuJapanese.Checked = !(sender as ToolStripMenuItem).Checked;
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
        }

        private void MenuSave_Click(object sender, EventArgs e)
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
            m_workingfile.SaveToFile(m_workingpath);
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void MenuSaveAs_Click(object sender, EventArgs e)
        {
            dlgSave.Reset();
            switch (m_workingfile.Type)
            {
                case cvnType.Text:
                    dlgSave.Filter = "cv0(*.cv0)|*.cv0";
                    break;
                case cvnType.CSV:
                    dlgSave.Filter = "cv1(*.cv1)|*.cv1";
                    break;
                case cvnType.Graphic:
                    dlgSave.Filter = "cv2(*.cv2)|*.cv2";
                    break;
            }
            dlgSave.Filter += "|모든 파일(*.*)|*.*";
            dlgSave.FilterIndex = 0;
            dlgSave.OverwritePrompt = true;
            if (dlgSave.ShowDialog() == DialogResult.OK)
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
                m_workingpath = (sender as SaveFileDialog).FileName;
                m_workingfile.SaveToFile(m_workingpath);
            }
        }
    }
}
