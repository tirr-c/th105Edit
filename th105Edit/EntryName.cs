using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace th105Edit
{
    public partial class frmEntryName : Form
    {
        private string m_entry;
        private bool m_decided;
        public string Entry
        {
            get { return m_entry; }
            set
            {
                lblEntry.Text = Path.GetFileName(value);
                txtEntry.Text = value.Replace('\\', '/').Substring((value.IndexOf("data") == -1) ? 0 : value.IndexOf("data"));
            }
        }

        public frmEntryName()
        {
            m_entry = string.Empty;
            m_decided = false;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_entry = txtEntry.Text;
            m_decided = true;
            Close();
        }

        private void frmEntryName_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_decided) m_entry = string.Empty;
        }
    }
}
