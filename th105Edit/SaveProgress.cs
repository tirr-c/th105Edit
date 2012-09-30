using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace th105Edit
{
    public partial class frmSaveProgress : Form
    {
        ushort m_count, m_total;

        public frmSaveProgress()
        {
            InitializeComponent();
        }

        public void UpdateProgress(ushort count, ushort total)
        {
            m_count = count;
            m_total = total;
            this.Invoke(new UpdateCallback(UpdateProgress));
        }

        private delegate void UpdateCallback();
        private void UpdateProgress()
        {
            Progress.Maximum = (int)m_total;
            Progress.Value = (int)m_count;
            lblProgress.Text = m_count + " / " + m_total;
        }

        private void frmSaveProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
